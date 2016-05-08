using DataServer.Models;
using LegendWorld.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace DataServer.Controllers
{
    public class AuthController : ApiController
    {
        private WorldDbContext db = new WorldDbContext();

        public async Task<IHttpActionResult> CreateSession(int characterId) //Could be moved to PlayerSessionsPost, just ignore all incoming fields except CharacterId.
        {
            string clientAddress = HttpContext.Current.Request.UserHostAddress.Trim();
            PlayerSession playerSession = await db.PlayerSessions.Where(ps => ps.CharacterId == characterId).FirstOrDefaultAsync();
            if (playerSession == null)
            {
                playerSession = db.PlayerSessions.Create();
                playerSession.CharacterId = characterId;
                playerSession.ClientAddress = clientAddress;
                playerSession.Created = DateTime.Now;
                db.PlayerSessions.Add(playerSession);
                int result = await db.SaveChangesAsync();
            }
            if (playerSession.ClientAddress != clientAddress)
                return BadRequest();

            return Ok(playerSession.Id);
        }

        public async Task<IHttpActionResult> GetCharacterList()
        {
            Random rnd = new Random(); //ToDo, Create a character table and connect it to authentication.
            return Ok(new SelectableCharacter[] {
                new SelectableCharacter() { CharacterId = rnd.Next(ushort.MinValue, ushort.MaxValue), Name = "Temp Character#1" },
                new SelectableCharacter() { CharacterId = rnd.Next(ushort.MinValue, ushort.MaxValue), Name = "Temp Character#2" },
                new SelectableCharacter() { CharacterId = rnd.Next(ushort.MinValue, ushort.MaxValue), Name = "Temp Character#3" } }.ToList());
        }
    }
}
