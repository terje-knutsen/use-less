using UseLess.Framework;

namespace UseLess.Domain.Values
{
    public sealed class IncomeId : Value<IncomeId>
    {
        private readonly Guid value;
        private IncomeId(Guid value)=> this.value = value;
        protected override bool CompareProperties(IncomeId? other)
        => value == other?.value;
        public static IncomeId From(Guid guid) => new(guid);
        public static IncomeId From(string value) => new(Guid.Parse(value));
        public static implicit operator Guid(IncomeId self)=> self?.value ?? Guid.Empty;
        public override string ToString()
        => value.ToString();
    }
}
