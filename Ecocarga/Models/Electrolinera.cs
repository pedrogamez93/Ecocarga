using Newtonsoft.Json;
using System.Security.Policy;

namespace Ecocarga.Models
{
    public class Coordinates
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }

    public class Connector
    {
        public int connector_id { get; set; }
        public int order_number { get; set; }
        public string status { get; set; }
        public string standard { get; set; }
        public string format { get; set; }
        public string power_type { get; set; }
        public decimal max_voltage { get; set; }
        public decimal max_amperage { get; set; }
        public decimal max_electric_power { get; set; }
        public decimal voltage { get; set; }
        public decimal amperage { get; set; }
        public decimal electric_power { get; set; }
        public DateTime last_updated { get; set; }
        public decimal soc { get; set; }
        public List<object> tariffs { get; set; }
    }



    public class Evse
    {
        public int EvseUid { get; set; }
        public string EvseId { get; set; }
        public string Directions { get; set; }
        public int OrderNumber { get; set; }
        public List<Connector> Connectors { get; set; }
        public DateTime LastUpdated { get; set; }
        public string Status { get; set; }
        public Coordinates Coordinates { get; set; }
        public bool PermiteCargaSimultanea { get; set; }
        public int MaxElectricPower { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
    }

    public class Owner
    {
        public int user_id { get; set; }
        public string name { get; set; }
        public string website { get; set; }
        public string RUT { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string commune { get; set; }
        public string region { get; set; }
        public string email { get; set; }
    }
    public class Item
    {
        public int LocationId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Commune { get; set; }
        public string Region { get; set; }
        public Coordinates Coordinates { get; set; }
        public List<Evse> Evses { get; set; }

        public Owner Owner { get; set; }


    }

    public class ElectrolineraResponse
    {
        [JsonProperty("total_items")]
        public int TotalItems { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }

        [JsonProperty("current_page")]
        public int CurrentPage { get; set; }

        [JsonProperty("items_per_page")]
        public int ItemsPerPage { get; set; }

        [JsonProperty("items")]
        public List<Item> Items { get; set; }
    }

}
