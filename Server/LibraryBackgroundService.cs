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

namespace Server
{
    partial class LibraryBackgroundService : ServiceBase
    {
        public ServiceHost host = null;
        public ServiceHost host2 = null;
        public LibraryBackgroundService()
        {
            InitializeComponent();
        }
        public static void Main()
        {
            ServiceBase.Run(new LibraryBackgroundService());
        }

        protected override void OnStart(string[] args)
        {
            if (host != null && host2 != null)
            {
                host.Close();
                host2.Close();
            }
            using (host = new ServiceHost(typeof(LibraryAdministrationService)))
            using (host2 = new ServiceHost(typeof(LibraryService)))
            {
                host.Open();
                host2.Open();
            }
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
