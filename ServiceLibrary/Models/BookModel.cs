using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceLibrary.Models
{
    public class BookModel
    {
        public int BookID { get; set; }
        public string? Name { get; set; }
        public string? Author { get; set; }
        public string? Genre { get; set; }
        public int? Rating { get; set; }
    }
}