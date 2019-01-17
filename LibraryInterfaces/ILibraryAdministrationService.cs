using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace LibraryInterfaces
{
    public interface ILibraryAdministrationService
    {
        void addBook(string titre, string auteur, string theme, int nbrExemplaire, string description);
        List<string[]> bookListAll();
        int reserver(int choice, int idOuvrage, int id);
        int confirmer(int choice, int idOuvrage, int id);
        int remise(int choice, int idOuvrage, int id);
        bool login(string email, string password);
    }
}
