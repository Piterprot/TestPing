using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace provaPing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();

            txtStatus.Text = "";
            txtMs.Text = "";


        }
        void Button_Click(object sender, RoutedEventArgs e)
        {
            ping(sender, e);
        }
        void txtIP_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                ping(sender, e);
            }
        }

        void ping(object sender, RoutedEventArgs e)
        {
            try
            {
                Ping pinger = new Ping();
                this.Cursor = Cursors.Wait;
                string Indirizzo = txtIP.Text;
                PingReply reply = pinger.Send(Indirizzo);
                string status = reply.Status.ToString();
                string millisec = reply.RoundtripTime.ToString();
                int millisecToInt = int.Parse(millisec);
                txtStatus.Text = status;
                if (status == "Success")
                {
                    txtStatus.Foreground = Brushes.Green;
                }
                else if (status == "TimedOut")
                {
                    txtStatus.Foreground = Brushes.Orange;
                }
                else if (millisecToInt > 50)
                {
                    txtStatus.Foreground = Brushes.Yellow;
                }
                else
                {
                    txtStatus.Foreground = Brushes.Red;
                }
                txtMs.Text = millisec + " ms";
                this.Cursor = Cursors.Arrow;
            }
            catch
            {
                MessageBox.Show("Inserire un indirizzo IP valido");
                return;
            }
        }


    }
}