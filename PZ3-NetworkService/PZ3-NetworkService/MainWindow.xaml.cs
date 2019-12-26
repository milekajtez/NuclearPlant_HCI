using PZ3_NetworkService.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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

namespace PZ3_NetworkService
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private int count = 15; // Inicijalna vrednost broja objekata u sistemu
                                // ######### ZAMENITI stvarnim brojem elemenata
        private DataIO serializer = new DataIO();               //serijalizacija

        public MainWindow()
        {
            InitializeComponent();
            createListener(); //Povezivanje sa serverskom aplikacijom
        }

        private void createListener()
        {
            var tcp = new TcpListener(IPAddress.Any, 25565);
            tcp.Start();

            var listeningThread = new Thread(() =>
            {
                while (true)
                {
                    var tcpClient = tcp.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(param =>
                    {
                        //Prijem poruke
                        NetworkStream stream = tcpClient.GetStream();
                        string incomming;
                        byte[] bytes = new byte[1024];
                        int i = stream.Read(bytes, 0, bytes.Length);
                        //Primljena poruka je sacuvana u incomming stringu
                        incomming = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                        //Ukoliko je primljena poruka pitanje koliko objekata ima u sistemu -> odgovor
                        if (incomming.Equals("Need object count"))
                        {
                            //Response
                            /* Umesto sto se ovde salje count.ToString(), potrebno je poslati 
                             * duzinu liste koja sadrzi sve objekte pod monitoringom, odnosno
                             * njihov ukupan broj (NE BROJATI OD NULE, VEC POSLATI UKUPAN BROJ)
                             * */

                            
                            Byte[] data = System.Text.Encoding.ASCII.GetBytes(NetworkDataViewModel.Lista_elektarana.Count.ToString());
                            stream.Write(data, 0, data.Length);
                        }
                        else
                        {

                            Byte[] data = System.Text.Encoding.ASCII.GetBytes(NetworkDataViewModel.Lista_elektarana.Count.ToString());
                            stream.Write(data, 0, data.Length);
                            //U suprotnom, server je poslao promenu stanja nekog objekta u sistemu
                            Console.WriteLine(incomming); //Na primer: "Objekat_1:272"

                            //################ IMPLEMENTACIJA ####################
                            // Obraditi poruku kako bi se dobile informacije o izmeni
                            // Azuriranje potrebnih stvari u aplikaciji


                            char[] separators = new char[] { '_', ':' };
                            string[] rez;
                            rez = incomming.Split(separators);
                            int id = -1;
                            id = Int32.Parse(rez[1]);

                            
                            if (NetworkDataViewModel.Lista_elektarana.Count > id)
                            {

                                NetworkDataViewModel.Lista_elektarana[id].Vrednost = Double.Parse(rez[2]);

                                serializer.SerializeObject<ObservableCollection<Model.WaterMachine>>(NetworkDataViewModel.Lista_elektarana, "fajl_sa_podacima.xml");


                                DateTime vreme = DateTime.Now;
                                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"log.txt", true))
                                {
                                    file.WriteLine(String.Format("{0},{1},{2}", vreme, id, rez[2]));
                                }
                            }
                        }
                    }, null);
                }
            });

            listeningThread.IsBackground = true;
            listeningThread.Start();
        }  
    }
}
