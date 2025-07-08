using System;
using System.Collections.Generic;

namespace DataModel.JobEntityModel.HrJobTable
{
    public partial class TAcmp
    {
        public string Oid { get; set; } = null!;
        public string AcmpText { get; set; } = null!;
        public string AcmpName { get; set; } = null!;
        public string? AcmpBnam { get; set; }
        public string? AcmpActv { get; set; }
        public string? Iuser { get; set; }
        public string? Euser { get; set; }
        public DateTime? Idat { get; set; }
        public DateTime? Edat { get; set; }
        public DateTime? Udt { get; set; }
        public string? AcmpAdd1 { get; set; }
        public decimal? AcmpSeqn { get; set; }
        public string? SlocOid { get; set; }
        public string? ItaxFlag { get; set; }
        public string? AcmpCcom { get; set; }
        public string? AcmpMcom { get; set; }
    }
}
