using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace LibraryBackgroundService
{
    public partial class LibraryBackgroundService : ServiceBase
    {
        ServiceHost host;
        public LibraryBackgroundService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (host != null)
            {
                host.Close();
            }
            host = new ServiceHost(typeof(LibraryService));
            host.Open();
            TcpChannel chnl = new TcpChannel(1234);
            ChannelServices.RegisterChannel(chnl, false);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(LibraryAdministrationService),
            "objLibraryAdministration", WellKnownObjectMode.Singleton);

        }

        protected override void OnStop()
        {
            if (host != null)
            {
                host.Close();
                host = null;
            }
        }
    }
}
