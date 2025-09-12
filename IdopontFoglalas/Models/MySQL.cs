using MySqlConnector;

namespace IdopontFoglalas.Models
{
    public class MySQL
    {
        static string kapcsolatiString = "server=...;uid=...;pwd=...;database=...;Connection Timeout=60;";
        public static MySqlConnection kapcsolat = new MySqlConnection(kapcsolatiString);

        public static List<Foglalas> FoglalasLista()
        {
            List<Foglalas> lista = new List<Foglalas>();
            kapcsolat.Open();

            string sql = "SELECT * FROM balindbarnafoglalasok";
            MySqlCommand parancs = new MySqlCommand(sql, kapcsolat);
            MySqlDataReader reader = parancs.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new Foglalas
                {
                    Id = reader.GetInt32("id"),
                    FoglaloNev = reader.GetString("Foglalonev"),
                    Eletkor = reader.GetInt32("Eletkor"),
                    LetrehozasIdopont = reader.GetDateTime("Datum"),
                    FoglalasIdopont = reader.GetDateTime("Idopont")
                });
            }

            reader.Close();
            kapcsolat.Close();
            return lista;
        }

        //foglalas letrehozasa
        public static bool UjFoglalas(Foglalas foglalas)
        {
            bool sikerult = true;
            string query = "INSERT INTO balindbarnafoglalasok (Foglalonev, Eletkor, Datum, Idopont) VALUES (@nev, @eletkor, @datum, @idopont)";

            using (MySqlCommand cmd = new MySqlCommand(query, kapcsolat))
            {
                try
                {
                    kapcsolat.Open();
                    cmd.Parameters.AddWithValue("@nev", foglalas.FoglaloNev);
                    cmd.Parameters.AddWithValue("@eletkor", foglalas.Eletkor);
                    cmd.Parameters.AddWithValue("@datum", foglalas.LetrehozasIdopont);
                    cmd.Parameters.AddWithValue("@idopont", foglalas.FoglalasIdopont);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    sikerult = false;
                }
                finally
                {
                    if (kapcsolat.State == System.Data.ConnectionState.Open)
                        kapcsolat.Close();
                }
            }

            return sikerult;
        }

        //foglalas torlese
        public static bool FoglalasTorles(int id)
        {
            bool sikerult = true;
            string query = "DELETE FROM balindbarnafoglalasok WHERE id = @id";

            using (MySqlCommand cmd = new MySqlCommand(query, kapcsolat))
            {
                try
                {
                    kapcsolat.Open();
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    sikerult = false;
                }
                finally
                {
                    if (kapcsolat.State == System.Data.ConnectionState.Open)
                        kapcsolat.Close();
                }
            }
            return sikerult;
        }

        public static bool SzabadeADatum(DateTime ido)
        {
            string query = "SELECT COUNT(*) FROM balindbarnafoglalasok WHERE Idopont = @idopont";
            using (MySqlCommand cmd = new MySqlCommand(query, kapcsolat))
            {
                try
                {
                    kapcsolat.Open();
                    cmd.Parameters.AddWithValue("@idopont", ido);
                    int db = Convert.ToInt32(cmd.ExecuteScalar());
                    return db > 0;
                }
                catch (Exception ex)
                {
                    return false;
                }
                finally
                {
                    if (kapcsolat.State == System.Data.ConnectionState.Open)
                        kapcsolat.Close();
                }
            }
        }
    }
}
