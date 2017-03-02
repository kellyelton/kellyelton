namespace KellyElton.ModuleHost.WindowsService
{
    partial class ModuleHostService
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing ) {
            if( disposing && (components != null) ) {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.LogComponent = new KellyElton.ModuleHost.WindowsService.Components.EventLogLogger(this.components);
            this.DownloadFolderClearer = new KellyElton.ModuleHost.WindowsService.Components.PeriodicFolderWiper(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.LogComponent)).BeginInit();
            // 
            // LogComponent
            // 
            this.LogComponent.Log = "KellyElton Module Host";
            this.LogComponent.Source = "ModuleHostService";
            // 
            // DownloadFolderClearer
            // 
            this.DownloadFolderClearer.Enabled = false;
            this.DownloadFolderClearer.FolderPath = "%HOMEPATH%\\Downloads";
            this.DownloadFolderClearer.Interval = 60000D;
            // 
            // ModuleHostService
            // 
            this.ServiceName = "KellyElton ModuleHost";
            ((System.ComponentModel.ISupportInitialize)(this.LogComponent)).EndInit();

        }

        #endregion

        private Components.EventLogLogger LogComponent;
        private Components.PeriodicFolderWiper DownloadFolderClearer;
    }
}
