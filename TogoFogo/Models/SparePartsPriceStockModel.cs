using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo.Models
{
    public class SparePartsPriceStockModel
    {
        public SparePartsPriceStockModel()
        {
            CatNameList = new SelectList(Enumerable.Empty<SelectListItem>());
            SubCatNameList = new SelectList(Enumerable.Empty<SelectListItem>());
            BrandList = new SelectList(Enumerable.Empty<SelectListItem>());
            ProductNameList = new SelectList(Enumerable.Empty<SelectListItem>());
            PartNameList = new SelectList(Enumerable.Empty<SelectListItem>());
            SpareTypeNameList = new SelectList(Enumerable.Empty<SelectListItem>());
        }
        public int ModelName { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int SparePriceStockId { get; set; }
        [Required]
        [DisplayName("Device category")]
        public string CatName { get; set; }
        [Required]
        [DisplayName("Device Sub category")]
        public string SubCatName { get; set; }
        [Required]
        [DisplayName("Brand")]
        public string Brand { get; set; }
        [Required]
        [DisplayName("Product Name")]
        public string ProductName { get; set; }
        [DisplayName("TRUPC")]
        public string TRUPC { get; set; }
        [Required]
        [DisplayName("Spare Type Name")]
        public string SpareTypeName { get; set; }
        [Required]
        [DisplayName("Part Name")]
        public string PartName { get; set; }
        [DisplayName("TGFG Part Code")]
        public string TGFGPartCode { get; set; }
        [DisplayName("CTH Number (GST Tariff item - HSN Code)")]
        public string CTH_Number { get; set; }
        public HttpPostedFileBase SparePartPhoto { get; set; }
        public int LocationId { get; set; }
        public int SpareTypeId { get; set; }
        public int SpareNameID { get; set; }
        public int PartId { get; set; }
        [DisplayName("Spare Code")]
        public string SpareCode { get; set; }
        [Required]
        [DisplayName("Estimated Spare Price (INR)")]
        public string EstimatedPrice { get; set; }
        [Required]
        [DisplayName("Market Spare Price (INR)")]
        public string MarketPrice { get; set; }
        [Required]
        [DisplayName("Quantity")]
        public string SpareQty { get; set; }
        [Required]
        [DisplayName("Is Active")]
        public string IsActive { get; set; }
        [DisplayName("Spare Part Photo")]
        public string Part_Image { get; set; }
        public SelectList CatNameList { get; set; }
        public SelectList SubCatNameList { get; set; }
        public SelectList BrandList { get; set; }
        public SelectList ProductNameList { get; set; }
        public SelectList PartNameList { get; set; }
        public SelectList SpareTypeNameList { get; set; }

    }
}