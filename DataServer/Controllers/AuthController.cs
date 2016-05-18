using Data;
using DataServer.Models;
using LegendWorld.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace DataServer.Controllers
{
    public class AuthController : ApiController
    {
        private WorldDbContext db = new WorldDbContext();

        [HttpGet]
        public async Task<IHttpActionResult> CreateSession(int id) //Could be moved to PlayerSessionsPost, just ignore all incoming fields except CharacterId.
        {
            string clientAddress = HttpContext.Current.Request.UserHostAddress.Trim();
            PlayerSession playerSession = await db.PlayerSessions.Where(ps => ps.CharacterId == id).FirstOrDefaultAsync();
            if (playerSession == null)
            {
                playerSession = db.PlayerSessions.Create();
                playerSession.ClientAddress = clientAddress;
                playerSession.Created = DateTime.Now;
                db.PlayerSessions.Add(playerSession);
            }
            if (playerSession.ClientAddress != clientAddress)
                return BadRequest();
            
            playerSession.CharacterId = id;
            int result = await db.SaveChangesAsync();

            return Ok(playerSession.Id);
        }

        [HttpGet]
        //[WorldServerAuthentication]
        public async Task<IHttpActionResult> EndSession(int id) //WorldServer ONLY
        {
            string clientAddress = HttpContext.Current.Request.UserHostAddress.Trim();
            PlayerSession playerSession = await db.PlayerSessions.FindAsync(id);
            if (playerSession != null)
            {
                //if (playerSession.ClientAddress != clientAddress)
                //    return BadRequest();

                db.PlayerSessions.Remove(playerSession);
                int result = await db.SaveChangesAsync();
            }

            return Ok();
        }

        public async Task<IHttpActionResult> GetCharacterList()
        {
            Random rnd = new Random(); //ToDo, Create a character table and connect it to authentication.
            return Ok(new SelectableCharacter[] {
                new SelectableCharacter() { CharacterId = rnd.Next(ushort.MinValue, ushort.MaxValue), Name = "Temp Character#1" },
                new SelectableCharacter() { CharacterId = rnd.Next(ushort.MinValue, ushort.MaxValue), Name = "Temp Character#2" },
                new SelectableCharacter() { CharacterId = rnd.Next(ushort.MinValue, ushort.MaxValue), Name = "Temp Character#3" } }.ToList());
        }

        //private string GetClientIp(HttpRequestMessage request = null)
        //{
        //    request = request ?? Request;

        //    if (request.Properties.ContainsKey("MS_HttpContext"))
        //    {
        //        return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
        //    }
        //    else if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
        //    {
        //        RemoteEndpointMessageProperty prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
        //        return prop.Address;
        //    }
        //    else if (HttpContext.Current != null)
        //    {
        //        return HttpContext.Current.Request.UserHostAddress;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
    }
}
