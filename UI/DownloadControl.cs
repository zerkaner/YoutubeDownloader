using System;
using System.Drawing;
using System.Windows.Forms;
using YouTubeDownloader.Workers;

namespace YouTubeDownloader.UI {

  /// <summary>
  ///   Controller for the download process.
  /// </summary>
  internal class DownloadControl {

    private readonly Dispatcher _dispatcher;   // UI dispatcher.
    private readonly ProgressBar _progressBar; // Progress bar for download.
    private readonly Label _progressLabel;     // Label for the percentage.
    private DownloadTask _downloadTask;        // Asynchronous download task. 


    /// <summary>
    ///   Create new download controller.
    /// </summary>
    /// <param name="dispatcher">Task dispatcher.</param>
    /// <param name="uiElem">Access to the UI elements.</param>
    public DownloadControl(Dispatcher dispatcher, Control.ControlCollection uiElem) {
      _dispatcher = dispatcher;
      _progressBar = (ProgressBar) uiElem.Find("progressDownload", true)[0];
      _progressLabel = (Label) uiElem.Find("procentDownload", true)[0];
    }


    /// <summary>
    ///   Start the video download.
    /// </summary>
    /// <param name="video">Container with the video parameters.</param>
    public void StartDownload(AcquisitionTask.VideoData video) {
      Console.WriteLine("[DownloadControl] Starting download of video '"+video.Title+"'.");
      if (_downloadTask != null) {
        Console.WriteLine("[DownloadControl] Error: Process already active!");
        return;
      }

      _downloadTask = new DownloadTask(video.VideoUrl, 
        
        // Download result handler.
        (completed, data) => {
          var success = completed && (data != null);
          if (success) {
            Console.WriteLine("[DownloadControl] Process finished successfully.");
            _progressLabel.ForeColor = Color.Green;
          }
          else {
            Console.WriteLine("[DownloadControl] Process finished with errors!");
            _progressLabel.ForeColor = Color.Red;
          }
        
          _downloadTask = null;
          _dispatcher.ReceiveResult("Download", success, new [] {video.Title, data});
        }, 
      
        // Progress change handler.
        percent => {
          _progressBar.Value = percent;
          _progressLabel.Text = percent+" %";
        }
      );
      _downloadTask.Start();
    }


    /// <summary>
    ///   Clear the download progress.
    /// </summary>
    public void Clear() {
      _progressBar.Value = 0;
      _progressLabel.Text = "0 %";
      _progressLabel.ForeColor = Color.Black;
    }
  }
}
