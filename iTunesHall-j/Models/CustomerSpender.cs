using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateAndAccessDatabaseFjy.Models
{
    public record struct CustomerSpender(int CustomerId, string FirstName, string LastName, decimal Spender);

}
