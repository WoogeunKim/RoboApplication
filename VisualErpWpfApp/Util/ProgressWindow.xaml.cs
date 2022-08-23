using DevExpress.Xpf.Core;
using System;
using System.Windows;

namespace AquilaErpWpfApp3
{
    public partial class ProgressWindow : Window, ISplashScreen {
        public ProgressWindow() {
            InitializeComponent();
            this.board.Completed += OnAnimationCompleted;
        }

        void ISplashScreen.CloseSplashScreen() {
            this.board.Begin(this);
        }

        void ISplashScreen.Progress(double value) {
        }

        void ISplashScreen.SetProgressState(bool isIndeterminate) {
        }
        #region Event Handlers
        void OnAnimationCompleted(object sender, EventArgs e) {
            this.board.Completed -= OnAnimationCompleted;
            this.Close();
        }
        #endregion
    }
}
