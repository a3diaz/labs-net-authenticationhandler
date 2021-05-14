using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Labs.NET.AuthenticationHandler.WebAPI
{
    public class FakeUserInfo
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
    }
}
