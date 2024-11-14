using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConsoleApp1
{
    public class DBClient
    {
        string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog = HotelDb; Integrated Security = True; Connect Timeout = 30; Encrypt=False;Trust Server Certificate=False;Application Intent = ReadWrite; Multi Subnet Failover=False";

        private int GetMaxFacilityNo (SqlConnection sqlConnection)
        {
            Console.WriteLine("Calling -> GetMaxFacilityNo");

            
            string queryStringMaxFacilityNo = "SELECT  MAX(Facility_No)  FROM DemoFacility";
            Console.WriteLine($"SQL applied: {queryStringMaxFacilityNo}");

         
            SqlCommand command = new SqlCommand(queryStringMaxFacilityNo, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();

            int MaxFacility_No = 0;

            
            if (reader.Read())
            {
                
                MaxFacility_No = reader.GetInt32(0); 
            }

         
            reader.Close();

            Console.WriteLine($"Max Facility#: {MaxFacility_No}");
            Console.WriteLine();

        
            return MaxFacility_No;
        }
        private int DeleteFacility(SqlConnection connection, int Facility_no)
        {
            Console.WriteLine("Calling -> DeleteFacility");

          
            string deleteCommandString = $"DELETE FROM DemoFacility  WHERE Facility_No = {Facility_no}";
            Console.WriteLine($"SQL applied: {deleteCommandString}");

          
            SqlCommand command = new SqlCommand(deleteCommandString, connection);
            Console.WriteLine($"Deleting hotel #{Facility_no}");
            int numberOfRowsAffected = command.ExecuteNonQuery();

            Console.WriteLine($"Number of rows affected: {numberOfRowsAffected}");
            Console.WriteLine();

           
            return numberOfRowsAffected;
        }
        private int UpdateFacility(SqlConnection connection, facility Facility)
        {
            Console.WriteLine("Calling -> UpdateFacility");

           
            string updateCommandString = $"UPDATE DemoFacility SET Name='{Facility.Name}' WHERE Facility_No = {Facility.Facility_No}";
            Console.WriteLine($"SQL applied: {updateCommandString}");

            SqlCommand command = new SqlCommand(updateCommandString, connection);
            Console.WriteLine($"Updating hotel #{Facility.Facility_No}");
            int numberOfRowsAffected = command.ExecuteNonQuery();

            Console.WriteLine($"Number of rows affected: {numberOfRowsAffected}");
            Console.WriteLine();

           
            return numberOfRowsAffected;
        }
        private int InsertFacility(SqlConnection connection, facility Facility)
        {
            Console.WriteLine("Calling -> InsertFacility");

          
            string insertCommandString = $"INSERT INTO DemoFacility VALUES({Facility.Facility_No}, '{Facility.Name}')";
            Console.WriteLine($"SQL applied: {insertCommandString}");

          
            SqlCommand command = new SqlCommand(insertCommandString, connection);

            Console.WriteLine($"Creating hotel #{Facility.Facility_No}");
            int numberOfRowsAffected = command.ExecuteNonQuery();

            Console.WriteLine($"Number of rows affected: {numberOfRowsAffected}");
            Console.WriteLine();

           
            return numberOfRowsAffected;
        }
        private List<facility> ListAllFacility(SqlConnection connection)
        {
            Console.WriteLine("Calling -> ListAllFacility");

          
            string queryStringAllFacility = "SELECT * FROM DemoFacility";
            Console.WriteLine($"SQL applied: {queryStringAllFacility}");

         
            SqlCommand command = new SqlCommand(queryStringAllFacility, connection);
            SqlDataReader reader = command.ExecuteReader();

            Console.WriteLine("Listing all Facilitys:");

        
            if (!reader.HasRows)
            {
            
                Console.WriteLine("No Facility in database");
                reader.Close();

             
                return null;
            }

        
            List<facility> Facility = new List<facility>();
            while (reader.Read())
            {
               
                facility nextFacility = new facility()
                {
                    Facility_No = reader.GetInt32(0), 
                    Name = reader.GetString(1),    
                   
                };

               
                Facility.Add(nextFacility);

                Console.WriteLine(nextFacility);
            }

         
            reader.Close();
            Console.WriteLine();

         
            return Facility;
        }
        private facility GetFacility(SqlConnection connection, int Facility_no)
        {
            Console.WriteLine("Calling -> GetFacility");

           
            string queryStringOneFacility = $"SELECT * FROM DemoFacility WHERE Facility_no = {Facility_no}";
            Console.WriteLine($"SQL applied: {queryStringOneFacility}");

      
            SqlCommand command = new SqlCommand(queryStringOneFacility, connection);
            SqlDataReader reader = command.ExecuteReader();

            Console.WriteLine($"Finding Facility#: {Facility_no}");

        
            if (!reader.HasRows)
            {
                //End here
                Console.WriteLine("No Facility in database");
                reader.Close();

               
                return null;
            }

        
            facility Facility = null;
            if (reader.Read())
            {
                Facility = new facility()
                {
                    Facility_No = reader.GetInt32(0), 
                    Name = reader.GetString(1),    
                   
                };

                Console.WriteLine(Facility);
            }

         
            reader.Close();
            Console.WriteLine();

          
            return Facility;
        }
        public void Start()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
            
                connection.Open();

              
                ListAllFacility(connection);

           
                facility newHotel = new facility()
                {
                    Facility_No = GetMaxFacilityNo(connection) + 1,
                    Name = "New Facility",
                   
                };

              
                InsertFacility(connection, newHotel);

               
                ListAllFacility(connection);

            
                facility hotelToBeUpdated = GetFacility(connection, newHotel.Facility_No);

             
                hotelToBeUpdated.Name += "(updated)";


                
                UpdateFacility(connection, hotelToBeUpdated);

                
                ListAllFacility(connection);

                
                facility hotelToBeDeleted = GetFacility(connection, hotelToBeUpdated.Facility_No);

             
                GetFacility(connection, hotelToBeDeleted.Facility_No);

              
                ListAllFacility(connection);
            }
        }
    }
}
