using System;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace VRChatInstanceJoiner.ViewModels
{
    public class TestViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private string _inputText = "";
        private string _outputText = "Output will appear here";
        
        public string InputText
        {
            get => _inputText;
            set 
            {
                if (_inputText != value) 
                {
                    _inputText = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public string OutputText
        {
            get => _outputText;
            set 
            {
                if (_outputText != value) 
                {
                    _outputText = value;
                    OnPropertyChanged();
                }
            }
        }
        
        // Modified to match the RelayCommand signature which expects Action<object>
        public ICommand TestCommand => new RelayCommand(param => ExecuteTest());
        
        /// <summary>
        /// Test command execution
        /// </summary>
        private void ExecuteTest()
        {
            OutputText = $"You entered: {InputText}";
            // We'd add actual functionality here in a real implementation
            // such as calling a service or performing some operation
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}