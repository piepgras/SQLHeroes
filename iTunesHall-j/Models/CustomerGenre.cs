using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTunesHall_j.Models
{
    public record struct CustomerGenre(int CustomerId, string FirstName, string LastName, string Genre);
}
