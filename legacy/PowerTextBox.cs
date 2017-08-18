using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace InfoFenix.Application.Code.Controls {

    public class PowerTextBox : TextBox {

        #region Private Fields

        private System.Timers.Timer _timer;

        private int _delay = 2 * 1000; // Default: 2 sec

        #endregion Private Fields

        #region Public Events

        public event EventHandler DelayedTextChanged;

        #endregion Public Events

        #region Public Properties

        [Category("Time")]
        public int Delay {
            get { return _delay; }
            set { _delay = value; }
        }

        #endregion Public Properties

        #region Public Constructors

        public PowerTextBox() {
        }

        #endregion Public Constructors

        #region Protected Override Methods

        protected override void Dispose(bool disposing) {
            if (disposing) {
                if (_timer != null) {
                    _timer.Stop();
                    _timer.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        protected override void OnTextChanged(EventArgs e) {
            InitializeTimer();

            base.OnTextChanged(e);
        }

        #endregion Protected Override Methods

        #region Protected Virtual Methods

        protected virtual void OnDelayedTextChanged(EventArgs e) {
            DelayedTextChanged?.Invoke(this, e);
        }

        #endregion Protected Virtual Methods

        #region Private Methods

        private void InitializeTimer() {
            if (_timer != null) { _timer.Stop(); }

            if (_timer == null || _timer?.Interval != Delay) {
                _timer = new System.Timers.Timer { Interval = Delay };
                _timer.Elapsed += TimerElapsedHandler;
            }

            _timer.Start();
        }

        private void TimerElapsedHandler(object sender, System.Timers.ElapsedEventArgs e) {
            if (sender is System.Timers.Timer timer) {
                timer.Stop();
                OnDelayedTextChanged(EventArgs.Empty);
            }
        }

        #endregion Private Methods
    }
}