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
    /// Логика взаимодействия для MessegeboxWindow.xaml
    /// </summary>
    public partial class MessegeboxWindow : Window
    {
        public enum MessageButtons
        {
            Ok,
        }
        public MessegeboxWindow(string Message, MessageButtons Buttons)
        {
            InitializeComponent();
            txtMessage.Text = Message;
            switch (Buttons)
            {
                case MessageButtons.Ok:
                    btnOk.Visibility = Visibility.Visible;

                    break;
            }
        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
