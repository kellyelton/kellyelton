using KellyElton.Logging;
using System;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;

namespace KellyElton.ModuleHost.WindowsService.Components
{
    public partial class PeriodicFolderWiper : Timer
    {
        public string FolderPath { get; set; }
        private ILog Log => LogComponent;

        public PeriodicFolderWiper() : base() {
            InitializeComponent();
        }

        public PeriodicFolderWiper( IContainer container ) : base(container) {
            InitializeComponent();
        }

        protected override void OnTick( EventArgs e ) {
            try {
                Log.Event( "Ticking" );
                var exp = Environment.ExpandEnvironmentVariables( FolderPath );
                var di = new DirectoryInfo( exp );
                WipeDirectory( di );
            } catch (Exception ex ) {
                Log.Error( ex );
            }
        }

        protected bool WipeDirectory( DirectoryInfo dir ) {
            if( dir.CreationTime > DateTime.Now.AddHours( -1 ) ) return false;
            Log.Event( $"Wiping {dir.FullName}" );
            var ret = true;
            foreach( var d in dir.GetDirectories() ) {
                if( WipeDirectory( d ) ) {
                    Log.Event( $"Deleting directory {d.FullName}" );
                    d.Delete();
                } else {
                    ret = false;
                }
            }
            foreach( var f in dir.GetFiles() ) {
                if( f.Extension.Equals( ".crdownload", StringComparison.InvariantCultureIgnoreCase ) )
                    continue;
                if( !WipeFile( f, 5 ) ) {
                    ret = false;
                }
            }
            return ret;
        }
        protected bool WipeFile( FileInfo file, int timesToWrite ) {
            try {
                if( !file.Exists ) return true;
                if( file.CreationTime > DateTime.Now.AddHours( -1 ) ) return false;
                // Set the files attributes to normal in case it's read-only.

                Log.Event( $"Wiping file {file.FullName}" );
                file.Attributes = FileAttributes.Normal;

                // Calculate the total number of sectors in the file.
                double sectors = Math.Ceiling( file.Length / 512.0 );

                // Create a dummy-buffer the size of a sector.

                byte[] dummyBuffer = new byte[512];

                // Create a cryptographic Random Number Generator.
                // This is what I use to create the garbage data.

                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

                // Open a FileStream to the file.
                FileStream inputStream = new FileStream( file.FullName, FileMode.Open );
                for( int currentPass = 0; currentPass < timesToWrite; currentPass++ ) {

                    // Go to the beginning of the stream

                    inputStream.Position = 0;

                    // Loop all sectors
                    for( int sectorsWritten = 0; sectorsWritten < sectors; sectorsWritten++ ) {

                        // Fill the dummy-buffer with random data

                        rng.GetBytes( dummyBuffer );

                        // Write it to the stream
                        inputStream.Write( dummyBuffer, 0, dummyBuffer.Length );
                    }
                }
                inputStream.Flush( true );

                // Truncate the file to 0 bytes.
                // This will hide the original file-length if you try to recover the file.

                inputStream.SetLength( 0 );
                inputStream.Flush( true );

                // Close the stream.
                inputStream.Close();

                // As an extra precaution I change the dates of the file so the
                // original dates are hidden if you try to recover the file.

                DateTime dt = new DateTime( 2037, 1, 1, 0, 0, 0 );
                file.CreationTime = dt;
                file.LastAccessTime = dt;
                file.LastWriteTime = dt;

                // Finally, delete the file

                Log.Event( $"Deleting file {file.FullName}" );

                file.Delete();
                return true;

            } catch( Exception e ) {
                Log.Error( $"Error deleting file {file.FullName}", e );
                return false;
            }
        }
    }
}
