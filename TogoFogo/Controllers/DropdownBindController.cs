using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Dapper;
using TogoFogo.Models;
using TogoFogo.Repository;
using ListItem = System.Web.UI.WebControls.ListItem;

namespace TogoFogo.Controllers
{
    public class DropdownBindController : Controller
    {
        private readonly IBank _bankRepo;
        private readonly IContactPerson _ContactRepo;
        private readonly string _connectionString;
        public DropdownBindController()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            _bankRepo = new Bank();
            _ContactRepo = new ContactPerson(); 
        }

           
        // GET: DropdownBind
        public JsonResult BindEmailGateway()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var gateway = con.Query<BindGatewayModel>("select SettingID,SettingName from MstEmailGateway", commandType: CommandType.Text);
                List<ListItem> items = new List<ListItem>();
                items.Add(new ListItem
                {
                    Value = "", //Value Field(ID)
                    Text = "--Select--" //Text Field(Name)
                });
                foreach (var val in gateway)
                {
                    items.Add(new ListItem
                    {
                        Value = val.SettingID.ToString(), //Value Field(ID)
                        Text = val.SettingName //Text Field(Name)
                    });
                }

                return Json(items, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult BindSMSGateway()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var gateway = con.Query<BindGatewayModel>("select SettingID,SettingName from MstSMSGateway", commandType: CommandType.Text);
                List<ListItem> items = new List<ListItem>();
                items.Add(new ListItem
                {
                    Value = "", //Value Field(ID)
                    Text = "--Select--" //Text Field(Name)
                });
                foreach (var val in gateway)
                {
                    items.Add(new ListItem
                    {
                        Value = val.SettingID.ToString(), //Value Field(ID)
                        Text = val.SettingName //Text Field(Name)
                    });
                }
                return Json(items, JsonRequestBehavior.AllowGet);
            }
        }

        public List<ListItem> BindEmailGatewayEdit()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var gateway = con.Query<BindGatewayModel>("select SettingID,SettingName from MstEmailGateway", commandType: CommandType.Text);
                List<ListItem> items = new List<ListItem>();
                items.Add(new ListItem
                {
                    Value = "", //Value Field(ID)
                    Text = "--Select--" //Text Field(Name)
                });
                foreach (var val in gateway)
                {
                    items.Add(new ListItem
                    {
                        Value = val.SettingID.ToString(), //Value Field(ID)
                        Text = val.SettingName //Text Field(Name)
                    });
                }
                return items;
            }
        }
        public List<ListItem> BindSMSGatewayEdit()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var gateway = con.Query<BindGatewayModel>("select SettingID,SettingName from MstSMSGateway", commandType: CommandType.Text);
                List<ListItem> items = new List<ListItem>();
                items.Add(new ListItem
                {
                    Value = "", //Value Field(ID)
                    Text = "--Select--" //Text Field(Name)
                });
                foreach (var val in gateway)
                {
                    items.Add(new ListItem
                    {
                        Value = val.SettingID.ToString(), //Value Field(ID)
                        Text = val.SettingName //Text Field(Name)
                    });
                }
                return items;
            }
        }
        public List<ListItem> BindWarrantyDropdown( int? ModelId)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                List<WarrantyModel> color = con
                    .Query<WarrantyModel>("WarrantyDropdown", new { ModelID= ModelId }, commandType: CommandType.StoredProcedure).ToList();
                List<ListItem> items = new List<ListItem>();

                foreach (var val in color)
                {
                    items.Add(new ListItem
                    {                        
                        Text = val.SNCTwo //Text Field(Name)
                    });
                }

                return items;
            }
        }
        public List<ListItem> BindCall_Status_Master()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                List<GetCallStatusModel> color = con
                    .Query<GetCallStatusModel>("select CallStatusId,CallStatusName from Call_Status_Master", null, commandType: CommandType.Text).ToList();
                List<ListItem> items = new List<ListItem>();

                foreach (var val in color)
                {
                    items.Add(new ListItem
                    {
                        Value = val.CallStatusId.ToString(), //Value Field(ID)
                        Text = val.CallStatusName //Text Field(Name)
                    });
                }

                return items;
            }
        }
        public JsonResult BindProblemDropdownjSON(string value)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                List<BindMstDeviceProblem> color = con
                     .Query<BindMstDeviceProblem>("select ProblemID, Problem from mstdeviceproblem", null, commandType: CommandType.Text).ToList();
                List<ListItem> items = new List<ListItem>();
                items.Add(new ListItem
                {
                    Value = "", //Value Field(ID)
                    Text = "--Select--" //Text Field(Name)
                });
                foreach (var val in color)
                {
                    items.Add(new ListItem
                    {
                        Value = val.ProblemID.ToString(), //Value Field(ID)
                        Text = val.Problem //Text Field(Name)
                    });
                }

                return Json(items, JsonRequestBehavior.AllowGet);
            }
        }
        public List<ListItem> BindProblemDropdown(string value)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                List<BindMstDeviceProblem> color = con
                     .Query<BindMstDeviceProblem>("select ProblemID, Problem from mstdeviceproblem", null, commandType: CommandType.Text).ToList();
                List<ListItem> items = new List<ListItem>();
                items.Add(new ListItem
                {
                    Value = "", //Value Field(ID)
                    Text = "--Select--" //Text Field(Name)
                });
                foreach (var val in color)
                {
                    items.Add(new ListItem
                    {
                        Value = val.ProblemID.ToString(), //Value Field(ID)
                        Text = val.Problem //Text Field(Name)
                    });
                }

                return items;
            }
        }
        public List<ListItem> BindMstDeviceProblemAbhishek()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                List<BindMstDeviceProblem> color = con
                    .Query<BindMstDeviceProblem>("select ProblemID, Problem from mstdeviceproblem", null, commandType: CommandType.Text).ToList();
                List<ListItem> items = new List<ListItem>();

                foreach (var val in color)
                {
                    items.Add(new ListItem
                    {
                        Value = val.ProblemID.ToString(), //Value Field(ID)
                        Text = val.Problem //Text Field(Name)
                    });
                }

                return items;
            }
        }
        public List<ListItem> BindGstHsnCode()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                List<GstHsnCodeDropdown> color = con
                    .Query<GstHsnCodeDropdown>("select SacCodesId,Gst_HSN_Code from MstSacCodes ", null, commandType: CommandType.Text).ToList();
                List<ListItem> items = new List<ListItem>();

                foreach (var val in color)
                {
                    items.Add(new ListItem
                    {
                        Value = val.SacCodesId.ToString(), //Value Field(ID)
                        Text = val.Gst_HSN_Code //Text Field(Name)
                    });
                }

                return items;
            }
        }
        public List<ListItem> BindProductColor()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                List<BindColor> color = con
                    .Query<BindColor>("select ColorId,ColorName from Color_Master where isactive=1", null, commandType: CommandType.Text).ToList();
                List<ListItem> items = new List<ListItem>();
           
                foreach (var val in color)
                {
                    items.Add(new ListItem
                    {
                        Value = val.ColorId.ToString(), //Value Field(ID)
                        Text = val.ColorName //Text Field(Name)
                    });
                }

                return items;
            }
        }
        public List<ListItem> BindMstDeviceProblem()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                List<BindMstDeviceProblem> color = con
                    .Query<BindMstDeviceProblem>("select ProblemID, Problem from mstdeviceproblem", null, commandType: CommandType.Text).ToList();
                List<ListItem> items = new List<ListItem>();
         
                foreach (var val in color)
                {
                    items.Add(new ListItem
                    {
                        Value = val.ProblemID.ToString(), //Value Field(ID)
                        Text = val.Problem //Text Field(Name)
                    });
                }

                return items;
            }
        }
        public List<ListItem> BindUserType()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                List<UserTypeModel> color = con
                    .Query<UserTypeModel>("select UserTypeId,UserType from User_Type", null, commandType: CommandType.Text).ToList();
                List<ListItem> items = new List<ListItem>();

                foreach (var val in color)
                {
                    items.Add(new ListItem
                    {
                        Value = val.UserTypeId.ToString(), //Value Field(ID)
                        Text = val.UserType //Text Field(Name)
                    });
                }

                return items;
            }
        }
        public List<ListItem> BindUserTypeAdmin()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                List<UserTypeModel> color = con
                    .Query<UserTypeModel>("select UserTypeId,UserType from User_Type where not id =2", null, commandType: CommandType.Text).ToList();
                List<ListItem> items = new List<ListItem>();

                foreach (var val in color)
                {
                    items.Add(new ListItem
                    {
                        Value = val.UserTypeId.ToString(), //Value Field(ID)
                        Text = val.UserType //Text Field(Name)
                    });
                }

                return items;
            }
        }
        public List<ListItem> BindUserTypeClient()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                List<UserTypeModel> color = con
                    .Query<UserTypeModel>("select UserTypeId,UserType from User_Type where  id =5", null, commandType: CommandType.Text).ToList();
                List<ListItem> items = new List<ListItem>();

                foreach (var val in color)
                {
                    items.Add(new ListItem
                    {
                        Value = val.UserTypeId.ToString(), //Value Field(ID)
                        Text = val.UserType //Text Field(Name)
                    });
                }

                return items;
            }
        }
        public List<ListItem> BindUserTypeServiceCenter()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                List<UserTypeModel> color = con
                    .Query<UserTypeModel>("select UserTypeId,UserType from User_Type where  id =4", null, commandType: CommandType.Text).ToList();
                List<ListItem> items = new List<ListItem>();

                foreach (var val in color)
                {
                    items.Add(new ListItem
                    {
                        Value = val.UserTypeId.ToString(), //Value Field(ID)
                        Text = val.UserType //Text Field(Name)
                    });
                }

                return items;
            }
        }
        public List<ListItem> BindUserTypeService_Provider()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                List<UserTypeModel> color = con
                    .Query<UserTypeModel>("select UserTypeId,UserType from User_Type where  id in(3,4)", null, commandType: CommandType.Text).ToList();
                List<ListItem> items = new List<ListItem>();

                foreach (var val in color)
                {
                    items.Add(new ListItem
                    {
                        Value = val.UserTypeId.ToString(), //Value Field(ID)
                        Text = val.UserType //Text Field(Name)
                    });
                }

                return items;
            }
        }
        public List<ListItem> BindUserRole()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                List<UserRoleModel> color = con
                    .Query<UserRoleModel>("select RoleId,UserRole from User_Role_Master", null, commandType: CommandType.Text).ToList();
                List<ListItem> items = new List<ListItem>();

                foreach (var val in color)
                {
                    items.Add(new ListItem
                    {
                        Value = val.RoleId.ToString(), //Value Field(ID)
                        Text = val.UserRole //Text Field(Name)
                    });
                }

                return items;
            }
        }
        public List<ListItem> BindUserRoleBYUserType(string userId)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                List<UserRoleModel> color = con
                    .Query<UserRoleModel>("select RoleId,UserRole from User_Role_master where UserTypeId=@UserTypeId", new { @UserTypeId=userId }, commandType: CommandType.Text).ToList();
                List<ListItem> items = new List<ListItem>();

                foreach (var val in color)
                {
                    items.Add(new ListItem
                    {
                        Value = val.RoleId.ToString(), //Value Field(ID)
                        Text = val.UserRole //Text Field(Name)
                    });
                }

                return items;
            }
        }
        public JsonResult BindSubRoleJson(string value)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var state = con.Query<UserRoleModel>("Select RoleId,UserRole from User_Role_Master  where userTypeId=@userTypeId  ", new { @userTypeId = value },
                    commandType: CommandType.Text);
                List<ListItem> items = new List<ListItem>();
                items.Add(new ListItem
                {
                    Value = "", //Value Field(ID)
                    Text = "Select" //Text Field(Name)
                });
                foreach (var val in state)
                {
                    items.Add(new ListItem
                    {
                        Value = val.RoleId.ToString(), //Value Field(ID)
                        Text = val.UserRole //Text Field(Name)
                    });
                }

                return Json(items, JsonRequestBehavior.AllowGet);
            }
        }
        public List<ListItem> BindModelName()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                List<BindDeviceModel> company = con
                    .Query<BindDeviceModel>("Select ProductId,ProductName from MstProduct",
                        null, commandType: CommandType.Text).ToList();
                List<ListItem> items = new List<ListItem>();

                foreach (var val in company)
                {
                    items.Add(new ListItem
                    {
                        Value = val.ProductId.ToString(), //Value Field(ID)
                        Text = val.ProductName //Text Field(Name)
                    });
                }

                return items;
            }
        }
        public List<ListItem> BindQC()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                List<BindQCModel> company = con
                    .Query<BindQCModel>("SELECT QCId,QCProblem from mst_QC",
                        null, commandType: CommandType.Text).ToList();
                List<ListItem> items = new List<ListItem>();
      
                foreach (var val in company)
                {
                    items.Add(new ListItem
                    {
                        Value = val.QCId, //Value Field(ID)
                        Text = val.QCProblem //Text Field(Name)
                    });
                }

                return items;
            }
        }
        public List<ListItem> BindServiceProvider()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                List<BindServiceProviderModel> company = con
                    .Query<BindServiceProviderModel>("SELECT ProviderId,ProviderName from MstServiceProvider",
                        null, commandType: CommandType.Text).ToList();
                List<ListItem> items = new List<ListItem>();
                items.Add(new ListItem
                {
                    Value = "", //Value Field(ID)
                    Text = "Select" //Text Field(Name)
                });
                foreach (var val in company)
                {
                    items.Add(new ListItem
                    {
                        Value = val.ProviderId, //Value Field(ID)
                        Text = val.ProviderName //Text Field(Name)
                    });
                }

                return items;
            }
        }

        public List<ListItem> BindCallStatusPOOWRR()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                List<Status_MasterModel> company = con
                    .Query<Status_MasterModel>("SELECT * from Status_Master WHERE StatusId IN(13,14,0,9,15)",
                        null, commandType: CommandType.Text).ToList();
                List<ListItem> items = new List<ListItem>();
                items.Add(new ListItem
                {
                    Value = "", //Value Field(ID)
                    Text = "Select" //Text Field(Name)
                });
                foreach (var val in company)
                {
                    items.Add(new ListItem
                    {
                        Value = val.StatusId, //Value Field(ID)
                        Text = val.StatusName //Text Field(Name)
                    });
                }

                return items;
            }
        }
        public List<ListItem> BindCallStatus()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                List<Status_MasterModel> company = con
                    .Query<Status_MasterModel>("SELECT * from Status_Master WHERE StatusId IN(10,11,12)",
                        null, commandType: CommandType.Text).ToList();
                List<ListItem> items = new List<ListItem>();
                items.Add(new ListItem
                {
                    Value = "", //Value Field(ID)
                    Text = "Select" //Text Field(Name)
                });
                foreach (var val in company)
                {
                    items.Add(new ListItem
                    {
                        Value = val.StatusId, //Value Field(ID)
                        Text = val.StatusName //Text Field(Name)
                    });
                }

                return items;
            }
        }
        public List<ListItem> BindStatusMaster()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                List<Status_MasterModel> company = con
                    .Query<Status_MasterModel>("SELECT StatusId,StatusName FROM Status_Master",
                        null, commandType: CommandType.Text).ToList();
                List<ListItem> items = new List<ListItem>();
                items.Add(new ListItem
                {
                    Value = "", //Value Field(ID)
                    Text = "Select" //Text Field(Name)
                });
                foreach (var val in company)
                {
                    items.Add(new ListItem
                    {
                        Value = val.StatusId, //Value Field(ID)
                        Text = val.StatusName //Text Field(Name)
                    });
                }

                return items;
            }
        }
        public List<ListItem> BindBrand()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                List<BindDropdownModel> company = con
                    .Query<BindDropdownModel>("SELECT DISTINCT BrandId, BrandName FROM MstBrand  where isActive=1  ORDER BY BrandName",
                        null, commandType: CommandType.Text).ToList();
                List<ListItem> items = new List<ListItem>();
                items.Add(new ListItem
                {
                    Value = "", //Value Field(ID)
                    Text = "Select" //Text Field(Name)
                });
                foreach (var val in company)
                {
                    items.Add(new ListItem
                    {
                        Value = val.BrandId, //Value Field(ID)
                        Text = val.BrandName //Text Field(Name)
                    });
                }

                return items;
            }
        }
        public List<ListItem> BindCategorySelectpicker()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                List<BindDropdownModel> company = con
                    .Query<BindDropdownModel>(
                        "SELECT DISTINCT CatName,CatId FROM MstCategory ORDER BY CatName", null,
                        commandType: CommandType.Text).ToList();
                List<ListItem> items = new List<ListItem>();
        
                foreach (var val in company)
                {
                    items.Add(new ListItem
                    {
                        Value = val.CatId, //Value Field(ID)
                        Text = val.CatName //Text Field(Name)
                    });
                }

                return items;
            }
        }
        public List<ListItem> BindCategory()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                List<BindDropdownModel> company = con
                    .Query<BindDropdownModel>(
                        "SELECT DISTINCT CatName,CatId FROM MstCategory where isactive=1 ORDER BY CatName", null,
                        commandType: CommandType.Text).ToList();
                List<ListItem> items = new List<ListItem>();
                /*items.Add(new ListItem
                {
                    Value = "", //Value Field(ID)
                    Text = "Select Category " //Text Field(Name)
                  
                });*/
                foreach (var val in company)
                {
                    items.Add(new ListItem
                    {
                        Value = val.CatId, //Value Field(ID)
                        Text = val.CatName //Text Field(Name)
                    });
                }

                return items;
            }
        }

        public List<ListItem> BindSubCategory()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                List<SubCategoryModel> company = con
                    .Query<SubCategoryModel>(
                        "SELECT SubCatId,SubCatName from MstSubCategory ORDER BY SubCatName", null,
                        commandType: CommandType.Text).ToList();
                List<ListItem> items = new List<ListItem>();
                items.Add(new ListItem
                {
                    Value = "", //Value Field(ID)
                    Text = "Select Sub Category " //Text Field(Name)
                });
                foreach (var val in company)
                {
                    items.Add(new ListItem
                    {
                        Value = val.SubCatId, //Value Field(ID)
                        Text = val.SubCatName //Text Field(Name)
                    });
                }

                return items;
            }
        }
        public List<ListItem> BindCountry()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                List<CountryModel> company = con
                    .Query<CountryModel>(
                        "select Cnty_ID,Cnty_Name from MstCountry", null,
                        commandType: CommandType.Text).ToList();
                List<ListItem> items = new List<ListItem>();
                items.Add(new ListItem
                {
                    Value = "", //Value Field(ID)
                    Text = "Select Country " //Text Field(Name)
                });
                foreach (var val in company)
                {
                    items.Add(new ListItem
                    {
                        Value = val.Cnty_ID, //Value Field(ID)
                        Text = val.Cnty_Name //Text Field(Name)
                    });
                }
        
                
                return items;
            }
        }
        public List<ListItem> BindState()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                List<BindDropdown> company = con
                    .Query<BindDropdown>(
                    "select St_ID,St_Name from mststate", new {},
                        commandType: CommandType.Text).ToList();
                List<ListItem> items = new List<ListItem>();
                items.Add(new ListItem
                {
                    Value = "", //Value Field(ID)
                    Text = "Select State " //Text Field(Name)
                });
                foreach (var val in company)
                {
                    items.Add(new ListItem
                    {
                        Value = val.St_ID.ToString(), //Value Field(ID)
                        Text = val.St_Name //Text Field(Name)
                    });
                }

                return items;
            }
        }
        public List<ListItem> MenuMaster()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                List<MenuMasterModel> company = con
                    .Query<MenuMasterModel>("select MenuCap_ID, Menu_Name from menuTable where ParentMenuId=0",
                        null, commandType: CommandType.Text).ToList();
                List<ListItem> items = new List<ListItem>();
                items.Add(new ListItem
                {
                    Value = "", //Value Field(ID)
                    Text = "Select" //Text Field(Name)
                });
                foreach (var val in company)
                {
                    items.Add(new ListItem
                    {
                        Value = val.MenuCapId.ToString(), //Value Field(ID)
                        Text = val.Menu_Name //Text Field(Name)
                    });
                }

                return items;
            }
        }
        public List<ListItem> BindTrc()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                List<BindTrcModel> company = con
                    .Query<BindTrcModel>("select TRC_ID,TRC_NAME from msttrc",
                        null, commandType: CommandType.Text).ToList();
                List<ListItem> items = new List<ListItem>();
                items.Add(new ListItem
                {
                    Value = "", //Value Field(ID)
                    Text = "Select" //Text Field(Name)
                });
                foreach (var val in company)
                {
                    items.Add(new ListItem
                    {
                        Value = val.TRC_Id.ToString(), //Value Field(ID)
                        Text = val.TRC_NAME //Text Field(Name)
                    });
                }

                return items;
            }
        }
        public List<ListItem> BindGst()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                List<BindGst> company = con
                    .Query<BindGst>("SELECT GstCategoryId,GSTCategory  from MstGstCategory",
                        null, commandType: CommandType.Text).ToList();
                List<ListItem> items = new List<ListItem>();
                items.Add(new ListItem
                {
                    Value = "", //Value Field(ID)
                    Text = "Select" //Text Field(Name)
                });
                foreach (var val in company)
                {
                    items.Add(new ListItem
                    {
                        Value = val.GstCategoryId.ToString(), //Value Field(ID)
                        Text = val.GSTCategory //Text Field(Name)
                    });
                }

                return items;
            }
        }
        public List<ListItem> BindProblemObserved()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                List<BindProblemObserved> company = con
                    .Query<BindProblemObserved>("select ProblemId,ProblemObserved from MstProblemObserved",
                        null, commandType: CommandType.Text).ToList();
                List<ListItem> items = new List<ListItem>();
                //items.Add(new ListItem
                //{
                //    Value = "", //Value Field(ID)
                //    Text = "" //Text Field(Name)
                //});
                foreach (var val in company)
                {
                    items.Add(new ListItem
                    {
                        Value = val.ProblemId.ToString(), //Value Field(ID)
                        Text = val.ProblemObserved //Text Field(Name)
                    });
                }

                return items;
            }
        }
        public JsonResult BindModelMulJson(string value)
        {
            List<ListItem> items = new List<ListItem>();
            using (var con = new SqlConnection(_connectionString))
            {
                string[] ar = value.Split(',');

               
                foreach (var it in ar)
                {
                    
                     var state = con.Query<BindDeviceModel>("GetProductByBrand", new { brand = it },
                    commandType: CommandType.StoredProcedure);
                       
                        foreach (var val in state)
                        {
                            items.Add(new ListItem
                            {
                                Value = val.ProductId.ToString(), //Value Field(ID)
                                Text = val.ProductName //Text Field(Name)
                            });
                        }
                    
                }

               
            }
            return Json(items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BindStateJson(string value)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var state = con.Query<BindDropdown>("Get_StateName", new {cntryid = value},
                    commandType: CommandType.StoredProcedure);
                List<ListItem> items = new List<ListItem>();
                items.Add(new ListItem
                {
                    Value = "", //Value Field(ID)
                    Text = "--Select--" //Text Field(Name)
                });
                foreach (var val in state)
                {
                    items.Add(new ListItem
                    {
                        Value = val.St_ID.ToString(), //Value Field(ID)
                        Text = val.St_Name //Text Field(Name)
                    });
                }

                return Json(items, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult BindLocationJson(string value)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var state = con.Query<BindLocation>("Get_LocationName", new { stateid = value },
                    commandType: CommandType.StoredProcedure);
                List<ListItem> items = new List<ListItem>();
                items.Add(new ListItem
                {
                    Value = "", //Value Field(ID)
                    Text = "--Select--" //Text Field(Name)
                });
                foreach (var val in state)
                {
                    items.Add(new ListItem
                    {
                        Value = val.LocationId.ToString(), //Value Field(ID)
                        Text = val.LocationName //Text Field(Name)
                    });
                }

                return Json(items, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult BindModelNameJson(string value)
        {
            using (var con = new SqlConnection(_connectionString))
            {
              
                var state = con.Query<BindDeviceModel>("GetProductByBrand", new { brand = value },
                    commandType: CommandType.StoredProcedure);
                List<ListItem> items = new List<ListItem>();
                items.Add(new ListItem
                {
                    Value = "", //Value Field(ID)
                    Text = "--Select--" //Text Field(Name)
                });
                foreach (var val in state)
                {
                    items.Add(new ListItem
                    {
                        Value = val.ProductId.ToString(), //Value Field(ID)
                        Text = val.ProductName //Text Field(Name)
                    });
                }

                return Json(items, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult BindModelJson(string value)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var filters = value.Split(',');                
                var products = con.Query<BindDeviceModel>("select productId,ProductName from MstProduct where BrandID=@brand and CategoryID=@category and IsActive =1", new {brand= filters[0],category=filters[1]}
                    );
                List<ListItem> items = new List<ListItem>();
                items.Add(new ListItem
                {
                    Value = "", //Value Field(ID)
                    Text = "--Select--" //Text Field(Name)
                });
                foreach (var val in products)
                {
                    items.Add(new ListItem
                    {
                        Value = val.ProductId.ToString(), //Value Field(ID)
                        Text = val.ProductName //Text Field(Name)
                    });
                }

                return Json(items, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult BindSubCategoryJson(string value)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var state = con.Query<SubCategoryModel>("GetSubCategory", new { category = value },
                    commandType: CommandType.StoredProcedure);
                List<ListItem> items = new List<ListItem>();
                items.Add(new ListItem
                {
                    Value = "", //Value Field(ID)
                    Text = "--Select--" //Text Field(Name)
                });
                foreach (var val in state)
                {
                    items.Add(new ListItem
                    {
                        Value = val.SubCatId.ToString(), //Value Field(ID)
                        Text = val.SubCatName //Text Field(Name)
                    });
                }

                return Json(items, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult NewBindSparePartNameJson(string value,string ModelValue)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var ProductId = con.Query<int>("select ProductId from MstProduct where ProductName=@ProductName", new { @ProductName = ModelValue },
                    commandType: CommandType.Text);
                var state = con.Query<BindSparePartnameModel>("select PartId,PartName from MstSparePart where SpareTypeId=@SpareTypeId and productId=@productId", new { @SpareTypeId = value , @productId= ProductId },
                    commandType: CommandType.Text);
                List<ListItem> items = new List<ListItem>();
                items.Add(new ListItem
                {
                    Value = "", //Value Field(ID)
                    Text = "--Select--" //Text Field(Name)
                });
                foreach (var val in state)
                {
                    items.Add(new ListItem
                    {
                        Value = val.PartId.ToString(), //Value Field(ID)
                        Text = val.PartName //Text Field(Name)
                    });
                }

                return Json(items, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult BindCustomerJson(string mobile)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var customers = con.Query<CustomerModel>("GetClientCustomerByMobile", new { MobileNumber = mobile },
                    commandType: CommandType.StoredProcedure);
                    return Json(customers, JsonRequestBehavior.AllowGet);

            }
        }
        
        public JsonResult BindSparePartNameJson(string value)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var state = con.Query<BindSparePartnameModel>("select PartId,PartName from MstSparePart where SpareTypeId=@SpareTypeId", new { @SpareTypeId = value },
                    commandType: CommandType.Text);
                List<ListItem> items = new List<ListItem>();
                items.Add(new ListItem
                {
                    Value = "", //Value Field(ID)
                    Text = "--Select--" //Text Field(Name)
                });
                foreach (var val in state)
                {
                    items.Add(new ListItem
                    {
                        Value = val.PartId.ToString(), //Value Field(ID)
                        Text = val.PartName //Text Field(Name)
                    });
                }

                return Json(items, JsonRequestBehavior.AllowGet);
            }
        }

        public List<ListItem> BindSpareType()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                List<SpareTypeModel> company = con
                    .Query<SpareTypeModel>(
                        "SELECT distinct SpareTypeName,SpareTypeId from MstSpareType", null,
                        commandType: CommandType.Text).ToList();
                List<ListItem> items = new List<ListItem>();
                items.Add(new ListItem
                {
                    Value = "", //Value Field(ID)
                    Text = "Select" //Text Field(Name)
                });
                foreach (var val in company)
                {
                    items.Add(new ListItem
                    {
                        Value = val.SpareTypeId, //Value Field(ID)
                        Text = val.SpareTypeName //Text Field(Name)
                    });
                }

                return items;
            }
        }
        
        public List<ListItem> BindLocation()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                List<BindLocation> company = con
                    .Query<BindLocation>(
                        "select distinct LocationId, LocationName as LocationName  from MstLocation", null,
                        commandType: CommandType.Text).ToList();
                List<ListItem> items = new List<ListItem>();
                items.Add(new ListItem
                {
                    Value = "", //Value Field(ID)
                    Text = "Select Location" //Text Field(Name)
                });
                foreach (var val in company)
                {
                    items.Add(new ListItem
                    {
                        Value = val.LocationId.ToString(), //Value Field(ID)
                        Text = val.LocationName //Text Field(Name)
                    });
                }

                return items;
            }
        }

        public List<ListItem> BindProduct()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                List<BindDeviceModel> company = con
                    .Query<BindDeviceModel>(
                        "select DISTINCT ProductId,ProductName  from MstProduct ", null,
                        commandType: CommandType.Text).ToList();
                List<ListItem> items = new List<ListItem>();
                items.Add(new ListItem
                {
                    Value = "", //Value Field(ID)
                    Text = "Select Product" //Text Field(Name)
                });
                foreach (var val in company)
                {
                    items.Add(new ListItem
                    {
                        Value = val.ProductId.ToString(), //Value Field(ID)
                        Text = val.ProductName //Text Field(Name)
                    });
                }

                return items;
            }
        }

        //bind dropdown against id
        public List<ListItem> BindProduct(int BrandId)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                List<BindDeviceModel> company = con
                    .Query<BindDeviceModel>(
                        "select DISTINCT ProductId,ProductName  from MstProduct Where BrandID=@BrandId", new { @BrandId = BrandId },
                        commandType: CommandType.Text).ToList();
                List<ListItem> items = new List<ListItem>();
                items.Add(new ListItem
                {
                    Value = "", //Value Field(ID)
                    Text = "Select" //Text Field(Name)
                });
                foreach (var val in company)
                {
                    items.Add(new ListItem
                    {
                        Value = val.ProductId.ToString(), //Value Field(ID)
                        Text = val.ProductName //Text Field(Name)
                    });
                }

                return items;
            }
        }
        public List<ListItem> BindSubCategory(int CategoryId)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                List<SubCategoryModel> company = con
                    .Query<SubCategoryModel>(
                        "SELECT SubCatId,SubCatName from MstSubCategory where CatId=@CatId", new { CatId = CategoryId },
                        commandType: CommandType.Text).ToList();
                List<ListItem> items = new List<ListItem>();
                items.Add(new ListItem
                {
                    Value = "", //Value Field(ID)
                    Text = "Select Sub Category " //Text Field(Name)
                });
                foreach (var val in company)
                {
                    items.Add(new ListItem
                    {
                        Value = val.SubCatId, //Value Field(ID)
                        Text = val.SubCatName //Text Field(Name)
                    });
                }

                return items;
            }
        }
        public List<ListItem> BindPartname(int SpareTypeId)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                List<BindSparePartnameModel> company = con
                    .Query<BindSparePartnameModel>(
                        "SELECT PartId,PartName from MstSparePart where PartId=@SpareTypeId", new { @SpareTypeId = SpareTypeId },
                        commandType: CommandType.Text).ToList();
                List<ListItem> items = new List<ListItem>();
                items.Add(new ListItem
                {
                    Value = "", //Value Field(ID)
                    Text = "Select Sub Category " //Text Field(Name)
                });
                foreach (var val in company)
                {
                    items.Add(new ListItem
                    {
                        Value = val.PartId.ToString(), //Value Field(ID)
                        Text = val.PartName //Text Field(Name)
                    });
                }

                return items;
            }
        }

        public List<ListItem> BindState(int CountryId)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                List<BindDropdown> company = con
                    .Query<BindDropdown>(
                        "SELECT St_ID,St_Name from mststate where St_CntyId=@St_CntyId", new { @St_CntyId = CountryId },
                        commandType: CommandType.Text).ToList();
                List<ListItem> items = new List<ListItem>();
                items.Add(new ListItem
                {
                    Value = "", //Value Field(ID)
                    Text = "Select " //Text Field(Name)
                });
                foreach (var val in company)
                {
                    items.Add(new ListItem
                    {
                        Value = val.St_ID.ToString(), //Value Field(ID)
                        Text = val.St_Name //Text Field(Name)
                    });
                }

                return items;
            }
        }
        public List<ListItem> BindLocation(int StateId)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                List<BindDropdown> company = con
                    .Query<BindDropdown>(
                        "select LocationId AS St_ID, LocationName as St_Name  from MstLocation where StateId=@St_StateId", new { @St_StateId = StateId },
                        commandType: CommandType.Text).ToList();
                List<ListItem> items = new List<ListItem>();
                items.Add(new ListItem
                {
                    Value = "", //Value Field(ID)
                    Text = "Select " //Text Field(Name)
                });
                foreach (var val in company)
                {
                    items.Add(new ListItem
                    {
                        Value = val.St_ID.ToString(), //Value Field(ID)
                        Text = val.St_Name //Text Field(Name)
                    });
                }

                return items;
            }
        }
        public List<ListItem> BindTrcAjax()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                List<BindTrcModel> company = con
                    .Query<BindTrcModel>("select TRC_ID,TRC_NAME from msttrc",
                        null, commandType: CommandType.Text).ToList();
                List<ListItem> items = new List<ListItem>();
                items.Add(new ListItem
                {
                    Value = "", //Value Field(ID)
                    Text = "Select " //Text Field(Name)
                });
                foreach (var val in company)
                {
                    items.Add(new ListItem
                    {
                        Value = val.TRC_Id.ToString(), //Value Field(ID)
                        Text = val.TRC_NAME //Text Field(Name)
                    });
                }

                return items;
            }
        }

        public List<ListItem> BindPinCode()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                List<BindPinCodeModel> company = con
                    .Query<BindPinCodeModel>("SELECT  ID, pin_code FROM Pincode_Master WHERE ID  IS NOT NULL",
                        null, commandType: CommandType.Text).ToList();
                List<ListItem> items = new List<ListItem>();
                items.Add(new ListItem
                {
                    Value = "", //Value Field(ID)
                    Text = "Select" //Text Field(Name)
                });
                foreach (var val in company)
                {
                    items.Add(new ListItem
                    {
                        Value = val.ID, //Value Field(ID)
                        Text = val.pin_code //Text Field(Name)
                    });
                }
                return items;
            }
        }

        public List<ListItem> BindCourier()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                List<BindCourierModel> company = con
                    .Query<BindCourierModel>(
                        "select CourierId,CourierName from Courier_Master WHERE CourierName is NOT NULL", null,
                        commandType: CommandType.Text).ToList();
                List<ListItem> items = new List<ListItem>();
                items.Add(new ListItem
                {
                    Value = "", //Value Field(ID)
                    Text = "Select " //Text Field(Name)
                });
                foreach (var val in company)
                {
                    items.Add(new ListItem
                    {
                        Value = val.CourierId, //Value Field(ID)
                        Text = val.CourierName //Text Field(Name)
                    });
                }

                return items;
            }
        }
        public List<ListItem> BindEngineer()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                List<BindEngineerModel> company = con
                    .Query<BindEngineerModel>(
                        "select EngineerId,EmployeeName FROM MstEngineer WHERE EmployeeName is NOT NULL", null,
                        commandType: CommandType.Text).ToList();
                List<ListItem> items = new List<ListItem>();
                items.Add(new ListItem
                {
                    Value = "", //Value Field(ID)
                    Text = "Select " //Text Field(Name)
                });
                foreach (var val in company)
                {
                    items.Add(new ListItem
                    {
                        Value = val.EngineerId, //Value Field(ID)
                        Text = val.EmployeeName //Text Field(Name)
                    });
                }

                return items;
            }
        }
        public JsonResult BindCityByPincode(string value)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var rmid = con.Query<BindDropdown1>("Get_State_City_pincode", new { pincode = value }, commandType: CommandType.StoredProcedure);
                List<ListItem> items = new List<ListItem>();

                foreach (var val in rmid)
                {
                    items.Add(new ListItem
                    {
                        Value = val.ID, //Value Field(ID)
                        Text = val.dist_Name //Text Field(Name)
                    });
                }
                return Json(items, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult BindSatetByPincode(string value)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var rmid = con.Query<BindDropdown1>("Get_State_City_pincode", new { pincode = value }, commandType: CommandType.StoredProcedure);
                List<ListItem> items = new List<ListItem>();

                foreach (var val in rmid)
                {
                    items.Add(new ListItem
                    {
                        Value = val.ID, //Value Field(ID)
                        Text = val.state_Name //Text Field(Name)
                    });
                }
                return Json(items, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult BindPinCodeByCountry(string value)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var pincodes = con.Query<BindPinCodeModel>("SELECT  ID, pin_code FROM Pincode_Master where CountryId=@CountryId", new { @CountryId = value },
                        commandType: CommandType.Text);
                List <ListItem> items = new List<ListItem>();
                items.Add(new ListItem
                {
                    Value = "", //Value Field(ID)
                    Text = "--Select--" //Text Field(Name)
                });
                foreach (var val in pincodes)
                {
                    items.Add(new ListItem
                    {
                        Value = val.ID.ToString(), //Value Field(ID)
                        Text = val.pin_code //Text Field(Name)
                    });
                }

                return Json(items, JsonRequestBehavior.AllowGet);
            }
        }
        public List<ListItem> BindPincodeListByCountry(int CountryId)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                List<BindDropdown> company = con
                    .Query<BindDropdown>(
                        "SELECT  ID St_ID, pin_code St_Name FROM Pincode_Master where CountryId=@CountryId", new { @CountryId = CountryId },
                        commandType: CommandType.Text).ToList();
                List<ListItem> items = new List<ListItem>();
                items.Add(new ListItem
                {
                    Value = "", //Value Field(ID)
                    Text = "Select " //Text Field(Name)
                });
                foreach (var val in company)
                {
                    items.Add(new ListItem
                    {
                        Value = val.St_ID.ToString(), //Value Field(ID)
                        Text = val.St_Name //Text Field(Name)
                    });
                }

                return items;
            }
        }
        public JsonResult JsonCentersByProvider(Guid value)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var rmid = con.Query<CheckBox>("Select CenterId Name,CenterName Text From  MSTServiceCenters where IsActive=1 and ProviderId=@providerId",
                    new {providerId=value});
                List<ListItem> items = new List<ListItem>();

                items.Add(new ListItem
                {
                    Value = null, //Value Field(ID)
                    Text = "--Select--" //Text Field(Name)
                });
                foreach (var val in rmid)
                {
                    items.Add(new ListItem
                    {
                        Value = val.Name.ToString(), //Value Field(ID)
                        Text = val.Text //Text Field(Name)
                    });
                }
                return Json(items, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> JsonGetBank(Guid Id)
        {            
                var _bank = await _bankRepo.GetBankByBankId(Id);

                return Json(_bank, JsonRequestBehavior.AllowGet);            
        }
        public async Task<ActionResult> JsonGetPerson(Guid Id)
        {
            try
            {
                var _contact = await _ContactRepo.GetContactPersonByContactId(Id);
                return Json(_contact, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex ;
            }
        }
    }
}