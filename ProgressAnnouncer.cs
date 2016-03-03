using System.Drawing;
using System.Windows.Forms;
// ReSharper disable SwitchStatementMissingSomeCases   

namespace YouTubeDownloader {
  
  
  /** The progress announcer handles the display of status messages. */
  internal class ProgressAnnouncer {
    
    private readonly Label _title, _length, _filesize;     // Video meta information.
    private readonly Label _download, _extract, _compress; // The processing stages.
    private readonly ProgressBar _downloadBar;             // Download progress bar.
    private readonly Label _finished;                      // The "finished" label.
    private readonly Button _abort;                        // Abort button to stop.


    /** Create a new progress announcer by setting up all references.
     * @param form The control group to query the elements from. */
    public ProgressAnnouncer(Control.ControlCollection form) {
      _title    = (Label) form.Find("textfieldTitle", true)[0];
      _length   = (Label) form.Find("textfieldLength", true)[0];
      _filesize = (Label) form.Find("textfieldSize", true)[0];
      _download = (Label) form.Find("procentDownload", true)[0];
      _extract  = (Label) form.Find("statusExtract", true)[0];
      _compress = (Label) form.Find("statusCompress", true)[0];
      _downloadBar = (ProgressBar) form.Find("progressDownload", true)[0];
      _finished = (Label) form.Find("labelDone", true)[0];
      _abort = (Button) form.Find("buttonAbort", true)[0];
      ClearAll();
    }


    /** Clear all display elements. */
    public void ClearAll() {
      SetStatus("EXTRACT", 0);
      SetStatus("COMPRESS", 0);
      SetVideoData("", "", "");
      SetDownloadProgress(0);
      ToggleBottomBar(0);
    }


    /** Output the video meta data. 
     * @param title The video title (equals later filename).
     * @param length Video length (already formatted). 
     * @param size The video file size. */
    public void SetVideoData(string title, string length, string size) {
      _title.Text = title;
      _length.Text = length;
      _filesize.Text = size;
    }


    /** Set the download progress text.
     * @param procent A progress value 0-100. */
    public void SetDownloadProgress(int procent) {
      if (procent < 0) procent = 0;
      if (procent > 100) procent = 100;
      _download.Text = procent+" %";
      _downloadBar.Value = procent;
    }


    /** Change the status display for extraction and compression operations.
     * @param type Either 'EXTRACT' or 'COMPRESS'.
     * @param mode One of the following: 0-disabled, 1-running, 2-finished. */
    public void SetStatus(string type, int mode) {
      Label lbl = null;
      if      (type.ToUpper().Equals("EXTRACT"))  lbl = _extract;
      else if (type.ToUpper().Equals("COMPRESS")) lbl = _compress;
      if (lbl == null) return;
      switch (mode) {
        case 0: {
          lbl.Text = "";
          break;
        }
        case 1: {
          lbl.Text = "läuft";
          lbl.Font = new Font(lbl.Font.FontFamily, lbl.Font.Size,FontStyle.Italic);
          break;          
        }
        case 2: {
          lbl.Text = "OK";
          lbl.Font = new Font(lbl.Font.FontFamily, lbl.Font.Size,FontStyle.Regular);
          break;          
        }
      }
    }


    /** Toggles the display in the last row.
     * @param mode 0-not running, 1-running, 2-finished. */
    public void ToggleBottomBar(int mode) {
      switch (mode) {
        case 0: {
          _finished.Visible = false;
          _abort.Enabled = false;
          _abort.Visible = true;
          break;
        }
        case 1: {
          _finished.Visible = false;
          _abort.Enabled = true;
          _abort.Visible = true;
          break;          
        }
        case 2: {
          _finished.Visible = true;
          _abort.Visible = false;
          break;          
        }        
      }
    }
  }
}