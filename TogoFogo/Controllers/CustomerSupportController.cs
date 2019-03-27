using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using Dapper;
using TogoFogo.Models;
using System.Data;
using System.Configuration;
using System.Threading.Tasks;

namespace TogoFogo.Controllers
{
    public class CustomerSupportController : Controller
    {
        private readonly string _connectionString =
        ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        DropdownBindController dropdown = new DropdownBindController();
        // GET: CustomerSupport  
        public ActionResult PCRC()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();

            }
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<EditRepairStatus>("getDataInPCRC",
                     commandType: CommandType.StoredProcedure).ToList();

                return View(result);
            }
        }

        public ActionResult EditRequest(string reqno)
        {
           
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<EditRepairStatus>("GetDataByCCNO",
                     new { CC_NO = reqno }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                var Problem = con.Query<GetProblem_Child_Order_problem>("GetProblem_From_Child_Order_problem",
                    new { CC_NO = result.CcNo }, commandType: CommandType.StoredProcedure).ToList();

                foreach (var item in Problem)
                {
                    var result1 = con.Query<string>("select Problem from mstdeviceproblem WHERE ProblemId =@ProblemId ", new { @ProblemId = item.ProblemId }, commandType: CommandType.Text).FirstOrDefault();
                    //var ProblemName = con.Query<string>("select Problem from mstdeviceproblem where ProblemID@ProblemID", new { @ProblemID =  item.ProblemId}, commandType: CommandType.Text).FirstOrDefault();
                    item.ProblemName = result1;
                }
                result.CallStatusList = new SelectList(dropdown.BindCallStatus(), "Value", "Text");
                result.ProblemList = new SelectList(dropdown.BindMstDeviceProblem(), "Value", "Text");

                if (Problem != null)
                {
                    result.Pro = Problem;
                }
                else { new List<GetProblem_Child_Order_problem>(); }



                return View(result);
            }
        }

        public JsonResult RemoveProblemButton(string ProblemId, string EstimatedPrice, string OrderId)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Execute("delete from Child_Order_problem where OrderId = @OrderId and ProblemId= @ProblemId and Estimated_Price=@Estimated_Price",
                new { @OrderId = OrderId, @ProblemId = ProblemId, @Estimated_Price = EstimatedPrice }, commandType: CommandType.Text);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetProblemRow(int? DropDownValue, string ModelName, string OrderId, string BrandName)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var ModelId = con.Query<int>("select productId from MstProduct WHERE ProductName=@ProductName", new { @ProductName = ModelName }, commandType: CommandType.Text).FirstOrDefault();
                var BrandId = con.Query<int>("select BrandId from MstBrand Where BrandName=@BrandName", new { @BrandName = BrandName }, commandType: CommandType.Text).FirstOrDefault();
                var result = con.Query<GetProblem_Child_Order_problem>("select estimated_Price from Probles_VS_Price_Matrix WHERE Model_Id=@Model_Id and Problem_Id=@Problem_Id", new { @Model_Id = ModelId, @Problem_Id = DropDownValue }
                , commandType: CommandType.Text).FirstOrDefault();
                result.ProblemId = ModelId.ToString();

                var Insert_Into_Child_Order_problem = con.Execute("insert into Child_Order_problem values(@OrderId,@ProblemId,@Estimated_Price,@BrandId,@ModelId)", new { OrderId, ProblemId = DropDownValue, Estimated_Price = result.Estimated_Price, BrandId = BrandId, ModelId = ModelId },
                      commandType: CommandType.Text);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult EditRequest(EditRepairStatus Emodel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("PCRC");
            }
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<int>("Insert_CustomerSupportAction",
                    new
                    {
                        CC_NO = Emodel.CcNo,
                        Emodel.CallStatus,
                        Emodel.CallBackDatetime,
                        Emodel.Customer_Name,
                        Emodel.Mobile_No,
                        Emodel.Email_Id,
                        Emodel.Cust_Add,
                        Emodel.Cust_City,
                        Emodel.Cust_State,
                        Emodel.Remarks,

                    }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                if (result == 2)
                {
                    TempData["Message"] = "Submitted Successfully";

                }
                else
                {
                    TempData["Message"] = "Something Went Wrong";
                }
                return RedirectToAction("PCRC");
            }
        }
        public ActionResult POWRR(string CcNO)
        {
            //ViewBag.PrblmObsrvdPoowrr = new SelectList(Enumerable.Empty<SelectListItem>());
            /* ViewBag.SpareType = new SelectList(Enumerable.Empty<SelectListItem>());
             ViewBag.SpareName = new SelectList(Enumerable.Empty<SelectListItem>());
             ViewBag.SelectTrc = new SelectList(Enumerable.Empty<SelectListItem>());
             ViewBag.CourierName = new SelectList(Enumerable.Empty<SelectListItem>());
             ViewBag.CallStatus = new SelectList(Enumerable.Empty<SelectListItem>());
             ViewBag.ServiceProviderName = new SelectList(Enumerable.Empty<SelectListItem>());*/
            if (CcNO != null)
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<AllData>("GetDataByCCNO",
                    new { CC_NO = CcNO }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    result.SpareTypeList = new SelectList(Enumerable.Empty<SelectListItem>());
                    result.SpareNameList = new SelectList(Enumerable.Empty<SelectListItem>());
                    result.SelectTrcList = new SelectList(Enumerable.Empty<SelectListItem>());
                    result.CourierNameList = new SelectList(Enumerable.Empty<SelectListItem>());
                    result.CallStatusList = new SelectList(Enumerable.Empty<SelectListItem>());
                    result.ServiceProviderNameList = new SelectList(Enumerable.Empty<SelectListItem>());
                    return View(result);
                }

            }
            return View();
        }

        public ActionResult POOWRR()
        {
            ViewBag.PrblmObsrvdPoowrr = new SelectList(Enumerable.Empty<SelectListItem>());

             ViewBag.SpareType = new SelectList(Enumerable.Empty<SelectListItem>());
             ViewBag.SpareName = new SelectList(Enumerable.Empty<SelectListItem>());
             ViewBag.SelectTrc = new SelectList(Enumerable.Empty<SelectListItem>());
             ViewBag.CourierName = new SelectList(Enumerable.Empty<SelectListItem>());
             ViewBag.CallStatus = new SelectList(Enumerable.Empty<SelectListItem>());
             ViewBag.ServiceProviderName= new SelectList(Enumerable.Empty<SelectListItem>());
             //ViewBag.Problem = new SelectList(Enumerable.Empty<SelectListItem>());
             //ViewBag.Problem = new SelectList(dropdown.BindMstDeviceProblem(), "Value", "Text");
             //ViewBag.Problem = new SelectList(dropdown.BindMstDeviceProblemAbhishek(), "Value", "Text");
             ViewBag.Problem = new SelectList(Enumerable.Empty<SelectListItem>());
             ViewBag.WS = new SelectList(Enumerable.Empty<SelectListItem>());
           // AllData ad = new AllData
            //{
            //    SpareTypeList = new SelectList(Enumerable.Empty<SelectListItem>()),
            //    SpareNameList = new SelectList(Enumerable.Empty<SelectListItem>()),
            //    SelectTrcList = new SelectList(Enumerable.Empty<SelectListItem>()),
            //    CourierNameList = new SelectList(Enumerable.Empty<SelectListItem>()),
            //    CallStatusList = new SelectList(Enumerable.Empty<SelectListItem>()),
            //    ServiceProviderNameList = new SelectList(Enumerable.Empty<SelectListItem>()),
            //    ProblemList = new SelectList(dropdown.BindMstDeviceProblem(), "Value", "Text"),
            //    WSList = new SelectList(Enumerable.Empty<SelectListItem>())
            //};


            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();

            }
            return View();
        }
        public ActionResult FindPOOWRR()
        {

            return View();
        }
        [HttpPost]
        public ActionResult FindPOOWRR(string CcNO)
        {

            using (var con = new SqlConnection(_connectionString))
            {
                var result = new AllData();

                result = con.Query<AllData>("GetDataByCCNO",
                    new { CC_NO = CcNO }, commandType: CommandType.StoredProcedure).FirstOrDefault();

                //var Problem1 = con.Query<CourierValuesModel>("get_Customer_problem_New",
                //   new { CC_NO = CcNO }, commandType: CommandType.StoredProcedure).ToList();

                var Problem = con.Query<GetProblem_Child_Order_problem>("GetProblem_From_Child_Order_problem",
                  new { CC_NO = CcNO }, commandType: CommandType.StoredProcedure).ToList();
                //var spareData = "";
                foreach (var item in Problem)
                {
                    var result1 = con.Query<string>("select Problem from mstdeviceproblem WHERE ProblemId =@ProblemId ", new { @ProblemId = item.ProblemId }, commandType: CommandType.Text).FirstOrDefault();
                    //var GetSpareType = con.Query<string>("select SpareId from Model_Problem_Spare_Price_Matrix Where BrandId=@BrandId and ModelId=@ModelId and ProblemId=@ProblemId ", new { @ProblemId = item.ProblemId, @BrandId=item.BrandId, @ModelId=item.ModelId }, commandType: CommandType.Text).FirstOrDefault();
                    item.ProblemName = result1;
                    //spareData = spareData + "," + GetSpareType;
                    // ViewBag.WS = new SelectList(dropdown.BindWarrantyDropdown(Convert.ToInt32(item.ModelId)), "Value", "Text");
                    result.WSList = new System.Web.Mvc.SelectList(dropdown.BindWarrantyDropdown(Convert.ToInt32(item.ModelId)), "Value", "Text");
                    TempData["ModelID"] = item.ModelId;
                }
                if (Problem != null && Problem.Count > 0)
                {
                    result.ChildtableDataProblem = Problem;
                }
                else
                {
                    new List<GetProblem_Child_Order_problem>();
                }
                result.Problem = null;



                if (result.StatusName == "Pending")
                {
                    result.CurrentStatus = "Request received";
                }


                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<ActionResult> POOWRRForm( string CcNO)
        {

            // ReceiveMaterials rm=new ReceiveMaterials
            using (var con = new SqlConnection(_connectionString))
            {
  

               var result = con.Query<ReceiveMaterials>("GetDataByCCNO",
                    new { CC_NO = CcNO }, commandType: CommandType.StoredProcedure).FirstOrDefault();

                //var Problem1 = con.Query<CourierValuesModel>("get_Customer_problem_New",
                //   new { CC_NO = CcNO }, commandType: CommandType.StoredProcedure).ToList();

                var Problem = con.Query<GetProblem_Child_Order_problem>("GetProblem_From_Child_Order_problem",
                  new { CC_NO = CcNO }, commandType: CommandType.StoredProcedure).ToList();
                //var spareData = "";
                foreach (var item in Problem)
                {
                    var result1 = con.Query<string>("select Problem from mstdeviceproblem WHERE ProblemId =@ProblemId ", new { @ProblemId = item.ProblemId }, commandType: CommandType.Text).FirstOrDefault();
                    //var GetSpareType = con.Query<string>("select SpareId from Model_Problem_Spare_Price_Matrix Where BrandId=@BrandId and ModelId=@ModelId and ProblemId=@ProblemId ", new { @ProblemId = item.ProblemId, @BrandId=item.BrandId, @ModelId=item.ModelId }, commandType: CommandType.Text).FirstOrDefault();
                    item.ProblemName = result1;
                    //spareData = spareData + "," + GetSpareType;
                    // ViewBag.WS = new SelectList(dropdown.BindWarrantyDropdown(Convert.ToInt32(item.ModelId)), "Value", "Text");
                    result.WSList = new System.Web.Mvc.SelectList(dropdown.BindWarrantyDropdown(Convert.ToInt32(item.ModelId)), "Value", "Text");
                    TempData["ModelID"] = item.ModelId;
                }
                if (Problem != null && Problem.Count > 0)
                {
                    result.ChildtableDataProblem = Problem;
                }
                else
                {
                    new List<GetProblem_Child_Order_problem>();
                }
                result.Problem = null;



                if (result.StatusName == "Pending")
                {
                    result.CurrentStatus = "Request received";
                }
                result.ProblemObservedList = new SelectList(dropdown.BindProblemObserved(), "Value", "Text");
                result.SpareTypeList = new SelectList(dropdown.BindMstDeviceProblemAbhishek(), "Value", "Text");
                result.SpareNameList = new SelectList(Enumerable.Empty<SelectListItem>());
                result.SelectTrcList = new SelectList(dropdown.BindTrc(), "Value", "Text");
                result.CourierNameList = new SelectList(dropdown.BindCourier(), "Value", "Text");
                result.CallStatusList = new SelectList(dropdown.BindCall_Status_Master(), "Value", "Text");
                result.ServiceProviderNameList = new SelectList(await CommonModel.GetServiceProviders (), "Name", "Text");
                result.ProblemList = new SelectList(dropdown.BindMstDeviceProblem(), "Value", "Text");
                result.WSList = new SelectList(dropdown.BindWarrantyDropdown(Convert.ToInt32( TempData["ModelID"])), "Value", "Text");
                return View(result);
            }
        }
    
        [HttpPost]
        public ActionResult POOWRRForm(ReceiveMaterials m)
        {
            try
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    if (m.TableData != null)
                    {
                        foreach (var item in m.TableData)
                        {
                            //item.spareCcNo
                            var result = con.Query<int>("Insert_Maintain_SpareTable_Data",
                                   new
                                   {
                                       CC_NO = item.TablespaceCC_NOField,                                       
                                       SpareType = item.TablespareTypeField,
                                       SpareCode=item.TablespareCodeField,
                                       SpareName = item.TablespareNameField,
                                       SpareQuantity = item.TablespareQuantityField,
                                       Price = item.TablesparePriceField,                                       
                                       Total =item.TablespareTotalField
                                   }

                         , commandType: CommandType.StoredProcedure).FirstOrDefault();
                        }
                    }
                    if (m.CcNo != null)
                    {
                        
                        var value = "";
                        var finalValue = "";
                        if (m.PrblmObsrvdPoowrr != null)
                        {
                            var problem = m.PrblmObsrvdPoowrr.Length;
                            for (var i = 0; i <= problem - 1; i++)
                            {
                                var Data = m.ProblemFound[i].FirstOrDefault();
                                value = Data + ",";
                                finalValue = finalValue + value;
                            }
                        }
                     
                        var result1 = con.Query<int>("InsertDataInto_Repair_Request_Details", 
                            new {
                                CC_NO=m.CcNo,
                                m.ServiceCharge,
                                m.SpareCost,
                                m.EstimatedCost,
                                EstimatedCostApproved=m.IsEstimatedCostApproved,
                                CustomerAgreeWipeData=m.wipedevicedata,
                                Current_Status='0',
                                CallBackDate=m.CallBackDatetime,
                                Email_SMS_To_Customer=m.EMail_SMS,
                                TrcId=m.SelectTrc,
                                CourierId=m.CourierName,
                                m.Remarks,
                                TrcAddress=m.TRCFullAddr,
                                m.CourierContact,
                                m.CourierActive,
                                ReversePickUpDate=m.ReversePickupDate,
                                m.EngineerVisitDate,
                                CallRequestReject=m.Reject,
                                ServiceProviderNameId=new Guid(m.ServiceProviderName),
                                m.PhysicalDamage,
                                DeviceWarrantyVoid=m.WarrantyVoid,
                                ProblemObserved=finalValue,
                                m.Customer_Name,
                                m.Mobile_No,
                                m.Email_Id,
                                m.Cust_Add,
                                m.Cust_City,
                                m.Cust_State,
                                m.IMEI1,
                                m.IMEI2,
                                SerialNumber=m.Serial_No,
                                m.AltNo,
                                Pincode= m.Pincode,
                                m.WS,
                                m.CallStatus,
                                m.CallRequestRejectReason,
                                SchedulePickupDate=m.SchedulePickup
                            }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        var response = new ResponseModel {IsSuccess=false };
                        if (result1 == 1)
                        {
                            response.IsSuccess = true;
                            response.Response = "Submitted Successfully";
                        }
                        else
                        {
                            response.Response = "Something went wrong";
                        }

                        TempData["response"] = response;
                    }                  
                }
                return RedirectToAction("POOWRR", "CustomerSupport");
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public ActionResult TablePOOWRR()
        {
            using (var con = new SqlConnection(_connectionString))
           {                             
                var result = con.Query<AllData>("GetTableDataForOOW",new { }, commandType: CommandType.StoredProcedure).ToList();
              // ViewBag.DropdownStatus = new SelectList(dropdown.BindStatusMaster(), "Value", "Text");
        
                foreach (var item in result)
                {
                    var finalValue = "";
                    var Problem1 = con.Query<GetProblem_Child_Order_problem>("GetProblem_From_Child_Order_problem",
                    new { CC_NO = item.CC_NO }, commandType: CommandType.StoredProcedure).ToList();
                    foreach (var item1 in Problem1)
                    {
                        var result1 = con.Query<string>("select Problem from mstdeviceproblem WHERE ProblemId =@ProblemId ", new { @ProblemId = item1.ProblemId }, commandType: CommandType.Text).FirstOrDefault();
                        
                        finalValue = finalValue + result1 + " , ";
                        
                    }
                    finalValue = finalValue.Trim().TrimEnd(',');
                    item.Problem = finalValue;
                }
                
               
                return View(result);
            }

        }
        public ActionResult PIWRR()
        {
            ViewBag.ServiceProviderName = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.SpareType = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.SpareName = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.ProblemObserved = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.SelectTrc = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.CourierName = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.CallStatus = new SelectList(Enumerable.Empty<SelectListItem>());
        //    AllData ad = new AllData
        //    {
        //       ServiceProviderNameList = new SelectList(Enumerable.Empty<SelectListItem>()),
        //       SpareTypeList = new SelectList(Enumerable.Empty<SelectListItem>()),
        //        SpareNameList = new SelectList(Enumerable.Empty<SelectListItem>()),
        //       ProblemObservedList = new SelectList(Enumerable.Empty<SelectListItem>()),
        //       SelectTrcList = new SelectList(Enumerable.Empty<SelectListItem>()),
        //        CourierNameList = new SelectList(Enumerable.Empty<SelectListItem>()),
        //        CallStatusList = new SelectList(Enumerable.Empty<SelectListItem>()),
        //};

            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();            
            }
            return View();
        }
        public ActionResult PIWRRFindByCcNo()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PIWRRFindByCcNo(string CcNO)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<AllData>("GetDataByCCNO",
                    new { CC_NO = CcNO }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                var problem = "";
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
                var Problem1 = con.Query<GetProblem_Child_Order_problem>("GetProblem_From_Child_Order_problem",
                 new { CC_NO= CcNO }, commandType: CommandType.StoredProcedure).ToList();
                var finalValue = "";
                foreach (var item in Problem1)
                {
                    var result1 = con.Query<string>("select Problem from mstdeviceproblem WHERE ProblemId =@ProblemId ", new { @ProblemId = item.ProblemId }, commandType: CommandType.Text).FirstOrDefault();

                    finalValue = string.Join(",", result1);
                }
                result.Problem = finalValue;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult EditPIWRR()
        {
            /* ViewBag.ServiceProviderName = new SelectList(dropdown.BindServiceProvider(), "Value", "Text");
             ViewBag.SpareType = new SelectList(dropdown.BindSpareType(), "Value", "Text");
             ViewBag.SpareName = new SelectList(Enumerable.Empty<SelectListItem>());
             ViewBag.ProblemObserved = new SelectList(dropdown.BindProblemObserved(), "Value", "Text");
             ViewBag.SelectTrc = new SelectList(dropdown.BindTrc(), "Value", "Text");
             ViewBag.CourierName = new SelectList(dropdown.BindCourier(), "Value", "Text");
             ViewBag.CallStatus = new SelectList(dropdown.BindStatusMaster(), "Value", "Text");*/
            //AllData ad = new AllData
            
                var pwirr = new ReceiveMaterials();
            pwirr.ServiceProviderNameList = new SelectList(dropdown.BindServiceProvider(), "Value", "Text");
            pwirr.SpareTypeList = new SelectList(dropdown.BindSpareType(), "Value", "Text");
            pwirr.SpareNameList = new SelectList(Enumerable.Empty<SelectListItem>());
            pwirr.ProblemObservedList = new SelectList(dropdown.BindProblemObserved(), "Value", "Text");
            pwirr.SelectTrcList = new SelectList(dropdown.BindTrc(), "Value", "Text");
            pwirr.CourierNameList = new SelectList(dropdown.BindCourier(), "Value", "Text");
            pwirr.CallStatusList = new SelectList(dropdown.BindStatusMaster(), "Value", "Text");

        

            return PartialView(pwirr);
        }
        [HttpPost]
        public ActionResult EditPIWRR(PWIRRModel m)
        {            
            try
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    if (m.test != null)
                    {
                        foreach (var item in m.test)
                        {
                            //item.spareCcNo
                            var result = con.Query<int>("Add_Update_QuantitySpareTypeName",
                                   new
                                   {
                                       CC_NO = item.spareCcNoField,
                                       JobNumber = item.spaceJobNumberField,
                                       SpareType = item.spareTypeField,
                                       SpareName = item.spareNameField,
                                       Price = item.sparePriceField,
                                       Quantity = item.spareQuantityField,
                                       UnitPrice = "",
                                       Total = ""
                                   }

                         , commandType: CommandType.StoredProcedure).FirstOrDefault();
                        }

                    }
                    var result1 = con.Query<int>("Add_Update_Pending_IW_Repair_Requests",
                      new
                      {
                          CC_No = m.CcNo,
                          m.CallStatus,
                          callBackDate = m.CallBackDatetime,
                          m.CallRequestReject,
                          m.Remarks,
                          TrcId = m.SelectTrc,
                          CourierNameId = m.CourierName,
                          m.CourierActive,
                          ReversePickUpDate = m.ReversePickupDate,
                          m.EngineerVisitDate,
                          TrcAddress = m.TRCFullAddr,
                          m.CourierContact,
                      }
                      , commandType: CommandType.StoredProcedure).FirstOrDefault();
                    var value = "";
                    var finalValue = "";
                    var problem = m.ProblemObserved.Length;
                    for (var i = 0; i <= problem - 1; i++)
                    {
                        var Data = m.ProblemObserved[i].FirstOrDefault();
                        value = Data + ",";
                        finalValue = finalValue + value;
                    }
                    var result2 = con.Query<int>("Add_Update_Pending_IW_Repair_Requests1",
                     new
                     {
                         CC_NO=m.CcNo,
                            JobNumber=m.CustomerId,
                            m.WarrantyStatus,
                            m.WarrantyExpiryDate,
                            PhysicallyDamaged=m.PhysicalDamage,
                            DeviceWarrantyVoid=m.DeviceWarranty,
                            ProblemObserved=finalValue,
                            ServiceCharge=m.BillServiceCharge,
                            SpareCost=m.BillSpareCost,
                            EstimatedCost=m.BillEstimatedCost,
                            WipeDeviceData=m.wipedevicedata,
                            EstimatedCostApproved=m.IsEstimatedCostApproved,
                     }
                     , commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result2 == 1)
                    {
                        TempData["Message"] = "Updated Successfully";
                    }
                    else if (result2 == 2)
                    {
                        TempData["Message"] = "Added Successfully";
                    }
                    else
                    {
                        TempData["Message"] = "Something Went Wrong";
                    }                   
                }
                return RedirectToAction("PIWRR","CustomerSupport");
            }
            catch (Exception e)
            {
                throw e;
            }
           // return View();
        }
        public ActionResult TablePIWRR()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<AllData>("GetTableDataForIW",
                   new { }, commandType: CommandType.StoredProcedure).ToList();
                return View(result);
            }
        }
        public JsonResult GetTrcFullAddress(int TRCID)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<GetTrcAddressInfo>("select [ADDRESS],LOCALITY,NEAR_BY_LOCATION,PIN_CODE from msttrc WHERE TRC_ID=@TRC_ID",
                    new { TRC_ID = TRCID }, commandType: CommandType.Text).FirstOrDefault();               
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetCourierInformation(int CourierId)
        {

            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<GetCourierInfoModel>("SELECT MobileNumber,UploadedCourierFile,IsActive,BikeNumber,BikeMakeandModel from Courier_Master WHERE CourierId=@CourierId",
                    new { CourierId = CourierId }, commandType: CommandType.Text).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult PRCC()

        {
           {
                var receiveMaterial = new ReceiveMaterials();
                //var receiveMaterial = new PRCCModel();
                receiveMaterial.ReceivedDeviceList = new SelectList(Enumerable.Empty<SelectListItem>());
                receiveMaterial.RecvdBrandlList = new SelectList(Enumerable.Empty<SelectListItem>());
                receiveMaterial.RecvdModelList = new SelectList(Enumerable.Empty<SelectListItem>());
                receiveMaterial.Engg_NameList = new SelectList(Enumerable.Empty<SelectListItem>());
                receiveMaterial.SpareTypeList = new SelectList(Enumerable.Empty<SelectListItem>());
                receiveMaterial.ProblemFoundList = new SelectList(Enumerable.Empty<SelectListItem>());

            };

            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();

            }
            return View();
        }
        public ActionResult FindPRCC()
        {
            return View();
        }
        [HttpPost]
        public ActionResult FindPRCC(string CcNO)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<AllData>("GetDataByCCNO",
                    new { CC_NO = CcNO }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                var problem = "";
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
                var Problem1 = con.Query<GetProblem_Child_Order_problem>("GetProblem_From_Child_Order_problem",
                 new { CC_NO = CcNO }, commandType: CommandType.StoredProcedure).ToList();
                var finalValue = "";
                foreach (var item in Problem1)
                {
                    var result1 = con.Query<string>("select Problem from mstdeviceproblem WHERE ProblemId =@ProblemId ", new { @ProblemId = item.ProblemId }, commandType: CommandType.Text).FirstOrDefault();

                    finalValue = string.Join(",", result1);
                }
                result.Problem = finalValue;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult PRCCForm()
        {
          
            var ReceiveMaterial = new ReceiveMaterials();
            ReceiveMaterial.ReceivedDeviceList = new SelectList(dropdown.BindCategory(), "Value", "Text");
            ReceiveMaterial.RecvdBrandlList = new SelectList(dropdown.BindBrand(), "Value", "Text");
            ReceiveMaterial.RecvdModelList = new SelectList(dropdown.BindProduct(), "Value", "Text");
            ReceiveMaterial.Engg_NameList = new SelectList(dropdown.BindEngineer(), "Value", "Text");
            ReceiveMaterial.SpareTypeList = new SelectList(dropdown.BindSpareType(), "Value", "Text");
            ReceiveMaterial.SpareNameList = new SelectList(Enumerable.Empty<SelectListItem>());
            ReceiveMaterial.ProblemFoundList = new SelectList(dropdown.BindProblemObserved(), "Value", "Text"); 



            return PartialView(ReceiveMaterial);
        }
        [HttpPost]
        public ActionResult PRCCForm(ReceiveMaterials m)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                if (m.TableData != null)
                {
                    foreach (var item in m.TableData)
                    {
                        var resul2 = con.Query<int>("InsertTableDataByPRCC",
                        new
                        {
                            CC_NO = item.TablespaceCC_NOField,                           
                            SpareType = item.TablespareTypeField,
                            SpareCode=item.TablespareCodeField,
                            SpareName = item.TablespareNameField,
                            Price = item.TablesparePriceField,
                            SpareQuantity = item.TablespareQuantityField,                           
                            Total = item.TablespareTotalField,
                            
                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    }

                }

                var result1 = con.Query<int>("InsertDataByPRCC",
                    new {
                        m.CC_NO,                   
                        m.Prcc_Is_Repair_Cost_Approved,
                        m.CallBackDatetime,
                        m.Prcc_EMail_SMS,
                        m.Prcc_AdvancePaymentAmount,
                        m.Prcc_SendCompanyBankAccountDetails,
                        m.Prcc_IsCustomerwantstopayAdvance,
                        m.Prcc_CustomerSupportRemarks
                    }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                if (result1 == 1)
                {
                    TempData["Message"] = "Updated Successfully";
                }
                else
                {
                    TempData["Message"] = "Something Went Wrong";
                }
               
            }
             return RedirectToAction("PRCC","CustomerSupport");
        }
        public ActionResult TablePRCC()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<AllData>("GetTableDataForAllPages",
                   new { }, commandType: CommandType.StoredProcedure).ToList().Take(10);
                return View(result);
            }

        }
        //[HttpPost]
        //public ActionResult TablePRCC()
        //{
        //    int start = Convert.ToInt32(Request["start"]);
        //    int length = Convert.ToInt32(Request["length"]);
        //    string searchValue = Request["search[value]"];
        //    string sortColumnName =Request["columns["+Request["order[0][column]"] +"][name]"];
        //    string sortDirection = Request["order[0][dir]"];
        //    using (var con = new SqlConnection(_connectionString))
        //    {
        //        var result = con.Query<PRCCModel>("GetTableDataForAllPages",
        //           new { }, commandType: CommandType.StoredProcedure).ToList();

        //        //if (!string.IsNullOrEmpty(searchValue))
        //        //{
        //        //    result=result.Where(x=>x.)
        //        //}
        //        result= result.OrderBy(sortColumnName + " " +sortDirection).ToList<PRCCModel>();

        //        result=result.Skip(start).Take(length).ToList<PRCCModel>();

        //        return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        //    }

        //}
        public ActionResult RPCAP()
        {
            /* ViewBag.ReceivedDevice = new SelectList(Enumerable.Empty<SelectListItem>());
             ViewBag.RecvdBrand = new SelectList(Enumerable.Empty<SelectListItem>());
             ViewBag.RecvdModel = new SelectList(Enumerable.Empty<SelectListItem>());
             ViewBag.Engg_Name = new SelectList(Enumerable.Empty<SelectListItem>());
             ViewBag.ReceivedDevice = new SelectList(Enumerable.Empty<SelectListItem>());
             ViewBag.SpareType = new SelectList(Enumerable.Empty<SelectListItem>());
             ViewBag.ProblemFound = new SelectList(Enumerable.Empty<SelectListItem>());*/

            //AllData ad = new AllData
            {
                var ReceiveMaterial = new ReceiveMaterials();
                ReceiveMaterial.ReceivedDeviceList = new SelectList(Enumerable.Empty<SelectListItem>());
                ReceiveMaterial.RecvdBrandlList = new SelectList(Enumerable.Empty<SelectListItem>());
                ReceiveMaterial.RecvdModelList = new SelectList(Enumerable.Empty<SelectListItem>());
                ReceiveMaterial.Engg_NameList = new SelectList(Enumerable.Empty<SelectListItem>());
                ReceiveMaterial.SpareTypeList = new SelectList(Enumerable.Empty<SelectListItem>());
                ReceiveMaterial.SpareNameList = new SelectList(Enumerable.Empty<SelectListItem>());
                ReceiveMaterial.ProblemFoundList = new SelectList(Enumerable.Empty<SelectListItem>());

            };

            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();

            }
            return View();
        }
        public ActionResult FindRPCAP()
        {
            return View();
        }
        public ActionResult RPCAPForm()
        {
                var ReceiveMaterials = new ReceiveMaterials();
            ReceiveMaterials.ReceivedDeviceList = new SelectList(dropdown.BindCategory(), "Value", "Text");
            ReceiveMaterials.RecvdBrandlList = new SelectList(dropdown.BindBrand(), "Value", "Text");
            ReceiveMaterials.RecvdModelList = new SelectList(dropdown.BindProduct(), "Value", "Text");
            ReceiveMaterials.Engg_NameList = new SelectList(dropdown.BindEngineer(), "Value", "Text");
            ReceiveMaterials.SpareTypeList = new SelectList(dropdown.BindSpareType(), "Value", "Text");
            ReceiveMaterials.SpareNameList = new SelectList(Enumerable.Empty<SelectListItem>());
            ReceiveMaterials.ProblemFoundList = new SelectList(dropdown.BindProblemObserved(), "Value", "Text");

            
            return PartialView(ReceiveMaterials);
        }
        [HttpPost]
        public ActionResult RPCAPForm(ReceiveMaterials m)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                if (ModelState.IsValid)
                {
                    var result1 = con.Query<int>("Insert_RPCAP_In_Repair_Request_Details",
                   new
                   {
                       m.CC_NO,
                       RPCAP_PaymentMode=m.PaymentMode,
                       RPCAP_TransNumber=m.TransNumber,
                       RPCAP_AccountHolderNameonCheque=m.AccountHolderNameonCheque,
                       RPCAP_CashDepositedByNameonSlip=m.CashDepositedByNameonSlip,
                       RPCAP_TransDateTime=m.TransDateTime,
                       RPCAP_ChequeNumber=m.ChequeNumber,
                       RPCAP_CashDepositedByMobileNumberonSlip=m.CashDepositedByMobileNumberonSlip
                   }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result1 == 1)
                    {
                        TempData["Message"] = "Updated Successfully";
                    }
                    else
                    {
                        TempData["Message"] = "Something Went Wrong";
                    }

                }
            }
            return RedirectToAction("RPCAP", "CustomerSupport");
        }
        public ActionResult TableRPCAP()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<AllData>("GetTableDataForAllPages",
                   new { }, commandType: CommandType.StoredProcedure).ToList().Take(10);
                return View(result);
            }

        }

        public ActionResult POOWRR_Test()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();

            }
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<AllData>("GetTableDataForOOW",
                     commandType: CommandType.StoredProcedure).ToList();

                return View(result);
            }
        }
        public ActionResult CJS()
        {
            //AllData ad = new AllData
            //{
            //    ReceivedDeviceList = new SelectList(Enumerable.Empty<SelectListItem>()),
            //    RecvdBrandlList = new SelectList(Enumerable.Empty<SelectListItem>()),
            //    RecvdModelList = new SelectList(Enumerable.Empty<SelectListItem>()),
            //    Engg_NameList = new SelectList(Enumerable.Empty<SelectListItem>()),
            //    SpareTypeList = new SelectList(Enumerable.Empty<SelectListItem>()),                
            //    ProblemFoundList = new SelectList(Enumerable.Empty<SelectListItem>())

            //};
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();

            }
            return View();
        }
        public ActionResult FindCJS()
        {
            return View();
        }
        [HttpPost]
        public JsonResult FindCJS(string CcNO)
        {
            using (var con = new SqlConnection(_connectionString))
            {

                var result = con.Query<CJSModel>("GetDataByCCNO",
               new { CC_NO = CcNO }, commandType: CommandType.StoredProcedure).FirstOrDefault();


                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult CJSForm()
        {
                var receivematerial = new ReceiveMaterials();
                receivematerial.ReceivedDeviceList = new SelectList(dropdown.BindCategory(), "Value", "Text");
                receivematerial.RecvdBrandlList = new SelectList(dropdown.BindBrand(), "Value", "Text");
                receivematerial.RecvdModelList = new SelectList(dropdown.BindProduct(), "Value", "Text");
                receivematerial.Engg_NameList = new SelectList(dropdown.BindEngineer(), "Value", "Text");
                receivematerial.SpareTypeList = new SelectList(dropdown.BindSpareType(), "Value", "Text");
                receivematerial.SpareNameList = new SelectList(Enumerable.Empty<SelectListItem>());
                receivematerial.ProblemFoundList = new SelectList(dropdown.BindProblemObserved(), "Value", "Text");
                return PartialView(receivematerial);
                 
        }
        [HttpPost]
        public ActionResult CJSForm(ReceiveMaterials m)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                if (ModelState.IsValid)
                {
                    var result1 = con.Query<int>("Insert_RPCAP_In_Repair_Request_Details",
                   new
                   {
                       m.CC_NO,
                       RPCAP_PaymentMode = m.PaymentMode,
                       RPCAP_TransNumber = m.TransNumber,
                       RPCAP_AccountHolderNameonCheque = m.AccountHolderNameonCheque,
                       RPCAP_CashDepositedByNameonSlip = m.CashDepositedByNameonSlip,
                       RPCAP_TransDateTime = m.TransDateTime,
                       RPCAP_ChequeNumber = m.ChequeNumber,
                       RPCAP_CashDepositedByMobileNumberonSlip = m.CashDepositedByMobileNumberonSlip
                   }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result1 == 1)
                    {
                        TempData["Message"] = "Updated Successfully";
                    }
                    else
                    {
                        TempData["Message"] = "Something Went Wrong";
                    }

                }
            }
            return RedirectToAction("RPCAP", "CustomerSupport");
        }
        public ActionResult TableCJS()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<AllData>("GetTableDataForAllPages",
                   new { }, commandType: CommandType.StoredProcedure).ToList().Take(10);
                return View(result);
            }

        }


    }
}