using Panaderia.App.DTOs;
using Panaderia.App.Services.Interfaces;
using Panaderia.Wpf.Views;
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
    public class ProductosViewModel : BaseViewModel
    {
        private readonly IProductoService _service;

        public RelayCommand EditarCommand { get; }
        public RelayCommand EliminarCommand { get; }
        public RelayCommand VerRecetaCommand { get; }
        public ICommand AgregarCommand { get; }

        public ObservableCollection<ProductoDto> Productos { get; } = new();

        private ProductoDto? _productoSeleccionado;
        public ProductoDto? ProductoSeleccionado
        {
            get => _productoSeleccionado;
            set
            {
                _productoSeleccionado = value;
                OnPropertyChanged();
                EditarCommand.RaiseCanExecuteChanged();
                EliminarCommand.RaiseCanExecuteChanged();
                VerRecetaCommand.RaiseCanExecuteChanged();
            }
        }

        public ProductosViewModel(IProductoService service)
        {
            _service = service;

            AgregarCommand = new RelayCommand(Agregar);
            EditarCommand = new RelayCommand(
                Editar,
                () => ProductoSeleccionado != null);
            EliminarCommand = new RelayCommand(
                Eliminar,
                () => ProductoSeleccionado != null);
            VerRecetaCommand = new RelayCommand(
                VerReceta,
                () => ProductoSeleccionado != null);

            _ = CargarAsync();
        }

        private async Task CargarAsync()
        {
            try
            {
                Productos.Clear();

                var resultado = await _service.ObtenerTodosAsync();

                if (resultado.IsSuccess && resultado.Data != null)
                {
                    foreach (var item in resultado.Data)
                        Productos.Add(item);
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
            if (ProductoSeleccionado == null) return;

            // Obtener el producto completo con recetas
            var resultado = await _service.ObtenerPorIdAsync(ProductoSeleccionado.Id);

            if (!resultado.IsSuccess || resultado.Data == null)
            {
                MessageBox.Show(resultado.ErrorMessage, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (AbrirFormulario(resultado.Data))
                await CargarAsync();
        }

        private async void Eliminar()
        {
            if (ProductoSeleccionado == null) return;

            var confirmar = MessageBox.Show(
                $"¿Eliminar el producto '{ProductoSeleccionado.Nombre}'?\n\nEsto también eliminará su receta.",
                "Confirmar eliminación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirmar == MessageBoxResult.Yes)
            {
                var resultado = await _service.EliminarAsync(ProductoSeleccionado.Id);

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

        private async void VerReceta()
        {
            if (ProductoSeleccionado == null) return;

            var resultado = await _service.ObtenerPorIdAsync(ProductoSeleccionado.Id);

            if (!resultado.IsSuccess || resultado.Data == null)
            {
                MessageBox.Show(resultado.ErrorMessage, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var window = new RecetaDetalleView
            {
                Owner = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive),
                DataContext = resultado.Data
            };

            window.ShowDialog();
        }

        private bool AbrirFormulario(ProductoConRecetasDto? producto = null)
        {
            var window = new ProductoFormView
            {
                Owner = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive)
            };

            var vm = new ProductoFormViewModel(_service, window, producto);
            window.DataContext = vm;

            return window.ShowDialog() == true;
        }
    }
}
