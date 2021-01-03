using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Blog.Models {
    public class UserHelper {
        private  SqlConnection createConnection() {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='C:\Users\chabd\source\repos\My Blog\My Blog\Database\MyBlogDB.mdf';Integrated Security=True";
            return new SqlConnection(connectionString);
        }
        public  bool SignUpUser(Users user) {
            SqlConnection connection = createConnection();
            string query = $"insert into Users(name, username, email, password, gender) values('{user.Name}','{user.Username}','{user.Email}','{user.Password}', '{user.Gender}')";  
            SqlCommand cmd = new SqlCommand(query, connection);
            connection.Open();

            try{
            cmd.ExecuteNonQuery();

            } catch(Exception e) {
                connection.Close();
                return false;
            }
            connection.Close();
            return true;
        }

        public  Users AuthenticateUser(string username , string password) {
            SqlConnection connection = createConnection();
            string query = $"select * from Users where username='{username}'";  
            SqlCommand cmd = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (!reader.HasRows) return null;
            Users authUser = new Users();
        
            while (reader.Read()) {
                
                authUser.Name = reader.GetString(1);
                authUser.Username = reader.GetString(2);
                authUser.Email = reader.GetString(3);
                authUser.Password = reader.GetString(4);
                authUser.Gender = reader.GetString(5);
                if(authUser.Username.Equals(username) && authUser.Password.Equals(password)) {
                    connection.Close();
                    return authUser;
                }
            }
            connection.Close();
            return null ;
        }

        public  Users UpdateUserProfile(Users user) {
            SqlConnection connection = createConnection();
            string query = String.Empty;
            if(user.Password == null) {
                query = $"update Users set name='{user.Name}', gender='{user.Gender}'  where username = '{user.Username}'";  

            }else{
                query =   $"update Users set name='{user.Name}', gender='{user.Gender}', password = '{user.Password}'  where username = '{user.Username}'";  
            }
            SqlCommand cmd = new SqlCommand(query, connection);
            connection.Open();
            try{
            cmd.ExecuteNonQuery();
            } catch(Exception e) {
                connection.Close();
                return null;
            }
            connection.Close();
            Users newUser = getUserByUsername(user.Username);
            return newUser;
        }

        public  Users getUserByUsername(string username) {
             SqlConnection connection = createConnection();
            string query = $"select * from Users where username='{username}'";  
            SqlCommand cmd = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (!reader.HasRows) return null;
            Users authUser = new Users();
        
            while (reader.Read()) {
                
              authUser.Name = reader.GetString(1);
                authUser.Username = reader.GetString(2);
                authUser.Email = reader.GetString(3);
                authUser.Password = reader.GetString(4);
                authUser.Gender = reader.GetString(5);
                    
            }
            connection.Close();
            return authUser;
        }

        

    }
}
