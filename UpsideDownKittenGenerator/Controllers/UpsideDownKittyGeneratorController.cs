using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpsideDownKittenGenerator.Models;

using System.IO;
using System.Net;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Formats;
using Microsoft.AspNetCore.Authorization;
using UpsideDownKittenGenerator.Authentication;
using Microsoft.Extensions.Configuration;

namespace UpsideDownKittenGenerator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpsideDownKittyGeneratorController : ControllerBase
    {
        private readonly UpsideDownKittyGenertorContext _context;
        private readonly IConfiguration _configuration;

        public UpsideDownKittyGeneratorController(UpsideDownKittyGenertorContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }

        [Authorize]
        [HttpGet]
        [Route("RotatedImage")]
        public IActionResult RotatedImage()

        {
            WebRequest request = HttpWebRequest.Create(_configuration["CatWebsiteConnectionString:RegisteredUserURL"]);
            var response = (HttpWebResponse)request.GetResponse();
            byte[] imageBytes;
          
            Stream image = response.GetResponseStream();
            using (BinaryReader br = new BinaryReader(image))
            {
                imageBytes = br.ReadBytes(Convert.ToInt32(response.ContentLength));
                br.Close();
            }

            var rotatedImage = RotateImage(imageBytes);
            return File(rotatedImage, "image/jpeg");
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet]
        [Route("RotatedImageAdmin")]
        public IActionResult RotatedImageAdmin()
        {
            WebRequest request = HttpWebRequest.Create(_configuration["CatWebsiteConnectionString:AdminUserURL"]);
            var response = (HttpWebResponse)request.GetResponse();
            byte[] imageBytes;
            Stream image = response.GetResponseStream();
            using (BinaryReader br = new BinaryReader(image))
            {
                imageBytes = br.ReadBytes(Convert.ToInt32(response.ContentLength));
                br.Close();
            }
            var rotatedImage = RotateImage(imageBytes);
            return File(rotatedImage, "image/jpeg");
        }

      
        [HttpGet]
        [Route("RotatedImageNoAuth")]
        public IActionResult RotatedImageNoAuth()
        {
            WebRequest request = HttpWebRequest.Create(_configuration["CatWebsiteConnectionString:NonRegisteredUserURL"]);
            var response = (HttpWebResponse)request.GetResponse();
            byte[] imageBytes;
            Stream image = response.GetResponseStream();
            using (BinaryReader br = new BinaryReader(image))
            {
                imageBytes = br.ReadBytes(Convert.ToInt32(response.ContentLength));
                br.Close();
            }
            var rotatedImage = RotateImage(imageBytes);
            return File(rotatedImage, "image/jpeg");
        }

        private byte[] RotateImage(byte[] imageInBytes)
        {
            using (var image = Image.Load(imageInBytes, out var imageFormat))
            {
                image.Mutate(x => x.Rotate(180));
                return ImageToByteArray((Image<Rgba32>)image, imageFormat);
            }
        }

        private byte[] ImageToByteArray(Image<Rgba32> image, IImageFormat imageFormat)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, imageFormat);
                return ms.ToArray();
            }
        }


    }
}
    




