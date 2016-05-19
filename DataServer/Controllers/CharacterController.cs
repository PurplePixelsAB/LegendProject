using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Data;
using DataServer.Models;

namespace DataServer.Controllers
{
    public class CharacterController : ApiController
    {
        private WorldDbContext db = new WorldDbContext();

        // GET: api/Character
        public IQueryable<CharacterData> GetCharacters()
        {
            return db.Characters;
        }

        // GET: api/Character/5
        [ResponseType(typeof(CharacterData))]
        public async Task<IHttpActionResult> GetCharacterData(int id)
        {
            CharacterData characterData = await db.Characters.FindAsync(id);
            if (characterData == null)
            {
                return NotFound();
            }

            return Ok(characterData);
        }

        // PUT: api/Character/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCharacterData(int id, CharacterData characterData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != characterData.CharacterDataID)
            {
                return BadRequest();
            }

            db.Entry(characterData).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CharacterDataExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Character
        [ResponseType(typeof(CharacterData))]
        public async Task<IHttpActionResult> PostCharacterData(CharacterData characterData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Characters.Add(characterData);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = characterData.CharacterDataID }, characterData);
        }

        // DELETE: api/Character/5
        [ResponseType(typeof(CharacterData))]
        public async Task<IHttpActionResult> DeleteCharacterData(int id)
        {
            CharacterData characterData = await db.Characters.FindAsync(id);
            if (characterData == null)
            {
                return NotFound();
            }

            db.Characters.Remove(characterData);
            await db.SaveChangesAsync();

            return Ok(characterData);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CharacterDataExists(int id)
        {
            return db.Characters.Count(e => e.CharacterDataID == id) > 0;
        }
    }
}