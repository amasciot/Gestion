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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace ConexionGestionPedidos
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        SqlConnection miConexionSql;

        public SqlDataAdapter SqlCommand { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            string miConexion = ConfigurationManager.ConnectionStrings["ConexionGestionPedidos.Properties.Settings.GestionPedidosConnectionString"].ConnectionString;
            
            miConexionSql = new SqlConnection(miConexion);

            MuestraClientes();

        }

        private void MuestraClientes()
        {
            string consulta = "SELECT * FROM CLIENTE";

            SqlDataAdapter miAdapatadorSql = new SqlDataAdapter(consulta, miConexionSql);

            using (miAdapatadorSql)
            {
                DataTable clientesTabla = new DataTable();

                miAdapatadorSql.Fill(clientesTabla);

                listaCliente.DisplayMemberPath = "nombre";
                listaCliente.SelectedValuePath = "Id";
                listaCliente.ItemsSource = clientesTabla.DefaultView;
               

            }
        }

        private void MuestraPedidos()
        {
            string consulta = "SELECT * FROM PEDIDO P INNER JOIN CLIENTE C ON C.ID=P.cCliente" +
                " WHERE C.ID=@ClienteId";

            SqlCommand sqlCommando = new SqlCommand(consulta, miConexionSql);

            SqlDataAdapter miAdapatadorSql = new SqlDataAdapter(sqlCommando);

            using (miAdapatadorSql)
            {
                sqlCommando.Parameters.AddWithValue("@ClienteId", listaCliente.SelectedValue); 

                DataTable pedidosTabla = new DataTable();

                miAdapatadorSql.Fill(pedidosTabla);

                listaPedidos.DisplayMemberPath = "fechaPedido";
                listaPedidos.SelectedValuePath = "Id";
                listaPedidos.ItemsSource = pedidosTabla.DefaultView;

                listaPago.DisplayMemberPath = "formaPago";
                listaPago.SelectedValuePath = "Id";
                listaPago.ItemsSource = pedidosTabla.DefaultView;

            }
         
        }
        private void listaCliente_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MuestraPedidos();

        }

    }
}
