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
using System.Net.Mail;

namespace LibraryBackgroundService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "LibraryService" in both code and config file together.
    public class LibraryService : ILibraryService
    {
        int eventCount = 0;
        string connectionString = @"Server=localhost;Database=library;Uid=root;Pwd=root;";
        MySqlConnection conn;
        MySqlCommand command;
        public LibraryService()
        {
            conn = new MySqlConnection(connectionString);
            command = conn.CreateCommand();
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

        public void addUserEtudiantInfo(string numCarte, string email, string name, string surname, string specialite, string niv)
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

        //Creates a User
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


        //A booking process
        // 0 -> user in blacklist
        // 1 -> you already took this book
        // 2 -> book not available
        // 3 -> error happened retry
        // 4 -> success
        public int reserver(string idOuvrage,string email)
        {
            String user = "";
            int ok = 0;
            try
            {
                conn.Open();
                command.CommandText = "SELECT idUser FROM `user` WHERE email ='" + email + "'";
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    user = reader["idUser"].ToString();
                }
                reader.Close();
                command.CommandText = "SELECT IF( EXISTS(SELECT idUser FROM blackliste WHERE idUser = " + user + "), 0,1)";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ok = Int32.Parse(reader[0].ToString());
                }
                reader.Close();
                
                if (ok == 1) {
                    command.CommandText = "SELECT IF( EXISTS(SELECT idUser FROM emprunt WHERE idUser = "+user+" AND idOuvrage = "+idOuvrage+"), 1,2)";
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        ok = Int32.Parse(reader[0].ToString());
                    }
                    reader.Close();
                    if (ok == 2)
                    {
                        command.CommandText = "SELECT * FROM `ouvrage` WHERE (nbrExemplaire > nbrExemplaireEmp) AND idOuvrage=" + idOuvrage;
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            ok = 3;
                        }
                        reader.Close();
                        if (ok == 3)
                        {
                            command.CommandText = "INSERT INTO emprunt (idUser, idOuvrage)"
                                        + " values (" + user + ","
                                        + "'" + idOuvrage + "') ";
                            command.ExecuteNonQuery();
                            command.CommandText = " UPDATE ouvrage"
                                                + " SET nbrExemplaireEmp = nbrExemplaireEmp + 1"
                                                + " WHERE idOuvrage = '" + idOuvrage + "'";
                            command.ExecuteNonQuery();

                            command.CommandText = "SELECT eventCount FROM `admin` WHERE adminID = 1";
                            reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                eventCount = Int32.Parse(reader["eventCount"].ToString());
                                eventCount++;
                            }
                            reader.Close();

                            command.CommandText = " CREATE EVENT event_" + eventCount
                                                + " ON SCHEDULE AT CURRENT_TIMESTAMP + INTERVAL 24 HOUR"
                                                + " DO "
                                                    + " CALL checkConfirme('" + email + "'," + idOuvrage + ")";
                            command.ExecuteNonQuery();
                            command.CommandText = "UPDATE `admin` SET eventCount = " + eventCount + " WHERE adminID = 1";
                            command.ExecuteNonQuery();
                            ok = 4;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
            }
            conn.Close();
            return ok;
        }

        //Authentication
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

        //Returns the list of booked books of a certain user
        public List<String[]> reservationList(string email)
        {
            List<string[]> list = new List<string[]>();
            try
            {
                conn.Open();
                command.CommandText = "SELECT idOuvrage FROM `emprunt` WHERE idUser = (SELECT idUser FROM `user` WHERE email ='" + email + "')";
                List<String> idOuvrageList = new List<String>();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    idOuvrageList.Add(reader["idOuvrage"].ToString());
                }
                reader.Close();
                bool v = idOuvrageList.Count() != 0;
                if (v)
                {
                    foreach (var idouvrage in idOuvrageList)
                    {
                        command.CommandText = "SELECT * FROM `ouvrage` WHERE idOuvrage = "+idouvrage;
                        reader = command.ExecuteReader();
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
                        reader.Close();
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            conn.Close();
            return list;
        }

        //Checks if the given user ID (1 -> student or 2 -> teacher) is a valid ID
        public bool checkEtudiantOrEnseignant(string choice, string id)
        {
            conn.Open();
            Boolean ok = false;
            switch (Int32.Parse(choice))
            {
                case 1:
                    {
                        command.CommandText = "SELECT * FROM `etudiantliste` WHERE numeroCarte ='" + id + "'";
                        try
                        {
                            MySqlDataReader reader = command.ExecuteReader();
                            string id2 = "";
                            while (reader.Read())
                            {
                                id2 = reader["numeroCarte"].ToString();
                            }
                            if (id2 == id)
                            {
                                ok = true;
                            }
                            
                        }
                        catch (Exception ex)
                        {
                        }
                        break;
                    }
                case 2:
                    {
                        command.CommandText = "SELECT * FROM `enseignantliste` WHERE matricule ='" + id + "'";
                        try
                        {
                            MySqlDataReader reader = command.ExecuteReader();
                            string id2 = "";
                            while (reader.Read())
                            {
                                id2 = reader["matricule"].ToString();
                            }
                            if (id2 == id)
                            {
                                ok = true;
                            }

                        }
                        catch (Exception ex)
                        {
                        }
                        break;
                    }
                default : break;
            }
            conn.Close();
            return ok;
        }

        //Checks if the given user ID (1 -> student or 2 -> teacher) is used in another account 
        public bool checkEtudiantOrEnseignantAvailable(string choice, string id)
        {
            conn.Open();
            Boolean ok = true;
            switch (Int32.Parse(choice))
            {
                case 1:
                    {
                        command.CommandText = "SELECT numeroCarte FROM `etudiant` WHERE numeroCarte ='" + id + "'";
                        try
                        {
                            MySqlDataReader reader = command.ExecuteReader();
                            string id2 = "";
                            while (reader.Read())
                            {
                                id2 = reader["numeroCarte"].ToString();
                            }
                            if (id2 == id)
                            {
                                ok = false;
                            }

                        }
                        catch (Exception ex)
                        {
                        }
                        break;
                    }
                case 2:
                    {
                        command.CommandText = "SELECT matricule FROM `enseignant` WHERE matricule ='" + id + "'";
                        try
                        {
                            MySqlDataReader reader = command.ExecuteReader();
                            string id2 = "";
                            while (reader.Read())
                            {
                                id2 = reader["matricule"].ToString();
                            }
                            if (id2 == id)
                            {
                                ok = false;
                            }

                        }
                        catch (Exception ex)
                        {
                        }
                        break;
                    }
                default: break;
            }
            conn.Close();
            return ok;
        }

        //Checks if the email is used
        public bool checkEmail(string email)
        {
            command.CommandText = "SELECT email FROM `user` WHERE email ='" + email + "'";
            Boolean ok = false;
            try
            {
                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();
                string email2 = "";
                while (reader.Read())
                {
                    email2 = reader["email"].ToString();
                }
                if (email2 == email)
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

        //Checks if the email belongs to a student or a teacher
        public bool isEtudiant(string email)
        {
            String user = "";
            int ok = 0;
            try
            {
                conn.Open();
                command.CommandText = "SELECT idUser FROM `user` WHERE email ='" + email + "'";
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    user = reader["idUser"].ToString();
                }
                reader.Close();
                command.CommandText = " SELECT IF( EXISTS(SELECT idUser FROM etudiant WHERE idUser = "+user+"), 0,1)";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ok = Int32.Parse(reader[0].ToString());
                }
                reader.Close();
            }
            catch (Exception ex)
            {
            }
            if (ok == 0)
                return true;
            else return false;
            }

        //Updates the student's info
        public bool updateUserEtudiantInfo(string numCarte, string name, string surname, string specialite, string niv)
        {
            bool ok = false;
            conn.Open();
            try
            {
                command.CommandText = " UPDATE etudiant"
                                                + " SET nom = '"+surname+"', "
                                                + " prenom = '"+name+"', "
                                                + " specialite = '"+specialite+"', "
                                                + " niveau = '"+niv+"'"
                                                + " WHERE numeroCarte = " + numCarte + "";
                if (command.ExecuteNonQuery() > 0)
                    ok = true;
            }
            catch(Exception ex)
            {

            }
            conn.Close();
            return ok;
        }

        //Updates the teacher's info
        public bool updateUserEnseignantInfo(string matricule, string name, string surname, string niv)
        {
            bool ok = false;
            conn.Open();
            try
            {
                command.CommandText = " UPDATE enseignant"
                                                + " SET nom = '" + surname + "', "
                                                + " prenom = '" + name + "', "
                                                + " grade = " + niv
                                                + " WHERE matricule = " + matricule + "";
                if(command.ExecuteNonQuery()>0)
                    ok= true;
            }
            catch (Exception ex)
            {

            }
            conn.Close();
            return ok;
        }

        //Forgets password
        public bool forgotPassword(string email)
        {
            bool ok = false;
            string password ="";
            conn.Open();
            try
            {
                command.CommandText = " SELECT password FROM `user` WHERE email = '" + email +"'";
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    password = reader["password"].ToString();
                    ok = true;
                }
                reader.Close();
            }
            catch (Exception ex)
            {

            }
            conn.Close();
            if (ok)
            {
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("amarhamlaoui@gmail.com", "password");
                MailMessage mm = new MailMessage("amarhamlaoui@gmail.com", email, "test", password);
                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                client.Send(mm);
            }
            return ok;
        }


    }
}
