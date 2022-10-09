using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Steps.Models
{
    [Serializable]
    public class Account
    {
        public int Rank { get; set; }
        public string User { get; set; }
        public string Status { get; set; }
        public int Steps { get; set; }
        public double? Average { get; set; }
        public double? Maximum { get; set; }
        public double? Minimum { get; set; }
        public string? Underrate { get; set; }

    }
}
