using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace DesktopCharacter.Data
{
    public class Model
    {
        [PrimaryKey]
        public string Guid { get; set; }
        
        public bool Enabled { get; set; }

        [MaxLength(1024)]
        public string Name { get; set; }

        public string Url { get; set; }
        public int LatestVersion { get; set; }
        public string LicenceFile { get; set; }
        // 0:yet 1:complete
        public int DownloadStatus { get; set; }
        public int LocalVersion { get; set; }

        public string FileName { get; set; }

        public string MotionGuid { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ModelData
    {
        private SQLiteConnection db;

        public ModelData(SQLiteConnection _db)
        {
            this.db = _db;
        }

        public List<Model> getEnabledModels(string directory)
        {
            db.CreateTable<Model>();

            if (this.getAllModels().Count() == 0)
            {
                this.initialize(directory);
            }

            var query = db.Table<Model>().Where(v => v.Enabled == true);
            List<Model> list = new List<Model>();
            foreach (var q in query)
            {
                list.Add(q);
            }
            return list;
        }

        public List<Model> getAllModels()
        {
            db.CreateTable<Model>();

            var query = db.Table<Model>();
            List<Model> list = new List<Model>();
            foreach (var q in query)
            {
                list.Add(q);
            }
            return list;
        }

        private void initialize(string directory)
        {
            db.Insert(new Model()
            {
                Guid = "4c738631-1a82-40da-a08d-51ba8d06011f",
                Enabled = true,
                Name = "Lat式ミク",
                Url = "",
                LatestVersion = 231,
                DownloadStatus = 1,
                LocalVersion = 231,
                FileName = System.IO.Path.Combine(directory, "data", "Lat式ミクVer2.31", "Lat式ミクVer2.31_Normal.pmd"),
                MotionGuid = "20222ace-f86c-4e7e-ad28-b81c711fe028",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            });
        }

        public void AddModel(Model model)
        {
            model.CreatedAt = DateTime.Now;
            var s = db.Insert(model);
        }

        public void UpdateModel(Model model)
        {
            model.UpdatedAt = DateTime.Now;
            var s = db.Update(model);
        }
    }
}
