using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilesCollections.Models.Data.FilesCollectionsContext
{
    public class FileFromDbAndFileSystemViewModel
    {
         public List<FilesOnFileSystem> FilesOnFileSystem { get; set; }

        public List<FilesOnDatabase> FilesOnDatabase { get; set; }
    }
}
