using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace YouTubeDownloader.Workers {
  
  /// <summary>
  ///   WAV-to-MP3 compression task.
  /// </summary>
  internal class CompressionTask : WorkerTask {

    private readonly string _wavFile; // Input WAV file to be compressed.


    /// <summary>
    ///   Create a new MP3 compression task.
    /// </summary>
    /// <param name="wavFile">Input Wave-Audio file.</param>
    /// <param name="handleResult">Processing result handler.</param>
    public CompressionTask(string wavFile, HandleResult handleResult)
      : base(handleResult, null) {
      _wavFile = wavFile;
    }
    

    /// <summary>
    ///   Worker main function - compress the WAV file to an MP3.
    /// </summary>
    /// <param name="worker">Worker reference for cancellation and reporting.</param>
    /// <param name="e">Event argument, used for result returning.</param>
    protected override void DoWork(BackgroundWorker worker, DoWorkEventArgs e) {
      var result = CompressWavToMp3(_wavFile);
      e.Result = result;
    }


    /// <summary>
    ///   Compress a WAV audio file as MP3.
    /// </summary>
    /// <param name="wavfile">The input WAV file.</param>
    /// <returns>Path to the output MP3.</returns>
    private static string CompressWavToMp3(string wavfile) {
      if (!wavfile.ToLower().EndsWith(".wav")) return null; // Abort on wrong file type.

      // Create target file name, save it optionally in an 'output' folder.
      var filename = Path.GetFileNameWithoutExtension(wavfile);
      var mp3File = filename+".mp3"; 
      if (Directory.Exists("output")) mp3File = "output\\" + mp3File;

      // Compress WAV audio to MP3 format.
      var lame = new ProcessStartInfo {
        FileName = "utils\\lame.exe",
        WindowStyle = ProcessWindowStyle.Hidden,
        Arguments = "--preset standard \""+wavfile+"\" \""+mp3File+"\""
      };        
      var lameProcess = Process.Start(lame);  // Start the LAME process.
      if (lameProcess == null) return null;   // Abort on failure.
      lameProcess.WaitForExit();              // Else wait for finish.
      return mp3File;
    }
  }
}