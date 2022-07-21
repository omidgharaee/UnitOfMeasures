using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UnitOfMeasures.Core.Application;
using UnitOfMeasures.Core.Domain.Dimension.Data;
using UnitOfMeasures.Core.Domain.Unit.Entities;

namespace UnitOfMeasures.Endpoints.WPF.Windows
{
    /// <summary>
    /// Interaction logic for UnitsWindow.xaml
    /// </summary>
    public partial class UnitsWindow : Window, IDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly IDimensionQueryService _dimensionQueryService;
        private readonly IUnitRepository _unitRepository;
        private readonly Core.Application.Unit.CommandHandlers.CreateHandler _unitCreateHandler;
        private readonly Core.Application.Unit.CommandHandlers.DeleteHandler _deleteHandler;
        private readonly Core.Application.Unit.CommandHandlers.UpdateHandler _updateHandler;
        private List<Core.Domain.Unit.Entities.Unit> _units;
        private List<UnitType> UnitTypes = new List<UnitType>();

        public Dictionary<string, string> ValidationErrors = new Dictionary<string, string>();

        public List<Core.Domain.Unit.Entities.Unit> Units
        {
            get
            {
                return _units;
            }
            set
            {
                _units = value;
                FirePropertyChanged();
            }
        }
        public string Error { get; }

        public string this[string columnName]
        {
            get
            {
                if (ValidationErrors.ContainsKey(columnName))
                {
                    ValidationErrors.Remove(columnName);
                }

                switch (columnName)
                {
                    case nameof(FormulaFromBase):

                        if (!string.IsNullOrWhiteSpace(FormulaFromBase))
                        {
                            try
                            {
                                if (FormulaToBase == "(")
                                {
                                    ValidationErrors[columnName] = "Formula is not valid!";
                                    return "Formula is not valid!";
                                }

                                var compute = new DataTable().Compute(FormulaFromBase.Replace("a" , "1"), "");
                            }
                            catch (Exception)
                            {
                                ValidationErrors[columnName] = "Formula is not valid!";
                                return "Formula is not valid!";
                            }
                        }
                        else
                        {
                            ValidationErrors[columnName] = "Please Type Formula To Base.";
                            return "Please Type Formula To Base.";
                        }


                        break;
                    case nameof(FormulaToBase):
                        if (!string.IsNullOrWhiteSpace(FormulaToBase))
                        {
                            try
                            {
                                if (FormulaToBase == "(")
                                {
                                    ValidationErrors[columnName] = "Formula is not valid!";
                                    return "Formula is not valid!";
                                }
                                var compute = new DataTable().Compute(FormulaToBase.Replace("a" , "1"), "");
                            }
                            catch (Exception)
                            {
                                ValidationErrors[columnName] = "Formula is not valid!";
                                return "Formula is not valid!";
                            }
                        }
                        else
                        {
                            ValidationErrors[columnName] = "Please Type Formula To Base.";
                            return "Please Type Formula To Base.";
                        }

                        break;
                    case nameof(ConversionFactor):
                        {
                            if (string.IsNullOrWhiteSpace(ConversionFactor))
                            {
                                ValidationErrors[columnName] = "Please Type Conversion Factor.";
                                return "Please Type Conversion Factor.";
                            }

                        }
                        break;

                    case nameof(UnitName):
                        {
                            if (string.IsNullOrWhiteSpace(UnitName))
                            {
                                ValidationErrors[columnName] = "Please Type Conversion Factor.";
                                return "Please Type Conversion Factor.";
                            }
                        }
                        break;

                    case nameof(Abbreviation):
                        {
                            if (string.IsNullOrWhiteSpace(Abbreviation))
                            {
                                ValidationErrors[columnName] = "Please Type Conversion Factor.";
                                return "Please Type Conversion Factor.";
                            }
                        }
                        break;
                }

                return string.Empty;
            }
        }



        private string _formulaFromBase;
        public string FormulaFromBase
        {
            get
            {
                return _formulaFromBase;
            }
            set
            {
                _formulaFromBase = value;
                FirePropertyChanged();
            }
        }

        private string _formulaToBase;
        public string FormulaToBase
        {
            get
            {
                return _formulaToBase;
            }
            set
            {
                _formulaToBase = value;
                FirePropertyChanged();
            }
        }

        private string _conversionFactor;
        public string ConversionFactor
        {
            get
            {
                return _conversionFactor;
            }
            set
            {
                _conversionFactor = value;
                FirePropertyChanged();
            }
        }

        private string _unitName;
        public string UnitName
        {
            get
            {
                return _unitName;
            }
            set
            {
                _unitName = value;
                FirePropertyChanged();
            }
        }

        private string _abbreviation;
        public string Abbreviation
        {
            get
            {
                return _abbreviation;
            }
            set
            {
                _abbreviation = value;
                FirePropertyChanged();
            }
        }
        private bool _isReadyState = true;
        public bool IsReadyState
        {
            get
            {
                return _isReadyState;
            }
            set
            {
                _isReadyState = value;
                FirePropertyChanged();
            }
        }

        public UnitsWindow()
        {
            InitializeComponent();
            DataContext = this;
            _dimensionQueryService = ServiceContainer.Instance.GetRequiredService<IDimensionQueryService>();
            _unitRepository = ServiceContainer.Instance.GetRequiredService<IUnitRepository>();
            _unitCreateHandler = ServiceContainer.Instance.GetRequiredService<Core.Application.Unit.CommandHandlers.CreateHandler>();
            _deleteHandler = ServiceContainer.Instance.GetRequiredService<Core.Application.Unit.CommandHandlers.DeleteHandler>();
            _updateHandler = ServiceContainer.Instance.GetRequiredService<Core.Application.Unit.CommandHandlers.UpdateHandler>();
            convertionType.SelectionChanged += ConvertionType_SelectionChanged;
            unitType.SelectionChanged += UnitType_SelectionChanged;
            convertionType.ItemsSource = _dimensionQueryService.QueryGetAll();
            AddUnitPanel.IsEnabled = false;
            dataGrid.RowEditEnding += DataGrid_RowEditEnding;
            dataGrid.SelectionChanged += DataGrid_SelectionChanged;

            UnitTypes.Add(new UnitType { Type = "Base", Name = "واحد سنجش پایه" });
            UnitTypes.Add(new UnitType { Type = "Multiplicative", Name = "واحد ضریب دار" });
            UnitTypes.Add(new UnitType { Type = "Formula", Name = "واحد فرمول دار" });


            unitType.ItemsSource = UnitTypes;
            unitType.SelectedItem = UnitTypes[0];


        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems == null || e.AddedItems.Count == 0 || e.AddedItems.Count > 1)
                return;

            Unit unit = (Unit)e.AddedItems[0];

            switch (unit.Type)
            {
                case "Base":
                    dataGrid.Columns[3].IsReadOnly = true;
                    dataGrid.Columns[4].IsReadOnly = true;
                    dataGrid.Columns[5].IsReadOnly = true;
                    dataGrid.Columns[6].IsReadOnly = true;
                    break;
                case "Multiplicative":
                    dataGrid.Columns[3].IsReadOnly = false;
                    dataGrid.Columns[4].IsReadOnly = true;
                    dataGrid.Columns[5].IsReadOnly = true;
                    dataGrid.Columns[6].IsReadOnly = true;
                    break;
                case "Formula":
                    dataGrid.Columns[3].IsReadOnly = true;
                    dataGrid.Columns[4].IsReadOnly = false;
                    dataGrid.Columns[5].IsReadOnly = false;
                    dataGrid.Columns[6].IsReadOnly = true;
                    break;
                default:
                    break;
            }
        }

        private void DataGrid_RowEditEnding(object? sender, DataGridRowEditEndingEventArgs e)
        {
            _updateHandler.Handle(null);
        }

        private void UnitType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            var type = unitType.SelectedValue.ToString();

            switch (type)
            {
                case "Base":
                    ConversionFactorPanel.Visibility = Visibility.Collapsed;
                    FormulaFromBasePanel.Visibility = Visibility.Collapsed;
                    FormulaToBasetextPanel.Visibility = Visibility.Collapsed;
                    break;
                case "Multiplicative":
                    ConversionFactorPanel.Visibility = Visibility.Visible;
                    FormulaFromBasePanel.Visibility = Visibility.Collapsed;
                    FormulaToBasetextPanel.Visibility = Visibility.Collapsed;
                    break;
                case "Formula":
                    ConversionFactorPanel.Visibility = Visibility.Collapsed;
                    FormulaFromBasePanel.Visibility = Visibility.Visible;
                    FormulaToBasetextPanel.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }


        }

        private void ConvertionType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var id = convertionType.SelectedValue.ToString();
            Guid _id;
            if (Guid.TryParse(id, out _id))
            {
                Units = _dimensionQueryService.QueryGetWithUnits(_id).Units;
                AddUnitPanel.IsEnabled = true;
                dataGrid.ItemsSource = Units;
            }
            else
            {
                AddUnitPanel.IsEnabled = false;
            }


        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ValidationErrors.Count > 0)
            {
                if (FormulaToBasetextPanel.Visibility == Visibility.Visible && ValidationErrors.ContainsKey(nameof(FormulaToBase)))
                {
                    return;
                }
                else if (FormulaFromBasePanel.Visibility == Visibility.Visible && ValidationErrors.ContainsKey(nameof(FormulaFromBase)))
                {
                    return;
                }
                else if (ConversionFactorPanel.Visibility == Visibility.Visible && ValidationErrors.ContainsKey(nameof(ConversionFactor)))
                {
                    return;
                }
            }


            var id = convertionType.SelectedValue.ToString();
            Guid _id;
            if (Guid.TryParse(id, out _id))
            {
                Core.Domain.Unit.Commands.Create unit = new Core.Domain.Unit.Commands.Create();
                unit.Abbreviation = AbbreviationtextBox.Text;
                unit.Name = NametextBox.Text;
                unit.PersianName = PersianNametextBox.Text;
                if (unitType.SelectedValue.ToString() == "Formula")
                {
                    unit.ConversionFactor = 0;
                }
                else
                {
                    unit.ConversionFactor = !string.IsNullOrWhiteSpace(ConversionFactortextBox.Text) ? double.Parse(ConversionFactortextBox.Text) : 1;
                }
                unit.FormulaFromBase = FormulaFromBasetextBox.Text;
                unit.FormulaToBase = FormulaToBasetextBox.Text;
                unit.Type = unitType.SelectedValue.ToString();
                unit.DimensionId = _id;
                unit.Id = Guid.NewGuid();
                _unitCreateHandler.Handle(unit);
                var inserted = _unitRepository.Load(unit.Id);
                dataGrid.ItemsSource = null;
                dataGrid.ItemsSource = Units;
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !DoubleCharChecker(e.Text);
            base.OnTextInput(e);
        }

        private bool DoubleCharChecker(string str)
        {
            foreach (char c in str)
            {
                if (c.Equals('.'))
                    return true;

                else if (Char.IsNumber(c))
                    return true;
            }
            return false;
        }

        public void FirePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            if (dg != null)
            {
                DataGridRow dgr = (DataGridRow)(dg.ItemContainerGenerator.ContainerFromIndex(dg.SelectedIndex));
                if (e.Key == Key.Delete && !dgr.IsEditing)
                {
                    if ((dgr.Item as Core.Domain.Unit.Entities.Unit) != null && (dgr.Item as Core.Domain.Unit.Entities.Unit).Id != null)
                    {
                        var result = MessageBox.Show($"Want to delete {(dgr.Item as Core.Domain.Unit.Entities.Unit).Name}?", "Delete", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                        if (result == MessageBoxResult.Cancel)
                        {
                            e.Handled = true;
                            return;
                        }

                        _deleteHandler.Handle(new Core.Domain.Unit.Commands.Delete((dgr.Item as Core.Domain.Unit.Entities.Unit).Id));
                        dataGrid.ItemsSource = null;
                        dataGrid.ItemsSource = Units;
                    }
                }
            }
        }

        private void FormulaToBasetextBox_PreviewKeyDown(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !TextboxValidationMathFormula(e.Text);
            base.OnTextInput(e);
        }

        private bool TextboxValidationMathFormula(string text)
        {
            foreach (char c in text)
            {
                if (c.Equals('.'))
                    return true;
                else if (c.Equals('*'))
                    return true;
                else if (c.Equals('/'))
                    return true;
                else if (c.Equals('-'))
                    return true;
                else if (c.Equals('+'))
                    return true;
                else if (c.Equals('('))
                    return true;
                else if (c.Equals(')'))
                    return true;
                else if (c.Equals('a'))
                    return true;
                else if (Char.IsNumber(c))
                    return true;
            }
            return false;
        }


    }

    public class UnitType
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
