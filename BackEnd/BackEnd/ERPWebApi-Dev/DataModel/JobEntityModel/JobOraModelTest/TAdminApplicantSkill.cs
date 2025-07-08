using System;
using System.Collections.Generic;

namespace DataModel.JobEntityModel.JobOraModelTest
{
    public partial class TAdminApplicantSkill
    {
        public string Oid { get; set; } = null!;
        public decimal Numid { get; set; }
        public string JobpostOid { get; set; } = null!;
        public string? Skill { get; set; }
        public string? Isactive { get; set; }
        public string? Iscancel { get; set; }
    }
}
