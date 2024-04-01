using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WeatherReport_UKSIVT.UserControls
{
    public partial class CardDay : UserControl
    {
        public string Day
        {
            get { return (string)GetValue(DayProperty); }
            set { SetValue(DayProperty, value); }
        }

        public static readonly DependencyProperty DayProperty =
            DependencyProperty.Register("Day", typeof(string), typeof(CardDay), new PropertyMetadata(""));

        public string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(string), typeof(CardDay), new PropertyMetadata(""));

        public string MaxNum
        {
            get { return (string)GetValue(MaxNumProperty); }
            set { SetValue(MaxNumProperty, value); }
        }

        public static readonly DependencyProperty MaxNumProperty =
            DependencyProperty.Register("MaxNum", typeof(string), typeof(CardDay), new PropertyMetadata(""));

        public string MinNum
        {
            get { return (string)GetValue(MinNumProperty); }
            set { SetValue(MinNumProperty, value); }
        }

        public static readonly DependencyProperty MinNumProperty =
            DependencyProperty.Register("MinNum", typeof(string), typeof(CardDay), new PropertyMetadata(""));

        public CardDay()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        // Пример использования метода для изменения внешнего вида при наведении мыши
        private void UserControl_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            cardBorder.BorderBrush = System.Windows.Media.Brushes.LightBlue;
        }

        // Пример использования метода для возврата внешнего вида после завершения наведения мыши
        private void UserControl_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            cardBorder.BorderBrush = System.Windows.Media.Brushes.LightGray;
        }

        public void SetImage(string imagePath)
        {
            iconImage.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
        }
    }
}
