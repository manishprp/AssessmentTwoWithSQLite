using Android.App;
using Android.Widget;
using System.Collections.Generic;
using SQLite;
using MoneyManager.Model;
using SQLiteNetExtensions.Extensions;
using System.Linq;

namespace MoneyManager
{
    public static class DataBaseClass
    {
        static string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        public static bool createDatabase<T>()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Persons.db")))
                {
                    connection.CreateTable<T>();
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Toast.MakeText(Application.Context, ex.Message, ToastLength.Short).Show();
                return false;
            }
        }

        public static bool DeleteFamily (FamilyRecord familyRecord)
        {
            try
            {
                using (var connection= new SQLiteConnection(System.IO.Path.Combine(folder, "Persons.db")))
                {
                    connection.Delete(familyRecord,true);
                }
                return true;
            }
            catch(SQLiteException ex)
            {
                Toast.MakeText(Application.Context, ex.Message, ToastLength.Short).Show();
                return false;
            }
        }

        public static FamilyRecord GetFamilyWithId(int famid)
        {
            var familyRecord = new FamilyRecord();
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Persons.db")))
                {
                    familyRecord = GetFamily().Where(u => u.id == famid).FirstOrDefault();
                }
                return familyRecord;
            }
            catch (SQLiteException ex)
            {
                Toast.MakeText(Application.Context, ex.Message, ToastLength.Short).Show();
                return null;
            }
        }


        public static bool DeleteFamily(int famid)
        {
            var familyRecord = new FamilyRecord();
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Persons.db")))
                {
                    connection.Delete(GetFamilyWithId(famid));
                }
                return true;
            }
            catch (SQLiteException ex)
            {
                Toast.MakeText(Application.Context, ex.Message, ToastLength.Short).Show();
                return false;
            }
        }

        public static bool UpdateFamily(FamilyRecord person, int id)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Persons.db")))
                {
                    person.id = id;
                        connection.InsertOrReplaceWithChildren(person, true);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Toast.MakeText(Application.Context, ex.Message, ToastLength.Short).Show();
                return false;
            }
        }

        public static bool insertIntoTable(FamilyRecord person)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Persons.db")))
                {
                    connection.InsertWithChildren(person, true);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Toast.MakeText(Application.Context, ex.Message, ToastLength.Short).Show();
                return false;
            }
        }

        public static List<FamilyRecord> GetFamily()
        {
            List<FamilyRecord> familyList = new List<FamilyRecord>();
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Persons.db")))
                {
                    familyList= connection.GetAllWithChildren<FamilyRecord>();
                }
                return familyList;
            }
            catch (SQLiteException ex)
            {
                Toast.MakeText(Application.Context, ex.Message, ToastLength.Short).Show();
                return null;
            }
        }

        public static bool removeTable<T>(T person)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Persons.db")))
                {
                    connection.Delete(person, true);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Toast.MakeText(Application.Context, ex.Message, ToastLength.Short).Show();
                return false;
            }
        }
    }

}