using System;
using System.Net;

namespace YouTubeDownloader.Workers {
  
  /// <summary>
  ///   Worker thread to download arbitrary resources from the Web.
  /// </summary>
  internal class DownloadTask : IWorkerTask {

    private readonly WebClient _worker;  // Web client to download data with.
    private readonly Uri _address;       // Resource address to fetch.


    /// <summary>
    ///   Create the download task by creating the web client and assigning the delegates.
    /// </summary>
    /// <param name="address">Address of resource to download.</param>
    /// <param name="handleResult">Delegate function to be called on download completion.</param>
    /// <param name="reporter">Progress announcement. Set to 'null', if not used.</param>
    public DownloadTask(Uri address, HandleResult handleResult, ReportProgress reporter = null) {
      _worker = new WebClient();
      _address = address;

      // Add the returning function.
      _worker.DownloadDataCompleted += (sender, e) => { handleResult(!e.Cancelled, e.Result); };

      // Add progress reporter, if available.
      if (reporter != null) {
        _worker.DownloadProgressChanged += (sender, e) => { reporter(e.ProgressPercentage); };  
      }
    }


    /// <summary>
    ///   Start the asynchronous worker thread.
    /// </summary>
    public void Start() {
      _worker.DownloadDataAsync(_address);
    }


    /// <summary>
    ///   Cancel the worker thread. Only supported on iterative tasks.
    /// </summary>
    public void Cancel() {
      _worker.CancelAsync();
    }
  }
}