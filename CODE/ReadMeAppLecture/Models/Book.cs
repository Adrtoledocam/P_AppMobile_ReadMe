using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadMeAppLecture.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string? Title { get; set; }

        public string Epub { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }       

        public override string ToString()
        {
            return $"[Book{Id}]";
        }
    }
}
