using Entities.Models;
using Entities.ViewModels;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using myWebApp.ViewModels.AcademicCalendar;
using Newtonsoft.Json;
using Org.BouncyCastle.Ocsp;
using System.Collections.Immutable;
using System.Data;
using System.Globalization;
using System.Numerics;
using System.Security.Claims;
using System.Security.Cryptography;

namespace myWebApp.Controllers
{
    [Authorize]
    public class AcademicCalendarController : Controller
    {
        public readonly IEFRepository _repository;
        public readonly SchoolDbContext _db;
        public AcademicCalendarController(IEFRepository repository, SchoolDbContext db)
        {
            _repository = repository;
            _db = db;
        }


        #region Year
        [Authorize(Policy = "Year.Read")]
        public async Task<IActionResult> Year()
        {
            var allyears = await _db.years.Where(x => x.IsActive == true).ToListAsync();
            return View(allyears);
        }
        [Authorize(Policy = "Year.Create")]
        [HttpGet]
        public IActionResult AddYear()
        {
            return View();
        }
        [Authorize(Policy = "Year.Create")]
        [HttpPost]
        public async Task<IActionResult> AddYear(YearVM year)
        {
            if (((year.StartDate.Year % 4 == 0) && (year.StartDate.Year % 100 != 0)) || (year.StartDate.Year % 400 == 0)) year.IsLeapYear = true;
            var newYear = new Year
            {
                YearName = year.YearName,
                StartDate = year.StartDate,
                EndDate = year.EndDate,
                IsLeapYear = year.IsLeapYear
            };
            await _repository.AddAsync(newYear);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Year");
            }
            else
            {
                ModelState.AddModelError("", "Error While Saving to Database");
            }
            return View(year);
        }
        [Authorize(Policy = "Year.Update")]
        [HttpGet]
        public async Task<IActionResult> UpdateYear(int id)
        {
            var year = await _db.years.Where(x => x.IsActive == true && x.YearId == id).FirstOrDefaultAsync();
            var Year = new UpdateYearVM
            {
                YearName = year.YearName,
                YearId = year.YearId,
                StartDate = (DateTime)year.StartDate,
                EndDate = (DateTime)year.EndDate,
                IsActive = (bool)year.IsActive
            };
            return View(Year);
        }
        [Authorize(Policy = "Year.Update")]
        [HttpPost]
        public async Task<IActionResult> UpdateYear(UpdateYearVM Uyear)
        {
            var year = await _repository.GetYearById(Uyear.YearId);
            year.StartDate = Uyear.StartDate;
            year.EndDate = Uyear.EndDate;
            year.YearName = Uyear.YearName;
            year.IsActive = Uyear.IsActive;
            if (DateTime.IsLeapYear(year.StartDate.Value.Year)) year.IsLeapYear = true;
            await _repository.UpdateAsync(year);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Year");
            }
            ModelState.AddModelError("", "Error While Saving to Database");
            return View(Uyear);
        }
        [Authorize(Policy = "Year.Delete")]
        [HttpGet]
        public async Task<IActionResult> DeleteYear(int id)
        {
            var year = await _repository.GetYearById(id);
            var terms = _db.terms.Where(x => x.YearId == year.YearId).ToList();
            foreach (var term in terms)
            {
                var holidays = _db.Holidays.Where(x => x.TermId == term.TermId).ToList();
                foreach (var holiday in holidays)
                {
                    holiday.IsActive = false;
                }
                term.IsActive = false;
            }
            if (year == null)
            {
                return NotFound();
            }
            year.IsActive = false;
            await _repository.UpdateAsync(year);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Year");
            }
            return RedirectToAction("Year");
        }
        #endregion

        #region Term
        //[HttpGet]
        //public async Task<IActionResult> Term()
        //{
        //    var terms = await _repository.GetTerms();
        //    return View(terms);
        //}
        [Authorize(Policy = "Term.Read")]
        [HttpGet]
        public async Task<IActionResult> Term(int Id)
        {
            var year = _db.years.Where(x => x.YearId == Id && x.IsActive == true).FirstOrDefault();
            //var terms = _db.terms.Where(x => x.YearId == Id).ToList();
            //ViewBag.Terms = terms;
            ViewBag.Year = year?.YearName;
            TermVM term = new TermVM();
            var AllTerms = from a in _db.terms
                           from b in _db.years
                           where a.YearId == Id && b.YearId == Id
                           select new TermList
                           {
                               YearName = b.YearName,
                               TermName = a.TermName,
                               TotalSatSundays = a.TotalSatSun,
                               TotalSchoolDays = a.TotalSchoolDays,
                               StartDate = a.StartDate,
                               EndDate = a.EndDate,
                               TotalDays = a.TotalDays,
                               AssesmentDays = a.AssesmentDays,
                               Holidays = a.TermHolidays,
                               TermId = a.TermId,
                               AssessmentWiseSchoolDays = a.AssesmentWiseTermDays,
                               IsActive = a.IsActive
                           };
            term.YearId = Id;
            term.MinDate = year.StartDate;
            term.MaxDate = year.EndDate;
            term.Terms = await AllTerms.Select(x => new TermList { IsActive = x.IsActive, TermId = x.TermId, AssessmentWiseSchoolDays = x.AssessmentWiseSchoolDays, AssesmentDays = x.AssesmentDays, EndDate = x.EndDate, Holidays = x.Holidays, StartDate = x.StartDate, TermName = x.TermName, TotalDays = x.TotalDays, TotalSatSundays = x.TotalSatSundays, TotalSchoolDays = x.TotalSchoolDays, YearName = x.YearName }).ToListAsync();
            return View(term);
        }
        [Authorize(Policy = "Term.Create")]
        [HttpPost]
        public async Task<IActionResult> Term(TermVM term)
        {
            var totalDays = term.TermEndDate - term.TermStartDate;
            int SatSun = 0;
            if (term.AreSaturdaysOff)
            {
                SatSun = CountWeekEnds(term.TermStartDate, term.TermEndDate);
            }
            else
            {
                SatSun = CountSundays(term.TermStartDate, term.TermEndDate);
            }
            int Days = (int)totalDays.TotalDays + 1;
            term.AssessmentDays = term.AssessmentDays == null ? 0 : term.AssessmentDays;
            var newTerm = new Term
            {
                TermName = term.TermName,
                StartDate = term.TermStartDate,
                EndDate = term.TermEndDate,
                YearId = term.YearId,
                TotalDays = Days,
                AssesmentDays = term.AssessmentDays,
                TotalSatSun = SatSun,
                TotalSchoolDays = Days - SatSun,
                AssesmentWiseTermDays = Days - (SatSun + term.AssessmentDays),
                AreSaturdaysOff = term.AreSaturdaysOff
            };
            await _repository.AddAsync(newTerm);
            if (await _repository.SaveChanges())
            {
                var LastTerm = _db.terms.OrderBy(x => x.TermId).LastOrDefault();
                var Parentyear = _db.years.Where(x => x.YearId == LastTerm.YearId).FirstOrDefault();
                if (Parentyear.TotalSatSundays == null)
                {
                    Parentyear.TotalSatSundays = 0;
                    Parentyear.TotalSatSundays += LastTerm.TotalSatSun;
                }
                else
                {
                    Parentyear.TotalSatSundays += LastTerm.TotalSatSun;
                }
                if (Parentyear.TotalSchoolDays == null)
                {
                    Parentyear.TotalSchoolDays = 0;
                    Parentyear.TotalSchoolDays += LastTerm.TotalSchoolDays;
                }
                else
                {
                    Parentyear.TotalSchoolDays += LastTerm.TotalSchoolDays;
                }
                if (Parentyear.AssesmentDays == null)
                {
                    Parentyear.AssesmentDays = 0;
                    Parentyear.AssesmentDays += LastTerm.AssesmentDays;
                }
                else
                {
                    Parentyear.AssesmentDays += LastTerm.AssesmentDays;
                }
                if (Parentyear.TotalAssesWiseSchoolDays == null)
                {
                    Parentyear.TotalAssesWiseSchoolDays = 0;
                    Parentyear.TotalAssesWiseSchoolDays += LastTerm.AssesmentWiseTermDays;
                }
                else
                {
                    Parentyear.TotalAssesWiseSchoolDays += LastTerm.AssesmentWiseTermDays;
                }
                if (Parentyear.TotalDays == null)
                {
                    Parentyear.TotalDays = 0;
                    Parentyear.TotalDays += LastTerm.TotalDays;
                }
                else
                {
                    Parentyear.TotalDays += LastTerm.TotalDays;
                }
                await _repository.UpdateAsync(Parentyear);
                if (await _repository.SaveChanges())
                {
                    return RedirectToAction("Term");
                }
                else
                {
                    ModelState.AddModelError("", "Error While Saving to Database");
                }
            }
            else
            {
                ModelState.AddModelError("", "Error While Saving to Database");
            }
            return View(term);
        }
        [Authorize(Policy = "Term.Update")]
        [HttpGet]
        public async Task<IActionResult> UpdateTerm(int id)
        {
            ViewBag.Years = await _db.years.Where(x => x.IsActive == true).ToListAsync();
            var term = await _repository.GetTermById(id);
            var Term = new UpdateTermVM
            {
                TermName = term.TermName,
                TermId = term.TermId,
                YearId = (int)term.YearId,
                StartDate = (DateTime)term.StartDate,
                EndDate = (DateTime)term.EndDate,
                AssessmentDays = term.AssesmentDays,
                IsActive = (bool)term.IsActive,
                AreSaturdaysOff = (bool)term.AreSaturdaysOff
            };
            return View(Term);
        }
        [Authorize(Policy = "Term.Update")]
        [HttpPost]
        public async Task<IActionResult> UpdateTerm(UpdateTermVM Uterm)
        {
            var totalDays = Uterm.EndDate - Uterm.StartDate;
            int SatSun = 0;
            if (Uterm.AreSaturdaysOff)
            {
                SatSun = CountWeekEnds(Uterm.StartDate, Uterm.EndDate);
            }
            else
            {
                SatSun = CountSundays(Uterm.StartDate, Uterm.EndDate);
            }
            int Days = (int)totalDays.TotalDays + 1;
            var term = await _repository.GetTermById(Uterm.TermId);
            if (Uterm.AssessmentDays == null) Uterm.AssessmentDays = 0;
            term.StartDate = Uterm.StartDate;
            term.EndDate = Uterm.EndDate;
            term.YearId = Uterm.YearId;
            term.TermName = Uterm.TermName;
            term.IsActive = (bool)Uterm.IsActive;
            term.AreSaturdaysOff = Uterm.AreSaturdaysOff;
            //var LastTerm = _db.terms.OrderBy(x => x.TermId).LastAsync();
            var ParentYear = _db.years.Where(x => x.YearId == term.YearId).FirstOrDefault();
            if (term.TotalSatSun < SatSun)
            {
                ParentYear.TotalSatSundays -= term.TotalSatSun;
                term.TotalSatSun += (SatSun - term.TotalSatSun);
                ParentYear.TotalSatSundays += term.TotalSatSun;
            }
            else
            {
                ParentYear.TotalSatSundays -= term.TotalSatSun;
                term.TotalSatSun -= (term.TotalSatSun - SatSun);
                ParentYear.TotalSatSundays += term.TotalSatSun;
            }
            term.TotalSatSun = SatSun;
            if (term.AssesmentDays < Uterm.AssessmentDays)
            {
                ParentYear.AssesmentDays -= term.AssesmentDays;
                term.AssesmentDays += (Uterm.AssessmentDays - term.AssesmentDays);
                ParentYear.AssesmentDays += term.AssesmentDays;
            }
            else
            {
                ParentYear.AssesmentDays -= term.AssesmentDays;
                term.AssesmentDays -= term.AssesmentDays - Uterm.AssessmentDays;
                ParentYear.AssesmentDays += term.AssesmentDays;
            }
            term.AssesmentDays = Uterm.AssessmentDays;
            if (term.TotalDays < Days)
            {
                ParentYear.TotalDays -= term.TotalDays;
                term.TotalDays += (Days - term.TotalDays);
                ParentYear.TotalDays += term.TotalDays;
            }
            else
            {
                ParentYear.TotalDays -= term.TotalDays;
                term.TotalDays -= (term.TotalDays - Days);
                ParentYear.TotalDays += term.TotalDays;
            }
            term.TotalDays = Days;
            if (term.TotalSchoolDays < (Days - SatSun))
            {
                ParentYear.TotalSchoolDays -= term.TotalSchoolDays;
                term.TotalSchoolDays += ((Days - SatSun) - term.TotalSchoolDays);
                ParentYear.TotalSchoolDays += term.TotalSchoolDays;
            }
            else
            {
                ParentYear.TotalSchoolDays -= term.TotalSchoolDays;
                term.TotalSchoolDays -= (term.TotalSchoolDays - (Days - SatSun));
                ParentYear.TotalSchoolDays += term.TotalSchoolDays;
            }
            term.TotalSchoolDays = Days - SatSun;
            ParentYear.TotalAssesWiseSchoolDays = ParentYear.TotalSchoolDays - ParentYear.AssesmentDays;
            await _repository.UpdateAsync(term);
            await _repository.UpdateAsync(ParentYear);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Term", new { id = Uterm.YearId });
            }
            ModelState.AddModelError("", "Error While Saving to Database");
            return View(Uterm);
        }
        [Authorize(Policy = "Term.Delete")]
        [HttpGet]
        public async Task<IActionResult> DeleteTerm(int id, int YearId)
        {
            var term = await _db.terms.Where(x => x.IsActive == true).FirstOrDefaultAsync();
            if (term == null)
            {
                return NotFound();
            }
            var year = _db.years.Where(x => x.YearId == term.YearId).FirstOrDefault();
            var TermHolidays = _db.Holidays.Where(x => x.TermId == id).ToList();
            foreach (var holiday in TermHolidays)
            {
                //year.TotalSchoolDays += holiday.NoOfHolidays;
                //year.TotalAssesWiseSchoolDays += holiday.NoOfHolidays;
                year.Holidays -= holiday.NoOfHolidays;
                holiday.IsActive = false;
                await _repository.UpdateAsync(holiday);
            }
            year.TotalDays -= term.TotalDays;
            year.TotalSchoolDays -= term.TotalSchoolDays;
            year.TotalSatSundays -= term.TotalSatSun;
            year.AssesmentDays -= term.AssesmentDays;
            year.TotalAssesWiseSchoolDays -= (term.TotalSchoolDays - term.AssesmentDays);
            term.IsActive = false;
            await _repository.UpdateAsync(term);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Term", new { id = YearId });
            }
            return RedirectToAction("Term", new { id = YearId });
        }
        #endregion

        #region Month
        //[HttpGet]
        //public async Task<IActionResult> Month(int Id)
        //{
        //    var term = _db.terms.Where(x => x.TermId == Id).FirstOrDefaultAsync();
        //    var months = from m in _db.months
        //                 from t in _db.terms
        //                 where m.TermId == Id && m.TermId == t.TermId
        //                 select new
        //                 {
        //                     TermName = t.TermName,
        //                     StartDate = m.StartDate,
        //                     EndDate = m.EndDate,
        //                     TotalDays = m.TotalDays,
        //                     TotalSatSundays = m.TotalSatSundays,
        //                     Event = m.Event,
        //                     EventDate = m.EventDate,
        //                     Holidays = m.Holidays,
        //                     AssessmentDays = m.AssesmentDays,
        //                     TotalSchoolDays = m.TotalSchoolDays,
        //                     MonthId = m.MonthId
        //                 };
        //    ViewBag.TermName = term.TermName;
        //    ViewBag.Months = months;
        //    MonthVM month = new MonthVM();
        //    month.TermId = Id;
        //    //if(months.Any())
        //    //{
        //    //    month.MinDate = _db.months.OrderBy(x => x.MonthId).LastOrDefault().EndDate.Value.AddDays(1);
        //    //}
        //    //else
        //    //{
        //    //    month.MinDate = term.StartDate;
        //    //}
        //    month.MinDate = term.StartDate;
        //    month.MaxDate = term.EndDate;
        //    return View(month);
        //}
        //[HttpPost]
        //public async Task<IActionResult> Month(MonthVM month)
        //{
        //    var TotalDays = month.EndDate - month.StartDate;
        //    int satSundays = CountWeekEnds((DateTime)month.StartDate, (DateTime)month.EndDate);
        //    int days = ((int)TotalDays.Value.TotalDays) + 1;
        //    if (month.Holidays == null) month.Holidays = 0;
        //    if (month.AssessmentDays == null) month.AssessmentDays = 0;
        //    var newMonth = new Month
        //    {
        //        TermId = month.TermId,
        //        Holidays = month.Holidays,
        //        Event = month.Event,
        //        EventDate = month.EventDate,
        //        StartDate = month.StartDate,
        //        EndDate = month.EndDate,
        //        TotalDays = days,
        //        TotalSatSundays = satSundays,
        //        AssesmentDays = month.AssessmentDays,
        //        TotalSchoolDays = days - (satSundays + month.Holidays + month.AssessmentDays)
        //    };
        //    await _repository.AddAsync(newMonth);
        //    if (await _repository.SaveChanges())
        //    {
        //        var lastmonth = _db.months.OrderBy(x => x.MonthId).LastOrDefault();
        //        var term = _db.terms.Where(x => x.TermId == lastmonth.TermId).FirstOrDefault();
        //        //var months = _db.months.Where(x => x.TermId == term.TermId);
        //        var year = _db.years.Where(x => x.YearId == term.YearId).FirstOrDefault();
        //        var months = _db.months.Where(x => x.TermId == lastmonth.TermId)
        //            .Select(x => new
        //            {
        //                x.TotalDays,
        //                x.TotalSchoolDays,
        //                x.Holidays,
        //                x.TotalSatSundays,
        //                x.AssesmentDays
        //            }).ToListAsync();
        //        int? termTDays = 0;
        //        int? termTSchoolDays = 0;
        //        int? termTSatSunDays = 0;
        //        int? termTHolidays = 0;
        //        int? termTAssessmentDays = 0;
        //        foreach (var TermMonth in months)
        //        {
        //            if (termTDays != null ) termTDays += TermMonth.TotalDays;
        //            if (termTSchoolDays != null) termTSchoolDays += TermMonth.TotalSchoolDays;
        //            if (termTSatSunDays != null) termTSatSunDays += TermMonth.TotalSatSundays;
        //            if (termTHolidays != null) termTHolidays += TermMonth.Holidays;
        //            if (termTAssessmentDays != null) termTAssessmentDays += TermMonth.AssesmentDays;
        //        }
        //        term.TotalDays = termTDays;
        //        term.TotalSchoolDays = termTSchoolDays;
        //        term.TotalSatSun = termTSatSunDays;
        //        term.TermHolidays = termTHolidays;
        //        term.AssesmentDays = termTAssessmentDays;
        //        await _repository.UpdateAsync(term);
        //        await _repository.SaveChanges();
        //        var YearTerms = _db.terms.Where(x => x.YearId == term.YearId)
        //            .Select(x => new
        //            {
        //                x.TotalDays,
        //                x.TotalSchoolDays,
        //                x.TotalSatSun,
        //                x.TermHolidays,
        //                x.AssesmentDays
        //            }).ToList();
        //        int? yearTdays = 0;
        //        int? yearTSchooldays = 0;
        //        int? yearTSatSunDays = 0;
        //        int? yearHolidays = 0;
        //        int? yearAssesmentDays = 0;
        //        foreach (var TermYear in YearTerms)
        //        {
        //            if(TermYear.TotalDays != null) yearTdays += TermYear.TotalDays;
        //            if(TermYear.TotalSchoolDays != null) yearTSchooldays += TermYear.TotalSchoolDays;
        //            if(TermYear.TotalSatSun != null) yearTSatSunDays += TermYear.TotalSatSun;
        //            if(TermYear.TermHolidays != null) yearHolidays += TermYear.TermHolidays;
        //            if(TermYear.AssesmentDays != null) yearAssesmentDays += TermYear.AssesmentDays;
        //        }
        //        year.TotalDays = yearTdays;
        //        year.TotalSchoolDays = yearTSchooldays;
        //        year.TotalSatSundays = yearTSatSunDays;
        //        year.Holidays = yearHolidays;
        //        year.AssesmentDays = yearAssesmentDays;
        //        year.TotalAssesWiseSchoolDays = year.TotalSchoolDays - year.AssesmentDays;
        //        await _repository.UpdateAsync(year);
        //        await _repository.SaveChanges();
        //        return RedirectToAction("Month");
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("", "Error While Saving to Database");
        //    }
        //    return View(month);
        //}

        //[HttpGet]
        //public async Task<IActionResult> UpdateMonth(int id)
        //{
        //    ViewBag.Terms = _db.terms.Select(x => new { TermId = x.TermId, TermName = x.TermName }).ToListAsync();
        //    var Month = await _repository.GetMonthById(id);
        //    //if(Month.EventDate == null)
        //    //{
        //    //    Month.EventDate = DateTime.Now;
        //    //}
        //    var Umonth = new UpdateMonthVM
        //    {
        //        MonthId = Month.MonthId,
        //        TermId = Month.TermId,
        //        StartDate = (DateTime)Month.StartDate,
        //        EndDate = (DateTime)Month.EndDate,
        //        Event = Month.Event,
        //        EventDate = Month.EventDate,
        //        Holidays = (int)Month.Holidays,
        //        AssesmentDays = Month.AssesmentDays
        //    };
        //    return View(Umonth);
        //}
        //[HttpPost]
        //public async Task<IActionResult> UpdateMonth(UpdateMonthVM Umonth)
        //{
        //    var month = await _repository.GetMonthById(Umonth.MonthId);
        //    var TotalDays = month.EndDate - month.StartDate;
        //    int days = (int)TotalDays.Value.TotalDays + 1;
        //    int SatSun = CountWeekEnds((DateTime)month.StartDate, (DateTime)month.EndDate);
        //    var term = _db.terms.Where(x => x.TermId == month.TermId).FirstOrDefaultAsync();
        //    var year = _db.years.Where(x => x.YearId == term.YearId).FirstOrDefaultAsync();
        //    if (month.Holidays < Umonth.Holidays)
        //    {
        //        year.Holidays -= term.TermHolidays;
        //        term.TermHolidays += (Umonth.Holidays - month.Holidays);
        //        year.Holidays += term.TermHolidays;
        //    }
        //    else
        //    {
        //        year.Holidays -= term.TermHolidays;
        //        term.TermHolidays -= (month.Holidays - Umonth.Holidays);
        //        year.Holidays += term.TermHolidays;
        //    }
        //    month.Holidays = Umonth.Holidays;
        //    month.TermId = Umonth.TermId;
        //    month.Event = Umonth.Event;
        //    month.EventDate = Umonth.EventDate;
        //    month.StartDate = Umonth.StartDate;
        //    month.EndDate = Umonth.EndDate;
        //    if (month.TotalDays < days)
        //    {
        //        year.TotalDays -= term.TotalDays;
        //        term.TotalDays += (days - month.TotalDays);
        //        year.TotalDays += term.TotalDays;
        //    }
        //    else
        //    {
        //        year.TotalDays -= term.TotalDays;
        //        term.TotalDays -= (month.TotalDays - days);
        //        year.TotalDays += term.TotalDays;
        //    }
        //    month.TotalDays = days;
        //    if (month.AssesmentDays < Umonth.AssesmentDays)
        //    {
        //        year.AssesmentDays -= term.AssesmentDays;
        //        term.AssesmentDays += (Umonth.AssesmentDays - month.AssesmentDays);
        //        year.AssesmentDays += term.AssesmentDays;
        //    }
        //    else
        //    {
        //        year.AssesmentDays -= term.AssesmentDays;
        //        term.AssesmentDays -= month.AssesmentDays - Umonth.AssesmentDays;
        //        year.AssesmentDays += term.AssesmentDays;
        //    }
        //    month.AssesmentDays = Umonth.AssesmentDays;
        //    if (month.TotalSatSundays < SatSun)
        //    {
        //        year.TotalSatSundays -= term.TotalSatSun;
        //        term.TotalSatSun += (SatSun - month.TotalSatSundays);
        //        year.TotalSatSundays += term.TotalSatSun;
        //    }
        //    else
        //    {
        //        year.TotalSatSundays -= term.TotalSatSun;
        //        term.TotalSatSun -= (month.TotalSatSundays - SatSun);
        //        year.TotalSatSundays += term.TotalSatSun;
        //    }
        //    month.TotalSatSundays = SatSun;
        //    int totalsDays = (int)(days - (SatSun + month.Holidays + month.AssesmentDays));
        //    if (month.TotalSchoolDays < totalsDays)
        //    {
        //        year.TotalSchoolDays -= term.TotalSchoolDays;
        //        term.TotalSchoolDays += (totalsDays - month.TotalSchoolDays);
        //        year.TotalSchoolDays += term.TotalSchoolDays;
        //    }
        //    else
        //    {
        //        year.TotalSchoolDays -= term.TotalSchoolDays;
        //        term.TotalSchoolDays -= (month.TotalSchoolDays - totalsDays);
        //        year.TotalSchoolDays += term.TotalSchoolDays;
        //    }
        //    month.TotalSchoolDays = totalsDays;
        //    await _repository.UpdateAsync(month);
        //    await _repository.UpdateAsync(term);
        //    await _repository.UpdateAsync(year);
        //    if (await _repository.SaveChanges())
        //    {
        //        return RedirectToAction("Month", new { id = Umonth.TermId });
        //    }
        //    ModelState.AddModelError("", "Error While Saving to Database");
        //    return View(Umonth);
        //}
        //[HttpGet]
        //public async Task<IActionResult> DeleteMonth(int id, int TermId)
        //{
        //    var month = await _repository.GetMonthById(id);
        //    if (month == null)
        //    {
        //        return NotFound();
        //    }
        //    var term = _db.terms.Where(x => x.TermId == month.TermId).FirstOrDefault();
        //    var year = _db.years.Where(x => x.YearId == term.YearId).FirstOrDefault();
        //    term.TotalSchoolDays -= month.TotalSchoolDays;
        //    term.TotalDays -= month.TotalDays;
        //    term.TermHolidays -= month.Holidays;
        //    term.AssesmentDays -= month.AssesmentDays;
        //    term.TotalSatSun -= month.TotalSatSundays;
        //    year.TotalSchoolDays -= month.TotalSchoolDays;
        //    year.TotalDays -= month.TotalDays;
        //    year.TotalSatSundays -= month.TotalSatSundays;
        //    year.Holidays -= month.Holidays;
        //    year.AssesmentDays -= month.AssesmentDays;
        //    await _repository.UpdateAsync(term);
        //    await _repository.SaveChanges();
        //    await _repository.Delete(month);
        //    if (await _repository.SaveChanges())
        //    {
        //        return RedirectToAction("Month", new { Id = TermId });
        //    }
        //    return RedirectToAction("Month", new { Id = TermId });
        //}

        public int CountWeekEnds(DateTime startDate, DateTime endDate)
        {
            int weekEndCount = 0;
            if (startDate > endDate)
            {
                DateTime temp = startDate;
                startDate = endDate;
                endDate = temp;
            }
            TimeSpan diff = endDate - startDate;
            int days = diff.Days;
            for (var i = 0; i <= days; i++)
            {
                var testDate = startDate.AddDays(i);
                if (testDate.DayOfWeek == DayOfWeek.Saturday || testDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    weekEndCount += 1;
                }
            }
            return weekEndCount;
        }

        public int CountSundays(DateTime startDate, DateTime endDate)
        {
            int weekEndCount = 0;
            if (startDate > endDate)
            {
                DateTime temp = startDate;
                startDate = endDate;
                endDate = temp;
            }
            TimeSpan diff = endDate - startDate;
            int days = diff.Days;
            for (var i = 0; i <= days; i++)
            {
                var testDate = startDate.AddDays(i);
                if (testDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    weekEndCount += 1;
                }
            }
            return weekEndCount;
        }

        #endregion

        #region Holidays
        [Authorize(Policy = "Term.Read")]
        [HttpGet]
        public async Task<IActionResult> Holidays(int Id, int YearId)
        {
            var holidays = _db.Holidays.Where(x => x.TermId == Id && x.IsActive == true).ToList();
            var term = _db.terms.Where(x => x.TermId == Id).FirstOrDefault();
            HolidaysVM holidaysVM = new HolidaysVM
            {
                MinDate = term.StartDate,
                MaxDate = term.EndDate,
                Holidays = holidays,
                TermId = Id
            };
            holidaysVM.YearId = YearId;
            ViewBag.TermName = term.TermName;
            return View(holidaysVM);
        }
        [Authorize(Policy = "Term.Create")]
        [HttpPost]
        public async Task<IActionResult> Holidays(HolidaysVM data)
        {
            var newHolidays = new Holiday
            {
                TermId = data.TermId,
                HolidayName = data.HolidayName,
                StartDate = data.StartDate,
                EndDate = data.EndDate,
                IsSchoolOff = data.IsSchoolOff
            };
            if (data.IsSchoolOff)
            {
                var holidaysCount = data.EndDate - data.StartDate;
                newHolidays.NoOfHolidays = (int)holidaysCount.Value.TotalDays + 1;
            }
            await _db.AddAsync(newHolidays);
            if (await _repository.SaveChanges())
            {
                var LastHoliday = _db.Holidays.OrderBy(x => x.HolidayId).LastOrDefault();
                if ((bool)LastHoliday.IsSchoolOff)
                {
                    var term = _db.terms.Where(x => x.TermId == data.TermId).FirstOrDefault();
                    var year = _db.years.Where(x => x.YearId == data.YearId).FirstOrDefault();
                    if (term.TermHolidays != null)
                    {
                        term.TermHolidays += LastHoliday.NoOfHolidays;
                    }
                    else
                    {
                        term.TermHolidays = 0;
                        term.TermHolidays += LastHoliday.NoOfHolidays;
                    }
                    term.TotalSchoolDays -= LastHoliday.NoOfHolidays;
                    term.AssesmentWiseTermDays -= LastHoliday.NoOfHolidays;
                    if (year.Holidays != null)
                    {
                        year.Holidays += LastHoliday.NoOfHolidays;
                    }
                    else
                    {
                        year.Holidays = 0;
                        year.Holidays += LastHoliday.NoOfHolidays;
                    }
                    year.TotalSchoolDays -= LastHoliday.NoOfHolidays;
                    year.TotalAssesWiseSchoolDays -= LastHoliday.NoOfHolidays;
                    await _repository.UpdateAsync(term);
                    await _repository.UpdateAsync(year);
                    if (await _repository.SaveChanges())
                    {
                        return RedirectToAction("Term", new { Id = data.YearId });
                    }
                    ModelState.AddModelError("", "Error While Saving to Database");
                }
                return RedirectToAction("Term", new { Id = data.YearId });
            }
            ModelState.AddModelError("", "Error While Saving to Database");
            return View(data);
        }
        [Authorize(Policy = "Holidays.Delete")]
        [HttpGet]
        public async Task<IActionResult> DeleteHoliday(int Id, int YearId)
        {
            var holiday = _db.Holidays.Where(x => x.HolidayId == Id).FirstOrDefault();
            if (holiday == null)
            {
                return NotFound();
            }
            var term = _db.terms.Where(x => x.TermId == holiday.TermId).FirstOrDefault();
            var year = _db.years.Where(x => x.YearId == YearId).FirstOrDefault();
            if ((bool)holiday.IsSchoolOff)
            {
                term.TermHolidays -= holiday.NoOfHolidays;
                term.TotalSchoolDays += holiday.NoOfHolidays;
                term.AssesmentWiseTermDays += holiday.NoOfHolidays;
                year.Holidays -= holiday.NoOfHolidays;
                year.TotalSchoolDays += holiday.NoOfHolidays;
                year.TotalAssesWiseSchoolDays += holiday.NoOfHolidays;
                await _repository.UpdateAsync(term);
                await _repository.UpdateAsync(year);
            }
            holiday.IsActive = false;
            await _repository.UpdateAsync(holiday);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Term", new { id = YearId });
            }
            return RedirectToAction("Term", new { id = YearId });
        }
        #endregion

        #region UnitAllocation
        [Authorize(Policy = "Academic Planning")]
        [HttpGet]
        public async Task<IActionResult> UnitAllocation(int Id, int YearId, UnitAllocationVM SearchUnit)
        {
            int empId = Convert.ToInt16(User.FindFirst(ClaimTypes.Sid)?.Value);
            ViewBag.Plans = (from a in _db.AcademicPlannings
                             from b in _db.Sections
                             from c in _db.Grades
                             from d in _db.Books
                             where a.EmployeeId == empId && a.IsActive == true && b.SectionId == a.ClassId && c.GradeId == b.GradeId && d.BookId == a.BookId
                             select new
                             {
                                 PlanId = a.AcademicPlanningsId,
                                 PlanName = a.PlanName,
                                 ClassName = c.GradeName + b.SectionName,
                                 BookName = d.BookName
                             }).Distinct().ToList();
            SearchUnit.TermId = Id;
            SearchUnit.YearId = YearId;
            ViewBag.WorkBooks = _db.Books.Where(x => x.IsWorkBook == true).ToList();
            ViewBag.TeachingClasses = await (from a in _db.SubjectTeacherAllocations
                                             from b in _db.Books
                                             from c in _db.Sections
                                             from d in _db.Grades
                                             where a.EmployeeId == empId && b.BookId == a.BookId && c.SectionId == a.SectionId && d.GradeId == c.GradeId
                                             select new
                                             {
                                                 ClassId = c.SectionId,
                                                 ClassName = d.GradeName + c.SectionName
                                             }).Distinct().ToListAsync();
            if (SearchUnit.PlanId != null && SearchUnit.PlanId != 0)
            {
                var plan = await _db.AcademicPlannings.Where(x => x.AcademicPlanningsId == SearchUnit.PlanId).FirstOrDefaultAsync();
                SearchUnit.CopyablePlans = await _db.AcademicPlannings.Where(x => x.BookId == plan.BookId && x.EmployeeId == empId && x.ClassId != plan.ClassId && x.IsActive == true).ToListAsync();
                if (SearchUnit.CopyablePlans.Any())
                {
                    SearchUnit.IsPlanCopiable = true;
                }
                ViewBag.BookName = (await (from a in _db.Books
                                           where a.BookId == plan.BookId && a.IsActive == true
                                           select a.BookName).FirstOrDefaultAsync());
                ViewBag.CalendarName = (await (from a in _db.years
                                               where a.YearId == YearId
                                               select a.YearName).FirstOrDefaultAsync());
                ViewBag.TermName = (await (from a in _db.terms
                                           where a.TermId == Id
                                           select a.TermName).FirstOrDefaultAsync());
                var AllUnits = (from c in _db.Units
                                from d in _db.Books
                                where c.BookId == plan.BookId && d.BookId == plan.BookId && c.IsActive == true
                                select new UnitList
                                {
                                    UnitName = c.UnitName,
                                    UnitId = c.UnitId,
                                    BookName = d.BookName
                                }).Distinct().OrderBy(x => x.UnitName).ToList();
                var AllocatedUnits = _db.UnitAllocations.Where(x => x.TermId == Id && x.PlanId == SearchUnit.PlanId).ToList();
                var month = await _repository.GetTermById(Id);
                ViewBag.Month = month.TermName;
                SearchUnit.MinDate = month.StartDate;
                SearchUnit.MaxDate = month.EndDate;
                SearchUnit.Units = AllUnits.Select(x => new UnitList { UnitId = x.UnitId, UnitName = x.UnitName, BookName = x.BookName }).ToList();
                foreach (var unit in SearchUnit.Units)
                {
                    foreach (var allocatedUnit in AllocatedUnits)
                    {
                        if (unit.UnitId == allocatedUnit.UnitId)
                        {
                            unit.preAllocation = true;
                            unit.StartDate = allocatedUnit.StartDate;
                            unit.EndDate = allocatedUnit.EndDate;
                            unit.WorkBookId = allocatedUnit.WorkBookId;
                            unit.WorkBookStartPage = allocatedUnit.WorkBookStartPage;
                            unit.WorkBookEndPage = allocatedUnit.WorkBookEndPage;
                        }
                    }
                }
                return View(SearchUnit);
            }
            return View(SearchUnit);
        }
        [Authorize(Policy = "Academic Planning")]
        [HttpPost]
        public async Task<IActionResult> UnitAllocation(UnitAllocationVM data)
        {
            int empId = Convert.ToInt16(User.FindFirst(ClaimTypes.Sid)?.Value);
            var UnitAllocationsToRemove = await (from a in _db.UnitAllocations
                                                 where a.TermId == data.TermId && a.PlanId == data.PlanId && a.UnitId == data.UnitId
                                                 select a).ToListAsync();
            _db.RemoveRange(UnitAllocationsToRemove);
            await _db.SaveChangesAsync();
            List<UnitAllocation> allocations = new List<UnitAllocation>();
            var selectedUnits = data.Units.Where(x => x.IsSelected == true && x.StartDate != null && x.EndDate != null).ToList();
            var unSelectedUnits = data.Units.Where(x => x.IsSelected == false).ToList();
            if (unSelectedUnits.Any())
            {
                var allocatedUnits = from a in unSelectedUnits
                                     from b in _db.UnitAllocations
                                     where b.UnitId == a.UnitId && b.TermId == data.TermId && b.PlanId == data.PlanId
                                     select b;
                _db.RemoveRange(allocatedUnits);
                var allocatedChapters = from a in unSelectedUnits
                                        from b in _db.ChapterAllocations
                                        where b.UnitId == a.UnitId && b.TermId == data.TermId && b.PlanId == data.PlanId
                                        select b;
                _db.RemoveRange(allocatedChapters);
                var allocatdTopics = from a in allocatedChapters
                                     from b in _db.TopicAllocations
                                     where b.ChapterId == a.ChapterId && b.TermId == data.TermId && b.PlanId == data.PlanId
                                     select b;
                _db.RemoveRange(allocatdTopics);
                var allocatedSubtopic = from a in allocatdTopics
                                        from b in _db.SubTopicAllocations
                                        where b.TopicId == a.TopicId && b.TermId == data.TermId && b.PlanId == data.PlanId
                                        select b;
                _db.RemoveRange(allocatedSubtopic);
                await _db.SaveChangesAsync();
            }
            int SectionId = (int)await _db.AcademicPlannings.Where(x => x.AcademicPlanningsId == data.PlanId && x.IsActive == true).Select(x => x.ClassId).FirstOrDefaultAsync();
            foreach (var chapter in selectedUnits)
            {
                var newUnitAllocation = new UnitAllocation
                {
                    UnitId = chapter.UnitId,
                    StartDate = chapter.StartDate,
                    EndDate = chapter.EndDate,
                    TermId = data.TermId,
                    WorkBookId = chapter.WorkBookId,
                    WorkBookStartPage = chapter.WorkBookStartPage,
                    WorkBookEndPage = chapter.WorkBookEndPage,
                    SectionId = SectionId,
                    PlanId = data.PlanId,
                    AreSaturdaysOff = data.AreSaturdaysOff
                };
                allocations.Add(newUnitAllocation);
            }
            if (allocations.Any())
            {
                await _db.AddRangeAsync(allocations);
                if (await _repository.SaveChanges())
                {
                    return RedirectToAction("Term", new { Id = data.YearId });
                }
                ModelState.AddModelError("", "Error While Saving to Database");
                return View(data);
            }

            return RedirectToAction("Term", new { Id = data.YearId });
        }
        #endregion

        #region ChapterAllocation
        [Authorize(Policy = "Academic Planning")]
        [HttpGet]
        public async Task<IActionResult> ChapterAllocation(int Id, int YearId, ChapterAllocationVM SearchChapter)
        {
            int empId = Convert.ToInt16(User.FindFirst(ClaimTypes.Sid)?.Value);
            ViewBag.Plans = (from a in _db.AcademicPlannings
                             from b in _db.Sections
                             from c in _db.Grades
                             from d in _db.Books
                             where a.EmployeeId == empId && a.IsActive == true && b.SectionId == a.ClassId && c.GradeId == b.GradeId && d.BookId == a.BookId
                             select new
                             {
                                 PlanId = a.AcademicPlanningsId,
                                 PlanName = a.PlanName,
                                 ClassName = c.GradeName + b.SectionName,
                                 BookName = d.BookName
                             }).Distinct().ToList();
            SearchChapter.TermId = Id;
            SearchChapter.YearId = YearId;
            ViewBag.TeachingClasses = await (from a in _db.SubjectTeacherAllocations
                                             from b in _db.Books
                                             from c in _db.Sections
                                             from d in _db.Grades
                                             where a.EmployeeId == empId && b.BookId == a.BookId && c.SectionId == a.SectionId && d.GradeId == c.GradeId
                                             select new
                                             {
                                                 ClassId = c.SectionId,
                                                 ClassName = d.GradeName + c.SectionName
                                             }).Distinct().ToListAsync();
            if (SearchChapter.UnitId != null && SearchChapter.UnitId != 0)
            {
                ViewBag.UnitName = (await (from a in _db.Units
                                           where a.UnitId == SearchChapter.UnitId
                                           select a.UnitName).FirstOrDefaultAsync());
                ViewBag.BookName = (await (from a in _db.Units
                                           from b in _db.Books
                                           where a.UnitId == SearchChapter.UnitId && a.BookId == b.BookId
                                           select b.BookName).FirstOrDefaultAsync());
                ViewBag.CalendarName = (await (from a in _db.years
                                               where a.YearId == YearId
                                               select a.YearName).FirstOrDefaultAsync());
                ViewBag.TermName = (await (from a in _db.terms
                                           where a.TermId == Id
                                           select a.TermName).FirstOrDefaultAsync());
                var AllChapters = await (from a in _db.UnitAllocations
                                         from chapter in _db.chapters
                                         from unit in _db.Units
                                         where chapter.UnitId == SearchChapter.UnitId && unit.UnitId == SearchChapter.UnitId && a.TermId == Id && a.UnitId == SearchChapter.UnitId
                                         select new ChapterList
                                         {
                                             ChapterName = chapter.ChapterName,
                                             ChapterId = chapter.ChapterId,
                                             UnitId = (int)chapter.UnitId,
                                             UnitName = unit.UnitName,
                                             WBMaxPage = a.WorkBookEndPage,
                                             WBMinPage = a.WorkBookStartPage
                                         }).Distinct().OrderBy(x => x.ChapterName).ToListAsync();
                ////List < ChapterList > AllChapters = _db.chapters.Select(x => new ChapterList { ChapterName = x.ChapterName, ChapterId = x.ChapterId }).ToListAsync();
                ////ChapterAllocationVM ChapterVM = new ChapterAllocationVM();

                var ParentUnit = await _db.UnitAllocations.Where(x => x.TermId == Id && x.UnitId == SearchChapter.UnitId && x.PlanId == SearchChapter.PlanId && x.IsActive == true).FirstOrDefaultAsync();
                SearchChapter.MinDate = ParentUnit != null ? ParentUnit.StartDate : DateTime.Now;
                SearchChapter.MaxDate = ParentUnit != null ? ParentUnit.EndDate : DateTime.Now;
                SearchChapter.Chapters = AllChapters.Select(x => new ChapterList { ChapterId = x.ChapterId, ChapterName = x.ChapterName, UnitId = x.UnitId, UnitName = x.UnitName, WBMinPage = x.WBMinPage, WBMaxPage = x.WBMaxPage }).Distinct().ToList();
                var AllocatedChapters = _db.ChapterAllocations.Where(x => x.TermId == Id && x.PlanId == SearchChapter.PlanId).Distinct().ToList();
                foreach (var chapter in SearchChapter.Chapters)
                {
                    foreach (var allocatedChapter in AllocatedChapters)
                    {
                        if (chapter.ChapterId == allocatedChapter.ChapterId)
                        {
                            chapter.preAllocation = true;
                            chapter.StartDate = allocatedChapter.StartDate;
                            chapter.EndDate = allocatedChapter.EndDate;
                            chapter.WorkBookId = allocatedChapter.WorkBookId;
                            chapter.WorkBookStartPage = allocatedChapter.WorkBookStartPage;
                            chapter.WorkBookEndPage = allocatedChapter.WorkBookEndPage;
                        }
                    }
                }
                var month = await _repository.GetTermById(Id);
                ViewBag.Month = month.TermName;
                return View(SearchChapter);
            }
            return View(SearchChapter);
        }
        [Authorize(Policy = "Academic Planning")]
        [HttpPost]
        public async Task<IActionResult> ChapterAllocation(ChapterAllocationVM data)
        {
            var result = _db.ChapterAllocations
            .Where(p => p.TermId == data.TermId && p.UnitId == data.UnitId && p.PlanId == data.PlanId).ToList();
            _db.RemoveRange(result);
            await _db.SaveChangesAsync();
            List<ChapterAllocation> allocations = new List<ChapterAllocation>();
            var selectedChapters = data.Chapters.Where(x => x.IsSelected == true && x.StartDate != null && x.EndDate != null).ToList();
            var unSelectedChapters = data.Chapters.Where(x => x.IsSelected == false).ToList();
            if (unSelectedChapters.Any())
            {
                var allocatdTopics = from a in unSelectedChapters
                                     from b in _db.TopicAllocations
                                     where b.ChapterId == a.ChapterId && b.PlanId == data.PlanId
                                     select b;
                _db.RemoveRange(allocatdTopics);
                var allocatedSubtopic = from a in allocatdTopics
                                        from b in _db.SubTopicAllocations
                                        where b.TopicId == a.TopicId && b.PlanId == data.PlanId
                                        select b;
                _db.RemoveRange(allocatedSubtopic);
                await _db.SaveChangesAsync();
            }
            foreach (var chapter in selectedChapters)
            {
                var newChapterAllocation = new ChapterAllocation
                {
                    ChapterId = chapter.ChapterId,
                    TermId = data.TermId,
                    StartDate = chapter.StartDate,
                    EndDate = chapter.EndDate,
                    UnitId = chapter.UnitId,
                    WorkBookId = chapter.WorkBookId,
                    WorkBookStartPage = chapter.WorkBookStartPage,
                    WorkBookEndPage = chapter.WorkBookEndPage,
                    PlanId = data.PlanId,
                    AreSaturdaysOff = data.AreSaturdaysOff
                };
                allocations.Add(newChapterAllocation);
            }
            if (allocations.Any())
            {
                await _db.AddRangeAsync(allocations);
                if (await _repository.SaveChanges())
                {
                    return RedirectToAction("Term", new { Id = data.YearId });
                }
                ModelState.AddModelError("", "Error While Saving to Database");
                return View(data);
            }

            return RedirectToAction("Term", new { Id = data.YearId });
        }
        #endregion

        #region TopicAllocation
        [Authorize(Policy = "Academic Planning")]
        [HttpGet]
        public async Task<IActionResult> SearchTopics(int Id, int YearId, TopicAllocationTestVM SearchTopics)
        {
            int empId = Convert.ToInt16(User.FindFirst(ClaimTypes.Sid)?.Value);
            ViewBag.Plans = (from a in _db.AcademicPlannings
                             from b in _db.Sections
                             from c in _db.Grades
                             from d in _db.Books
                             where a.EmployeeId == empId && a.IsActive == true && b.SectionId == a.ClassId && c.GradeId == b.GradeId && d.BookId == a.BookId
                             select new
                             {
                                 PlanId = a.AcademicPlanningsId,
                                 PlanName = a.PlanName,
                                 ClassName = c.GradeName + b.SectionName,
                                 BookName = d.BookName
                             }).Distinct().ToList();
            if (SearchTopics.ChapterId != null && SearchTopics.ChapterId != 0)
            {
                ViewBag.Units = await (from a in _db.AcademicPlannings
                                       from b in _db.Units
                                       from c in _db.UnitAllocations
                                       where a.AcademicPlanningsId == SearchTopics.PlanId && b.BookId == a.BookId && c.UnitId == b.UnitId && b.IsActive == true
                                       select b).OrderBy(x => x.UnitName).Distinct().ToListAsync();
                ViewBag.Chapters = await (from a in _db.chapters
                                          from b in _db.UnitAllocations
                                          from c in _db.Units
                                          from e in _db.Books
                                          from f in _db.AcademicPlannings
                                          where a.UnitId == b.UnitId && c.UnitId == b.UnitId && e.BookId == c.BookId && f.BookId == e.BookId && f.AcademicPlanningsId == SearchTopics.PlanId && f.IsActive == true && a.IsActive == true
                                          select a).OrderBy(x => x.ChapterName).Distinct().ToListAsync();
            }
            SearchTopics.TermId = Id;
            SearchTopics.YearId = YearId;
            ViewBag.TeachingClasses = await (from a in _db.SubjectTeacherAllocations
                                             from b in _db.Books
                                             from c in _db.Sections
                                             from d in _db.Grades
                                             where a.EmployeeId == empId && b.BookId == a.BookId && c.SectionId == a.SectionId && d.GradeId == c.GradeId
                                             select new
                                             {
                                                 ClassId = c.SectionId,
                                                 ClassName = d.GradeName + c.SectionName
                                             }).Distinct().ToListAsync();
            ViewBag.WorkBooks = await _db.Books.Where(x => x.IsWorkBook == true).ToListAsync();
            ViewBag.TMethods = await _db.TeachingMethodologies.OrderBy(x => x.TMethodologyName).ToListAsync();
            return View(SearchTopics);
        }

        [Authorize(Policy = "Academic Planning")]
        [HttpGet]
        public async Task<IActionResult> TopicAllocation(int Id, int YearId, TopicAllocationTestVM SearchTopics)
        {
            int empId = Convert.ToInt16(User.FindFirst(ClaimTypes.Sid)?.Value);
            //ViewBag.Plans = await (from a in _db.AcademicPlannings
            //                       from b in _db.Sections
            //                       from c in _db.Grades
            //                       from d in _db.Books
            //                       where a.EmployeeId == empId && a.IsActive == true && b.SectionId == a.ClassId && c.GradeId == b.GradeId && d.BookId == a.BookId
            //                       select new
            //                       {
            //                           PlanId = a.AcademicPlanningsId,
            //                           PlanName = a.PlanName,
            //                           ClassName = c.GradeName + b.SectionName,
            //                           BookName = d.BookName
            //                       }).Distinct().ToListAsync();
            //ViewBag.TeachingClasses = await (from a in _db.SubjectTeacherAllocations
            //                                 from b in _db.Books
            //                                 from c in _db.Sections
            //                                 from d in _db.Grades
            //                                 where a.EmployeeId == empId && b.BookId == a.BookId && c.SectionId == a.SectionId && d.GradeId == c.GradeId
            //                                 select new
            //                                 {
            //                                     ClassId = c.SectionId,
            //                                     ClassName = d.GradeName + c.SectionName
            //                                 }).Distinct().ToListAsync();
            //ViewBag.WorkBooks = await _db.Books.Where(x => x.IsWorkBook == true).ToListAsync();
            //if (SearchTopics.ChapterId != null)
            //{
            ViewBag.CalendarName = (await (from a in _db.years
                                           where a.YearId == YearId
                                           select a.YearName).FirstOrDefaultAsync());
            ViewBag.TermName = (await (from a in _db.terms
                                       where a.TermId == Id
                                       select a.TermName).FirstOrDefaultAsync());
            ViewBag.BookName = (await (from a in _db.Books
                                       from b in _db.Units
                                       from c in _db.chapters
                                       where c.ChapterId == SearchTopics.ChapterId && c.UnitId == b.UnitId && a.BookId == b.BookId
                                       select a.BookName).FirstOrDefaultAsync());
            ViewBag.UnitName = (await (from b in _db.Units
                                       from c in _db.chapters
                                       where c.ChapterId == SearchTopics.ChapterId && c.UnitId == b.UnitId
                                       select b.UnitName).FirstOrDefaultAsync());
            ViewBag.ChapterName = (await (from c in _db.chapters
                                          where c.ChapterId == SearchTopics.ChapterId
                                          select c.ChapterName).FirstOrDefaultAsync());
            ViewBag.TMName = (await (from c in _db.TeachingMethodologies
                                          where c.TeachingMethodologyId == SearchTopics.TeachingMethodologyId
                                          select c.TMethodologyName).FirstOrDefaultAsync());
            var chapter = await _db.ChapterAllocations.Where(x => x.ChapterId == SearchTopics.ChapterId).FirstOrDefaultAsync();
            //List<TopicList> topics = new List<TopicList>();
            ////TopicAllocationVM TopicVM = new TopicAllocationVM();
            //foreach (var chapter in chapters)
            //{
            List<TopicListTest> ChapterTopics = await _db.topics.Where(x => x.ChapterId == SearchTopics.ChapterId).Select(x => new TopicListTest { TopicId = x.TopicId, TopicName = x.TopicName }).Distinct().OrderBy(x => x.TopicName).ToListAsync();
            foreach (var each in ChapterTopics)
            {
                each.WBMaxPage = chapter.WorkBookEndPage;
                each.WBMinPage = chapter.WorkBookStartPage;
            }
            //topics.AddRange(ChapterTopics);
            //}
            SearchTopics.Topics = ChapterTopics;
            var allocatinos = (from t in ChapterTopics
                               from allo in _db.TopicAllocations
                               where allo.TopicId == t.TopicId && allo.PlanId == SearchTopics.PlanId && allo.TeachingMethodologyId == SearchTopics.TeachingMethodologyId
                               select new
                               {
                                   topic = allo.TopicId,
                                   startDate = allo.StartDate,
                                   endDate = allo.EndDate,
                                   TeachingMethodologyId = allo.TeachingMethodologyId,
                                   TMethodDesc = allo.TMethodDesc,
                                   WorkBookId = allo.WorkBookId,
                                   WorkBookStartPage = allo.WorkBookStartPage,
                                   WorkBookEndPage = allo.WorkBookEndPage
                               }).Distinct().ToList();
            foreach (var topic in ChapterTopics)
            {
                //var selectedTopic = _db.TopicAllocations.Where(x => x.TopicId == topic.TopicId).FirstOrDefault();
                //if (selectedTopic != null)
                //{
                //    var selectedChapter = _db.ChapterAllocations.Where(x => x.ChapterId == selectedTopic.ChapterId && x.PlanId == SearchTopics.PlanId && x.TermId == SearchTopics.TermId).FirstOrDefault();
                //    topic.ChapterStartDate = selectedChapter?.StartDate;
                //    topic.ChapterEndDate = selectedChapter?.EndDate;
                //}
                //else
                //{
                //    var nonSelectedTopic = _db.topics.Where(x => x.TopicId == topic.TopicId).FirstOrDefault();
                //    var nonSelectedChapter = _db.ChapterAllocations.Where(x => x.ChapterId == nonSelectedTopic.ChapterId && x.PlanId == SearchTopics.PlanId && x.TermId == SearchTopics.TermId).FirstOrDefault();
                //    topic.ChapterStartDate = nonSelectedChapter?.StartDate;
                //    topic.ChapterEndDate = nonSelectedChapter?.EndDate;
                //}
                foreach (var allo in allocatinos)
                {
                    if (topic.TopicId == allo.topic)
                    {
                        topic.Check = true;
                        topic.StartDate = allo.startDate;
                        topic.EndDate = allo.endDate;
                        topic.TeachingMethodologyDesc = allo.TMethodDesc;
                        topic.WorkBookId = allo.WorkBookId;
                        topic.WorkBookStartPage = allo.WorkBookStartPage;
                        topic.WorkBookEndPage = allo.WorkBookEndPage;
                    }
                }
            }
            var month = await _repository.GetTermById(SearchTopics.TermId);
            ViewBag.Month = month.TermName;
            //var Dates = _db.ChapterAllocations.Where(x => x.TermId == Id && x.ChapterId == SearchTopics.ChapterId).Select(x => new { StartDate = x.StartDate, EndDate = x.EndDate }).ToList();
            //if (_db.ChapterAllocations.ToListAsync().Result.Any()) ;
            var chapAllocation = await _db.ChapterAllocations.Where(x => x.TermId == SearchTopics.TermId && x.PlanId == SearchTopics.PlanId && x.ChapterId == SearchTopics.ChapterId).FirstOrDefaultAsync();
            SearchTopics.MinDate = chapAllocation != null ? chapAllocation.StartDate : DateTime.Now;
            SearchTopics.MaxDate = chapAllocation != null ? chapAllocation.EndDate : DateTime.Now;
            return View(SearchTopics);
            //}
            //    return View(SearchTopics);
        }

        [Authorize(Policy = "Academic Planning")]
        [HttpPost]
        public async Task<IActionResult> TopicAllocation(TopicAllocationTestVM data)
        {
            var oldTopics = _db.TopicAllocations
            .Where(p => p.TermId.Equals(data.TermId) && p.ChapterId == data.ChapterId && p.PlanId == data.PlanId && p.TeachingMethodologyId == data.TeachingMethodologyId).ToList();
            _db.RemoveRange(oldTopics);
            await _db.SaveChangesAsync();
            var test = from a in data.Topics
                       where a.Check == true && a.StartDate != null && a.EndDate != null
                       select a;
            var unSelectedTopics = from c in data.Topics
                                   where c.Check == false
                                   select c;
            if (unSelectedTopics.Any())
            {
                var subTopics = from a in unSelectedTopics
                                from b in _db.SubTopicAllocations
                                where b.TopicId == a.TopicId && b.PlanId == data.PlanId
                                select b;
                if (subTopics.Any())
                {
                    _db.RemoveRange(subTopics);
                    await _db.SaveChangesAsync();
                }
            }
            List<TopicAllocation> allocations = new List<TopicAllocation>();
            foreach (var bid in test)
            {
                var topic = _db.topics.Where(x => x.TopicId == bid.TopicId).FirstOrDefault();
                var chapter = _db.ChapterAllocations.Where(x => x.ChapterId == topic.ChapterId).FirstOrDefault();
                var newTopic = new TopicAllocation
                {
                    TermId = data.TermId,
                    TopicId = bid.TopicId,
                    ChapterId = chapter.ChapterId,
                    StartDate = bid.StartDate,
                    EndDate = bid.EndDate,
                    TeachingMethodologyId = data.TeachingMethodologyId,
                    TMethodDesc = bid.TeachingMethodologyDesc,
                    WorkBookId = bid.WorkBookId,
                    WorkBookEndPage = bid.WorkBookEndPage,
                    WorkBookStartPage = bid.WorkBookStartPage,
                    PlanId = data.PlanId,
                    AreSaturdaysOff = data.AreSaturdaysOff
                };
                allocations.Add(newTopic);
            }
            if (allocations.Any())
            {
                await _db.AddRangeAsync(allocations);
                if (await _repository.SaveChanges())
                {
                    return RedirectToAction("Term", new { Id = data.YearId });
                }
                ModelState.AddModelError("", "Error While Saving to Database");
                return View(data);
            }
            return RedirectToAction("Term", new { Id = data.YearId });
        }
        #endregion

        #region SubTopicAllocation
        [Authorize(Policy = "Academic Planning")]
        [HttpGet]
        public async Task<IActionResult> SubTopicAllocation(int Id, int YearId, SubTopicAllocationVM SearchSubTopics)
        {
            int empId = Convert.ToInt16(User.FindFirst(ClaimTypes.Sid)?.Value);
            ViewBag.Plans = (from a in _db.AcademicPlannings
                             from b in _db.Sections
                             from c in _db.Grades
                             from d in _db.Books
                             where a.EmployeeId == empId && a.IsActive == true && b.SectionId == a.ClassId && c.GradeId == b.GradeId && d.BookId == a.BookId
                             select new
                             {
                                 PlanId = a.AcademicPlanningsId,
                                 PlanName = a.PlanName,
                                 ClassName = c.GradeName + b.SectionName,
                                 BookName = d.BookName
                             }).Distinct().ToList();
            //ViewBag.WorkBooks = _db.Books.Where(x => x.IsWorkBook == true).ToListAsync();
            SearchSubTopics.TermId = Id;
            SearchSubTopics.YearId = YearId;
            ViewBag.TeachingClasses = await (from a in _db.SubjectTeacherAllocations
                                             from b in _db.Books
                                             from c in _db.Sections
                                             from d in _db.Grades
                                             where a.EmployeeId == empId && b.BookId == a.BookId && c.SectionId == a.SectionId && d.GradeId == c.GradeId
                                             select new
                                             {
                                                 ClassId = c.SectionId,
                                                 ClassName = d.GradeName + c.SectionName
                                             }).Distinct().ToListAsync();
            if (SearchSubTopics.TopicId != null && SearchSubTopics.TopicId != 0)
            {
                ViewBag.CalendarName = (await (from a in _db.years
                                               where a.YearId == YearId
                                               select a.YearName).FirstOrDefaultAsync());
                ViewBag.TermName = (await (from a in _db.terms
                                           where a.TermId == Id
                                           select a.TermName).FirstOrDefaultAsync());
                ViewBag.BookName = (await (from a in _db.Books
                                           from b in _db.Units
                                           from c in _db.chapters
                                           from d in _db.topics
                                           where d.TopicId == SearchSubTopics.TopicId && c.ChapterId == d.ChapterId && c.UnitId == b.UnitId && a.BookId == b.BookId
                                           select a.BookName).FirstOrDefaultAsync());
                ViewBag.UnitName = (await (from b in _db.Units
                                           from c in _db.chapters
                                           from d in _db.topics
                                           where d.TopicId == SearchSubTopics.TopicId && c.ChapterId == d.ChapterId && c.UnitId == b.UnitId
                                           select b.UnitName).FirstOrDefaultAsync());
                ViewBag.ChapterName = (await (from c in _db.chapters
                                              from d in _db.topics
                                              where d.TopicId == SearchSubTopics.TopicId && c.ChapterId == d.ChapterId
                                              select c.ChapterName).FirstOrDefaultAsync());
                ViewBag.TopicName = (await (from d in _db.topics
                                            where d.TopicId == SearchSubTopics.TopicId
                                            select d.TopicName).FirstOrDefaultAsync());
                var subTopics = await (from a in _db.ChapterAllocations
                                       from b in _db.TopicAllocations
                                       from c in _db.subTopics
                                       where a.TermId == Id && b.ChapterId == a.ChapterId && c.TopicId == b.TopicId && c.TopicId == SearchSubTopics.TopicId
                                       select c).Distinct().OrderBy(x => x.SubTopicName).ToListAsync();
                //SubTopicAllocationVM subTopic = new SubTopicAllocationVM();
                if (subTopics.Any())
                {
                    var allocatedSubTopics = from a in _db.ChapterAllocations
                                             from b in _db.TopicAllocations
                                             from c in _db.SubTopicAllocations
                                             where a.TermId == Id && b.ChapterId == a.ChapterId && c.TopicId == b.TopicId && c.TopicId == SearchSubTopics.TopicId && c.PlanId == SearchSubTopics.PlanId
                                             select c;
                    List<SubTopicList> subTopicList = subTopics.Select(x => new SubTopicList { SubTopicId = x.SubTopicId, SubTopicName = x.SubTopicName }).ToList();
                    foreach (var topic in subTopicList)
                    {
                        var selectedSubTopic = _db.subTopics.Where(x => x.SubTopicId == topic.SubTopicId).FirstOrDefault();
                        var SelectedTopic = _db.TopicAllocations.Where(x => x.TopicId == selectedSubTopic.TopicId).OrderBy(x => x.StartDate).FirstOrDefault();
                        var SelectedTopicEndDate = _db.TopicAllocations.Where(x => x.TopicId == selectedSubTopic.TopicId).OrderByDescending(x => x.EndDate).FirstOrDefault();
                        topic.TopicStartDate = SelectedTopic.StartDate;
                        topic.TopicEndDate = SelectedTopicEndDate.EndDate;
                        topic.WBMinPage = SelectedTopic.WorkBookStartPage;
                        topic.WBMaxPage = SelectedTopic.WorkBookEndPage;
                        foreach (var allocated in allocatedSubTopics)
                        {
                            if (topic.SubTopicId == allocated.SubTopicId)
                            {
                                topic.preAllocation = true;
                                topic.StartDate = allocated.StartDate;
                                topic.EndDate = allocated.EndDate;
                                topic.WorkBookId = allocated.WorkBookId;
                                topic.WorkBookStartPage = allocated.WorkBookStartPage;
                                topic.WorkBookEndPage = allocated.WorkBookEndPage;
                            }
                        }
                    }
                    var month = await _repository.GetTermById(Id);
                    ViewBag.Month = month.TermName;
                    SearchSubTopics.SubTopics = subTopicList;
                    var ParentTopicSDate = _db.TopicAllocations.Where(x => x.TopicId == SearchSubTopics.TopicId && x.TermId == Id && x.PlanId == SearchSubTopics.PlanId).OrderBy(x => x.StartDate).FirstOrDefault();
                    var ParentTopicEDate = _db.TopicAllocations.Where(x => x.TopicId == SearchSubTopics.TopicId && x.TermId == Id && x.PlanId == SearchSubTopics.PlanId).OrderByDescending(x => x.EndDate).FirstOrDefault();
                    SearchSubTopics.MinDate = ParentTopicSDate.StartDate;
                    SearchSubTopics.MaxDate = ParentTopicEDate.EndDate;
                    return View(SearchSubTopics);
                }
                else
                {
                    List<SubTopicList> subTopicList = new List<SubTopicList>();
                    SearchSubTopics.SubTopics = subTopicList;
                    SearchSubTopics.MinDate = DateTime.Now;
                    SearchSubTopics.MaxDate = DateTime.Now;
                    return View(SearchSubTopics);
                }
            }
            return View(SearchSubTopics);
        }
        [Authorize(Policy = "Academic Planning")]
        [HttpPost]
        public async Task<IActionResult> SubTopicAllocation(SubTopicAllocationVM data)
        {
            var oldsubTopics = from a in _db.ChapterAllocations
                               from b in _db.TopicAllocations
                               from c in _db.SubTopicAllocations
                               where a.TermId == data.TermId && b.ChapterId == a.ChapterId && c.TopicId == b.TopicId && c.TopicId == data.TopicId && c.PlanId == data.PlanId
                               select c;
            _db.RemoveRange(oldsubTopics);
            await _db.SaveChangesAsync();
            var test = data.SubTopics.Where(x => x.IsSelected == true && x.StartDate != null && x.EndDate != null).ToList();
            List<SubTopicAllocation> allocations = new List<SubTopicAllocation>();
            foreach (var bid in test)
            {
                bid.preAllocation = true;
                var SubTopic = _db.subTopics.Where(x => x.SubTopicId == bid.SubTopicId).FirstOrDefault();
                var newSubTopic = new SubTopicAllocation
                {
                    TermId = data.TermId,
                    SubTopicId = bid.SubTopicId,
                    TopicId = SubTopic.TopicId,
                    StartDate = bid.StartDate,
                    EndDate = bid.EndDate,
                    WorkBookId = bid.WorkBookId,
                    WorkBookStartPage = bid.WorkBookStartPage,
                    WorkBookEndPage = bid.WorkBookEndPage,
                    PlanId = data.PlanId,
                    AreSaturdaysOff = data.AreSaturdaysOff
                };
                allocations.Add(newSubTopic);
            }
            if (allocations.Any())
            {
                await _db.AddRangeAsync(allocations);
                if (await _repository.SaveChanges())
                {
                    return RedirectToAction("Term", new { Id = data.YearId });
                }
                ModelState.AddModelError("", "Error While Saving to Database");
                return View(data);
            }
            return RedirectToAction("Term", new { Id = data.YearId });
        }
        #endregion

        [Authorize(Policy = "PlanDetails")]
        [HttpGet]
        public async Task<IActionResult> PlanDetails(int PlanId)
        {
            ViewBag.BookSelected = true;
            PlanDetailsVM calender = new PlanDetailsVM();
            calender.BookId = Convert.ToInt16((from a in _db.UnitAllocations
                                               from b in _db.Units
                                               from c in _db.Books
                                               where a.PlanId == PlanId && b.UnitId == a.UnitId && c.BookId == b.BookId
                                               select c.BookId).FirstOrDefault());

            #region Client-View
            if (calender.BookId == null)
            {
                ViewBag.BookSelected = true;
                return View(calender);
            }

            var allocatedUnits = (from a in _db.UnitAllocations
                                  from b in _db.Units
                                  from c in _db.Books
                                  where b.UnitId == a.UnitId && b.BookId == c.BookId && c.BookId == calender.BookId && a.PlanId == PlanId
                                  select new
                                  {
                                      UnitName = b.UnitName,
                                      StartDate = a.StartDate,
                                      EndDate = a.EndDate,
                                      UnitId = a.UnitId,
                                      WBStartPage = a.WorkBookStartPage,
                                      WBEndPage = a.WorkBookEndPage,
                                      SLO = b.SLO,
                                      TermId = a.TermId,
                                      AreSaturdaysOff = a.AreSaturdaysOff
                                  }).Distinct();
            if (User.IsInRole("Subject Teacher"))
            {
                calender.TermId = allocatedUnits.FirstOrDefault()?.TermId;
                var yearId = _db.terms.Where(x => x.TermId == calender.TermId).Select(x => x.YearId).FirstOrDefault();
                calender.YearId = yearId.Value;
            }
            foreach (var unit in allocatedUnits)
            {
                var AllocatedChapters = (from a in _db.ChapterAllocations
                                         join b in _db.chapters on a.ChapterId equals b.ChapterId into chapNames
                                         from names in chapNames.DefaultIfEmpty()
                                         where a.UnitId == unit.UnitId && a.PlanId == PlanId
                                         select new
                                         {
                                             ChapterName = names.ChapterName,
                                             StartDate = a.StartDate,
                                             EndDate = a.EndDate,
                                             ChapterId = a.ChapterId,
                                             WBStartPage = a.WorkBookStartPage,
                                             WBEndPage = a.WorkBookEndPage,
                                             SLO = names.SLO
                                         }).Distinct();
                if (!AllocatedChapters.Any())
                {
                    PLanList Unitplan = new PLanList() { StartDate = Convert.ToDateTime(unit.StartDate).ToString("dd-MMM-yyyy"), EndDate = Convert.ToDateTime(unit.EndDate).ToString("dd-MMM-yyyy"), UnitName = unit.UnitName /*, ChapterName = chapter.ChapterName, TopicName = topic.TopicName, TeachingMethodologyName = topic.TeachingMethodology,*/ , WbStartPage = unit.WBStartPage.ToString(), WbEndPage = unit.WBEndPage.ToString(), SLO = unit.SLO };
                    calender.Plan.Add(Unitplan);
                }
                foreach (var chapter in AllocatedChapters)
                {
                    var AllocatedTopics = (from a in _db.TopicAllocations
                                           join b in _db.topics on a.TopicId equals b.TopicId into topNames
                                           from names in topNames.DefaultIfEmpty()
                                           join c in _db.TeachingMethodologies on a.TeachingMethodologyId equals c.TeachingMethodologyId into TMethods
                                           from TMethod in TMethods.DefaultIfEmpty()
                                           where a.ChapterId == chapter.ChapterId && a.PlanId == PlanId
                                           select new
                                           {
                                               TopicName = names.TopicName,
                                               StartDate = a.StartDate,
                                               EndDate = a.EndDate,
                                               TopicId = a.TopicId,
                                               TeachingMethodology = TMethod.TMethodologyName,
                                               WBStartPage = a.WorkBookStartPage,
                                               WBEndPage = a.WorkBookEndPage,
                                               TeachingMethodologyDesc = a.TMethodDesc
                                           }).Distinct();
                    if (!AllocatedTopics.Any())
                    {
                        PLanList Chapterplan = new PLanList() { StartDate = Convert.ToDateTime(chapter.StartDate).ToString("dd-MMM-yyyy"), EndDate = Convert.ToDateTime(chapter.EndDate).ToString("dd-MMM-yyyy"), UnitName = unit.UnitName, ChapterName = chapter.ChapterName, /*TopicName = topic.TopicName, TeachingMethodologyName = topic.TeachingMethodology,*/ WbStartPage = chapter.WBStartPage.ToString(), WbEndPage = chapter.WBEndPage.ToString(), SLO = chapter.SLO };
                        calender.Plan.Add(Chapterplan);
                    }
                    else
                    {
                        foreach (var topic in AllocatedTopics)
                        {
                            var subTopics = (from a in _db.SubTopicAllocations
                                             join b in _db.subTopics on a.SubTopicId equals b.SubTopicId into SubtopNames
                                             from names in SubtopNames.DefaultIfEmpty()
                                             where a.TopicId == topic.TopicId && a.PlanId == PlanId
                                             select new
                                             {
                                                 TopicName = names.SubTopicName,
                                                 StartDate = a.StartDate,
                                                 EndDate = a.EndDate,
                                                 WBStartPage = a.WorkBookStartPage,
                                                 WBEndPage = a.WorkBookEndPage
                                             }).Distinct();
                            if (!subTopics.Any())
                            {
                                PLanList TopicPlan = new PLanList() { StartDate = Convert.ToDateTime(topic.StartDate).ToString("dd-MMM-yyyy"), EndDate = Convert.ToDateTime(topic.EndDate).ToString("dd-MMM-yyyy"), UnitName = unit.UnitName, ChapterName = chapter.ChapterName, TopicName = topic.TopicName, TeachingMethodologyName = topic.TeachingMethodology, TeachingMethodologyDesc = topic.TeachingMethodologyDesc, WbStartPage = topic.WBStartPage.ToString(), WbEndPage = topic.WBEndPage.ToString(), SLO = chapter.SLO };
                                calender.Plan.Add(TopicPlan);
                            }
                            else
                            {
                                foreach (var stopic in subTopics)
                                {
                                    PLanList plan = new PLanList() { StartDate = Convert.ToDateTime(stopic.StartDate).ToString("dd-MMM-yyyy"), EndDate = Convert.ToDateTime(stopic.EndDate).ToString("dd-MMM-yyyy"), UnitName = unit.UnitName, ChapterName = chapter.ChapterName, TopicName = topic.TopicName, TeachingMethodologyName = topic.TeachingMethodology, WbStartPage = stopic.WBStartPage.ToString(), WbEndPage = stopic.WBEndPage.ToString(), SubTopicName = stopic.TopicName, SLO = chapter.SLO, TeachingMethodologyDesc = topic.TeachingMethodologyDesc };
                                    calender.Plan.Add(plan);
                                }
                            }
                        }
                    }
                }
            }
            calender.Plan.OrderBy(x => x.StartDate);
            #endregion

            //calender.GradeName = (await (from a in _db.Books
            //                             from b in _db.Grades
            //                             where a.BookId == calender.BookId && b.GradeId == a.GradeId
            //                             select b.GradeName).FirstOrDefaultAsync())?.ToString();

            calender.SubjectName = (await (from a in _db.AcademicPlannings
                                           from b in _db.Subjects
                                           where a.AcademicPlanningsId == PlanId && b.SubjectId == a.SubjectId && b.IsActive == true
                                           select b.SubjectName).FirstOrDefaultAsync())?.ToString();
            calender.Textbook = _db.Books.Where(x => x.BookId == calender.BookId).FirstOrDefault()?.BookName;

            calender.Workbook = (await (from b in _db.UnitAllocations
                                        from c in _db.Books
                                        where b.PlanId == PlanId && c.BookId == b.WorkBookId && c.IsWorkBook == true && c.IsActive == true
                                        select c.BookName).FirstOrDefaultAsync())?.ToString();

            calender.TeacherName = (await (from a in _db.AcademicPlannings
                                           from d in _db.Employees
                                           where a.AcademicPlanningsId == PlanId && d.EmployeeId == a.EmployeeId
                                           select d.FName + " " + d.LName).FirstOrDefaultAsync())?.ToString();

            calender.ClassName = await (from a in _db.AcademicPlannings
                                  from b in _db.Sections
                                  from d in _db.Grades
                                  where a.AcademicPlanningsId == PlanId && b.SectionId == a.ClassId && d.GradeId == b.GradeId
                                  select d.GradeName + "-" + b.SectionName).FirstOrDefaultAsync();
            int weekEndCount = 0;
            DateTime startDate = Convert.ToDateTime(calender.Plan.OrderBy(x => x.StartDate).FirstOrDefault().StartDate);
            TimeSpan diff = Convert.ToDateTime(calender.Plan.OrderByDescending(x => x.EndDate).FirstOrDefault().EndDate) - startDate;
            int days = diff.Days;
            for (var i = 0; i <= days; i++)
            {
                var testDate = startDate.AddDays(i);
                if ((bool)allocatedUnits.FirstOrDefault().AreSaturdaysOff == false)
                {
                    if (testDate.DayOfWeek != DayOfWeek.Sunday || testDate.DayOfWeek != DayOfWeek.Saturday)
                    {
                        weekEndCount += 1;
                    }
                }
                else
                {
                    if (testDate.DayOfWeek != DayOfWeek.Sunday)
                    {
                        weekEndCount += 1;
                    }
                }
            }
            calender.DurationDays = weekEndCount;
            //calender.totalItems = totalItems;

            calender.PlanId = PlanId;
            //if(User.IsInRole("Director Academics") || User.IsInRole("Deputy Coordinator"))
            //{
            calender.IsApprovedByMe = _db.PlanApproval.Where(x => x.PlanId == PlanId && x.ApprovingPerson == User.FindFirst(ClaimTypes.Role).Value && x.Status == "Pending").Any();
            //}
            return View(calender);
        }

        #region AcademicPlanning-Plans
        [Authorize(Policy = "AcademicPlannings.Read")]
        [HttpGet]
        public async Task<IActionResult> AcademicPlannings()
        {
            AcademicPlanningsVM plans = new AcademicPlanningsVM();
            int empId = Convert.ToInt16(User.FindFirst(ClaimTypes.Sid)?.Value);
            plans.plans = await (from a in _db.AcademicPlannings
                                 join b in _db.Sections on a.ClassId equals b.SectionId into PlanClass
                                 from CPlan in PlanClass.DefaultIfEmpty()
                                 join c in _db.Grades on CPlan.GradeId equals c.GradeId into PlanGrade
                                 from PGrade in PlanGrade.DefaultIfEmpty()
                                 join d in _db.Books on a.BookId equals d.BookId into PlanBook
                                 from PBook in PlanBook.DefaultIfEmpty()
                                 join e in _db.Subjects on PBook.SubjectId equals e.SubjectId into PlanSubject
                                 from PSubject in PlanSubject.DefaultIfEmpty()
                                 where a.EmployeeId == empId && a.IsActive == true
                                 select new AcademicPlanningsVM
                                 {
                                     AcademicPlanningsId = a.AcademicPlanningsId,
                                     EmployeeId = a.EmployeeId,
                                     EndDate = a.EndDate,
                                     StartDate = a.StartDate,
                                     IsActive = a.IsActive,
                                     PlanName = a.PlanName,
                                     PlannedBy = a.PlannedBy,
                                     ApprovalStatus = (
                                         from pa in _db.PlanApproval
                                         where pa.PlanId == a.AcademicPlanningsId
                                         orderby pa.CreatedDate descending
                                         select pa.Status == "Pending" ? "Pending" :
                                             pa.ApprovingPerson == "Director Academics" && pa.Status == "Approved" ? "Approved" :
                                             "Rejected"
                                     ).FirstOrDefault(),
                                     ActiveSubmitPlan = _db.PlanApproval.Where(x => x.PlanId == a.AcademicPlanningsId).Any(),
                                     ClassId = (int)a.ClassId,
                                     ClassName = PGrade.GradeName + CPlan.SectionName,
                                     SubjectName = PSubject.SubjectName,
                                     BookName = PBook.BookName
                                 }
                            ).Distinct().ToListAsync();
            ViewBag.Classes = (from a in _db.Sections
                               from b in _db.SubjectTeacherAllocations
                               from c in _db.Grades
                               where c.GradeId == a.GradeId && a.SectionId == b.SectionId && b.EmployeeId == empId
                               select new
                               {
                                   SectionId = b.SectionId,
                                   SectionName = c.GradeName + a.SectionName
                               }).Distinct().ToList();
            if (!plans.plans.Any())
            {
                plans.plans = await _db.AcademicPlannings.Where(x => x.EmployeeId == empId && x.IsActive == true).Select(x => new AcademicPlanningsVM { IsActive = x.IsActive, EmployeeId = x.EmployeeId, StartDate = x.StartDate, PlanName = x.PlanName, EndDate = x.EndDate, PlannedBy = x.PlannedBy, AcademicPlanningsId = x.AcademicPlanningsId }).ToListAsync();
            }
            return View(plans);
        }
        [Authorize(Policy = "AcademicPlannings.Create")]
        [HttpPost]
        public async Task<IActionResult> AcademicPlannings(AcademicPlanningsVM plan)
        {
            if (ModelState.IsValid)
            {
                int empId = Convert.ToInt16(User.FindFirst(ClaimTypes.Sid)?.Value);
                var employe = await _db.Employees.Where(x => x.EmployeeId == empId).FirstOrDefaultAsync();
                var AcademicPlanning = new AcademicPlannings
                {
                    EmployeeId = empId,
                    PlanName = plan.PlanName,
                    StartDate = plan.StartDate,
                    EndDate = plan.EndDate,
                    PlannedBy = employe.FName + " " + employe.LName,
                    ClassId = plan.ClassId,
                    BookId = plan.BookId,
                    SubjectId = plan.SubjectId
                };
                await _repository.AddAsync(AcademicPlanning);
                if (await _repository.SaveChanges())
                {
                    return RedirectToAction("AcademicPlannings");
                }
                ModelState.AddModelError("", "Error While Saving in Database!");
            }
            return View(plan);
        }
        [Authorize(Policy = "AcademicPlannings.Update")]
        [HttpGet]
        public async Task<IActionResult> UpdateAcademicPlanning(int AcademicPlanningId)
        {
            int empId = Convert.ToInt16(User.FindFirst(ClaimTypes.Sid)?.Value);
            var Dbplan = await _db.AcademicPlannings.Where(x => x.AcademicPlanningsId == AcademicPlanningId).FirstOrDefaultAsync();
            AcademicPlanningsVM plan = new AcademicPlanningsVM
            {
                PlanName = Dbplan.PlanName,
                AcademicPlanningsId = Dbplan.AcademicPlanningsId,
                IsActive = Dbplan.IsActive
            };
            plan.ClassId = Dbplan.ClassId == null ? 0 : Dbplan.ClassId;
            plan.BookId = Dbplan.BookId == null ? 0 : Dbplan.BookId;
            plan.SubjectId = Dbplan.SubjectId == null ? 0 : Dbplan.SubjectId;
            ViewBag.Books = await (from a in _db.SubjectTeacherAllocations
                                   from b in _db.Books
                                   from c in _db.Subjects
                                   where a.EmployeeId == empId && b.BookId == a.BookId && c.SubjectId == b.SubjectId && c.SubjectId == Dbplan.SubjectId
                                   select new
                                   {
                                       BookId = b.BookId,
                                       BookName = b.BookName
                                   }).Distinct().ToListAsync();
            ViewBag.Subjects = await (from a in _db.SubjectTeacherAllocations
                                      join b in _db.Books on a.BookId equals b.BookId into EmpBooks
                                      from EBs in EmpBooks.DefaultIfEmpty()
                                      join c in _db.Subjects on EBs.SubjectId equals c.SubjectId into BookSubs
                                      from BSs in BookSubs.DefaultIfEmpty()
                                      where a.EmployeeId == empId && a.SectionId == Dbplan.ClassId && BSs.IsActive == true
                                      select BSs).Distinct().ToListAsync();
            ViewBag.Classes = (from a in _db.Sections
                               from b in _db.SubjectTeacherAllocations
                               from c in _db.Grades
                               where c.GradeId == a.GradeId && a.SectionId == b.SectionId && b.EmployeeId == empId
                               select new
                               {
                                   SectionId = b.SectionId,
                                   SectionName = c.GradeName + a.SectionName
                               }).Distinct().ToList();
            return View(plan);
        }
        [Authorize(Policy = "AcademicPlannings.Update")]
        [HttpPost]
        public async Task<IActionResult> UpdateAcademicPlanning(AcademicPlanningsVM plan)
        {
            if (ModelState.IsValid)
            {
                var temp = await _db.AcademicPlannings.Where(x => x.AcademicPlanningsId == plan.AcademicPlanningsId).FirstOrDefaultAsync();
                temp.PlanName = plan.PlanName;
                temp.ClassId = plan.ClassId;
                temp.BookId = plan.BookId;
                temp.SubjectId = plan.SubjectId;
                if (User.IsInRole("Director Academics") || User.IsInRole("Deputy Coordinator"))
                {
                    temp.IsActive = plan.IsActive;
                    if (temp.IsActive == false && plan.IsActive == true)
                    {
                        var plannedUnits = await _db.UnitAllocations.Where(x => x.PlanId == plan.AcademicPlanningsId).ToListAsync();
                        var plannedChapters = await _db.ChapterAllocations.Where(x => x.PlanId == plan.AcademicPlanningsId).ToListAsync();
                        var plannedTopic = await _db.TopicAllocations.Where(x => x.PlanId == plan.AcademicPlanningsId).ToListAsync();
                        var plannedSubTopic = await _db.SubTopicAllocations.Where(x => x.PlanId == plan.AcademicPlanningsId).ToListAsync();
                        var planApprovals = await _db.PlanApproval.Where(x => x.PlanId == plan.AcademicPlanningsId).ToListAsync();
                        if (plannedUnits.Any())
                        {
                            foreach (var unit in plannedUnits)
                            {
                                unit.IsActive = true;
                            }
                            _db.UpdateRange(plannedUnits);
                        }
                        if (plannedChapters.Any())
                        {
                            foreach (var chapter in plannedChapters)
                            {
                                chapter.IsActive = true;
                            }
                            _db.UpdateRange(plannedChapters);
                        }
                        if (plannedTopic.Any())
                        {
                            foreach (var topic in plannedTopic)
                            {
                                topic.IsActive = true;
                            }
                            _db.UpdateRange(plannedTopic);
                        }
                        if (plannedSubTopic.Any())
                        {
                            foreach (var sTopic in plannedSubTopic)
                            {
                                sTopic.IsActive = true;
                            }
                            _db.UpdateRange(plannedSubTopic);
                        }
                        if (planApprovals.Any())
                        {
                            foreach (var planAppval in planApprovals)
                            {
                                planAppval.IsActive = true;
                            }
                            _db.UpdateRange(planApprovals);
                        }
                    }
                }
                await _repository.UpdateAsync(temp);
                if (await _repository.SaveChanges())
                {
                    return RedirectToAction("AcademicPlannings");
                }
                ModelState.AddModelError("", "Error While Saving in Database");
            }
            return View(plan);
        }
        [Authorize(Policy = "AcademicPlannings.Delete")]
        [HttpGet]
        public async Task<IActionResult> DeletePlan(int AcademicPlanningId)
        {
            var plan = await _db.AcademicPlannings.Where(x => x.AcademicPlanningsId == AcademicPlanningId).FirstOrDefaultAsync();
            plan.IsActive = false;
            var plannedUnits = await _db.UnitAllocations.Where(x => x.PlanId == AcademicPlanningId).ToListAsync();
            var plannedChapters = await _db.ChapterAllocations.Where(x => x.PlanId == AcademicPlanningId).ToListAsync();
            var plannedTopic = await _db.TopicAllocations.Where(x => x.PlanId == AcademicPlanningId).ToListAsync();
            var plannedSubTopic = await _db.SubTopicAllocations.Where(x => x.PlanId == AcademicPlanningId).ToListAsync();
            var planApprovals = await _db.PlanApproval.Where(x => x.PlanId == AcademicPlanningId).ToListAsync();
            if (plannedUnits.Any())
            {
                foreach (var unit in plannedUnits)
                {
                    unit.IsActive = false;
                }
                _db.UpdateRange(plannedUnits);
            }
            if (plannedChapters.Any())
            {
                foreach (var chapter in plannedChapters)
                {
                    chapter.IsActive = false;
                }
                _db.UpdateRange(plannedChapters);
            }
            if (plannedTopic.Any())
            {
                foreach (var topic in plannedTopic)
                {
                    topic.IsActive = false;
                }
                _db.UpdateRange(plannedTopic);
            }
            if (plannedSubTopic.Any())
            {
                foreach (var sTopic in plannedSubTopic)
                {
                    sTopic.IsActive = false;
                }
                _db.UpdateRange(plannedSubTopic);
            }
            if (planApprovals.Any())
            {
                foreach (var planAppval in planApprovals)
                {
                    planAppval.IsActive = false;
                }
                _db.UpdateRange(planApprovals);
            }
            await _repository.UpdateAsync(plan);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("AcademicPlannings");
            }
            ModelState.AddModelError("", "Error While Saving to Database");
            return View(AcademicPlanningId);
        }

        [Authorize(Policy = "Academic Planning")]
        [HttpPost]
        public async Task<IActionResult> CopyAcademicPlanning(UnitAllocationVM unit)
        {
            var CurrentUAllocations = await _db.UnitAllocations.Where(x => x.PlanId == unit.PlanId && x.TermId == unit.TermId && x.IsActive == true).ToListAsync();
            if (CurrentUAllocations.Any())
            {
                _db.RemoveRange(CurrentUAllocations);
            }
            List<UnitAllocation> PlannedUnits = await _db.UnitAllocations.Where(x => x.PlanId == unit.CopiedPlanId && x.TermId == unit.TermId && x.IsActive == true).Select(x => new UnitAllocation { IsActive = x.IsActive, EndDate = x.EndDate, PlanId = unit.PlanId, SectionId = x.SectionId, StartDate = x.StartDate, TermId = x.TermId, WorkBookId = x.WorkBookId, WorkBookEndPage = x.WorkBookEndPage, WorkBookStartPage = x.WorkBookStartPage, UnitId = x.UnitId }).ToListAsync();
            if (PlannedUnits.Any())
            {
                await _db.AddRangeAsync(PlannedUnits);
            }
            var CurrentCAllocations = await _db.ChapterAllocations.Where(x => x.PlanId == unit.PlanId && x.TermId == unit.TermId && x.IsActive == true).ToListAsync();
            if (CurrentUAllocations.Any())
            {
                _db.RemoveRange(CurrentCAllocations);
            }
            List<ChapterAllocation> PlannedChapters = await _db.ChapterAllocations.Where(x => x.PlanId == unit.CopiedPlanId && x.TermId == unit.TermId && x.IsActive == true).Select(x => new ChapterAllocation { UnitId = x.UnitId, WorkBookStartPage = x.WorkBookStartPage, WorkBookEndPage = x.WorkBookEndPage, WorkBookId = x.WorkBookId, TermId = x.TermId, SectionId = x.SectionId, ChapterId = x.ChapterId, EndDate = x.EndDate, IsActive = x.IsActive, PlanId = unit.PlanId, StartDate = x.StartDate }).ToListAsync();
            if (PlannedChapters.Any())
            {
                await _db.AddRangeAsync(PlannedChapters);
            }
            var CurrentTAllocations = await _db.TopicAllocations.Where(x => x.PlanId == unit.PlanId && x.TermId == unit.TermId && x.IsActive == true).ToListAsync();
            if (CurrentUAllocations.Any())
            {
                _db.RemoveRange(CurrentTAllocations);
            }
            List<TopicAllocation> PlannedTopics = await _db.TopicAllocations.Where(x => x.PlanId == unit.CopiedPlanId && x.TermId == unit.TermId && x.IsActive == true).Select(x => new TopicAllocation { StartDate = x.StartDate, PlanId = unit.PlanId, IsActive = x.IsActive, EndDate = x.EndDate, ChapterId = x.ChapterId, SectionId = x.SectionId, TermId = x.TermId, TeachingMethodologyId = x.TeachingMethodologyId, TMethodDesc = x.TMethodDesc, TopicId = x.TopicId, WorkBookEndPage = x.WorkBookEndPage, WorkBookId = x.WorkBookId, WorkBookStartPage = x.WorkBookStartPage }).ToListAsync();
            if (PlannedTopics.Any())
            {
                await _db.AddRangeAsync(PlannedTopics);
            }
            var CurrentSTAllocations = await _db.SubTopicAllocations.Where(x => x.PlanId == unit.PlanId && x.TermId == unit.TermId && x.IsActive == true).ToListAsync();
            if (CurrentUAllocations.Any())
            {
                _db.RemoveRange(CurrentSTAllocations);
            }
            List<SubTopicAllocation> PlannedSubTopics = await _db.SubTopicAllocations.Where(x => x.PlanId == unit.CopiedPlanId && x.TermId == unit.TermId && x.IsActive == true).Select(x => new SubTopicAllocation { PlanId = unit.PlanId, WorkBookStartPage = x.WorkBookStartPage, WorkBookEndPage = x.WorkBookEndPage, WorkBookId = x.WorkBookId, TopicId = x.TopicId, IsActive = x.IsActive, EndDate = x.EndDate, SectionId = x.SectionId, StartDate = x.StartDate, SubTopicId = x.SubTopicId, TermId = x.TermId }).ToListAsync();
            if (PlannedSubTopics.Any())
            {
                await _db.AddRangeAsync(PlannedSubTopics);
            }
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("PlanDetails", new { PlanId = unit.PlanId });
            }
            return View(PlannedUnits);
        }
        #endregion

        #region Plan-Approval
        [Authorize(Policy = "AcademicPlannings.Create")]
        [HttpGet]
        public async Task<IActionResult> PlanApprovalSubmit(int PlanId)
        {
            int empId = Convert.ToInt16(User.FindFirst(ClaimTypes.Sid)?.Value);
            //if (User.IsInRole("Subject Teacher"))
            //{
            //int CTID = Convert.ToInt16((from a in _db.UnitAllocations
            //                            from b in _db.Units
            //                            from c in _db.Books
            //                            from d in _db.SubjectTeacherAllocations
            //                            from e in _db.Sections
            //                            where a.PlanId == PlanId && b.UnitId == a.UnitId && c.BookId == b.BookId && d.BookId == c.BookId && e.SectionId == d.SectionId
            //                            select e.ClassTeacherId).FirstOrDefault());
            if (!User.IsInRole("Deputy Coordinator") && !User.IsInRole("Director Academics"))
            {
                int ACID = Convert.ToInt16((from a in _db.AcademicPlannings
                                            from b in _db.Sections
                                            from c in _db.Grades
                                            from d in _db.SchoolSections
                                            where a.AcademicPlanningsId == PlanId && b.SectionId == a.ClassId && c.GradeId == b.GradeId && d.SchoolSectionId == c.SchoolSectionId && a.IsActive == true && b.IsActive == true && c.IsActive == true && d.IsActive == true
                                            select d.AssistantCoordinatorId).FirstOrDefault());
                var AC = await _db.Employees.Where(x => x.EmployeeId == ACID).FirstOrDefaultAsync();
                var PlanApproval = new PlanApproval
                {
                    ApprovingPerson = "Assistant Coordinator",
                    ApprovingPersonName = AC?.FName + " " + AC?.LName,
                    PlanId = PlanId,
                    Remarks = "Status Pending",
                    Status = "Pending",
                    CreatedDate = DateTime.Now
                };
                await _repository.AddAsync(PlanApproval);
                if (await _repository.SaveChanges())
                {
                    return RedirectToAction("AcademicPlannings");
                }
                ModelState.AddModelError("", "Error While Saving in Database!");
                return View(PlanId);
            }
            return View(PlanId);
            //}
            //else if (User.IsInRole("Class Teacher"))
            //{
            //    bool IsST = (from a in _db.AcademicPlannings
            //                 from b in _db.SubjectTeacherAllocations
            //                 where a.AcademicPlanningsId == PlanId && b.SectionId == a.ClassId && b.EmployeeId == empId
            //                 select a).Any();
            //    if (IsST)
            //    {
            //        int CTID = Convert.ToInt16((from a in _db.AcademicPlannings
            //                                    from b in _db.Sections
            //                                    where a.AcademicPlanningsId == PlanId && b.SectionId == a.ClassId && a.IsActive == true && b.IsActive == true
            //                                    select b.ClassTeacherId).FirstOrDefault());
            //        var ClassTeacher = await _db.Employees.Where(x => x.EmployeeId == CTID).FirstOrDefaultAsync();
            //        var PlanApproval = new PlanApproval
            //        {
            //            ApprovingPerson = "Class Teacher",
            //            ApprovingPersonName = ClassTeacher?.FName + " " + ClassTeacher?.LName,
            //            PlanId = PlanId,
            //            Remarks = "Status Pending",
            //            Status = "Pending",
            //            CreatedDate = DateTime.Now
            //        };
            //        await _repository.AddAsync(PlanApproval);
            //    }
            //    else
            //    {
            //        int GMID = Convert.ToInt16((from a in _db.UnitAllocations
            //                                    from b in _db.Units
            //                                    from c in _db.Books
            //                                    from d in _db.SubjectTeacherAllocations
            //                                    from e in _db.Sections
            //                                    from f in _db.Grades
            //                                    where a.PlanId == PlanId && b.UnitId == a.UnitId && c.BookId == b.BookId && d.BookId == c.BookId && e.SectionId == d.SectionId && f.GradeId == e.GradeId
            //                                    select f.GradeManagerId).FirstOrDefault());
            //        var GradeManager = await _db.Employees.Where(x => x.EmployeeId == GMID).FirstOrDefaultAsync();
            //        var GMApproval = new PlanApproval
            //        {
            //            ApprovingPerson = "Grade Manager",
            //            ApprovingPersonName = GradeManager?.FName + " " + GradeManager?.LName,
            //            PlanId = PlanId,
            //            Remarks = "Status Pending (Class Teacher's Plan)",
            //            Status = "Pending",
            //            CreatedDate = DateTime.Now
            //        };
            //        await _repository.AddAsync(GMApproval);
            //    }
            //}
            //else if (User.IsInRole("Grade Manager"))
            //{
            //    bool IsST = (from a in _db.AcademicPlannings
            //                 from b in _db.SubjectTeacherAllocations
            //                 where a.AcademicPlanningsId == PlanId && a.ClassId == b.SectionId && b.EmployeeId == empId
            //                 select a).Any();
            //    if (IsST)
            //    {
            //        int CTID = Convert.ToInt16((from a in _db.AcademicPlannings
            //                                    from b in _db.Sections
            //                                    from c in _db.Grades
            //                                    where a.AcademicPlanningsId == PlanId && b.SectionId == a.ClassId && b.GradeId == c.GradeId && c.GradeManagerId == empId && a.EmployeeId == empId && a.IsActive == true && b.IsActive == true && c.IsActive == true
            //                                    select b.ClassTeacherId).FirstOrDefault());
            //        var ClassTeacher = await _db.Employees.Where(x => x.EmployeeId == CTID).FirstOrDefaultAsync();
            //        var PlanApproval = new PlanApproval
            //        {
            //            ApprovingPerson = "Class Teacher",
            //            ApprovingPersonName = ClassTeacher?.FName + " " + ClassTeacher?.LName,
            //            PlanId = PlanId,
            //            Remarks = "Status Pending",
            //            Status = "Pending",
            //            CreatedDate = DateTime.Now
            //        };
            //        await _repository.AddAsync(PlanApproval);
            //    }
            //    else
            //    {
            //        int ACID = Convert.ToInt16((from a in _db.UnitAllocations
            //                                    from b in _db.Units
            //                                    from c in _db.Books
            //                                    from d in _db.SubjectTeacherAllocations
            //                                    from e in _db.Sections
            //                                    from f in _db.Grades
            //                                    from g in _db.SchoolSections
            //                                    where a.PlanId == PlanId && b.UnitId == a.UnitId && c.BookId == b.BookId && d.BookId == c.BookId && e.SectionId == d.SectionId && f.GradeId == e.GradeId && g.SchoolSectionId == f.SchoolSectionId
            //                                    select g.AssistantCoordinatorId).FirstOrDefault());
            //        var AssCoordinator = await _db.Employees.Where(x => x.EmployeeId == ACID).FirstOrDefaultAsync();
            //        var ACPlanApproval = new PlanApproval
            //        {
            //            ApprovingPerson = "Assistant Coordinator",
            //            ApprovingPersonName = AssCoordinator?.FName + " " + AssCoordinator?.LName,
            //            PlanId = PlanId,
            //            Remarks = "Status Pending (Grade Manager's Plan)",
            //            Status = "Pending",
            //            CreatedDate = DateTime.Now
            //        };
            //        await _repository.AddAsync(ACPlanApproval);
            //    }
            //}
        }
        [Authorize(Policy = "AcademicPlannings.Create")]
        [HttpGet]
        public async Task<IActionResult> PlanApprovalReSubmit(int PlanId)
        {
            var RejectedApproval = await _db.PlanApproval.Where(x => x.PlanId == PlanId).OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync();
            //var ApprovalRequest = await _db.PlanApproval.Where(x => x.PlanId == calendarVM.PlanId && x.IsActive == true && x.ApprovingPerson == role && x.Status == "Pending").FirstOrDefaultAsync();
            var ApprovalRequest = new PlanApproval();
            ApprovalRequest.ApprovingPerson = RejectedApproval.ApprovingPerson;
            ApprovalRequest.ApprovingPersonName = RejectedApproval.ApprovingPersonName;
            ApprovalRequest.PlanId = RejectedApproval.PlanId;
            ApprovalRequest.Remarks = "Resubmitted After Rejeted by " + ApprovalRequest.ApprovingPersonName + " (" + ApprovalRequest.ApprovingPerson + ")";
            ApprovalRequest.Status = "Pending";
            ApprovalRequest.CreatedDate = DateTime.Now;
            await _repository.AddAsync(ApprovalRequest);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("AcademicPlannings");
            }
            ModelState.AddModelError("", "Error While Saving in Database!");
            return View(PlanId);
        }
        [Authorize(Policy = "ManagementPlanApproving")]
        [HttpGet]
        public async Task<IActionResult> PlanApprovals()
        {
            int empId = Convert.ToInt16(User.FindFirst(ClaimTypes.Sid)?.Value);
            if (User.IsInRole("Class Teacher"))
            {
                ViewBag.Plans = await (from a in _db.SubjectTeacherAllocations
                                       from b in _db.UnitAllocations
                                       from c in _db.Units
                                       from d in _db.AcademicPlannings
                                       from f in _db.Sections
                                       where c.BookId == a.BookId && b.UnitId == c.UnitId && d.AcademicPlanningsId == b.PlanId && f.SectionId == a.SectionId && f.ClassTeacherId == empId && d.IsActive == true
                                       select new
                                       {
                                           Status = (
                                                     from pa in _db.PlanApproval
                                                     where pa.PlanId == d.AcademicPlanningsId
                                                     orderby pa.CreatedDate descending
                                                     select pa.Status == "Pending" ? "Pending" :
                                                         pa.ApprovingPerson == "Director Academics" && pa.Status == "Approved" ? "Approved" :
                                                         "Rejected").FirstOrDefault(),
                                           PlanName = d.PlanName,
                                           PlannedBy = d.PlannedBy,
                                           AcademicPlanningsId = d.AcademicPlanningsId,
                                           IsActive = d.IsActive,
                                           MyDecision = (from appr in _db.PlanApproval
                                                         where appr.PlanId == d.AcademicPlanningsId && appr.ApprovingPerson == "Class Teacher"
                                                         orderby appr.CreatedDate descending
                                                         select appr.Status == "Pending" ? "Pending" :
                                                         appr.Status == "Approved" ? "Approved" :
                                                         "Rejected").FirstOrDefault(),
                                           MyApproval = _db.PlanApproval.Where(x => x.PlanId == d.AcademicPlanningsId && x.ApprovingPerson == "Class Teacher").Any()
                                       }).Distinct().ToListAsync();
            }
            else if (User.IsInRole("Grade Manager"))
            {
                ViewBag.Plans = await (from a in _db.SubjectTeacherAllocations
                                       from b in _db.UnitAllocations
                                       from c in _db.Units
                                       from d in _db.AcademicPlannings
                                       from f in _db.Sections
                                       from g in _db.Grades
                                       where c.BookId == a.BookId && b.UnitId == c.UnitId && d.AcademicPlanningsId == b.PlanId && f.SectionId == a.SectionId && f.GradeId == g.GradeId && g.GradeManagerId == empId && d.IsActive == true
                                       select new
                                       {
                                           Status = (
                                                     from pa in _db.PlanApproval
                                                     where pa.PlanId == d.AcademicPlanningsId
                                                     orderby pa.CreatedDate descending
                                                     select pa.Status == "Pending" ? "Pending" :
                                                         pa.ApprovingPerson == "Director Academics" && pa.Status == "Approved" ? "Approved" :
                                                         "Rejected").FirstOrDefault(),
                                           PlanName = d.PlanName,
                                           PlannedBy = d.PlannedBy,
                                           AcademicPlanningsId = d.AcademicPlanningsId,
                                           IsActive = d.IsActive,
                                           MyDecision = (from appr in _db.PlanApproval
                                                         where appr.PlanId == d.AcademicPlanningsId && appr.ApprovingPerson == "Grade Manager"
                                                         orderby appr.CreatedDate descending
                                                         select appr.Status == "Pending" ? "Pending" :
                                                         appr.Status == "Approved" ? "Approved" :
                                                         "Rejected").FirstOrDefault(),
                                           MyApproval = _db.PlanApproval.Where(x => x.PlanId == d.AcademicPlanningsId && x.ApprovingPerson == "Grade Manager").Any()
                                       }).Distinct().ToListAsync();
            }
            else if (User.IsInRole("Assistant Coordinator"))
            {
                ViewBag.Plans = await (from a in _db.SubjectTeacherAllocations
                                       from b in _db.UnitAllocations
                                       from c in _db.Units
                                       from d in _db.AcademicPlannings
                                       from f in _db.Sections
                                       from g in _db.Grades
                                       from h in _db.SchoolSections
                                       where c.BookId == a.BookId && b.UnitId == c.UnitId && d.AcademicPlanningsId == b.PlanId && f.SectionId == a.SectionId && f.GradeId == g.GradeId && h.SchoolSectionId == g.SchoolSectionId && h.AssistantCoordinatorId == empId && d.IsActive == true
                                       select new
                                       {
                                           Status = (
                                                     from pa in _db.PlanApproval
                                                     where pa.PlanId == d.AcademicPlanningsId
                                                     orderby pa.CreatedDate descending
                                                     select pa.Status == "Pending" ? "Pending" :
                                                         pa.ApprovingPerson == "Director Academics" && pa.Status == "Approved" ? "Approved" :
                                                         "Rejected").FirstOrDefault(),
                                           PlanName = d.PlanName,
                                           PlannedBy = d.PlannedBy,
                                           AcademicPlanningsId = d.AcademicPlanningsId,
                                           IsActive = d.IsActive,
                                           MyDecision = (from appr in _db.PlanApproval
                                                         where appr.PlanId == d.AcademicPlanningsId && appr.ApprovingPerson == "Assistant Coordinator"
                                                         orderby appr.CreatedDate descending
                                                         select appr.Status == "Pending" ? "Pending" :
                                                         appr.Status == "Approved" ? "Approved" :
                                                         "Rejected").FirstOrDefault(),
                                           MyApproval = _db.PlanApproval.Where(x => x.PlanId == d.AcademicPlanningsId && x.ApprovingPerson == "Assistant Coordinator").Any()
                                       }).Distinct().ToListAsync();
            }
            else if (User.IsInRole("Deputy Coordinator"))
            {
                ViewBag.Plans = await (from a in _db.SubjectTeacherAllocations
                                       from b in _db.UnitAllocations
                                       from c in _db.Units
                                       from d in _db.AcademicPlannings
                                       from f in _db.Sections
                                       from g in _db.Grades
                                       from h in _db.SchoolSections
                                       where c.BookId == a.BookId && b.UnitId == c.UnitId && d.AcademicPlanningsId == b.PlanId && f.SectionId == a.SectionId && f.GradeId == g.GradeId && h.SchoolSectionId == g.SchoolSectionId && d.IsActive == true
                                       select new
                                       {
                                           Status = (
                                                     from pa in _db.PlanApproval
                                                     where pa.PlanId == d.AcademicPlanningsId
                                                     orderby pa.CreatedDate descending
                                                     select pa.Status == "Pending" ? "Pending" :
                                                         pa.ApprovingPerson == "Director Academics" && pa.Status == "Approved" ? "Approved" :
                                                         "Rejected").FirstOrDefault(),
                                           PlanName = d.PlanName,
                                           PlannedBy = d.PlannedBy,
                                           AcademicPlanningsId = d.AcademicPlanningsId,
                                           IsActive = d.IsActive,
                                           MyDecision = (from appr in _db.PlanApproval
                                                         where appr.PlanId == d.AcademicPlanningsId && appr.ApprovingPerson == "Deputy Coordinator"
                                                         orderby appr.CreatedDate descending
                                                         select appr.Status == "Pending" ? "Pending" :
                                                         appr.Status == "Approved" ? "Approved" :
                                                         "Rejected").FirstOrDefault(),
                                           MyApproval = _db.PlanApproval.Where(x => x.PlanId == d.AcademicPlanningsId && x.ApprovingPerson == "Deputy Coordinator").Any()
                                       }).Distinct().ToListAsync();
            }
            else
            {
                ViewBag.Plans = await (from a in _db.SubjectTeacherAllocations
                                       from b in _db.UnitAllocations
                                       from c in _db.Units
                                       from d in _db.AcademicPlannings
                                       from f in _db.Sections
                                       from g in _db.Grades
                                       from h in _db.SchoolSections
                                       where c.BookId == a.BookId && b.UnitId == c.UnitId && d.AcademicPlanningsId == b.PlanId && f.SectionId == a.SectionId && f.GradeId == g.GradeId && h.SchoolSectionId == g.SchoolSectionId
                                       select new
                                       {
                                           Status = (
                                                     from pa in _db.PlanApproval
                                                     where pa.PlanId == d.AcademicPlanningsId
                                                     orderby pa.CreatedDate descending
                                                     select pa.Status == "Pending" ? "Pending" :
                                                         pa.ApprovingPerson == "Director Academics" && pa.Status == "Approved" ? "Approved" :
                                                         "Rejected").FirstOrDefault(),
                                           PlanName = d.PlanName,
                                           PlannedBy = d.PlannedBy,
                                           AcademicPlanningsId = d.AcademicPlanningsId,
                                           IsActive = d.IsActive,
                                           MyDecision = (from appr in _db.PlanApproval
                                                         where appr.PlanId == d.AcademicPlanningsId && appr.ApprovingPerson == "Director Academics"
                                                         orderby appr.CreatedDate descending
                                                         select appr.Status == "Pending" ? "Pending" :
                                                         appr.Status == "Approved" ? "Approved" :
                                                         "Rejected").FirstOrDefault(),
                                           MyApproval = _db.PlanApproval.Where(x => x.PlanId == d.AcademicPlanningsId && x.ApprovingPerson == "Director Academics").Any()
                                       }).Distinct().OrderBy(x => x.IsActive).ToListAsync();
            }
            return View();
        }
        [Authorize(Policy = "ManagementPlanApproving")]
        [HttpPost]
        public async Task<IActionResult> PlanApprovals(PlanDetailsVM calendarVM)
        {
            if (calendarVM.PlanStatus == "Approved")
            {
                if (User.IsInRole("Class Teacher"))
                {
                    var prevApprovalRequest = await _db.PlanApproval.Where(x => x.PlanId == calendarVM.PlanId && x.IsActive == true && x.ApprovingPerson == "Class Teacher" && x.Status == "Pending").FirstOrDefaultAsync();
                    var approval = new PlanApproval
                    {
                        ApprovingPerson = "Class Teacher",
                        ApprovingPersonName = prevApprovalRequest.ApprovingPersonName,
                        PlanId = calendarVM.PlanId,
                        CreatedDate = DateTime.Now,
                        Remarks = calendarVM.PlanRemarks,
                        Status = calendarVM.PlanStatus,
                    };
                    await _repository.AddAsync(approval);
                    prevApprovalRequest.ModifiedBy = User.FindFirst(ClaimTypes.Role)?.Value;
                    int GMID = Convert.ToInt16((from a in _db.AcademicPlannings
                                                from b in _db.Sections
                                                from c in _db.Grades
                                                where a.AcademicPlanningsId == calendarVM.PlanId && b.SectionId == a.ClassId && b.GradeId == c.GradeId && a.IsActive == true && b.IsActive == true && c.IsActive == true
                                                select c.GradeManagerId).FirstOrDefault());
                    var GradeManager = await _db.Employees.Where(x => x.EmployeeId == GMID).FirstOrDefaultAsync();
                    var CTPlanApproval = new PlanApproval
                    {
                        ApprovingPerson = "Grade Manager",
                        ApprovingPersonName = GradeManager?.FName + " " + GradeManager?.LName,
                        PlanId = calendarVM.PlanId,
                        Remarks = "Approval Pending",
                        Status = "Pending",
                        CreatedDate = DateTime.Now
                    };
                    await _repository.AddAsync(CTPlanApproval);
                    if (await _repository.SaveChanges())
                    {
                        return RedirectToAction("PlanApprovals");
                    }
                    ModelState.AddModelError("", "Error While Saving in Database!");
                }
                else if (User.IsInRole("Grade Manager"))
                {
                    var prevApprovalRequest = await _db.PlanApproval.Where(x => x.PlanId == calendarVM.PlanId && x.IsActive == true && x.ApprovingPerson == "Grade Manager" && x.Status == "Pending").FirstOrDefaultAsync();
                    var approval = new PlanApproval
                    {
                        ApprovingPerson = prevApprovalRequest.ApprovingPerson,
                        ApprovingPersonName = prevApprovalRequest.ApprovingPersonName,
                        PlanId = calendarVM.PlanId,
                        CreatedDate = DateTime.Now,
                        Remarks = calendarVM.PlanRemarks,
                        Status = calendarVM.PlanStatus,
                    };
                    await _repository.AddAsync(approval);
                    int ACID = Convert.ToInt16((from a in _db.AcademicPlannings
                                                from b in _db.Sections
                                                from c in _db.Grades
                                                from d in _db.SchoolSections
                                                where a.AcademicPlanningsId == calendarVM.PlanId && b.SectionId == a.ClassId && b.GradeId == c.GradeId && d.SchoolSectionId == c.SchoolSectionId && d.IsActive == true && a.IsActive == true && b.IsActive == true && c.IsActive == true
                                                select d.AssistantCoordinatorId).FirstOrDefault());
                    var GradeManager = await _db.Employees.Where(x => x.EmployeeId == ACID).FirstOrDefaultAsync();
                    var GMPlanApproval = new PlanApproval
                    {
                        ApprovingPerson = "Assistant Coordinator",
                        ApprovingPersonName = GradeManager?.FName + " " + GradeManager?.LName,
                        PlanId = calendarVM.PlanId,
                        Remarks = "Status Pending",
                        Status = "Pending",
                        CreatedDate = DateTime.Now
                    };
                    await _repository.AddAsync(GMPlanApproval);
                    if (await _repository.SaveChanges())
                    {
                        return RedirectToAction("PlanApprovals");
                    }
                    ModelState.AddModelError("", "Error While Saving in Database!");
                }
                else if (User.IsInRole("Assistant Coordinator"))
                {
                    var prevApprovalRequest = await _db.PlanApproval.Where(x => x.PlanId == calendarVM.PlanId && x.IsActive == true && x.ApprovingPerson == "Assistant Coordinator" && x.Status == "Pending").FirstOrDefaultAsync();
                    var approval = new PlanApproval
                    {
                        ApprovingPerson = prevApprovalRequest.ApprovingPerson,
                        ApprovingPersonName = prevApprovalRequest.ApprovingPersonName,
                        PlanId = calendarVM.PlanId,
                        CreatedDate = DateTime.Now,
                        Remarks = calendarVM.PlanRemarks,
                        Status = calendarVM.PlanStatus,
                    };
                    await _repository.AddAsync(approval);
                    int DCID = Convert.ToInt16((from a in _db.Employees
                                                from b in _db.Roles
                                                where b.RoleId == a.RoleId && b.RollName == "Deputy Coordinator"
                                                select a.EmployeeId).FirstOrDefault());
                    var DeputyCoordinator = await _db.Employees.Where(x => x.EmployeeId == DCID).FirstOrDefaultAsync();
                    var ACPlanApproval = new PlanApproval
                    {
                        ApprovingPerson = "Deputy Coordinator",
                        ApprovingPersonName = DeputyCoordinator?.FName + " " + DeputyCoordinator?.LName,
                        PlanId = calendarVM.PlanId,
                        Remarks = "Status Pending",
                        Status = "Pending",
                        CreatedDate = DateTime.Now
                    };
                    await _repository.AddAsync(ACPlanApproval);
                    if (await _repository.SaveChanges())
                    {
                        return RedirectToAction("PlanApprovals");
                    }
                    ModelState.AddModelError("", "Error While Saving in Database!");
                }
                else if (User.IsInRole("Deputy Coordinator"))
                {
                    var prevApprovalRequest = await _db.PlanApproval.Where(x => x.PlanId == calendarVM.PlanId && x.IsActive == true && x.ApprovingPerson == "Deputy Coordinator" && x.Status == "Pending").FirstOrDefaultAsync();
                    var approval = new PlanApproval
                    {
                        ApprovingPerson = prevApprovalRequest.ApprovingPerson,
                        ApprovingPersonName = prevApprovalRequest.ApprovingPersonName,
                        PlanId = calendarVM.PlanId,
                        CreatedDate = DateTime.Now,
                        Remarks = calendarVM.PlanRemarks,
                        Status = calendarVM.PlanStatus,
                    };
                    await _repository.AddAsync(approval);
                    int DAID = Convert.ToInt16((from a in _db.Employees
                                                from b in _db.Roles
                                                where b.RoleId == a.RoleId && b.RollName == "Director Academics"
                                                select a.EmployeeId).FirstOrDefault());
                    var DirectorAcademics = await _db.Employees.Where(x => x.EmployeeId == DAID).FirstOrDefaultAsync();
                    var DCPlanApproval = new PlanApproval
                    {
                        ApprovingPerson = "Director Academics",
                        ApprovingPersonName = DirectorAcademics?.FName + " " + DirectorAcademics?.LName,
                        PlanId = calendarVM.PlanId,
                        Remarks = "Status Pending",
                        Status = "Pending",
                        CreatedDate = DateTime.Now
                    };
                    await _repository.AddAsync(DCPlanApproval);
                    if (await _repository.SaveChanges())
                    {
                        return RedirectToAction("PlanApprovals");
                    }
                    ModelState.AddModelError("", "Error While Saving in Database!");
                }
                else
                {
                    var prevApprovalRequest = await _db.PlanApproval.Where(x => x.PlanId == calendarVM.PlanId && x.IsActive == true && x.ApprovingPerson == "Director Academics" && x.Status == "Pending").FirstOrDefaultAsync();
                    var approval = new PlanApproval
                    {
                        ApprovingPerson = prevApprovalRequest.ApprovingPerson,
                        ApprovingPersonName = prevApprovalRequest.ApprovingPersonName,
                        PlanId = calendarVM.PlanId,
                        CreatedDate = DateTime.Now,
                        Remarks = calendarVM.PlanRemarks,
                        Status = calendarVM.PlanStatus,
                    };
                    await _repository.AddAsync(approval);
                    var plan = await _db.AcademicPlannings.Where(x => x.AcademicPlanningsId == calendarVM.PlanId).FirstOrDefaultAsync();
                    await _repository.UpdateAsync(prevApprovalRequest);
                    if (await _repository.SaveChanges())
                    {
                        return RedirectToAction("PlanApprovals");
                    }
                    ModelState.AddModelError("", "Error While Saving in Database!");
                }
            }
            int empId = Convert.ToInt16(User.FindFirst(ClaimTypes.Sid)?.Value);
            var employee = await _db.Employees.Where(x => x.EmployeeId == empId).FirstOrDefaultAsync();
            string role = User.FindFirst(ClaimTypes.Role)?.Value;
            //var ApprovalRequest = await _db.PlanApproval.Where(x => x.PlanId == calendarVM.PlanId && x.IsActive == true && x.ApprovingPerson == role && x.Status == "Pending").FirstOrDefaultAsync();
            var ApprovalRequest = new PlanApproval();
            ApprovalRequest.ApprovingPerson = role;
            ApprovalRequest.ApprovingPersonName = employee?.FName + " " + employee?.LName;
            ApprovalRequest.PlanId = calendarVM.PlanId;
            ApprovalRequest.Remarks = calendarVM.PlanRemarks;
            ApprovalRequest.Status = calendarVM.PlanStatus;
            ApprovalRequest.CreatedDate = DateTime.Now;
            await _repository.AddAsync(ApprovalRequest);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("PlanApprovals");
            }
            ModelState.AddModelError("", "Error While Saving in Database!");
            return View(calendarVM);
        }

        public async Task<IActionResult> ApprovalHistory(int PlanId)
        {
            var approvals = await _db.PlanApproval.OrderByDescending(x => x.CreatedDate).Where(x => x.PlanId == PlanId).ToListAsync();
            ViewBag.PlanName = await _db.AcademicPlannings.Where(x => x.AcademicPlanningsId == PlanId).Select(x => x.PlanName).FirstOrDefaultAsync();
            return View(approvals);
        }
        #endregion

        #region DinamicData-Comment

        //public JsonResult GetSchoolSections(int YearId)
        //{
        //    //if(User.IsInRole("Deputy Coordinator") || User.IsInRole("Director Academics"))
        //    //{
        //    //int userId = Convert.ToInt16(User.FindFirst(ClaimTypes.Sid)?.Value);
        //    var ss = (from a in _db.SchoolSections
        //              from b in _db.Employees
        //              from c in _db.SubjectTeacherAllocations
        //              from d in _db.Books
        //              from e in _db.Units
        //              from f in _db.UnitAllocations
        //              from g in _db.terms
        //              from h in _db.years
        //              where h.YearId == YearId && g.YearId == h.YearId && f.TermId == g.TermId && f.UnitId == e.UnitId && e.BookId == d.BookId && d.BookId == c.BookId && c.EmployeeId == b.EmployeeId && a.SchoolSectionId == b.SchoolSectionId
        //              select a).Distinct();
        //    return Json(ss);
        //}
        //public JsonResult GetGrades(int SchoolSectionId)
        //{
        //    var grades = (from b in _db.Grades
        //                  from c in _db.Books
        //                  from d in _db.Units
        //                  from e in _db.UnitAllocations
        //                  where b.SchoolSectionId == SchoolSectionId && c.GradeId == b.GradeId && d.BookId == c.BookId && e.UnitId == d.UnitId
        //                  select new
        //                  {
        //                      GradeId = b.GradeId,
        //                      GradeName = b.GradeName
        //                  }).Distinct();
        //    return Json(grades);
        //}
        //public JsonResult GetClasses(int GradeId)
        //{
        //    var sections = (from b in _db.Grades
        //                    from c in _db.Books
        //                    from d in _db.Units
        //                    from e in _db.UnitAllocations
        //                    from f in _db.Sections
        //                    where b.GradeId == GradeId && c.GradeId == b.GradeId && d.BookId == c.BookId && e.UnitId == d.UnitId && f.GradeId == b.GradeId
        //                    select new
        //                    {
        //                        SectionId = f.SectionId,
        //                        SectionName = f.SectionName
        //                    }).Distinct();
        //    return Json(sections);
        //}

        //public JsonResult GetBooks(int SubjectId, int GradeId)
        //{
        //    IQueryable classes;
        //    if (User.IsInRole("Subject Teacher"))
        //    {
        //        classes = (from a in _db.Books
        //                   from b in _db.Units
        //                   from c in _db.UnitAllocations
        //                   from e in _db.Subjects
        //                   where a.SubjectId == SubjectId && b.BookId == a.BookId && c.UnitId == b.UnitId
        //                   select a).Distinct();
        //    }
        //    else
        //    {
        //        classes = (from a in _db.Books
        //                   from b in _db.Units
        //                   from c in _db.UnitAllocations
        //                   from d in _db.Grades
        //                   from e in _db.Subjects
        //                   where a.SubjectId == SubjectId && d.GradeId == GradeId && a.GradeId == GradeId && b.BookId == a.BookId && c.UnitId == b.UnitId
        //                   select a).Distinct();
        //    }
        //    return Json(classes);
        //}
        #endregion

        #region Dynamic-Data
        public async Task<JsonResult> GetBooks(int SubjectId)
        {
            int empId = Convert.ToInt16(User.FindFirst(ClaimTypes.Sid)?.Value);
            var books = await (from a in _db.SubjectTeacherAllocations
                               from b in _db.Books
                               from c in _db.Subjects
                               where a.EmployeeId == empId && b.BookId == a.BookId && c.SubjectId == b.SubjectId && c.SubjectId == SubjectId && b.IsActive == true
                               select new
                               {
                                   BookId = b.BookId,
                                   BookName = b.BookName
                               }).Distinct().ToListAsync();
            return Json(books);
        }
        public async Task<JsonResult> GetUnits(int PlanId)
        {
            var units = await (from a in _db.AcademicPlannings
                               from b in _db.Units
                               from c in _db.UnitAllocations
                               where a.AcademicPlanningsId == PlanId && b.BookId == a.BookId && c.UnitId == b.UnitId && b.IsActive == true
                               select b).OrderBy(x => x.UnitName).Distinct().ToListAsync();
            //var units = await (from a in _db.AcademicPlannings
            //                   from c in _db.Units
            //                   where a.AcademicPlanningsId == PlanId && c.BookId == a.BookId && a.IsActive == true && c.IsActive == true
            //                   select c).Distinct().ToListAsync();
            return Json(units);
        }
        public async Task<JsonResult> GetChapters(int UnitId, int PlanId)
        {
            int empId = Convert.ToInt16(User.FindFirst(ClaimTypes.Sid)?.Value);
            var chaps = await (from a in _db.AcademicPlannings
                               from b in _db.Books
                               from c in _db.Units
                               from d in _db.UnitAllocations
                               from e in _db.chapters
                               from f in _db.ChapterAllocations
                               where a.EmployeeId == empId && a.AcademicPlanningsId == PlanId && b.BookId == a.BookId && c.BookId == b.BookId && d.UnitId == c.UnitId && e.UnitId == d.UnitId && f.ChapterId == e.ChapterId && e.UnitId == UnitId && c.IsActive == true && d.IsActive == true && e.IsActive == true
                               select e).Distinct().OrderBy(x => x.ChapterName).ToListAsync();
            return Json(chaps);
        }
        public async Task<JsonResult> GetTopics(int ChapterId, int PlanId, int UnitId)
        {
            int empId = Convert.ToInt16(User.FindFirst(ClaimTypes.Sid)?.Value);
            //var topics = await (from z in _db.topics
            //                      from a in _db.chapters
            //                      from b in _db.UnitAllocations
            //                      from c in _db.Units
            //                      from e in _db.Books
            //                      from f in _db.AcademicPlannings
            //                      where z.ChapterId == a.ChapterId && a.UnitId == b.UnitId && c.UnitId == b.UnitId && e.BookId == c.BookId && f.BookId == e.BookId && f.AcademicPlanningsId == plan.AcademicPlanningsId && f.IsActive == true && a.IsActive == true
            //                      select z).OrderBy(x => x.TopicName).Distinct().ToListAsync();
            //var topics = await _db.topics.Where(x => x.ChapterId == ChapterId).Distinct().ToListAsync();
            var topics = await (from a in _db.AcademicPlannings
                                from b in _db.Books
                                from c in _db.Units
                                from d in _db.UnitAllocations
                                from e in _db.chapters
                                from f in _db.ChapterAllocations
                                from g in _db.topics
                                from h in _db.TopicAllocations
                                where a.EmployeeId == empId && a.AcademicPlanningsId == PlanId && b.BookId == a.BookId && c.BookId == b.BookId && d.UnitId == c.UnitId && e.UnitId == d.UnitId && f.ChapterId == e.ChapterId && g.ChapterId == f.ChapterId && h.TopicId == g.TopicId && c.UnitId == UnitId && e.ChapterId == ChapterId && c.IsActive == true && d.IsActive == true && e.IsActive == true && g.IsActive == true
                                select g).Distinct().OrderBy(x => x.TopicName).ToListAsync();
            return Json(topics);
        }
        public async Task<JsonResult> GetSubTopics(int TopicId)
        {
            var SubTopics = await _db.subTopics.Where(x => x.TopicId == TopicId).Distinct().ToListAsync();
            return Json(SubTopics);
        }
        public async Task<JsonResult> GetSubjects(int ClasssId)
        {
            //var plan = await _db.AcademicPlannings.Where(x => x.AcademicPlanningsId == PlanId).FirstOrDefaultAsync();
            int empId = Convert.ToInt16(User.FindFirst(ClaimTypes.Sid).Value);
            var subjects = await (from a in _db.SubjectTeacherAllocations
                                  join b in _db.Books on a.BookId equals b.BookId into EmpBooks
                                  from EBs in EmpBooks.DefaultIfEmpty()
                                  join c in _db.Subjects on EBs.SubjectId equals c.SubjectId into BookSubs
                                  from BSs in BookSubs.DefaultIfEmpty()
                                  where a.EmployeeId == empId && a.SectionId == ClasssId && BSs.IsActive == true
                                  select BSs).Distinct().ToListAsync();
            return Json(subjects);
        }
        #endregion
    }
}
