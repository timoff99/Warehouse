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
using System.Net;

using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;

namespace Warehouse.Views.User
{
    /// <summary>
    /// Логика взаимодействия для UserMapPage.xaml
    /// </summary>
    public partial class UserMapPage : UserControl
    {
        public UserMapPage()
        {
            InitializeComponent();
        }


        private void mapView_Loaded(object sender, RoutedEventArgs e)
        {
            mapView.Bearing = 0;
            mapView.MinZoom = 3;
            mapView.MaxZoom = 17;
            // whole world zoom
            mapView.Zoom = 17;
            // lets the map use the mousewheel to zoom
            mapView.MouseWheelZoomType = MouseWheelZoomType.MousePositionAndCenter;
            // lets the user drag the map
            mapView.CanDragMap = true;
            // lets the user drag the map with the left mouse button
            mapView.DragButton = MouseButton.Left;
            mapView.ShowCenter = false;
            GMaps.Instance.Mode = AccessMode.ServerAndCache;
            // choose your provider here
            mapView.MapProvider = GMapProviders.YandexMap;
            mapView.Position = new PointLatLng(53.900834993450694, 27.412874965790175);
            GMapMarker marker = new GMapMarker(new PointLatLng(53.900834993450694, 27.412874965790177));

            marker.Shape = new Image
            {
                Source = new BitmapImage(new Uri(@"C:\Users\User\Pictures\marker1.png")),
                Width = 25,
                Height = 25,
                ToolTip = "Warehouse",
            };
            
           
            mapView.Markers.Add(marker);
        }
    }
}
