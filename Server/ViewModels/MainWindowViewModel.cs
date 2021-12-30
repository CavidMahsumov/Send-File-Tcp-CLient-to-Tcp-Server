using SendFileClientToServerWithTCP.Additional;
using Server.Command;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Server.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public RelayCommand ShowBtnClick { get; set; }
        public RelayCommand ConnectBtnCLick { get; set; }
        public RelayCommand OpenBtnCommand { get; set; }
        static TcpListener listener = null;
        static BinaryWriter bw = null;
        static BinaryReader br = null;
        public DispatcherTimer dispatcherTimer { get; set; } = new DispatcherTimer();
        public string Path1 { get; set; }
        string t;

        public string ImagePath { get; set; }

        public List<PDF> PDFs { get; set; }

        string text = String.Empty;
        private PDF selecteditempdf;

        public PDF SelectedItemPdf
        {
            get { return selecteditempdf; }
            set { selecteditempdf = value;OnPropertyChanged(); }
        }

        private Images selecteditemimg;

        public Images SelectedItemImg
        {
            get { return selecteditemimg; }
            set { selecteditemimg = value;OnPropertyChanged(); }
        }


        int count = 0;
        public MainWindow MainWindow { get; set; }

        public MainWindowViewModel(MainWindow mainWindow)
        {
            mainWindow.openbtn.IsEnabled = false;
            selecteditempdf = new PDF();
            selecteditemimg = new Images();
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(500);
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Start();
            PDFs = new List<PDF>();
            MainWindow = mainWindow;
            ShowBtnClick = new RelayCommand((sender) =>
            {
                //


            });
            ConnectBtnCLick = new RelayCommand((sender) =>
            {

                Task.Run(() =>
                {
                    var ip = IPAddress.Parse(ConnectHelper.IPAdress);
                    var ep = new IPEndPoint(ip, ConnectHelper.Port);
                    listener = new TcpListener(ep);
                    listener.Start();

                    while (true)
                    {
                        var client = listener.AcceptTcpClient();
                        MessageBox.Show($"{client.Client.RemoteEndPoint} connected");
                        byte[] bytes = new byte[50000000];

                        Task.Run(() =>
                        {

                            var reader = Task.Run(() =>
                            {

                                var stream = client.GetStream();
                                br = new BinaryReader(stream);
                                bw = new BinaryWriter(stream);
                                while (true)
                                {
                                    ++count;

                                    Path1 = br.ReadString();
                                    t = br.ReadString();

                                    if (".pdf".Equals(Path.GetExtension(path: Path1), StringComparison.OrdinalIgnoreCase))
                                    {

                                        App.Current.Dispatcher.Invoke(() =>
                                        {

                                            mainWindow.Listbox.Items.Add(new PDF { ImagePath = "/Images/pdficon.png", PdfPath = Path1, SenderName = $"Sender Name : {t}" });

                                        });

                                    }
                                    else
                                    {
                                        App.Current.Dispatcher.Invoke(() =>
                                        {

                                            mainWindow.ListboxImg.Items.Add(new Images { ImagePath = Path1, SenderName = $"Sender Name : {t}" });


                                        });
                                    }
                                    break;

                                }
                            });
                        });
                    };
                });
            });
            OpenBtnCommand = new RelayCommand((sender) =>
            {
                try
                {


                    if (mainWindow.Listbox.Items.Count > 0&&selecteditempdf!=null)
                    {
                        Process.Start(selecteditempdf.PdfPath);
                        mainWindow.Listbox.SelectedItem = null;
                    }
                    else
                    {
                        Process.Start(selecteditemimg.ImagePath);
                        mainWindow.ListboxImg.SelectedItem = null;
                    }
                    

                   

                }
                catch (Exception ex)
                {
                }

            });
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (MainWindow.Listbox.SelectedItem != null||MainWindow.ListboxImg.SelectedItem!=null)
            {

                MainWindow.openbtn.IsEnabled = true;
            }
        }
    }
};
