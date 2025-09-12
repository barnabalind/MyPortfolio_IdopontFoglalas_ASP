using System.ComponentModel.DataAnnotations;

namespace IdopontFoglalas.Models
{
    public class Foglalas
    {
        public int Id { get; set; }
        public string FoglaloNev { get; set; }
        public int Eletkor { get; set; }
        public DateTime LetrehozasIdopont { get; set; } = DateTime.Now;
        public DateTime FoglalasIdopont { get; set; }

        public override string ToString()
        {
            return $"{FoglaloNev} - {FoglalasIdopont:yyyy-MM-dd HH:mm}";
        }

    }

}
