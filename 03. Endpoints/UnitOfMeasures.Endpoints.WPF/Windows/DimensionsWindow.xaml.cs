using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UnitOfMeasures.Core.Application;
using UnitOfMeasures.Core.Application.Dimension.CommandHandlers;
using UnitOfMeasures.Core.Domain.Dimension.Data;
using UnitOfMeasures.Core.Domain.Dimension.Entities;

namespace UnitOfMeasures.Endpoints.WPF.Windows
{
    /// <summary>
    /// Interaction logic for DimensionsWindow.xaml
    /// </summary>
    public partial class DimensionsWindow : Window
    {
        private readonly CreateHandler _createHandler;
        private readonly UpdateHandler _updateHandler;
        private readonly DeleteHandler _deleteHandler;
        private readonly IDimensionQueryService _dimensionQueryService;
        public List<Dimension> Dimensions { get; set; } = new List<Dimension>();
        public DimensionsWindow()
        {
            InitializeComponent();
            DataContext = this;
            _createHandler = ServiceContainer.Instance.GetRequiredService<CreateHandler>();
            _updateHandler = ServiceContainer.Instance.GetRequiredService<UpdateHandler>();
            _deleteHandler = ServiceContainer.Instance.GetRequiredService<DeleteHandler>();
            _dimensionQueryService = ServiceContainer.Instance.GetRequiredService<IDimensionQueryService>();

            Dimensions = _dimensionQueryService.QueryGetAll();

            this.Closed += DimensionsWindow_Closed;
            dataGrid.RowEditEnding += DataGrid_RowEditEnding;
        }

        private void DataGrid_RowEditEnding(object? sender, DataGridRowEditEndingEventArgs e)
        {
            _updateHandler.Handle();
        }

        private void DimensionsWindow_Closed(object? sender, EventArgs e)
        {
            foreach (var item in Dimensions)
            {
                if (item.Id == null || item.Id == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                    _createHandler.Handle(new Core.Domain.Dimension.Commands.Create(Guid.NewGuid(), item.Name));
            }
        }

        void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            DataGridRow dgr = (DataGridRow)(dg.ItemContainerGenerator.ContainerFromIndex(dg.SelectedIndex));


            if (dg != null)
            {

                if (e.Key == Key.Delete && !dgr.IsEditing)
                {
                    if ((dgr.Item as Dimension) != null && (dgr.Item as Dimension).Id != null)
                    {

                        var result = MessageBox.Show($"Want to delete {(dgr.Item as Dimension).Name}?", "Delete", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                        if (result == MessageBoxResult.Cancel)
                        {
                            e.Handled = true;
                            return;
                        }

                        _deleteHandler.Handle(new Core.Domain.Dimension.Commands.Delete((dgr.Item as Dimension).Id));
                    }
                }
            }
        }


        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


    }
}
