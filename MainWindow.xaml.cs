using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
            StatIcon.Source = null;

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
                BitmapImage biError = new BitmapImage();
                biError.BeginInit();
                biError.UriSource = new Uri("error.png", UriKind.Relative);
                biError.EndInit();

                BitmapImage biAttention = new BitmapImage();
                biAttention.BeginInit();
                biAttention.UriSource = new Uri("attention.png", UriKind.Relative);
                biAttention.EndInit();

                BitmapImage biOk = new BitmapImage();
                biOk.BeginInit();
                biOk.UriSource = new Uri("ok.png", UriKind.Relative);
                biOk.EndInit();

                StatIcon.Stretch = Stretch.Fill;

                int millisecToInt = int.Parse(millisec);
                txtStatus.Text = status;
                if (status == "Success")
                {
                    if (reply.RoundtripTime < 50)
                    {
                        txtStatus.Foreground = Brushes.Green;
                        StatIcon.Stretch = Stretch.Fill;
                        StatIcon.Source = biOk;
                    }
                    else
                    {
                        txtStatus.Foreground = Brushes.Yellow;
                        StatIcon.Source = biAttention;
                    }
                }
                else if (millisecToInt > 50)
                {
                    txtStatus.Foreground = Brushes.Yellow;
                    StatIcon.Source = biAttention;
                }
                else
                {
                    txtStatus.Foreground = Brushes.Red;
                    StatIcon.Source = biError;
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