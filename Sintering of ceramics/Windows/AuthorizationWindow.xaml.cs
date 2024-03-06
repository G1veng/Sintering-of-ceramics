﻿using Entity;
using Microsoft.EntityFrameworkCore;
using Sintering_of_ceramics.Enums;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Sintering_of_ceramics
{
    public partial class AuthorizationWindow : Window
    {
        #region Private fields

        private readonly Context _context;
        private readonly MainWindow _mainWindow;
        private string _login = "";
        private string _password = "";
        private int _wrongPasswordInputs = 0;
        private readonly int _attemptsAmount = 5;

        #endregion


        #region Properties

        public string Login { get => _login; set => _login = value; }
        public string Password { get => _password; set => _password = value; }

        #endregion


        public AuthorizationWindow(Context context, MainWindow mainWindow)
        {
            _context = context;
            _mainWindow = mainWindow;

            InitializeComponent();
        }

        private void LogIn(object sender, RoutedEventArgs e)
        {
            var user = _context.Users.AsNoTracking().FirstOrDefault(user => user.Login == _login && user.Password == _password);
            if(user != null)
            {
                _wrongPasswordInputs = 0;
                this.Hide();
                _mainWindow.RoleId = user.RoleId;
                _mainWindow.IsAdmin = user.RoleId == (int?)UserRoleEnum.Administrator;
                _mainWindow.IsInstructor = user.RoleId == (int?)UserRoleEnum.Instructor;
                _mainWindow.UserId = user.Id;

                _mainWindow.Show();

                return;
            }

            _wrongPasswordInputs++;
            if(_wrongPasswordInputs == _attemptsAmount)
            {
                MessageBox.Show("Превышено допустимое количество попыток входа, выполняется выход из программы",
                    "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                Application.Current.Shutdown();

                return;
            }

            MessageBox.Show($"Осталось попыток для входа: {_attemptsAmount - _wrongPasswordInputs}",
                "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
