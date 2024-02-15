using aspnet_b8_Md_Faisal_Mahmud.src.Assignment_4.Object_Relational_Mapper;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic.FileIO;
using Object_Relational_Mapper;
using System.Data.Common;

class Program
{
    public List<DbCommand> commands = new List<DbCommand>();
    public static void Main(string[] args)
    {
        var adoNetUtility = new AdoNetUtility("Server=.\\SQLEXPRESS; Database=mini-orm; " +
                                      "User Id=;Password=; Encrypt=True; " +
                                      "TrustServerCertificate=True;");

        var myOrm = new MyORM<Guid, Course>("Server=.\\SQLEXPRESS; Database=mini-orm; " +
                                              "User Id=;Password=; Encrypt=True; " +
                                              "TrustServerCertificate=True;");

        Course course = new Course();
        myOrm.Insert(course);
        //myOrm.Update(course);
        //myOrm.Delete(course);
        //myOrm.Delete(1);
        //myOrm.GetAll();
       // myOrm.GetById(1);
    }
}

#region For initial practice


//var itemss = myOrm.GetAll();


//var getByidss = myOrm.GetById(10);
//myOrm.Delete(101);
//myOrm.Update(course);
//myOrm.Delete(course);



/*
Course course = new();
course.Id = 101;
course.Name = "CSharp";
course.Credits = 4.00;

Course course1 = new();
course1.Id = 103;
course1.Name = "asp.net";
course1.Credits = 4.00;

var sql = "insert into courses(Id, Name, Credits) values(@id, @name, @credits)";
adoNetUtility.WriteOperation(sql, new List<DbParameter>()
            {
                new SqlParameter("id", course1.Id),
                new SqlParameter("name", course1.Name),
                new SqlParameter("credits", course.Credits)
            });
*/


/*
var sql = "DELETE FROM Courses WHERE Id = 103";
adoNetUtility.WriteOperation(sql, null);
*/


/*
string sql = "CREATE TABLE my_table (id INT PRIMARY KEY, name VARCHAR(50), age INT)";
adoNetUtility.WriteOperation(sql, null);
*/


/*
var sql = "UPDATE Courses SET Name = 'Asp dot net' WHERE Name = 'asp.net'";
adoNetUtility.WriteOperation(sql, null);
*/


//var sql = "SELECT *FROM Courses;";
//var result = adoNetUtility.ReadOperation(sql, null, false);


//foreach (var kvp in  result[0])
//{
//    Console.Write(kvp.Key + "         ");
//}
//Console.WriteLine();
//foreach (var dict in result)
//{
//    foreach (var kvp in dict)
//    {
//        Console.Write(kvp.Value + "      ");
//    }
//    Console.WriteLine();
//}










//var connectionString =
//    "Server=.\\SQLEXPRESS; Database = AspnetB8; " +
//                "User Id = aspnetb8; Password = 123456;TrustServerCertificate=True;";
//var tableName = "Courses";
//var orm = new MyORM<Guid, Course>(connectionString, tableName);

//// Insert a new course
//var course = new Course { Id = Guid.NewGuid(), Name = "Programming 101", Credits = 3 };
//orm.Insert(course);

//// Update an existing course
//course.Name = "Introduction to Programming";
//orm.Update(course);

//// Delete a course by ID
//orm.Delete(course.Id);

//// Get a course by ID
//var retrievedCourse = orm.GetById(course.Id);

//// Get all courses
//var allCourses = orm.GetAll();
#endregion








