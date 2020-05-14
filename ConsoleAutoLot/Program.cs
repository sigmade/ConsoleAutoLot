using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data;
using static System.Console;
using System.Data.SqlClient;

namespace ConsoleAutoLot
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=AutoLot;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("Подключение открыто");

                // Вывод информации о подключении
                Console.WriteLine("Свойства подключения:");
                Console.WriteLine("\tСтрока подключения: {0}", connection.ConnectionString);
                Console.WriteLine("\tБаза данных: {0}", connection.Database);
                Console.WriteLine("\tСервер: {0}", connection.DataSource);
                Console.WriteLine("\tВерсия сервера: {0}", connection.ServerVersion);
                Console.WriteLine("\tСостояние: {0}", connection.State);
                Console.WriteLine("\tWorkstationld: {0}", connection.WorkstationId);
            }
            // Получить строку подключения и поставщик из файла *.config,
            string dataProvider = ConfigurationManager.AppSettings["provider"];
            string connectionstring = ConfigurationManager.AppSettings["connectionstring"];
            Console.WriteLine(connectionstring);
            // Получить фабрику поставщиков.
            DbProviderFactory factory = DbProviderFactories.GetFactory(dataProvider);
            // Получить объект подключения.
            using (DbConnection connection = factory.CreateConnection())
            {
                WriteLine($"Your connection object is a: {connection.GetType().Name}");
                connection.ConnectionString = connectionstring;
                connection.Open();
                // Создать объект команды.
                DbCommand command = factory.CreateCommand();
                WriteLine($"Your command object is a: {command.GetType().Name}");
                command.Connection = connection;
                command.CommandText = "Select * From Inventory";
                using (DbDataReader dataReader = command.ExecuteReader())
                {
                    WriteLine($"Your data reader object is a: {dataReader.GetType().Name}");
                    WriteLine("\n***** Current Inventory *****");
                    while (dataReader.Read())
                        WriteLine($"-> Car #{dataReader ["CarId"] } is a {dataReader["Make"]}.") ;
                }


            }
        

            Console.WriteLine("Подключение закрыто...");
        }
    }
        
}
