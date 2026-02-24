using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Panaderia.Wpf.Views
{
    /// <summary>
    /// Lógica de interacción para RecetaDetalleView.xaml
    /// </summary>
    public partial class RecetaDetalleView : Window
    {
        public RecetaDetalleView()
        {
            InitializeComponent();
        }
        private void Cerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
