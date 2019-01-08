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

        //public void Start(DateTime date)
        //{
        //    NameValueCollection props = new NameValueCollection
        //    {
        //        { "quartz.serializer.type", "binary" }
        //    };
        //    StdSchedulerFactory factory = new StdSchedulerFactory(props);
        //    IScheduler scheduler = StdSchedulerFactory.GetScheduler();
        //    scheduler.Start();
        //    IJobDetail job = JobBuilder.Create<triggerWork>().Build();
        //    ITrigger trigger = TriggerBuilder.Create()
        //     .WithIdentity("IDGJob", "IDG")
        //       .StartAt(date)
        //       .WithPriority(1)
        //       .Build();
        //    scheduler.ScheduleJob(job, trigger);
        //}
        //private void SetUpTimer(TimeSpan alertTime)
        //{
        //    DateTime current = DateTime.Now;
        //    TimeSpan timeToGo = alertTime - current.TimeOfDay;
        //    if (timeToGo < TimeSpan.Zero)
        //    {
        //        return;//time already passed
        //    }
        //    Timer timer = new System.Threading.Timer(x =>
        //    {
        //        doWork();
        //    }, null, timeToGo, Timeout.InfiniteTimeSpan);
        //    timerList.Add(timer);

        //}

        //private void doWork()
        //{
        //    //this runs at 16:00:00
        //}


    }
}
