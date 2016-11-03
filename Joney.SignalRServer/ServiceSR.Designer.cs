namespace Joney.SignalRServer
{
    partial class ServiceSR
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        //private System.ServiceProcess.ServiceProcessInstaller spInstaller;
        //private System.ServiceProcess.ServiceInstaller sInstaller;
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.eventLog = new System.Diagnostics.EventLog();
            ((System.ComponentModel.ISupportInitialize)(this.eventLog)).BeginInit();

            components = new System.ComponentModel.Container();
            this.ServiceName = "SKIMService";
            ((System.ComponentModel.ISupportInitialize)(this.eventLog)).EndInit();
            //this.sInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            //this.spInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            //this.sInstaller = new System.ServiceProcess.ServiceInstaller();
            //this.spInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            //this.spInstaller.Username = null;
            //this.spInstaller.Password = null;
            //this.sInstaller.ServiceName = "ServiceSR";
            //this.sInstaller.Description = "IM SignalR Service.";
            
        }

        #endregion

        private System.Diagnostics.EventLog eventLog;
    }
}
