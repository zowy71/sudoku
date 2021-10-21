using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CS_Sudoku.ViewModel
{
    public interface IRelayCommand : ICommand
    {
        void SetActive(bool p);
    }
    /// <summary>
    /// Classe simple implémentant l'interface ICommand
    /// Dans cette classe, les actions ne nécessitent pas de paramètres...
    /// </summary>
    public class RelayCommand : IRelayCommand
    {
        public event EventHandler CanExecuteChanged;

        private Action action;
        private Func<bool> _predicate;
        public Func<bool> Predicate
        {
            get => _predicate;
            private set
            {
                if (value != _predicate)
                {
                    _predicate = value;
                    this.CanExecuteChanged?.Invoke(this, new EventArgs());
                }
            }
        }

        public void SetActive(bool active)
        {
            if (active)
            {
                this.Predicate = DefaultPredicates.True;
            }
            else
            {
                this.Predicate = DefaultPredicates.False;
            }
        }

        public RelayCommand(Action action, Func<bool> predicate = null)
        {
            this.action = action;
            this.Predicate = predicate ?? DefaultPredicates.True;
        }

        public bool CanExecute(object parameter)
        {
            return Predicate();
        }

        public void Execute(object parameter)
        {
            action?.Invoke();
        }
    }

    public static class DefaultPredicates
    {
        public static bool True() { return true; }
        public static bool False() { return false; }
    }

    public static class DefaultPredicates<T>
    {
        public static bool True(T p) { return true; }
        public static bool False(T p) { return false; }
    }

    /// <summary>
    /// Classe simple implémentant l'interface ICommand
    /// Dans cette classe, les actions ne nécessitent pas de paramètres...
    /// </summary>
    public class RelayCommand<T> : IRelayCommand
    {
        public event EventHandler CanExecuteChanged;

        private Action<T> action;
        private Func<T, bool> _predicate;
        public Func<T, bool> Predicate
        {
            get => _predicate;
            private set
            {
                if (value != _predicate)
                {
                    _predicate = value;
                    this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public RelayCommand(Action<T> action, Func<T, bool> predicate = null)
        {
            this.action = action;
            if (predicate is null)
                this.Predicate = DefaultPredicates<T>.True;
            else
                this.Predicate = predicate;
        }

        public bool CanExecute(object parameter)
        {
            if (parameter is T p)
                return Predicate(p);
            throw new InvalidOperationException("Le paramètre n'est pas du bon type... ?");
        }

        public void Execute(object parameter)
        {
            if (parameter is T p)
                action?.Invoke(p);
            else
                throw new InvalidOperationException("Le paramètre n'est pas du bon type... ?");
        }

        public void SetActive(bool active)
        {
            if (active)
            {
                this.Predicate = DefaultPredicates<T>.True;
            }
            else
            {
                this.Predicate = DefaultPredicates<T>.False;
            }
        }

    }
}
