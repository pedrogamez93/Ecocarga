using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cl.Gob.Energia.Ecocarga.Api
{
    public class TransactionResponse
    {
        public int Id { get; set; }
        public bool Error { get; set; }
        public string Message { get; set; }
        public string UserMessage { get; set; }

        public TransactionResponse()
        {
            Error = false;
            Message = string.Empty;
            UserMessage = string.Empty;
        }
    }
}
