using System;
using System.Diagnostics;
using YouTubeDownloader.Workers;
// ReSharper disable MemberCanBePrivate.Global

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

    private readonly ProgramController _controller;  // Program workflow controller.

    //_________________________________________________________________________
    // View model properties and button controls.

    public VideoInfoVm VideoInfo { get; }   // Video description view model.
    public string VideoLink { get; set; }   // Video URL (bound to the address field).

    public DelegateCommand InsertClipboard { get; private set; }
    public DelegateCommand FetchVideoData  { get; private set; }
    public DelegateCommand DownloadAudio   { get; private set; }
    public DelegateCommand DownloadVideo   { get; private set; }
    public DelegateCommand CancelProcess   { get; private set; }
    public DelegateCommand OpenFile        { get; private set; }


    /// <summary>
    ///   Create a new user interface VM, setting up initial values.
    /// </summary>
    /// <param name="ctrl">Main controller reference.</param>
    internal ViewModel(ProgramController ctrl) {
      _controller = ctrl;
      VideoInfo = new VideoInfoVm();
      SetupButtonCommands();
      VideoLink = "https://www.youtube.com/watch?v=Dihka60wAmU";  // TODO Remove!
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
    }


    /// <summary>
    ///   Write the video information from the acquired description to the view model.
    /// </summary>
    /// <param name="videoInfo">Video data received.</param>
    private void SetVideoInfo(VideoInfo videoInfo) {
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
          VideoInfo videoInfo;
          var result = Acquisor.GetVideoMetadata(VideoLink, out videoInfo);
          Debug.WriteLine(result);
          if (result == QueryStatus.Found || result == QueryStatus.Copyrighted) {
            SetVideoInfo(videoInfo);
          }
        }, () => true       
      );

      InsertClipboard = new DelegateCommand(
        () => { Debug.WriteLine("clicked"); },
        () => true
      );

      OpenFile = new DelegateCommand(
        () => {
          var appPath = AppDomain.CurrentDomain.BaseDirectory;
          Process.Start(appPath+"test.txt");
        },
        () => true       
      );
    }
  }
}
