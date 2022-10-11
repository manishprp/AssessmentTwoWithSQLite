using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace MoneyManager.Model
{
    [Table("FamilyRecord")]
    public class FamilyRecord
    {
        [PrimaryKey, AutoIncrement, Column("Id")]
        public int id { get; set; }
        [Column("Father_Name")]
        public string FatherName { get; set; }
        [Column("Mother_Name")]
        public string MotherName { get; set; }
        [Column("Address")]
        public string Address { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Children> Child { get; set; }
    }
}