using System;
using System.Collections.Generic;

namespace DataModel.JobEntityModel.JobOraModelTest
{
    public partial class TJobApplicantMainMaster
    {
        public string Oid { get; set; } = null!;
        public string? Jobid { get; set; }
        public string? Profileoid { get; set; }
        public DateTime? Applydate { get; set; }
        public string? ApplicantCode { get; set; }
        public string? Email { get; set; }
        public string? OfficialMsg { get; set; }
        public string? Isactive { get; set; }
        public string? Isdelete { get; set; }
    }
}
