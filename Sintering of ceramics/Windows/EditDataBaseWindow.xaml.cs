using Entity;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Sintering_of_ceramics.Helpers;
using Sintering_of_ceramics.Models;
using Sintering_of_ceramics.Windows;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Sintering_of_ceramics
{
    public partial class EditDataBaseWindow : Window, INotifyPropertyChanged
    {
        #region Private

        private readonly Context _context;
        private readonly CreateEditDeleteWindow _createEditDeleteWindow;
        private delegate void NoArgDelegate();
        private string _defaultDescription = "Нету описания для свойства";
        private int _mathModelStepsAmount;
        private double _epsilon;
        private int _mathModelMaxDivisionAmount;

        #endregion

        #region Properties

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<User> Users { get; set; }

        public ObservableCollection<Material> Materials { get; set; }

        public ObservableCollection<TheoreticalMMParams> TheoreticalMMParams { get; set; }

        public User? SelectedUser { get; set; }

        public Material? SelectedMaterial { get; set; }

        public TheoreticalMMParams? SelectedTheoreticalMMParam {  get; set; }

        public int MathModelStepsAmount { get => _mathModelStepsAmount; 
            set 
            {
                _mathModelStepsAmount = value;

                Properties.Settings.Default.StepsAmount = value;
                Properties.Settings.Default.Save();

                NotifyPropertyChanged(nameof(MathModelStepsAmount)); 
            }
        }

        public double Epsilon
        {
            get => _epsilon;
            set
            {
                _epsilon = value;

                Properties.Settings.Default.Epsilon = value;
                Properties.Settings.Default.Save();

                NotifyPropertyChanged(nameof(Epsilon));
            }
        }

        public int MathModelMaxDivisionAmount
        {
            get => _mathModelMaxDivisionAmount;
            set
            {
                _mathModelMaxDivisionAmount = value;

                Properties.Settings.Default.MaxDivisionAmount = value;
                Properties.Settings.Default.Save();

                NotifyPropertyChanged(nameof(MathModelMaxDivisionAmount));
            }
        }

        #endregion


        public EditDataBaseWindow(Context context, CreateEditDeleteWindow createEditDeleteWindow)
        {
            _context = context;
            _createEditDeleteWindow = createEditDeleteWindow;
            MathModelStepsAmount = Properties.Settings.Default.StepsAmount;
            Epsilon = Properties.Settings.Default.Epsilon;

            Users = new ObservableCollection<User>(_context.Users.AsNoTracking().ToList());
            Materials = new ObservableCollection<Material>(_context.Materials
                .Include(m => m.TheoreticalMMParam)
                .AsNoTracking()
                .ToList());
            TheoreticalMMParams = new ObservableCollection<TheoreticalMMParams>(
                _context.TheoreticalMMParams.AsNoTracking().ToList());

            InitializeComponent();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void TextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Users

        private void CreateUser(object sender, RoutedEventArgs e)
        {
            User u = new();

            _createEditDeleteWindow.InitializeWindow("Добавление пользователя", Enums.WindowActionTypeEnum.Create,
                new ModelParamDTO()
                {
                    Description = ClassHelper.GetAttributeOfType<DescriptionAttribute>(typeof(User),
                            nameof(u.Login))?.Description ?? _defaultDescription,
                    SValue = "",
                    Name = nameof(u.Login)
                },
                new ModelParamDTO()
                {
                    Description = ClassHelper.GetAttributeOfType<DescriptionAttribute>(typeof(User),
                            nameof(u.Password))?.Description ?? _defaultDescription,
                    SValue = "",
                    Name = nameof(u.Password)
                },
                new ModelParamDTO()
                {
                    Description = ClassHelper.GetAttributeOfType<DescriptionAttribute>(typeof(User),
                            nameof(u.IsAdmin))?.Description ?? _defaultDescription,
                    BValue = false,
                    Name = nameof(u.IsAdmin)
                });

            _createEditDeleteWindow.ShowDialog();

            if (_createEditDeleteWindow.ResultValues.Count != 3)
                return;
            
            var user = new User()
            {
                IsAdmin = (bool)_createEditDeleteWindow.ResultValues[nameof(u.IsAdmin)],
                Login = (string)_createEditDeleteWindow.ResultValues[nameof(u.Login)],
                Password = (string)_createEditDeleteWindow.ResultValues[nameof(u.Password)]
            };

            _context.Users.Add(user);

            _context.SaveChanges();
            Users.Add(user);

            usersGrid.ItemsSource = null;
            usersGrid.ItemsSource = Users;            
        }

        private void DeleteUser(object sender, RoutedEventArgs e)
        {
            var selectedUsers = usersGrid.SelectedItems.Cast<User>().ToList();

            _context.Users.RemoveRange(selectedUsers);
            _context.SaveChanges();

            Users = new ObservableCollection<User>(Users.Except(selectedUsers));

            usersGrid.ItemsSource = null;
            usersGrid.ItemsSource = Users;
        }

        private void EditUser(object sender, RoutedEventArgs e)
        {
            if (SelectedUser == null)
                return;

            User u = new();

            _createEditDeleteWindow.InitializeWindow("Редактирование пользователя", Enums.WindowActionTypeEnum.Edit,
                new ModelParamDTO()
                {
                    Description = ClassHelper.GetAttributeOfType<DescriptionAttribute>(typeof(User),
                            nameof(u.Login))?.Description ?? _defaultDescription,
                    SValue = SelectedUser.Login,
                    Name = nameof(u.Login)
                },
                new ModelParamDTO()
                {
                    Description = ClassHelper.GetAttributeOfType<DescriptionAttribute>(typeof(User),
                            nameof(u.Password))?.Description ?? _defaultDescription,
                    SValue = SelectedUser.Password,
                    Name = nameof(u.Password)
                },
                new ModelParamDTO()
                {
                    Description = ClassHelper.GetAttributeOfType<DescriptionAttribute>(typeof(User),
                            nameof(u.IsAdmin))?.Description ?? _defaultDescription,
                    BValue = SelectedUser.IsAdmin,
                    Name = nameof(u.IsAdmin)
                });

            _createEditDeleteWindow.ShowDialog();

            if (_createEditDeleteWindow.ResultValues.Count != 3)
                return;
            
            var user = _context.Users.FirstOrDefault(user => user.Id == SelectedUser.Id);
            if (user != null)
            {
                user.Login = (string)_createEditDeleteWindow.ResultValues[nameof(u.Login)];
                user.Password = (string)_createEditDeleteWindow.ResultValues[nameof(u.Password)];
                user.IsAdmin = (bool)_createEditDeleteWindow.ResultValues[nameof(u.IsAdmin)];
            }

            _context.SaveChanges();

            foreach(var arrUser in Users)
            {
                if (arrUser.Id != SelectedUser.Id)
                    continue;

                arrUser.Login = (string)_createEditDeleteWindow.ResultValues[nameof(u.Login)];
                arrUser.Password = (string)_createEditDeleteWindow.ResultValues[nameof(u.Password)];
                arrUser.IsAdmin = (bool)_createEditDeleteWindow.ResultValues[nameof(u.IsAdmin)];
            }

            usersGrid.ItemsSource = null;
            usersGrid.ItemsSource = Users;
        }

        #endregion

        #region TheoreticalMMParams

        private void CreateTheoreticalMMParam(object sender, RoutedEventArgs e)
        {
            TheoreticalMMParams param = new();
            Material m = new();

            _createEditDeleteWindow.InitializeWindow("Добавление параметра теоретической математической модели", Enums.WindowActionTypeEnum.Create,
                new ModelParamDTO()
                {
                    Description = 
                        ClassHelper.GetAttributeOfType<DescriptionAttribute>(typeof(TheoreticalMMParams),
                            nameof(param.PreExponentialFactorOfGraindBoundaryDiffusionCoefficient))?.Description ?? _defaultDescription,
                    DValue = 0,
                    Name = nameof(param.PreExponentialFactorOfGraindBoundaryDiffusionCoefficient)
                },
                new ModelParamDTO()
                {
                    Description =
                        ClassHelper.GetAttributeOfType<DescriptionAttribute>(typeof(TheoreticalMMParams),
                            nameof(param.PreExponentialFactorOfSurfaceSelfCoefficient))?.Description ?? _defaultDescription,
                    DValue = 0,
                    Name = nameof(param.PreExponentialFactorOfSurfaceSelfCoefficient)
                },
                new ModelParamDTO()
                {
                    Description =
                        ClassHelper.GetAttributeOfType<DescriptionAttribute>(typeof(TheoreticalMMParams),
                            nameof(param.GrainBoundaryDiffusionActivationEnergy))?.Description ?? _defaultDescription,
                    DValue = 0,
                    Name = nameof(param.GrainBoundaryDiffusionActivationEnergy)
                },
                new ModelParamDTO()
                {
                    Description =
                        ClassHelper.GetAttributeOfType<DescriptionAttribute>(typeof(TheoreticalMMParams),
                            nameof(param.SurfaceSelfDiffusionActivationEnergy))?.Description ?? _defaultDescription,
                    DValue = 0,
                    Name = nameof(param.SurfaceSelfDiffusionActivationEnergy)
                },
                new ModelParamDTO()
                {
                    Description =
                        ClassHelper.GetAttributeOfType<DescriptionAttribute>(typeof(TheoreticalMMParams),
                            nameof(param.MaterialId))?.Description ?? _defaultDescription,
                    LValues = Materials.Cast<object>().ToList(),
                    Name = nameof(param.MaterialId),
                    DisplayMemberPath = nameof(m.Name)
                }
            );

            _createEditDeleteWindow.ShowDialog();

            if (_createEditDeleteWindow.ResultValues.Count != 5)
                return;
            
            var theoreticalMMParam = new TheoreticalMMParams()
            {
                GrainBoundaryDiffusionActivationEnergy = 
                    Double.Parse((string)_createEditDeleteWindow.ResultValues[nameof(param.GrainBoundaryDiffusionActivationEnergy)]),
                PreExponentialFactorOfSurfaceSelfCoefficient =
                    Double.Parse((string)_createEditDeleteWindow.ResultValues[nameof(param.PreExponentialFactorOfSurfaceSelfCoefficient)]),
                PreExponentialFactorOfGraindBoundaryDiffusionCoefficient =
                    Double.Parse((string)_createEditDeleteWindow.ResultValues[nameof(param.PreExponentialFactorOfGraindBoundaryDiffusionCoefficient)]),
                SurfaceSelfDiffusionActivationEnergy =
                    Double.Parse((string)_createEditDeleteWindow.ResultValues[nameof(param.SurfaceSelfDiffusionActivationEnergy)]),
                MaterialId = Materials[(int)_createEditDeleteWindow.ResultValues[nameof(param.MaterialId)]].Id
            };

            if(_context.TheoreticalMMParams.AsNoTracking().FirstOrDefault(p => p.MaterialId == theoreticalMMParam.MaterialId) != null)
            {
                MessageBox.Show($"У выбранного материала уже есть параметры теоретической математической модели",
                    "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            _context.TheoreticalMMParams.Add(theoreticalMMParam);

            _context.SaveChanges();
            TheoreticalMMParams.Add(theoreticalMMParam);

            theoreticalMMParamsGrid.ItemsSource = null;
            theoreticalMMParamsGrid.ItemsSource = TheoreticalMMParams;            
        }

        private void DeleteTheoreticalMMParam(object sender, RoutedEventArgs e)
        {
            var selectedTheoreticalMMParams = theoreticalMMParamsGrid.SelectedItems.Cast<TheoreticalMMParams>().ToList();

            _context.TheoreticalMMParams.RemoveRange(selectedTheoreticalMMParams);
            _context.SaveChanges();

            TheoreticalMMParams = new ObservableCollection<TheoreticalMMParams>(TheoreticalMMParams.Except(selectedTheoreticalMMParams));

            theoreticalMMParamsGrid.ItemsSource = null;
            theoreticalMMParamsGrid.ItemsSource = TheoreticalMMParams;
        }

        private void EditTheoreticalMMParam(object sender, RoutedEventArgs e)
        {
            if (SelectedTheoreticalMMParam == null)
                return;

            TheoreticalMMParams param = new();
            Material m = new();

            var selectedIndex = 0;
            for(int i = 0; i < Materials.Count; i++)
            {
                if (Materials[i].TheoreticalMMParam.Id != SelectedTheoreticalMMParam.Id)
                    continue;

                selectedIndex = i;
                break;
            }

            _createEditDeleteWindow.InitializeWindow("Редактирование параметра теоретической математической модели", Enums.WindowActionTypeEnum.Edit,
                new ModelParamDTO()
                {
                    Description =
                        ClassHelper.GetAttributeOfType<DescriptionAttribute>(typeof(TheoreticalMMParams),
                            nameof(param.PreExponentialFactorOfGraindBoundaryDiffusionCoefficient))?.Description ?? _defaultDescription,
                    DValue = SelectedTheoreticalMMParam.PreExponentialFactorOfGraindBoundaryDiffusionCoefficient,
                    Name = nameof(param.PreExponentialFactorOfGraindBoundaryDiffusionCoefficient)
                },
                new ModelParamDTO()
                {
                    Description =
                        ClassHelper.GetAttributeOfType<DescriptionAttribute>(typeof(TheoreticalMMParams),
                            nameof(param.PreExponentialFactorOfSurfaceSelfCoefficient))?.Description ?? _defaultDescription,
                    DValue = SelectedTheoreticalMMParam.PreExponentialFactorOfSurfaceSelfCoefficient,
                    Name = nameof(param.PreExponentialFactorOfSurfaceSelfCoefficient)
                },
                new ModelParamDTO()
                {
                    Description =
                        ClassHelper.GetAttributeOfType<DescriptionAttribute>(typeof(TheoreticalMMParams),
                            nameof(param.GrainBoundaryDiffusionActivationEnergy))?.Description ?? _defaultDescription,
                    DValue = SelectedTheoreticalMMParam.GrainBoundaryDiffusionActivationEnergy,
                    Name = nameof(param.GrainBoundaryDiffusionActivationEnergy)
                },
                new ModelParamDTO()
                {
                    Description =
                        ClassHelper.GetAttributeOfType<DescriptionAttribute>(typeof(TheoreticalMMParams),
                            nameof(param.SurfaceSelfDiffusionActivationEnergy))?.Description ?? _defaultDescription,
                    DValue = SelectedTheoreticalMMParam.SurfaceSelfDiffusionActivationEnergy,
                    Name = nameof(param.SurfaceSelfDiffusionActivationEnergy)
                },
                new ModelParamDTO()
                {
                    Description =
                        ClassHelper.GetAttributeOfType<DescriptionAttribute>(typeof(TheoreticalMMParams),
                            nameof(param.MaterialId))?.Description ?? _defaultDescription,
                    LValues = Materials.Cast<object>().ToList(),
                    Name = nameof(param.MaterialId),
                    DisplayMemberPath = nameof(m.Name),
                    SelectedIndex = selectedIndex
                }
            );

            _createEditDeleteWindow.ShowDialog();

            if (_createEditDeleteWindow.ResultValues.Count != 5)
                return;
            
            var theoreticalMMParam = _context.TheoreticalMMParams.FirstOrDefault(param => param.Id == SelectedTheoreticalMMParam.Id);
            if (theoreticalMMParam != null)
            {
                theoreticalMMParam.GrainBoundaryDiffusionActivationEnergy = 
                    Double.Parse((string)_createEditDeleteWindow.ResultValues[nameof(param.GrainBoundaryDiffusionActivationEnergy)]);
                theoreticalMMParam.PreExponentialFactorOfSurfaceSelfCoefficient =
                    Double.Parse((string)_createEditDeleteWindow.ResultValues[nameof(param.PreExponentialFactorOfSurfaceSelfCoefficient)]);
                theoreticalMMParam.PreExponentialFactorOfGraindBoundaryDiffusionCoefficient =
                    Double.Parse((string)_createEditDeleteWindow.ResultValues[nameof(param.PreExponentialFactorOfGraindBoundaryDiffusionCoefficient)]);
                theoreticalMMParam.SurfaceSelfDiffusionActivationEnergy =
                    Double.Parse((string)_createEditDeleteWindow.ResultValues[nameof(param.SurfaceSelfDiffusionActivationEnergy)]);
                theoreticalMMParam.MaterialId = Materials[(int)_createEditDeleteWindow.ResultValues[nameof(param.MaterialId)]].Id;
            }

            _context.SaveChanges();

            foreach (var arrTheoreticalMMParam in TheoreticalMMParams)
            {
                if (arrTheoreticalMMParam.Id != SelectedTheoreticalMMParam.Id)
                    continue;

                arrTheoreticalMMParam.GrainBoundaryDiffusionActivationEnergy =
                    Double.Parse((string)_createEditDeleteWindow.ResultValues[nameof(param.GrainBoundaryDiffusionActivationEnergy)]);
                arrTheoreticalMMParam.PreExponentialFactorOfSurfaceSelfCoefficient =
                    Double.Parse((string)_createEditDeleteWindow.ResultValues[nameof(param.PreExponentialFactorOfSurfaceSelfCoefficient)]);
                arrTheoreticalMMParam.PreExponentialFactorOfGraindBoundaryDiffusionCoefficient =
                    Double.Parse((string)_createEditDeleteWindow.ResultValues[nameof(param.PreExponentialFactorOfGraindBoundaryDiffusionCoefficient)]);
                arrTheoreticalMMParam.SurfaceSelfDiffusionActivationEnergy =
                    Double.Parse((string)_createEditDeleteWindow.ResultValues[nameof(param.SurfaceSelfDiffusionActivationEnergy)]);
                arrTheoreticalMMParam.MaterialId = Materials[(int)_createEditDeleteWindow.ResultValues[nameof(param.MaterialId)]].Id;
            }

            theoreticalMMParamsGrid.ItemsSource = null;
            theoreticalMMParamsGrid.ItemsSource = TheoreticalMMParams;            
        }

        #endregion

        #region Materials

        private void CreateMaterial(object sender, RoutedEventArgs e)
        {
            Material m = new();

            _createEditDeleteWindow.InitializeWindow("Добавление материала", Enums.WindowActionTypeEnum.Create,
                new ModelParamDTO() 
                {
                    Description = 
                        ClassHelper.GetAttributeOfType<DescriptionAttribute>(typeof(Material), nameof(m.Porosity))?.Description ?? _defaultDescription,
                    DValue = 0,
                    Name = nameof(m.Porosity) 
                },
                new ModelParamDTO()
                {
                    Description =
                        ClassHelper.GetAttributeOfType<DescriptionAttribute>(typeof(Material), nameof(m.AvarageGrainSize))?.Description ?? _defaultDescription,
                    DValue = 0,
                    Name = nameof(m.AvarageGrainSize)
                },
                new ModelParamDTO()
                {
                    Description =
                        ClassHelper.GetAttributeOfType<DescriptionAttribute>(typeof(Material), nameof(m.SurfaceLayerThickness))?.Description ?? _defaultDescription,
                    DValue = 0,
                    Name = nameof(m.SurfaceLayerThickness)
                },
                new ModelParamDTO()
                {
                    Description =
                        ClassHelper.GetAttributeOfType<DescriptionAttribute>(typeof(Material), nameof(m.SpecificSurfaceEnergy))?.Description ?? _defaultDescription,
                    DValue = 0,
                    Name = nameof(m.SpecificSurfaceEnergy)
                },
                new ModelParamDTO()
                {
                    Description =
                        ClassHelper.GetAttributeOfType<DescriptionAttribute>(typeof(Material), nameof(m.CompactMaterialDensity))?.Description ?? _defaultDescription,
                    DValue = 0,
                    Name = nameof(m.CompactMaterialDensity)
                },
                new ModelParamDTO()
                {
                    Description =
                        ClassHelper.GetAttributeOfType<DescriptionAttribute>(typeof(Material), nameof(m.CompactMaterialViscosity))?.Description ?? _defaultDescription,
                    DValue = 0,
                    Name = nameof(m.CompactMaterialViscosity)
                },
                new ModelParamDTO()
                {
                    Description =
                        ClassHelper.GetAttributeOfType<DescriptionAttribute>(typeof(Material), nameof(m.Weight))?.Description ?? _defaultDescription,
                    DValue = 0,
                    Name = nameof(m.Weight)
                },
                new ModelParamDTO()
                {
                    Description =
                        ClassHelper.GetAttributeOfType<DescriptionAttribute>(typeof(Material), nameof(m.Name))?.Description ?? _defaultDescription,
                    SValue = "",
                    Name = nameof(m.Name)
                });

            _createEditDeleteWindow.ShowDialog();

            if (_createEditDeleteWindow.ResultValues.Count != 8)
                return;
            
            var material = new Material()
            {
                MaterialType = 1,
                AvarageGrainSize = Double.Parse((string)_createEditDeleteWindow.ResultValues[nameof(m.AvarageGrainSize)]),
                CompactMaterialViscosity = Double.Parse((string)_createEditDeleteWindow.ResultValues[nameof(m.CompactMaterialViscosity)]),
                CompactMaterialDensity = Double.Parse((string)_createEditDeleteWindow.ResultValues[nameof(m.CompactMaterialDensity)]),
                Name = (string)_createEditDeleteWindow.ResultValues[nameof(m.Name)],
                Porosity = Double.Parse((string)_createEditDeleteWindow.ResultValues[nameof(m.Porosity)]),
                SpecificSurfaceEnergy = Double.Parse((string)_createEditDeleteWindow.ResultValues[nameof(m.SpecificSurfaceEnergy)]),
                SurfaceLayerThickness = Double.Parse((string)_createEditDeleteWindow.ResultValues[nameof(m.SurfaceLayerThickness)]),
                Weight = Double.Parse((string)_createEditDeleteWindow.ResultValues[nameof(m.Weight)])
            };

            _context.Materials.Add(material);

            _context.SaveChanges();
            Materials.Add(material);

            materialGrid.ItemsSource = null;
            materialGrid.ItemsSource = Materials;
            
        }

        private void DeleteMaterial(object sender, RoutedEventArgs e)
        {
            var selectedMaterials = materialGrid.SelectedItems.Cast<Material>().ToList();

            _context.Materials.RemoveRange(selectedMaterials);
            _context.SaveChanges();

            Materials = new ObservableCollection<Material>(Materials.Except(selectedMaterials));

            materialGrid.ItemsSource = null;
            materialGrid.ItemsSource = Materials;

            theoreticalMMParamsGrid.ItemsSource = null;
            TheoreticalMMParams = new ObservableCollection<TheoreticalMMParams>(_context.TheoreticalMMParams.AsNoTracking().ToList());
            theoreticalMMParamsGrid.ItemsSource = TheoreticalMMParams;
        }

        private void EditMaterial(object sender, RoutedEventArgs e)
        {
            if (SelectedMaterial == null)
                return;

            Material m = new();

            _createEditDeleteWindow.InitializeWindow("Редактирование материала", Enums.WindowActionTypeEnum.Edit,
                new ModelParamDTO()
                {
                    Description =
                        ClassHelper.GetAttributeOfType<DescriptionAttribute>(typeof(Material), nameof(m.Porosity))?.Description ?? _defaultDescription,
                    DValue = SelectedMaterial.Porosity,
                    Name = nameof(m.Porosity)
                },
                new ModelParamDTO()
                {
                    Description =
                        ClassHelper.GetAttributeOfType<DescriptionAttribute>(typeof(Material), nameof(m.AvarageGrainSize))?.Description ?? _defaultDescription,
                    DValue = SelectedMaterial.AvarageGrainSize,
                    Name = nameof(m.AvarageGrainSize)
                },
                new ModelParamDTO()
                {
                    Description =
                        ClassHelper.GetAttributeOfType<DescriptionAttribute>(typeof(Material), nameof(m.SurfaceLayerThickness))?.Description ?? _defaultDescription,
                    DValue = SelectedMaterial.SurfaceLayerThickness,
                    Name = nameof(m.SurfaceLayerThickness)
                },
                new ModelParamDTO()
                {
                    Description =
                        ClassHelper.GetAttributeOfType<DescriptionAttribute>(typeof(Material), nameof(m.SpecificSurfaceEnergy))?.Description ?? _defaultDescription,
                    DValue = SelectedMaterial.SpecificSurfaceEnergy,
                    Name = nameof(m.SpecificSurfaceEnergy)
                },
                new ModelParamDTO()
                {
                    Description =
                        ClassHelper.GetAttributeOfType<DescriptionAttribute>(typeof(Material), nameof(m.CompactMaterialDensity))?.Description ?? _defaultDescription,
                    DValue = SelectedMaterial.CompactMaterialDensity,
                    Name = nameof(m.CompactMaterialDensity)
                },
                new ModelParamDTO()
                {
                    Description =
                        ClassHelper.GetAttributeOfType<DescriptionAttribute>(typeof(Material), nameof(m.CompactMaterialViscosity))?.Description ?? _defaultDescription,
                    DValue = SelectedMaterial.CompactMaterialViscosity,
                    Name = nameof(m.CompactMaterialViscosity)
                },
                new ModelParamDTO()
                {
                    Description =
                        ClassHelper.GetAttributeOfType<DescriptionAttribute>(typeof(Material), nameof(m.Weight))?.Description ?? _defaultDescription,
                    DValue = SelectedMaterial.Weight,
                    Name = nameof(m.Weight)
                },
                new ModelParamDTO()
                {
                    Description =
                        ClassHelper.GetAttributeOfType<DescriptionAttribute>(typeof(Material), nameof(m.Name))?.Description ?? _defaultDescription,
                    SValue = SelectedMaterial.Name,
                    Name = nameof(m.Name)
                });

            _createEditDeleteWindow.ShowDialog();

            if (_createEditDeleteWindow.ResultValues.Count != 8)
                return;

            var material = _context.Materials.FirstOrDefault(m => m.Id == SelectedMaterial.Id);
            if (material != null)
            {
                material.MaterialType = 1;
                material.AvarageGrainSize = Double.Parse((string)_createEditDeleteWindow.ResultValues[nameof(m.AvarageGrainSize)]);
                material.CompactMaterialViscosity = Double.Parse((string)_createEditDeleteWindow.ResultValues[nameof(m.CompactMaterialViscosity)]);
                material.CompactMaterialDensity = Double.Parse((string)_createEditDeleteWindow.ResultValues[nameof(m.CompactMaterialDensity)]);
                material.Name = (string)_createEditDeleteWindow.ResultValues[nameof(m.Name)];
                material.Porosity = Double.Parse((string)_createEditDeleteWindow.ResultValues[nameof(m.Porosity)]);
                material.SpecificSurfaceEnergy = Double.Parse((string)_createEditDeleteWindow.ResultValues[nameof(m.SpecificSurfaceEnergy)]);
                material.SurfaceLayerThickness = Double.Parse((string)_createEditDeleteWindow.ResultValues[nameof(m.SurfaceLayerThickness)]);
                material.Weight = Double.Parse((string)_createEditDeleteWindow.ResultValues[nameof(m.Weight)]);
            }

            _context.SaveChanges();

            foreach (var arrMaterial in Materials)
            {
                if (arrMaterial.Id != SelectedMaterial.Id)
                    continue;

                arrMaterial.MaterialType = 1;
                arrMaterial.AvarageGrainSize = Double.Parse((string)_createEditDeleteWindow.ResultValues[nameof(m.AvarageGrainSize)]);
                arrMaterial.CompactMaterialViscosity = Double.Parse((string)_createEditDeleteWindow.ResultValues[nameof(m.CompactMaterialViscosity)]);
                arrMaterial.CompactMaterialDensity = Double.Parse((string)_createEditDeleteWindow.ResultValues[nameof(m.CompactMaterialDensity)]);
                arrMaterial.Name = (string)_createEditDeleteWindow.ResultValues[nameof(m.Name)];
                arrMaterial.Porosity = Double.Parse((string)_createEditDeleteWindow.ResultValues[nameof(m.Porosity)]);
                arrMaterial.SpecificSurfaceEnergy = Double.Parse((string)_createEditDeleteWindow.ResultValues[nameof(m.SpecificSurfaceEnergy)]);
                arrMaterial.SurfaceLayerThickness = Double.Parse((string)_createEditDeleteWindow.ResultValues[nameof(m.SurfaceLayerThickness)]);
                arrMaterial.Weight = Double.Parse((string)_createEditDeleteWindow.ResultValues[nameof(m.Weight)]);
            }

            materialGrid.ItemsSource = null;
            materialGrid.ItemsSource = Materials;            
        }

        #endregion


    }
}
