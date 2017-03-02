using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KellyElton.ModuleHost.WindowsService.Components
{
    public partial class Timer : Component
    {
        public double Interval {
            get { return _timer.Interval; }
            set { _timer.Interval = value; }
        }

        public bool Enabled {
            get { return _timer.Enabled; }
            set { _timer.Enabled = value; }
        }

        private readonly System.Timers.Timer _timer = new System.Timers.Timer();

        public Timer() : this( null ) { }

        public Timer( IContainer container ) {
            _timer.Elapsed += Timer_Elapsed;
            container?.Add( this );

            InitializeComponent();
        }

        private void Timer_Elapsed( object sender, System.Timers.ElapsedEventArgs e ) {
            OnTick( e );
        }

        public virtual void Start() {
            Enabled = true;
        }

        public virtual void Stop() {
            Enabled = false;
        }

        protected virtual void OnTick( EventArgs args ) {

        }

        protected virtual void OnDisposing( bool disposing ) {
            if( !disposing ) return;

            _timer?.Dispose();
        }
    }
}
