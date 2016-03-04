using System;
using System.Windows.Forms;
using YouTubeDownloader.Workers;

namespace YouTubeDownloader.UI {

  /// <summary>
  ///   Control class for file conversion on drag-and-drop. 
  /// </summary>
  internal class DragDropControl {

    private readonly Form1 _form;



    public DragDropControl(Form1 form) {
      _form = form;
      _form.DragEnter += (sender, e) => {
        if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
      };
      _form.DragDrop += (sender, e) => {
        //_form.SetMode(1);
        var files = (string[]) e.Data.GetData(DataFormats.FileDrop);
        foreach (string file in files) {


          if (file.ToLower().EndsWith(".mp4")) {
            
            Console.WriteLine("MP4 to MP3 started for file '"+file+"'.");

            var extr = new ExtractionTask("blobb.mp4", (completed, data) => {
              Console.WriteLine("Extraction task was "+(completed? "completed: "+(string)data+"." : "aborted."));
              if (data == null) {
                Console.WriteLine("Error during extraction!");
                //_form.SetMode(0);
              }
              else {
                var compr = new CompressionTask((string)data, (completed2, data2) => {
                  Console.WriteLine("Compression task was "+(completed2? "completed." : "aborted."));
                  if (data2 == null) {
                    Console.WriteLine("Error during compression!");
                    //_form.SetMode(0);
                  }
                  //else _form.SetMode(0);
                });
                compr.Start();
              }
              

            });
            extr.Start();        
          }
        }
        
      };
    }


    public void Enable(bool enabled) {
      _form.AllowDrop = enabled;
    }
  }
}
