using System;
using System.Windows.Controls;

namespace PersonalFinanceManager.Services
{
    public class NavigationService
    {
        private Frame _mainFrame;

        public void Initialize(Frame frame)
        {
            _mainFrame = frame;
        }

        public void NavigateTo<T>() where T : Page
        {
            var page = Activator.CreateInstance<T>();
            _mainFrame.Navigate(page);
        }
    }
}