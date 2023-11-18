using Sintering_of_ceramics.Enums;
using Sintering_of_ceramics.Helpers;
using Sintering_of_ceramics.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sintering_of_ceramics.Windows
{
    /// <summary>
    /// Interaction logic for CreateEditDeleteWindow.xaml
    /// </summary>
    public partial class CreateEditDeleteWindow : Window
    {
        private List<TextBox> Values { get; set; } = new List<TextBox>();
        private List<CheckBox> CheckBoxesValues { get; set; } = new List<CheckBox>();
        private List<ComboBox> ComboBoxesValues { get; set; } = new List<ComboBox>();

        public Dictionary<string, object> ResultValues { get; set; } = new Dictionary<string, object>();


        public CreateEditDeleteWindow()
        {
            InitializeComponent();
        }

        public void InitializeWindow(string actionName, WindowActionTypeEnum actionType, params ModelParamDTO[] values)
        {
            panel.Children.Clear();
            Values.Clear();
            CheckBoxesValues.Clear();
            ResultValues.Clear();
            ComboBoxesValues.Clear();

            this.Title = actionName;

            actionButton.Content = actionType.GetAttributeOfType<DescriptionAttribute>()?.Description ?? string.Empty;

            foreach(var prop in values)
            {
                CreateGridRow(prop.Description, prop.Name, prop.SValue, prop.DValue, prop.BValue, 
                    prop.LValues, prop.DisplayMemberPath, prop.SelectedIndex);
            }
        }


        private void ActionButtonClick(object sender, RoutedEventArgs e)
        {
            foreach (var value in Values)
            {
                if (value.Text.Length == 0)
                {
                    ResultValues.Clear();
                    MessageBox.Show($"Заполнены не все поля", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);

                    return;
                }

                ResultValues.Add(value.Name, value.Text);
            }

            foreach (var value in CheckBoxesValues)
            {
                ResultValues.Add(value.Name, value.IsChecked ?? false);
            }

            foreach (var value in ComboBoxesValues)
            {
                ResultValues.Add(value.Name, value.SelectedIndex);
            }

            this.Hide();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void CreateGridRow(string descrption, string name, string? sValue = null, double? dValue = null, 
            bool? admin = null, List<object>? lValues = null, string? displayMemberPath = null, 
            int? selectedIndex = null)
        {
            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.Margin = new Thickness() { Top = 20, Right = 20, Left = 20 };

            TextBlock info = new TextBlock();
            info.Text = descrption;
            info.FontSize = (double)this.Resources["MainFontSize"];
            info.TextWrapping = TextWrapping.Wrap;
            info.HorizontalAlignment = HorizontalAlignment.Right;
            info.VerticalAlignment = VerticalAlignment.Center;

            grid.Children.Add(info);

            if(lValues != null)
            {
                ComboBox comboBox = new()
                {
                    ItemsSource = lValues,
                    DisplayMemberPath = displayMemberPath,
                    SelectedIndex = selectedIndex ?? 0,
                    Name = name
                };

                Grid.SetColumn(comboBox, 1);
                grid.Children.Add(comboBox);
                ComboBoxesValues.Add(comboBox);
            }
            else if (admin != null)
            {
                CheckBox value = new()
                {
                    IsChecked = admin,
                    FontSize = (double)this.Resources["MainFontSize"],
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness() { Right = 150, Left = 50 },
                    Name = name
                };

                Grid.SetColumn(value, 1);
                grid.Children.Add(value);
                CheckBoxesValues.Add(value);
            }
            else
            {
                TextBox value = new()
                {
                    Text = sValue ?? dValue.ToString(),
                    FontSize = (double)this.Resources["MainFontSize"],
                    VerticalAlignment = VerticalAlignment.Center,
                    Style = (Style)this.Resources["CustomErrorControlOnErrorStyle"],
                    Name = name
                };
                if (dValue != null)
                {
                    value.PreviewTextInput += TextBox_OnPreviewTextInput;
                }
                value.Margin = new Thickness() { Right = 150, Left = 50 };

                Grid.SetColumn(value, 1);
                grid.Children.Add(value);
                Values.Add(value);
            }

            panel.Children.Add(grid);
        }

        private void TextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            var fullText = textBox!.Text.Insert(textBox.SelectionStart, e.Text);

            e.Handled = !double.TryParse(fullText,
                            NumberStyles.AllowDecimalPoint,
                            CultureInfo.InvariantCulture,
                            out var val);
        }
    }
}
