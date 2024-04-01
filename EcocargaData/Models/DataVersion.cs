using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cl.Gob.Energia.Ecocarga.Data.Models
{
    [Table("data_version")]
    public partial class DataVersion
    {
        [Column("id")]
        [JsonIgnore]
        public int Id { get; set; }
        [Column("version")]
        [JsonProperty("version")]
        public Guid Version { get; set; }
        [Column("timestamp")]
        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
