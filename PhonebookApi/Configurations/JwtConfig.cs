
using System;

namespace PhonebookApi.Configurations
{
    public class JwtConfig
    {
        public string Secret { get; set; }

        public int ExpiresIn { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }
    }
}
