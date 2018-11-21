using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GuildrAPI.Models;
using GuildrAPI.Helpers;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.Extensions.Configuration;

namespace GuildrAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Profile")]
    public class ProfileController : Controller
    {
        private readonly GuildrAPIContext _context;

        private IConfiguration _configuration;

        public ProfileController(GuildrAPIContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Profile
        [HttpGet]
        public IEnumerable<ProfileItem> GetProfileItem()
        {
            return _context.ProfileItem;
        }

        // GET: api/Profile/class
        [HttpGet("{Class}")]
        public async Task<IActionResult> GetProfileItem([FromRoute] string Class)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var profileItem = await _context.ProfileItem.SingleOrDefaultAsync(m => m.Class == Class);

            if (profileItem == null)
            {
                return NotFound();
            }

            return Ok(profileItem);
        }

        // PUT: api/Profile/id (Update profile #id)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProfileItem([FromRoute] int id, [FromBody] ProfileItem profileItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != profileItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(profileItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfileItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Profile
        [HttpPost]
        public async Task<IActionResult> PostProfileItem([FromBody] ProfileItem profileItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ProfileItem.Add(profileItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProfileItem", new { id = profileItem.Id }, profileItem);
        }

        // DELETE: api/Profile/id (Delete profile #id)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfileItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var profileItem = await _context.ProfileItem.SingleOrDefaultAsync(m => m.Id == id);
            if (profileItem == null)
            {
                return NotFound();
            }

            _context.ProfileItem.Remove(profileItem);
            await _context.SaveChangesAsync();

            return Ok(profileItem);
        }

        private bool ProfileItemExists(int id)
        {
            return _context.ProfileItem.Any(e => e.Id == id);
        }

        // GET: api/Profile/Class (Get all classes)
        [Route("class")]
        [HttpGet]
        public async Task<List<string>> GetClass()
        {
            var profiles = (from m in _context.ProfileItem
                         select m.Class).Distinct();

            var returned = await profiles.ToListAsync();

            return returned;
        }

        //Upload profile image to blob storage
        [HttpPost, Route("upload")]
        public async Task<IActionResult> UploadFile([FromForm]ProfileImageItem profile)
        {
            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                return BadRequest($"Expected a multipart request, but got {Request.ContentType}");
            }
            try
            {
                using (var stream = profile.Image.OpenReadStream())
                {
                    var cloudBlock = await UploadToBlob(profile.Image.FileName, null, stream);

                    if (string.IsNullOrEmpty(cloudBlock.StorageUri.ToString()))
                    {
                        return BadRequest("An error has occured while uploading your file. Please try again.");
                    }

                    //Upload all variables
                    ProfileItem profileItem = new ProfileItem();
                    profileItem.Name = profile.Name;
                    profileItem.Class = profile.Class;
                    profileItem.Level = profile.Level;

                    //Upload url of image
                    System.Drawing.Image image = System.Drawing.Image.FromStream(stream);
                    profileItem.Url = cloudBlock.SnapshotQualifiedUri.AbsoluteUri;
                    profileItem.Uploaded = DateTime.Now.ToString();

                    _context.ProfileItem.Add(profileItem);
                    await _context.SaveChangesAsync();

                    return Ok($"File: {profile.Name} has successfully uploaded");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"An error has occured. Details: {ex.Message}");
            }


        }

        //Send image file to blob storage on Azure
        private async Task<CloudBlockBlob> UploadToBlob(string filename, byte[] imageBuffer = null, System.IO.Stream stream = null)
        {

            var accountName = _configuration["AzureBlob:name"];
            var accountKey = _configuration["AzureBlob:key"]; ;
            var storageAccount = new CloudStorageAccount(new StorageCredentials(accountName, accountKey), true);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer imagesContainer = blobClient.GetContainerReference("profileimages");

            string storageConnectionString = _configuration["AzureBlob:connectionString"];

            //Check whether the connection string can be parsed
            if (CloudStorageAccount.TryParse(storageConnectionString, out storageAccount))
            {
                try
                {
                    //Generate a new filename for every new blob
                    var fileName = Guid.NewGuid().ToString();
                    fileName += GetFileExtention(filename);

                    //Get a reference to the blob address, then upload the file to the blob
                    CloudBlockBlob cloudBlockBlob = imagesContainer.GetBlockBlobReference(fileName);

                    if (stream != null)
                    {
                        await cloudBlockBlob.UploadFromStreamAsync(stream);
                    }
                    else
                    {
                        return new CloudBlockBlob(new Uri(""));
                    }

                    return cloudBlockBlob;
                }
                catch (StorageException ex)
                {
                    return new CloudBlockBlob(new Uri(""));
                }
            }
            else
            {
                return new CloudBlockBlob(new Uri(""));
            }

        }

        private string GetFileExtention(string fileName)
        {
            //No extension
            if (!fileName.Contains("."))
                return ""; 
            else
            {
                //Assumes last item is the extension 
                var extentionList = fileName.Split('.');
                return "." + extentionList.Last();             }
        }
    }
}