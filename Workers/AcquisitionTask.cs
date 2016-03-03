using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace YouTubeDownloader.Workers {
  
  /// <summary>
  ///   Worker task to fetch video information from the YouTube API.
  /// </summary>
  internal class AcquisitionTask : WorkerTask {


    /// <summary>
    ///   Video meta data container.
    /// </summary>
    internal class VideoData {
      public Uri VideoUrl;   // Specific video URL (MP4, 480p).
      public string Title;   // Video title.
      public string Length;  // Video length as formatted string.
      public string Size;    // Size of MP4 file (for download progress).
    }

    private readonly string _videoAddress; // The video to query.


    /// <summary>
    ///   Create a new video meta data acquisition task.
    /// </summary>
    /// <param name="videoAddress">The video to query information for.</param>
    /// <param name="handleResult">Result processing function.</param>
    public AcquisitionTask(string videoAddress, HandleResult handleResult)
      : base(handleResult, null) {
      _videoAddress = videoAddress;
    }
    
    
    /// <summary>
    ///   Worker main function - fetch video meta information.
    /// </summary>
    /// <param name="worker">Worker reference for cancellation and reporting.</param>
    /// <param name="e">Event argument, used for result returning.</param>
    protected override void DoWork(BackgroundWorker worker, DoWorkEventArgs e) {
      var data = FetchVideoInformation(_videoAddress);
      e.Result = data;
    }


    /// <summary>
    ///   Get the video meta data from the YouTube API.
    /// </summary>
    /// <param name="url">The youtube video address.</param>
    /// <returns>Structure with video data information or null, if an error occured.</returns>
    private static VideoData FetchVideoInformation(string url) {
      var videodata = new VideoData();
 
      var videoUri = new Uri(url);
      var videoId = HttpUtility.ParseQueryString(videoUri.Query).Get("v");
      var videoInfoUrl = "http://www.youtube.com/get_video_info?video_id=" + videoId;

      var request = (HttpWebRequest) WebRequest.Create(videoInfoUrl);
      var response = (HttpWebResponse) request.GetResponse();
      var responseStream = response.GetResponseStream();
      if (responseStream != null) {
        var reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));

        var videoInfo = HttpUtility.HtmlDecode(reader.ReadToEnd());
        var videoParams = HttpUtility.ParseQueryString(videoInfo);

        var lSec = int.Parse(videoParams["length_seconds"]);      

        videodata.Title = videoParams["title"];
        videodata.Length = lSec/60 + ":"+ lSec%60;

        var streams = new Dictionary<string, string>();

        var videoUrls = videoParams["url_encoded_fmt_stream_map"].Split(',');  
        foreach (var vUrl in videoUrls) {
          var sUrl = HttpUtility.HtmlDecode(vUrl);       
          var urlParams = HttpUtility.ParseQueryString(sUrl);
          var videoFormat = HttpUtility.HtmlDecode(urlParams["type"]);
          videoFormat = urlParams["quality"] + " - " + videoFormat.Split(';')[0].Split('/')[1];

          sUrl = HttpUtility.HtmlDecode(urlParams["url"]);
          sUrl += "&signature=" + HttpUtility.HtmlDecode(urlParams["sig"]);
          sUrl += "&type=" + videoFormat;
          sUrl += "&title=" + HttpUtility.HtmlDecode(videoParams["title"]);

          if (videoFormat.Contains("mp4")) streams.Add(videoFormat,sUrl);
        }


        if (streams.Count > 0) {
          var addr = "";
          foreach(var entry in streams) {
            if (entry.Key.Contains("medium")) {
              addr = entry.Value;
              break;
            }
          }
          if (addr == "") addr = streams.Values.ElementAt(streams.Count - 1);
          var address = new Uri(addr);

          var wc = new WebClient();
          wc.OpenRead(address);
          var bytesTotal= Convert.ToInt64( wc.ResponseHeaders["Content-Length"]);
          videodata.Size = Math.Round((float)bytesTotal / (1024*1024), 2)+"";
          videodata.VideoUrl = address;

          // Success: Return data structure.
          return videodata;
        }
      }
      return null;
    }
  }
}