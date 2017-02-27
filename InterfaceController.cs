using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
// ReSharper disable MemberCanBePrivate.Global

namespace YouTubeDownloader {

  public class InterfaceController : ObservableObject {

    private readonly MainController _mc;

    /// <summary>
    ///   Create a new interface controller/VM, setting up initial values.
    /// </summary>
    /// <param name="mc">Main controller reference.</param>
    internal InterfaceController(MainController mc) {
      _mc = mc;
      SetupButtonCommands();
      VideoTitle = "Dropkick Murphys - I'm Shipping up to Boston 2";
      Debug.WriteLine("IFC CTOR called!");
    }

    public InterfaceController() {
      VideoTitle = "Test!";
    }

    public string VideoTitle { get; private set; }


    //_________________________________________________________________________
    // BUTTON CONTROLS AND SETUP.

    public DelegateCommand InsertClipboard { get; private set; }
    public DelegateCommand FetchVideoData  { get; private set; }
    public DelegateCommand DownloadAudio   { get; private set; }
    public DelegateCommand DownloadVideo   { get; private set; }
    public DelegateCommand CancelProcess   { get; private set; }
    public DelegateCommand OpenFile        { get; private set; }


    /// <summary>    
    ///   Initialize the button commands.
    /// </summary>
    private void SetupButtonCommands() {
      
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






  #region HelperClasses
  public class ObservableObject : INotifyPropertyChanged {
    public event PropertyChangedEventHandler PropertyChanged;
 
    protected void RaisePropertyChangedEvent(string propertyName = null) {
      var handler = PropertyChanged;
      handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }


  public class DelegateCommand : ICommand {

    private readonly Action _action;
    private readonly CanExecuteFcn _canExecute;
    public delegate bool CanExecuteFcn();
    public event EventHandler CanExecuteChanged;


    public DelegateCommand(Action action, CanExecuteFcn canExecute = null) {
      _action = action;
      if (canExecute == null) _canExecute = () => true;
      else _canExecute = canExecute;
    }


    public void Execute(object parameter) {
      _action();
    }


    public bool CanExecute(object parameter) {
      return _canExecute();
    }


    public void RaiseCanExecuteChanged() {
      CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
  }
  #endregion
}