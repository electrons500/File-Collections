using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FilesCollections.Models.Data.FilesCollectionsContext
{
    public class FilesModel
    {
        [Key]
        public int FileId { get; set; }
        public string Name { get; set; }
        public string FileType { get; set; }
        public string Extension { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
