using System;
using System.Drawing;
using System.Windows.Forms;
using YouTubeDownloader.Workers;

namespace YouTubeDownloader.UI {

  /// <summary>
  ///   Video meta data acquisition controller.
  /// </summary>
  internal class AcquisitionControl {

    private readonly Dispatcher _dispatcher;  // UI dispatcher.
    private readonly Label _textfieldTitle;   // The video title.
    private readonly Label _textfieldLength;  // Length of video.
    private readonly Label _textfieldSize;    // Video file size.
    private AcquisitionTask _acquisitionTask; // Worker task for data query. 


    /// <summary>
    ///   Create new meta data acquisor.
    /// </summary>
    /// <param name="dispatcher">Task dispatcher.</param>
    /// <param name="uiElem">Access to the UI elements.</param>
    public AcquisitionControl(Dispatcher dispatcher, Control.ControlCollection uiElem) {
      _dispatcher = dispatcher;
      _textfieldTitle = (Label) uiElem.Find("textfieldTitle", true)[0];
      _textfieldLength = (Label) uiElem.Find("textfieldLength", true)[0];
      _textfieldSize = (Label) uiElem.Find("textfieldSize", true)[0];
    }


    /// <summary>
    ///   Start the acquisition process.
    /// </summary>
    /// <param name="youtubeLink">YouTube link to the video.</param>
    public void StartQuery(string youtubeLink) {
      Console.WriteLine("[AcquisitionControl] Starting process for video '"+youtubeLink+"'.");
      if (_acquisitionTask != null) {
        Console.WriteLine("[AcquisitionControl] Error: Process already active!");
        return;
      }

      _acquisitionTask = new AcquisitionTask(youtubeLink, (completed, data) => {
        var success = completed && (data != null);
        if (success) {
          Console.WriteLine("[AcquisitionControl] Process finished successfully.");
          var metadata = (AcquisitionTask.VideoData) data;
          _textfieldTitle.Text = metadata.Title;
          _textfieldLength.Text = metadata.Length;
          _textfieldSize.Text = metadata.Size;
        }
        else {
          Console.WriteLine("[AcquisitionControl] Process finished with errors!");
          _textfieldTitle.Text = "Fehler!";
          _textfieldTitle.Font = new Font(_textfieldTitle.Font.FontFamily, _textfieldTitle.Font.Size, FontStyle.Regular);
          _textfieldTitle.ForeColor = Color.Red;
        }
        
        _acquisitionTask = null;
        _dispatcher.ReceiveResult("Acquisition", success, data);
      });
      _acquisitionTask.Start();
    }


    /// <summary>
    ///   Clear the video information text fields.
    /// </summary>
    public void Clear() {
      _textfieldTitle.Text = "";
      _textfieldLength.Text = "";
      _textfieldSize.Text = "";
      _textfieldTitle.ForeColor = Color.Black;
    }
  }
}