﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BreadCrumbs.Controllers
{
    public class TicketViewController : Controller
    {
        // GET: TicketView
        public ActionResult Index()
        {
            return View();
        }

        // GET: TicketView/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TicketView/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TicketView/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TicketView/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TicketView/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TicketView/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TicketView/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}