using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestExcelAddin.Export
{
    public class ExportParameters
    {
        public ExportParameters()
        {
            ShowPerformance = true;
        }

        public DateTime? StartDate
        {
            get;
            set;
        }

        public DateTime? EndDate
        {
            get;
            set;
        }

        public bool ShowPerformance
        {
            get;
            set;
        }

        public bool ExportObjectId
        {
            get;
            set;
        }

        //public SEMObjectDetailType ExportDetailType
        //{
        //    get;
        //    set;
        //}

        //public SEMObjectType ExportObjectType
        //{
        //    get;
        //    set;
        //}

        //public SEMObjectBase ExportParent
        //{
        //    get;
        //    set;
        //}

        //public SEMAccountBase ExportParentAccount
        //{
        //    get;
        //    set;
        //}
        public bool IsMultiCampaigns { get; set; }
        public long[] CampaignIds { get; set; }

        public string IDColumnName { get; set; }
        public long[] DeletedIds { get; set; }
        public double SliceBid { get; set; }
        public bool IsDuplicateKeyword { get; set; }
    }
}
