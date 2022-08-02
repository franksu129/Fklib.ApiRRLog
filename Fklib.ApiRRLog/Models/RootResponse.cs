using System;

namespace Fklib.ApiRRLog.Models
{
    public class RootResponse
    {
        public DateTime OnTime { get; set; }
        public int Status { get; set; }
        public string Content { get; set; }
    }
}