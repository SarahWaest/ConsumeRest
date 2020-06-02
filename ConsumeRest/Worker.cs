using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ModelLib.Model;
using Newtonsoft.Json;

namespace ConsumeRest
{
    public class Worker
    {
        //private string URI = "https://itemsrvice.azurewebsites.net/api/items";
        private  string URI = "https://restapidemo.azurewebsites.net/api/localItems/";
        public async void Start()
        {
            Console.WriteLine("Start method call");

            //GetAllItems
            //foreach (var item in await  GetAllItemsAsync(URI))
            //{
            //    Console.WriteLine(item.Name + ":" + item.Quality + ":" + item.Quantity);
            //}

            Console.WriteLine("Trying another method");
            Item i = await GetOneItemsAsync(2);
            System.Console.WriteLine(i.Name);

            DeleteItem(2);

            //Console.WriteLine(GetAllItemsAsync(URI).Result);
        }

        public async Task<IList<Item>> GetAllItemsAsync(string URI)
        {
            using (HttpClient client = new HttpClient())
            {
                string content = await client.GetStringAsync(URI);
                IList<Item> cList = JsonConvert.DeserializeObject<IList<Item>>(content);
                return cList;
            }
        }

        public async Task<Item> GetOneItemsAsync(int id)
        {
                IList<Item> items = await GetAllItemsAsync(URI);
                return items.First(item => item.ID == id);
        }

        public async Task PostItemAsync()
        {
            Console.WriteLine("Post Item");

            IList<Item> items = await GetAllItemsAsync(this.URI);
            List<Item> i = items.ToList();
            int nesId = 0;

            i.ForEach(item => {
                if (item.ID > nesId)
                {
                    nesId = item.ID;
                }
            });

            nesId++;
            Item newItem = new Item(11, "Pasta", "Low", 22);

            using (HttpClient client = new HttpClient())
            {
                string newItemJson = JsonConvert.SerializeObject(newItem);
                var content = new StringContent(newItemJson, Encoding.UTF8, "application/json");
                client.BaseAddress = new Uri(this.URI);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.PostAsync("/api/localitems/", content);

                Console.WriteLine("An Item posted to service");
                Console.WriteLine("Response is " + response);
                response.EnsureSuccessStatusCode();
                var httpResponseBody = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(httpResponseBody);
            }

            Console.WriteLine("------Verification------");
            items = await GetAllItemsAsync(URI);

            Item itemtest = items.First(item => item.ID == newItem.ID);

            if (itemtest != null)
            {
                Console.WriteLine("Item has been added");
            }
            else
            {
                Console.WriteLine("Failed to add item");
            }
        }

        public async Task PutItem(Item item)
        {
            using (HttpClient client = new HttpClient())
            {
                string JsonString = JsonConvert.SerializeObject(item);
                var content = new StringContent(JsonString, Encoding.UTF8, "application/json");
                client.BaseAddress = new Uri(URI);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.PutAsync("/api/localitems/", content);

                Console.WriteLine("An Item posted to service");
                Console.WriteLine("Response is " + response);
                response.EnsureSuccessStatusCode();
                var httpResponseBody = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(httpResponseBody);
            }
        }

        public async void DeleteItem(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(URI);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.DeleteAsync("/api/localitems/" + id);

                Console.WriteLine("Item deleted from service");
                Console.WriteLine("Response is: " + response);

                response.EnsureSuccessStatusCode();

                var httpResponseBody = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(httpResponseBody);
            }
        }
    }
}