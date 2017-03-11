using System.IO;

namespace YouTubeDownloader.Workers {

  /// <summary>
  ///   Main controller for the YouTube-Downloader user workflow.
  /// </summary>
  internal class ProgramController {


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
  }
}
