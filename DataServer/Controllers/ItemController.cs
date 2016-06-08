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
using System.Runtime.Serialization;
using Data;

namespace DataServer.Controllers
{
    public class ItemController : ApiController
    {
        private WorldDbContext db = new WorldDbContext();

        [Route("API/Item/GetItems/{mapId}")]
        public IQueryable<ItemModel> GetItems(int mapId)//(int mapID)
        {
            return db.Items.Select(i => new ItemModel() { Id = i.Id, Identity = i.Identity, SubType = i.SubType, Count = i.Count, ContainerId = i.ContainerId, WorldMapId = i.WorldMapId, WorldX = i.WorldX, WorldY = i.WorldY }); //.Where(i => i.WorldMapID == mapID);
        }

        //public IQueryable<ItemData> GetContainerItems(int containerID)
        //{
        //    return db.Items.Where(i => i.ContainerID == containerID);
        //}

        [ResponseType(typeof(ItemModel))]
        public async Task<IHttpActionResult> GetItem(int id)
        {
            ItemModel item = await db.Items.Select(i => new ItemModel() { Id = i.Id, Identity = i.Identity, SubType = i.SubType, Count = i.Count, ContainerId = i.ContainerId, WorldMapId = i.WorldMapId, WorldX = i.WorldX, WorldY = i.WorldY }).FirstOrDefaultAsync(i => i.Id == id); //.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }   
        
        [HttpGet]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Use(int id)
        {
            //ItemModel item = await db.Items.Select(i => new ItemModel() { Id = i.Id, Identity = i.Identity, SubType = i.SubType, Count = i.Count, ContainerId = i.ContainerId, WorldMapId = i.WorldMapId, WorldX = i.WorldX, WorldY = i.WorldY }).FirstOrDefaultAsync(i => i.Id == id); //.FindAsync(id);
            Item item = await db.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            item.Count--;
            if (item.Count <= 0)
                db.Items.Remove(item);

            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PostContainerPosition(ItemContainerPositionModel positionModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Item itemToUpdate = await db.Items.FindAsync(positionModel.ItemId);
            if (itemToUpdate == null)
            {
                return BadRequest(ModelState);
            }

            itemToUpdate.ContainerId = positionModel.ContainerId;
            itemToUpdate.WorldMapId = itemToUpdate.WorldX = itemToUpdate.WorldY = null;

            //db.Items.Add(item);
            await db.SaveChangesAsync();

            //return CreatedAtRoute("DefaultApi", new { id = item.Id }, item);
            return StatusCode(HttpStatusCode.NoContent);
        }
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PostWorldPosition(ItemWorldPositionModel positionModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Item itemToUpdate = await db.Items.FindAsync(positionModel.ItemId);
            if (itemToUpdate == null)
            {
                return BadRequest(ModelState);
            }

            itemToUpdate.WorldMapId = positionModel.WorldMapId;
            itemToUpdate.WorldX = positionModel.WorldX;
            itemToUpdate.WorldY = positionModel.WorldY;
            itemToUpdate.ContainerId = null;

            //db.Items.Add(item);
            await db.SaveChangesAsync();

            //return CreatedAtRoute("DefaultApi", new { id = item.Id }, item);
            return StatusCode(HttpStatusCode.NoContent);
        }
        //// PUT: api/Items/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutItem(int id, ItemModel item)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != item.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(item).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ItemExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        // POST: api/Items
        [ResponseType(typeof(ItemModel))]
        public async Task<IHttpActionResult> PostItem(ItemModel itemModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Item newItem = db.Items.Create();
            newItem.Identity = itemModel.Identity;
            newItem.SubType = itemModel.SubType;
            newItem.WorldMapId = itemModel.WorldMapId;
            newItem.WorldX = itemModel.WorldX;
            newItem.WorldY = itemModel.WorldY;
            newItem.ContainerId = itemModel.ContainerId;

            newItem = db.Items.Add(newItem);
            await db.SaveChangesAsync();

            itemModel.Id = newItem.Id;

            return Ok(itemModel); //CreatedAtRoute("DefaultApi", new { id = itemModel.Id }, itemModel);
        }

        //// DELETE: api/Items/5
        //[ResponseType(typeof(ItemModel))]
        //public async Task<IHttpActionResult> DeleteItem(int id)
        //{
        //    ItemModel item = await db.Items.FindAsync(id);
        //    if (item == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Items.Remove(item);
        //    await db.SaveChangesAsync();

        //    return Ok(item);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ItemExists(int id)
        {
            return db.Items.Count(e => e.Id == id) > 0;
        }
    }
}