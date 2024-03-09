using Entity;
using Entity.Models;
using Mathematics;
using Mathematics.Models;
using Microsoft.EntityFrameworkCore;
using Sintering_of_ceramics.Enums;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
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

        private void GenerateProtocolClick(object sender, RoutedEventArgs e)
        {
            if (_selectedUser == null)
            {
                MessageBox.Show("Выберите пользователя, для которого необходимо сформировать протокол",
                            "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            MaterialCharacteristicsDTO? result = null;
            var model = new Sintering(
                t0: 20,
                tk: 1450,
                l0: 1 / 1000000,
                p0: 40,
                tau1: 70 * 60,
                d: 0.1 * 0.000000001,
                db0: 0.35,
                ds0: 0.4,
                eb: 181 * 1000,
                es: 245 * 1000,
                s: 3.5,
                eta0: 170 * 1000000,
                pg: 6 * 1000000,
                m: 0.1,
                ro0: 14600,
                tau2: 30 * 60);

            try
            {
                result = model.Calculate(false, 1, 1, 10);
            }
            catch
            {
                MessageBox.Show("Возникла непредвиденная ошибка при расчете", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (result == null)
            {
                MessageBox.Show("Не найдено устойчивого решения", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var resultPorosity = Math.Round(result.PP, 2);
            var resultDensity = Math.Round(result.Ro, 0);
            var resultViscosity = Math.Round(result.Ett, 2);
            var resultAvarageGrainSize = Math.Round(result.LL, 2);

            var temperaturePlot = model.GetTemperatureChartValues();
            var densityPlot = model.GetDensityChartValues();
            var porosityPlot = model.GetPorosityChartValues();
            var grainSizePlot = model.GetGrainSizeChartValues();

            var logs = new List<string> { "Изменил начальную температуру c 50 на 100", "Изменил давление инертного газа с 5 на 6", "Изменил время изотермической выдержики с 30 на 50"};
            Workbook wb = new Workbook();
            wb.Worksheets.Clear();
            var sheet = wb.Worksheets.Add("Протокол");

            sheet.Range["A1"].Value = "Пользователь";
            sheet.Range["B1"].Value = _selectedUser.Login;

            sheet.Range["A2"].Value = "Прошел обучение?";
            sheet.Range["B2"].Value = "Да";

            DataTable dt = new DataTable();
            dt.Columns.Add("Действие");

            foreach (var log in logs)
            {
                dt.Rows.Add(log);
            }

            sheet.InsertDataTable(dt, true, 3, 1);

            sheet.Range[$"A{2 + logs.Count}"].Value = "Результаты моделирования";

            sheet.Range[$"A{3 + logs.Count}"].Value = "Пористость, %";
            sheet.Range[$"B{3 + logs.Count}"].Value = resultPorosity.ToString();

            sheet.Range[$"A{4 + logs.Count}"].Value = "Конечный средний диаметр зерна, мкм";
            sheet.Range[$"B{4 + logs.Count}"].Value = resultAvarageGrainSize.ToString();

            sheet.Range[$"A{4 + logs.Count}"].Value = "Конечная плотность материала, кг/м³";
            sheet.Range[$"B{4 + logs.Count}"].Value = resultDensity.ToString();

            var (bendingStrengthE, hardnessE, porosityE, densityE) = CalculateImpericalModels(1450, 30, 6);

            if (bendingStrengthE.HasValue)
            {
                sheet.Range[$"A{5 + logs.Count}"].Value = "Плотность, кг/см²";
                sheet.Range[$"B{5 + logs.Count}"].Value = bendingStrengthE.Value.ToString();
            }
            if (hardnessE.HasValue)
            {
                sheet.Range[$"A{6 + logs.Count}"].Value = "Прочность при поперечном изгибе, МПа";
                sheet.Range[$"B{6 + logs.Count}"].Value = hardnessE.Value.ToString();
            }
            if (porosityE.HasValue)
            {
                sheet.Range[$"A{7 + logs.Count}"].Value = "Остаточная пористость, %";
                sheet.Range[$"B{7 + logs.Count}"].Value = porosityE.Value.ToString();
            }
            if (densityE.HasValue)
            {
                sheet.Range[$"A{8 + logs.Count}"].Value = "Твердость сплава, кг/см²";
                sheet.Range[$"B{8 + logs.Count}"].Value = densityE.Value.ToString();
            }

            var chartSheet = wb.Worksheets.Add("Графики");
            Chart chart = chartSheet.Charts.Add(ExcelChartType.ScatterLine);
            var series = chart.Series.Add();
            series.EnteredDirectlyValues = new object[] { temperaturePlot.Select(x => Math.Round(x.Value, 2)).ToArray() };
            series.EnteredDirectlyCategoryLabels = new object[] { temperaturePlot.Select(x => x.Key).ToArray() };

            wb.SaveToFile("test.xlsx");
        }

        private (double?, double?, double?, double?) CalculateImpericalModels(double finalTemperatureInFurnace, double excerptTime, double pressure)
        {
            var empiricalModels = _context.EmpiricalModels.AsNoTracking()
                .Include(x => x.ParamsRanges)
                .Include(x => x.EmpiricalModelCoeffs)
                .Where(x => x.MaterialId == _selectedMaterial.Id)
                .ToList();

            if (!empiricalModels.Any())
                return (null, null, null, null);

            double? bendingStrengthE = null, hardnessE = null, porosityE = null, densityE = null;

            empiricalModels.ForEach(model =>
            {
                int satisfiesAmount = 0;

                model.ParamsRanges.ForEach(param =>
                {
                    if (param.UnitId == (int)EmpiricalModelUnitTypeEnum.Temperature &&
                        param.MaxValue >= finalTemperatureInFurnace &&
                        param.MinValue <= finalTemperatureInFurnace)
                        satisfiesAmount++;

                    if (param.UnitId == (int)EmpiricalModelUnitTypeEnum.Time &&
                        param.MaxValue >= excerptTime &&
                        param.MinValue <= excerptTime)
                        satisfiesAmount++;

                    if (param.UnitId == (int)EmpiricalModelUnitTypeEnum.Atmosphere &&
                        param.MaxValue >= pressure &&
                        param.MinValue <= pressure)
                        satisfiesAmount++;
                });

                if (satisfiesAmount == model.ParamsRanges.Count)
                {
                    NCalc.Expression e = new NCalc.Expression(model.Formula);

                    e.Parameters["tao"] = excerptTime;
                    e.Parameters["T"] = finalTemperatureInFurnace;
                    e.Parameters["Pg"] = pressure;

                    model.EmpiricalModelCoeffs.ForEach(coef =>
                    {
                        e.Parameters[coef.Alias] = coef.Value;
                    });

                    try
                    {
                        if (model.TypeId == (int)EmpiricalModelTypeEnum.Endurance)
                        {
                            bendingStrengthE = e.Evaluate() as double? ?? 0;
                        }
                        else if (model.TypeId == (int)EmpiricalModelTypeEnum.Hardness)
                        {
                            hardnessE = e.Evaluate() as double? ?? 0;
                        }
                        else if (model.TypeId == (int)EmpiricalModelTypeEnum.Porosity)
                        {
                            porosityE = e.Evaluate() as double? ?? 0;
                        }
                        else if (model.TypeId == (int)EmpiricalModelTypeEnum.Density)
                        {
                            densityE = e.Evaluate() as double? ?? 0;
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Произошла ошибка при расчете эмипирических моделей",
                            "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            });

            return (bendingStrengthE, hardnessE, porosityE, densityE);
        }
    }
}
