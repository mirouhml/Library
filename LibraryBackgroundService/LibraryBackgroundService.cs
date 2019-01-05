using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Server;

namespace LibraryBackgroundService
{
    public partial class LibraryBackgroundService : ServiceBase
    {
        ServiceHost host;
        ServiceHost host2;
        public LibraryBackgroundService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (host != null && host2 != null)
            {
                host.Close();
                host2.Close();
            }
            host = new ServiceHost(typeof(LibraryAdministrationService));
            host2 = new ServiceHost(typeof(LibraryService));
                host.Open();
                host2.Open();
        }

        protected override void OnStop()
        {
            if (host != null && host2 != null)
            {
                host.Close();
                host2.Close();
                host = null;
                host2 = null;
            }
        }
    }
}
