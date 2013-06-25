using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models;

namespace Core.Services
{
    public interface IImageService
    {
        byte[] GetData(int id);
        Image AddImage(string filename, byte[] imageContent, int? uploadedByUserId);
    }
}
