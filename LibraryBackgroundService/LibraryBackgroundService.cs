using MySql.Data.MySqlClient;
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
using System.Timers;

namespace LibraryBackgroundService
{
    public partial class LibraryBackgroundService : ServiceBase
    {
        ServiceHost host;
        private static System.Timers.Timer aTimer;
        string connectionString = @"Server=localhost;Database=library;Uid=root;Pwd=root;";
        static MySqlConnection conn;
        static MySqlCommand command;
        public LibraryBackgroundService()
        {
            InitializeComponent();
            conn = new MySqlConnection(connectionString);
            command = conn.CreateCommand();
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
            if (aTimer == null)
            {
                System.Timers.Timer aTimer;
                //86400000  > 24h
                aTimer = new System.Timers.Timer(86400000);
                
                aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                aTimer.Enabled = true;
                GC.KeepAlive(aTimer);
            }

        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            conn.Open();
            command.CommandText = "DELETE FROM `blackliste` WHERE DATEDIFF(NOW(),DATE(day))>30";
            command.ExecuteNonQuery();
            conn.Close();
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
