using LegendWorld.Data;
using Newtonsoft.Json;
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
        private const string itemsAddress = "API/Items/";
        private const string sessionAddress = "API/PlayerSessions/";
        private const string groundItemsAddress = "API/GroundItems/";
        private const string authAddress = "RCP/Auth/";
        private HttpClient httpClient;

        public WorldWebDataContext(string serverAdress)
        {
            if (httpClient == null)
                httpClient = new HttpClient();

            httpClient.BaseAddress = new Uri(serverAdress);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public Item GetItem(int itemId)
        {
            return this.Get<Item>(itemsAddress + itemId);
        }
        public IEnumerable<Item> GetItems(int mapId)
        {
            return this.Get<IEnumerable<Item>>(itemsAddress); // + "/all/" + mapId);
        }
        public IEnumerable<GroundItem> GetGroundItems(int mapId)
        {
            return this.Get<IEnumerable<GroundItem>>(groundItemsAddress);
        }

        private TObject Get<TObject>(string requestAdress)
        {
            TObject returnObject = default(TObject);
            HttpResponseMessage response = httpClient.GetAsync(requestAdress).Result;
            if (response.IsSuccessStatusCode)
            {
                string strJson = response.Content.ReadAsStringAsync().Result;
                //Deserialize to strongly typed class i.e., RootObject
                returnObject = JsonConvert.DeserializeObject<TObject>(strJson);
                //string content = response.Content.ReadAsStringAsync().Result;
                //returnObject = response.Content.ReadAsAsync<TObject>().Result;
                //if (!string.IsNullOrEmpty(content))
                //{

                //}
            }

            return returnObject;
        }

        public PlayerSession GetSession(int sessionId)
        {
            return this.Get<PlayerSession>(sessionAddress + sessionId);
        }

        public int CreateSession(int characterId)
        {
            return this.Get<int>(authAddress + "CreateSession/" + characterId);
        }

        public IEnumerable<SelectableCharacter> GetCharacters()
        {
            return this.Get<IEnumerable<SelectableCharacter>>(authAddress + "GetCharacterList/");
        }

        public void EndSession(int sessionId)
        {
            var result = this.Get<object>(authAddress + "EndSession/" + sessionId); ;
        }
    }
}
