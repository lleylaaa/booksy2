namespace ServiceLibrary.Models
{
    // FR-10 / B-10-01: een boek heeft precies een van deze leesstatussen.
    public enum ReadingStatus
    {
        WilIkLezen,
        Bezig,
        Gelezen
    }

    public static class ReadingStatusExtensions
    {
        // De waarde zoals die in de database/DTO staat en aan de gebruiker getoond wordt.
        public static string ToText(this ReadingStatus status) => status switch
        {
            ReadingStatus.WilIkLezen => "Wil ik lezen",
            ReadingStatus.Bezig => "Bezig",
            ReadingStatus.Gelezen => "Gelezen",
            _ => "Wil ik lezen"
        };

        // Zet de opgeslagen tekst weer om naar de enum. Onbekende of lege waarde
        // valt terug op "Wil ik lezen".
        public static ReadingStatus FromText(string? text) => text switch
        {
            "Bezig" => ReadingStatus.Bezig,
            "Gelezen" => ReadingStatus.Gelezen,
            _ => ReadingStatus.WilIkLezen
        };

        public static bool IsValid(string? text) =>
            text == "Wil ik lezen" || text == "Bezig" || text == "Gelezen";
    }
}
