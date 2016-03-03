namespace YouTubeDownloader.Workers {
  
  /// <summary>
  ///   Progress announcement function.
  /// </summary>
  /// <param name="percent">Task completion in percent.</param>
  public delegate void ReportProgress(int percent);


  /// <summary>
  ///   Result processing function.
  /// </summary>
  /// <param name="completed">Task completion flag. Set to 'false', if task was cancelled.</param>
  /// <param name="data">The result data. Has to be casted to expected return value.</param>
  public delegate void HandleResult(bool completed, object data);


  /// <summary>
  ///   Base interface for worker tasks.
  /// </summary>
  internal interface IWorkerTask {


    /// <summary>
    ///   Start the asynchronous worker thread.
    /// </summary>
    void Start();


    /// <summary>
    ///   Cancel the worker thread. Only supported on iterative tasks.
    /// </summary>
    void Cancel();
  }
}