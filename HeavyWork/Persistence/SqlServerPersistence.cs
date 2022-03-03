using System;
using System.Data.SqlClient;
using HeavyWork.Entities;

namespace HeavyWork.Persistence
{
    public class SqlServerPersistence : IPersistence
    {
        private TestConfiguration _testConfiguration;

        public SqlServerPersistence(TestConfiguration testConfiguration) =>
            this._testConfiguration = testConfiguration;

        public void PersistData(TestDataCollected testDataCollected)
        {
            SqlConnection sqlConnection = new SqlConnection(_testConfiguration.ConnectionInfo);
            sqlConnection.Open();
            try
            {
                if (TableSourceExist(sqlConnection))
                {
                    SqlCommand command = new SqlCommand(@"INSERT INTO SOURCE ( 
                       TESTLABEL, STARTTEST, ENDTEST, TIMEELAPSED, STATUS, ERRORMESSAGE
                    ) VALUES (
                       @TESTLABEL, @STARTTEST, @ENDTEST, @TIMEELAPSED, @STATUS, @ERRORMESSAGE
                    )", sqlConnection);

                    command.Parameters.AddWithValue("@TESTLABEL", testDataCollected.TestLabel);
                    command.Parameters.AddWithValue("@STARTTEST", testDataCollected.StartTest);
                    command.Parameters.AddWithValue("@ENDTEST", testDataCollected.EndTest);
                    command.Parameters.AddWithValue("@TIMEELAPSED", testDataCollected.TimeElapsed);
                    command.Parameters.AddWithValue("@STATUS", testDataCollected.Status);

                    if (testDataCollected.ErrorMessage == null)
                      command.Parameters.AddWithValue("@ERRORMESSAGE", DBNull.Value);
                    else
                      command.Parameters.AddWithValue("@ERRORMESSAGE", testDataCollected.ErrorMessage);

                    command.ExecuteNonQuery();
                } else {
                    CreateTable(sqlConnection);
                    PersistData(testDataCollected);
                }
            }
            catch (SqlException error)
            {
                Console.WriteLine($"Erro de persistencia {error.Message}");
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        private void CreateTable(SqlConnection sqlConnection)
        {
            SqlCommand command = new SqlCommand(@"CREATE TABLE [dbo].[Source](
            	[TestLabel] [varchar](100) NOT NULL,
            	[StartTest] [datetime2](7) NOT NULL,
            	[EndTest] [datetime2](7) NOT NULL,
            	[TimeElapsed] [datetime2](7) NOT NULL,
            	[Status] [int] NOT NULL,
            	[ErrorMessage] [varchar](100) NULL
            ) ON [PRIMARY]", sqlConnection);

            command.ExecuteNonQuery();
        }

        private bool TableSourceExist(SqlConnection sqlConnection)
        {
            SqlCommand sqlCommand = new SqlCommand("select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Source'", sqlConnection);
            var response = sqlCommand.ExecuteReader();

            var HasRows = response.HasRows;

            response.Close();

            return HasRows;
        }
    }
}
