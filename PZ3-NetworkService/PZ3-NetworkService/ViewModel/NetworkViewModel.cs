using PZ3_NetworkService.Model;
using PZ3_NetworkService.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PZ3_NetworkService.ViewModel
{
    public class NetworkViewModel : BindableBase
    {
        public static ObservableCollection<Model.WaterMachine> Drag_and_drop_lista { get; set; }

        public DataIO serializer = new DataIO();
        public MyICommand<ListView> DragAndDropCommand1 { get; set; }           //komanda za prevlacenje
        public MyICommand DragAndDropCommand2 { get; set; }                     //komanda za prevlacenje
        public MyICommand<Canvas> DragAndDropCommand3 { get; set; }             //komanda za prevlacenje


        private WaterMachine draggedItem = new WaterMachine();                  //objekat koji se prevlaci
        private bool dragging = false;                                          //da li je u togu prevlacenje
        private WaterMachine selectedObject;                                    //koji je selektovan objekat iz listView-a

        

        public NetworkViewModel()
        {
            Drag_and_drop_lista = serializer.DeSerializeObject<ObservableCollection<Model.WaterMachine>>("fajl_sa_podacima.xml");
            draggedItem = new WaterMachine();
            DragAndDropCommand1 = new MyICommand<ListView>(listaDrop_SelectionChanged);
            DragAndDropCommand2 = new MyICommand(listaDrop_MouseLeftButtonUp);
            DragAndDropCommand3 = new MyICommand<Canvas>(Canvas_Drop);
        }
        
        public WaterMachine DraggedItem { get => draggedItem; set => draggedItem = value; }
        public WaterMachine SelectedObject { get => selectedObject; set => selectedObject = value; }

        
        #region metoda vezana za listView
        public void listaDrop_SelectionChanged(ListView listViewObject)
        {
            if (!dragging)                                                                         
            {
                dragging = true;

                for (int i = 0; i < Drag_and_drop_lista.Count; i++)
                {
                    if (Drag_and_drop_lista[i] == SelectedObject)
                    {
                        draggedItem = Drag_and_drop_lista[i];
                    }
                }
                
                if (draggedItem != null)
                {
                DragDrop.DoDragDrop(listViewObject, draggedItem, DragDropEffects.Copy | DragDropEffects.Move);}
            }
        }
        #endregion
        #region Metoda za spustanje misa
        private void listaDrop_MouseLeftButtonUp()
        {
            draggedItem = null;
            SelectedObject = null;
            dragging = false;
        }
        #endregion
        #region METODA ZA CANVAS
        private void Canvas_Drop(Canvas canvas)
        {
            //base.OnDrop(e);
            if (draggedItem != null)
            {
                if ((canvas).Resources["taken"] == null)
                {
                    BitmapImage logo = new BitmapImage();
                    if (draggedItem.Tip_name == "Turbo795")
                    {
                        logo.BeginInit();
                        logo.UriSource = new Uri("C:/Users/Mile/Desktop/MVVM (Mile)/PZ3-NetworkService/PZ3-NetworkService/WaterImages/slika1.jpg", UriKind.Absolute);
                        logo.EndInit();
                    }
                    else if (draggedItem.Tip_name == "Turbo796")
                    {
                        logo.BeginInit();
                        logo.UriSource = new Uri("C:/Users/Mile/Desktop/MVVM (Mile)/PZ3-NetworkService/PZ3-NetworkService/WaterImages/slika2.jpg", UriKind.Absolute);
                        logo.EndInit();
                    }
                    else if (draggedItem.Tip_name == "HidroMer71")
                    {
                        logo.BeginInit();
                        logo.UriSource = new Uri("C:/Users/Mile/Desktop/MVVM (Mile)/PZ3-NetworkService/PZ3-NetworkService/WaterImages/slika3.jpg", UriKind.Absolute);
                        logo.EndInit();
                    }
                    else if (draggedItem.Tip_name == "HidroMer81")
                    {
                        logo.BeginInit();
                        logo.UriSource = new Uri("C:/Users/Mile/Desktop/MVVM (Mile)/PZ3-NetworkService/PZ3-NetworkService/WaterImages/slika4.jpg", UriKind.Absolute);
                        logo.EndInit();
                    }
                    else if (draggedItem.Tip_name == "SuperAqua10")
                    {
                        logo.BeginInit();
                        logo.UriSource = new Uri("C:/Users/Mile/Desktop/MVVM (Mile)/PZ3-NetworkService/PZ3-NetworkService/WaterImages/slika5.jpg", UriKind.Absolute);
                        logo.EndInit();
                    }


                    (canvas).Background = new ImageBrush(logo);         //children je ono sto se nalazi u canvasu
                    if (SelectedObject.Vrednost < 670 || SelectedObject.Vrednost > 735)
                    {
                        ((TextBlock)canvas.Children[0]).Background = Brushes.Red;
                    }
                    else
                    {
                        ((TextBlock)canvas.Children[0]).Background = Brushes.Green;
                    }
                    (canvas).Resources.Add("taken", true);
                }
                draggedItem = null;
                SelectedObject = null;
                dragging = false;
            }
        }
        #endregion
    }
}
