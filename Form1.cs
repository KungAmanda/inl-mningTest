using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace inlämningTest
{
    public partial class Form1 : Form
    {
        // gör så att den kan kontakta databasen 
        MySqlConnection conn;
        private string connectionString;

        public Form1()
        {
            InitializeComponent();

            //Bygg upp MySQLCOnneection obejkt
            string server = "localhost";
            string database = "databoos";
            string user = "root";
            string password = "Simon123";

            string connString = $"SERVER={server};DATABASE={database};UID={user};PASSWORD={password};";

            conn = new MySqlConnection(connString);
        }

       /* private bool ValidateTextBoxes()
        {
             bool isValid = true;

            //loop genom alla textboxar i formuläret
            foreach (Control control in this.Controls)
            {
                if (control is TextBox)
                {
                    TextBox textBox = control as TextBox;
                    if (string.IsNullOrWhiteSpace(textBox.Text))
                    {
                        textBox.BackColor = Color.IndianRed;
                        isValid = false;
                    }
                    else
                    {
                        textBox.BackColor = SystemColors.Window;
                    }
                }
            }

            if (!isValid)
            {
                MessageBox.Show("Please fill in all the fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return isValid;
        }*/

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {    
            // Gör en ny koppling 
            string connectionString = "Server=localhost;Port=3306;Database=databoos;Uid=root;Pwd=Simon123;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM film", connection);

            // Så att datan kan visa sig på griden när man klickar på sök knappen
            try
            {
                connection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Validera att alla textboxar är fyllda
            if (string.IsNullOrWhiteSpace(filmBox1.Text) || string.IsNullOrWhiteSpace(genreBox2.Text) || 
                string.IsNullOrWhiteSpace(regiBox3.Text) || string.IsNullOrWhiteSpace(ratingBox4.Text))
            {
                MessageBox.Show("Alla fält måste vara ifyllda!");
                return;
            }

            // Hämta värden från textboxarna
            string filmBox1Value = filmBox1.Text;
            string genreBox4Value = genreBox2.Text;
            string regiBox2Value = regiBox3.Text;
            string ratingBox5Value = ratingBox4.Text;

            // Skapa en anslutning till MySQL Workbench
            string connectionString = "Server=localhost;Database=databoos;Uid=root;Pwd=Simon123;";
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                // Öppna anslutningen
                connection.Open();

                // Skapa och exekvera SQL-fråga för att lägga till data i databasen
                string query = $"INSERT INTO film (Film, Regissör, Genre, Rating) VALUES ('{filmBox1Value}', '{regiBox2Value}', '{genreBox4Value}', '{ratingBox5Value}')";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();

                // Visa bekräftelsemeddelande
                MessageBox.Show("Filmen har nu sparats! Du kan klicka på 'hämta inlagda filmer' knappen för att se den in tabellen!");
            }
            catch (Exception ex)
            {
                // Visa felmeddelande
                MessageBox.Show(ex.Message);
            }
            finally
            {
                // Stäng anslutningen
                connection.Close();
            }


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

            // Ange information för att ansluta till databasen
            string connectionString = "Server=localhost;Port=3306;Database=databoos;Uid=root;Pwd=Simon123;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand cmd;
            string searchText = textBox3.Text; // Spara det som skrivs i textBox3 i en variabel

            // Kontrollera vilken tabell som ska sökas i beroende på innehållet i textBox3
            if (searchText.Contains("film"))
            {
                cmd = new MySqlCommand("SELECT * FROM film WHERE Film LIKE '%" + searchText + "%'", connection);
            }
            else if (searchText.Contains("genre"))
            {
                cmd = new MySqlCommand("SELECT * FROM genre WHERE Genre LIKE '%" + searchText + "%'", connection);
            }
            else if (searchText.Contains("reggisör"))
            {
                cmd = new MySqlCommand("SELECT * FROM regissör WHERE Regissör LIKE '%" + searchText + "%'", connection);
            }
            else
            {
                cmd = new MySqlCommand("SELECT * FROM film WHERE Film LIKE '%" + searchText + "%' OR Genre LIKE '%" + searchText + "%' OR Regissör LIKE '%" + searchText + "%'", connection);
            }

            try
            {
                connection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt; // Visa resultatet i dataGridView1
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            string connectionString = "Server=localhost;Port=3306;Database=databoos;Uid=root;Pwd=Simon123;";
            MySqlConnection connection = new MySqlConnection(connectionString);

            // Skapar en SQL-fråga för att uppdatera en rad i tabellen 'film' med de värden som finns i textboxarna
            string updateQuery = "UPDATE film SET Film = @Film, Regissör = @Regissör, Genre = @Genre, Rating = @Rating WHERE Film = @Film";
            MySqlCommand cmd = new MySqlCommand(updateQuery, connection);

            // Lägger till värdena i textboxarna som parametrar i SQL-frågan
            cmd.Parameters.AddWithValue("@Film", filmBox1.Text);
            cmd.Parameters.AddWithValue("@Regissör", regiBox3.Text);
            cmd.Parameters.AddWithValue("@Genre", genreBox2.Text);
            cmd.Parameters.AddWithValue("@Rating", ratingBox4.Text);

            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Data har uppdaterats!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }





        }
    }
}
