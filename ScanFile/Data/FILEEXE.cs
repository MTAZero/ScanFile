namespace ScanFile.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FILEEXE")]
    public partial class FILEEXE
    {
        public int ID { get; set; }

        public string TEN { get; set; }

        public string DUONGDAN { get; set; }

        public int SOLANSUDUNG { get; set; }
    }
}
