using System;
using System.Collections.Generic;

namespace DataModel.JobEntityModel.HrJobTable
{
    public partial class TDept
    {
        public string Oid { get; set; } = null!;
        public string DeptText { get; set; } = null!;
        public string DeptName { get; set; } = null!;
        public string DeptActv { get; set; } = null!;
        public string? Iuser { get; set; }
        public string? Euser { get; set; }
        public DateTime? Idat { get; set; }
        public DateTime? Edat { get; set; }
        public DateTime? Udt { get; set; }
        public string? DeptBnam { get; set; }
        public string? InvFlag { get; set; }
        public string? NavFlag { get; set; }
        public string? SptvFlag { get; set; }
        public string? DeptAcmp { get; set; }
    }
}
