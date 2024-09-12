using System.Text.Json.Serialization;
using MyBooks.Services.Data.Enums;
using Newtonsoft.Json;

namespace MyBooks.Models.Overview
{
    public class SearchVM
    {
        public string Query { get; set; }
    }


}