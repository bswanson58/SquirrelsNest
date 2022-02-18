using System;
using System.ComponentModel;
using SquirrelsNest.Desktop.Platform;
using SquirrelsNest.Desktop.Properties;

namespace SquirrelsNest.Desktop.Views {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        public MainWindow() {
            InitializeComponent();

            Closing += OnWindowClosing;
        }

        private void OnWindowClosing( object ? sender, CancelEventArgs args ) {
            Settings.Default.ApplicationWindowPlacement = this.GetPlacement();
            Settings.Default.Save();
        }

        protected override void OnSourceInitialized( EventArgs e ) {
            base.OnSourceInitialized( e );

            this.SetPlacement( Settings.Default.ApplicationWindowPlacement );
        }
    }
}
