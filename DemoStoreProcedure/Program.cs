using System;
using System.ComponentModel.DataAnnotations;
using MySql.Data.MySqlClient;

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

                var mysqlCreateSchemaCommand = new MySqlCommand(sqlQueryCreateSchema,mySqlConnection);

                mySqlConnection.Open();

                var readerCreateSchema = mysqlCreateSchemaCommand.ExecuteReader();
                readerCreateSchema.Close();

                const string sqlQueryUseDb = "use  test;";
                
                var mySqlCommandUseDb = new MySqlCommand(sqlQueryUseDb,mySqlConnection);

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

                const string sqlQuery = "DROP PROCEDURE IF EXISTS DemoCreateProcedure;" +
                                        "DELIMITER $$" +
                                        "CREATE DEFINER =`root`@`%` PROCEDURE" +
                                        "`DemoCreateProcedure`(IN _endDate datetime)" +
                                        "BEGIN" +
                                        "Select* From tutorials_tbl Where submission_date <= submissionEnd_Date order by" +
                                        "tutorial_author asc;" +
                                        "END$$" +
                                        "DELIMITER;";

                MySqlCommand mySqlCommand = new MySqlCommand(sqlQuery, mySqlConnection);

                mySqlConnection.Open();
                var reader = mySqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    //do something
                }
                reader.Close();
                mySqlConnection.Close();

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
