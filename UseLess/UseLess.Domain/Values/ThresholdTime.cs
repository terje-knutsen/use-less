using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseLess.Framework;

namespace UseLess.Domain.Values
{
    public sealed class ThresholdTime : Value<ThresholdTime>
    {
        private readonly DateTime value;

        private ThresholdTime(DateTime value) => this.value = value;
        public static ThresholdTime From(DateTime value)=> new ThresholdTime(value);
        public static implicit operator DateTime(ThresholdTime value) => value?.value ?? default;   
        public override CompareResult CompareTo(ThresholdTime? other)
        {
            if (other is null || other == default(DateTime))
                return CompareResult.GREATER;
            if (other == value)
                return CompareResult.EQUAL;
            if (value > other)
                return CompareResult.GREATER;
            return CompareResult.LESS;
        }

        public override string ToString()
        => value.ToString("yy.MM.dd HH:mm");
    }
}
