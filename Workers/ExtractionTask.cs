using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace YouTubeDownloader.Workers {

  /// <summary>
  ///   Extraction task to get the audio from a MP4 video.
  /// </summary>
  internal class ExtractionTask : WorkerTask {

    private readonly string _mp4File; // The MP4 video file.


    /// <summary>
    ///   Create a new extraction task.
    /// </summary>
    /// <param name="mp4File">The MP4 video.</param>    
    /// <param name="handleResult">Delegate to be called on process finish.</param>
    public ExtractionTask(string mp4File, HandleResult handleResult)
      : base(handleResult, null) {
      _mp4File = mp4File;
    }
    
    
    /// <summary>
    ///   Worker main function - extract an MP4's audio as WAV file.
    /// </summary>
    /// <param name="worker">Worker reference for cancellation and reporting.</param>
    /// <param name="e">Event argument, used for result returning.</param>
    protected override void DoWork(BackgroundWorker worker, DoWorkEventArgs e) {
      var success = ExtractAudioFromMp4(_mp4File);
      e.Result = success;
    }


    /// <summary>
    ///   Extract the audio as WAV from a MP4 video. 
    /// </summary>
    /// <param name="videofile">Path to the MP4 file.</param>
    /// <returns>Boolean flag indicating success of operation.</returns>
    private static bool ExtractAudioFromMp4(string videofile) {
      if (!videofile.ToLower().EndsWith(".mp4")) return false; // Ensure MP4 file.
      
      // Build output file name and ensure existance of temporary directory.
      var filename = Path.GetFileNameWithoutExtension(videofile);
      var wavFile = "temp\\"+filename+".wav";
      if (!Directory.Exists("temp")) Directory.CreateDirectory("temp");

      // Extract audio track from MP4 file and save it as WAV.
      var faad = new ProcessStartInfo {
        FileName = "utils\\faad.exe",
        WindowStyle = ProcessWindowStyle.Hidden,
        Arguments = videofile+" -o "+wavFile
      };
      var faadProcess = Process.Start(faad);  // Try to start FAAD process.
      if (faadProcess == null) return false;  // Abort on failure.
      faadProcess.WaitForExit();              // Else wait for finish.
      return true;
    }
  }
}