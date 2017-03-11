using YouTubeDownloader.UserInterface;
using YouTubeDownloader.Workers;

namespace YouTubeDownloader {
  
  /// <summary>
  ///   Main window initialization code.
  /// </summary>
  public partial class MainWindow {


    /// <summary>
    ///   Create the controller classes and start the WPF.
    /// </summary>
    public MainWindow() {
      var ctrl = new ProgramController();
      var vm = new ViewModel(ctrl);
      InitializeComponent();
      DataContext = vm;
    }
  }
}
