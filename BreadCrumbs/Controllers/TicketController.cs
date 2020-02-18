using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BreadCrumbs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BreadCrumbs.Areas.Identity.Data;

namespace BreadCrumbs.Controllers
{
    [Authorize]
    public class TicketController : Controller
    {
        private readonly TicketContext _context;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<BreadCrumbsUser> userManager;

        public TicketController(TicketContext context, RoleManager<IdentityRole> roleManager, UserManager<BreadCrumbsUser> userManager)
        {
            _context = context;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        // GET: Ticket
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["UserSortParm"] = String.IsNullOrEmpty(sortOrder) ? "user_desc" : "";
            ViewData["StatusSortParm"] = sortOrder == "Status" ? "status_desc" : "Status";
            ViewData["CurrentFilter"] = searchString;
            var tickets = from t in _context.Tickets
                           select t;
            if (!String.IsNullOrEmpty(searchString))
            {
                tickets = tickets.Where(s => s.Title.Contains(searchString)
                                       || s.Description.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "user_desc":
                    tickets = tickets.OrderByDescending(t => t.CreatedByUser);
                    break;
                case "Status":
                    tickets = tickets.OrderBy(t => t.TicketStatus);
                    break;
                case "status_desc":
                    tickets = tickets.OrderByDescending(t => t.TicketStatus);
                    break;
                default:
                    tickets = tickets.OrderByDescending(t => t.TicketId);
                    break;

            }
            return View(await tickets.AsNoTracking().ToListAsync().ConfigureAwait(true));
        }

        // GET: Ticket/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FirstOrDefaultAsync(m => m.TicketId == id);

            var role = await roleManager.FindByNameAsync("Ticket #" + id);

            if (ticket == null)
            {
                return NotFound();
            }

            if (role == null)
            {
                var ticketRoleName = "Ticket #" + ticket.TicketId;

                IdentityRole identityRole = new IdentityRole
                {
                    Name = ticketRoleName
                };

                await roleManager.CreateAsync(identityRole);

                var user = await userManager.FindByNameAsync(ticket.CreatedByUser);

                await userManager.AddToRoleAsync(user, ticketRoleName);

                role = await roleManager.FindByNameAsync("Ticket #" + id);
            }

            var editRoleViewModel = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };

            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    editRoleViewModel.Users.Add(user.UserName);
                }
            }

            var model = new TicketDetailViewModel
            {
                Ticket = ticket,
                EditRoleViewModel = editRoleViewModel
            };

            return View(model);
        }

        // GET: Ticket/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ticket/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TicketId,Title,Description,CreatedByUser,TicketStatus")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync();

                var ticketRoleName = "Ticket #" + ticket.TicketId;

                IdentityRole identityRole = new IdentityRole
                {
                    Name = ticketRoleName
                };

                await roleManager.CreateAsync(identityRole);

                var user = await userManager.FindByNameAsync(ticket.CreatedByUser);

                await userManager.AddToRoleAsync(user, ticketRoleName);

                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: Ticket/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FindAsync(id);

            var role = await roleManager.FindByNameAsync("Ticket #" + id);

            if (ticket == null)
            {
                return NotFound();
            }

            if (role == null)
            {
                var ticketRoleName = "Ticket #" + ticket.TicketId;

                IdentityRole identityRole = new IdentityRole
                {
                    Name = ticketRoleName
                };

                await roleManager.CreateAsync(identityRole);

                var user = await userManager.FindByNameAsync(ticket.CreatedByUser);

                await userManager.AddToRoleAsync(user, ticketRoleName);

                role = await roleManager.FindByNameAsync("Ticket #" + id);
            }

            var editRoleViewModel = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };

            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    editRoleViewModel.Users.Add(user.UserName);
                }
            }

            var model = new TicketDetailViewModel
            {
                Ticket = ticket,
                EditRoleViewModel = editRoleViewModel
            };

            return View(model);
        }

        // POST: Ticket/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TicketId,Title,Description,CreatedByUser,TicketStatus")] Ticket ticket)
        {
            if (id != ticket.TicketId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.TicketId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: Ticket/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FirstOrDefaultAsync(m => m.TicketId == id);

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Ticket/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.TicketId == id);
        }

        [HttpGet]
        public async Task<IActionResult> EditTicketMember(string roleId, int ticketId)
        {
            ViewBag.roleId = roleId;

            ViewBag.ticketId = ticketId;

            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found.";
                return View("Error");
            }

            var model = new List<UserRoleViewModel>();

            foreach (var user in userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }

                model.Add(userRoleViewModel);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditTicketMember(List<UserRoleViewModel> model, string roleId, int ticketId)
        {
            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found.";
                return View("Error");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);

                IdentityResult result = null;

                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("Edit", new { Id = ticketId });
                }

            }
            return RedirectToAction("Edit", new { Id = ticketId });
        }
    }
}
