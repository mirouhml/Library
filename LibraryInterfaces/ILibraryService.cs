﻿using System;
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
            UriTemplate = "bookListSearchByKeyWords/{search}")]
        List<string[]> bookListSearchByKeyWords(string search);

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "reserver/idOuvrage/idUser")]
        void reserver(int idOuvrage, int idUser);


        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "createUser/{username}/{password}")]
        void createUser(string username, string password);


        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "addUserEtudiantInfo/numCarte/idUser/{name}/{surname}/{specialite}/{niv}")]
        void addUserEtudiantInfo(int numCarte, int idUser, string name, string surname, string specialite, string niv);


        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "addUserEnseignant/matricule/idUser/{name}/{surname}/{grade}")]
        void addUserEnseignantInfo(int matricule, int idUser, string name, string surname, string grade);


        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "checkUserEtudiantInfo/email")]
        List<string[]> checkUserEtudiantInfo(string email);


        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "checkUserEnseignantInfo/email")]
        List<string[]> checkUserEnseignantInfo(string email);
    }
}
