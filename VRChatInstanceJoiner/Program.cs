using System;
using System.Windows;

namespace VRChatInstanceJoiner
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            // Use the App class defined in App.xaml.cs
            var app = new App();
            app.Run();
        }
    }
}