using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceLibrary.Models
{
    public class UserModel
    {
        public int UserID { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
    }
}