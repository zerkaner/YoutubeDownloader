using System.Collections.Generic;

namespace YouTubeDownloader {


    /// <summary>
    ///   Structure for a video's meta information.
    /// </summary>
    internal class MetadataResponse {
      public QueryStatus Success;
      public string Title;
      public string Uploader;
      public int LengthSec;
      public int ViewCount;
      public string Thumbnail;
      public List<Mp4VideoStream> Streams;
    }

    internal enum QueryStatus {NetworkError, LinkInvalid, Found, NotFound, ParserError}

    internal class Mp4VideoStream {
      public int Resolution; // 360p-normal (18), 720p-high (22), 1080p-HD (37).
      public string Address; // Download URL.
      public long ByteSize;  // Size of the video in bytes.
    }
}
