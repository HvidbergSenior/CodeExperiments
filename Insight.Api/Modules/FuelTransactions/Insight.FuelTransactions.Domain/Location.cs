using Insight.BuildingBlocks.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insight.FuelTransactions.Domain
{
    public sealed class Location : ValueObject
    {
        public string Value { get; private set; }

        private Location()
        {
            Value = string.Empty;
        }

        private Location(string value)
        {
            Value = value;
        }

        public static Location Create(string value)
        {
            return new Location(value);
        }

        public static Location Empty()
        {
            return new Location();
        }
    }
}
