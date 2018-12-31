using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using LibraryInterfaces;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Server
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "LibraryService" in both code and config file together.
    public class LibraryService : ILibraryService
    {
        string connectionString = @"Server=localhost;Database=library;Uid=root;Pwd=root;";
        MySqlConnection conn;
        MySqlCommand command;
        public LibraryService()
        {
            conn = new MySqlConnection(connectionString);
            command = conn.CreateCommand();
        }
      

        public void addUserEnseignantInfo(int matricule, int idUser, string name, string surname, string grade)
        {
            command.CommandText = "INSERT INTO enseignant (matricule,idUser,nom,prenom,grade)"
                                 + " values ('" + matricule + "',"
                                 + "'" + idUser + "',"
                                 + "'" + surname + "',"
                                 + "'" + name + "',"
                                 + "'" + grade + "')";
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

        public void addUserEtudiantInfo(int numCarte, int idUser, string surname, string name, string specialite, string niv)
        {
            command.CommandText = "INSERT INTO etudiant (numeroCarte,idUser,nom,prenom,specialite,niveau)"
                     + " values ('" + numCarte + "',"
                     + "'" + idUser + "',"
                     + "'" + surname + "',"
                     + "'" + name + "',"
                     + "'" + specialite +"',"
                     + "'" + niv + "')";
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

        public List<string[]> bookListAllDisponible()
        {
            command.CommandText = "SELECT idOuvrage,titre FROM `ouvrage` WHERE (nbrExemplaire > nbrExemplaireEmp)";
            List<string[]> list = new List<string[]>();
            try
            {
                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string[] line = new string[2];
                    line[0] = reader["idOuvrage"].ToString();
                    line[1] = reader["titre"].ToString();
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

        public List<string[]> bookListSearch(string i, string search)
        {
            List<string[]> list = new List<string[]>();
            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            int j = Int32.Parse(i);
            switch (j)
            {   //search by title
                case 1: {
                        command.CommandText = "SELECT idOuvrage, titre FROM `ouvrage` WHERE (nbrExemplaire > nbrExemplaireEmp) AND titre = '"+search+"'";
                        MySqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            string[] line = new string[2];
                            line[0] = reader["idOuvrage"].ToString();
                            line[1] = reader["titre"].ToString();
                            list.Add(line);
                        }
                        break; }
                //search by author
                case 2: {
                        command.CommandText = "SELECT idOuvrage, titre FROM `ouvrage` WHERE (nbrExemplaire > nbrExemplaireEmp) AND auteur = '" + search + "'";
                        MySqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            string[] line = new string[2];
                            line[0] = reader["idOuvrage"].ToString();
                            line[1] = reader["titre"].ToString();
                            list.Add(line);
                        }
                        break; }
                //search by theme
                case 3: {
                        command.CommandText = "SELECT idOuvrage, titre FROM `ouvrage` WHERE (nbrExemplaire > nbrExemplaireEmp) AND theme = '" + search + "'";
                        MySqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            string[] line = new string[2];
                            line[0] = reader["idOuvrage"].ToString();
                            line[1] = reader["titre"].ToString();
                            list.Add(line);
                        }
                        break; }
                default: break;
            }
            conn.Close();
            return list;
        }

        public List<string[]> bookListSearchByKeyWords(string search)
        {
            command.CommandText = "SELECT idOuvrage,titre FROM `ouvrage` WHERE (nbrExemplaire > nbrExemplaireEmp) AND MATCH (description) AGAINST('" + search+"')";
            List<string[]> list = new List<string[]>();
            try
            {
                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string[] line = new string[2];
                    line[0] = reader["idOuvrage"].ToString();
                    line[1] = reader["titre"].ToString();
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

        public List<string[]> checkUserEnseignantInfo(int idUser)
        {
            command.CommandText = "SELECT * FROM `enseignant` WHERE idUser = '"+idUser+"'";
            List<string[]> list = new List<string[]>();
            try
            {
                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string[] line = new string[4];
                    line[0] = reader["matricule"].ToString();
                    line[1] = reader["nom"].ToString();
                    line[2] = reader["prenom"].ToString();
                    line[3] = reader["grade"].ToString();
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

        public List<string[]> checkUserEtudiantInfo(int idUser)
        {
            command.CommandText = "SELECT * FROM `etudiant` WHERE idUser='"+idUser+"'";
            List<string[]> list = new List<string[]>();
            try
            {
                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string[] line = new string[5];
                    line[0] = reader["numeroCarte"].ToString();
                    line[1] = reader["nom"].ToString();
                    line[2] = reader["prenom"].ToString();
                    line[3] = reader["specialite"].ToString();
                    line[4] = reader["niveau"].ToString();
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

        public void createUser(string username, string password)
        {
            command.CommandText = "INSERT INTO user (username,password)"
                     + " values ('" + username + "',"
                     + "'" + password + "')";
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

        public void reserver(int idOuvrage,int idUser)
        {
            try
            {
                command.CommandText = "INSERT INTO emprunt (idUser, idOuvrage)"
                                + " values ('" + idUser + "',"
                                + "'" + idOuvrage + "')";
                conn.Open();
                command.ExecuteNonQuery();
                command.CommandText = " UPDATE ouvrage"
                                    + " SET nbrExemplaireEmp = nbrExemplaireEmp + 1"
                                    + " WHERE idOuvrage = '" + idOuvrage + "'";
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            conn.Close();
        }
    }
}
