using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using doorserve.Filters;
using doorserve.Models;
using doorserve.Models.WildCards;
using doorserve.Permission;
using doorserve.Repository.WildCards;

namespace doorserve.Controllers
{
    public class WildCardsController : BaseController
    {
        private readonly IWildCards _wildCardRepo;
        public WildCardsController()
        {
            _wildCardRepo = new WildCards();

        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Wild_Cards)]
        public async Task<ActionResult> Index()
        {
      
            var wildcards = await _wildCardRepo.GetWildCards(new FilterModel { CompId= CurrentUser.CompanyId} );
  
            return View(wildcards);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Wild_Cards)]
        public async Task<ActionResult> Create()
        {
            var wildcardmodel = new WildCardModel();
            wildcardmodel.ActionTypeList = new SelectList(await CommonModel.GetActionTypes(), "Value", "Text");
            if (CurrentUser.UserTypeName.ToLower() == "super admin")
            {
                wildcardmodel.IsAdmin = true;
                wildcardmodel.CompanyList = new SelectList(await CommonModel.GetCompanies(), "Name", "Text");
            }
            return View(wildcardmodel);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Wild_Cards)]
        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult> Create(WildCardModel wildcardModel)
        {

            wildcardModel.UserId = CurrentUser.UserId;
            if (CurrentUser.UserTypeName.ToLower() != "super admin")
                wildcardModel.CompanyId = CurrentUser.CompanyId;
 
            var response = await _wildCardRepo.AddUpdateDeleteWildCards(wildcardModel, 'I');
                _wildCardRepo.Save();
                TempData["response"] = response;
                return RedirectToAction("Index");
                     
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Wild_Cards)]
        public async Task<ActionResult> Edit(int id)
        {
            var wildcard = await _wildCardRepo.GetWildCardByWildCardId(id);
            var array = wildcard.ActionTypeIds.Split(',');
            List<int> ActionTypes = new List<int>();
            for (int i = 0; i < array.Length; i++)
            {
                ActionTypes.Add(Convert.ToInt32(array[i]));


            }
            wildcard.ActionTypeList = new SelectList(await CommonModel.GetActionTypes(), "Value", "Text");
            wildcard.actionTypes = ActionTypes;
            if (CurrentUser.UserTypeName.ToLower() == "super admin")
            {
                wildcard.IsAdmin = true;
                wildcard.CompanyList = new SelectList(await CommonModel.GetCompanies(), "Name", "Text");
            }
            return View(wildcard);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Wild_Cards)]
        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult> Edit(WildCardModel wildcardModel)
        {
    

            wildcardModel.UserId = CurrentUser.UserId;
            if (CurrentUser.UserTypeName.ToLower() != "super admin")
                wildcardModel.CompanyId = CurrentUser.CompanyId;

                var response = await _wildCardRepo.AddUpdateDeleteWildCards(wildcardModel, 'U');
                _wildCardRepo.Save();
                
                TempData["response"] = response;
                return RedirectToAction("Index");
          

        }
    }
}