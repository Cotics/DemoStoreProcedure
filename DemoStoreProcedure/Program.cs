using System;

using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Data;

namespace DemoStoreProcedure
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                const string connectionString = "Your Mysql Connection String";

                var mySqlConnection = new MySqlConnection(connectionString);

                #region Create Schema

                const string sqlQueryCreateSchema = "Create schema test;";

                var mysqlCreateSchemaCommand = new MySqlCommand(sqlQueryCreateSchema, mySqlConnection);

                mySqlConnection.Open();

                var readerCreateSchema = mysqlCreateSchemaCommand.ExecuteReader();
                readerCreateSchema.Close();

                const string sqlQueryUseDb = "use  test;";

                var mySqlCommandUseDb = new MySqlCommand(sqlQueryUseDb, mySqlConnection);

                var readerForUseDb = mySqlCommandUseDb.ExecuteReader();
                readerForUseDb.Close();

                mySqlConnection.Close();


                #endregion

                #region Create tutorials_tbl table

                const string sqlQueryForCreatetutorials_tblTable = "create table tutorials_tbl(" +
                                                          "tutorial_id INT NOT NULL AUTO_INCREMENT," +
                                                          "tutorial_title VARCHAR(100) NOT NULL," +
                                                          "tutorial_author VARCHAR(40) NOT NULL," +
                                                          "submission_date DATE," +
                                                          "submissionEnd_Date DATE," +
                                                          "PRIMARY KEY(tutorial_id)" +
                                                          ");";


                var mySqlCommandCreateTable = new MySqlCommand(sqlQueryForCreatetutorials_tblTable, mySqlConnection);
                mySqlConnection.Open();
                var readerForCreateTable = mySqlCommandCreateTable.ExecuteReader();
                readerForCreateTable.Close();
                mySqlConnection.Close();

                #endregion

                #region Stored Procedure Method Create
                // first create the store proc 
                const string sqlQuery = "DROP PROCEDURE IF EXISTS DemoCreateProcedure;" +

                                        "CREATE  PROCEDURE" +
                                        "DemoCreateProcedure(IN _endDate _datetime)" +
                                        "BEGIN" +
                                        "Select * From tutorials_tbl Where submission_date <= _endDate order by" +
                                        "tutorial_author;" +
                                        "END";

                MySqlCommand mySqlCommand = new MySqlCommand(sqlQuery, mySqlConnection);

                mySqlConnection.Open();
                // Please see ExecuteNonQuery
                mySqlCommand.ExecuteNonQuery();

                mySqlConnection.Close();


                //Now call the proc

                /* change Below to Date time needed*/
                DateTime Date = DateTime.Now;
                using (SqlConnection conn = new SqlConnection("Your Mysql Connection String"))
                {
                    conn.Open();

                    // 1.  create a command object identifying the stored procedure
                    SqlCommand cmd = new SqlCommand("DemoCreateProcedure", conn);

                    // 2. set the command object so it knows to execute a stored procedure
                    cmd.CommandType = CommandType.StoredProcedure;

                    // 3. add parameter to command, which will be passed to the stored procedure
                    cmd.Parameters.Add(new SqlParameter("@_endDate", Date));

                    // execute the command
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        // iterate through results, printing each to console
                        while (rdr.Read())
                        {

                            //do something
                            Console.WriteLine(rdr[1].ToString());
                        }
                    }
                }

                #endregion

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadLine();
                throw;
            }
        }
    }
}
