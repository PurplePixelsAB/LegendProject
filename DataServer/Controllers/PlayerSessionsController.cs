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
using System.Web;
using LegendWorld.Data;

namespace DataServer.Controllers
{
    public class PlayerSessionsController : ApiController
    {
        private WorldDbContext db = new WorldDbContext();

        //public async Task<IHttpActionResult> CreateSession(int characterId)
        //{
        //    string clientAddress = HttpContext.Current.Request.UserHostAddress.Trim();
        //    PlayerSession playerSession = await db.PlayerSessions.Where(ps => ps.CharacterId == characterId).FirstOrDefaultAsync();
        //    if (playerSession == null)
        //    {
        //        playerSession = db.PlayerSessions.Create();
        //        playerSession.CharacterId = characterId;
        //        playerSession.ClientAddress = clientAddress;
        //        playerSession.Created = DateTime.Now;
        //        db.PlayerSessions.Add(playerSession);
        //        int result = await db.SaveChangesAsync();
        //    }
        //    if (playerSession.ClientAddress != clientAddress)
        //        return BadRequest();

        //    return Ok(playerSession.Id);
        //}

        // GET: api/PlayerSessions
        public IQueryable<PlayerSession> GetPlayerSessions()
        {
            return db.PlayerSessions;
        }

        // GET: api/PlayerSessions/5
        [ResponseType(typeof(PlayerSession))]
        public async Task<IHttpActionResult> GetPlayerSession(int id)
        {
            PlayerSession playerSession = await db.PlayerSessions.FindAsync(id);
            if (playerSession == null)
            {
                return NotFound();
            }

            return Ok(playerSession);
        }

        // PUT: api/PlayerSessions/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPlayerSession(int id, PlayerSession playerSession)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != playerSession.PlayerSessionID)
            {
                return BadRequest();
            }

            db.Entry(playerSession).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerSessionExists(id))
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

        // POST: api/PlayerSessions
        [ResponseType(typeof(PlayerSession))]
        public async Task<IHttpActionResult> PostPlayerSession(PlayerSession playerSession)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PlayerSessions.Add(playerSession);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = playerSession.PlayerSessionID }, playerSession);
        }

        // DELETE: api/PlayerSessions/5
        [ResponseType(typeof(PlayerSession))]
        public async Task<IHttpActionResult> DeletePlayerSession(int id)
        {
            PlayerSession playerSession = await db.PlayerSessions.FindAsync(id);
            if (playerSession == null)
            {
                return NotFound();
            }

            db.PlayerSessions.Remove(playerSession);
            await db.SaveChangesAsync();

            return Ok(playerSession);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PlayerSessionExists(int id)
        {
            return db.PlayerSessions.Count(e => e.PlayerSessionID == id) > 0;
        }
    }
}