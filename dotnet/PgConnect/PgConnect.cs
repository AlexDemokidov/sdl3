using Npgsql;
using Npgsql.Replication;
using System;
using System.Data;

//Для отладки
// string logFilePath = "app.log";
// StreamReader sr = new StreamReader("../../../sdl1.conf");


string logFilePath = Environment.GetEnvironmentVariable("LOG_FILE_PATH");
StreamReader sr = new StreamReader("sdl3.conf");


var connectionStringFromConf = sr.ReadLine();

Console.WriteLine("User is ?");
var userName = Console.ReadLine();
Console.WriteLine("Password is ?");
var password = Console.ReadLine();

Console.WriteLine($"User is {userName}");
Console.WriteLine($"Password is {password}");

Npgsql.NpgsqlConnectionStringBuilder csb = new Npgsql.NpgsqlConnectionStringBuilder(connectionStringFromConf);

// Замена в строке подключения пользователя и пароля
csb.Username = userName;
csb.Password = password;

string connectionString = csb.ToString();

await using var dataSource = NpgsqlDataSource.Create(connectionString);

// StreamWriter logWriter = new StreamWriter(logFilePath, true);

var action = "0";

while (action != "q")
{
    try
    {
        await using var connection = await dataSource.OpenConnectionAsync();
        Console.WriteLine("To choose action input number: ");
        Console.WriteLine("1. Show Projects");
        Console.WriteLine("2. Create Project");
        Console.WriteLine("3. Open Project");
        Console.WriteLine("To close app enter 'q'");
        action = Console.ReadLine();

        if (action == "1") // Show Projects
        {
            await using var command = new NpgsqlCommand($"SELECT * FROM PROJECTS", connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.Write(reader[i].ToString() + " ");
                    using (StreamWriter logWriter = new StreamWriter(logFilePath, true))
                    {
                        // Сообщение записывается в файл
                        logWriter.Write(reader[i].ToString() + " ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        if (action == "2") // Create Project
        {
            Console.WriteLine("Enter name of Project: ");
            var name = Console.ReadLine();
            Console.WriteLine("Enter model of Project: ");
            var model = Console.ReadLine();
            var idProject = "";

            using var command = new NpgsqlCommand($"INSERT INTO Projects (name, model) VALUES ('{name}', '{model}'); SELECT id FROM Projects WHERE name = '{name}'", connection);
            command.ExecuteNonQuery();
            using var command2 = new NpgsqlCommand($"SELECT id FROM Projects WHERE name = '{name}'", connection);
            command2.ExecuteNonQuery();

            command2.Parameters.Add(new NpgsqlParameter("p_out", DbType.String) { Direction = ParameterDirection.Output });
            command2.ExecuteNonQuery();
            idProject = command2.Parameters[0].Value.ToString();

            using var command3 = new NpgsqlCommand($"INSERT INTO Components (project_id, name, class, current, voltage) VALUES ('{idProject}', 'test', 'test', '0.3', '0.4')", connection);
            command3.ExecuteNonQuery();
        }
        if (action == "3") // Open Project
        {
            var action2 = "0";
            Console.WriteLine("Enter id of Project: ");
            var idProject = Console.ReadLine();

            while (action2 != "q")
            {
                Console.WriteLine("To choose action input number: ");
                Console.WriteLine("1. Show components");
                Console.WriteLine("2. Show components with filter");
                Console.WriteLine("3. Add component");
                Console.WriteLine("4. Update component");
                Console.WriteLine("To return to the main menu input 'q'");
                action2 = Console.ReadLine();

                if (action2 == "1") // Show components
                {
                    await using var command = new NpgsqlCommand($"SELECT id, name, class, current, voltage FROM Components WHERE project_id = {idProject}", connection);
                    await using var reader = await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.Write(reader[i].ToString() + " ");
                            using (StreamWriter logWriter = new StreamWriter(logFilePath, true))
                            {
                                // Сообщение записывается в файл
                                logWriter.Write(reader[i].ToString() + " ");
                            }
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                }

                if (action2 == "2") // Show components with filter
                {
                    Console.WriteLine("Enter filter (voltage, current): ");
                    var field = Console.ReadLine();
                    Console.WriteLine("Enter filter (=, >, <): ");
                    var operand = Console.ReadLine();
                    Console.WriteLine("Enter value: ");
                    var value = Console.ReadLine();

                    await using var command = new NpgsqlCommand($"SELECT id, name, class, current, voltage FROM Components WHERE {field} {operand} '{value}' AND project_id = {idProject}", connection);
                    await using var reader = await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.Write(reader[i].ToString() + " ");
                            using (StreamWriter logWriter = new StreamWriter(logFilePath, true))
                            {
                                // Сообщение записывается в файл
                                logWriter.Write(reader[i].ToString() + " ");
                            }
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                }

                if (action2 == "3") // Add component
                {
                    Console.WriteLine("Enter name:");
                    var name = Console.ReadLine();
                    Console.WriteLine("Enter class:");
                    var type = Console.ReadLine();
                    Console.WriteLine("Enter current: ");
                    var current = Console.ReadLine();
                    Console.WriteLine("Enter voltage: ");
                    var voltage = Console.ReadLine();

                    await using var command = new NpgsqlCommand($"INSERT INTO Components (project_id, name, class, current, voltage) VALUES ('{idProject}', '{name}', '{type}', '{current}', '{voltage}')", connection);
                    await using var reader = await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.Write(reader[i].ToString() + " ");
                            using (StreamWriter logWriter = new StreamWriter(logFilePath, true))
                            {
                                // Сообщение записывается в файл
                                logWriter.Write(reader[i].ToString() + " ");
                            }
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                }

                if (action2 == "4") // Update component
                {
                    Console.WriteLine("Enter name of component:");
                    var name = Console.ReadLine();
                    Console.WriteLine("Enter column: ");
                    var column = Console.ReadLine();
                    Console.WriteLine("Enter value: ");
                    var value = Console.ReadLine();

                    await using var command = new NpgsqlCommand($"UPDATE Components SET {column} = {value} WHERE name = '{name}'", connection);
                    await using var reader = await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.Write(reader[i].ToString() + " ");
                            using (StreamWriter logWriter = new StreamWriter(logFilePath, true))
                            {
                                // Сообщение записывается в файл
                                logWriter.Write(reader[i].ToString() + " ");
                            }
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                }
            }
        }
    }
    catch (Exception ex)
    {
        // Сообщение записывается в файл
        using (StreamWriter logWriter = new StreamWriter(logFilePath, true))
        {
            // Сообщение записывается в файл
            logWriter.Write($"{DateTime.Now}:Error: " + ex.Message);
        }
        // Выводит ошибку в stderr
        Console.Error.WriteLine($"{DateTime.Now}:Error: " + ex.Message);
        break;
    }

}