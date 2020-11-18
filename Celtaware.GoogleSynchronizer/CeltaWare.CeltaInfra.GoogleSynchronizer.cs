using Celtaware.GoogleSynchronizer.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Celtaware.GoogleSynchronizer
{
    public partial class GoogleSynchronizer : ServiceBase
    {
        private Timer timer1 = null;

        public GoogleSynchronizer()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            int _interval = Properties.Settings.Default.IntervaloAtualizacao;
            _interval *= 60;
            _interval *= 1000;
            timer1 = new Timer();
            this.timer1.Interval = _interval;
            this.timer1.Elapsed += new System.Timers.ElapsedEventHandler(this.timer1_Tick);
            timer1.Enabled = true;
            Logs.WriteLog("Serviço GoogleDrive Synchronizer iniciado");
        }

        private void timer1_Tick(object sender, ElapsedEventArgs e)
        {
            Logs.WriteLog("Verifica lista de diretorio.");
            StringBuilder folders = new StringBuilder();
            IList<string> list = Models.ModelGoogleDrive.GetFolders("GDrive");
            if (list.Count < 1)
            {
                Logs.WriteLog("Não existe diretório novo.");
            }
            else
            {
                foreach (var item in list)
                {
                    folders.AppendLine(item);
                }
                Logs.WriteLog(folders.ToString()); 
            }
        }

        protected override void OnStop()
        {
            timer1.Enabled = false;
            Logs.WriteLog("Serviço GoogleDrive Synchronizer parado.");
        }
    }
}
