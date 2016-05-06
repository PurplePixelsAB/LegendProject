using LegendWorld.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DataClient
{
    public class WorldWebDataContext
    {
        private const string itemsAddress = "api/items/";
        private const string groundItemsAddress = "api/grounditems/";
        private HttpClient httpClient;

        public WorldWebDataContext(string serverAdress)
        {
            if (httpClient == null)
                httpClient = new HttpClient();

            httpClient.BaseAddress = new Uri(serverAdress);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<Item> GetItem(ushort itemId)
        {
            return await this.Get<Item>(itemsAddress + itemId);
        }
        public async Task<IEnumerable<Item>> GetItems(ushort mapId)
        {
            return await this.Get<IEnumerable<Item>>(itemsAddress); // + "/all/" + mapId);
        }
        public async Task<IEnumerable<GroundItem>> GetGroundItems(ushort mapId)
        {
            return await this.Get<IEnumerable<GroundItem>>(groundItemsAddress);
        }

        private async Task<TObject> Get<TObject>(string requestAdress)
        {
            TObject returnObject = default(TObject);
            HttpResponseMessage response = await httpClient.GetAsync(requestAdress);
            if (response.IsSuccessStatusCode)
            {
                returnObject = await response.Content.ReadAsAsync<TObject>();
            }

            return returnObject;
        }
    }
}
