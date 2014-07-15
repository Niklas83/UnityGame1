using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace GameWCFservice
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "UserService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select UserService.svc or UserService.svc.cs at the Solution Explorer and start debugging.
    //******* To Test DB press play in this view and run the console application!
    public class UserService : IUserService
    {
        private SqlConnection DBConn;
        private SqlCommand DBCommand;
        private readonly string _connStr = ConfigurationManager.ConnectionStrings["azureSqlConnectionString"].ConnectionString;

        public string UserRegister(string userName, string password, string email)
        {
            //Prepared data for db storage of user account
            DateTime registrationDate = DateTime.Now;
            int proUser = 0;
            DateTime lastLoginDate = DateTime.Now;
            int highScore = 0;
            email = email.ToLower();
            userName = userName.ToLower();

            //Sets values if they already exists in the DB
            string DbUserName = "";
            string DbEmail = "";

            DBConn = new SqlConnection(_connStr);
            DBCommand = DBConn.CreateCommand();
            string queryString = "SELECT * FROM Users WHERE UserName = @userName OR Email = @email";
            using (this.DBConn)
            using (this.DBCommand)
            {
                DBCommand.CommandText = queryString;
                DBCommand.Parameters.AddWithValue("@userName", userName);
                DBCommand.Parameters.AddWithValue("@email", email);

                DBConn.Open();

                using (SqlDataReader reader = DBCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DbUserName = (string)reader["UserName"];
                        DbEmail = (string)reader["Email"];
                    }
                }
            }

            if (DbEmail == email)
            {
                return "An account has already been registered with this email.";
            }
            else if (DbUserName == userName)
            {
                return "Username already taken.";
            }
            else
            {
                DBConn = new SqlConnection(_connStr);
                DBCommand = DBConn.CreateCommand();

                queryString =
                    "INSERT INTO Users (UserName, Password, RegistrationDate, ProUser, Email, LastLoggedOn, Highscore) " +
                    "VALUES(@userName, @password, @registrationDate, @proUser, @email, @lastLoginDate, @highScore)";

                using (this.DBConn)
                using (this.DBCommand)
                {
                    DBCommand.CommandText = queryString;

                    DBCommand.Parameters.AddWithValue("@userName", userName);
                    DBCommand.Parameters.AddWithValue("@password", password);
                    DBCommand.Parameters.AddWithValue("@registrationDate", registrationDate);
                    DBCommand.Parameters.AddWithValue("@proUser", proUser);
                    DBCommand.Parameters.AddWithValue("@email", email);
                    DBCommand.Parameters.AddWithValue("@lastLoginDate", lastLoginDate);
                    DBCommand.Parameters.AddWithValue("@highScore", highScore);

                    DBConn.Open();
                    DBCommand.ExecuteNonQuery();
                }
                return "Account successfully created!";
            }
        }

        public UserInformation GetUser(int userId, int selfId)
        {
            if (userId == selfId)
            {
                return new UserInformation { Id = selfId, Name = " Returning my own info", AchievementPoints = 97854 };
            }
            else
            {
                return new UserInformation { Id = userId, Name = "Returning other user info", AchievementPoints = 45432 };
            }
        }
    }
}
