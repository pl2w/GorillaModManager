using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GorillaModManager.Models
{
    public class BlockList
    {
        public List<int> blockedAuthors { get; set; } = [];
        public List<int> blockedPages { get; set; } = [];
    }
}
