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
using DataServer.Models;
using LegendWorld.Data;
using Data;

namespace DataServer.Controllers
{
    public class GroundItemsController : ApiController
    {
        private WorldDbContext db = new WorldDbContext();

        // GET: api/GroundItems
        public IQueryable<ItemData> GetGroundItems()
        {
            return db.GroundItems;
        }

        // GET: api/GroundItems/5
        [ResponseType(typeof(GroundItem))]
        public async Task<IHttpActionResult> GetGroundItem(int id)
        {
            GroundItem groundItem = await db.GroundItems.FindAsync(id);
            if (groundItem == null)
            {
                return NotFound();
            }

            return Ok(groundItem);
        }

        // PUT: api/GroundItems/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutGroundItem(int id, GroundItem groundItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != groundItem.Id)
            {
                return BadRequest();
            }

            db.Entry(groundItem).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroundItemExists(id))
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

        // POST: api/GroundItems
        [ResponseType(typeof(GroundItem))]
        public async Task<IHttpActionResult> PostGroundItem(GroundItem groundItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.GroundItems.Add(groundItem);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = groundItem.Id }, groundItem);
        }

        // DELETE: api/GroundItems/5
        [ResponseType(typeof(GroundItem))]
        public async Task<IHttpActionResult> DeleteGroundItem(int id)
        {
            GroundItem groundItem = await db.GroundItems.FindAsync(id);
            if (groundItem == null)
            {
                return NotFound();
            }

            db.GroundItems.Remove(groundItem);
            await db.SaveChangesAsync();

            return Ok(groundItem);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GroundItemExists(int id)
        {
            return db.GroundItems.Count(e => e.Id == id) > 0;
        }
    }
}