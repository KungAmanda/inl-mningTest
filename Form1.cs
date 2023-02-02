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

        public Form1()
        {
            InitializeComponent();

        }



        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Gör en ny koppling 
            string connectionString = "Server=localhost;Port=3306;Database=databoos;Uid=root;Pwd=Simon123!;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM film", connection);

            // Så att datan kan visa sig på griden när man klickar på hämta knappen
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





            // Kollar så att alla textboxar är fyllda

            if (string.IsNullOrWhiteSpace(filmBox1.Text) || string.IsNullOrWhiteSpace(genreBox2.Text) ||
                string.IsNullOrWhiteSpace(regiBox3.Text) || string.IsNullOrWhiteSpace(ratingBox4.Text))
            {
                MessageBox.Show("Alla fält måste vara ifyllda!");
                return;
            }

            // Hämtar värden från textboxarna
            string filmBox1Value = filmBox1.Text;
            string genreBox4Value = genreBox2.Text;
            string regiBox2Value = regiBox3.Text;
            string ratingBox5Value = ratingBox4.Text;

            // Gör en ny koppling
            string connectionString = "Server=localhost;Database=databoos;Uid=root;Pwd=Simon123!;";
            MySqlConnection connection = new MySqlConnection(connectionString);

            //Kollar så de står nått i textboxarna samt sparar värdet 
            try
            {

                connection.Open();

                string query = $"INSERT INTO film (Film, Regissör, Genre, Rating) VALUES ('{filmBox1Value}', '{regiBox2Value}', '{genreBox4Value}', '{ratingBox5Value}')";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();


                MessageBox.Show("Filmen har nu sparats! Du kan klicka på 'hämta inlagda filmer' knappen för att se den in tabellen!");
            }
            catch (Exception ex)
            {
                // Detta vissas om det inte finns nått i textboxarna AKA validering 
                MessageBox.Show(ex.Message);
            }
            finally
            {

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

            // Gör en ny koppling 
            string connectionString = "Server=localhost;Port=3306;Database=databoos;Uid=root;Pwd=Simon123!;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand cmd;

            // Sparar det som skrivs textbox i en variabel
            string searchText = textBox3.Text;

            // Kollar vilken tabell som ska sökas i 
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
                // 
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

        private void button2_Click(object sender, EventArgs e)
        {
            // Skapa en ny koppling
            string connectionString = "Server=localhost;Port=3306;Database=databoos;Uid=root;Pwd=Simon123!;";
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                
                connection.Open();

                // Gör en commando för att göra sql kommando
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;

                // Loop för rader i dataGridView1
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    // Kollar om raden har ändrats
                    if (row.Cells[0].Value != null)
                    {
                        // sql-kommando för att uppdatera tabellen
                         cmd.CommandText = "UPDATE film SET Film = @Film, Genre = @Genre, Regissör = @Regissör, Rating = @Rating WHERE Film_id = @Film_id";

                        
                        // Lägg till parametrar för  skydd mot sql-injection
                         cmd.Parameters.AddWithValue("@Film", row.Cells[1].Value);
                         cmd.Parameters.AddWithValue("@Genre", row.Cells[3].Value);
                         cmd.Parameters.AddWithValue("@Regissör", row.Cells[2].Value);
                         cmd.Parameters.AddWithValue("@Rating", row.Cells[4].Value);
                         cmd.Parameters.AddWithValue("@Film_id", row.Cells[0].Value); 

                        // Exekvera kommandot
                        cmd.ExecuteNonQuery();

                        // Rensar parametrarna 
                        cmd.Parameters.Clear();
                    }
                }

                 //Medd som visar att det sparats
                MessageBox.Show("Ändringarna har sparats!");
            }
            catch (Exception ex)
            {
                // Medd om nått är fel
                MessageBox.Show("Ett fel uppstod: " + ex.Message);
            }
            finally
            {
                
                connection.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Kollar så en rad är markerad
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Fråga användaren om de är säkra på att de vill ta bort den valda raden
                DialogResult result = MessageBox.Show("Är du säker på att du vill ta bort den valda raden?", "Bekräfta", MessageBoxButtons.YesNo);

                // Om ja, tar detta bort den raden
                if (result == DialogResult.Yes)
                {
                    string connectionString = "Server=localhost;Port=3306;Database=databoos;Uid=root;Pwd=Simon123!;";
                    MySqlConnection connection = new MySqlConnection(connectionString);
                    MySqlCommand cmd = new MySqlCommand("DELETE FROM film WHERE Film_id = @Film_id", connection);
                    cmd.Parameters.AddWithValue("@Film_id", Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value));

                    try
                    {
                        connection.Open();
                        cmd.ExecuteNonQuery();

                        // Uppdatera griden
                        RefreshDataGridView();
                    }
                  
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            else
            {    // Medd om rad inte är markerad 
                MessageBox.Show("Vänligen välj en rad att ta bort.");
            }
        }

        //funktion för att update ze grid
        private void RefreshDataGridView()
        {
            
            string connectionString = "Server=localhost;Port=3306;Database=databoos;Uid=root;Pwd=Simon123!;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM film", connection);

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

    }
}
