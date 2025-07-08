using System;
using System.Collections.Generic;

namespace DataModel.JobEntityModel.JobOraModelTest
{
    public partial class TCheckAuthorisedDetail
    {
        public string Oid { get; set; } = null!;
        public string? AuthorisedOid { get; set; }
        public string? AccNo { get; set; }
        public string? CheckNo { get; set; }
        public string? TrNo { get; set; }
    }
}
