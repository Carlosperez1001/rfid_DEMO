﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ThingMagic;
using static System.Net.Mime.MediaTypeNames;
using Image = System.Drawing.Image;

namespace RFID_Demo
{

    public static class DBHelper
    {
        private static MySqlConnection connection;
        private static MySqlCommand cmd = null;
        private static MySqlDataAdapter da;
        private static DataSet ds;
        public static DataTable dt = new DataTable();
        public static DataTable dtCloned = new DataTable();

        private static MySqlDataAdapter sda;

        /// <summary>
        /// 
        /// </summary>
        public static void EstablishConnection()
        {
            try
            {
                //Setup AWS RDS connection for the MySQL DB
                MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
                builder.Server = "aws-instance.chisnihuqkko.us-east-2.rds.amazonaws.com";
                builder.UserID = "admin";
                builder.Password = "password";
                builder.Database = "items";
                builder.SslMode = MySqlSslMode.None;

                connection = new MySqlConnection(builder.ToString());

                Console.WriteLine("[DB Connection attempted]");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database is disconnected!");
            }

        }


        /// <summary>
        /// 
        /// </summary>
        public static void UpdateBookTable()
        {
            dt.Clear();
            string query = "Select Book_Title, Book_Autor, Book_Genre, Book_Image, Book_RFID_EPC, Book_RFID_TimeStamp, Book_RFID_RSSI from Book";
            try
            {
                if (connection != null && connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                    cmd = new MySqlCommand(query, connection);
                    da = new MySqlDataAdapter(cmd);
                    ds = new DataSet();
                    da.Fill(dt);

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public static void addBookQuery(string EPC, string TimeStamp, string RSSI, string Book_Title, string Book_Autor, string Book_Genre, Byte[] source)
        {
            String query = "INSERT INTO Book (Book_id, Book_Title, Book_Autor, Book_Genre, Book_Image, Book_RFID_EPC, Book_RFID_TimeStamp, Book_RFID_RSSI) " +
                "VALUE(@Book_id, @Book_Title, @Book_Autor, @Book_Genre, @Book_Image, @Book_RFID_EPC, @Book_RFID_TimeStamp, @Book_RFID_RSSI)";
            try
            {
                if (connection != null && connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                    cmd = new MySqlCommand(query, connection);

                    cmd.Parameters.Add("@Book_id", MySqlDbType.VarChar, 255);
                    cmd.Parameters.Add("@Book_Title", MySqlDbType.VarChar, 255);
                    cmd.Parameters.Add("@Book_Autor", MySqlDbType.VarChar, 255);
                    cmd.Parameters.Add("@Book_Genre", MySqlDbType.VarChar, 255);
                    cmd.Parameters.Add("@Book_Image", MySqlDbType.Blob);
                    cmd.Parameters.Add("@Book_RFID_EPC", MySqlDbType.VarChar, 255);
                    cmd.Parameters.Add("@Book_RFID_TimeStamp", MySqlDbType.VarChar, 255);
                    cmd.Parameters.Add("@Book_RFID_RSSI", MySqlDbType.VarChar, 255);

                    cmd.Parameters["@Book_id"].Value = Guid.NewGuid().ToString();
                    cmd.Parameters["@Book_Title"].Value = Book_Title;
                    cmd.Parameters["@Book_Autor"].Value = Book_Autor;
                    cmd.Parameters["@Book_Genre"].Value = Book_Genre;
                    cmd.Parameters["@Book_Image"].Value = source;
                    cmd.Parameters["@Book_RFID_EPC"].Value = EPC;
                    cmd.Parameters["@Book_RFID_TimeStamp"].Value = TimeStamp;
                    cmd.Parameters["@Book_RFID_RSSI"].Value = RSSI;

                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("New book add to the data base: \n" +
                            "Summary \n" +
                            "");
                    }

                    connection.Close();

                    UpdateBookTable();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database is disconnected. Can not add item.");
            }

        }


        /// <summary>
        /// /
        /// </summary>
        public static void RemoveBookQuery(string EPC)
        {

            String query = "DELETE FROM Book Where Book_RFID_EPC = '" + EPC + "'";
            if (connection != null && connection.State == ConnectionState.Closed)
            {
                connection.Open();
                cmd = new MySqlCommand(query, connection);
                if (cmd.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Item with the EPC:" + EPC + " was removed from the database.");
                }
                connection.Close();
                UpdateBookTable();
            }
        }

        public static void updateBook(TagReadDataEventArgs e)
        {
            String query = "Update Book SET Book_RFID_TimeStamp = @Book_RFID_TimeStamp, Book_RFID_RSSI = @Book_RFID_RSSI where Book_RFID_EPC = '" + e.TagReadData.EpcString + "'";
            try
            {

                if (connection != null && connection.State == ConnectionState.Closed)
                {
                    Console.WriteLine("[Update Book] " + e.TagReadData.EpcString + e.TagReadData.Time.ToString() + " -  " + e.TagReadData.Rssi);
                    connection.Open();
                    cmd = new MySqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("@Book_RFID_TimeStamp", e.TagReadData.Time.ToString());
                    cmd.Parameters.AddWithValue("@Book_RFID_RSSI", e.TagReadData.Rssi);
                    cmd.ExecuteNonQuery();

                    connection.Close();

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }


        }

        /// <summary>
        /// 
        /// </summary>
        public static DataTable GetDT()
        {

            return dt;
        }

    }
}