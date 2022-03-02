namespace UseLess.Messages
{
    public static class Events
    {
        public abstract class Event 
        {
            public Event(Guid id, DateTime entryDate)
            {
                Id = id;
                EntryDate = entryDate;
            }
            public Guid Id { get; }
            public DateTime EntryDate { get; }
        }
        public class BudgetCreated : Event 
        {
            private readonly string name;
            public BudgetCreated(Guid id, DateTime entryDate, string name)
                :base(id,entryDate)
            {
                this.name = name;
            }
            public string Name => name;
        }

        public class IncomeAdded
        {
            private readonly Guid id;
            private readonly decimal amount;
            private readonly string type;

            public IncomeAdded(Guid id, decimal amount, string type)
            {
                this.id = id;
                this.amount = amount;
                this.type = type;
            }
            public Guid Id => id;
            public decimal Amount => amount;
            public string Type => type;
        }

        public class OutgoAdded
        {
            private readonly Guid id;
            private readonly decimal amount;
            private readonly string type;

            public OutgoAdded(Guid id, decimal amount, string type)
            {
                this.id = id;
                this.amount = amount;
                this.type = type;
            }
            public Guid Id => id;
            public decimal Amount => amount;
            public string Type => type;
        }
    }
}
