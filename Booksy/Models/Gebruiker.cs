namespace Booksy.Models
{
    public class Gebruiker
    {
        public int GebruikerID { get; set; }
        public string? Naam { get; set; }
        public string? Email { get; set; }

        public void VoegBoekToe(Boek boek) { }
        public List<Boek> GetBoeken() => new List<Boek>();
        public void VerwijderBoek(int boekID) { }
    }
}