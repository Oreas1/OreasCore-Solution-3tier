using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OreasModel
{
    [Keyless]
    public class USP_Pro_Composition_GetMaterialAvailability
    {
        public string FilterName { get; set; }
        public string WareHouseName { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public double FormulaQty { get; set; }
        public double FormulaBatchSize { get; set; }
        public double FormulaPackSize { get; set; }
        public string UnitPrimary { get; set; }

        [Column(TypeName = "decimal(18, 4)")]
        public decimal AvailableQty { get; set; }
        public int ITEMID { get; set; }
        public string UnitItem { get; set; }
        public string FormulaFor { get; set; }

    }
}
