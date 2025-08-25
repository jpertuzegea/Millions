//----------------------------------------------------------------------- 
// Copyright (c) 2019 All rights reserved.
// </copyright>
// <author>Jorge Pertuz Egea/Jpertuz</author>
// <date>Agosto 2025</date>
//-----------------------------------------------------------------------

namespace Commons.Dtos.Configurations
{
    public class ResultModel<T> where T : class
    {
        public bool HasError { get; set; }
        public string? ExceptionMessage { get; set; }
        public string? Messages { get; set; }
        public T? Data { get; set; }
    }
}
