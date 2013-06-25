using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Model.Models;

namespace Core.Services
{
    public class ImageService: IImageService
    {
        private readonly ConferenceContext _context;
        public ImageService(ConferenceContext context)
        {
            _context = context;
        }
        public const int MaximumFileSize = 20 * 1024 * 1024;
        public string[] ImageExtensions = new[] {"png","tga","gif","jpg","jpeg","bmp"};

        public byte[] GetData(int id)
        {
            var image = _context.Images
                .Include("FileData")
                .FirstOrDefault(i => i.Id == id);

            if (image == null)
                return null;

            return image.FileData.Data;
        }

        public Image AddImage(string filename, byte[] imageContent, int? uploadedByUserId)
        {
            string ext = Path.GetExtension(filename).TrimStart('.').Trim();

            if (!ImageExtensions.Any(e => string.Equals(e, ext, StringComparison.InvariantCultureIgnoreCase)))
                return null;

            // If the image is larger than 20MB, reject it.
            if (imageContent.Length > MaximumFileSize)
                return null;

            var image = Add(imageContent, filename, uploadedByUserId);

            return image;
        }

        Image Add(byte[] fileContent, string filename, int? uploadedByUserId)
        {
            if (filename == null)
                filename = Guid.NewGuid().ToString();

            var image = new Image
            {
                DateCreated = DateTime.Now,
                FileData = new FileData
                {
                    Data = fileContent
                },
                FileName = filename,
                FileSize = fileContent.Length,
                UserId = uploadedByUserId
            };

            _context.Images.Add(image);
            _context.SaveChanges();

            return image;
        }
    }
}
