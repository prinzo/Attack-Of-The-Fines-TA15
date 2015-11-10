using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FineBot.API.FinesApi
{
    public class FineExportModel
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public int Fines { get; set; }
    }
}
