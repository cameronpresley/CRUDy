using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Optionally;

namespace CRUDy.Domain
{
    public class Item
    {
        public string Title { get; }
        public string Description { get; }
        public int Id { get; set; }

        private Item(int id, string title, string description)
        {
            Id = id;
            Title = title;
            Description = description;
        }

        public static Item Create(string title, string description)
        {
            return new Item(-1, title, description);
        }
    }
}
