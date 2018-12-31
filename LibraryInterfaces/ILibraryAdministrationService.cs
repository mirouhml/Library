using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace LibraryInterfaces
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ILibraryAdministrationService" in both code and config file together.
    [ServiceContract]
    public interface ILibraryAdministrationService
    {
        [OperationContract]
        void addBook(string titre, string auteur, string theme, int nbrExemplaire, string description);

        [OperationContract]
        List<string[]> bookListAll();

        [OperationContract]
        void reserver(int idOuvrage, int idUser);

        [OperationContract]
        void confirmer(int idOuvrage, int idUser);
    }
}
