using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using TogoFogo.Models;
using TogoFogo.Permission;

namespace TogoFogo.Controllers
{
  
    public class Trc_ReceiveMaterialsController : Controller
    {
        private readonly string _connectionString =
                   ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        DropdownBindController dropdown = new DropdownBindController();

        // File Save Code
        private string SaveImageFile(HttpPostedFileBase file)
        {
            try
            {
                string path = Server.MapPath("~/Uploaded Images");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var fileFullName = file.FileName;
                var fileExtention = Path.GetExtension(fileFullName);
                var fileName = Path.GetFileNameWithoutExtension(fileFullName);
                var savedFileName = fileName + fileExtention;
                file.SaveAs(Path.Combine(path, savedFileName));
                return savedFileName;
            }
            catch (Exception ex)
            {

                return ViewBag.Message = ex.Message;
            }
        }

        // GET: ReceiveMaterials
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Receive_Materials)]
        public ActionResult Index()
        {

            ViewBag.RecvdBrand = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.RecvdModel=new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.EnggName = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.ReceivedDevice = new SelectList(Enumerable.Empty<SelectListItem>());
            
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
           return View();
        }

        public ActionResult RmForm()
        {
            var alldata = new AllData();
            return PartialView(alldata);
        }
        [HttpPost]
        public ActionResult RmForm(string CcNO)
        {
            new AllData();
            var finalValue = "";
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<AllData>("GetDataByCCNO",new { CC_NO = CcNO }, commandType: CommandType.StoredProcedure).FirstOrDefault();
               
                if (result.ChildtableDataProblem == null)
                {
                    var Problem = con.Query<GetProblem_Child_Order_problem>("GetProblem_From_Child_Order_problem",new { CC_NO=CcNO }, commandType: CommandType.StoredProcedure).ToList();
                    result.ChildtableDataProblem = Problem;
                   
                    foreach (var item in result.ChildtableDataProblem)
                    {
                        var result1 = con.Query<string>("select Problem from mstdeviceproblem WHERE ProblemId =@ProblemId ", new { @ProblemId = item.ProblemId }, commandType: CommandType.Text).FirstOrDefault();

                        finalValue = string.Join(",", result1);
                    }
                   
                }

                result.Problem = finalValue;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Receive_Materials)]
        public ActionResult RM()
        {
            var SessionModel = Session["User"] as SessionModel;
            var receivematerials = new ReceiveMaterials();
            receivematerials.ReceivedDeviceList = new SelectList(dropdown.BindCategory(SessionModel.CompanyId), "Value", "Text");
            receivematerials.RecvdBrandlList = new SelectList(dropdown.BindBrand(SessionModel.CompanyId), "Value", "Text");
            receivematerials.RecvdModelList = new SelectList(Enumerable.Empty<SelectListItem>());
            receivematerials.Engg_NameList = new SelectList(dropdown.BindEngineer(SessionModel.CompanyId), "Value", "Text");
            return PartialView(receivematerials);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Receive_Materials)]
        [HttpPost]
        public ActionResult RM(ReceiveMaterials m)
        {
            try
            {
                if (m.PhotoOfDevice1 != null)
                {
                    m.PhotoOfDevice = SaveImageFile(m.PhotoOfDevice1);
                }
                if (m.DamagedBox1 != null)
                {
                    m.DamagedBox = SaveImageFile(m.DamagedBox1);
                }

                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("InsertReceiveMaterialDetails",
                        new
                        {
                            m.CC_NO,
                            m.BtryRemovable,
                            m.BtryReq,
                            m.BtryReceived,
                            m.IsDiffDeviceRecvd,
                            m.BtryBrandAndModel,
                            m.DeviceTUPC,
                            RecvdDevice=m.ReceivedDevice,
                            m.RecvdBrand,
                            m.RecvdModel,
                            m.RecvdSerialNo,
                            m.RecvdIMEI1,
                            m.RecvdIMEI2,
                            m.IsPhyDamage,
                            m.PhotoOfDevice,
                            m.ReceiveDate,
                            m.ReceiveApprovalNeeded,
                            m.RecvRemarks,
                            m.EnggName,
                            m.RecvApprovalStatus,
                            m.RecvApprovalDate,
                            m.RecvApprovedBy,
                            DamagedBoxImage=m.DamagedBox,
                            ReceivingDocument = m.ReceivingDoc,
                            m.BoxCondition,
                            IsReturnable = m.Returnable,
                            NoOfBoxesReceived = m.NoOfBoxesReceived,
                            CreatedBy =""
                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 1)
                    {
                        TempData["Message"] = "Successfully Added";
                    }
                    else if (result == 2)
                    {
                        TempData["Message"] = "Successfully Updated";
                    }
                    else
                    {
                        TempData["Message"] = "Something Went Wrong";
                    }
                    if (m.ReceiveApprovalNeeded)
                    {
                        return RedirectToAction("Index", "Trc_PFRMA");
                    }
                    else if (!m.ReceiveApprovalNeeded)
                    {
                        return RedirectToAction("Index", "Trc_PFELS");
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                    
                }
            }
            catch (Exception e)
            {

                throw e;
            }
            
        }
        public ActionResult JobSheet(string CC_NO)
        {
            var JobNumber = "";
            var ImagePath = "";
            using (var con = new SqlConnection(_connectionString))
            {
                var res = con.Query<string>("Select JobNumber from ReceiveMaterialDetails where CC_NO=@CC_NO", new { @CC_NO = CC_NO }, commandType: CommandType.Text).FirstOrDefault();
                JobNumber = res;
            }
                using (MemoryStream ms = new MemoryStream())
            {
                //The Image is drawn based on length of Barcode text.
                const int PaddingTop = 15;

                string barcodeText;
                barcodeText = "*" + JobNumber + "*";
                int barcodeFontSize = 12;
                int numericFontSize = 6;
                int width = 400;
                int height = 90;
                Bitmap bmp = new Bitmap(width, height);
                Graphics gfx = Graphics.FromImage(bmp);
                gfx.Clear(Color.White);
                gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                gfx.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                Brush b = Brushes.Black;
                PrivateFontCollection pfc = new PrivateFontCollection();
                pfc.AddFontFile(Server.MapPath("~/Barcode_Font/IDAutomationHC39M.ttf"));
                string fileName = JobNumber;
                string path2 = string.Format("{0}{1}.png", Server.MapPath("~/Barcode_file/"), fileName);
                FontFamily ff = pfc.Families[0];
                System.Drawing.Font f = new System.Drawing.Font(ff, barcodeFontSize);
                SizeF fSize = gfx.MeasureString(barcodeText, f);
                gfx.DrawString(barcodeText, f, b, ((width - fSize.Width) / 2), PaddingTop);
                System.Drawing.Font f2 = new System.Drawing.Font("Arial", numericFontSize);
                SizeF f2Size = gfx.MeasureString(JobNumber, f2);
                gfx.TextRenderingHint = TextRenderingHint.AntiAlias;
                gfx.DrawString(JobNumber, f2, b, ((width - f2Size.Width) / 2), (PaddingTop + fSize.Height));
                ImageFormat format1 = ImageFormat.Png;
                bmp.Save(path2, format1);
                bmp.Dispose();
                //TempData["BarcodeImage"] = fileName + ".png";
                ImagePath= fileName + ".png";

            }
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<AllData>("GetDataByJobNumber1",
                    new { JobNumber }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                if (result != null)
                {
                    result.ImagePath ="~/Barcode_file/"+ ImagePath;
                }
                return View(result);
            }


        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Receive_Materials)]
        public ActionResult TableRM()
        {
            //ReceiveMaterials obj_receivematerials = new ReceiveMaterials();
            using (var con = new SqlConnection(_connectionString))
            {
                //var result = con.Query<AllData>("GetTableDataForRM",
                //   new { }, commandType: CommandType.StoredProcedure).ToList();
                //return View(result);
               var result = con.Query<AllData>("GetTableDataForRM",
                   new { }, commandType: CommandType.StoredProcedure).ToList();
                return View(result);
            }
            

        }
    }
}