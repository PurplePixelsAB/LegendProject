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
        protected const string itemsAddress = "API/Item/"; 
        protected const string characterAddress = "API/Character/";
        protected const string sessionAddress = "API/PlayerSessions/";
        protected const string authAddress = "RCP/Auth/";
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
        //public IEnumerable<ItemData> GetGroundItems(int mapId)
        //{
        //    return this.Get<IEnumerable<ItemData>>(groundItemsAddress);
        //}

        protected TObject Get<TObject>(string requestAdress)
        {
            TObject returnObject = default(TObject);
            HttpResponseMessage response = httpClient.GetAsync(requestAdress).Result;
            if (response.IsSuccessStatusCode)
            {
                string strJson = response.Content.ReadAsStringAsync().Result;
                returnObject = JsonConvert.DeserializeObject<TObject>(strJson);
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
        protected TObject Post<TObject>(string requestAdress, TObject objectToPut)
        {
            string strJson = JsonConvert.SerializeObject(objectToPut);
            StringContent content = new System.Net.Http.StringContent(strJson, Encoding.UTF8, "application/json");
            HttpResponseMessage response = httpClient.PostAsync(requestAdress, content).Result;
            if (response.IsSuccessStatusCode)
            {
                string returnJson = response.Content.ReadAsStringAsync().Result;
                objectToPut = JsonConvert.DeserializeObject<TObject>(strJson);
            }

            return objectToPut;
        }
        public PlayerSession GetSession(int sessionId)
        {
            return this.Get<PlayerSession>(sessionAddress + sessionId);
        }

        public int CreateSession(int characterId)
        {
            return this.Get<int>(authAddress + "CreateSession/" + characterId);
        }

        public IEnumerable<SelectableCharacter> GetSelectableCharacter()
        {
            return this.Get<IEnumerable<SelectableCharacter>>(authAddress + "GetCharacterList/");
        }

        public CharacterData GetCharacter(int characterId)
        {
            return this.Get<CharacterData>(characterAddress + characterId);
        }
        public IEnumerable<CharacterData> GetCharacters(int mapId)
        {
            return this.Get<IEnumerable<CharacterData>>(characterAddress); // + mapId);
        }
    }
}
