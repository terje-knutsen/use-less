using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseLess.Framework;

namespace UseLess.Domain.Enumerations
{
    public sealed class BudgetState : Enumeration
    {
        public static BudgetState Active = new(1, "ACTIVE");
        public static BudgetState Deleted = new(2,"DELETED");
        public BudgetState(int id, string name) : base(id, name)
        { }
    }
}
