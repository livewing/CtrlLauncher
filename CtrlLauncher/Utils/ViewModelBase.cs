using Livet;
using Livet.EventListeners;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CtrlLauncher
{
    public abstract class ViewModelBase : ViewModel
    {
        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value)) return false;
            storage = value;
            RaisePropertyChanged(propertyName);
            return true;
        }
    }

    public abstract class ViewModelBase<TModel> : ViewModel where TModel : INotifyPropertyChanged
    {
        protected TModel Model { get; }

        public ViewModelBase(TModel model, bool addListener = true)
        {
            Model = model;
            if (addListener)
                CompositeDisposable.Add(new PropertyChangedEventListener(Model, (sender, e) => RaisePropertyChanged(e.PropertyName)));
        }

        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value)) return false;
            storage = value;
            RaisePropertyChanged(propertyName);
            return true;
        }
    }
}
