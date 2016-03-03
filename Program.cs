using System;
using System.Windows.Forms;

namespace YouTubeDownloader {
  
  /// <summary>
  ///   YouTube downloader main program.
  /// </summary>
  internal static class Program {

    /// <summary>
    ///   Main entry point for this application.
    /// </summary>
    [STAThread]
    private static void Main() {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new Form1());
    }
  }
}