using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using NBL.CMS.Models;
using NBL.CMS.Extensions;
using NBLEFC1;
using PagedList;

namespace NBL.CMS.Controllers
{
    public class BIMObjectController : Controller
    {
        private NBLDataAccess nbl = new NBLDataAccess();

        public BIMObjectController()
        {

         
            Mapper.CreateMap<BIMObject, BIMObjectDetailsViewModel>();
            Mapper.CreateMap<BIMObject, BIMObjectCreateViewModel>();
            Mapper.CreateMap<BIMObject, BIMObjectEditViewModel>();
            Mapper.CreateMap<BIMObject, BIMObjectListViewModel>();
            Mapper.CreateMap<BIMObjectShortUrl, BIMObjectShortUrlViewModel>();
            Mapper.CreateMap<BIMThumbnail, BIMThumbnailViewModel>().IgnoreAllNonExisting();

            Mapper.CreateMap<BIMObjectDetailsViewModel, BIMObject>().IgnoreAllNonExisting();
            Mapper.CreateMap<BIMObjectCreateViewModel, BIMObject>().IgnoreAllNonExisting();
            Mapper.CreateMap<BIMObjectEditViewModel, BIMObject>().IgnoreAllNonExisting();
            Mapper.CreateMap<BIMObjectListViewModel, BIMObject>().IgnoreAllNonExisting();
            Mapper.CreateMap<BIMObjectShortUrlViewModel, BIMObjectShortUrl>().IgnoreAllNonExisting();
            Mapper.CreateMap<BIMThumbnailViewModel, BIMThumbnail>().IgnoreAllNonExisting();


            Mapper.AssertConfigurationIsValid();
        }

        //
        // GET: /BIMObjectNBL/

        public ActionResult Index(int? p, int? ps, string so, string s, string cs)
        {
            int page = p ?? 1;
            int pageSize = ps ?? 10;
            SetupViewBag(page, pageSize, so, cs);

            ViewBag.NameSortParm = so == "Name" ? "Name desc" : "Name";
            ViewBag.VersionSortParm = so == "Version" ? "Version desc" : "Version";
      
            var obs = nbl.GetObjects();
            List<BIMObjectListViewModel> lboms = Mapper.Map<List<BIMObject>, List<BIMObjectListViewModel>>(obs);

            if (Request.HttpMethod == "GET")
            {
                s = cs;
            }
            else
            {
                page = 1;
                ViewBag.Page = null;
            }
            ViewBag.CurrentSearch = s;

            if (!String.IsNullOrEmpty(s))
            {
                lboms = lboms.Where(x => x.Name.ToLower().Contains(s.ToLower())).ToList();
            }

            switch (so)
            {
                case "Name":
                    lboms = lboms.OrderBy(x => x.Name).ToList();
                    break;
                case "Name desc":
                    lboms = lboms.OrderByDescending(x => x.Name).ToList();
                    break;
                case "Version":
                    lboms = lboms.OrderBy(x => x.Version).ToList();
                    break;
                case "Version desc":
                    lboms = lboms.OrderByDescending(x => x.Version).ToList();
                    break;
                default:
                    break;
            }


            return View(lboms.ToPagedList(page, pageSize));
        }

        //
        // GET: /BIMObjectNBL/Details/5

        public ActionResult Details(int id = 0, int p = 1, int ps = 10, string so = null, string cs = null)
        {
            SetupViewBag(p, ps, so, cs);
            BIMObject bo = nbl.GetObject(id);
            if (bo == null)
            {
                return HttpNotFound();
            }
            return View(Mapper.Map<BIMObject, BIMObjectDetailsViewModel>(bo));
        }

        //
        // GET: /BIMObjectNBL/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /BIMObjectNBL/Create

        [HttpPost]
        public ActionResult Create(BIMObjectCreateViewModel bimobject)
        {
            if (ModelState.IsValid)
            {
                nbl.CreateObject(Mapper.Map<BIMObjectCreateViewModel, BIMObject>(bimobject));
                return RedirectToAction("Index");
            }

            return View(bimobject);
        }

        //
        // GET: /BIMObjectNBL/Edit/5

        public ActionResult Edit(int id = 0, int p = 1, int ps = 10, string so = null, string cs = null)
        {
            SetupViewBag(p, ps, so, cs);
            NBLDataAccess nbl = new NBLDataAccess();
            BIMObject bo = nbl.GetObject(id, new string[] { "ShortUrl", "BIMThumbnail" });
            if (bo == null)
            {
                return HttpNotFound();
            }
            if (bo.StateFK == 7)
            {
                int newId = nbl.DraftObject(id);
                return RedirectToAction("Edit", new { id = newId, p = ViewBag.Page, ps = ViewBag.PageSize, so = ViewBag.CurrentSort, cs = ViewBag.CurrentSearch });
            }
 
            BIMObjectEditViewModel bimobject = Mapper.Map<BIMObject, BIMObjectEditViewModel>(bo);
            return View(bimobject);
        }

        //
        // POST: /BIMObjectNBL/Edit/5

        [HttpPost]
        public ActionResult Edit(BIMObjectEditViewModel bimobject, int p = 1, int ps = 10, string so = null, string cs = null)
        {
            SetupViewBag(p, ps, so, cs);
            if (ModelState.IsValid)
            {
                BIMObject bo = nbl.GetObject(bimobject.ID, new string[] {"ShortUrl", "BIMThumbnail"});
                Mapper.Map<BIMObjectEditViewModel, BIMObject>(bimobject, bo);
                nbl.UpdateObject(bo);
                return RedirectToAction("Index", new {p = ViewBag.Page, ps = ViewBag.PageSize, so = ViewBag.CurrentSort, cs = ViewBag.CurrentSearch });
            }
            return View(bimobject);
        }

        //
        // GET: /BIMObjectNBL/Delete/5

        public ActionResult Delete(int id = 0)
        {
            BIMObject bo = nbl.GetObject(id);
            if (bo == null)
            {
                return HttpNotFound();
            }
            return View(Mapper.Map<BIMObject, BIMObjectDetailsViewModel>(bo));
        }

        //
        // POST: /BIMObjectNBL/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            nbl.PermanentlyDeleteObject(id);
            return RedirectToAction("Index");
        }

        private void SetupViewBag(int p = 1, int ps = 10, string so = null, string cs = null)
        {
            if (p > 1)
            {
                ViewBag.Page = p;
            }
            if (ps != 10)
            {
                ViewBag.PageSize = ps;
            }
            ViewBag.CurrentSort = so;
            ViewBag.CurrentSearch = cs;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}