using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows;
using YouTubeDownloader.Workers;

namespace YouTubeDownloader.UserInterface {


  public class VideoInfoVm {
    public string Title { get; internal set; }
    public string Uploader { get; internal set; }
    public string Length { get; internal set; }
    public string Views { get; internal set; }
    public string Rating { get; internal set; }
    public string CoverImage { get; internal set; }
  }


  public class ViewModel : ObservableObject {

    private readonly TaskWorker _taskWorker;

    //_________________________________________________________________________
    // View model properties and button controls.

    private VideoInfo _videoInfo;
    public VideoInfoVm VideoInfo { get; }   // Video description view model.
    public string VideoLink { get; set; }   // Video URL (bound to the address field).

    public string ErrorMessage { get; private set; }
    public string DownloadMessage { get; private set; }
    public string DownloadProgress { get; private set; }
    public string ExtractionProgress { get; private set; }
    public string CompressionProgress { get; private set; }
    public string ClearUpProgress { get; private set; }

    public DelegateCommand InsertClipboard { get; private set; }
    public DelegateCommand FetchVideoData  { get; private set; }
    public DelegateCommand DownloadAudio   { get; private set; }
    public DelegateCommand DownloadVideo   { get; private set; }
    public DelegateCommand CancelProcess   { get; private set; }
    public DelegateCommand OpenFile        { get; private set; }



    public bool DownloadVideoOnly { get; set; }
    public bool DownloadOptionsVisible { get; private set; }
    public bool ErrorPanelVisible { get; private set; }
    public bool DownloadPanelVisible { get; private set; }

    private enum State { QueryOk, QueryStarted, Error, ProcessStarted }


    private void SetState(State content) {
      switch (content) {
        case State.QueryStarted:
          DownloadOptionsVisible = false; RaisePropertyChangedEvent("DownloadOptionsVisible");
          DownloadPanelVisible = false;   RaisePropertyChangedEvent("DownloadPanelVisible"); 
          break;
        case State.QueryOk:
          ErrorPanelVisible = false;      RaisePropertyChangedEvent("ErrorPanelVisible");
          DownloadOptionsVisible = true;  RaisePropertyChangedEvent("DownloadOptionsVisible");   
          break;
        case State.Error:
          DownloadPanelVisible = false;   RaisePropertyChangedEvent("DownloadPanelVisible"); 
          ErrorPanelVisible = true;       RaisePropertyChangedEvent("ErrorPanelVisible");  
          if (Directory.Exists("temp")) Directory.Delete("temp", true);
          break;
        case State.ProcessStarted:
          DownloadProgress = "";          RaisePropertyChangedEvent("DownloadProgress"); 
          ExtractionProgress = "";        RaisePropertyChangedEvent("ExtractionProgress"); 
          CompressionProgress = "";       RaisePropertyChangedEvent("CompressionProgress"); 
          ClearUpProgress = "";           RaisePropertyChangedEvent("ClearUpProgress"); 
          DownloadOptionsVisible = false; RaisePropertyChangedEvent("DownloadOptionsVisible"); 
          DownloadPanelVisible = true;    RaisePropertyChangedEvent("DownloadPanelVisible");
          PerformVideoDownload(); 
          break;
      }
    }


    /// <summary>
    ///   Create a new user interface VM, setting up initial values.
    /// </summary>
    /// <param name="prod">Production start disambiguation.</param>
    internal ViewModel(bool prod) {
      _taskWorker = new TaskWorker();
      VideoInfo = new VideoInfoVm();
      SetupButtonCommands();
      if (!prod) {
        VideoLink = "https://www.youtube.com/watch?v=Dihka60wAmU";
      }
    }


    /// <summary>
    ///   Default constructor. This one is called by the WPF-designer and presents a preview.
    /// </summary>
    public ViewModel() {
      VideoInfo = new VideoInfoVm();
      SetVideoInfo(new VideoInfo {
        Title = "Dropkick Murphys - I'm Shipping up to Boston",
        Uploader = "Dropkick Murphys",
        ViewCount = 3467537,
        LengthSec = 3642,
        Rating = 4.7865f,
        Thumbnail = "https://i.ytimg.com/vi/x-64CaD8GXw/hqdefault.jpg?custom=true&amp;w=168&amp;h=94&amp;stc=true&amp;jpg444=true&amp;jpgq=90&amp;sp=68&amp;sigh=uK_m3jr89fDSer2e6sU3w8Ie9G8"       
      });
      DownloadPanelVisible = true;
      ErrorMessage = "Interner Fehler. Die genaue Beschreibung liegt unter ...";
      DownloadMessage = "Lade Audio herunter ...";
      DownloadVideoOnly = true;
    }


    /// <summary>
    ///   Write the video information from the acquired description to the view model.
    /// </summary>
    /// <param name="videoInfo">Video data received.</param>
    private void SetVideoInfo(VideoInfo videoInfo) {
      _videoInfo = videoInfo;
      VideoInfo.Title = videoInfo.Title;
      VideoInfo.Uploader = videoInfo.Uploader;
      VideoInfo.CoverImage = videoInfo.Thumbnail;
      VideoInfo.Views = videoInfo.ViewCount.ToString();
      VideoInfo.Rating = videoInfo.Rating.ToString("N2") + " / 5 Sterne";
      var secs = videoInfo.LengthSec;
      if (secs < 60) VideoInfo.Length = secs + " Sekunden";
      else if (secs < 3600) {
        var min = secs/60;
        var sek = secs - min*60;
        VideoInfo.Length = min + ":" + sek.ToString("D2") + " Minuten";
      }
      else {
        var hrs = secs/3600;
        var r1 = secs - hrs*3600;
        var min = r1/60;
        var sek = r1 - min*60;
        VideoInfo.Length = hrs + ":" + min.ToString("D2") + ":" + sek.ToString("D2") + " Stunden";
      }
      RaisePropertyChangedEvent("VideoInfo");      
    }



    /// <summary>    
    ///   Initialize the button commands.
    /// </summary>
    private void SetupButtonCommands() {
      
      FetchVideoData = new DelegateCommand(
        () => {
          SetState(State.QueryStarted);
          VideoInfo videoInfo;
          var result = Acquisor.GetVideoMetadata(VideoLink, out videoInfo);
          Debug.WriteLine(result);
          if (result == QueryStatus.Found) {
            SetVideoInfo(videoInfo);
            SetState(State.QueryOk);
          }
          else if (result == QueryStatus.Copyrighted) {
            SetVideoInfo(videoInfo);
            ErrorMessage = 
              "Video ist Copyright-geschützt.\n"+
              "Das ausgewählte Video ist leider derzeit nicht extern abrufbar und kann "+
              "daher nicht heruntergeladen werden.";
            RaisePropertyChangedEvent("ErrorMessage");
            SetState(State.Error);            
          }
        }, () => true       
      );

      InsertClipboard = new DelegateCommand(
        () => {
          VideoLink = Clipboard.GetText();
          RaisePropertyChangedEvent("VideoLink");
          CancelProcess.Execute(null);
        },
        () => true
      );

      OpenFile = new DelegateCommand(
        () => {
          var appPath = AppDomain.CurrentDomain.BaseDirectory;
          Process.Start(appPath+"test.txt");
        },
        () => true       
      );

      DownloadVideo = new DelegateCommand(
        () => {
          DownloadMessage = "Lade Video herunter ..."; RaisePropertyChangedEvent("DownloadMessage");
          DownloadVideoOnly = true;                    RaisePropertyChangedEvent("DownloadVideoOnly");
          SetState(State.ProcessStarted);
        }, () => true
      );

      DownloadAudio = new DelegateCommand(
        () => {
          DownloadMessage = "Lade Audio herunter ..."; RaisePropertyChangedEvent("DownloadMessage");
          DownloadVideoOnly = false;                   RaisePropertyChangedEvent("DownloadVideoOnly");
          SetState(State.ProcessStarted);
        }, () => true
      );

      CancelProcess = new DelegateCommand(
        () => {
          _taskWorker.CancelDownload();
          _taskWorker.CancelExtraction();
          _taskWorker.CancelCompression();
        }, () => true
      );
    }



    private void PerformVideoDownload() {
      var stream = _videoInfo.Streams[0].Address;
      _taskWorker.StartDownload(stream, (completed, data) => {
        if (completed) {
          DownloadProgress = "fertig";
          RaisePropertyChangedEvent("DownloadProgress");
          var fileName = VideoInfo.Title;
          foreach (var ch in Path.GetInvalidFileNameChars()) {
            fileName = fileName.Replace(ch, '_');
          }
          if (!Directory.Exists("temp")) Directory.CreateDirectory("temp");
          fileName = "temp" + Path.DirectorySeparatorChar + fileName + ".mp4";
          var bytes = (byte[]) data;
          File.WriteAllBytes(fileName, bytes);
          if (DownloadVideoOnly) SaveFile(fileName);
          else ExtractAndRecompress(fileName); 
        }
        else {
          if (data != null) ErrorMessage = 
            "Interner Fehler während des Download-Vorgangs:\n\n"+(WebException) data;
          else ErrorMessage = "Der Download wurde abgebrochen.";
          RaisePropertyChangedEvent("ErrorMessage");
          SetState(State.Error);
        }            
      }, percent => {
        DownloadProgress = percent + " %";
        RaisePropertyChangedEvent("DownloadProgress");
      });      
    }


    private void ExtractAndRecompress(string videoFile) {
      _taskWorker.StartExtraction(videoFile, (c1, d1) => {
        if (d1 != null) {
          ExtractionProgress = "fertig";
          RaisePropertyChangedEvent("ExtractionProgress");
          var wavFile = (string) d1;
          _taskWorker.StartCompression(wavFile, (c2, d2) => {
            if (d2 != null) {
              CompressionProgress = "fertig";
              RaisePropertyChangedEvent("CompressionProgress"); 
              SaveFile((string) d2);
            }
            else {
              ErrorMessage = "MP3-Kompression fehlgeschlagen oder abgebrochen.";
              RaisePropertyChangedEvent("ErrorMessage");                
            }
          }, p2 => {
            CompressionProgress = p2 + " %";
            RaisePropertyChangedEvent("CompressionProgress");  
          });
        }
        else {
          ErrorMessage = "Extraktion fehlgeschlagen oder abgebrochen.";
          RaisePropertyChangedEvent("ErrorMessage");          
        }
      }, p1 => {
        ExtractionProgress = p1 + " %";
        RaisePropertyChangedEvent("ExtractionProgress");        
      });
    }


    private void SaveFile(string file) {

      // Get unique save name and directory.
      var isVideo = file.ToLower().EndsWith(".mp4");
      var name = Path.GetFileNameWithoutExtension(file);
      var path = isVideo ? "Videos" : "Musik";
      var ext = isVideo ? ".mp4" : ".mp3";
      if (!Directory.Exists(path)) Directory.CreateDirectory(path);
      path += Path.DirectorySeparatorChar + name;
      var i = 0;
      var fullpath = path + ext;
      while (File.Exists(fullpath)) fullpath = path + " (" + ++i + ")" + ext;

      // Copy output file to destination and clear temporary folder.
      File.Copy(file, fullpath);
      if (Directory.Exists("temp")) Directory.Delete("temp", true);

      // Report finish status.
      ClearUpProgress = "fertig";
      RaisePropertyChangedEvent("ClearUpProgress");  
    }
  }
}
