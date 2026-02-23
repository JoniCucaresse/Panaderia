using Panaderia.App.Common;
using Panaderia.App.DTOs;
using Panaderia.App.Services.Interfaces;
using Panaderia.Domain.Entities;
using Panaderia.Domain.Repositories;
using Panaderia.Infrastructure.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Panaderia.Wpf.ViewModels
{
    public class MateriaPrimaFormViewModel: BaseViewModel
    {
        private readonly IMateriaPrimaService _service;
        private readonly Window _window;
        private readonly bool _esEdicion;
        private readonly int _id;

        public RelayCommand GuardarCommand { get; }
        public RelayCommand CancelarCommand { get; }

        private string _nombre = string.Empty;
        public string Nombre
        {
            get => _nombre;
            set
            {
                _nombre = value;
                OnPropertyChanged();
            }
        }

        private decimal _costoPorUnidad;
        public decimal CostoPorUnidad
        {
            get => _costoPorUnidad;
            set
            {
                _costoPorUnidad = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<UnidadMedida> Unidades { get; }
            = new(Enum.GetValues<UnidadMedida>());

        private UnidadMedida _unidadSeleccionada;
        public UnidadMedida UnidadSeleccionada
        {
            get => _unidadSeleccionada;
            set
            {
                _unidadSeleccionada = value;
                OnPropertyChanged();
            }
        }

        public MateriaPrimaFormViewModel(
            IMateriaPrimaService service,
            Window window,
            ActualizarMateriaPrimaDto? dto = null)
        {
            _service = service;
            _window = window;

            if (dto == null)
            {
                _esEdicion = false;
                _id = 0;
                Nombre = string.Empty;
                CostoPorUnidad = 0;
                UnidadSeleccionada = UnidadMedida.Gramo;
            }
            else
            {
                _esEdicion = true;
                _id = dto.Id;
                Nombre = dto.Nombre;
                CostoPorUnidad = dto.CostoPorUnidad;
                UnidadSeleccionada = dto.UnidadMedida;
            }

            GuardarCommand = new RelayCommand(Guardar);
            CancelarCommand = new RelayCommand(Cancelar);
        }

        private async void Guardar()
        {
            try
            {
                if (_esEdicion)
                {
                    var dto = new ActualizarMateriaPrimaDto
                    {
                        Id = _id,
                        Nombre = Nombre,
                        UnidadMedida = UnidadSeleccionada,
                        CostoPorUnidad = CostoPorUnidad
                    };

                    var resultado = await _service.ActualizarAsync(dto);

                    if (!resultado.IsSuccess)
                    {
                        MostrarErrores(resultado);
                        return;
                    }
                }
                else
                {
                    var dto = new CrearMateriaPrimaDto
                    {
                        Nombre = Nombre,
                        UnidadMedida = UnidadSeleccionada,
                        CostoPorUnidad = CostoPorUnidad
                    };

                    var resultado = await _service.CrearAsync(dto);

                    if (!resultado.IsSuccess)
                    {
                        MostrarErrores(resultado);
                        return;
                    }
                }

                _window.DialogResult = true;
                _window.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancelar()
        {
            _window.DialogResult = false;
            _window.Close();
        }

        private void MostrarErrores<T>(Result<T> resultado)
        {
            if (resultado.Errors.Any())
            {
                var mensajes = string.Join("\n• ", resultado.Errors);
                MessageBox.Show($"Errores de validación:\n• {mensajes}",
                    "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MessageBox.Show(resultado.ErrorMessage, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
