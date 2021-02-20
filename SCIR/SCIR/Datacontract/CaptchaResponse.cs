using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCIR.Datacontract
{
    public class CaptchaReponse
    {
        [JsonProperty]
        public bool Success { get; set; }
        [JsonProperty]
        public List<string> ErroMessege { get; set; }

    }
}