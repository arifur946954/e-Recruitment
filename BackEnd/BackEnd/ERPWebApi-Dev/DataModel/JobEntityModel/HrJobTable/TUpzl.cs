using System;
using System.Collections.Generic;

namespace DataModel.JobEntityModel.HrJobTable
{
    public partial class TUpzl
    {
        public string Oid { get; set; } = null!;
        public string? UpzlText { get; set; }
        public string? UpzlDist { get; set; }
        public string? UpzlName { get; set; }
        public string? UpzlBnam { get; set; }
        public string? UpzlActv { get; set; }
        public string? Iuser { get; set; }
        public DateTime? Idat { get; set; }
        public string? Euser { get; set; }
        public DateTime? Edat { get; set; }

        public virtual TDist? UpzlDistNavigation { get; set; }
    }
}
