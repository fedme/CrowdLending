using AspNet.Security.OpenIdConnect.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrowdLending.Models
{
    public class UserInfoResponse
    {
        [JsonProperty(PropertyName = OpenIdConnectConstants.Claims.Subject)]
        public string Subject { get; set; }

        [JsonProperty(PropertyName = OpenIdConnectConstants.Claims.GivenName)]
        public string GivenName { get; set; }

        [JsonProperty(PropertyName = OpenIdConnectConstants.Claims.FamilyName)]
        public string FamilyName { get; set; }

        [JsonProperty(PropertyName = OpenIdConnectConstants.Claims.Email)]
        public string Email { get; set; }

        [JsonProperty(PropertyName = OpenIdConnectConstants.Claims.Picture)]
        public string AvatarSrc { get; set; }
    }
}
