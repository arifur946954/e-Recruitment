using System;
using System.Collections.Generic;

namespace DataModel.JobEntityModel.JobOraModelTest
{
    public partial class TCheckAuthorisedMaster
    {
        public string Oid { get; set; } = null!;
        public string? Company { get; set; }
        public string? Bank { get; set; }
        public DateTime? AuthorisedDate { get; set; }
        public DateTime? TrDate { get; set; }
        public string? AuthorisedBy { get; set; }
        public string? Isapprove { get; set; }
        public string? ApproveBy { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string? Isauthorised { get; set; }
    }
}
