using System.ComponentModel;

namespace YouTubeDownloader.Workers {

  /// <summary>
  ///   Worker task base class.
  /// </summary>
  internal abstract class WorkerTask : IWorkerTask {

    private readonly BackgroundWorker _worker; // Worker thread reference.


    /// <summary>
    ///   Create a new worker task.
    /// </summary>
    /// <param name="handleResult">Delegate function to be called on task completion.</param>
    /// <param name="reportProgress">Progress announcement. Set to 'null', if not used.</param>
    protected WorkerTask(HandleResult handleResult, ReportProgress reportProgress = null) {
      _worker = new BackgroundWorker {
        WorkerReportsProgress = true,
        WorkerSupportsCancellation = reportProgress != null
      };

      // Add work logic and returning function.
      _worker.DoWork += (sender, e) => { DoWork((BackgroundWorker) sender, e); };      
      _worker.RunWorkerCompleted += (sender, e) => { handleResult(!e.Cancelled, e.Result); };

      // Add progress reporter, if available.
      if (reportProgress != null) {
        _worker.ProgressChanged += (sender, e) => { reportProgress(e.ProgressPercentage); };  
      }
    }


    /// <summary>
    ///   Start the asynchronous worker thread.
    /// </summary>
    public void Start() {
      _worker.RunWorkerAsync(); 
    }


    /// <summary>
    ///   Cancel the worker thread. Only supported on iterative tasks.
    /// </summary>
    public void Cancel() {
      _worker.CancelAsync();
    }


    /// <summary>
    ///   Worker main function - to be implemented by specific task class.
    /// </summary>
    /// <param name="worker">Worker reference for cancellation and reporting.</param>
    /// <param name="e">Event argument, used for result returning.</param>
    protected abstract void DoWork(BackgroundWorker worker, DoWorkEventArgs e);
  }
}