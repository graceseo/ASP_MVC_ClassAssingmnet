using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GraceSail.Models;
using Microsoft.AspNetCore.Http;

namespace GraceSail.Controllers
{
    public class GraceMembershipsController : Controller
    {
        private readonly SailContext _context;

        public GraceMembershipsController(SailContext context)
        {
            _context = context;
        }

        // GET: GraceMemberships
        //Add a parameter which will recieve Query string --Grace
        public async Task<IActionResult> Index(int memberId = 0, string memberFullName="")
        {
            //Check memberId is not 0 --Grace
            if (memberId > 0)
            {
                HttpContext.Session.SetInt32("memberId", memberId);
            }

            //check whether the Session has memberId --Grace
            if (HttpContext.Session.GetInt32("memberId") != null)
            {
                memberId = Convert.ToInt32(HttpContext.Session.GetInt32("memberId"));
                var sailContext = _context.Membership.Where(m => m.MemberId == memberId).Include(m => m.Member).Include(m => m.MembershipTypeNameNavigation).OrderByDescending(m => m.Year);

                return View(await sailContext.ToListAsync());
            }
            //if the session doesn't have memberId, return MemebersControllers
            else
            {
                return RedirectToAction("Index", "GraceMembers");
            }
        }

        // GET: GraceMemberships/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membership = await _context.Membership
                .Include(m => m.Member)
                .Include(m => m.MembershipTypeNameNavigation)
                .FirstOrDefaultAsync(m => m.MembershipId == id);
            if (membership == null)
            {
                return NotFound();
            }

            return View(membership);
        }

        // GET: GraceMemberships/Create
        public IActionResult Create()
        {
            ViewData["MemberId"] = new SelectList(_context.Member, "MemberId", "FirstName");
            ViewData["MembershipTypeName"] = new SelectList(_context.MembershipType, "MembershipTypeName", "MembershipTypeName");
            return View();
        }

        // POST: GraceMemberships/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MembershipId,MemberId,Year,MembershipTypeName,Fee,Comments,Paid")] Membership membership)
        {
            if (ModelState.IsValid)
            {
                _context.Add(membership);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemberId"] = new SelectList(_context.Member, "MemberId", "FirstName", membership.MemberId);
            ViewData["MembershipTypeName"] = new SelectList(_context.MembershipType, "MembershipTypeName", "MembershipTypeName", membership.MembershipTypeName);
            return View(membership);
        }

        // GET: GraceMemberships/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membership = await _context.Membership.FindAsync(id);
            if (membership == null)
            {
                return NotFound();
            }
            ViewData["MemberId"] = new SelectList(_context.Member, "MemberId", "FirstName", membership.MemberId);
            ViewData["MembershipTypeName"] = new SelectList(_context.MembershipType, "MembershipTypeName", "MembershipTypeName", membership.MembershipTypeName);
            return View(membership);
        }

        // POST: GraceMemberships/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MembershipId,MemberId,Year,MembershipTypeName,Fee,Comments,Paid")] Membership membership)
        {
            if (id != membership.MembershipId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(membership);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MembershipExists(membership.MembershipId))
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
            ViewData["MemberId"] = new SelectList(_context.Member, "MemberId", "FirstName", membership.MemberId);
            ViewData["MembershipTypeName"] = new SelectList(_context.MembershipType, "MembershipTypeName", "MembershipTypeName", membership.MembershipTypeName);
            return View(membership);
        }

        // GET: GraceMemberships/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membership = await _context.Membership
                .Include(m => m.Member)
                .Include(m => m.MembershipTypeNameNavigation)
                .FirstOrDefaultAsync(m => m.MembershipId == id);
            if (membership == null)
            {
                return NotFound();
            }

            return View(membership);
        }

        // POST: GraceMemberships/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var membership = await _context.Membership.FindAsync(id);
            _context.Membership.Remove(membership);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MembershipExists(int id)
        {
            return _context.Membership.Any(e => e.MembershipId == id);
        }
    }
}
