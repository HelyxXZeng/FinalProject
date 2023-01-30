using System;
using System.Collections.Generic;
using System.IO;
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

namespace MP3_Final.UserControls
{
    /// <summary>
    /// Interaction logic for PlaylistCard.xaml
    /// </summary>
    public partial class PlaylistCard : UserControl
    {
        public PlaylistCard()
        {
            InitializeComponent();
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register
            ("Title", typeof(string), typeof(PlaylistCard));

        public BitmapImage PathImage
        {
            get { return (BitmapImage)GetValue(PathImageProperty); }
            set { SetValue(PathImageProperty, value); }
        }

        public static BitmapImage bmI = new BitmapImage(new Uri(Directory.GetParent(Directory.GetParent(Directory.GetParent(Environment.CurrentDirectory).ToString()).ToString()) + @"\Images\folderSpotify.png"));

        public static readonly DependencyProperty PathImageProperty = DependencyProperty.Register
            ("PathImage", typeof(BitmapImage), typeof(PlaylistCard), new PropertyMetadata(bmI));

        public static readonly RoutedEvent ClickOpenEvent = EventManager.RegisterRoutedEvent(
        "ClickOpen", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PlaylistCard));

        public event RoutedEventHandler ClickOpen
        {
            add { AddHandler(Button.ClickEvent, value); }
            remove { AddHandler(Button.ClickEvent, value); }
        }
    }
}
