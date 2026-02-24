using Panaderia.App.Common;
using Panaderia.App.DTOs;
using Panaderia.App.Services.Interfaces;
using Panaderia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Panaderia.Wpf.ViewModels
{
    public class ProductoFormViewModel : BaseViewModel
    {
        private readonly IProductoService _productoService;
        private readonly IMateriaPrimaService _materiaPrimaService;
        private readonly Window _window;
        private readonly bool _esEdicion;
        private readonly int _id;

        public RelayCommand GuardarCommand { get; }
        public RelayCommand CancelarCommand { get; }
        public RelayCommand AgregarRecetaCommand { get; }
        public RelayCommand<RecetaItemViewModel> EliminarRecetaCommand { get; }

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

        private int _rinde = 1;
        public int Rinde
        {
            get => _rinde;
            set
            {
                _rinde = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CostoUnitario));
            }
        }

        private decimal? _precioSugerido;
        public decimal? PrecioSugerido
        {
            get => _precioSugerido;
            set
            {
                _precioSugerido = value;
                OnPropertyChanged();
            }
        }

        public decimal CostoTotal => Recetas.Sum(r => r.CostoLinea);
        public decimal CostoUnitario => Rinde > 0 ? CostoTotal / Rinde : 0;

        public ObservableCollection<RecetaItemViewModel> Recetas { get; } = new();
        public ObservableCollection<MateriaPrimaDto> MateriasPrimasDisponibles { get; } = new();

        public ProductoFormViewModel(
            IProductoService productoService,
            Window window,
            ProductoConRecetasDto? producto = null)
        {
            _productoService = productoService;
            _window = window;

            // Obtener servicio de materias primas desde el service provider
            var serviceProvider = ((App)Application.Current).Services;
            _materiaPrimaService = (IMateriaPrimaService)serviceProvider.GetService(typeof(IMateriaPrimaService))!;

            if (producto == null)
            {
                _esEdicion = false;
                _id = 0;
            }
            else
            {
                _esEdicion = true;
                _id = producto.Id;
                Nombre = producto.Nombre;
                Rinde = producto.Rinde;
                PrecioSugerido = producto.PrecioSugerido;

                foreach (var receta in producto.Recetas)
                {
                    var item = new RecetaItemViewModel
                    {
                        Id = receta.Id,
                        MateriaPrimaId = receta.MateriaPrimaId,
                        MateriaPrimaNombre = receta.MateriaPrimaNombre,
                        UnidadMedida = receta.UnidadMedida,
                        Cantidad = receta.Cantidad,
                        CostoPorUnidad = receta.CostoPorUnidad
                    };
                    item.PropertyChanged += (s, e) => ActualizarCostos();
                    Recetas.Add(item);
                }
            }

            GuardarCommand = new RelayCommand(Guardar);
            CancelarCommand = new RelayCommand(Cancelar);
            AgregarRecetaCommand = new RelayCommand(AgregarReceta);
            EliminarRecetaCommand = new RelayCommand<RecetaItemViewModel>(EliminarReceta);

            _ = CargarMateriasPrimasAsync();
        }

        private async Task CargarMateriasPrimasAsync()
        {
            var resultado = await _materiaPrimaService.ObtenerTodasAsync();
            if (resultado.IsSuccess && resultado.Data != null)
            {
                MateriasPrimasDisponibles.Clear();
                foreach (var materia in resultado.Data)
                    MateriasPrimasDisponibles.Add(materia);
            }
        }

        private void AgregarReceta()
        {
            if (!MateriasPrimasDisponibles.Any())
            {
                MessageBox.Show("No hay materias primas disponibles", "Advertencia",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var primera = MateriasPrimasDisponibles.First();
            var item = new RecetaItemViewModel
            {
                MateriaPrimaId = primera.Id,
                MateriaPrimaNombre = primera.Nombre,
                UnidadMedida = primera.UnidadMedida,
                Cantidad = 0,
                CostoPorUnidad = primera.CostoPorUnidad
            };
            item.PropertyChanged += (s, e) => ActualizarCostos();
            Recetas.Add(item);
        }

        private void EliminarReceta(RecetaItemViewModel? item)
        {
            if (item != null)
            {
                Recetas.Remove(item);
                ActualizarCostos();
            }
        }

        private void ActualizarCostos()
        {
            OnPropertyChanged(nameof(CostoTotal));
            OnPropertyChanged(nameof(CostoUnitario));
        }

        private async void Guardar()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Nombre))
                {
                    MessageBox.Show("El nombre es obligatorio", "Validación",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (Rinde <= 0)
                {
                    MessageBox.Show("El rinde debe ser mayor a 0", "Validación",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!Recetas.Any())
                {
                    MessageBox.Show("Debe agregar al menos una materia prima a la receta", "Validación",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (_esEdicion)
                {
                    var dto = new ActualizarProductoDto
                    {
                        Id = _id,
                        Nombre = Nombre,
                        Rinde = Rinde,
                        PrecioSugerido = PrecioSugerido,
                        Recetas = Recetas.Select(r => new ActualizarRecetaDto
                        {
                            Id = r.Id,
                            MateriaPrimaId = r.MateriaPrimaId,
                            Cantidad = r.Cantidad
                        }).ToList()
                    };

                    var resultado = await _productoService.ActualizarAsync(dto);

                    if (!resultado.IsSuccess)
                    {
                        MostrarErrores(resultado);
                        return;
                    }
                }
                else
                {
                    var dto = new CrearProductoDto
                    {
                        Nombre = Nombre,
                        Rinde = Rinde,
                        PrecioSugerido = PrecioSugerido,
                        Recetas = Recetas.Select(r => new CrearRecetaDto
                        {
                            MateriaPrimaId = r.MateriaPrimaId,
                            Cantidad = r.Cantidad
                        }).ToList()
                    };

                    var resultado = await _productoService.CrearAsync(dto);

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
    public class RecetaItemViewModel : BaseViewModel
    {
        public int? Id { get; set; }

        private int _materiaPrimaId;
        public int MateriaPrimaId
        {
            get => _materiaPrimaId;
            set
            {
                _materiaPrimaId = value;
                OnPropertyChanged();
            }
        }

        private string _materiaPrimaNombre = string.Empty;
        public string MateriaPrimaNombre
        {
            get => _materiaPrimaNombre;
            set
            {
                _materiaPrimaNombre = value;
                OnPropertyChanged();
            }
        }

        private UnidadMedida _unidadMedida;
        public UnidadMedida UnidadMedida
        {
            get => _unidadMedida;
            set
            {
                _unidadMedida = value;
                OnPropertyChanged();
            }
        }

        private decimal _cantidad;
        public decimal Cantidad
        {
            get => _cantidad;
            set
            {
                _cantidad = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CostoLinea));
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
                OnPropertyChanged(nameof(CostoLinea));
            }
        }

        public decimal CostoLinea => Cantidad * CostoPorUnidad;
    }
}
