using System.Diagnostics;
using System.Windows.Input;
using YouTubeDownloader.UserInterface;

namespace YouTubeDownloader {
  
  /// <summary>
  ///   Main window initialization code.
  /// </summary>
  public partial class MainWindow {


    /// <summary>
    ///   Create the controller classes and start the WPF.
    /// </summary>
    public MainWindow() {
      var vm = new ViewModel(true);
      InitializeComponent();
      DataContext = vm;
      
      // Handler to automatically select all text on click.
      LinkInputBox.AddHandler(
        MouseLeftButtonDownEvent, 
        new MouseButtonEventHandler((sender, e) => {
          LinkInputBox.SelectAll();
        }), 
        true
      );

      // Navigate to GitHub page on version link click.
      VersionLink.RequestNavigate += (sender, e) => {
        Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
        e.Handled = true;
      };
    }
  }
}
