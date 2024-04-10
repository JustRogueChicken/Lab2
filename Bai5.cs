using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Lab2
{
    public partial class Bai5 : Form
    {
        public Bai5()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string inputFile = openFileDialog.FileName;
                    string outputFile = openFileDialog.FileName;
                    ProcessInputFile(inputFile, outputFile);
                }
            }
        }
        private void ProcessInputFile(string inputFile, string outputFile)
        {
            if (File.Exists(inputFile))
            {

                string[] lines = File.ReadAllLines(inputFile);
                int totalLines = lines.Length;
                progressBar.Minimum = 0;
                progressBar.Maximum = totalLines;
                progressBar.Step = 1;

                List<Movie> movies = new List<Movie>();

                for (int i = 0; i < totalLines; i += 5)
                {
                    string movieName = lines[i];
                    double ticketPrice = double.Parse(lines[i + 1]);
                    string theater = lines[i + 2];
                    int totalTickets = int.Parse(lines[i + 3]);
                    int ticketsSold = int.Parse(lines[i + 4]);
                    Movie movie = new Movie(movieName, ticketPrice, theater, totalTickets, ticketsSold);
                    movies.Add(movie);
                    progressBar.PerformStep();
                }

                using (StreamWriter writer = new StreamWriter(outputFile))
                {
                    progressBar.Value = 0;
                    progressBar.Maximum = movies.Count;
                    progressBar.Step = 1;

                    foreach (var movie in movies)
                    {
                        movie.CalculateRevenue();
                        movie.CalculateRank(movies);
                        writer.WriteLine($"Tên phim: {movie.Name}");
                        writer.WriteLine($"Giá vé: {movie.TicketPrice:C}");
                        writer.WriteLine($"Tổng số lượng vé: {movie.TotalTickets}");
                        writer.WriteLine($"Số lượng vé bán ra: {movie.TicketsSold}");
                        writer.WriteLine($"Số lượng vé tồn: {movie.AvailableTickets}");
                        writer.WriteLine($"Tỉ lệ vé bán ra: {movie.SellRate:P}");
                        writer.WriteLine($"Doanh thu: {movie.Revenue:C}");
                        writer.WriteLine($"Xếp hạng doanh thu: {movie.Rank}");
                        writer.WriteLine();
                        progressBar.PerformStep();
                    }
                }

                MessageBox.Show("Xử lý hoàn thành!");
            }
        }
    }


    public class Movie
    {
        public string Name { get; set; }
        public double TicketPrice { get; set; }
        public string Theater { get; set; }
        public int TotalTickets { get; set; }
        public int TicketsSold { get; set; }
        public double Revenue { get; set; }
        public double SellRate { get; set; }
        public int AvailableTickets { get { return TotalTickets - TicketsSold; } }
        public int Rank { get; set; }

        public Movie(string name, double ticketPrice, string theater, int totalTickets, int ticketsSold)
        {
            Name = name;
            TicketPrice = ticketPrice;
            Theater = theater;
            TotalTickets = totalTickets;
            TicketsSold = ticketsSold;
        }


        public void CalculateRevenue()
        {
            Revenue = TicketsSold * TicketPrice;
            SellRate = (double)TicketsSold / TotalTickets;
        }


        public void CalculateRank(List<Movie> movies)
        {
            movies = movies.OrderByDescending(m => m.Revenue).ToList();
            for (int i = 0; i < movies.Count; i++)
            {
                movies[i].Rank = i + 1;
            }
        }
    }
}