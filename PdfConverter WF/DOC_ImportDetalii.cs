using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfConverter_WF
{
    class DOC_ImportDetalii
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey("DOC_Import")]
        public long? ImportId { get; set; }
        public virtual DOC_Import DOC_Import { get; set; }

        //public Guid? LinieID { get; set; }

        //public Guid? StructCod { get; set; }

        //[StringLength(50)]
        //public string CodBare { get; set; }

        [StringLength(50)]
        public string ProdusCod { get; set; }

        [StringLength(1000)]
        public string ProdusNume { get; set; }

        public string Custom { get; set; }

        [StringLength(50)]
        public string UnitateCod { get; set; }

        [Column(TypeName = "money")]
        public decimal? Cantitate { get; set; }

        [Column(TypeName = "money")]
        public decimal? PretUnitar { get; set; }

        [Column(TypeName = "money")]
        public decimal? TVALinie { get; set; } //Cota de TVA

        //[StringLength(250)]
        //public string CategorieContabilaNume { get; set; }

        //[Column(TypeName = "money")]
        //public decimal? AdaosUnitarLinie { get; set; }

        [Column(TypeName = "money")]
        public decimal? ValoareAdaosLinie { get; set; }

        [Column(TypeName = "money")]
        public decimal? AdaosLinie { get; set; }

        [Column(TypeName = "money")]
        public decimal? ValoareTVALinie { get; set; }

        [Column(TypeName = "money")]
        public decimal? ValoareLinie { get; set; }

        //[StringLength(50)]
        //public string CategoriePretCod { get; set; }

        //[StringLength(50)]
        //public string CapitolCod { get; set; }

        public int? GarantieLuni { get; set; }

        //[Column(TypeName = "money")]
        //public decimal? Brut { get; set; }

        //public Guid? LinieLotID {/* get; set; }*/

        //[StringLength(50)]
        //public string LinieCustomCod { get; set; }​
    }
}
