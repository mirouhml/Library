using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace BackgroundLibraryService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(LibraryAdministrationService)))
            using (ServiceHost host2 = new ServiceHost(typeof(LibraryService)))
            {
                host.Open();
                host2.Open();
            }
        }

        protected override void OnStop()
        {
        }
    }
}
