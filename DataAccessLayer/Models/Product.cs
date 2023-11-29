using DataAccessLayer.Reposatries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Product :ISoftDeletable
    {
        public int Id { get; set; }

        [MaxLength(20)]
        public string? Name { get; set; }
        public bool Activated { get; set; }

        [JsonIgnore]
        public bool IsDeleted { get; set; }

    }
}
