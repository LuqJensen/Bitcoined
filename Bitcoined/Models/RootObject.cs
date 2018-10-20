using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bitcoined.Models
{
    public class RootObject
    {
        [JsonProperty("txid")]
        public string Txid { get; set; }

        [JsonProperty("vout")]
        public long Vout { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("redeemScript", NullValueHandling = NullValueHandling.Ignore)]
        public string RedeemScript { get; set; }

        [JsonProperty("scriptPubKey")]
        public string ScriptPubKey { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("confirmations")]
        public long Confirmations { get; set; }

        [JsonProperty("spendable")]
        public bool Spendable { get; set; }

        [JsonProperty("solvable")]
        public bool Solvable { get; set; }

        [JsonProperty("safe")]
        public bool Safe
        {
            get; set;
        }
    }
}
