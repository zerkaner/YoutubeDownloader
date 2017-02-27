namespace YouTubeDownloader {
  
  /// <summary>
  ///   Main window initialization code.
  /// </summary>
  public partial class MainWindow {


    /// <summary>
    ///   Create the controller classes and start the WPF.
    /// </summary>
    public MainWindow() {
      var mainCtrl = new MainController();
      var ifCtrl = new InterfaceController(mainCtrl);
      InitializeComponent();
      DataContext = ifCtrl;
    }
  }
}
