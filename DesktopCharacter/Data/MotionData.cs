using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace DesktopCharacter.Data
{
    public class Motion
    {
        [PrimaryKey]
        public string Guid { get; set; }

        [MaxLength(1024)]
        public string Name { get; set; }

        public string Url { get; set; }
        public int LatestVersion { get; set; }
        public string LicenceFile { get; set; }
        // 0:yet 1:complete
        public int DownloadStatus { get; set; }
        public int LocalVersion { get; set; }

        public string FileName { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class MotionData
    {
        private SQLiteConnection db;

        public MotionData(SQLiteConnection _db)
        {
            this.db = _db;
        }

        public List<Motion> getMotions(string directory)
        {
            db.CreateTable<Model>();

            if (this.getAllMotions().Count() == 0)
            {
                this.initialize(directory);
            }

            return this.getAllMotions();
        }

        public List<Motion> getAllMotions()
        {
            db.CreateTable<Motion>();

            var query = db.Table<Motion>();
            List<Motion> list = new List<Motion>();
            foreach (var q in query)
            {
                list.Add(q);
            }
            return list;
        }

        private void initialize(string directory)
        {
            db.Insert(new Motion()
            {
                Guid = "ae61d85b-2bba-4ed7-b57b-f630f3a3d30b",
                Name = "女の子の撮影モーション(カメラアピール編)",
                Url = "",
                LatestVersion = 1,
                DownloadStatus = 1,
                LocalVersion = 1,
                FileName = System.IO.Path.Combine(directory, "data", "女の子の撮影モーション(カメラアピール編)", "ミク.vmd"),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            });
            db.Insert(new Motion()
            {
                Guid = "20222ace-f86c-4e7e-ad28-b81c711fe028",
                Name = "女の子の撮影モーション(寝そべり編)",
                Url = "",
                LatestVersion = 1,
                DownloadStatus = 1,
                LocalVersion = 1,
                FileName = System.IO.Path.Combine(directory, "data", "女の子の撮影モーション(寝そべり編)", "女の子の撮影モーション(寝そべり編).vmd"),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            });
        }

        public void AddMotion(Motion motion)
        {
            motion.CreatedAt = DateTime.Now;
            var s = db.Insert(motion);
        }

        public void UpdateMotion(Motion motion)
        {
            motion.UpdatedAt = DateTime.Now;
            var s = db.Update(motion);
        }
    }
}
