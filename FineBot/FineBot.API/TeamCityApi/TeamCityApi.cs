using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FineBot.API.TeamCityApi.TeamCityModels;
using Newtonsoft.Json;

namespace FineBot.API.TeamCityApi
{
    public class TeamCityApi : ITeamCityApi
    {
        public DeepBuildModel GetBuildById(string buildId)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(ConfigurationManager.AppSettings["TeamCityServer"]),
            };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1")
                .GetBytes(
                ConfigurationManager.AppSettings["UserName"] + ":" + ConfigurationManager.AppSettings["Password"]));
            client.DefaultRequestHeaders.Add("Authorization", "Basic " + encoded);

            var response = client.GetAsync("app/rest/builds/id:" + buildId).Result;
            var responseAsyncString = response.Content.ReadAsStringAsync();
            var responseResult = responseAsyncString.Result;

            return JsonConvert.DeserializeObject<DeepBuildModel>(responseResult);
        }

        public List<ShallowBuildModel> GetAllBuildsThatFailed()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["TeamCityServer"]);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1")
                    .GetBytes(
                    ConfigurationManager.AppSettings["UserName"] + ":" + ConfigurationManager.AppSettings["Password"]));
                client.DefaultRequestHeaders.Add("Authorization", "Basic " + encoded);

                var response =  client.GetAsync("app/rest/builds").Result;
                var responseAsyncString = response.Content.ReadAsStringAsync();
                var responseResult = responseAsyncString.Result;

                return JsonConvert.DeserializeObject<BuildListModel>(responseResult).build.Where(x=>x.status == "FAILURE").ToList();

            }
        }
    }
}
