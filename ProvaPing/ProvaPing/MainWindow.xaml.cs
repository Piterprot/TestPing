using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.NetworkInformation;

namespace ProvaPing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            txtStatus.Text = "";
            txtMs.Text = "";
            string Indirizzo = txtIP.Text;


            try
            {
                Ping pinger = new Ping();
                PingReply reply = pinger.Send(Indirizzo);
                string status = reply.Status.ToString();
                string millisec = reply.RoundtripTime.ToString();
                txtStatus.Text = status;
                txtMs.Text = millisec;
                this.Cursor = Cursors.Arrow;
            }
            catch (System.Net.NetworkInformation.PingException)
            {
                MessageBox.Show("Errore, l'host risulta sconosciuto o non raggiungibile");
            }

        }
    }
}