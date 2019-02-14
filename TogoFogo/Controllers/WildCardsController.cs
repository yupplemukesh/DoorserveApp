using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TogoFogo.Models;
using TogoFogo.Repository.WildCards;

namespace TogoFogo.Controllers
{
    public class WildCardsController : Controller
    {
        private readonly IWildCards _wildCardModel;
        public WildCardsController()
        {
            _wildCardModel = new WildCards();

        }

        public async Task<ActionResult> Index()
        {
            var wildcard = await _wildCardModel.GetWildCards();
            return View(wildcard);
        }

        public async Task<ActionResult> Create()
        {
            var wildcardmodel = new WildCardModel();
            return View(wildcardmodel);
        }

        [HttpPost]
        public async Task<ActionResult> Create(WildCardModel wildcard)
        {
            if (ModelState.IsValid)
            {
                wildcard.AddedBy = Convert.ToInt32(Session["User_ID"]);
                var response = await _wildCardModel.AddUpdateDeleteWildCards(wildcard, 'I');
                _wildCardModel.Save();
                TempData["response"] = response;
                TempData.Keep("response");
                return RedirectToAction("Index");
            }
            else
                return View(wildcard);

        }
        public async Task<ActionResult> Edit(int id)
        {
            var wildcard = await _wildCardModel.GetActionByWildCardId(id);
            return View(wildcard);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(WildCardModel wildcard)
        {
            if (ModelState.IsValid)
            {
                var response = await _wildCardModel.AddUpdateDeleteWildCards(wildcard, 'U');
                _wildCardModel.Save();
                TempData["response"] = response;
                TempData.Keep("response");
                return RedirectToAction("Index");
            }
            else

                return View(wildcard);

        }
    }
}