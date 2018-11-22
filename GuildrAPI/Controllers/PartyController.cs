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
    [Route("api/Party")]
    public class PartyController : Controller
    {
        private readonly GuildrAPIContext _context;

        private IConfiguration _configuration;

        public PartyController(GuildrAPIContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Party
        [HttpGet]
        public IEnumerable<PartyItem> GetAllPartyItems()
        {
            return _context.PartyItem;
        }

        // GET: api/Party/PartyName
        [HttpGet("{PartyName}")]
        public IEnumerable<PartyItem> GetPartyItemWithName([FromRoute] string PartyName)
        {
            return _context.PartyItem.Where(m => m.PartyName == PartyName);
        }

        // PUT: api/Party/id (Update party #id)
        [HttpPut("{PartyID}")]
        public async Task<IActionResult> PutPartyItem([FromRoute] int PartyID, [FromBody] PartyItem partyItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (PartyID != partyItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(partyItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PartyItemExists(PartyID))
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

        // POST: api/Party
        [HttpPost]
        public async Task<IActionResult> PostPartyItem([FromBody] PartyItem partyItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.PartyItem.Add(partyItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPartyItem", new { id = partyItem.Id }, partyItem);
        }

        // DELETE: api/Party/id (Delete party #id)
        [HttpDelete("{PartyID}")]
        public async Task<IActionResult> DeletePartyItem([FromRoute] int PartyID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var partyItem = await _context.PartyItem.SingleOrDefaultAsync(m => m.Id == PartyID);
            if (partyItem == null)
            {
                return NotFound();
            }

            _context.PartyItem.Remove(partyItem);
            await _context.SaveChangesAsync();

            return Ok(partyItem);
        }

        private bool PartyItemExists(int id)
        {
            return _context.PartyItem.Any(e => e.Id == id);
        }

        //Upload party
        [HttpPost, Route("newParty")]
        public async Task<IActionResult> UploadParty([FromForm]PartyUploadItem party)
        {
            //Upload all variables
            PartyItem partyItem = new PartyItem();
            partyItem.PartyName = party.PartyName;
            partyItem.Organizer = party.Organizer;
            partyItem.Uploaded = DateTime.Now.ToString();

            _context.PartyItem.Add(partyItem);
            await _context.SaveChangesAsync();

            return Ok($"File: {partyItem.PartyName} has successfully uploaded");

        }
    }
}