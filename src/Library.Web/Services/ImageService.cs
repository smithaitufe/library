using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Library.Web.Code;
using Library.Core.Models;
using Library.Repo;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Library.Web.Services
{
    public class ImageService
    {
        LibraryDbContext _context;
        IHostingEnvironment _environment;
        ImageUploadSettings _imageUploadSettings;

        public ImageService(LibraryDbContext context){
            _context = context;
        }
        public ImageService(ImageUploadSettings imageUploadSettings) {
            _imageUploadSettings = imageUploadSettings;
        }
        public ImageService(LibraryDbContext context, ImageUploadSettings imageUploadSettings)
        {
            _context = context;
            _imageUploadSettings = imageUploadSettings;
        }
        public ImageService(LibraryDbContext context, IHostingEnvironment environment, ImageUploadSettings imageUploadSettings)
        {
            _context = context;
            _environment = environment;
            _imageUploadSettings = imageUploadSettings;
        }
        public async Task<Image> SaveToDirectory(IFormFile file)
        {
            var image = new Image();
            var folder = _imageUploadSettings.BookLocation;            
            var path = Path.Combine(folder, file.FileName); 
            var uploads = Path.Combine(_environment.WebRootPath, folder.Substring(1));
            if(!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }

            if (file.Length > 0)
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);  
                    MemoryStream ms = new MemoryStream();
                    fileStream.CopyTo(ms);

                    string base64String = System.Convert.ToBase64String(ms.ToArray());
                    image = new Image { Path = path, Extension = Path.GetExtension(file.FileName), ContentType = file.ContentType, Base64 = base64String };
                    return image;
                }
            }
            return image;            
        }


    }
}