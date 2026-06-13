namespace Interface
{
    public class GenreDTO
    {
        public int GenreID { get; }
        public string Name { get; }

        public GenreDTO(int genreID, string name)
        {
            GenreID = genreID;
            Name = name;
        }
    }
}
