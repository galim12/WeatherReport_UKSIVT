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
using System.Windows.Shapes;

namespace WeatherReport_UKSIVT
{
    /// <summary>
    /// Логика взаимодействия для FavouritesSpisok.xaml
    /// </summary>
    public partial class FavouritesSpisok : Window
    {
        public FavouritesSpisok()
        {
            InitializeComponent();
            // Создаем список данных
            List<MyData> dataList = new List<MyData>();

            // Добавляем элементы в список
            dataList.Add(new MyData { IconSource = "/Images/sun.png", ButtonContent = "Уфа", DeleteSource= "/Images/delete_city.png" });
            dataList.Add(new MyData { IconSource = "/Images/dir.png", ButtonContent = "Москва", DeleteSource = "/Images/delete_city.png" });
            dataList.Add(new MyData { IconSource = "/Images/rain.png", ButtonContent = "Казань", DeleteSource = "/Images/delete_city.png" });
            dataList.Add(new MyData { IconSource = "/Images/icon2.png", ButtonContent = "Питер", DeleteSource = "/Images/delete_city.png" });
            dataList.Add(new MyData { IconSource = "/Images/storm.png", ButtonContent = "Хабаровск", DeleteSource = "/Images/delete_city.png" });
            dataList.Add(new MyData { IconSource = "/Images/snow.png", ButtonContent = "Оренбург", DeleteSource = "/Images/delete_city.png" });
            dataList.Add(new MyData { IconSource = "/Images/icon2.png", ButtonContent = "Москва", DeleteSource = "/Images/delete_city.png" });
            dataList.Add(new MyData { IconSource = "/Images/icon2.png", ButtonContent = "Стерлитамак", DeleteSource = "/Images/delete_city.png" });

            // Устанавливаем список данных в качестве источника данных для ListView
            listView.ItemsSource = dataList;
        }
        private void CloseImage_MouseEnter(object sender, MouseEventArgs e)
        {
            Image image = sender as Image;
            if (image != null)
            {
                image.Source = new BitmapImage(new Uri("/Images/close_dark.png", UriKind.Relative));
            }
        }

        private void CloseImage_MouseLeave(object sender, MouseEventArgs e)
        {
            Image image = sender as Image;
            if (image != null)
            {
                image.Source = new BitmapImage(new Uri("/Images/close_white.png", UriKind.Relative));
            }
        }


        private void CloseImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void textSearch_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        public class MyData
        {
            public string IconSource { get; set; }
            public string ButtonContent { get; set; }
            public string DeleteSource { get; set; }
        }

    }
}

        
   
