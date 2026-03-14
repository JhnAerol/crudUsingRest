using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crudUsingRest.MVVM.Models
{
    public class Book
    {
        public string title { get; set; }
        public string author { get; set; }
        public DateTime datePublished { get; set; }
        public bool isDeleted { get; set; }
        public string id { get; set; }

    }
}
