using System;
using System.Drawing;
using System.Windows.Forms;
using YouTubeDownloader.Workers;

namespace YouTubeDownloader.UI {

  /// <summary>
  ///   This controller manages the extraction progress and its UI elements.
  /// </summary>
  internal class ExtractionControl {

    private readonly Dispatcher _dispatcher; // UI dispatcher.
    private readonly Label _statusExtract;   // Label for extraction process.
    private ExtractionTask _extractionTask;  // Extraction worker task. 


    /// <summary>
    ///   Create new extraction control.
    /// </summary>
    /// <param name="dispatcher">Task dispatcher.</param>
    /// <param name="uiElem">Access to the UI elements.</param>
    public ExtractionControl(Dispatcher dispatcher, Control.ControlCollection uiElem) {
      _dispatcher = dispatcher;
      _statusExtract = (Label) uiElem.Find("statusExtract", true)[0];
    }


    /// <summary>
    ///   Start the extraction process.
    /// </summary>
    /// <param name="mp4File">MP4 video to extract audio from.</param>
    public void StartExtraction(string mp4File) {
      Console.WriteLine("[ExtractionControl] Starting process for file '"+mp4File+"'.");
      _statusExtract.Text = "läuft";
      _statusExtract.Font = new Font(_statusExtract.Font.FontFamily, _statusExtract.Font.Size,FontStyle.Italic);
      if (_extractionTask != null) {
        Console.WriteLine("[ExtractionControl] Error: Process already active!");
        return;
      }

      _extractionTask = new ExtractionTask(mp4File, (completed, data) => {
        var success = completed && (data != null);
        if (success) {
          Console.WriteLine("[ExtractionControl] Process finished successfully.");
          _statusExtract.Text = "OK.";
          _statusExtract.Font = new Font(_statusExtract.Font.FontFamily, _statusExtract.Font.Size, FontStyle.Regular);
          _statusExtract.ForeColor = Color.Green;
        }
        else {
          Console.WriteLine("[ExtractionControl] Process finished with errors!");
          _statusExtract.Text = "Fehler!";
          _statusExtract.Font = new Font(_statusExtract.Font.FontFamily, _statusExtract.Font.Size, FontStyle.Regular);
          _statusExtract.ForeColor = Color.Red;
        }
        
        _extractionTask = null;
        _dispatcher.ReceiveResult("Extraction", success, data);
      });
      _extractionTask.Start();
    }


    /// <summary>
    ///   Clear the status text.
    /// </summary>
    public void Clear() {
      _statusExtract.Text = "";
      _statusExtract.ForeColor = Color.Black;
    }
  }
}