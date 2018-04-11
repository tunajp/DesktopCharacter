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
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        
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

        public int MotionId { get; set; }

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

    }
}
