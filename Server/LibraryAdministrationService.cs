using LibraryInterfaces;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Server
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "LibraryAdministrationService" in both code and config file together.
    public class LibraryAdministrationService : ILibraryAdministrationService
    {
        string connectionString = @"Server=localhost;Database=library;Uid=root;Pwd=root;";
        MySqlConnection conn;
        MySqlCommand command;
        public LibraryAdministrationService()
        {
            conn = new MySqlConnection(connectionString);
            command = conn.CreateCommand();
        }
        public void addBook(string titre, string auteur, string theme, int nbrExemplaire, string description)
        {
            command.CommandText = "INSERT INTO ouvrage (titre,auteur,theme,nbrExemplaire,nbrExemplaireEmp,description)"
                                 + " values ('" + titre + "',"
                                 + "'" + auteur + "',"
                                 + "'" + theme + "',"
                                 + "'" + nbrExemplaire + "',"
                                 + "'0',"
                                 + "'" + description + "')";
            try
            {
                conn.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            conn.Close();
        }
        public List<string[]> bookListAll()
        {
            command.CommandText = "SELECT * FROM `ouvrage`";
            List<string[]> list = new List<string[]>();
            try
            {
                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string[] line = new string[7];
                    line[0] = reader["idOuvrage"].ToString();
                    line[1] = reader["titre"].ToString();
                    line[2] = reader["auteur"].ToString();
                    line[3] = reader["theme"].ToString();
                    line[4] = reader["nbrExemplaire"].ToString();
                    line[5] = reader["nbrExemplaireEmp"].ToString();
                    line[6] = reader["description"].ToString();
                    list.Add(line);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            conn.Close();
            return list;
        }
    }
}
