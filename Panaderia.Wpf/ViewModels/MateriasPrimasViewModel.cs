using Panaderia.Domain.Entities;
using Panaderia.Domain.Repositories;
using Panaderia.Infrastructure.Repositories;
using Panaderia.Wpf.Views;
using Panaderia.App.DTOs;
using Panaderia.App.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Panaderia.Wpf.ViewModels
{
    public class MateriasPrimasViewModel : BaseViewModel
    {
        private readonly IMateriaPrimaService _service;

        public RelayCommand EditarCommand { get; }
        public RelayCommand EliminarCommand { get; }
        public ICommand AgregarCommand { get; }

        public ObservableCollection<MateriaPrimaDto> MateriasPrimas { get; } = new();

        private MateriaPrimaDto? _materiaPrimaSeleccionada;
        public MateriaPrimaDto? MateriaPrimaSeleccionada
        {
            get => _materiaPrimaSeleccionada;
            set
            {
                _materiaPrimaSeleccionada = value;
                OnPropertyChanged();
                EditarCommand.RaiseCanExecuteChanged();
                EliminarCommand.RaiseCanExecuteChanged();
            }
        }

        public MateriasPrimasViewModel(IMateriaPrimaService service)
        {
            _service = service;

            AgregarCommand = new RelayCommand(Agregar);
            EditarCommand = new RelayCommand(
                Editar,
                () => MateriaPrimaSeleccionada != null);
            EliminarCommand = new RelayCommand(
                Eliminar,
                () => MateriaPrimaSeleccionada != null);

            _ = CargarAsync();
        }

        private async Task CargarAsync()
        {
            try
            {
                MateriasPrimas.Clear();

                var resultado = await _service.ObtenerTodasAsync();

                if (resultado.IsSuccess && resultado.Data != null)
                {
                    foreach (var item in resultado.Data)
                        MateriasPrimas.Add(item);
                }
                else
                {
                    MessageBox.Show(resultado.ErrorMessage, "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void Agregar()
        {
            if (AbrirFormulario())
                await CargarAsync();
        }

        private async void Editar()
        {
            if (MateriaPrimaSeleccionada == null) return;

            var dto = new ActualizarMateriaPrimaDto
            {
                Id = MateriaPrimaSeleccionada.Id,
                Nombre = MateriaPrimaSeleccionada.Nombre,
                UnidadMedida = MateriaPrimaSeleccionada.UnidadMedida,
                CostoPorUnidad = MateriaPrimaSeleccionada.CostoPorUnidad
            };

            if (AbrirFormulario(dto))
                await CargarAsync();
        }

        private async void Eliminar()
        {
            if (MateriaPrimaSeleccionada == null) return;

            var confirmar = MessageBox.Show(
                $"¿Eliminar '{MateriaPrimaSeleccionada.Nombre}'?",
                "Confirmar eliminación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirmar == MessageBoxResult.Yes)
            {
                var resultado = await _service.EliminarAsync(MateriaPrimaSeleccionada.Id);

                if (resultado.IsSuccess)
                {
                    await CargarAsync();
                }
                else
                {
                    MessageBox.Show(resultado.ErrorMessage, "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool AbrirFormulario(ActualizarMateriaPrimaDto? dto = null)
        {
            var window = new MateriaPrimaFormView
            {
                Owner = Application.Current.MainWindow
            };

            var vm = new MateriaPrimaFormViewModel(_service, window, dto);
            window.DataContext = vm;

            return window.ShowDialog() == true;
        }

    }
}
