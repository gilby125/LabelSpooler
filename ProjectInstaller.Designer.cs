namespace LabelSpooler
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione componenti

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.ProcInstall = new System.ServiceProcess.ServiceProcessInstaller();
            this.ServInstall = new System.ServiceProcess.ServiceInstaller();
            // 
            // ProcInstall
            // 
            this.ProcInstall.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.ProcInstall.Password = null;
            this.ProcInstall.Username = null;
            // 
            // ServInstall
            // 
            this.ServInstall.Description = "Database Spool Print Service for Label Printers";
            this.ServInstall.DisplayName = "LabelSpooler";
            this.ServInstall.ServiceName = "LabelSpooler";
            this.ServInstall.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.ServInstall.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.serviceInstaller1_AfterInstall);
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.ProcInstall,
            this.ServInstall});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller ProcInstall;
        private System.ServiceProcess.ServiceInstaller ServInstall;
    }
}