using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(LibraryAdministrationService)))
            using (ServiceHost host2 = new ServiceHost(typeof(LibraryService)))
            {
                host.Open();
                host2.Open();
                Console.WriteLine("Server is open");
                Console.WriteLine("Press enter to exit");
                Console.ReadLine();
            }
        }
    }
}
