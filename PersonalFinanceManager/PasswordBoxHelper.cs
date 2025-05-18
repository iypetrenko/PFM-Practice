using System.Windows;
using System.Windows.Controls;

namespace PersonalFinanceManager.Helpers
{
    public static class PasswordBoxHelper
    {
        public static readonly DependencyProperty BoundPassword =
            DependencyProperty.RegisterAttached("BoundPassword", typeof(string), typeof(PasswordBoxHelper),
                new PropertyMetadata(string.Empty, OnBoundPasswordChanged));

        public static readonly DependencyProperty BindPassword =
            DependencyProperty.RegisterAttached("BindPassword", typeof(bool), typeof(PasswordBoxHelper),
                new PropertyMetadata(false, OnBindPasswordChanged));

        private static readonly DependencyProperty UpdatingPassword =
            DependencyProperty.RegisterAttached("UpdatingPassword", typeof(bool), typeof(PasswordBoxHelper), new PropertyMetadata(false));

        public static string GetBoundPassword(DependencyObject dp) => (string)dp.GetValue(BoundPassword);
        public static void SetBoundPassword(DependencyObject dp, string value) => dp.SetValue(BoundPassword, value);

        public static bool GetBindPassword(DependencyObject dp) => (bool)dp.GetValue(BindPassword);
        public static void SetBindPassword(DependencyObject dp, bool value) => dp.SetValue(BindPassword, value);

        private static bool GetUpdatingPassword(DependencyObject dp) => (bool)dp.GetValue(UpdatingPassword);
        private static void SetUpdatingPassword(DependencyObject dp, bool value) => dp.SetValue(UpdatingPassword, value);

        private static void OnBoundPasswordChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            if (dp is PasswordBox pb && !GetUpdatingPassword(pb))
            {
                pb.Password = e.NewValue?.ToString() ?? string.Empty;
            }
        }

        private static void OnBindPasswordChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            if (dp is PasswordBox pb)
            {
                if ((bool)e.NewValue)
                {
                    pb.PasswordChanged += (s, ev) =>
                    {
                        SetUpdatingPassword(pb, true);
                        SetBoundPassword(pb, pb.Password);
                        SetUpdatingPassword(pb, false);
                    };
                }
            }
        }
    }
}
