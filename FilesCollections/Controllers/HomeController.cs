 using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FilesCollections.Models;
using FilesCollections.Models.Data.FilesCollectionsContext;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace FilesCollections.Controllers
{
    public class HomeController : Controller
    {
        private readonly FilesCollectionsContext _Context;

        public HomeController(FilesCollectionsContext context)
        {
            _Context = context;
        }

        public IActionResult Index()
        {
            var fileuploadViewModel = GetFiles();
            ViewBag.Message = TempData["Message"];
            return View(fileuploadViewModel);
        }
        [HttpPost]
        public IActionResult UploadToFileSystem(List<IFormFile> Files, string description)
        {
         
            
            foreach (var file in Files)
            {
                var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\FilesUploads\\");
                bool basePathExists = System.IO.Directory.Exists(basePath);
                if (!basePathExists) Directory.CreateDirectory(basePath);
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var filePath = Path.Combine(basePath, file.FileName);
                var extension = Path.GetExtension(file.FileName);
                if (!System.IO.File.Exists(filePath))
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    FilesOnFileSystem model = new FilesOnFileSystem
                    {
                        Name = fileName,
                        FileType = file.ContentType,
                        Extension = extension,
                        Description = description,
                        CreatedOn = DateTime.UtcNow,
                        FilePath = filePath
                    };

                    _Context.FilesOnFileSystem.Add(model);
                    _Context.SaveChanges();
                }

            }
            TempData["Message"] = "File successfully uploaded to File System.";
            return RedirectToAction("Index");
        }
     

        [HttpPost]
        public IActionResult UploadToDatabase(List<IFormFile> Files, string description)
        {
            foreach (var file in Files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var extension = Path.GetExtension(file.FileName);
                var model = new FilesOnDatabase
                {
                    CreatedOn = DateTime.UtcNow,
                    FileType = file.ContentType,
                    Extension = extension,
                    Name = fileName,
                    Description = description
                };
                using (var stream = new MemoryStream())
                {
                     file.CopyTo(stream);
                    model.Data = stream.ToArray();
                }
                _Context.FilesOnDatabase.Add(model);
                _Context.SaveChanges();
            }
            TempData["Message"] = "File successfully uploaded to Database";
            return RedirectToAction("Index");
        }


        public FileFromDbAndFileSystemViewModel GetFiles()
        {
            var model = new FileFromDbAndFileSystemViewModel
            {
                FilesOnDatabase = _Context.FilesOnDatabase.ToList(),
                FilesOnFileSystem = _Context.FilesOnFileSystem.ToList()
            };
            return model;
        }
        public IActionResult DownloadFileFromDatabase(int id)
        {

            var file = _Context.FilesOnDatabase.Where(x => x.FileId == id).FirstOrDefault();
            if (file == null) return null;
            return File(file.Data, file.FileType, file.Name + file.Extension);
        }
        public IActionResult DownloadFileFromFileSystem(int id)
        {

            var file = _Context.FilesOnFileSystem.Where(x => x.FileId == id).FirstOrDefault();
            if (file == null) return null;
            var memory = new MemoryStream();
            using (var stream = new FileStream(file.FilePath, FileMode.Open))
            {
                stream.CopyTo(memory);
            }
            memory.Position = 0;
            return File(memory, file.FileType, file.Name + file.Extension);
        }

        public IActionResult DeleteFileFromDatabase(int id)
        {

            var file = _Context.FilesOnDatabase.Where(x => x.FileId == id).FirstOrDefault();
            _Context.FilesOnDatabase.Remove(file);
            _Context.SaveChanges();
            TempData["Message"] = $"Removed {file.Name + file.Extension} successfully from Database.";
            return RedirectToAction("Index");
        }
        public IActionResult DeleteFileFromFileSystem(int id)
        {

            var file = _Context.FilesOnFileSystem.Where(x => x.FileId == id).FirstOrDefault();
            if (file == null) return null;
            if (System.IO.File.Exists(file.FilePath))
            {
                System.IO.File.Delete(file.FilePath);
            }
            _Context.FilesOnFileSystem.Remove(file);
            _Context.SaveChanges();
            TempData["Message"] = $"Removed {file.Name + file.Extension} successfully from File System.";
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
