using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using WEB_API_DI_Test.Models;

namespace WEB_API_DI_Test
{
    

    public interface IDatabase
    {
        
        List<Post> GetPost();
        Post GetPostByID(int ID);
        string PostPost(Post postss);
        string UpdatePost([FromBody]JsonPatchDocument<Post> postss, int ID);
        string DeletePost(int ID);
    }

    public class Database : IDatabase
    {
        NpgsqlConnection _connection;
        public Database(NpgsqlConnection connection)
        {
            _connection = connection;
            _connection.Open();
        }

        public List<Post> GetPost()
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM post";
            var result = command.ExecuteReader();
            var Postss = new List<Post>();

            while (result.Read())
            {
                Postss.Add(new Post()
                {
                    id = (int)result[0],
                    title = (string)result[1],
                    content = (string)result[2],
                    tags = (string)result[3],
                    status = (bool)result[4],
                    create_time = (DateTime)result[5],
                    updated_time = (DateTime)result[6]
                });
            }
                
            _connection.Close();

            return Postss;
        }

        public Post GetPostByID(int ID)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM post WHERE id = @ID";
            command.Parameters.AddWithValue("@ID", ID);
            var result = command.ExecuteReader();
            result.Read();

            var Postss = new Post()
                
                {
                    id = (int)result[0],
                    title = (string)result[1],
                    content = (string)result[2],
                    tags = (string)result[3],
                    status = (bool)result[4],
                    create_time = (DateTime)result[5],
                    updated_time = (DateTime)result[6]
                };

          
                

            _connection.Close();

            return Postss;
        }

        public string PostPost(Post postsss)
        {
            
            var command = _connection.CreateCommand();
            command.CommandText = "INSERT INTO post WHERE id = @ID (title,content,tags,status,create_time,updated_time) VALUES (@title,@content,@tags,@status,@create_time,@updated_time) RETURNING id";

            command.Parameters.AddWithValue("@title", postsss.title);
            command.Parameters.AddWithValue("@content", postsss.content);
            command.Parameters.AddWithValue("@tags", postsss.tags);
            command.Parameters.AddWithValue("@status", postsss.status);
            command.Parameters.AddWithValue("@create_time", DateTime.Now);
            command.Parameters.AddWithValue("@updated_time", DateTime.Now);

            command.Prepare();

            var result = command.ExecuteScalar();

            _connection.Close();

            return $"New Post ID:{(int)result} Added";

        }

        public string UpdatePost([FromBody]JsonPatchDocument<Post> postss, int ID)
        {
            var oldPost = GetPostByID (ID);
            
            var command = _connection.CreateCommand();
            _connection.Open();

            postss.ApplyTo(oldPost);

            command.CommandText = $"UPDATE post SET (title,content,tags,status,updated_time) = (@title,@content,@tags,@status,@updated_time) WHERE id = {ID}";
            
            command.Parameters.AddWithValue("@title", oldPost.title);
            command.Parameters.AddWithValue("@content", oldPost.content);
            command.Parameters.AddWithValue("@tags", oldPost.tags);
            command.Parameters.AddWithValue("@status", oldPost.status);

            command.Parameters.AddWithValue("@updated_time", DateTime.Now);

            command.Prepare();
            //var  = command.ExecuteScalar();
            _connection.Close();

            return $"Post ID: {ID} updated";
        }



        public string DeletePost(int ID)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "DELETE FROM Post WHERE id = @ID";
            command.Parameters.AddWithValue("@ID", ID);
            
            _connection.Close();

            return $"Post ID:{ID} removed";
        }
    }


}


//patch postman
//[
//	{
//		"do" : "replace",
//		"path" : "username", 
//		"value" : "hftdhrdhbxdehrx"

//	},
//	{
//		"do" : "replace", 
//		"path" : "salt",
//		"value" : "xf57uxd 6s4e6"

//	}
//]