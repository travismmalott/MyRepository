﻿using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Threading;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var dt = new DispatcherTimer();
            dt.Tick += Dt_Tick;
            dt.Start();
        }

        private int mru = int.MinValue;

        private void Dt_Tick(object sender, EventArgs e)
        {
            using (var connection = new SqlConnection("server=saftsqlserver.database.windows.net;database=mysqldatabase;user id=student;password=Pa$$w0rd"))
            {
                connection.Open();
                using (var command = new SqlCommand("select messageid, value from messages where messageid > @mru order by messageid", connection))
                {
                    command.Parameters.AddWithValue("@mru", mru);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var messageId = reader.GetInt32(0);
                            var value = reader.GetString(1);
                            mru = messageId;
                            lbMessages.Items.Add(value);
                        }
                    }
                }
            }
        }
    }
}
