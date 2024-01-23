using System;
using System.Collections.Generic;
using System.Text;

namespace PPR.Lite.Shared.General
{
    public class AppTokenSettings<T>
    {
        public T Data { get; set; }
        public string Type { get; set; }
        public TokenConfigration Configration { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime ExpiredAt { get; set; }
        public string Token { get; set; }
        public Guid TokenIdentifier { get; set; }
    }
}
