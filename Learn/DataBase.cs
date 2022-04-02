using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Realms;
using MongoDB.Bson;

namespace Learn
{
    public class VocabularyList
    {
        public string Learn { get; set; }
        public string Translate { get; set; }
        public string LearnText { get; set; }
        public string TranslateText { get; set; }
        public string Translit { get; set; }
        public bool Remembered { get; set; }
        public string URL { get; set; }
    }

    public class Bases : RealmObject
    {
        [PrimaryKey]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
        public string Name { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public bool Active { get; set; }
        public IList<Vocabulary> Vocabulary { get; }
    }

    public class Vocabulary : RealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

        public string Learn { get; set; }
        public string Translate { get; set; }

        public string LearnText { get; set; }
        public string TranslateText { get; set; }

        public string Translit { get; set; }
        public string URL { get; set; }
        public int LookAT { get; set; }

        public bool Remembered { get; set; }
    }

    public class RememberedWordList
    {
        public string Learn { get; set; }
    }
    public class RememberedWord : RealmObject
    {
        public string Learn { get; set; }
    }

    public static class DataBase
    {
        public static Realm realm;

        public static void Init(string currentBase)
        {
            var config = new RealmConfiguration(currentBase)
            {
                IsReadOnly = false,
                ShouldDeleteIfMigrationNeeded = true,
            };
            realm = Realm.GetInstance(config);

            
        }

        public static void Remove(Vocabulary vocabulary = null)
        {
            realm.Write(() =>
            {
                realm.Remove(vocabulary);
            });
        }

        public static void Add(Vocabulary vocabulary)
        {
            realm.Write(() =>
            {
                realm.Add(vocabulary);
            });
        }


    }
}
