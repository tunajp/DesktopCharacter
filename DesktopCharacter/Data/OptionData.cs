using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace DesktopCharacter.Data
{
    public class Option
    {
        [PrimaryKey, MaxLength(1024)]
        public string Name { get; set; }

        public string Value { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class OptionData
    {
        private SQLiteConnection db;

        public OptionData(SQLiteConnection _db)
        {
            this.db = _db;
        }

        public string getLanguage()
        {
            db.CreateTable<Option>();

            string lang = "Default";
            var query = db.Table<Option>().Where(v => v.Name == "Language");
            if (query.Count() == 0)
            {
                // Set default value
                AddOption("Language", lang);
                return lang;
            }
            foreach (var language in query)
            {
                System.Diagnostics.Trace.WriteLine(language.Name + " " + language.Value);
                lang = language.Value;
            }
            return lang;
        }
        public void setLanguage(string lang)
        {
            this.UpdateOption("Language", lang);
        }

        private void AddOption(string name, string value)
        {
            var s = db.Insert(new Option()
            {
                Name = name,
                Value = value,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            });
        }

        private void UpdateOption(string key, string value)
        {
            Option option = null;
            var query = db.Table<Option>().Where(v => v.Name == key);
            foreach (var q in query)
            {
                option = q;
            }
            option.Value = value;
            option.UpdatedAt = DateTime.Now;
            var s = db.Update(option);
        }
    }
}
