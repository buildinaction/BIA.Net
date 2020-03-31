using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZZCompanyNameZZ.ZZProjectNameZZ.Business.DTO;

namespace ZZCompanyNameZZ.ZZProjectNameZZ.WebApi.DTO
{
    public class UserInfoDTO
    {
        [JsonProperty(PropertyName = "language")]
        public string Language { get; set; }

        [JsonProperty(PropertyName = "login")]
        public string Login { get; set; }

        [JsonProperty(PropertyName = "roles")]
        public IList<string> Roles { get; set; }

        [JsonProperty(PropertyName = "user")]
        public UserDTO User { get; set; }

        [JsonProperty(PropertyName = "userProfile")]
        public Dictionary<string, string> UserProfile { get; set; }
    }
}