using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetProvider
{
    public class StudentRepository
    {
        private const string ConnectionString = "Data Source=DESKTOP-2EUV2Q6;Initial Catalog=CreateDatabase_StudyManager;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public IEnumerable<Student> GetStudents()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                return GetStudents(connection);
            }
        }
        public IEnumerable<Student> GetStudentID(int id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                return GetStudentID(id, connection);
            }
        }

        public async Task CreateStudent(Student student)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                await InsertStudentAsync(connection, student);
            }
        }

        public async Task DeleteStudent(int Id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                await DeleteStudentAsync(Id, connection);
            }
        }
        public async Task PutID(int Id, Student student)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                await UpdateStudentAsync(connection, student, Id);
            }
        }

        private static IEnumerable<Student> GetStudents(SqlConnection connection)
        {
            var command = new SqlCommand(@" SELECT STUDENTID, NAME, LASTNAME, MOBILE, GITHUB FROM STUDENTS", connection);

            var students = new List<Student>();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var student = GetStudent(reader);
                    students.Add(student);
                }
            }
            return students;
        }
        private static IEnumerable<Student> GetStudentID(int id, SqlConnection connection)
        {
            var command = new SqlCommand(@" SELECT STUDENTID, NAME, LASTNAME, MOBILE, GITHUB FROM STUDENTS", connection);

            var student = new List<Student>();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var studentID = GetStudent(reader);
                    if (studentID.StudentID == id)
                    {
                        student.Add(studentID);
                    }
                    else
                    {

                    }
                }
            }
            return student;
        }
        private static async Task DeleteStudentAsync(int Id, SqlConnection connection)
        {
            var deleteCommnd = string.Format("DELETE FROM STUDENTS WHERE STUDENTID = '{0}'", Id);
            SqlCommand command = new SqlCommand(deleteCommnd, connection);
            await command.ExecuteNonQueryAsync();
        }
        private static async Task InsertStudentAsync(SqlConnection connection, Student student)
        {
            var insertCommand = new SqlCommand("INSERT INTO STUDENTS (NAME, LASTNAME, MOBILE, GITHUB) VALUES(@student, @lastname, @mobile, @github);", connection);

            insertCommand.Parameters.AddWithValue("@student", student.Name);
            insertCommand.Parameters.AddWithValue("@lastname", student.LastName);
            insertCommand.Parameters.AddWithValue("@mobile", student.Mobile);
            insertCommand.Parameters.AddWithValue("@github", student.Github);
            await insertCommand.ExecuteNonQueryAsync();
        }

        private static async Task UpdateStudentAsync(SqlConnection connection, Student student, int Id)
        {
            string sql = string.Format("Update Students Set NAME = '{0}', LASTNAME= '{1}', MOBILE= '{2}', GITHUB= '{3}' Where STUDENTID = '{4}';",
        student.Name, student.LastName, student.Mobile, student.Github, Id);
            var insertCommand = new SqlCommand(sql, connection);
            await insertCommand.ExecuteNonQueryAsync();
        }
        private static Student GetStudent(SqlDataReader reader)
        {
            var studentID = reader.GetInt32(0);
            var name = reader.GetString(1);
            var lastName = reader.GetString(2);
            var mobile = reader.GetString(3);
            var github = reader.GetString(4);

            return new Student { StudentID = studentID, Name = name, LastName = lastName, Mobile = mobile, Github = github };
        }

    }
}
