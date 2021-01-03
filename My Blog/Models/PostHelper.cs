using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace My_Blog.Models {
    public class Posthelper {
        private  SqlConnection createConnection() {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='C:\Users\chabd\source\repos\My Blog\My Blog\Database\MyBlogDB.mdf';Integrated Security=True";
            return new SqlConnection(connectionString);
        }
        public bool AddPost(Posts post) {
                SqlConnection connection = createConnection();
            string query = $"insert into Posts(owner, title, body) values('{post.Owner}','{post.Title}','{post.Body}')";  
            SqlCommand cmd = new SqlCommand(query, connection);
            connection.Open();
            Regex trimmer = new Regex(@"\s\s+");
            post.Body = trimmer.Replace(post.Body, " ");
            string q = post.Body;
            post.Body = String.Join(" ", q.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries));

            try {
              cmd.ExecuteNonQuery();


            } catch (Exception e) {
                connection.Close();
                return false;
            }
            connection.Close();
            return true;
        }
        public bool DelPost(string ID) {
            SqlConnection connection = createConnection();
            string query = $"delete from Posts where Id = {ID}";  
            SqlCommand cmd = new SqlCommand(query, connection);
            connection.Open();
            try {
                cmd.ExecuteNonQuery();
            } catch (Exception e) {
                connection.Close();
                return false;
            }
            connection.Close();
            return true;
        }
        public bool EditPost(Posts post) {
            SqlConnection connection = createConnection();
            string query = $"update Posts set title = '{post.Title}', body= '{post.Body}' where Id = {post.ID}";  
            SqlCommand cmd = new SqlCommand(query, connection);
            connection.Open();
                cmd.ExecuteNonQuery();

            try {
            } catch (Exception e) {
                connection.Close();
                return false;
            }
            connection.Close();
            return true;
        }
        public List<Posts> GetPosts() {
            SqlConnection connection = createConnection();
            string query = $"select * from Posts";  
            SqlCommand cmd = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (!reader.HasRows) return null;
            List<Posts> posts = new List<Posts>();
        
            while (reader.Read()) {
                Posts temp = new Posts();
                temp.ID = reader.GetInt32(0);
                temp.Owner = reader.GetString(1);
                temp.Title = reader.GetString(2);
                temp.Body = reader.GetString(3);
                
                posts.Add(temp);
            }
            connection.Close();
            return posts;
        }
        public List<Posts> GetUserPosts(string username) {
            SqlConnection connection = createConnection();
            string query = $"select * from Posts where owner = '{username}'";  
            SqlCommand cmd = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (!reader.HasRows) return null;
            List<Posts> posts = new List<Posts>();
        
            while (reader.Read()) {
                Posts temp = new Posts();
                temp.ID = reader.GetInt32(0);
                temp.Owner = reader.GetString(1);
                temp.Title = reader.GetString(2);
                temp.Body = reader.GetString(3);
                
                posts.Add(temp);
            }
            connection.Close();
            return posts;
        }
        public Posts GetPostByID(string ID) {
            SqlConnection connection = createConnection();
            string query = $"select * from Posts where Id = {ID}";  
            SqlCommand cmd = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (!reader.HasRows) return null;
            Posts temp = new Posts();
            while (reader.Read()) {
               
                temp.ID = reader.GetInt32(0);
                temp.Owner = reader.GetString(1);
                temp.Title = reader.GetString(2);
                temp.Body = reader.GetString(3);
            }
            connection.Close();
            return temp;
        }

    }

}
