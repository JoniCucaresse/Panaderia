using Panaderia.Domain.Entities;
using Panaderia.Infrastructure.Repositories;
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
    public class MateriasPrimasViewModel : BaseViewModel
    {
        private readonly IMateriaPrimaRepository _repository;
        public RelayCommand EditarCommand { get; }
        public RelayCommand EliminarCommand { get; }

        public ObservableCollection<MateriaPrima> MateriasPrimas { get; } = new();

        private MateriaPrima? _materiaPrimaSeleccionada;
        public MateriaPrima? MateriaPrimaSeleccionada
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

        public ICommand AgregarCommand { get; }
        //public ICommand EditarCommand { get; }
        //public ICommand EliminarCommand { get; }

        public MateriasPrimasViewModel(IMateriaPrimaRepository repository)
        {
            _repository = repository;

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
            MateriasPrimas.Clear();
            var lista = await _repository.GetAllAsync();
            foreach (var item in lista)
                MateriasPrimas.Add(item);
        }

        private async void Agregar()
        {
            if (AbrirFormulario())
                await CargarAsync();
        }

        private async void Editar()
        {
            if (MateriaPrimaSeleccionada == null) return;

            // Copia para no editar directo
            var copia = new MateriaPrima
            {
                Id = MateriaPrimaSeleccionada.Id,
                Nombre = MateriaPrimaSeleccionada.Nombre,
                UnidadMedida = MateriaPrimaSeleccionada.UnidadMedida,
                CostoPorUnidad = MateriaPrimaSeleccionada.CostoPorUnidad
            };

            if (AbrirFormulario(copia))
                await CargarAsync();
        }

        private async void Eliminar()
        {
            if (MateriaPrimaSeleccionada == null) return;

            var confirmar = MessageBox.Show(
                "¿Eliminar materia prima?",
                "Confirmar",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirmar == MessageBoxResult.Yes)
            {
                await _repository.DeleteAsync(MateriaPrimaSeleccionada.Id);
                await CargarAsync();
            }
        }
        private bool AbrirFormulario(MateriaPrima? materia = null)
        {
            var window = new MateriaPrimaFormView
            {
                Owner = Application.Current.MainWindow
            };

            var vm = new MateriaPrimaFormViewModel(_repository, window, materia);
            window.DataContext = vm;

            return window.ShowDialog() == true;
        }

    }
}
