﻿using Entity;
using Microsoft.EntityFrameworkCore;
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

        public event PropertyChangedEventHandler? PropertyChanged;
        public string Login { get => _login; set => _login = value; }
        public string Password { get => _password; set => _password = value; }

        public AuthorizationWindow(Context context, MainWindow mainWindow)
        {
            _context = context;
            _mainWindow = mainWindow;

            InitializeComponent();
        }

        #endregion

        private void LogIn(object sender, RoutedEventArgs e)
        {
            var user = _context.Users.AsNoTracking().FirstOrDefault(user => user.Login == _login && user.Password == _password);
            if(user != null)
            {
                _wrongPasswordInputs = 0;
                this.Hide();
                _mainWindow.Show();

                return;
            }

            _wrongPasswordInputs++;
            if(_wrongPasswordInputs == _attemptsAmount)
            {
                MessageBox.Show("Превышено допустимое количество попыток ввода пароля, выполняется выход из программы",
                    "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                Application.Current.Shutdown();

                return;
            }

            MessageBox.Show($"Осталось {_attemptsAmount - _wrongPasswordInputs} попыток ввода пароля",
                "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
