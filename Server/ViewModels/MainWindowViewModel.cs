using SendFileClientToServerWithTCP.Additional;
using Server.Command;
using Server.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Server.ViewModels
{
    public class MainWindowViewModel
    {
        public RelayCommand ShowBtnClick { get; set; }
        public RelayCommand ConnectBtnCLick { get; set; }
        static TcpListener listener = null;
        static BinaryWriter bw = null;
        static BinaryReader br = null;
        public string Path { get; set; }

        public List<PDF> PDFs { get; set; }

        string text = String.Empty;
        public MainWindowViewModel(MainWindow mainWindow)
        {

            PDFs = new List<PDF>();
            ShowBtnClick = new RelayCommand((sender) =>
            {
                //


            });
            ConnectBtnCLick = new RelayCommand((sender) =>
            {

                Task.Run(() =>
                {
                    var ip = IPAddress.Parse("192.168.1.103");
                    var ep = new IPEndPoint(ip, 27001);
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
                                    bytes = br.ReadBytes(count: 5000000);

                                    text = br.ReadString();
                                    //PDFs.Add(new PDF { Text = PdfHelper.GetTextFromPDF(Path) });
                                    
                                    break;

                                }
                                App.Current.Dispatcher.Invoke(() =>
                                {


                                    mainWindow.Listbox.Items.Add(text);

                                });

                            });



                        });





                    };
                });
            });
        }
    };
};
