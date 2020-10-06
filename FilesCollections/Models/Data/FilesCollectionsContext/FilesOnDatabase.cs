using System;
using System.Collections.Generic;

namespace FilesCollections.Models.Data.FilesCollectionsContext
{
    public partial class FilesOnDatabase
    {
        public int FileId { get; set; }
        public string Name { get; set; }
        public string FileType { get; set; }
        public string Extension { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedOn { get; set; }
        public byte[] Data { get; set; }
    }
}
