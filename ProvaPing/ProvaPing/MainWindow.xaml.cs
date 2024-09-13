using System.Net.NetworkInformation;
using System.Runtime.Intrinsics.X86;
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

                    BitmapImage bi3 = new BitmapImage();
                    bi3.BeginInit();
                    bi3.UriSource = new Uri("success.png", UriKind.Relative);
                    bi3.EndInit();
                    StatIcon.Stretch = Stretch.Fill;
                    StatIcon.Source = bi3;



                }
                else if (status == "TimedOut")
                {
                    txtStatus.Foreground = Brushes.Orange;
                }
                else if (millisecToInt > 50)
                {
                    txtStatus.Foreground = Brushes.Yellow;
                    BitmapImage bi2 = new BitmapImage();
                    bi2.BeginInit();
                    bi2.UriSource = new Uri("warning.png", UriKind.Relative);
                    bi2.EndInit();
                    StatIcon.Source = bi2;



                }
                else
                {
                    txtStatus.Foreground = Brushes.Red;

                    BitmapImage bi1 = new BitmapImage();
                    bi1.BeginInit();
                    bi1.UriSource = new Uri("error.png", UriKind.Relative);
                    bi1.EndInit();
                    StatIcon.Source = bi1;

                }
                txtMs.Text = millisec + " ms";
                this.Cursor = Cursors.Arrow;
            }
            catch(System.Net.NetworkInformation.PingException)
            {
                MessageBox.Show("L'indirizzo IP non risulta essere valido");
                return;
            }
        }


    }
}