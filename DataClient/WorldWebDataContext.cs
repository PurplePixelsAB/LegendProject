using Data;
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
        private const string itemsAddress = "API/Item/"; 
        protected const string characterAddress = "API/Character/";
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

        public ItemData GetItem(int itemId)
        {
            return this.Get<ItemData>(itemsAddress + itemId);
        }
        public IEnumerable<ItemData> GetItems(int mapId)
        {
            return this.Get<IEnumerable<ItemData>>(itemsAddress); // + "/all/" + mapId);
        }
        public IEnumerable<ItemData> GetGroundItems(int mapId)
        {
            return this.Get<IEnumerable<ItemData>>(groundItemsAddress);
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
        protected bool Put<TObject>(string requestAdress, TObject objectToPut)
        {
            string strJson = JsonConvert.SerializeObject(objectToPut);
            StringContent content = new System.Net.Http.StringContent(strJson, Encoding.UTF8, "application/json");
            HttpResponseMessage response = httpClient.PutAsync(requestAdress, content).Result;
            return response.IsSuccessStatusCode;
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

        //public Character

        public void EndSession(int sessionId)
        {
            var result = this.Get<object>(authAddress + "EndSession/" + sessionId); ;
        }

        public CharacterData GetCharacter(int characterId)
        {
            return this.Get<CharacterData>(characterAddress + characterId);
        }
    }
}
