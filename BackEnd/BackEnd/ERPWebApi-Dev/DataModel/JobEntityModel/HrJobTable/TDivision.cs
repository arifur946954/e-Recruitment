using System;
using System.Collections.Generic;

namespace DataModel.JobEntityModel.HrJobTable
{
    public partial class TDivision
    {
        public string? Oid { get; set; }
        public string? DivName { get; set; }
        public string? DivNameBn { get; set; }
        public string? Active { get; set; }
        public string? Createby { get; set; }
        public DateTime? Createon { get; set; }
        public string? Createpc { get; set; }
        public string? Updateby { get; set; }
        public DateTime? Updateon { get; set; }
        public string? Updatepc { get; set; }
        public string? Cancelby { get; set; }
        public DateTime? Cancelon { get; set; }
        public string? Cancelpc { get; set; }
    }
}
