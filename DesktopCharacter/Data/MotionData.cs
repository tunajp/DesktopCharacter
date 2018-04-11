﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace DesktopCharacter.Data
{
    public class Motion
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(1024)]
        public string Name { get; set; }

        public string Url { get; set; }
        // 0:yet 1:complete
        public int DownloadStatus { get; set; }

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
    }
}
