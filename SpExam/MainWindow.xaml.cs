using SpExam.DataAccess;
using SpExam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpExam
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int[] Numbers = new int[0];
        private object _locker = new object();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartButtonClick(object sender, RoutedEventArgs e)
        {
            int countNumbers;
            if (int.TryParse(countTextBox.Text, out countNumbers))
            {
                for (int i = 0; i < countNumbers; i++)
                {
                   ThreadPool.QueueUserWorkItem(AddNumber);
                }
            }
            else
                MessageBox.Show("Enter a valid number");
        }
        private void AddNumber(object index)
        {
            lock (_locker)
            {
                Array.Resize(ref Numbers, Numbers.Length+1);
                Numbers[Numbers.Length - 1] = Numbers.Length-1;
            }
        }

        private async void LoadButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(urlTextBox.Text))
                {
                    MessageBox.Show("Enter a download link");
                }
                else if (string.IsNullOrWhiteSpace(fileNameTextBox.Text))
                {
                    MessageBox.Show("Enter a file name");
                }
                else
                {
                    using (var client = new WebClient())
                    {
                        client.DownloadFileAsync(new Uri(urlTextBox.Text), fileNameTextBox.Text);
                    }

                    Report report = new Report()
                    {
                        LoadDate = DateTime.Now,
                        LoadResult = "Загружен успешно"
                    };
                    using (var context = new LoaderContext())
                    {
                        context.Reports.Add(report);
                        await context.SaveChangesAsync();
                    }
                    MessageBox.Show("Файл успешно загружен");
                }
            }
            catch(UriFormatException)
            {
                MessageBox.Show("Недопустимый URI");

                Report report = new Report()
                {
                    LoadDate = DateTime.Now,
                    LoadResult = "Недопустимый URI"
                };
                using (var context = new LoaderContext())
                {
                    context.Reports.Add(report);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
