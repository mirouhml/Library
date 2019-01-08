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
        void reserver(int idOuvrage, string email);
        void confirmer(int idOuvrage, string email);
        void remise(int idOuvrage, string email);
    }
}
