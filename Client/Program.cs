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
            proxy.reserver(5, "amarhamlaoui@gmail.com");
            Console.WriteLine("done!");
            Console.ReadLine();
        }
    }
}
