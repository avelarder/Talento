using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Talento.Models;

namespace Talento.Controllers
{
    public class TagsController : Controller
    {
        Core.ITag TagHelper;

        public TagsController(Core.ITag tagHelper)
        {
            TagHelper = tagHelper;

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Entities.Tag, Models.TagModel>();
            });
        }
        // GET: Tags
        public async Task<ActionResult> Index()
        {
            var entities = await TagHelper.GetAll();
            var model = AutoMapper.Mapper.Map<List<Models.TagModel>>(entities);
            return View(model);
        }

        // GET: Tags/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TagModel tag = AutoMapper.Mapper.Map<TagModel>(await TagHelper.Get(Convert.ToInt32(id)));
            if (tag == null)
            {
                return HttpNotFound();
            }
            return View(tag);
        }

        // GET: Tags/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tags/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Name")] TagModel tag)
        {
            if (ModelState.IsValid)
            {
                var entity = AutoMapper.Mapper.Map<Entities.Tag>(tag);
                await TagHelper.Create(entity);
                return RedirectToAction("Index");
            }

            return View(tag);
        }

        // GET: Tags/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TagModel tag = AutoMapper.Mapper.Map<TagModel>( await TagHelper.Get(Convert.ToInt32(id)));
            if (tag == null)
            {
                return HttpNotFound();
            }
            return View(tag);
        }

        // POST: Tags/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Name")] TagModel tag)
        {
            if (ModelState.IsValid)
            {
                var entity = AutoMapper.Mapper.Map<Entities.Tag>(tag);
                await TagHelper.Edit(entity);
                return RedirectToAction("Index");
            }
            return View(tag);
        }

        // GET: Tags/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TagModel tag = AutoMapper.Mapper.Map<TagModel>(await TagHelper.Get(Convert.ToInt32(id)));
            if (tag == null)
            {
                return HttpNotFound();
            }
            return View(tag);
        }

        // POST: Tags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            await TagHelper.Delete(Convert.ToInt32(id));
            return RedirectToAction("Index");
        }

    }
}
