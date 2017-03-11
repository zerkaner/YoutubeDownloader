using System;
using System.ComponentModel;
using System.Windows.Input;

namespace YouTubeDownloader.UserInterface {


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
}
