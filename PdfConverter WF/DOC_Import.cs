using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PdfConverter_WF
{
    class DOC_Import
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DOC_Import()
        {
            DOC_ImportDetalii = new HashSet<DOC_ImportDetalii>();
        }
        [Key]
        public long Id { get; set; }

        public Guid? DocumentID { get; set; }

        [StringLength(50)]
        public string StructuraCod { get; set; }

        [StringLength(50)]
        public string TipCod { get; set; }

        [StringLength(50)]
        public string Numar { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Data { get; set; }
        public DateTime? Scadenta { get; set; }

        //[Column(TypeName = "smalldatetime")]
        //public DateTime? DataDoc { get; set; }

        [StringLength(50)]
        public string PartenerCUI { get; set; }

        [StringLength(250)]
        public string PartenerNume { get; set; }

        //[StringLength(50)]
        //public string GestiuneIntrareCod { get; set; }

        //[StringLength(50)]
        //public string GestionarIntrareCod { get; set; }

        //[StringLength(50)]
        //public string GestiuneIesireCod { get; set; }

        //[StringLength(50)]
        //public string GestionarIesireCod { get; set; }

        [StringLength(50)]
        public string ValutaSimbol { get; set; }

        [Column(TypeName = "money")]
        public decimal? Curs { get; set; }

        [Column(TypeName = "money")]
        public decimal? TVA { get; set; }

        public string Explicatie { get; set; }

        public string Observatii { get; set; }

        //[StringLength(50)]
        //public string ContBancaCasaCod { get; set; }

        [StringLength(50)]
        public string ContractCod { get; set; }

        [StringLength(50)]
        public string ComandaCod { get; set; }

        [StringLength(50)]
        public string ProiectCod { get; set; }

        //[StringLength(50)]
        //public string LotFabricatieCod { get; set; }

        //public Guid? DocGeneratAutomatID { get; set; }

        //[Column(TypeName = "smalldatetime")]
        //public DateTime? RezervatData { get; set; }

        [StringLength(50)]
        public string AgentCod { get; set; }

        //public Guid? SpitalizareID { get; set; }

        //[StringLength(250)]
        //public string UserCreare { get; set; }

        //public DateTime? DataCreare { get; set; }

        //[StringLength(250)]
        //public string UserModificare { get; set; }

        //public DateTime? DataModificare { get; set; }

        //[StringLength(50)]
        //public string StatusERA { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<DOC_ImportDetalii> DOC_ImportDetalii { get; set; }

        public ICollection<DOC_ImportDetalii> DOC_ImportDetalii { get; set; }
    }
}
