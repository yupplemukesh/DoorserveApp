using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using TogoFogo.Models;
using TogoFogo.Permission;

namespace TogoFogo.Controllers
{
    public class PurchaseProcurementController : Controller
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        DropdownBindController dropdown = new DropdownBindController();
        // GET: PurchaseProcurement
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, "Manage Spare Parts Price & Stock")]
        public ActionResult ManageSparePartsPriceandStock()
        {
            ViewBag.CatName = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.SubCatName = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.Brand = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.ProductName = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.PartName = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.SpareTypeName = new SelectList(Enumerable.Empty<SelectListItem>());
            if (TempData["AddPriceandStock"] != null)
            {
                ViewBag.AddPriceandStock = TempData["AddPriceandStock"].ToString();
            }
            var _UserActionRights = (UserActionRights)HttpContext.Items["ActionsRights"];
            return View(_UserActionRights);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, "Manage Spare Parts Price & Stock")]
        public ActionResult AddSparePartsPriceandStock()
        {
            //using (var con = new SqlConnection(_connectionString))
            //{
            //    var result = con.Query<SparePartsPriceStockModel>("GetSparePriceData",
            //            new{ID=1}, commandType: CommandType.StoredProcedure).FirstOrDefault();


            //    return View(result);
            //}
            var sparepartspricestock = new SparePartsPriceStockModel();
            sparepartspricestock.CatNameList = new SelectList(dropdown.BindCategory(), "Value", "Text");
            sparepartspricestock.SubCatNameList = new SelectList(Enumerable.Empty<SelectListItem>());
            sparepartspricestock.BrandList = new SelectList(dropdown.BindBrand(), "Value", "Text");
            sparepartspricestock.ProductNameList = new SelectList(Enumerable.Empty<SelectListItem>());
            sparepartspricestock.PartNameList = new SelectList(Enumerable.Empty<SelectListItem>());
            sparepartspricestock.SpareTypeNameList = new SelectList(dropdown.BindSpareType(), "Value", "Text");
            return PartialView(sparepartspricestock);
        }
        [HttpPost]
        public ActionResult AddSparePartsPriceandStock(SparePartsPriceStockModel model)
        {
            try
            {

                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("Add_Edit_SpareStock",
                        new
                        {
                            model.SparePriceStockId,
                            CategoryId=model.CatName,
                            SubCatId=model.SubCatName,
                            BrandId=model.Brand,
                            ModelName=model.ProductName,
                            model.CTH_Number,
                            model.TRUPC,
                            model.TGFGPartCode,
                            SpareTypeId=model.SpareTypeName,
                            SpareNameID=model.PartName,
                            model.SpareCode,
                            model.EstimatedPrice,
                            model.MarketPrice,
                            model.SpareQty,
                            model.IsActive,
                            User = "",
                            Action = "add"
                        }
                        , commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 1)
                    {

                        TempData["AddPriceandStock"] = "Successfully Added";
                    }
                    else
                    {
                        TempData["AddPriceandStock"] = "Location Name Already Exist";
                    }
                }

            }
            catch (Exception e)
            {

                throw;
            }

            return RedirectToAction("ManageSparePartsPriceandStock");
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View}, "Manage Spare Parts Price & Stock")]
        public ActionResult PriceAndStockTable()
        {
            SparePartsPriceStockModel objSparePartsPriceStockModel = new SparePartsPriceStockModel();
            using (var con = new SqlConnection(_connectionString))
            {
                objSparePartsPriceStockModel._SparePartsPriceStockList = con.Query<SparePartsPriceStockModel>("Get_price_stock", null, commandType: CommandType.Text).ToList();

                //return View(result);
            }
            objSparePartsPriceStockModel._UserActionRights = (UserActionRights)HttpContext.Items["ActionsRights"];
            return View(objSparePartsPriceStockModel);
        }

        public JsonResult GetSpareCode(int partId)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<GetData>("select SpareCode,Part_Image from MstSparePart where PartId=@PartId", new { @PartId = partId }, commandType: CommandType.Text).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, "Manage Spare Parts Price & Stock")]
        public ActionResult EditSparePartsPriceandStock(int sparePriceStockId)
        {
           
            ViewBag.CatName = new SelectList(dropdown.BindCategory(), "Value", "Text");
            ViewBag.SubCatName = new SelectList(dropdown.BindSubCategory(),"Value","Text");
            ViewBag.Brand = new SelectList(dropdown.BindBrand(), "Value", "Text");
            
           
            ViewBag.SpareTypeName = new SelectList(dropdown.BindSpareType(), "Value", "Text");
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<SparePartsPriceStockModel>("SELECT * from MstSparePriceStock where SparePriceStockId=@sparePriceStockId",
                    new { @sparePriceStockId = sparePriceStockId }, commandType: CommandType.Text).FirstOrDefault();
                if (result !=null)
                {
                    result.SpareTypeName = result.SpareTypeId.ToString();
                    result.SpareTypeName = result.SpareNameID.ToString();
                    result.CatName = result.CategoryId.ToString();
                    result.SubCatName = result.SubCategoryId.ToString();
                    ViewBag.PartName = new SelectList(dropdown.BindPartname(result.SpareNameID), "Value", "Text");
                    ViewBag.ProductName = new SelectList(dropdown.BindProduct(result.BrandId), "Value", "text");
                    result.PartName = result.SpareNameID.ToString();
                    result.Brand = result.BrandId.ToString();
                    result.ProductName = result.ModelName.ToString();
                }
               
                PartialView("EditSparePartsPriceandStock", result);
            }

            return View();
        }
        [HttpPost]
        public ActionResult EditSparePartsPriceandStock(SparePartsPriceStockModel model)
        {
            try
            {

                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("Add_Edit_SpareStock",
                        new
                        {
                            model.SparePriceStockId,
                            CategoryId = model.CatName,
                            SubCatId = model.SubCatName,
                            BrandId = model.Brand,
                            ModelName = model.ProductName,
                            model.CTH_Number,
                            model.TRUPC,
                            model.TGFGPartCode,
                            SpareTypeId = model.SpareTypeName,
                            SpareNameID = model.PartName,
                            model.SpareCode,
                            model.EstimatedPrice,
                            model.MarketPrice,
                            model.SpareQty,
                            model.IsActive,
                            User = "",
                            Action = "edit"
                        }
                        , commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 2)
                    {

                        TempData["AddPriceandStock"] = "Updated Successfully";
                    }
                    else
                    {
                        TempData["AddPriceandStock"] = "Something Went Wrong";
                    }
                }

            }
            catch (Exception e)
            {

                throw;
            }

            return RedirectToAction("ManageSparePartsPriceandStock");
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, "Repair Cost Estimation")]
        public ActionResult RCE()
        {
            ViewBag.ReceivedDevice = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.RecvdBrand = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.RecvdModel = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.Engg_Name = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.ReceivedDevice = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.SpareType = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.ProblemFound = new SelectList(Enumerable.Empty<SelectListItem>());
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();

            }
            var _UserActionRights = (UserActionRights)HttpContext.Items["ActionsRights"];
            return View(_UserActionRights);
        }
        public ActionResult FindRCE()
        {
            return View();
        }

        public ActionResult RCEForm()
        {
            var rpcap = new RPCAPModel();
            rpcap.ReceivedDeviceList1 = new SelectList(dropdown.BindCategory(), "Value", "Text");
            rpcap.RecvdBrandList = new SelectList(dropdown.BindBrand(), "Value", "Text");
            rpcap.RecvdModelList1 = new SelectList(dropdown.BindProduct(), "Value", "Text");
            rpcap.Engg_NameList1 = new SelectList(dropdown.BindEngineer(), "Value", "Text");
            rpcap.SpareTypeList1 = new SelectList(dropdown.BindSpareType(), "Value", "Text");
            rpcap.SpareNameList1 = new SelectList(Enumerable.Empty<SelectListItem>());
            rpcap.ProblemFoundList1 = new SelectList(dropdown.BindProblemObserved(), "Value", "Text");
            return PartialView(rpcap);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, "Repair Cost Estimation")]
        public ActionResult TableRCE()
        {
            RPCAPModel objRpcaModel = new RPCAPModel();
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<AllData>("GetTableDataForAllPages",
                   new { }, commandType: CommandType.StoredProcedure).ToList();
                //return View(result);
            }
            objRpcaModel._UserActionRights = (UserActionRights)HttpContext.Items["ActionsRights"];
            return View(objRpcaModel);

        }
        public ActionResult SPPLtable()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<AllData>("GetTableDataForAllPages",
                   new { }, commandType: CommandType.StoredProcedure).ToList();
                return View(result);
            }
        }
        public ActionResult SPPL()
        {
            return View();
        }
        public ActionResult FindSPPL()
        {
            return View();
        }
        public ActionResult SPPLForm(string CC_NO)
        {
            ViewBag.ReceivedDevice = new SelectList(dropdown.BindCategory(), "Value", "Text");
            ViewBag.RecvdBrand = new SelectList(dropdown.BindBrand(), "Value", "Text");
            ViewBag.RecvdModel = new SelectList(dropdown.BindProduct(), "Value", "Text");
            ViewBag.Engg_Name = new SelectList(dropdown.BindEngineer(), "Value", "Text");
            ViewBag.SpareType = new SelectList(dropdown.BindSpareType(), "Value", "Text");
            ViewBag.SpareName = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.ProblemFound = new SelectList(dropdown.BindProblemObserved(), "Value", "Text");
            using (var con = new SqlConnection(_connectionString))
            {

                var result = con.Query<AllData>("GetDataByCCNO",
               new { CC_NO = CC_NO }, commandType: CommandType.StoredProcedure).FirstOrDefault();

                var problem = "";
                var problemFound = "";
                var QCReason = "";
                if (result.PrblmObsrvd != null)
                {
                    foreach (var item in result.PrblmObsrvd)
                    {
                        if (item != ',')
                        {
                            var result1 = con.Query<string>("select ProblemObserved from MstProblemObserved WHERE ProblemId =@ProblemId ", new { @ProblemId = item }, commandType: CommandType.Text).FirstOrDefault();
                            problem = problem + result1 + " , ";
                        }

                    }
                    result.PrblmObsrvd = problem;
                }

                if (result.PfelsProblemFound != null)
                {
                    foreach (var item in result.PfelsProblemFound)
                    {
                        if (item != ',')
                        {
                            var result1 = con.Query<string>("select ProblemObserved from MstProblemObserved WHERE ProblemId =@ProblemId ", new { @ProblemId = item }, commandType: CommandType.Text).FirstOrDefault();
                            problemFound = problemFound + result1 + " , ";
                        }

                    }
                    result.PfelsProblemFound = problemFound;
                }
                if (result.QC_Fail_Reason != null)
                {
                    foreach (var item in result.QC_Fail_Reason)
                    {
                        if (item != ',')
                        {
                            var result1 = con.Query<string>("SELECT QCProblem from MST_QC WHERE QCId=@QCId ", new { @QCId = item }, commandType: CommandType.Text).FirstOrDefault();
                            QCReason = QCReason + result1 + " , ";
                        }

                    }
                    result.QC_Fail_Reason = QCReason;
                }
                var d = con.Query<spareTestPFELSForm1>("getTableDataList",
                   new { CC_NO = CC_NO }, commandType: CommandType.StoredProcedure).ToList();
                result.TableData1 = d;
                int spareCost1 = 0;
                foreach (var item in result.TableData1)
                {

                    int sc = Int32.Parse(item.TablespareTotalField1);
                    spareCost1 = spareCost1 + sc;
                }

                result.ApprovedSpareCost = spareCost1.ToString();
                var QC = con.Query<QCtableData>("Select * from mst_QC", null, commandType: CommandType.Text).ToList();
                result.QC_Data = QC;

                return View(result);
            }
        }
    }
}