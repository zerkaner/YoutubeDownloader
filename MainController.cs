using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;

namespace YouTubeDownloader {

  internal class MainController {




    
    public MainController() {
    }
    


    public static void Test() {

      const string addr = "https://www.youtube.com/watch?v=Dihka60wAmU";

      var result = GetVideoMetadata(addr);
      switch (result.Success) {
        case QueryStatus.Found:
          Console.WriteLine("Title: "+result.Title);
          Console.WriteLine("Uploader: "+result.Uploader);
          Console.WriteLine("Length (s): "+result.LengthSec);
          Console.WriteLine("Views: "+result.ViewCount);
          Console.WriteLine("Thumbnail: "+result.Thumbnail);

          Console.WriteLine("Streams:");
          foreach (var stream in result.Streams) {
            Console.WriteLine(" - "+stream.Resolution+"px : "+stream.Address+" : "+stream.ByteSize+" bytes");
          }

          DownloadVideo(result.Streams[1]);

/*
          var path = GetSavePath(result.Title, true);
          Console.WriteLine(path);
          File.WriteAllText(path, "test");
*/
          break;
        case QueryStatus.NetworkError:
          Console.WriteLine("nw error");
          break;
        case QueryStatus.LinkInvalid:
          Console.WriteLine("link inv");
          break;
        case QueryStatus.NotFound:
          Console.WriteLine("not found");
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }



    /// <summary>
    ///   Create a valid filename for a video or audio file.
    /// </summary>
    /// <param name="videoName">Name of the YouTube video.</param>
    /// <param name="isVideo">Is this a video ('true') or just an audio file ('false')?</param>
    /// <returns>The complete file path, including folder and extension.</returns>
    private static string GetSavePath(string videoName, bool isVideo) {
      var fileName = videoName;
      foreach (var ch in Path.GetInvalidFileNameChars()) {
        fileName = fileName.Replace(ch, '_');
      }
      var path = isVideo ? "videos" : "music";
      var ext = isVideo ? ".mp4" : ".mp3";
      if (!Directory.Exists(path)) Directory.CreateDirectory(path);
      path += Path.DirectorySeparatorChar + fileName;
      var i = 0;
      var fullpath = path + ext;
      while (File.Exists(fullpath)) fullpath = path + " (" + (++i) + ")" + ext;
      return fullpath;
    }



    /// <summary>
    ///   Get the meta information for a YouTube video.
    /// </summary>
    /// <param name="videolink">The YouTube video link.</param>
    /// <returns>Meta data query response.</returns>
    private static MetadataResponse GetVideoMetadata(string videolink) {
      var dict = new Dictionary<string, string>();
      try {
        var id = HttpUtility.ParseQueryString(new Uri(videolink).Query).Get("v");
        var query = "http://youtube.com/get_video_info?video_id=" + id;
        var apiResult = new WebClient().DownloadString(query).Split('&');
        foreach (var entry in apiResult) {
          var kvArr = entry.Split('=');
          dict.Add(kvArr[0], kvArr[1]);
        }
      }

      // Input parameters could not be parsed. Probably the video link is malformed.
      catch (UriFormatException) {
        return new MetadataResponse {Success = QueryStatus.LinkInvalid};
      }

      // The query failed. Either the internet connection is down or the API has changed.
      catch (WebException) {
        return new MetadataResponse {Success = QueryStatus.NetworkError};
      }

      // The server responded, but returned a negative result.
      if (!dict.ContainsKey("status") || dict["status"].Equals("fail")) {
        return new MetadataResponse {Success = QueryStatus.NotFound};
      }


      // The query succeeded. Now comes the hard part ...
      var result = new MetadataResponse {
        Success = QueryStatus.Found,
        Title = HttpUtility.UrlDecode(dict["title"]),
        Uploader = HttpUtility.UrlDecode(dict["author"]),
        LengthSec = int.Parse(dict["length_seconds"]),
        ViewCount = int.Parse(dict["view_count"]),
        Thumbnail = HttpUtility.UrlDecode(dict["thumbnail_url"]),
        Streams = new List<Mp4VideoStream>()
      };

      var streamMap = dict["url_encoded_fmt_stream_map"];
      var decoded = HttpUtility.UrlDecode(streamMap);
      if (decoded == null) return new MetadataResponse {Success = QueryStatus.ParserError};
      var streams = decoded.Split(',');

      // Loop over all streams and get the MP4 ones.
      foreach (var stream in streams) {
        var streamProps = new Dictionary<string, string>();
        foreach (var streamProp in stream.Split('&')) {
          var kvProp = streamProp.Split('=');
          streamProps.Add(kvProp[0], HttpUtility.UrlDecode(kvProp[1]));
        }
        int res;
        switch (int.Parse(streamProps["itag"])) {
          case 18: res =  360; break;
          case 22: res =  720; break;
          case 37: res = 1080; break;
          default: res =   -1; break;
        }
        if (res != -1) {
          result.Streams.Add(new Mp4VideoStream {
            Resolution = res,
            Address = streamProps["url"]
          });
        }
      }

      // Probe the video byte sizes.
      foreach (var stream in result.Streams) {
        var wc = new WebClient();
        using (wc.OpenRead(stream.Address)) {
          stream.ByteSize = Convert.ToInt64(wc.ResponseHeaders["Content-Length"]);
        }
      }
      return result;
    }



    private static void DownloadVideo(Mp4VideoStream stream) {

      var wc = new WebClient();

      wc.DownloadProgressChanged += (sender, args) => {
        Console.WriteLine(args.BytesReceived+" / "+args.TotalBytesToReceive+" : "+args.ProgressPercentage);
      };
      wc.DownloadDataCompleted += (sender, e) => {
        Console.WriteLine("Finished: "+!e.Cancelled+" "+e.Result.Length);
      };

      wc.DownloadDataAsync(new Uri(stream.Address));


      Console.ReadLine();
      Console.WriteLine("fertig gewartet!");

    }

  }
}
