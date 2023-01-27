using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using MP3_Final.UserControls;
using MP3_Final.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using TagLib;
using TagLib.Aac;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using File = System.IO.File;
using MessageBox = System.Windows.MessageBox;
using Path = System.IO.Path;

namespace MP3_Final
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MediaPlayer media = new MediaPlayer();
        string fileName = string.Empty, path = string.Empty;

        List<Song> songs = new List<Song>();
        // dùng cho shuffle
        List<Song> subSongs = new List<Song>();
        // dùng cho search
        List<Song> search = new List<Song>();

        string current; // bài hát đang được phát
        public static string currentPlaylist = string.Empty;

        int i = 0; // bien toan cuc chi vi tri bai hat trong playlist
        public static string tail = @".txt";
        public static string head = Directory.GetParent(Directory.GetParent(Directory.GetParent(Environment.CurrentDirectory).ToString()).ToString()) + @"\PlayList\";
        string fav = head + @"Favorite.txt";
        string history = head + @"History.txt";
        DispatcherTimer timer;
        bool shuffleMedia = false;

        //
        public static string headCard = Directory.GetParent(Directory.GetParent(Directory.GetParent(Environment.CurrentDirectory).ToString()).ToString()) + @"\LocalFiles\";
        string localfilesPath = headCard + "Local Files.txt";
        PlaylistsView plViewUC = new PlaylistsView();
        songLyricView slrViewUC = new songLyricView();
        //


        public MainWindow()
        {
            InitializeComponent();
            if (!System.IO.Directory.Exists(head))
            {
                System.IO.Directory.CreateDirectory(head);
            }
            if (!System.IO.File.Exists(fav))
            {
                using (System.IO.File.Create(fav)) ;
            }
            if (!System.IO.File.Exists(history))
            {
                using (System.IO.File.Create(history)) ;
            }

            if (!System.IO.File.Exists(localfilesPath))
            {
                using (System.IO.File.Create(localfilesPath)) ;
            }

            LoadPlayList(head);
            //
            LoadPlaylistUC(headCard);
            LoadPlaylistUC(head);
            //
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick;
            media.MediaOpened += Media_MediaOpened;
            media.MediaEnded += Media_MediaEnded;
            media.MediaEnded += Media_Ended;// them event chay bai tiep theo
        }

        // khi kết thúc bài hát
        bool repeatMedia = false;
        private void Media_MediaEnded(object? sender, EventArgs e)
        {
            if (repeatMedia)
            {
                i--;
                media.Position = TimeSpan.Zero;
                media.Play();
            }
            else
            {
                slider_seek.Value = 0;
                tbStart.Text = "00:00";
                media.Pause();
                pausebtn.Content = pausebtn.FindResource("Play");
                Storyboard s = (Storyboard)pausebtn.FindResource("stopellipse");
                s.Begin();
            }
        }

        // load info bài hát lên display
        private void Coverload()
        {
            TagLib.File file;
            file = TagLib.File.Create(current);
            var firstp = file.Tag.Pictures.FirstOrDefault();
            if (firstp != null)
            {
                MemoryStream memoryStream = new MemoryStream(file.Tag.Pictures[0].Data.Data);
                memoryStream.Seek(0, SeekOrigin.Begin);
                if (file.Tag.Pictures != null && file.Tag.Pictures.Length != 0)
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = memoryStream;
                    bitmap.EndInit();

                    img.ImageSource = bitmap;//load hinh
                    staticImage.ImageSource = bitmap; // load hinh len anh nho 
                }
            }
            else
            {
                staticImage.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/Images/m2.png"));
                img.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/Images/m2.png"));
            }
            string tbh = "";
            if (file.Tag.Title == "" || file.Tag.Title == null)
            {
                string[] temp = file.Name.Split("\\");
                tbh = temp[temp.Length - 1].Split(".")[0];
            }
            else tbh = file.Tag.Title;
            string title = "Tên bài hát:" + tbh, album = "Album: " + file.Tag.Album, date = "Năm ra mắt: " + ((file.Tag.Year == 0) ? "" : file.Tag.Year.ToString()),
                kbit = "Bitrate: " + file.Properties.AudioBitrate.ToString() + "kbps";
            string[] artist = file.Tag.Performers, gerne = file.Tag.Genres;
            // infortexblock at sliderbar section
            titleTxtBlock.Text = tbh;
            singerTxtBlock.Text = "";
            for (int i = 0; i < artist.Count(); i++)
            {
                singerTxtBlock.Text += artist[i];
                if (i > 0 && i < artist.Count() - 1) infotextblock.Text += ",";
            }
            // infortextblock at left section 
            infotextblock.Text = title + "\n" + album + "\nCa sĩ: ";
            for (int i = 0; i < artist.Count(); i++)
            {
                infotextblock.Text += artist[i];
                if (i > 0 && i < artist.Count() - 1) infotextblock.Text += ",";
            }
            infotextblock.Text += "\nThể loại: ";
            for (int i = 0; i < gerne.Count(); i++)
            {
                infotextblock.Text += gerne[i];
                if (i > 0 && i < gerne.Count() - 1) infotextblock.Text += ",";
            }
            infotextblock.Text += "\n" + kbit;
        }

        // khi bài hát được mở
        private void Media_MediaOpened(object? sender, EventArgs e)
        {
            ActiveSongName(i);
            tbEnd.Text = string.Format("{0}", media.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
            TimeSpan ts = media.NaturalDuration.TimeSpan;
            slider_seek.Maximum = ts.TotalSeconds;
            timer.Start();
            pausebtn.Content = pausebtn.FindResource("Pause");
            Coverload();
            Storyboard s = (Storyboard)pausebtn.FindResource("spinellipse");
            s.Begin();
            string[] files = System.IO.File.ReadAllLines(fav);
            foreach (string file in files)
            {
                if (current == file)
                {
                    heartbtn.Foreground = Brushes.DeepPink;
                    return;
                }
            }
            heartbtn.Foreground = Brushes.White;
        }

        // timer cho bài hát
        private void Timer_Tick(object? sender, EventArgs e)
        {
            slider_seek.Value = media.Position.TotalSeconds;
            tbStart.Text = string.Format("{0}", media.Position.ToString(@"mm\:ss"));
        }

        // function khi bấm yêu thích
        private void heartbtn_Click(object sender, RoutedEventArgs e)
        {
            if (heartbtn.Foreground == Brushes.White)
            {
                System.IO.File.AppendAllText(fav, songs[i].path + "\n");
            }
            else System.IO.File.WriteAllLines(fav, System.IO.File.ReadLines(fav).Where(l => l != songs[i].path).ToList());
            heartbtn.Foreground = (heartbtn.Foreground != Brushes.DeepPink) ? Brushes.DeepPink : Brushes.White;
        }

        // dừng bài hát
        private void pausebtn_Click(object sender, RoutedEventArgs e)
        {
            if (media.Source != null)
            {
                if (pausebtn.Content == pausebtn.FindResource("Pause"))
                {
                    pausebtn.Content = pausebtn.FindResource("Play");
                    Storyboard s = (Storyboard)pausebtn.FindResource("stopellipse");
                    s.Begin();
                    media.Pause();
                }
                else
                {
                    pausebtn.Content = pausebtn.FindResource("Pause");
                    Storyboard s = (Storyboard)pausebtn.FindResource("spinellipse");
                    s.Begin();
                    media.Play();
                }
            }
        }

        // add songname to playlist table
        private void Add_UcSongName(Song song, int index)
        {
            SongName uc = new SongName();
            songMenu.Children.Add(uc);
            uc.Path = song.path;
            TagLib.File file = TagLib.File.Create(song.path);
            int temp = songMenu.Children.IndexOf(uc) + 1;
            if (temp >= 1 && temp <= 9)
                uc.Number = '0' + temp.ToString();
            else
                uc.Number = temp.ToString();
            //code fix title begin
            string tbh = "";
            if (file.Tag.Title == "" || file.Tag.Title == null)
            {
                tbh = System.IO.Path.GetFileNameWithoutExtension(file.Name);
            }
            else tbh = file.Tag.Title;
            uc.Title = tbh;
            // code fix title end 
            string[] artist = file.Tag.Performers;
            string listSinger = "";
            for (int i = 0; i < artist.Count(); i++)
            {
                listSinger += artist[i];
                if (i > 0 && i < artist.Count() - 1) listSinger += ",";
            }
            uc.Singer = listSinger;
            TimeSpan t = new TimeSpan();
            t = file.Properties.Duration;
            uc.Time = string.Format("{0}", t.ToString(@"mm\:ss"));
            uc.IndexOfSong = songMenu.Children.IndexOf(uc);
            uc.MLeftBtnD_BdSongName += Uc_MLeftBtnD_BdSongName;
            uc.DeleteClick += new Action<object>(OnDelete);
        }

        // ấn delete ở popup
        private void OnDelete(object sender)
        {
            SongName uc = (SongName)sender;
            songMenu.Children.Remove(uc);
            //Nếu xóa bài đang phát thì dừng bài đó
            if (i == uc.IndexOfSong)
            {
                media.Stop();
                Storyboard s = (Storyboard)pausebtn.FindResource("stopellipse");
                s.Begin();
                infotextblock.Text = "";
            }
            for (int i = 0; i < songs.Count; i++)
            {
                if (songs[i].path == uc.Path)
                {
                    songs.RemoveAt(i);
                    break;
                }

            }
            for (int i = 0; i < subSongs.Count; i++)
            {
                if (subSongs[i].path == uc.Path)
                {
                    subSongs.RemoveAt(i);
                    break;
                }

            }

            for (int i = 0; i < songMenu.Children.Count; i++)
            {
                SongName songname = (SongName)songMenu.Children[i];
                int temp = i + 1;
                if (temp >= 1 && temp <= 9)
                    songname.Number = '0' + temp.ToString();
                else
                    songname.Number = temp.ToString();
                if (!shuffleMedia)
                    for (int j = 0; j < songs.Count; j++)
                    {
                        if (songname.Path == songs[j].path)
                        {
                            songname.IndexOfSong = j;
                            break;
                        }
                    }
                else
                {
                    for (int j = 0; j < subSongs.Count; j++)
                    {
                        if (songname.Path == subSongs[j].path)
                        {
                            songname.IndexOfSong = j;
                            break;
                        }
                    }
                }

            }
        }

        // MouseLeftButtonDown Event of usercontrol songname
        private void Uc_MLeftBtnD_BdSongName(object sender, RoutedEventArgs e)
        {
            if (onSearch) playSearch = true;
            SongName uc = (SongName)sender;
            current = uc.Path;
            try
            {
                uc.IsActive = (uc.IsActive == false) ? true : false;
                if (uc.IsActive == true)
                {
                    // gán index của bài hát cho biến toàn cục vị trí bài hát i 
                    i = uc.IndexOfSong;
                    ActiveSongName(i);
                    fileName = uc.Path;
                    media.Open(new Uri(fileName));
                    media.Play();
                    pausebtn.Content = pausebtn.FindResource("Pause");
                    Storyboard s = (Storyboard)pausebtn.FindResource("spinellipse");
                    s.Begin();

                }
                else
                {
                    media.Pause();
                    pausebtn.Content = pausebtn.FindResource("Play");
                    Storyboard s = (Storyboard)pausebtn.FindResource("stopellipse");
                    s.Begin();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        //Change properties of ucSongName fit current playing song
        private void ActiveSongName(int IndexOfCurrentSong)
        {
            SongName CurrentSong = new SongName();
            for (int i = 0; i < songMenu.Children.Count; i++)
            {
                SongName sn = (SongName)songMenu.Children[i];

                if (sn.IndexOfSong == IndexOfCurrentSong)
                {
                    CurrentSong = sn;
                    CurrentSong.IsActive = true;
                }
            }
            for (int i = 0; i < songMenu.Children.Count; i++)
            {
                SongName remainUc = (SongName)songMenu.Children[i];
                if (remainUc.Path != CurrentSong.Path)
                    remainUc.IsActive = false;
            }
        }

        // Song ended
        private void Media_Ended(object sender, EventArgs e)
        {
            i++;
            if (onSearch && playSearch)
            {
                if (i == search.Count) i = 0;
                current = search[i].path;
                media.Stop();
                media.Open(new Uri(search[i].path));
                media.Position = TimeSpan.Zero;// chay nhac tu 00:00
                media.Play();
                return;
            }
            if (i == songs.Count)
            {
                i = 0;
            }
            media.Stop();
            if (!shuffleMedia)
            {
                current = songs[i].path;
                media.Open(new Uri(songs[i].path));
            }
            else
            {
                current = subSongs[i].path;
                media.Open(new Uri(subSongs[i].path));
            }
            media.Position = TimeSpan.Zero;// chay nhac tu 00:00
            media.Play();
        }

        // replay button press
        private void replaybtn_Click(object sender, RoutedEventArgs e)
        {
            replaybtn.Foreground = (replaybtn.Foreground != Brushes.DeepPink) ? Brushes.DeepPink : Brushes.White;
            repeatMedia = (repeatMedia == false && replaybtn.Foreground == Brushes.DeepPink) ? true : false;
        }

        // media time slider change
        private void slider_seek_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            media.Position = TimeSpan.FromSeconds(slider_seek.Value);
        }

        // volumn button click for media mute
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sldVolume.Value == 0)

            {
                sldVolume.Value = 1;
            }
            else
            {
                sldVolume.Value = 0;
            }
        }

        // volumn slider change
        private void sldVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            media.Volume = sldVolume.Value;
        }

        // next button click
        private void nextbtn_Click(object sender, RoutedEventArgs e)
        {
            if (onSearch && playSearch)
            {
                if (i < search.Count - 1)
                    ++i;
                else i = 0;
                current = search[i].path;
                media.Stop();
                media.Open(new Uri(search[i].path));
                media.Position = TimeSpan.Zero;// chay nhac tu 00:00
                media.Play();
                return;
            }
            if (songs.Count == 0) { return; }
            if (i < songs.Count - 1)
                ++i;
            else i = 0;
            media.Stop();
            if (!shuffleMedia)
            {
                current = songs[i].path;
                media.Open(new Uri(songs[i].path));
            }
            else
            {
                current = subSongs[i].path;
                media.Open(new Uri(subSongs[i].path));
            }
            getLyric(slrViewUC, current);
            media.Position = TimeSpan.Zero;// chay nhac tu 00:00
            media.Play();
        }

        // favorite button on the left section
        private void Favorite_Click(object sender, RoutedEventArgs e)
        {
            NonePlaylist();
            onSearch = false;
            playSearch = false;
            StreamReader reader = new StreamReader(fav);
            i = 0;
            songs = new List<Song>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                Song song = new Song(line, true);
                songs.Add(song);
            }
            reader.Close();
            songMenu.Children.Clear();
            if (songs.Count == 0) { return; }
            for (int i = 0; i < songs.Count; i++)
            {
                Add_UcSongName(songs[i], i);
            }
        }

        // playlist click on the left section
        private void PlayListClick(object sender, RoutedEventArgs e)
        {
            onSearch = false;
            playSearch = false;
            System.Windows.Controls.Button button = sender as System.Windows.Controls.Button;
            string path = head + button.Content.ToString() + tail;
            currentPlaylist = button.Tag.ToString();
            ChangeColorClickPlayList();
            OpenPlayList(path);
        }

        // highlight the selected playlist
        void ChangeColorClickPlayList()
        {
            var list = listMenu.Children;
            foreach (var item in list)
            {
                System.Windows.Controls.Button button = item as System.Windows.Controls.Button;
                button.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("Transparent");
                if (button.Tag == currentPlaylist)
                {
                    button.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#1aab7a");
                }
            }
        }

        // function to open clicked playlist
        void OpenPlayList(string path)
        {
            if (!System.IO.File.Exists(path))
            {
                using (System.IO.File.Create(path)) ;
            }
            string[] contents = System.IO.File.ReadAllLines(path);
            string line;
            bool heart = false;
            songMenu.Children.Clear();
            songs = new List<Song>();
            subSongs = new List<Song>();

            foreach (var file in contents)
            {
                StreamReader reader = new StreamReader(fav);
                while ((line = reader.ReadLine()) != null)
                {
                    if (line == file)
                    {
                        heart = true;
                        break;
                    }
                }
                reader.Close();
                Song song = new Song(file, heart);
                heart = false;
                songs.Add(song);
                subSongs.Add(song);
                int index = songs.Count - 1;
                Add_UcSongName(songs[index], index);
            }
        }

        // create playlist button on the left section
        private void AddPlayList(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button button = new System.Windows.Controls.Button();
            button.Style = System.Windows.Application.Current.TryFindResource("albumButton") as Style;
            button.Content = "Album mới";
            button.Tag = ButtonToPath(button.Content.ToString());
            button.Click += PlayListClick;
            button.MouseRightButtonDown += ChangePlayListName;
            string path = ButtonToPath(button.Content.ToString());
            if (System.IO.File.Exists(path))
            {
                System.Windows.MessageBox.Show("Album bị trùng tên!");
            }
            else
            {
                using (System.IO.File.Create(path)) ;
                listMenu.Children.Add(button);
            }
        }

        // load all created playlists to the left section
        void LoadPlayList(string path)
        {
            string[] files = Directory.GetFiles(path);
            listMenu.Children.Clear();
            foreach (var file in files)
            {
                if (System.IO.Path.GetFileName(file) != "Favorite.txt" && System.IO.Path.GetFileName(file) != "History.txt")
                {
                    System.Windows.Controls.Button button = new System.Windows.Controls.Button();
                    button.Style = System.Windows.Application.Current.TryFindResource("albumButton") as Style;
                    button.Content = GetFileNameOnly(System.IO.Path.GetFileName(file));
                    button.Click += PlayListClick;
                    button.MouseRightButtonDown += ChangePlayListName;
                    button.Tag = file;
                    listMenu.Children.Add(button);
                }
            }
        }

        // get the media files' name
        public static string GetFileNameOnly(string fullName)
        {
            string res = "";
            foreach (var i in fullName)
            {
                if (i == '.')
                {
                    return res;
                }
                res += i;
            }
            return res;
        }

        // previous button
        private void previousbtn_Click(object sender, RoutedEventArgs e)
        {
            if (onSearch && playSearch)
            {
                if (i < search.Count - 1)
                    ++i;
                else i = 0;
                media.Stop();
                media.Open(new Uri(search[i].path));
                media.Position = TimeSpan.Zero; // chay nhac tu 00:00
                media.Play();
                return;
            }
            if (songs.Count == 0) { return; }
            if (i > 0)
                i--;
            else i = songs.Count - 1;
            media.Stop();
            if (!shuffleMedia)
            {
                current = songs[i].path;
                media.Open(new Uri(songs[i].path));
            }
            else
            {
                current = subSongs[i].path;
                media.Open(new Uri(subSongs[i].path));
            }
            getLyric(slrViewUC, current);
            media.Position = TimeSpan.Zero; // chay nhac tu 00:00
            media.Play();
        }

        // shuffle function
        private void shufflebtn_Click(object sender, RoutedEventArgs e)
        {
            shufflebtn.Foreground = (shufflebtn.Foreground != Brushes.DeepPink) ? Brushes.DeepPink : Brushes.White;
            shuffleMedia = (shuffleMedia == false && shufflebtn.Foreground == Brushes.DeepPink) ? true : false;

            if (shuffleMedia)
            {
                subSongs.Clear();
                foreach (var song in songs)
                {
                    subSongs.Add(song);
                }
                subSongs.Shuffle();
                for (int i = 0; i < songMenu.Children.Count; i++)
                {
                    SongName songname = (SongName)songMenu.Children[i];
                    for (int j = 0; j < subSongs.Count; j++)
                    {
                        if (songname.Path == subSongs[j].path)
                        {
                            songname.IndexOfSong = j;
                            break;
                        }
                    }
                }
                SongName thisSong = (SongName)songMenu.Children[i];
                i = thisSong.IndexOfSong;
                return;
            }
            else
            {
                for (int i = 0; i < songMenu.Children.Count; i++)
                {
                    SongName songname = (SongName)songMenu.Children[i];
                    for (int j = 0; j < songs.Count; j++)
                    {
                        if (songname.Path == songs[j].path)
                        {
                            songname.IndexOfSong = j;
                            break;
                        }
                    }
                }
                for (int n = 0; n < songMenu.Children.Count; n++)
                {
                    SongName songname = (SongName)songMenu.Children[n];
                    if (songname.Path == subSongs[i].path)
                    {
                        i = n;
                        return;
                    }
                }
            }
        }

        private void ChangePlayListName(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.Button button = sender as System.Windows.Controls.Button;
            string old = head + button.Content.ToString() + tail;
            ChangeAlbumName change = new ChangeAlbumName(button.Content.ToString());
            change.ShowDialog();
            button.Content = change.name;
            string newName = ButtonToPath(button.Content.ToString());
            button.Tag = newName;
            System.IO.File.Move(old, newName);
        }

        private void DeletePlayList(object sender, RoutedEventArgs e)
        {
            foreach (var playList in listMenu.Children)
            {
                System.Windows.Controls.Button button = playList as System.Windows.Controls.Button;
                if (button.Tag.ToString() == currentPlaylist)
                {
                    listMenu.Children.Remove(button);
                    System.IO.File.Delete(button.Tag.ToString());
                    return;
                }
            }
        }

        public static string ButtonToPath(string content)
        {
            return head + content + tail;
        }

        private void minimizeBtnClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;

        }

        private void maximizeBtnClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = (this.WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;

        }

        private void closeBtnClick(object sender, RoutedEventArgs e)
        {
            this.Close();

        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        void AddList(string playlist, string path)
        {
            string[] files = System.IO.File.ReadAllLines(playlist);
            foreach (string file in files)
            {
                if (path == file)
                {
                    return;
                }
            }

            if (new FileInfo(playlist).Length != 0)
            {
                System.IO.File.AppendAllText(playlist, "\n");
            }

            System.IO.File.AppendAllText(playlist, path);
        }
        // real load file
        private void LoadFileButton(object sender, RoutedEventArgs e)
        {
            NonePlaylist();
            if (onSearch)
            {
                songMenu.Children.Clear();
                onSearch = false;
                playSearch = false;
            }
            var dialog = new Microsoft.Win32.OpenFileDialog();
            // code open 1 file
            dialog.Multiselect = false;
            dialog.DefaultExt = ".mp3,.flac,.ogg,.wav"; // Default file extension
            dialog.Filter = "All Media Files|*.wav;*.flac;*.aac;*.wma;*.wmv;*.avi;*.m1v;*.mp2;*.mp3;*.mpa;*.mpe;*.m3u;*.mp4;*.mov;*.3g2;*.3gp2;*.3gp;*.3gpp;*.m4a;*.cda;*.aif;*.aifc;*.aiff;*.mid;*.midi;*.rmi;*.mkv;*.WAV;*.AAC;*.WMA;*.WMV;*.AVI;*.M1V;*.MP2;*.MP3;*.MPA;*.MPE;*.M3U;*.MP4;*.MOV;*.3G2;*.3GP2;*.3GP;*.3GPP;*.M4A;*.CDA;*.AIF;*.AIFC;*.AIFF;*.MID;*.MIDI;*.RMI;*.MKV"; // Filter files by extension 

            // ket qua cua dialog
            if (dialog.ShowDialog() == false)
            {
                return;
            }
            AddList(history, dialog.FileName);
            foreach (var s in songs)
            {
                if (s.path == dialog.FileName) return;
            }
            string line;
            bool heart = false;
            StreamReader reader = new StreamReader(fav);
            while ((line = reader.ReadLine()) != null)
            {
                if (line == dialog.FileName)
                {
                    heart = true;
                    break;
                }
            }
            reader.Close();
            Song song = new Song(dialog.FileName, heart);
            songs.Add(song);
            subSongs.Add(song);
            i = songs.Count - 1;
            Add_UcSongName(songs[i], i);
            AddFile(localfilesPath, song.path);
        }
        // real load folder
        private void LoadFolderButton(object sender, RoutedEventArgs e)
        {
            NonePlaylist();
            if (onSearch)
            {
                songMenu.Children.Clear();
                onSearch = false;
                playSearch = false;
            }
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.RootFolder = Environment.SpecialFolder.MyDocuments;//
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.Cancel) return;// thoat va khong thuc hien doan code duoi
                path = dialog.SelectedPath;
                var fileInfos = new DirectoryInfo(path).GetFilesByExtentions(".wav", ".flac", ".aac", ".wma", ".wmv", ".avi", ".m1v", ".mp2", ".mp3", ".mpa", ".mpe", ".m3u", ".mp4", ".mov", ".3g2", ".3gp2", ".3gp", ".3gpp", ".m4a", ".cda", ".aif", ".aifc", ".aiff", ".mid", ".midi", ".rmi", ".mkv", ".WAV", ".AAC", ".WMA", ".WMV", ".AVI", ".M1V", ".MP2", ".MP3", ".MPA", ".MPE", ".M3U", ".MP4", ".MOV", ".3G2", ".3GP2", ".3GP", ".3GPP", ".M4A", ".CDA", ".AIF", ".AIFC", ".AIFF", ".MID", ".MIDI", ".RMI", ".MKV");


                //
                string folderName = Path.GetFileName(path);
                folderName = headCard + folderName + tail;
                string[] folders = Directory.GetFiles(headCard);
                bool dupCheck = false;
                foreach (var fd in folders)
                {
                    if (fd == folderName)
                    {
                        dupCheck = true;
                        break;
                    }
                }
                if (!dupCheck)
                {
                    PlaylistCard plCard = new PlaylistCard();
                    plCard.Title = GetFileNameOnly(System.IO.Path.GetFileName(folderName));
                    plCard.ClickOpen += (sender, e) => OpenPlCard(sender, e, folderName);
                    plViewUC.playlist.Children.Add(plCard);
                    using (System.IO.File.Create(folderName)) ;
                }
                //


                string line;
                bool heart = false;
                foreach (FileInfo fil in fileInfos)
                {
                    AddList(history, fil.FullName);
                    bool check = false;
                    foreach (var s in songs)
                    {
                        if (s.path == fil.FullName) { check = true; break; }
                    }
                    if (check) continue;
                    StreamReader reader = new StreamReader(fav);
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line == fil.FullName)
                        {
                            heart = true;
                            break;
                        }
                    }
                    reader.Close();
                    Song song = new Song(fil.FullName, heart);
                    heart = false;
                    songs.Add(song);
                    subSongs.Add(song);
                    int index = songs.Count - 1;
                    Add_UcSongName(songs[index], index);
                    AddFile(folderName, song.path);
                }
            }
        }

        // get lyrics of the song
        songLyricView activeSlv = null;

        private void getLyric(songLyricView slr, string path)
        {
            TagLib.File file;
            file = TagLib.File.Create(path);
            var firstp = file.Tag.Pictures.FirstOrDefault();
            if (firstp != null)
            {
                MemoryStream memoryStream = new MemoryStream(file.Tag.Pictures[0].Data.Data);
                memoryStream.Seek(0, SeekOrigin.Begin);
                if (file.Tag.Pictures != null && file.Tag.Pictures.Length != 0)
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = memoryStream;
                    bitmap.EndInit();

                    slr.PathImage = bitmap;
                }
            }
            slr.Lyric = file.Tag.Lyrics;
        }

        // display lyrics
        private void DisplayLyric(object sender, RoutedEventArgs e)
        {
            try
            {
                getLyric(slrViewUC, current);

                if (activeSlv != null) centerUI.Children.Remove(activeSlv);
                activeSlv = slrViewUC;
                slrViewUC.Close += new Action<object>(CloseLyric);
                Grid.SetColumn(slrViewUC, 0);

                Grid.SetColumnSpan(slrViewUC, 2);

                centerUI.Children.Add(slrViewUC);
            }
            catch { }
        }

        private void CloseLyric(object sender)
        {
            centerUI.Children.Remove((songLyricView)sender);
        }

        // this help search with or without Vietnamese
        public static string RemoveSign(string str)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = str.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }

        // search function
        bool onSearch = false;
        bool playSearch = false;
        private void searchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            onSearch = true;
            string[] keywords = searchTB.Text.Split(' ');
            search.Clear();
            songMenu.Children.Clear();
            StreamReader reader = new StreamReader(history);
            string line;
            string convertedLine;
            while ((line = reader.ReadLine()) != null)
            {
                TagLib.File file = TagLib.File.Create(line);
                string title = file.Tag.Title;
                string[] artists = file.Tag.Performers;
                convertedLine = RemoveSign(line);
                int flag = 0, flagArtist = 0, flagTitle = 0, count = 0;
                foreach (var keyword in keywords)
                {
                    if (keyword != "")
                    {
                        count++;
                        if (System.Text.RegularExpressions.Regex.IsMatch(convertedLine, RemoveSign(keyword), System.Text.RegularExpressions.RegexOptions.IgnoreCase)) { flag++; }
                        if (title != null && System.Text.RegularExpressions.Regex.IsMatch(RemoveSign(title), RemoveSign(keyword), System.Text.RegularExpressions.RegexOptions.IgnoreCase)) { flagTitle++; }
                        foreach (var artist in artists)
                        {
                            if (System.Text.RegularExpressions.Regex.IsMatch(RemoveSign(artist), RemoveSign(keyword), System.Text.RegularExpressions.RegexOptions.IgnoreCase)) { flagArtist++; break; }
                        }
                    }
                }
                if ((flag > count / 2 && flag >= 1) || (flagArtist > count / 2 && flagArtist >= 1) || (flagTitle > count / 2 && flagTitle >= 1))
                {
                    StreamReader reader2 = new StreamReader(fav);
                    string line2;
                    Song song;
                    while ((line2 = reader2.ReadLine()) != null)
                    {
                        if (line == line2)
                        {
                            song = new Song(line, true);
                            break;
                        }
                    }
                    song = new Song(line, false);
                    reader2.Close();
                    search.Add(song);
                }
            }
            reader.Close();
            for (int i = 0; i < search.Count; i++)
            {
                Add_UcSongName(search[i], i);
            }
        }


        PlaylistsView active = null;

        private void DisplayPlaylists(object sender, RoutedEventArgs e)
        {
            if (active != null) centerUI.Children.Remove(active);
            active = plViewUC;
            plViewUC.Close += new Action<object>(ClosePlaylistView);
            Grid.SetColumn(plViewUC, 0);
            Grid.SetColumnSpan(plViewUC, 2);
            centerUI.Children.Add(plViewUC);
            string[] files = Directory.GetFiles(head);

            for (int i = 0; i < files.Length; i++)
            {
                string filepath = GetFileNameOnly(System.IO.Path.GetFileName(files[i]));
                string[] lines = File.ReadAllLines(files[i]);
                for (int j = 0; j < plViewUC.playlist.Children.Count; j++)
                {
                    PlaylistCard plCard = (PlaylistCard)plViewUC.playlist.Children[j];
                    if (filepath == plCard.Title.ToString())
                    {
                        if (lines.Length == 0)
                        {
                            BitmapImage bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            string urisource = Directory.GetParent(Directory.GetParent(Directory.GetParent(Environment.CurrentDirectory).ToString()).ToString()) + "/Images/m2.png";
                            bitmap.UriSource = new Uri(urisource);
                            bitmap.EndInit();
                            plCard.PathImage = bitmap;
                        }
                        else
                        {
                            TagLib.File file = TagLib.File.Create(lines[0]);
                            var firstp = file.Tag.Pictures.FirstOrDefault();
                            if (firstp != null)
                            {
                                MemoryStream memoryStream = new MemoryStream(file.Tag.Pictures[0].Data.Data);
                                memoryStream.Seek(0, SeekOrigin.Begin);
                                if (file.Tag.Pictures != null && file.Tag.Pictures.Length != 0)
                                {
                                    BitmapImage bitmap = new BitmapImage();
                                    bitmap.BeginInit();
                                    bitmap.StreamSource = memoryStream;
                                    bitmap.EndInit();

                                    plCard.PathImage = bitmap;
                                }
                            }
                        }

                    }
                }
            }
        }

        // Load playlists to left section
        private void LoadPlaylistUC(string path)
        {
            string[] files = Directory.GetFiles(path);
            if (System.IO.Path.GetFileName(path) == "LocalFiles")
                plViewUC.playlist.Children.Clear();
            foreach (var file in files)
            {
                PlaylistCard plCard = new PlaylistCard();
                plCard.Title = GetFileNameOnly(System.IO.Path.GetFileName(file));
                plViewUC.playlist.Children.Add(plCard);
                plCard.ClickOpen += (sender, e) => OpenPlCard(sender, e, file);
            }
        }


        private void OpenPlCard(object sender, RoutedEventArgs e, string Path)
        {
            OpenPlayList(Path);
            ClosePlaylistView(plViewUC);
        }

        private void ClosePlaylistView(object sender)
        {
            centerUI.Children.Remove((PlaylistsView)sender);
        }

        // upload file function
        private void AddFile(string Playlistpath, string songPath)
        {
            if (Playlistpath != null)
            {
                string[] files = System.IO.File.ReadAllLines(Playlistpath);
                foreach (string file in files)
                {
                    if (songPath == file)
                    {
                        return;
                    }
                }

                if (new FileInfo(Playlistpath).Length != 0)
                {
                    System.IO.File.AppendAllText(Playlistpath, "\n");
                }
                System.IO.File.AppendAllText(Playlistpath, songPath);
            }
        }
        // Home button on the left section
        private void Home_Click(object sender, RoutedEventArgs e)
        {
            NonePlaylist();
            onSearch = false;
            playSearch = false;
            OpenPlayList(head + @"History" + tail);
        }
        // revert non-selected playlists
        void NonePlaylist()
        {
            var list = listMenu.Children;
            foreach (var item in list)
            {
                System.Windows.Controls.Button button = item as System.Windows.Controls.Button;
                button.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("Transparent");
            }
        }

        private void darkmodeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (darkmodeBtn.Content == darkmodeBtn.FindResource("Light"))
            {
                darkmodeBtn.Content = darkmodeBtn.FindResource("Dark");
                mainBorder.Background = Brushes.Black;
                searchBar.Background = (Brush)new BrushConverter().ConvertFrom("#ffffff");
                searchTB.Foreground = Brushes.Black;
                menuBorder.Background = this.FindResource("BlackBgrMenu") as LinearGradientBrush;
                CenterBorder.Background = this.FindResource("BlackBgrCenter") as LinearGradientBrush;
                PlayerBorder.Background = this.FindResource("BlackBgrPlayer") as LinearGradientBrush;
                BorderPopupMoreOption.Background = this.FindResource("BlackBgrMenu") as LinearGradientBrush;
                singerTxtBlock.Foreground = (Brush)new BrushConverter().ConvertFrom("#6e6e6e");
                plViewUC.border.Background = this.FindResource("BlackBgrCenter") as LinearGradientBrush;
                slrViewUC.border.Background = this.FindResource("BlackBgrCenter") as LinearGradientBrush;
            }
            else
            {
                darkmodeBtn.Content = darkmodeBtn.FindResource("Light");
                mainBorder.Background = (Brush)new BrushConverter().ConvertFrom("#18c274");
                searchBar.Background = (Brush)new BrushConverter().ConvertFrom("#97e6c1");
                searchTB.Foreground = Brushes.Black;
                menuBorder.Background = this.FindResource("greenBgr1") as LinearGradientBrush;
                CenterBorder.Background = this.FindResource("greenBgr1") as LinearGradientBrush;
                PlayerBorder.Background = this.FindResource("greenBgr2") as LinearGradientBrush;
                BorderPopupMoreOption.Background = this.FindResource("greenBgr1") as LinearGradientBrush;
                singerTxtBlock.Foreground = (Brush)new BrushConverter().ConvertFrom("#9ae5c3");
                plViewUC.border.Background = this.FindResource("greenBgr1") as LinearGradientBrush;
                slrViewUC.border.Background = this.FindResource("greenBgr1") as LinearGradientBrush;
            }
        }
    }
}
