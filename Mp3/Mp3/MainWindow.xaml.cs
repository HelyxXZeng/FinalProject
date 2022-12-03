﻿using System;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MP3_Final
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void heartbtn_Click(object sender, RoutedEventArgs e)
        {
            heartbtn.Foreground = (heartbtn.Foreground != Brushes.DeepPink) ? Brushes.DeepPink : Brushes.White;
        }

        private void pausebtn_Click(object sender, RoutedEventArgs e)
        {
            if (pausebtn.Content == pausebtn.FindResource("Pause"))
            {
                pausebtn.Content = pausebtn.FindResource("Play");
                Storyboard s = (Storyboard)pausebtn.FindResource("stopellipse");
                s.Begin();
                //mediaElement1.Pause();
            }
            else
            {
                pausebtn.Content = pausebtn.FindResource("Pause");
                Storyboard s = (Storyboard)pausebtn.FindResource("spinellipse");
                s.Begin();
                //mediaElement1.Play();
            }
        }

        private void darkmodeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (darkmodeBtn.Content == darkmodeBtn.FindResource("Light"))
            {
                darkmodeBtn.Content = darkmodeBtn.FindResource("Dark");
            }
            else
            {
                darkmodeBtn.Content = darkmodeBtn.FindResource("Light");
            }
        }
    }
}