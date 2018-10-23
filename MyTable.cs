namespace Linq
{
    [Table("Person")]
    class MyTable
    {
        [Column("firstname")]
        public string FirstName { get; set; }
        [Column("lastname")]
        public string LastName { get; set; }
        [Column("age")]
        public int Age { get; set; }

        public override string ToString()
        {
            return $"{FirstName} {LastName} {Age}";
        }
    }
}
