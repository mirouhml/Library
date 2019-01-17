using LibraryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpChannel chl = new TcpChannel();
            ChannelServices.RegisterChannel(chl, false);
            ILibraryAdministrationService proxy = (ILibraryAdministrationService)Activator.GetObject(typeof(ILibraryAdministrationService),
                 "tcp://localhost:1234/objLibraryAdministration");
            //Console.WriteLine("Add a book?");
            //Console.WriteLine("titre?");
            //string titre = Console.ReadLine();
            //Console.WriteLine("auteur?");
            //string auteur = Console.ReadLine();
            //Console.WriteLine("theme?");
            //string theme = Console.ReadLine();
            //Console.WriteLine("number of books");
            //int number = Int32.Parse(Console.ReadLine());
            //Console.WriteLine("description");
            //string description = Console.ReadLine();
            //proxy.addBook(titre, auteur, theme, number, description);
            //Console.WriteLine("done!");
            //Console.WriteLine("Press any key to continue");
            //Console.WriteLine("username:");
            //string username = Console.ReadLine();
            //Console.WriteLine("password");
            //string password = Console.ReadLine();
            //proxy.createUser(username, password);
            //List<string[]> list;
            //list = proxy.bookListAll();
            //foreach (var p in list)
            //{
            //    Console.WriteLine(p[0]+" "+p[1]+" " + p[2] + " " + p[3] + " " + p[4] + " " + p[5] + " " + p[6]);
            //}
            //proxy.reserver(2, 1);

            //Console.WriteLine("reserver no user: "+proxy.reserver(2,5,654));
            //Console.WriteLine("reserver 140: " + proxy.reserver(2, 5, 140));
            //Console.WriteLine("reserver 150: " + proxy.reserver(2, 8, 150));
            //Console.WriteLine("confirmer no user: " + proxy.confirmer(2, 6, 12));
            //Console.WriteLine("confirmer amarhml  6: " + proxy.confirmer(2, 6, 150));
            //Console.WriteLine("confirmer amarhml  8: " + proxy.confirmer(2, 8, 150));
            //Console.WriteLine("remise no user: " + proxy.remise(2, 6, 12));
            //Console.WriteLine("remise amarhml 6: " + proxy.remise(2, 6, 150));
            //Console.WriteLine("remise amarhml 8: " + proxy.remise(2, 5, 150));
            //Console.WriteLine("result: " +proxy.login("admin@universite2.dz", "adminadmin"));
            //Console.WriteLine("reserver 1234: " + proxy.reserver(1, 1, 1234));
            //Console.WriteLine("reserver 123: " + proxy.reserver(1, 1, 123));
            Console.WriteLine("remise 123 1: " + proxy.remise(1,1,123));
            Console.WriteLine("done!");
            Console.ReadLine();
        }
    }
}
