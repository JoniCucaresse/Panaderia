using Panaderia.Domain.Entities;
using Panaderia.Infrastructure.Repositories;
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
    public class MateriaPrimaFormViewModel: BaseViewModel
    {
        private readonly IMateriaPrimaRepository _repository;
        private readonly Window _window;
        private readonly MateriaPrima _materiaPrima;
        private readonly bool _esEdicion;
        public RelayCommand GuardarCommand { get; }
        public RelayCommand CancelarCommand { get; }
        public Action? Cerrar { get; set; }
        public string Nombre
        {
            get => _materiaPrima.Nombre;
            set
            {
                _materiaPrima.Nombre = value;
                OnPropertyChanged();
            }
        }

        public decimal CostoPorUnidad
        {
            get => _materiaPrima.CostoPorUnidad;
            set
            {
                _materiaPrima.CostoPorUnidad = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<UnidadMedida> Unidades { get; }
            = new(Enum.GetValues<UnidadMedida>());

        public UnidadMedida UnidadSeleccionada
        {
            get => _materiaPrima.UnidadMedida;
            set
            {
                _materiaPrima.UnidadMedida = value;
                OnPropertyChanged();
            }
        }

        

        public MateriaPrimaFormViewModel(
            IMateriaPrimaRepository repository,
            Window window,
            MateriaPrima? materiaPrima = null)
        {
            _repository = repository;
            _window = window;
            if (materiaPrima == null)
            {
                materiaPrima = new MateriaPrima();
                _esEdicion = false;
            }
            else
            {
                materiaPrima = materiaPrima; // 🔥 MISMA INSTANCIA
                _esEdicion = true;
            }

            GuardarCommand = new RelayCommand(Guardar);
            CancelarCommand = new RelayCommand(() => Cerrar?.Invoke());
        }

        private async void Guardar()
        {
            if (string.IsNullOrWhiteSpace(Nombre))
            {
                MessageBox.Show("El nombre es obligatorio");
                return;
            }

            if (CostoPorUnidad <= 0)
            {
                MessageBox.Show("El costo debe ser mayor a 0");
                return;
            }

            if (_esEdicion)
                await _repository.UpdateAsync(_materiaPrima);
            else
                await _repository.AddAsync(_materiaPrima);

            Cerrar?.Invoke();
        }
    }
}
