using System.Windows.Forms;
using YouTubeDownloader.UI;

namespace YouTubeDownloader {


  public sealed partial class Form1 : Form {


    public Form1() {
      InitializeComponent();
      new Dispatcher(this);
    }
  }
}
