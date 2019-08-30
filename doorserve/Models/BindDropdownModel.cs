﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace doorserve.Models
{
    public class BindProblemObserved
    {
        public int ProblemId { get; set; }
        public string ProblemObserved { get; set; }

    }
    public class BindMstDeviceProblem
    {
        public string ProblemID { get; set; }
        public string Problem { get; set; }
    }
    public class BindColor
    {
        public int ColorId { get; set; }
        public string ColorName { get; set; }
    }
    public class BindGst
    {
        public int GstCategoryId { get; set; }
        public string GSTCategory { get; set; }

    }
    public class MenuMasterModel
    {
        public int Id { get; set; }
        public int MenuCapId { get; set; }
        public string CapName { get; set; }
        public string Path { get; set; }
        [DisplayName("Menu Name")]
        [Required]
        public string Menu_Name { get; set; }
        public int shortOrder   { get; set; }
        public string PagePath { get; set; }
        public int ParentMenuId { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string IconFileName { get; set; }
        public HttpPostedFileBase IconFileNamePath { get; set; }
        public string IconFileNameUl { get; set; }
        public string ParentMenuName { get; set; }
        public string MenuCode { get; set; }  
        public Boolean CheckedStatus { get; set; }
        public string ActionIds { get; set; }
        public Int64 ActionRightId { get; set; }
        public string ServiceTypeIds  { get; set; } 
        public List<CheckBox> ServiceTypeList { get; set; }
        public List<CheckBox> RightActionList { get; set; }
        public List<MenuMasterModel> ParentMenuList { get; set; }
        public List<MenuMasterModel> SubMenuList { get; set; }
    }
    public class BindTrcModel
    {
        public int TRC_Id { get; set; }
        public string TRC_NAME { get; set; }

    }
    public class BindSparePartnameModel
    {
        public int PartId { get; set; }
        public string PartName { get; set; }

    }
    public class BindCourierModel
    {
        public string CourierId { get; set; }
        public string CourierName { get; set; }

    }
    public class BindEngineerModel
    {
        public Guid EmpId { get; set; }
        public string EmployeeName { get; set; }

    }
    public class BindPinCodeModel
    {
        public string ID { get; set; }
        public string pin_code { get; set; }
       
    }
    public class BindServiceProviderModel
    {
        public Guid? ProviderId { get; set; }
        public string ProviderName { get; set; }

    }
    public class BindQCModel
    {
        public string QCId { get; set; }
        public string QCProblem { get; set; }

    }
    public class Status_MasterModel
    {
        public string StatusId { get; set; }
        public string StatusName { get; set; }

    }
    public class BindDropdownModel
    {
        public string BrandId { get; set; }
        public string BrandName { get; set; }
        public string CatId { get; set; }
        public string CatName { get; set; }
    }

    public class SubCategoryModel
    {
        public string SubCatId { get; set; }
        public string CatId { get; set; }
        public string SubCatName { get; set; }
    }
    public class SpareTypeModel
    {
        public string SpareTypeName { get; set; }
        public string SpareTypeId { get; set; }
    }
    public class CountryModel
    {
        public string Cnty_ID { get; set; }
        public string Cnty_Name { get; set; }
    }
    public class StateModel
    {
        public string St_ID { get; set; }
        public string St_Name { get; set; }
    }
    public class BindDropdown
    {
        public int St_ID { get; set; }
        public string St_Name { get; set; }
    }
    public class BindDropdown1
    {
        public string  ID { get; set; }
        public string St_ID { get; set; }
        public string St_Name { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public string district { get; set; }
        public string state_Name { get; set; }
        public string Problem { get; set; }
        public string ProblemID { get; set; }
        public string ProductName { get; set; }
        public string ProductId { get; set; }
        public string BrandId { get; set; }
        public string BrandName { get; set; }
        public string State { get; set; }
        public int StateId { get; set; }
    }
    public class BindLocation
    {
        public int LocationId { get; set; }
        public string LocationName { get; set; }
    }
    public class BindDeviceModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
    }

   
}