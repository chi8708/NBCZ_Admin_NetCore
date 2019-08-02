using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NBCZ.Api.Model.Request
{
    public class PageDataReq
    {
      //  int pageNum = 1, int pageSize = 10, string field = "id", string order = " desc "
        public int pageNum { get; set; } = 1;
        public int pageSize { get; set; } = 10;

        public string field { get; set; }
        public string order { get; set; }

        public Dictionary<string, object> query{ get; set; }
}
}
