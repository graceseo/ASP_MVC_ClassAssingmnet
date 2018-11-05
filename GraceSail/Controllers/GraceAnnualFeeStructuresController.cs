using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GraceSail.Models;

namespace GraceSail.Controllers
{
    public class GraceAnnualFeeStructuresController : Controller
    {
        private readonly SailContext _context;

        public GraceAnnualFeeStructuresController(SailContext context)
        {
            _context = context;
        }

        // GET: GraceAnnualFeeStructures
        public async Task<IActionResult> Index()
        {
            //Order the record by year -Grace
            var annualFee =await _context.AnnualFeeStructure.OrderByDescending(a => a.Year).ToListAsync();
            return View(annualFee);
        }

        // GET: GraceAnnualFeeStructures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var annualFeeStructure = await _context.AnnualFeeStructure
                .FirstOrDefaultAsync(m => m.Year == id);
            if (annualFeeStructure == null)
            {
                return NotFound();
            }

            return View(annualFeeStructure);
        }

        // GET: GraceAnnualFeeStructures/Create
        public IActionResult Create()
        {
            //retrieve most recent first -Grace
            var recentFee = _context.AnnualFeeStructure.OrderByDescending(a => a.Year).First();

            return View(recentFee);
            //return View();
        }

        // POST: GraceAnnualFeeStructures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Year,AnnualFee,EarlyDiscountedFee,EarlyDiscountEndDate,RenewDeadlineDate,TaskExemptionFee,SecondBoatFee,ThirdBoatFee,ForthAndSubsequentBoatFee,NonSailFee,NewMember25DiscountDate,NewMember50DiscountDate,NewMember75DiscountDate")] AnnualFeeStructure annualFeeStructure)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(annualFeeStructure);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.GetBaseException().Message);
            }
           
            return View(annualFeeStructure);
        }

        // GET: GraceAnnualFeeStructures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            int currentYear = DateTime.Now.Year;

            //if the user tries to edit a prior years's record, return the index view --Grace
            if (id >= currentYear)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var annualFeeStructure = await _context.AnnualFeeStructure.FindAsync(id);
                if (annualFeeStructure == null)
                {
                    return NotFound();
                }
                //Show the edit view --Grace
                return View(annualFeeStructure);
            }
            else
            {
                //show the index view --Grace
                return RedirectToAction(nameof(Index));
            }

        }

        // POST: GraceAnnualFeeStructures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Year,AnnualFee,EarlyDiscountedFee,EarlyDiscountEndDate,RenewDeadlineDate,TaskExemptionFee,SecondBoatFee,ThirdBoatFee,ForthAndSubsequentBoatFee,NonSailFee,NewMember25DiscountDate,NewMember50DiscountDate,NewMember75DiscountDate")] AnnualFeeStructure annualFeeStructure)
        {
            if (id != annualFeeStructure.Year)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(annualFeeStructure);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnnualFeeStructureExists(annualFeeStructure.Year))
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
            return View(annualFeeStructure);
        }

        // GET: GraceAnnualFeeStructures/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var annualFeeStructure = await _context.AnnualFeeStructure
                .FirstOrDefaultAsync(m => m.Year == id);
            if (annualFeeStructure == null)
            {
                return NotFound();
            }

            return View(annualFeeStructure);
        }

        // POST: GraceAnnualFeeStructures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var annualFeeStructure = await _context.AnnualFeeStructure.FindAsync(id);
            _context.AnnualFeeStructure.Remove(annualFeeStructure);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnnualFeeStructureExists(int id)
        {
            return _context.AnnualFeeStructure.Any(e => e.Year == id);
        }
    }
}
