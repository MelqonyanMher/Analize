using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Analizer.NetCore.Services
{
    public static class DemoDbMeneger
    {
        public static void FillDb()
        {
            string[] cities = { "Yerevan", "Gyumri", "Vanadzor", "Ijevan", "Ashtarak", "Hrazdan", "Gavar", "Armavir", "Artashat", "Exegnadzor", "Kapan" };
            string connectionstring = @"Server=(localdb)\Mssqllocaldb;Database=HidrometDb;Trusted_Connection=True;";
            SqlConnection connection = new SqlConnection(connectionstring);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            DateTime start = new DateTime(2018, 5, 1);
            Random rd = new Random();
            StringBuilder cmdQuery = new StringBuilder();
            connection.Open();
            for (int i = 0; i < 100000; i++)
            {
                if (i % cities.Length == 0)
                {
                    start = start.AddHours(3);
                }
                int texum = rd.Next(1000);
                String query = $"insert into Hidromet (Date,City,Temperature,DewPoint,Precipitation,Wind,MeterologicalPressure) values (" +
                    $"'{(start.ToString("yyyy-MM-ddTHH:mm:ss"))}'," +
                    $"'{cities[i % cities.Length]}'," +
                    $"{rd.Next(15, 40)}," +
                    $"{rd.Next(15)}," +
                    $"{(texum < 700 ? 0 : texum < 840 ? 1 : texum < 920 ? 2 : texum < 980 ? 3 : 4)}," +
                    $"{(texum < 800 ? 0 : texum < 900 ? 5 : 10)}," +
                    $"{rd.Next(44, 46)})";
                cmdQuery.Append(query);
                if ((i + 1) % 1000 == 0)
                {
                    cmd.CommandText = cmdQuery.ToString();
                    Console.WriteLine(cmdQuery.ToString());
                    cmd.ExecuteNonQuery();
                    cmdQuery = new StringBuilder();
                }
            }
            cmd.Dispose();
            connection.Dispose();
        }
    }
}
