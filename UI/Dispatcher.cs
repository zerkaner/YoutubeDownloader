using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using YouTubeDownloader.Workers;

namespace YouTubeDownloader.UI {


  internal class Dispatcher {

    private readonly AcquisitionControl _acquisitionControl;
    private readonly DownloadControl _downloadControl;
    private readonly CompressionControl _compressionControl;
    private readonly ExtractionControl _extractionControl;


    private readonly Control _form;
    private readonly Label _labelDone;
    private readonly Button _buttonStart;
    private readonly Button _buttonCancel;
    private readonly TextBox _inputLink;
    private readonly Panel _panelSetup;

    private enum ProgramState { Reset, Working, Done }
    private ProgramState _state;


    /// <summary>
    ///   Create a new UI dispatcher.
    /// </summary>
    /// <param name="form">GUI element.</param>
    public Dispatcher(Control form) {
      _acquisitionControl = new AcquisitionControl(this, form.Controls);
      _downloadControl = new DownloadControl(this, form.Controls);
      _compressionControl = new CompressionControl(this, form.Controls);
      _extractionControl = new ExtractionControl(this, form.Controls);

      _inputLink = (TextBox) form.Controls.Find("textfieldLink", true)[0];
      _buttonStart = (Button) form.Controls.Find("buttonStart", true)[0];
      _buttonCancel = (Button) form.Controls.Find("buttonAbort", true)[0];
      _labelDone = (Label) form.Controls.Find("labelDone", true)[0];
      _panelSetup = (Panel) form.Controls.Find("panelSetup", true)[0]; 

      _buttonStart.Click += (sender, args) => {
        if (Uri.IsWellFormedUriString(_inputLink.Text, UriKind.Absolute)) {
          if (_state == ProgramState.Done) SwitchState(ProgramState.Reset);
          _acquisitionControl.StartQuery(_inputLink.Text);
          SwitchState(ProgramState.Working);
        }

        // If address is invalid, set font color to red.
        else _inputLink.ForeColor = Color.Red;
                
      };

      // Set text color to black on any edit.
      _inputLink.TextChanged += (sender, args) => {
        _inputLink.ForeColor = Color.Black;
        _buttonStart.Enabled = _inputLink.Text != "";
      };
      
      // Always reset better?
      _inputLink.Click += (sender, args) => {
        _inputLink.SelectAll();
      };


      // Set up Drag 'n Drop.
      form.DragEnter += (sender, e) => {
        if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
      };
      form.DragDrop += (sender, e) => {
        var files = (string[]) e.Data.GetData(DataFormats.FileDrop);
        if (files.Length > 0) {  
          if (_state == ProgramState.Done) SwitchState(ProgramState.Reset);
          _extractionControl.StartExtraction(files[0]);
          SwitchState(ProgramState.Working);
        }
      };
      _form = form;

      SwitchState(ProgramState.Reset);
    }


    public void ReceiveResult(string task, bool success, object data) {

      if (!success) {
        Console.WriteLine("[Dispatcher] Error: Task '"+task+"' failed!");
        SwitchState(ProgramState.Done);
        return;
      }


      switch (task) {

        // 1st stage: Meta data received.
        case "Acquisition":
          _downloadControl.StartDownload((AcquisitionTask.VideoData) data);
          break;

        // 2nd stage: Video download finished.
        case "Download":
          var arr = (object[]) data;
          var title = (string) arr[0];
          var bytes = (byte[]) arr[1];
          var filename = "temp\\" + CheckForIllegalChars(title) + ".mp4";
          if (!Directory.Exists("temp")) Directory.CreateDirectory("temp");
          SaveFile(bytes, filename);
          _extractionControl.StartExtraction(filename);
          break;

        // 3rd stage: WAV extraction completed.
        case "Extraction":
          var wavFile = (string) data;
          _compressionControl.StartCompression(wavFile);
          break;

        // 4th stage: MP3 compression done.
        case "Compression":
          Console.WriteLine("MP3 download completed.");
          _labelDone.Visible = true;
          SwitchState(ProgramState.Done);
          break;
      }
    }


    /// <summary>
    ///   Switch the program state.
    /// </summary>
    /// <param name="state">The new state to switch to.</param>
    private void SwitchState(ProgramState state) {
      switch (state) {

        // Program at start: Reset all UI elements.
        case ProgramState.Reset:
          _acquisitionControl.Clear();
          _downloadControl.Clear();
          _extractionControl.Clear();
          _compressionControl.Clear();
          _labelDone.Visible = false;
          _buttonCancel.Visible = false;
          _panelSetup.Enabled = true;
          _form.AllowDrop = true;
          break;

        // Program running.
        case ProgramState.Working:
          _panelSetup.Enabled = false;
          _form.AllowDrop = false;
          _buttonCancel.Visible = true;
          break;

        // Program halted.
        case ProgramState.Done:
          _buttonCancel.Visible = false;
          _panelSetup.Enabled = true;
          _form.AllowDrop = true;
          EventHandler handler = null;
          handler = (sender, args) => {
            _inputLink.Enter -= handler;
            SwitchState(ProgramState.Reset);
          };
          _inputLink.Enter += handler;
          break;
      }
      _state = state;
    }


    /// <summary>
    ///   Eliminate illegal characters in the file name.
    /// </summary>
    /// <param name="filename">The filename to check.</param>
    /// <returns>Checked filename.</returns>
    private static string CheckForIllegalChars(string filename) {
      var invalid = new string(Path.GetInvalidFileNameChars());
      invalid += new string(Path.GetInvalidPathChars());
      foreach (char c in invalid) filename = filename.Replace(c.ToString(), "");
      return filename;
    }


    /// <summary>
    ///   Save a byte stream to a file.
    /// </summary>
    /// <param name="data">Bytes to write.</param>
    /// <param name="name">Name of the file.</param>
    private static void SaveFile(byte[] data, string name) {
      var stream = new FileStream(name, FileMode.Create);
      var writer = new BinaryWriter(stream);
      writer.Write(data);
      writer.Close();     
    }
  }
}
