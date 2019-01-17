using LibraryInterfaces;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace LibraryBackgroundService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "LibraryAdministrationService" in both code and config file together.
    public class LibraryAdministrationService : MarshalByRefObject, ILibraryAdministrationService
    {
        string connectionString = @"Server=localhost;Database=library;Uid=root;Pwd=root;";
        MySqlConnection conn;
        MySqlCommand command;

        public delegate void remiseEmailSentEventHandler(object source, EmailEventArgs args);
        public event remiseEmailSentEventHandler remiseEmailSent;

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

        // Reservation
        // 0 -> error: couldn't find the user
        // 1 -> error: user in blacklist
        // 2 -> error: user already took a copy of this book
        // 3 -> error: book not available the user was added to the wait list
        // 4 -> error: couldn't do the reservation
        // 5 -> success
        public int reserver(int choice, int idOuvrage, int id)
        {
            string user = "";
            int ok = 0;
            switch (choice)
            {
                case 1:
                    {
                        try
                        {
                            conn.Open();
                            command.CommandText = "SELECT idUser FROM `etudiant` WHERE numeroCarte =" + id;
                            MySqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                user = reader["idUser"].ToString();
                                ok = 1;
                            }
                            reader.Close();
                            if (ok == 1)
                            {
                                command.CommandText = "SELECT IF( EXISTS(SELECT idUser FROM blackliste WHERE idUser = " + user + "), 1,2)";
                                reader = command.ExecuteReader();
                                while (reader.Read())
                                {
                                    ok = Int32.Parse(reader[0].ToString());
                                }
                                reader.Close();
                                if (ok == 2)
                                {
                                    command.CommandText = "SELECT IF( EXISTS(SELECT idUser FROM emprunt WHERE idUser = " + user + " AND idOuvrage = " + idOuvrage + "), 2,3)";
                                    reader = command.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        ok = Int32.Parse(reader[0].ToString());
                                    }
                                    reader.Close();
                                    if (ok == 3)
                                    {
                                        command.CommandText = "SELECT * FROM `ouvrage` WHERE (nbrExemplaire > nbrExemplaireEmp) AND idOuvrage=" + idOuvrage;
                                        reader = command.ExecuteReader();
                                        while (reader.Read())
                                        {
                                            ok = 4;
                                        }
                                        reader.Close();
                                        if (ok == 4)
                                        {
                                            command.CommandText = "INSERT INTO emprunt (idUser, idOuvrage, confirme)"
                                                                   + " values (" + user + ","
                                                                   + "'" + idOuvrage + "', '1')";
                                            if (command.ExecuteNonQuery() >= 1)
                                            {
                                                command.CommandText = " UPDATE ouvrage"
                                                            + " SET nbrExemplaireEmp = nbrExemplaireEmp + 1"
                                                            + " WHERE idOuvrage = '" + idOuvrage + "'";
                                                command.ExecuteNonQuery();
                                                ok = 5;
                                            }
                                        }
                                        else
                                        {
                                            command.CommandText = "INSERT INTO liste_attente (idOuvrage, idUser)"
                                                                + " values (" + idOuvrage + "," + user + ")";
                                            command.ExecuteNonQuery();
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        break;
                    }
                case 2:
                    {
                        try
                        {
                            conn.Open();
                            command.CommandText = "SELECT idUser FROM `enseignant` WHERE matricule ='" + id + "'";
                            MySqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                user = reader["idUser"].ToString();
                                ok = 1;
                            }
                            reader.Close();
                            if (ok == 1)
                            {
                                command.CommandText = "SELECT IF( EXISTS(SELECT idUser FROM blackliste WHERE idUser = " + user + "), 1,2)";
                                reader = command.ExecuteReader();
                                while (reader.Read())
                                {
                                    ok = Int32.Parse(reader[0].ToString());
                                }
                                reader.Close();
                                if (ok == 2)
                                {
                                    command.CommandText = "SELECT IF( EXISTS(SELECT idUser FROM emprunt WHERE idUser = " + user + " AND idOuvrage = " + idOuvrage + "), 2,3)";
                                    reader = command.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        ok = Int32.Parse(reader[0].ToString());
                                    }
                                    reader.Close();
                                    if (ok == 3)
                                    {
                                        command.CommandText = "SELECT * FROM `ouvrage` WHERE (nbrExemplaire > nbrExemplaireEmp) AND idOuvrage=" + idOuvrage;
                                        reader = command.ExecuteReader();
                                        while (reader.Read())
                                        {
                                            ok = 4;
                                        }
                                        reader.Close();
                                        if (ok == 4)
                                        {
                                            command.CommandText = "INSERT INTO emprunt (idUser, idOuvrage, confirme)"
                                                                   + " values (" + user + ","
                                                                   + "'" + idOuvrage + "', '1')";
                                            if (command.ExecuteNonQuery() >= 1)
                                            {
                                                command.CommandText = " UPDATE ouvrage"
                                                            + " SET nbrExemplaireEmp = nbrExemplaireEmp + 1"
                                                            + " WHERE idOuvrage = '" + idOuvrage + "'";
                                                command.ExecuteNonQuery();
                                                ok = 5;
                                            }
                                        }
                                        else
                                        {
                                            command.CommandText = "INSERT INTO liste_attente (idOuvrage, idUser)"
                                                                + " values (" + idOuvrage + "," + user + ")";
                                            command.ExecuteNonQuery();
                                        }
                                    }
                                }
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


        // Confirmation
        // 0 -> error: couldn't find the user
        // 1 -> error: either he never did a "reservation" or 24h have passed since the user did a "reservation"
        // 2 -> success
        public int confirmer(int choice, int idOuvrage, int id)
        {
            string user = "";
            int ok = 0;
            switch (choice)
            {
                case 1:
                    {
                        try
                        {
                            conn.Open();
                            command.CommandText = "SELECT idUser FROM `etudiant` WHERE numeroCarte ='" + id + "'";
                            MySqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                user = reader["idUser"].ToString();
                                ok = 1;
                            }
                            reader.Close();
                            if (ok == 1)
                            {
                                command.CommandText = " UPDATE emprunt"
                                                    + " SET confirme = '1'"
                                                    + " WHERE idOuvrage = '" + idOuvrage + "' AND idUser = '" + user + "'";
                                if (command.ExecuteNonQuery() > 0)
                                {
                                    ok = 2;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        break;
                    }
                case 2:
                    {
                        try
                        {
                            conn.Open();
                            command.CommandText = "SELECT idUser FROM `enseignant` WHERE matricule ='" + id + "'";
                            MySqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                user = reader["idUser"].ToString();
                                ok = 1;
                            }
                            reader.Close();
                            if (ok == 1)
                            {
                                command.CommandText = " UPDATE emprunt"
                                                    + " SET confirme = '1'"
                                                    + " WHERE idOuvrage = '" + idOuvrage + "' AND idUser = '" + user + "'";
                                if (command.ExecuteNonQuery() > 0)
                                {
                                    ok = 2;
                                }
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

        // Remise
        // 0 -> error: couldn't find the user
        // 1 -> error: couldn't return the book
        // 2 -> success
        public int remise(int choice, int idOuvrage, int id)
        {
            string user = "";
            int ok = 0;
            conn.Open();
            switch (choice)
            {
                case 1:
                    {
                        try
                        {
                            command.CommandText = "SELECT idUser FROM `etudiant` WHERE numeroCarte ='" + id + "'";
                            MySqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                user = reader["idUser"].ToString();
                                ok = 1;
                            }
                            reader.Close();
                            if (ok == 1)
                            {
                                command.CommandText = " DELETE FROM emprunt WHERE idOuvrage = "+idOuvrage+" AND idUser = "+user;
                                if (command.ExecuteNonQuery() > 0)
                                {
                                    command.CommandText = " UPDATE ouvrage"
                                                    + " SET nbrExemplaireEmp = nbrExemplaireEmp - 1"
                                                    + " WHERE idOuvrage = '" + idOuvrage + "'";
                                    command.ExecuteNonQuery();

                                    ok = 2;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                        break;
                    }
                case 2:
                    {
                        try
                        {
                            command.CommandText = "SELECT idUser FROM `enseignant` WHERE matricule ='" + id + "'";
                            MySqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                user = reader["idUser"].ToString();
                                ok = 1;
                            }
                            reader.Close();
                            if (ok == 1)
                            {
                                command.CommandText = " DELETE FROM emprunt WHERE idOuvrage = " + idOuvrage + " AND idUser = " + user;
                                if (command.ExecuteNonQuery() > 0)
                                {
                                    command.CommandText = " UPDATE ouvrage"
                                                    + " SET nbrExemplaireEmp = nbrExemplaireEmp - 1"
                                                    + " WHERE idOuvrage = '" + idOuvrage + "'";
                                    command.ExecuteNonQuery();
                                    ok = 2;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                        break;
                    }
                default: break;
            }
            if (ok == 2)
            {
                string idUser = "";
                command.CommandText = "SELECT idUser FROM liste_attente where idOuvrage = '" + idOuvrage + "' order by idListe asc limit 1";
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    idUser = reader["idUser"].ToString();
                }
                reader.Close();

                string email ="";
                command.CommandText = "SELECT email FROM `user` WHERE idUser =" + idUser;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    email = reader["email"].ToString();
                }
                reader.Close();

                string title = "";
                command.CommandText = "SELECT titre FROM `ouvrage` WHERE idOuvrage =" + idOuvrage;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    title = reader["titre"].ToString();
                }
                reader.Close();

                OnremiseEmailSent(email, title);

                command.CommandText = "DELETE FROM liste_attente WHERE idUser = " + idUser + " AND idOuvrage = " + idOuvrage;
                command.ExecuteNonQuery();

            }
            conn.Close();
            return ok;
        }

        public bool login(string email, string password)
        {
            conn.Open();
            string email2 = "";
            string password2 = "";
            try
            {
                command.CommandText = "SELECT email,password FROM `admin` WHERE adminID = 1";
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    email2 = reader["email"].ToString();
                    password2 = reader["password"].ToString();
                }
                reader.Close();
            }
            catch(Exception ex)
            {

            }
            if (email == email2 && password == password2)
                return true;
            else
                return false;
        }

        protected virtual void OnremiseEmailSent(string email, string title)
        {
            if (remiseEmailSent != null)
                remiseEmailSent.Invoke(this, new EmailEventArgs() { email = email, title = title });
        }

        public class EmailEventArgs : EventArgs
        {
            public string email { get; set; }
            public string title { get; set; }
        }

        public class Mailservice
        {
            public void OnremiseEmailSent(Object sender, LibraryAdministrationService.EmailEventArgs args)
            {
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                string emailCore = "Hello, \nThe book '"+args.title+"' is now ready for you to borrow.";
                client.Credentials = new System.Net.NetworkCredential("library.universite2@gmail.com", "LibraryOfNTIC");
                MailMessage mm = new MailMessage("library.universite2@gmail.com", args.email, "Library Notification", emailCore);
                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                client.Send(mm);
            }
        }

    }

}
