using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoneyManager.Model
{
    [Table("Children")]
    public class Children
    {
        [PrimaryKey, AutoIncrement, Column("Id")]
        public int id { get; set; }

        [ForeignKey(typeof(FamilyRecord)), Column("F_Id")]
        public int FamilyId { get; set; }
        [Column("Child_Name")]
        public string ChildName { get; set; }
        [ManyToOne, Column("Family_Col")]
        public FamilyRecord family { get; set; }

    }
}