using System;
using System.Collections.Generic;

namespace DataModel.JobEntityModel.HrJobTable
{
    public partial class TDsig
    {
        public string Oid { get; set; } = null!;
        public string DsigText { get; set; } = null!;
        public string DsigName { get; set; } = null!;
        public string? DsigActv { get; set; }
        public string? Iuser { get; set; }
        public string? Euser { get; set; }
        public DateTime? Idat { get; set; }
        public DateTime? Edat { get; set; }
        public DateTime? Udt { get; set; }
        public string? DsigBnam { get; set; }
        public decimal? DsigSeqn { get; set; }
        public decimal? NavSeqn { get; set; }
    }
}
