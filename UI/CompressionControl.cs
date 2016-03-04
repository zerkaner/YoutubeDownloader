using System;
using System.Drawing;
using System.Windows.Forms;
using YouTubeDownloader.Workers;

namespace YouTubeDownloader.UI {

  /// <summary>
  ///   Controller for the MP3 compression task.
  /// </summary>
  internal class CompressionControl {

    private readonly Dispatcher _dispatcher;  // UI dispatcher.
    private readonly Label _statusCompress;   // Label for the compression process.
    private CompressionTask _compressionTask; // Compression worker task. 


    /// <summary>
    ///   Create new compression controller.
    /// </summary>
    /// <param name="dispatcher">Task dispatcher.</param>
    /// <param name="uiElem">Access to the UI elements.</param>
    public CompressionControl(Dispatcher dispatcher, Control.ControlCollection uiElem) {
      _dispatcher = dispatcher;
      _statusCompress = (Label) uiElem.Find("statusCompress", true)[0];
    }


    /// <summary>
    ///   Start the compression process.
    /// </summary>
    /// <param name="wavFile">Wave audio file to compress.</param>
    /// <param name="deleteOnSuccess">Delete WAV file on successful compression? (Default: true)</param>
    public void StartCompression(string wavFile, bool deleteOnSuccess = true) {
      Console.WriteLine("[CompressionControl] Starting process for file '"+wavFile+"'.");
      _statusCompress.Text = "läuft";
      _statusCompress.Font = new Font(_statusCompress.Font.FontFamily, _statusCompress.Font.Size,FontStyle.Italic);
      if (_compressionTask != null) {
        Console.WriteLine("[CompressionControl] Error: Process already active!");
        return;
      }

      _compressionTask = new CompressionTask(wavFile, (completed, data) => {
        var success = completed && (data != null);
        if (success) {
          Console.WriteLine("[CompressionControl] Process finished successfully.");
          _statusCompress.Text = "OK.";
          _statusCompress.Font = new Font(_statusCompress.Font.FontFamily, _statusCompress.Font.Size, FontStyle.Regular);
          _statusCompress.ForeColor = Color.Green;
        }
        else {
          Console.WriteLine("[CompressionControl] Process finished with errors!");
          _statusCompress.Text = "Fehler!";
          _statusCompress.Font = new Font(_statusCompress.Font.FontFamily, _statusCompress.Font.Size, FontStyle.Regular);
          _statusCompress.ForeColor = Color.Red;
        }
        
        _compressionTask = null;
        _dispatcher.ReceiveResult("Compression", success, data);
      });
      _compressionTask.Start();
    }


    /// <summary>
    ///   Clear the status text.
    /// </summary>
    public void Clear() {
      _statusCompress.Text = "";
      _statusCompress.ForeColor = Color.Black;
    }
  }
}