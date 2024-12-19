using System.IO;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace provaPing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Image> ImageArray = new List<Image>();
        List<TextBox> IPAddressArray = new List<TextBox>();
        List<TextBox> StatusArray = new List<TextBox>();
        List<TextBox> MsArray = new List<TextBox>();
        Ping pinger = new Ping();
        int nTabelle = 10;
        int tempoTrascorso = 0;



        public MainWindow()
        {
            InitializeComponent();
            CreoTabelle();
            CaricoIpDaConfig(); // chiamo la funzione per caricare gli ip da config
        }

        int progresso;
        DispatcherTimer Timer = new DispatcherTimer();
        DispatcherTimer Timer2 = new DispatcherTimer();
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            VerificaEAggiungiIPNelFile();


            tempoTrascorso = 0;
            barProgresso.Value = 0;
            barProgresso.Visibility = Visibility.Visible;

            Timer.Tick += new EventHandler(timer_Tick);
            Timer.Interval = TimeSpan.FromSeconds(30);

            Timer2.Tick += new EventHandler(timer_Tick);
            Timer2.Interval = TimeSpan.FromSeconds(1);

            Ping();
            Timer.Start();
            Timer2.Start();

            btnAdd.IsEnabled = false;
            btnDel.IsEnabled = false;
        }

        void timer2_tick(object sender, EventArgs e)
        {
            tempoTrascorso++;
            barProgresso.Value = progresso;  // Aggiorna la barra di progresso

            if (progresso >= 30)
            {
                progresso = 0;
            }

        }


        void timer_Tick(object sender, EventArgs e)
        {
            progresso++;
            barProgresso.Value = progresso;  // Aggiorna la barra di progresso

            if (progresso >= 30)
            {
                progresso = 0;
                Ping();  // Esegui la funzione Ping ogni 5 secondi
            }
        }
        void Ping()
        {
            string ipDaPingare = null;
            for (int i = 0; i < nTabelle; i++)
            {
                ipDaPingare = IPAddressArray[i].Text;
                if (ipDaPingare == " " || ipDaPingare == "" || ipDaPingare == null)
                {
                    return;
                }

                PingReply reply = pinger.Send(ipDaPingare);
                string status = reply.Status.ToString();
                string millisec = reply.RoundtripTime.ToString();
                double millisecNum = reply.RoundtripTime;
                StatusArray[i].Text = status;
                MsArray[i].Text = millisec;
                if (status == "Success" && millisecNum < 50)
                {
                    ImageArray[i].Visibility = Visibility.Visible;
                    ImageArray[i].Source = new BitmapImage(new Uri("/ok.png", UriKind.Relative));

                }
                else if (status == "Success" && millisecNum > 50)
                {
                    ImageArray[i].Visibility = Visibility.Visible;
                    ImageArray[i].Source = new BitmapImage(new Uri("/attention.png", UriKind.Relative));

                }
                else
                {
                    ImageArray[i].Visibility = Visibility.Visible;
                    ImageArray[i].Source = new BitmapImage(new Uri("/error.png", UriKind.Relative));

                }
            }
        }
        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            Timer.Stop();
            Timer2.Start();
            barProgresso.Visibility = Visibility.Hidden;
            btnAdd.IsEnabled = true;
            btnDel.IsEnabled = true;
        }


        private void CaricoIpDaConfig()
        {
            try
            {
                StreamReader sr = new StreamReader("config.txt");
                string[] ipDaConfig = File.ReadAllLines("config.txt");



                for (int i = 0; i < ipDaConfig.Length && i < nTabelle; i++)
                {
                    if (!string.IsNullOrWhiteSpace(ipDaConfig[i]))
                    {
                        IPAddressArray[i].Text = ipDaConfig[i];
                    }
                }

            }
            catch (System.IO.FileNotFoundException)
            {
                string filePath = "config.txt";
                File.Create(filePath).Dispose();
            }
        }

        private void VerificaEAggiungiIPNelFile()
        {
            // Leggi gli IP attualmente presenti nel file config.txt
            string[] ipDaConfig;
            try
            {
                ipDaConfig = File.ReadAllLines("config.txt");
            }
            catch (FileNotFoundException)
            {
                // Se il file non esiste, creiamo un nuovo file
                string filePath = "config.txt";
                File.Create(filePath).Dispose();
                ipDaConfig = new string[0]; // File appena creato, quindi vuoto
            }

            // Controlliamo se gli IP inseriti manualmente nei TextBox sono già nel file
            for (int i = 0; i < IPAddressArray.Count; i++)
            {
                string ipInserito = IPAddressArray[i].Text;

                if (!string.IsNullOrWhiteSpace(ipInserito) && !ipDaConfig.Contains(ipInserito))
                {
                    // Se l'IP non è nel file, lo aggiungiamo
                    using (StreamWriter sw = File.AppendText("config.txt"))
                    {
                        sw.WriteLine(ipInserito);
                    }
                    MessageBox.Show($"Aggiunto nuovo IP nel file config: {ipInserito}");
                }
            }
        }

        private void CreoTabelle()
        {
            for (int i = 0; i < nTabelle; i++)
            {
                ImageArray.Add(new Image());
                ImageArray[i].Source = new BitmapImage(new Uri("/error.png", UriKind.Relative));
                ImageArray[i].Source = new BitmapImage(new Uri("/attention.png", UriKind.Relative));
                ImageArray[i].Source = new BitmapImage(new Uri("/ok.png", UriKind.Relative));
                Grid.SetRow(ImageArray[i], i + 2);
                Grid.SetColumn(ImageArray[i], 0);
                this.gridPing.Children.Add(ImageArray[i]);
                ImageArray[i].Visibility = Visibility.Hidden;
            }

            //IPAddress array creation
            for (int i = 0; i < nTabelle; i++)
            {
                IPAddressArray.Add(new TextBox());
                IPAddressArray[i].Text = "";
                Grid.SetRow(IPAddressArray[i], i + 2);
                Grid.SetColumn(IPAddressArray[i], 1);
                this.gridPing.Children.Add(IPAddressArray[i]);
            }

            //Status array creation
            for (int i = 0; i < nTabelle; i++)
            {
                StatusArray.Add(new TextBox());
                StatusArray[i].Text = " ";
                StatusArray[i].IsReadOnly = true;
                Grid.SetRow(StatusArray[i], i + 2);
                Grid.SetColumn(StatusArray[i], 2);
                this.gridPing.Children.Add(StatusArray[i]);
            }

            //Ms array creation
            for (int i = 0; i < nTabelle; i++)
            {
                MsArray.Add(new TextBox());
                MsArray[i].Text = " ";
                MsArray[i].IsReadOnly = true;
                Grid.SetRow(MsArray[i], i + 2);
                Grid.SetColumn(MsArray[i], 3);
                this.gridPing.Children.Add(MsArray[i]);
            }
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            nTabelle++;
            cancella();
            CreoTabelle();
            CaricoIpDaConfig();
            VerificaEAggiungiIPNelFile();
        }
        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            nTabelle--;
            cancella();
            CreoTabelle();
            CaricoIpDaConfig();
            VerificaEAggiungiIPNelFile();
        }
        private void cancella()
        {
            for (int i = gridPing.Children.Count - 1; i >= 0; i--)
            {
                var child = gridPing.Children[i];
                if (child is TextBox || child is Image)
                {
                    gridPing.Children.RemoveAt(i);
                }
            }
        }
    }

}