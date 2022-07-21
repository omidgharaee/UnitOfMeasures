using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UnitOfMeasures.Core.Domain.Dimension.Data;
using UnitOfMeasures.Core.Domain.Services;
using UnitOfMeasures.Core.Domain.Unit.Entities;
using UnitOfMeasures.Endpoints.WPF.Windows;

namespace UnitOfMeasures.Endpoints.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IDimensionQueryService _dimensionQueryService;
        private readonly IUnitRepository _unitRepository;
        private readonly IConverter _converter;
        private decimal[,] daDataMatrix = null;
        public MainWindow(IDimensionQueryService dimensionQueryService, IUnitRepository unitRepository, IConverter converter)
        {
            InitializeComponent();
            _dimensionQueryService = dimensionQueryService;
            _unitRepository = unitRepository;
            _converter = converter;

            this.ComboBoxDimensionType.SelectionChanged += ComboBoxDimensionType_SelectionChanged;
            var dimensions = _dimensionQueryService.QueryGetAll();
            this.ComboBoxDimensionType.ItemsSource = dimensions;
            if (dimensions.Any())
                this.ComboBoxDimensionType.SelectedValue = dimensions[0].Id;
        }

        private void ComboBoxDimensionType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var id = this.ComboBoxDimensionType.SelectedValue.ToString();
            Guid _id;
            if (Guid.TryParse(id, out _id))
            {
                ComboBoxConvertUnit.ItemsSource = _dimensionQueryService.QueryGetWithUnits(_id).Units;
                ComboBoxIntoUnit.ItemsSource = _dimensionQueryService.QueryGetWithUnits(_id).Units;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DimensionsWindow dimensionsWindow = new DimensionsWindow();
            dimensionsWindow.ShowDialog();
            var dimensions = _dimensionQueryService.QueryGetAll();
            this.ComboBoxDimensionType.ItemsSource = dimensions;
        }

        private void ManageUnitsBtn_Click(object sender, RoutedEventArgs e)
        {
            UnitsWindow unitsWindow = new UnitsWindow();
            unitsWindow.ShowDialog();

            if (this.ComboBoxDimensionType.SelectedValue != null)
            {
                var id = this.ComboBoxDimensionType.SelectedValue.ToString();
                Guid _id;
                if (Guid.TryParse(id, out _id))
                {
                    ComboBoxConvertUnit.ItemsSource = _dimensionQueryService.QueryGetWithUnits(_id).Units;
                    ComboBoxIntoUnit.ItemsSource = _dimensionQueryService.QueryGetWithUnits(_id).Units;
                }
            }
        }

        private void ConvertBtn_Click(object sender, RoutedEventArgs e)
        {
            var result = _converter.Convert(ConvertTextBox.Text, (Unit)ComboBoxConvertUnit.SelectedItem, (Unit)ComboBoxIntoUnit.SelectedItem, (List<Unit>)ComboBoxConvertUnit.ItemsSource);
            ConvertedValueTextBox.Text = result;
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


        private void ConvertTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !DoubleCharChecker(e.Text);
            base.OnTextInput(e);
        }
    }
}
