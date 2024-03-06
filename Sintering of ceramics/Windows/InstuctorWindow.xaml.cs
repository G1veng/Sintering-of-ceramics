using Entity;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Sintering_of_ceramics.Windows
{
    public partial class InstuctorWindow : Window, INotifyPropertyChanged
    {
        #region Private

        private readonly Context _context;

        private int _currentUserId;
        private ObservableCollection<Material> _materials;
        private ObservableCollection<Equipment> _equipments;
        private ObservableCollection<Qualities> _qualities;
        private ObservableCollection<User> _users;
        private Material _selectedMaterial;
        private Equipment _selectedEquipment;
        private Qualities _selectedQualities;
        private User _selectedUser;

        #endregion

        #region Properties

        public ObservableCollection<Material> Materials 
        { 
            get => _materials;
            set { _materials = value; NotifyPropertyChanged(nameof(Materials)); } 
        }
        public ObservableCollection<Equipment> Equipments
        {
            get => _equipments;
            set { _equipments = value; NotifyPropertyChanged(nameof(Equipments)); }
        }
        public ObservableCollection<Qualities> Qualities
        {
            get => _qualities;
            set { _qualities = value; NotifyPropertyChanged(nameof(Qualities)); }
        }
        public ObservableCollection<User> Users
        {
            get => _users;
            set { _users = value; NotifyPropertyChanged(nameof(Users)); }
        }
        public Material SelectedMaterial { get => _selectedMaterial; 
            set { _selectedMaterial = value; NotifyPropertyChanged(nameof(SelectedMaterial)); } }
        public Qualities SelectedQualities { get => _selectedQualities;
            set { _selectedQualities = value; NotifyPropertyChanged(nameof(SelectedQualities)); } }
        public Equipment SelectedEquipment { get => _selectedEquipment; 
            set { _selectedEquipment = value; NotifyPropertyChanged(nameof(SelectedEquipment)); } }
        public User SelectedUser { get => _selectedUser; set { _selectedUser = value; NotifyPropertyChanged(nameof(SelectedUser)); } }

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        public InstuctorWindow(Context context) 
        {
            _context = context;

            _users = new(_context.Users.AsNoTracking().ToList());
            _materials = new(_context.Materials.AsNoTracking().ToList());
            _qualities = new(_context.Qualities.AsNoTracking().ToList());
            _equipments = new(_context.Equipments.AsNoTracking().ToList());

            _selectedMaterial = _materials.FirstOrDefault() ?? new();
            _selectedEquipment = _equipments.FirstOrDefault() ?? new();
            _selectedUser = _users.FirstOrDefault() ?? new();
            _selectedQualities = _qualities.FirstOrDefault() ?? new();

            InitializeComponent();
        }

        public void InitializeWindow(int currentUserId)
        {
            _currentUserId = currentUserId;
        }

        private void CreateScript(object sender, RoutedEventArgs e)
        {
            _context.Scripts.Add(new Script
            {   
                InstructorId = _currentUserId,
                TraineeId = _selectedUser.Id,
                Status = "Что класть в статус?",
                Protocol = taskText.Text, //Положил текст задания сюда
                Task = new Task
                {
                    OvenTypeId = _selectedEquipment.Id,
                    MaterialId = _selectedMaterial.Id,
                    QualityId = _selectedQualities.Id,
                }
            });

            _context.SaveChanges();

            MessageBox.Show("Задание успешно создано",
                            "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);

            this.Hide();
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
