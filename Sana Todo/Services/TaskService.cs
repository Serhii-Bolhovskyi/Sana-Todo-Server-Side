namespace Sana_Todo.Services;

using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Sana_Todo.Models;

public class TaskService : ITaskImplement
{
    private readonly string _connectionString;
    public TaskService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public List<TaskModel> GetAllTasks()
    {
        var tasks = new List<TaskModel>();

        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(@"SELECT Id, Title, CategoryId, Deadline, CompleteDate, IsCompleted FROM Tasks 
                ORDER BY 
                IsCompleted ASC,            
                CASE WHEN IsCompleted = 1 THEN CompleteDate END DESC", connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var task = new TaskModel
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            CategoryId = reader.IsDBNull(2) ? null : reader.GetInt32(2),
                            Deadline = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
                            CompleteDate = reader.IsDBNull(4) ? null : reader.GetDateTime(4),
                            IsCompleted = reader.GetBoolean(5),
                        };
                        tasks.Add(task);
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Exception" + ex.Message);
        }
        return tasks;
    }

    public List<CategoryModel> GetAllCategories()
    {
        var categories = new List<CategoryModel>();
        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(@"SELECT * FROM Categories", connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var category = new CategoryModel
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        };
                        categories.Add(category);
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Exception" + ex.Message);
        }

        return categories;
    }
    public TaskModel AddTask(TaskModel task)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(@"INSERT INTO Tasks(Title, CategoryId, Deadline, IsCompleted) VALUES (@Title,@CategoryId, @Deadline, @IsCompleted)", connection);
                command.Parameters.AddWithValue("@Title", task.Title);
                command.Parameters.AddWithValue("@CategoryId", task.CategoryId ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Deadline", task.Deadline ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@IsCompleted", task.IsCompleted);
                command.ExecuteNonQuery();

                return task;
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Exception" + ex.Message);
            throw;
        }
       
    }
    public bool DeleteTask(int Id)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(@"Delete FROM Tasks WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", Id);
                int rowsAffected = command.ExecuteNonQuery(); // кількість змінених рядків
                return rowsAffected > 0;
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Exception" + ex.Message);
            throw;
        }
       
    }
    public TaskModel CompleteTask(TaskModel task)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(@"UPDATE Tasks SET IsCompleted = @IsCompleted, CompleteDate = @CompleteDate  WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@IsCompleted", task.IsCompleted);
                command.Parameters.AddWithValue("@Id", task.Id);
                command.Parameters.AddWithValue("@CompleteDate", DateTime.Now);
                command.ExecuteNonQuery();

                var selectCommand = new SqlCommand("SELECT * FROM Tasks WHERE Id = @Id", connection);
                selectCommand.Parameters.AddWithValue("@Id", task.Id);
                
                using(var reader =  selectCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new TaskModel
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            CategoryId = reader.IsDBNull(2) ? null : reader.GetInt32(2),
                            Deadline = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
                            CompleteDate = reader.IsDBNull(4) ? null : reader.GetDateTime(4),
                            IsCompleted = reader.GetBoolean(5),
                        };
                        
                    }
                }
                
                return null!;


            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("SQL Exception" + ex.Message);
            throw;
        }
        
    }
}
