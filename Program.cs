using CarPark.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using static System.Console;

namespace CarPark
{
    class Program
    {
        static string connectionString = "Server=(local);Database=CarPark;Integrated Security=True";
        static void Main(string[] args)
        {
            bool shouldExit = false;

            while (!shouldExit)
            {
                WriteLine("1. Register arrival");
                WriteLine("2. Register departure");
                WriteLine("3. Show registry");
                WriteLine("4. Exit");

                ConsoleKeyInfo keyPressed = ReadKey(true);

                Clear();

                switch (keyPressed.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        {
                            Write("First name: ");

                            string firstName = ReadLine();

                            Write("Last name: ");

                            string lastName = ReadLine();

                            Write("Social security number: ");

                            string socialSecurityNumber = ReadLine();

                            // Ersätt "Contact details" med följande

                            Write("Phone number: ");

                            string phoneNumber = ReadLine();

                            Write("E-mail: ");

                            string email = ReadLine();

                            Write("Registration number: ");

                            string registrationNumber = ReadLine();

                            Write("Notes: ");

                            string notes = ReadLine();

                            Parking parking = new Parking(firstName,
                                lastName,
                                socialSecurityNumber,
                                phoneNumber,
                                email,
                                registrationNumber,
                                notes,
                                DateTime.Now);

                            SaveParking(parking);

                            ShowNotice("Parking registered");
                        }

                        break;

                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        {
                            Write("Registration number: ");

                            string registrationNumber = ReadLine();

                            Clear();

                            if (RegisterDeparture(registrationNumber))
                            {
                                ShowNotice("Departure registered");
                            }
                            else
                            {
                                ShowNotice("Car not found");
                            }
                        }

                        break;

                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        {
                            List<Parking> parkingList = FetchAllParkings();

                            Write("Reg. nr".PadRight(20, ' '));
                            Write("Arrival".PadRight(25, ' '));
                            WriteLine("Departure");

                            WriteLine("-------------------------------------------------------------------");

                            foreach (Parking parking in parkingList)
                            {
                                string registrationNumber = parking.RegistrationNumber.PadRight(20, ' ');
                                string arrivedAt = parking.ArrivedAt.ToString().PadRight(25, ' ');
                                string departedAt = parking.DepartedAt.ToString();

                                Write(registrationNumber);
                                Write(arrivedAt);
                                WriteLine(departedAt);
                            }

                            ReadKey(true);
                        }

                        break;

                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:

                        shouldExit = true;

                        break;
                }

                Clear();
            }
        }

        static void SaveParking(Parking parking)
        {
            string cmdText = @"
                INSERT INTO Parking (
                    FirstName, 
                    LastName,
                    SocialSecurityNumber,
                    PhoneNumber,
                    Email,
                    RegistrationNumber,
                    Notes,
                    ArrivedAt
                ) VALUES (
                    @FirstName, 
                    @LastName,
                    @SocialSecurityNumber,
                    @PhoneNumber,
                    @Email,
                    @RegistrationNumber,
                    @Notes,
                    @ArrivedAt
                )
            ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(cmdText, connection))
            {
                command.Parameters.AddWithValue("FirstName", parking.FirstName);
                command.Parameters.AddWithValue("LastName", parking.LastName);
                command.Parameters.AddWithValue("SocialSecurityNumber", parking.SocialSecurityNumber);
                command.Parameters.AddWithValue("PhoneNumber", parking.PhoneNumber);
                command.Parameters.AddWithValue("Email", parking.Email);
                command.Parameters.AddWithValue("RegistrationNumber", parking.RegistrationNumber);
                command.Parameters.AddWithValue("Notes", parking.Notes);
                command.Parameters.AddWithValue("ArrivedAt", DateTime.Now);

                connection.Open();

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        static bool RegisterDeparture(string registrationNumber)
        {
            string cmdText = $@"UPDATE Parking
                                   SET DepartedAt ='{DateTime.Now}'
                                 WHERE RegistrationNumber = @RegistrationNumber
                                   AND DepartedAt IS NULL";

            int result = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(cmdText, connection))
            {
                command.Parameters.AddWithValue("RegistrationNumber", registrationNumber);

                connection.Open();

                result = command.ExecuteNonQuery();

                connection.Close();
            }

            return result > 0;
        }

        static List<Parking> FetchAllParkings()
        {
            List<Parking> parkingList = new List<Parking>();

            string cmdText = @"
                SELECT FirstName,
                       LastName,
                       SocialSecurityNumber,
                       PhoneNumber,
                       Email,
                       RegistrationNumber,
                       Notes,
                       ArrivedAt,
                       DepartedAt
                  FROM Parking
            ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(cmdText, connection))
            {
                connection.Open();

                SqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    string firstName = dataReader["FirstName"].ToString();
                    string lastName = dataReader["LastName"].ToString();
                    string socialSecurityNumber = dataReader["SocialSecurityNumber"].ToString();

                    string phoneNumber = dataReader["PhoneNumber"].ToString();
                    string email = dataReader["Email"].ToString();

                    string registrationNumber = dataReader["RegistrationNumber"].ToString();
                    string notes = dataReader["Notes"].ToString();

                    DateTime arrivedAt = DateTime.Parse(dataReader["ArrivedAt"].ToString());

                    DateTime? departedAt = null;

                    if (DateTime.TryParse(dataReader["DepartedAt"].ToString(), out DateTime date))
                    {
                        departedAt = date;
                    }

                    Parking parking = new Parking(
                        firstName,
                        lastName,
                        socialSecurityNumber,
                        phoneNumber,
                        email,
                        registrationNumber,
                        notes,
                        arrivedAt,
                        departedAt);

                    parkingList.Add(parking);
                }

                connection.Close();
            }

            return parkingList;
        }

        static void ShowNotice(string message)
        {
            Clear();

            WriteLine(message);

            Thread.Sleep(2000);
        }
    }
}