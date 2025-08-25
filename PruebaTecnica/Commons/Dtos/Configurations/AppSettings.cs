//----------------------------------------------------------------------- 
// Copyright (c) 2019 All rights reserved.
// </copyright>
// <author>Jorge Pertuz Egea/Jpertuz</author>
// <date>Agosto 2025</date>
//-----------------------------------------------------------------------

namespace Commons.Dtos.Configurations
{
    public class JWTAuthentication
    {
        public int ExpirationInMinutes { get; set; }
        public string Secret { get; set; }
        public string[] HostOriginPermited { get; set; }

    }

    public class Cache
    {
        public int ExpirationCacheInHours { get; set; }
    }


}
