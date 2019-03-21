using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace CrowdLending.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class UserEntity : IdentityUser<Guid>
    {
        [JsonProperty]
        public string FirstName { get; set; }

        [JsonProperty]
        public string LastName { get; set; }

        [JsonProperty]
        public string AvatarSrc { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}
