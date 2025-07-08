using System;
using System.Collections.Generic;

namespace DataModel.JobEntityModel.HrJobTable
{
    public partial class TDist
    {
        public TDist()
        {
            TUpzls = new HashSet<TUpzl>();
        }

        public string Oid { get; set; } = null!;
        public string? DistText { get; set; }
        public string? DistName { get; set; }
        public string? DistBnam { get; set; }
        public string? IsActive { get; set; }
        public string? Iuser { get; set; }
        public string? Euser { get; set; }
        public DateTime? DtInserted { get; set; }
        public DateTime? DtModified { get; set; }
        public byte[]? Roguid { get; set; }
        public string? DivOid { get; set; }

        public virtual ICollection<TUpzl> TUpzls { get; set; }
    }
}
