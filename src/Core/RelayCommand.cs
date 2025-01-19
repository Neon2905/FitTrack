using System;
using System.Windows.Input;

namespace FitTrack.Core
{
    /// <summary>
    /// Represents a command that can be executed.
    /// </summary>
    class RelayCommand : ICommand
    {
        private readonly Action execute;
        private readonly Func<object, bool> canExecute;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute">The action to execute when the command is invoked.</param>
        /// <param name="canExecute">A function to determine whether the command can be executed. If null, the command is always executable.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="execute"/> is null.</exception>
        public RelayCommand(Action execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Occurs when the ability of the command to execute has changed.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Determines whether the command can be executed.
        /// </summary>
        /// <param name="parameter">Data to pass to the command's execute method.</param>
        /// <returns>true if the command can be executed; otherwise, false.</returns>
        public bool CanExecute(object parameter) => this.canExecute == null || this.canExecute(parameter);

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">Data to pass to the command's execute method.</param>
        public void Execute(object parameter) => this.execute();
    }
}
