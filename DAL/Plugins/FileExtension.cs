using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Plugins
{
    public static class FileExtension
    {
        public static string Upload(IFormFile file, string rootPath, string folder, string fileName = "")
        {
            var uploadDirectory = "Upload\\" + folder;
            var uploadPath = Path.Combine(rootPath, uploadDirectory);
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);
            string fileNameOnly = fileName.Trim() == "" ? Guid.NewGuid().ToString() : fileName;
            if (fileNameOnly.IndexOf(".jpg") == -1)
                fileNameOnly += ".jpg";
            fileName = fileNameOnly + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadPath, fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            return "/" + uploadDirectory.Replace("\\", "/") + "/" + fileName;

        }
    }
}
