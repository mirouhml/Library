using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace LibraryInterfaces
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ILibraryService" in both code and config file together.
    [ServiceContract]
    public interface ILibraryService
    {
        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "bookListAllDisponible")]
        List<string[]> bookListAllDisponible();


        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "bookListSearch/{i}/{search}")]
        List<string[]> bookListSearch(string i, string search);


        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "reserver/{idOuvrage}/{email}")]
        int reserver(string idOuvrage, string email);


        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "createUser/{email}/{password}")]
        void createUser(string email, string password);


        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "login/{email}/{password}")]
        bool login(string email, string password);


        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "addUserEtudiantInfo/{numCarte}/{email}/{name}/{surname}/{specialite}/{niv}")]
        void addUserEtudiantInfo(string numCarte, string email, string name, string surname, string specialite, string niv);


        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "addUserEnseignantInfo/{matricule}/{email}/{name}/{surname}/{grade}")]
        void addUserEnseignantInfo(string matricule, string email, string name, string surname, string grade);

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "updateUserEtudiantInfo/{numCarte}/{name}/{surname}/{specialite}/{niv}")]
        bool updateUserEtudiantInfo(string numCarte, string name, string surname, string specialite, string niv);

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "updateUserEnseignantInfo/{matricule}/{name}/{surname}/{niv}")]
        bool updateUserEnseignantInfo(string matricule, string name, string surname, string niv);

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "checkUserEtudiantInfo/{email}")]
        List<string[]> checkUserEtudiantInfo(string email);


        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "checkUserEnseignantInfo/{email}")]
        List<string[]> checkUserEnseignantInfo(string email);


        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "reservationList/{email}")]
        List<String[]> reservationList(string email);

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "checkEtudiantOrEnseignant/{choice}/{id}")]
        bool checkEtudiantOrEnseignant(string choice, string id);

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "checkEtudiantOrEnseignantAvailable/{choice}/{id}")]
        bool checkEtudiantOrEnseignantAvailable(string choice, string id);

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "checkEmail/{email}")]
        bool checkEmail(string email);

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "isEtudiant/{email}")]
        bool isEtudiant(string email);


        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "forgotPassword/{email}")]
        bool forgotPassword(string email);
    }
}
