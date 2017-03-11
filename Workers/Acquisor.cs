using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Web;

namespace YouTubeDownloader.Workers {

  /// <summary>
  ///   The acquisor fetches the meta information for a YouTube video.
  ///   It uses to the public query endpoint 'get_video_info'.
  /// </summary>
  internal class Acquisor {


    /// <summary>
    ///   Get the meta information for a YouTube video.
    /// </summary>
    /// <param name="videolink">The YouTube video link.</param>
    /// <param name="videoInfo">Video description.</param>
    /// <returns>Query success indicator.</returns>
    public static QueryStatus GetVideoMetadata(string videolink, out VideoInfo videoInfo) {
      var dict = new Dictionary<string, string>();
      videoInfo = new VideoInfo();
      try {
        var id = HttpUtility.ParseQueryString(new Uri(videolink).Query).Get("v");
        var query = "http://youtube.com/get_video_info?video_id=" + id + "&el=vevo&el=embedded";
        var apiResult = new WebClient().DownloadString(query).Split('&');
        foreach (var entry in apiResult) {
          var kvArr = entry.Split('=');
          dict.Add(kvArr[0], kvArr[1]);
        }
      }
      catch (UriFormatException) {       //| Input parameters could not be parsed.
        return QueryStatus.LinkInvalid;  //| Probably the video link is malformed.
      }  
      catch (WebException) {             //| The query failed. Either the internet
        return QueryStatus.NetworkError; //| connection is down or the API has changed.
      }
      if (!dict.ContainsKey("status") || //| The server responded, but 
        dict["status"].Equals("fail")) { //| returned a negative result.
        return QueryStatus.NotFound;
      }


      // The query succeeded. Now comes the hard part ...
      videoInfo = new VideoInfo {
        Title = HttpUtility.UrlDecode(dict["title"]),
        Uploader = HttpUtility.UrlDecode(dict["author"]),
        LengthSec = int.Parse(dict["length_seconds"]),
        ViewCount = int.Parse(dict["view_count"]),
        Rating = float.Parse(dict["avg_rating"], CultureInfo.InvariantCulture),
        Streams = new List<Mp4VideoStream>()
      };
      var thumbnail = dict.ContainsKey("iurlmq")? dict["iurlmq"] : dict["thumbnail_url"];
      videoInfo.Thumbnail = HttpUtility.UrlDecode(thumbnail);
      var streamMap = dict["url_encoded_fmt_stream_map"];
      var decoded = HttpUtility.UrlDecode(streamMap);
      if (decoded == null) return QueryStatus.ParserError;

      // Loop over all streams and get the MP4 ones.
      var streams = decoded.Split(',');
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
          videoInfo.Streams.Add(new Mp4VideoStream {
            Resolution = res,
            Address = streamProps["url"]
          });
        }
      }

      // Probe the video byte sizes.
      foreach (var stream in videoInfo.Streams) {
        var wc = new WebClient();
        try {
          using (wc.OpenRead(stream.Address)) {
            stream.ByteSize = Convert.ToInt64(wc.ResponseHeaders["Content-Length"]);
          }
        }
        catch (WebException) {
          return QueryStatus.Copyrighted;
        }
      }
      return QueryStatus.Found;
    }
  }


  /// <summary>
  ///   Query status response.
  /// </summary>
  public enum QueryStatus {NetworkError, LinkInvalid, Found, NotFound, ParserError, Copyrighted}


  /// <summary>
  ///   Structure for a video's meta information.
  /// </summary>
  public class VideoInfo {
    public string Title;                 // Title of the video.
    public string Uploader;              // Video author name.
    public int LengthSec;                // Video length in seconds.
    public int ViewCount;                // Number of views.
    public float Rating;                 // Average rating (0-5 stars).
    public string Thumbnail;             // Video thumbnail URL.
    public List<Mp4VideoStream> Streams; // List of video streams.
  }


  /// <summary>
  ///   Description for a MP4 video stream.
  /// </summary>
  public class Mp4VideoStream {
    public int Resolution;  // 360p-normal (18), 720p-high (22), 1080p-HD (37).
    public string Address;  // Download URL.
    public long ByteSize;   // Size of the video in bytes.
  }
}
