using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace YouTubeDownloader.Workers {

  /// <summary>
  ///   This worker class offers the various asyncronous methods for downloading,
  ///   extracting and compressing MP4 videos and their audio streams.
  /// </summary>
  internal class TaskWorker {

    private WebClient _dlWorker;   // Web client to download data with.
    private Process _extrProcess;  // MP4 -> WAV extraction task.
    private Process _comprProcess; // WAV -> MP3 compression task.


    /// <summary>
    ///   Download a file from the internet.
    /// </summary>
    /// <param name="address">HTTP address of the file to download.</param>
    /// <param name="resHndl">Callback to receive the file's bytes [array of bytes].</param>
    /// <param name="progHndl">Callback for progress announcements.</param>
    public void StartDownload(string address, ResultHandler resHndl, ProgressHandler progHndl) {      
      _dlWorker = new WebClient();
      if (progHndl != null) _dlWorker.DownloadProgressChanged += (sender, e) => {
        progHndl(e.ProgressPercentage);
      };  
      _dlWorker.DownloadDataCompleted += (sender, e) => {
        if (e.Error != null) resHndl(false, e.Error);
        else {
          if (e.Cancelled) resHndl(false, null);
          else resHndl(true, e.Result);          
        }
        _dlWorker.Dispose();
      };
      _dlWorker.DownloadDataAsync(new Uri(address));
    }


    /// <summary>
    ///   Extract the audio track as WAV from a MP4 file.
    ///   This function uses the external tool "faad.exe", expected to be in the 'utils' folder.
    /// </summary>
    /// <param name="mp4File">The MP4 video file.</param>
    /// <param name="resHndl">Callback to receive the final WAV audio file [string path].</param>
    /// <param name="progHndl">Callback for progress announcements.</param>
    public void StartExtraction(string mp4File, ResultHandler resHndl, ProgressHandler progHndl) {
      var worker = new BackgroundWorker {
        WorkerReportsProgress = progHndl != null
      };
      if (progHndl != null) worker.ProgressChanged += (sender, e) => {
        progHndl(e.ProgressPercentage);
      };  
      worker.RunWorkerCompleted += (sender, e) => {
        resHndl(true, e.Result);
        worker.Dispose();
      };
      worker.DoWork += (sender, e) => {

        // Build output file name and ensure existance of temporary directory.
        var wavFile = mp4File.Substring(0, mp4File.Length-4)+".wav";

        // Extract audio track from MP4 file and save it as WAV.
        _extrProcess = new Process {
          StartInfo = new ProcessStartInfo("utils\\faad.exe") {
            CreateNoWindow = true,
            UseShellExecute = false,
            Arguments = $"\"{mp4File}\" -o \"{wavFile}\"",
            RedirectStandardError = true,
            RedirectStandardOutput = true
          }
        };
        // FAAD writes its output on STDERR instead of STDOUT. No idea, why...
        _extrProcess.ErrorDataReceived += (pSender, pEventArgs) => {
          if (!string.IsNullOrEmpty(pEventArgs.Data)) {
            var msg = pEventArgs.Data.Split(' ');
            if (msg.Length > 0 && msg[0].EndsWith("%")) {
              var percentage = int.Parse(msg[0].Substring(0, msg[0].Length - 1));
              worker.ReportProgress(percentage);
            } 
          }
        };
        _extrProcess.Start();               // Process start signal.        
        _extrProcess.BeginOutputReadLine(); // Listen for output and reroute.
        _extrProcess.BeginErrorReadLine();  // Also listen for the errors.
        _extrProcess.WaitForExit();         // Halt task until process termination.
        _extrProcess = null;
        e.Result = File.Exists(wavFile) ? wavFile : null;
      };      
      worker.RunWorkerAsync(); 
    }


    /// <summary>
    ///   Compress a WAV audio file as MP3.
    ///   This function uses the external tool "lame.exe", expected in the 'utils' subfolder.
    /// </summary>
    /// <param name="wavFile">The input WAV file.</param>
    /// <param name="resHndl">Callback function receiving the output file [string path].</param>
    /// <param name="progHndl">Callback for progress announcements.</param>
    public void StartCompression(string wavFile, ResultHandler resHndl, ProgressHandler progHndl) {
      var worker = new BackgroundWorker {
        WorkerReportsProgress = progHndl != null
      };
      if (progHndl != null) worker.ProgressChanged += (sender, e) => {
        progHndl(e.ProgressPercentage);
      };  
      worker.RunWorkerCompleted += (sender, e) => {
        resHndl(true, e.Result);
        worker.Dispose();
      };
      worker.DoWork += (sender, e) => {
        
        // Create file save name.
        var mp3File = wavFile.Substring(0, wavFile.Length-4)+".mp3";

        // Extract audio track from MP4 file and save it as WAV.
        _comprProcess = new Process {
          StartInfo = new ProcessStartInfo("utils\\lame.exe") {
            CreateNoWindow = true,
            UseShellExecute = false,
            Arguments = $"--preset standard \"{wavFile}\" \"{mp3File}\"",
            RedirectStandardError = true,
            RedirectStandardOutput = true
          }
        };
        // LAME also writes on STDERR. Find that percentage value and report it.
        _comprProcess.ErrorDataReceived += (pSender, pEventArgs) => {
          if (!string.IsNullOrEmpty(pEventArgs.Data)) {
            foreach (var str in pEventArgs.Data.Split(' ')) {
              if (str.EndsWith("%)|")) {
                var pct = str.Substring(0, str.Length - 3);
                if (pct.StartsWith("(")) pct = pct.Substring(1);
                worker.ReportProgress(int.Parse(pct));
                break;
              }
            }
          }
        };
        _comprProcess.Start();               // Process start signal.        
        _comprProcess.BeginOutputReadLine(); // Listen for output and reroute.
        _comprProcess.BeginErrorReadLine();  // Also listen for the errors.
        _comprProcess.WaitForExit();         // Halt task until process termination.

        // Process killed or failed.
        if (_comprProcess.ExitCode == -1) {
          if (File.Exists(mp3File)) File.Delete(mp3File);
          e.Result = null;
        }

        // Process terminated successfully (code 0).
        else {
          e.Result = File.Exists(mp3File) ? mp3File : null;
        }
      };
      worker.RunWorkerAsync(); 
    }


    /// <summary>
    ///   Cancel an ongoing file download.
    /// </summary>
    public void CancelDownload() {
      if (_dlWorker != null && _dlWorker.IsBusy) _dlWorker.CancelAsync();
    }


    /// <summary>
    ///   Stop the MP4->WAV extraction task.
    /// </summary>
    public void CancelExtraction() {
      _extrProcess?.Kill();
    }


    /// <summary>
    ///   Stop the WAV->MP3 compression task.
    /// </summary>
    public void CancelCompression() {
      _comprProcess?.Kill();
    }


    /// <summary>
    ///   Progress announcement function.
    /// </summary>
    /// <param name="percent">Task completion in percent.</param>
    public delegate void ProgressHandler(int percent);


    /// <summary>
    ///   Result processing function.
    /// </summary>
    /// <param name="completed">Task completion flag. Set to 'false', if task was cancelled.</param>
    /// <param name="data">The result data. Has to be casted to expected return value.</param>
    public delegate void ResultHandler(bool completed, object data);
  }
}
