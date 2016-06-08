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
using System.Data.Entity.Migrations;

namespace DataServer.Controllers
{
    public class CharacterController : ApiController
    {
        private WorldDbContext db = new WorldDbContext();

        [ResponseType(typeof(IEnumerable<CharacterModel>))]
        public async Task<IHttpActionResult> GetSessionCharacters(int id)
        {
            PlayerSession session = await db.PlayerSessions.FindAsync(id);
            if (session == null)
            {
                return NotFound();
            }

            IEnumerable<CharacterModel> charactersOnMap = db.Characters.Where(character => character.MapId == db.Characters.First(sessionChar => sessionChar.Id == session.Id).MapId).Select(c =>
                new CharacterModel() { Id = c.Id, Name = c.Name, MapId = c.MapId, WorldX = c.WorldX, WorldY = c.WorldY, InventoryId = c.InventoryId, Energy = (byte)c.Energy, Health = (byte)c.Health, ArmorId = c.ArmorId, LeftHandId = c.LeftHandId, RightHandId = c.RightHandId }); //.Include(c => c.Powers);

            return Ok(charactersOnMap);
        }

        [ResponseType(typeof(IEnumerable<int>))]
        public async Task<IHttpActionResult> GetCharacterPowers(int id)
        {
            IEnumerable<int> characterPowers = await db.CharacterPowers.Where(cp => cp.CharacterId == id).Select(cp => cp.Power).ToListAsync();

            return Ok(characterPowers);
        }

        // GET: api/Character
        public IEnumerable<CharacterModel> GetCharactersOnMap(int mapId)
        {
            return db.Characters.Where(chr => db.PlayerSessions.Any(ps => ps.CharacterId == chr.Id) && chr.MapId == mapId).Select(c =>
                new CharacterModel() { Id = c.Id, Name = c.Name, MapId = c.MapId, WorldX = c.WorldX, WorldY = c.WorldY, InventoryId = c.InventoryId, Energy = (byte)c.Energy, Health = (byte)c.Health, ArmorId = c.ArmorId, LeftHandId = c.LeftHandId, RightHandId = c.RightHandId }); //.Include(c => c.Powers);
        }

        // GET: api/Character/5
        [ResponseType(typeof(CharacterModel))]
        public async Task<IHttpActionResult> GetCharacterData(int id)
        {
            CharacterModel characterData = await db.Characters.Select(c =>
                new CharacterModel() { Id = c.Id, Name = c.Name, MapId = c.MapId, WorldX = c.WorldX, WorldY = c.WorldY, InventoryId = c.InventoryId, Energy = (byte)c.Energy, Health = (byte)c.Health, ArmorId = c.ArmorId, LeftHandId = c.LeftHandId, RightHandId = c.RightHandId }).FirstOrDefaultAsync(c => c.Id == id); //.Include(c => c.Powers)
            if (characterData == null)
            {
                return NotFound();
            }
            
            return Ok(characterData);
        }


        [ResponseType(typeof(bool))]
        public async Task<IHttpActionResult> LearnPower(int characterId, int power)
        {
            //CharacterModel characterData = await db.Characters.Select(c =>
            //    new CharacterModel() { Id = c.Id, Name = c.Name, MapId = c.MapId, WorldX = c.WorldX, WorldY = c.WorldY, InventoryId = c.InventoryId, Energy = (byte)c.Energy, Health = (byte)c.Health, ArmorId = c.ArmorId, LeftHandId = c.LeftHandId, RightHandId = c.RightHandId }).FirstOrDefaultAsync(c => c.Id == id); //.Include(c => c.Powers)
            Character character = await db.Characters.FindAsync(characterId);
            if (character == null)
            {
                return NotFound();
            }

            int powerCount = await db.CharacterPowers.CountAsync(c => c.CharacterId == characterId);
            if (powerCount >= 7)
            {
                return BadRequest();
            }

            CharacterPower newPower = db.CharacterPowers.Create();
            newPower.Power = power;
            newPower.CharacterId = characterId;
            db.CharacterPowers.Add(newPower);

            return Ok(true);
        }

        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PostItems(CharacterItemsModel characterItemsModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Character characterToUpdate = await db.Characters.FindAsync(characterItemsModel.CharacterId);
            if (characterToUpdate == null)
            {
                return BadRequest(ModelState);
            }

            characterToUpdate.ArmorId = characterItemsModel.ArmorId;
            characterToUpdate.LeftHandId = characterItemsModel.LeftHandId;
            characterToUpdate.RightHandId = characterItemsModel.RightHandId;

            //db.Characters.Add(characterItemsModel);
            await db.SaveChangesAsync();

            //return CreatedAtRoute("DefaultApi", new { id = characterItemsModel.Id }, characterItemsModel);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PostPosition(CharacterPositionModel characterPositionModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Character characterToUpdate = await db.Characters.FindAsync(characterPositionModel.CharacterId);
            if (characterToUpdate == null)
            {
                return BadRequest(ModelState);
            }
            
            characterToUpdate.MapId = characterPositionModel.MapId;
            characterToUpdate.WorldX = characterPositionModel.WorldX;
            characterToUpdate.WorldY = characterPositionModel.WorldY;
            
            await db.SaveChangesAsync();
            
            return StatusCode(HttpStatusCode.NoContent);
        }

        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PostStats(CharacterStatsModel characterStatsModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Character characterToUpdate = await db.Characters.FindAsync(characterStatsModel.CharacterId);
            if (characterToUpdate == null)
            {
                return BadRequest(ModelState);
            }

            characterToUpdate.Health = characterStatsModel.Health;
            characterToUpdate.Energy = characterStatsModel.Energy;
            
            await db.SaveChangesAsync();
            
            return StatusCode(HttpStatusCode.NoContent);
        }


        //// PUT: api/Character/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutCharacterData(int id, CharacterData characterData)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != characterData.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(characterData).State = EntityState.Modified;

        //    //List<int> previousIds = db.Characters.AsNoTracking().FirstOrDefault(chr => chr.Id == id).Powers.Select(pwr => pwr.Id).ToList();
        //    //List<int> currentIds = characterData.Powers.Select(pwr => pwr.Id).ToList();
        //    //List<int> deletedIds = previousIds.Except(currentIds).ToList();

        //    foreach (var deletedId in deletedIds)
        //    {
        //        CharacterPowerLearned characterPowerLearned = characterData.Powers.Single(od => od.Id == deletedId);
        //        db.Entry(characterPowerLearned).State = EntityState.Deleted;
        //    }

        //    foreach (CharacterPowerLearned power in characterData.Powers)
        //    {
        //        if (power.Id == 0)
        //            db.Entry(power).State = EntityState.Added;
        //        else
        //            db.Entry(power).State = EntityState.Modified;
        //    }

        //    //db.Set<CharacterData>().AddOrUpdate(characterData);

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CharacterDataExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        //// POST: api/Character
        //[ResponseType(typeof(CharacterData))]
        //public async Task<IHttpActionResult> PostCharacterData(CharacterData characterData)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Characters.Add(characterData);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = characterData.Id }, characterData);
        //}

        ////DELETE: api/Character/5
        //[ResponseType(typeof(CharacterData))]
        //public async Task<IHttpActionResult> DeleteCharacterData(int id)
        //{
        //    CharacterData characterData = await db.Characters.FindAsync(id);
        //    if (characterData == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Characters.Remove(characterData);
        //    await db.SaveChangesAsync();

        //    return Ok(characterData);
        //}

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
            return db.Characters.Count(e => e.Id == id) > 0;
        }
    }
}