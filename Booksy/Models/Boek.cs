namespace Booksy.Models
{
    public class Boek
    {
        public int BoekID { get; set; }
        public int GebruikerID { get; set; }
        public string? Naam { get; set; } 
        public string? Auteur { get; set; }
        public string? Genre { get; set; }
        public string? GetNaam() => Naam;
        public string? GetAuteur() => Auteur;
        public string? GetGenre() => Genre;
    }
}
