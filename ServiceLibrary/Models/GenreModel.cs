namespace ServiceLibrary.Models
{
    public class GenreModel
    {
        public int GenreID { get; }
        public string Name { get; }

        public GenreModel(int genreID, string name)
        {
            GenreID = genreID;
            Name = name;
        }
    }
}
