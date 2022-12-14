﻿#pragma checksum "..\..\..\MainWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8211E7EC02DBE7306C1BF1564E85BABEA2D52BD7"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using MP3_Final;
using MP3_Final.UserControls;
using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace MP3_Final {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 15 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid Music_Player;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Playlist;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Favorite;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider sldVolume;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button darkmodeBtn;
        
        #line default
        #line hidden
        
        
        #line 93 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MaterialDesignThemes.Wpf.ColorZone searchBar;
        
        #line default
        #line hidden
        
        
        #line 112 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox searchTB;
        
        #line default
        #line hidden
        
        
        #line 224 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbStart;
        
        #line default
        #line hidden
        
        
        #line 225 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbEnd;
        
        #line default
        #line hidden
        
        
        #line 229 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider slider_seek;
        
        #line default
        #line hidden
        
        
        #line 241 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button shufflebtn;
        
        #line default
        #line hidden
        
        
        #line 245 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button previousbtn;
        
        #line default
        #line hidden
        
        
        #line 249 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button pausebtn;
        
        #line default
        #line hidden
        
        
        #line 269 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button nextbtn;
        
        #line default
        #line hidden
        
        
        #line 273 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button replaybtn;
        
        #line default
        #line hidden
        
        
        #line 277 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button heartbtn;
        
        #line default
        #line hidden
        
        
        #line 297 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Media.RotateTransform MyAnimatedTransform;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/MP3_Final;component/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.Music_Player = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.Playlist = ((System.Windows.Controls.Button)(target));
            return;
            case 3:
            this.Favorite = ((System.Windows.Controls.Button)(target));
            
            #line 36 "..\..\..\MainWindow.xaml"
            this.Favorite.Click += new System.Windows.RoutedEventHandler(this.Favorite_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 53 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.sldVolume = ((System.Windows.Controls.Slider)(target));
            
            #line 56 "..\..\..\MainWindow.xaml"
            this.sldVolume.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.sldVolume_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 61 "..\..\..\MainWindow.xaml"
            ((MaterialDesignThemes.Wpf.PackIcon)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.FolderUpload_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 65 "..\..\..\MainWindow.xaml"
            ((MaterialDesignThemes.Wpf.PackIcon)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.FileUpload_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 8:
            this.darkmodeBtn = ((System.Windows.Controls.Button)(target));
            
            #line 72 "..\..\..\MainWindow.xaml"
            this.darkmodeBtn.Click += new System.Windows.RoutedEventHandler(this.darkmodeBtn_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.searchBar = ((MaterialDesignThemes.Wpf.ColorZone)(target));
            return;
            case 10:
            this.searchTB = ((System.Windows.Controls.TextBox)(target));
            return;
            case 11:
            this.tbStart = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 12:
            this.tbEnd = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 13:
            this.slider_seek = ((System.Windows.Controls.Slider)(target));
            
            #line 229 "..\..\..\MainWindow.xaml"
            this.slider_seek.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.slider_seek_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 14:
            this.shufflebtn = ((System.Windows.Controls.Button)(target));
            
            #line 241 "..\..\..\MainWindow.xaml"
            this.shufflebtn.Click += new System.Windows.RoutedEventHandler(this.shufflebtn_Click);
            
            #line default
            #line hidden
            return;
            case 15:
            this.previousbtn = ((System.Windows.Controls.Button)(target));
            
            #line 245 "..\..\..\MainWindow.xaml"
            this.previousbtn.Click += new System.Windows.RoutedEventHandler(this.previousbtn_Click);
            
            #line default
            #line hidden
            return;
            case 16:
            this.pausebtn = ((System.Windows.Controls.Button)(target));
            
            #line 249 "..\..\..\MainWindow.xaml"
            this.pausebtn.Click += new System.Windows.RoutedEventHandler(this.pausebtn_Click);
            
            #line default
            #line hidden
            return;
            case 17:
            this.nextbtn = ((System.Windows.Controls.Button)(target));
            
            #line 269 "..\..\..\MainWindow.xaml"
            this.nextbtn.Click += new System.Windows.RoutedEventHandler(this.nextbtn_Click);
            
            #line default
            #line hidden
            return;
            case 18:
            this.replaybtn = ((System.Windows.Controls.Button)(target));
            
            #line 273 "..\..\..\MainWindow.xaml"
            this.replaybtn.Click += new System.Windows.RoutedEventHandler(this.replaybtn_Click);
            
            #line default
            #line hidden
            return;
            case 19:
            this.heartbtn = ((System.Windows.Controls.Button)(target));
            
            #line 277 "..\..\..\MainWindow.xaml"
            this.heartbtn.Click += new System.Windows.RoutedEventHandler(this.heartbtn_Click);
            
            #line default
            #line hidden
            return;
            case 20:
            this.MyAnimatedTransform = ((System.Windows.Media.RotateTransform)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

