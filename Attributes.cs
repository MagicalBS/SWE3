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
        private string name;

        public Column(string name = "")
        {
            this.name = name;
        }

        public override string ToString()
        {
            return $"Column: {name}";
        }
    }
}