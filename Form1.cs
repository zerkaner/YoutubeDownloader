using System;
using System.Windows.Forms;
using YouTubeDownloader.Workers;

namespace YouTubeDownloader {


  public partial class Form1 : Form {


    private readonly ProgressAnnouncer _announcer;

    public Form1() {
      InitializeComponent();
      
      _announcer = new ProgressAnnouncer(Controls);


      /*
      // Set the event handler for the progress bars (during progress).
      converter.DownloadThread.DownloadProgressChanged += (sender, e) => {
        progressDownload.Value = e.ProgressPercentage;
        procentDownload.Text = e.ProgressPercentage + " %";
      };
      converter.ConverterThread.ProgressChanged += (sender, e) => {
        //progressConversion.Value = e.ProgressPercentage;
        //procentConversion.Text = e.ProgressPercentage + " %";
      };

      // Event handler for finished download and conversion.
      converter.DownloadThread.DownloadDataCompleted += (sender, e) => {
        if (e.Cancelled) SwitchState(ProgramState.Reset);
        else {
          progressDownload.Value = 100;
          procentDownload.Text = "100 %";
          _converter.ConvertAudio(e.Result);          
        }
      };
      converter.ConverterThread.RunWorkerCompleted += (sender, e) => {
        if (e.Cancelled) SwitchState(ProgramState.Reset);
        else {
          //progressConversion.Value = 100;
          //procentConversion.Text = "100 %";
          //TODO Converter.SaveFile();
          SwitchState(ProgramState.Done);
        }
      };*/

      // Always reset better?
      textfieldLink.Click += (sender, args) => {
        textfieldLink.SelectAll();
      };


      ReportProgress rp = delegate(int i) {
        Console.WriteLine(i+" %");
      };
      HandleResult hr = delegate(bool b, object o) {
        Console.WriteLine("Run " + (b ? "completed" : "aborted") + ", got: " + o.GetType().Name);
        if (o is bool) Console.WriteLine("Is bool: "+(bool)o);
      };

      var dummy = new ExtractionTask("testvideo.mp4", hr);
      dummy.Start();
    }


    private enum ProgramState { Reset, Working, Done }
    private void SwitchState(ProgramState state) {
      switch (state) {

        case ProgramState.Reset:
          panelWork.Enabled = false;
          ClearWorkPanel();
          textfieldLink.Text = "";
          panelSetup.Enabled = true;
          break;

        case ProgramState.Working:
          panelSetup.Enabled = false;
          panelWork.Enabled = true;
          break;


        case ProgramState.Done:
          panelSetup.Enabled = true;
          buttonAbort.Visible = false;
          labelDone.Visible = true;
          
          EventHandler handler = null;
          handler = (sender, args) => {
            textfieldLink.Enter -= handler;
            SwitchState(ProgramState.Reset);
          };
          textfieldLink.Enter += handler;

          buttonStart.Enabled = false;
          break;
      }
    }


    private void ClearWorkPanel() {
      textfieldTitle.Text = "";
      textfieldLength.Text = "";
      textfieldSize.Text = "";
      progressDownload.Value = 0;
      procentDownload.Text = "0 %";
      labelDone.Visible = false;
      buttonAbort.Visible = true;
      buttonAbort.Enabled = true;
    }


    /* Zustände:          | Adreßfeld+Start | Arbeitsf.| Abbrechen |  Fertig
     * 1) Programm-Start: | aktiv           | gesperrt | sichtbar  |  weg         
     * 2) Arbeit in Gange | gesperrt        | aktiv    | sichtbar  |  weg
     * 3) Arbeit fertig.  | aktiv           | aktiv    | weg       |  sichtbar
     * 4) Arbeit abgebr.  | aktiv           | gesperrt | sichtbar  |  weg
     */


    private void button1_Click(object sender, EventArgs e) {
      SwitchState(ProgramState.Working);
      //TODO _converter.LoadVideo(textfieldLink.Text);
    }


    public void SetVideoInformation(string[] info) {
      textfieldTitle.Text = info[0];
      textfieldLength.Text = info[1];
      textfieldSize.Text = info[2];
    }


    /** Enable "Start" button only if there's some text in the address field. */
    private void textfieldLink_TextChanged (object sender, EventArgs e) {
      buttonStart.Enabled = textfieldLink.Text != "";
    }

    /** Abort all actions. */
    private void buttonAbort_Click (object sender, EventArgs e) {
      //TODO if (_converter.DownloadThread.IsBusy) _converter.DownloadThread.CancelAsync();
      //TODO if (_converter.ConverterThread.IsBusy) _converter.ConverterThread.CancelAsync();
      buttonAbort.Enabled = false;
    }
  }
}



/*
{
  for (var j = 0; j <= 200; j++) {
    if (worker.CancellationPending) {e.Cancel = true; return;}
    var pow = Math.Pow(j, j);
    worker.ReportProgress(j*100 / 200);
    Thread.Sleep(1);
  }
  e.Result = 42;  
}

 
/// <summary>
///   Save a byte stream to a file.
/// </summary>
/// <param name="data">Bytes to write.</param>
/// <param name="name">Name of the file.</param>
public static void SaveFile(byte[] data, string name) {
  var stream = new FileStream(name, FileMode.Create);
  var writer = new BinaryWriter(stream);
  writer.Write(data);
  writer.Close();     
}
*/