using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using LibraryInterfaces;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Threading;

namespace LibraryBackgroundService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "LibraryService" in both code and config file together.
    public class LibraryService : ILibraryService
    {
        int eventCount = 0;
        string connectionString = @"Server=localhost;Database=library;Uid=root;Pwd=root;";
        MySqlConnection conn;
        MySqlCommand command;
        private List<Timer> timerList;
        public LibraryService()
        {
            conn = new MySqlConnection(connectionString);
            command = conn.CreateCommand();
            timerList = new List<Timer>();
        }
      

        public void addUserEnseignantInfo(string matricule, string email, string name, string surname, string grade)
        {
            
            command.CommandText = "INSERT INTO enseignant (matricule,idUser,nom,prenom,grade)"
                                 + " values ('" + matricule + "',"
                                 + "(SELECT idUser FROM `user` WHERE email ='" + email + "'),"
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

        public void addUserEtudiantInfo(string numCarte, string email, string surname, string name, string specialite, string niv)
        {
            command.CommandText = "INSERT INTO etudiant (numeroCarte,idUser,nom,prenom,specialite,niveau)"
                     + " values ('" + numCarte + "',"
                     + "(SELECT idUser FROM `user` WHERE email ='" + email + "'),"
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
            command.CommandText = "SELECT * FROM `ouvrage` WHERE (nbrExemplaire > nbrExemplaireEmp)";
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
                        command.CommandText = "SELECT * FROM `ouvrage` WHERE (nbrExemplaire > nbrExemplaireEmp) AND `titre` LIKE '%" + search + "%'";
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
                        break; }
                //search by author
                case 2: {
                        command.CommandText = "SELECT * FROM `ouvrage` WHERE (nbrExemplaire > nbrExemplaireEmp)  AND auteur LIKE '%" + search + "%'";
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
                        break; }
                //search by theme
                case 3: {
                        command.CommandText = "SELECT * FROM `ouvrage` WHERE (nbrExemplaire > nbrExemplaireEmp)  AND theme LIKE '%" + search + "%'";
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
                        break; }
                //search by keywords
                case 4:
                    {
                        command.CommandText = "SELECT * FROM `ouvrage` WHERE (nbrExemplaire > nbrExemplaireEmp) AND MATCH (description) AGAINST('" + search + "')";
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
                        break;
                    }
                default: break;
            }
            conn.Close();
            return list;
        }

        public List<string[]> checkUserEnseignantInfo(string email)
        {
            command.CommandText = "SELECT * FROM `enseignant` WHERE idUser = (SELECT idUser FROM `user` WHERE email ='" + email+"')";
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

        public List<string[]> checkUserEtudiantInfo(string email)
        {
            command.CommandText = "SELECT * FROM `etudiant` WHERE idUser = (SELECT idUser FROM `user` WHERE email ='" + email + "')";
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

        public void createUser(string email, string password)
        {
            command.CommandText = "INSERT INTO user (email,password)"
                     + " values ('" + email + "',"
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

        public void reserver(string idOuvrage,string email)
        {
            eventCount++;
            try
            {
                command.CommandText = "INSERT INTO emprunt (idUser, idOuvrage)"
                                + " values ((SELECT idUser FROM `user` WHERE email ='" + email + "'),"
                                + "'" + idOuvrage + "')";
                conn.Open();
                command.ExecuteNonQuery();
                command.CommandText = " UPDATE ouvrage"
                                    + " SET nbrExemplaireEmp = nbrExemplaireEmp + 1"
                                    + " WHERE idOuvrage = '" + idOuvrage + "'";
                command.ExecuteNonQuery();
                command.CommandText = " CREATE EVENT event_"+eventCount
                                    + " ON SCHEDULE AT CURRENT_TIMESTAMP + INTERVAL 24 HOUR"
                                    + " DO "
                                        + " CALL checkConfirme('"+email+"',"+idOuvrage+")";
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            conn.Close();
        }

        public bool login(string email, string password)
        {
            command.CommandText = "SELECT email,password FROM `user` WHERE email ='" + email + "'";
            Boolean ok = false;
            try
            {
                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();
                string email2 = "";
                string password2 = "";
                while (reader.Read())
                {
                    email2 = reader["email"].ToString();
                    password2 = reader["password"].ToString();
                }
                if (email2 == email && password2 == password)
                {
                    ok = true;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            conn.Close();
            return ok;
        }

        public List<String[]> reservationList(string idOuvrage, string email)
        {
            command.CommandText = "SELECT * FROM `ouvrage` WHERE (nbrExemplaire > nbrExemplaireEmp)";
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
