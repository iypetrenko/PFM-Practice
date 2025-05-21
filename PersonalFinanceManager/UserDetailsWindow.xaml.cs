using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalFinanceManager.Model;
using PersonalFinanceManager.Repository;
using System.Windows;

namespace PersonalFinanceManager
{
    public partial class UserDetailsWindow : Window
    {
        private readonly User _user;
        private readonly UserRepository _repository = new UserRepository();

        public UserDetailsWindow(User user)
        {
            InitializeComponent();
            _user = user;
            LoadData();
        }

        private void LoadData()
        {
            txtId.Text = _user.Id.ToString();
            txtUsername.Text = _user.UserName;
            cmbRole.ItemsSource = Enum.GetValues(typeof(UserRole));
            cmbRole.SelectedItem = _user.Role;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            _user.UserName = txtUsername.Text;
            _user.Role = (UserRole)cmbRole.SelectedItem;

            if (_repository.UpdateUser(_user))
            {
                MessageBox.Show("Дані оновлено!");
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Помилка оновлення!");
            }
        }
    }
}
