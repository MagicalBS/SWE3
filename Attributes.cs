using System;

namespace Linq
{
    public class Table : Attribute
    {
        public string Name;

        public Table(string name = "")
        {
            this.Name = name;
        }

        public override string ToString()
        {
            return $"Table: {Name}";
        }
    }

    public class Column : Attribute
    {
        public string Name;

        public Column(string name = "")
        {
            this.Name = name;
        }

        public override string ToString()
        {
            return $"Column: {Name}";
        }
    }
}