using Entities.Models;
using Entities.ViewModels;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myWebApp.ViewModels.AcademicCalendar;
using Newtonsoft.Json;
using System.Collections.Immutable;
using System.Data;
using System.Globalization;
using System.Security.Claims;

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
            var allyears = await _repository.GetYears();
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
            var year = await _repository.GetYearById(id);
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
            var terms = await _db.terms.Where(x => x.YearId == year.YearId).ToListAsync();
            foreach (var term in terms)
            {
                var holidays = await _db.Holidays.Where(x => x.TermId == term.TermId).ToListAsync();
                foreach (var holiday in holidays)
                {
                    holiday.IsActive = false;
                }
            }
            if (year == null)
            {
                return NotFound();
            }
            await _repository.Delete(year);
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
            var year = _db.years.Where(x => x.YearId == Id).FirstOrDefault();
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
            var SatSun = CountWeekEnds(term.TermStartDate, term.TermEndDate);
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
                AssesmentWiseTermDays = Days - (SatSun + term.AssessmentDays)
            };
            await _repository.AddAsync(newTerm);
            if (await _repository.SaveChanges())
            {
                var LastTerm = await _db.terms.OrderBy(x => x.TermId).LastAsync();
                var Parentyear = await _db.years.Where(x => x.YearId == LastTerm.YearId).FirstOrDefaultAsync();
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
            ViewBag.Years = await _repository.GetYears();
            var term = await _repository.GetTermById(id);
            var Term = new UpdateTermVM
            {
                TermName = term.TermName,
                TermId = term.TermId,
                YearId = (int)term.YearId,
                StartDate = (DateTime)term.StartDate,
                EndDate = (DateTime)term.EndDate,
                AssessmentDays = term.AssesmentDays,
                IsActive = (bool)term.IsActive
            };
            return View(Term);
        }
        [Authorize(Policy = "Term.Update")]
        [HttpPost]
        public async Task<IActionResult> UpdateTerm(UpdateTermVM Uterm)
        {
            var totalDays = Uterm.EndDate - Uterm.StartDate;
            var SatSun = CountWeekEnds(Uterm.StartDate, Uterm.EndDate);
            int Days = (int)totalDays.TotalDays + 1;
            var term = await _repository.GetTermById(Uterm.TermId);
            if (Uterm.AssessmentDays == null) Uterm.AssessmentDays = 0;
            term.StartDate = Uterm.StartDate;
            term.EndDate = Uterm.EndDate;
            term.YearId = Uterm.YearId;
            term.TermName = Uterm.TermName;
            term.IsActive = (bool)Uterm.IsActive;
            //var LastTerm = await _db.terms.OrderBy(x => x.TermId).LastAsync();
            var ParentYear = await _db.years.Where(x => x.YearId == term.YearId).FirstOrDefaultAsync();
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
            var term = await _repository.GetTermById(id);
            var year = _db.years.Where(x => x.YearId == term.YearId).FirstOrDefault();
            var TermHolidays = await _db.Holidays.Where(x => x.TermId == id).ToListAsync();
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
            if (term == null)
            {
                return NotFound();
            }
            await _repository.Delete(term);
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
        //    var term = await _db.terms.Where(x => x.TermId == Id).FirstOrDefaultAsync();
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
        //        var months = await _db.months.Where(x => x.TermId == lastmonth.TermId)
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
        //    ViewBag.Terms = await _db.terms.Select(x => new { TermId = x.TermId, TermName = x.TermName }).ToListAsync();
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
        //    var term = await _db.terms.Where(x => x.TermId == month.TermId).FirstOrDefaultAsync();
        //    var year = await _db.years.Where(x => x.YearId == term.YearId).FirstOrDefaultAsync();
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

        #endregion

        #region Holidays
        [Authorize(Policy = "Holidays.Read")]
        [HttpGet]
        public async Task<IActionResult> Holidays(int Id, int YearId)
        {
            var holidays = await _db.Holidays.Where(x => x.TermId == Id && x.IsActive == true).ToListAsync();
            var term = await _db.terms.Where(x => x.TermId == Id).FirstOrDefaultAsync();
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
        [Authorize(Policy = "Holidays.Create")]
        [HttpPost]
        public async Task<IActionResult> Holidays(HolidaysVM data)
        {
            var newHolidays = new Holiday
            {
                TermId = data.TermId,
                HolidayName = data.HolidayName,
                StartDate = data.StartDate,
                EndDate = data.EndDate
            };
            var holidaysCount = data.EndDate - data.StartDate;
            newHolidays.NoOfHolidays = (int)holidaysCount.Value.TotalDays + 1;
            await _db.AddAsync(newHolidays);
            if (await _repository.SaveChanges())
            {
                var LastHoliday = await _db.Holidays.OrderBy(x => x.HolidayId).LastOrDefaultAsync();
                var term = await _db.terms.Where(x => x.TermId == data.TermId).FirstOrDefaultAsync();
                var year = await _db.years.Where(x => x.YearId == data.YearId).FirstOrDefaultAsync();
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
            ModelState.AddModelError("", "Error While Saving to Database");
            return View(data);
        }
        [Authorize(Policy = "Holidays.Delete")]
        [HttpGet]
        public async Task<IActionResult> DeleteHoliday(int Id, int YearId)
        {
            var holiday = await _db.Holidays.Where(x => x.HolidayId == Id).FirstOrDefaultAsync();
            var term = await _db.terms.Where(x => x.TermId == holiday.TermId).FirstOrDefaultAsync();
            var year = await _db.years.Where(x => x.YearId == YearId).FirstOrDefaultAsync();
            if (holiday == null)
            {
                return NotFound();
            }
            term.TermHolidays -= holiday.NoOfHolidays;
            term.TotalSchoolDays += holiday.NoOfHolidays;
            term.AssesmentWiseTermDays += holiday.NoOfHolidays;
            year.Holidays -= holiday.NoOfHolidays;
            year.TotalSchoolDays += holiday.NoOfHolidays;
            year.TotalAssesWiseSchoolDays += holiday.NoOfHolidays;
            holiday.IsActive = false;
            await _repository.UpdateAsync(holiday);
            await _repository.UpdateAsync(term);
            await _repository.UpdateAsync(year);
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
            SearchUnit.TermId = Id;
            SearchUnit.YearId = YearId;
            ViewBag.WorkBooks = await _db.Books.Where(x => x.IsWorkBook == true).ToListAsync();
            int empId = Convert.ToInt16(User.FindFirst(ClaimTypes.Sid)?.Value);
            ViewBag.Books = (from a in _db.Books
                             from c in _db.SubjectTeacherAllocations
                             from d in _db.Grades
                             from e in _db.Sections
                             where a.BookId == c.BookId && c.EmployeeId == empId && e.SectionId == c.SectionId && d.GradeId == e.GradeId
                             select new
                             {
                                 BookName = a.BookName,
                                 ClassName = d.GradeName + " " + e.SectionName,
                                 Value = a.BookId +"," +c.SectionId
                             }).Distinct();
            SearchUnit.BookId = Convert.ToInt16(SearchUnit.SearchValues?.Split(",")[0]);
            if (SearchUnit.BookId == 0) SearchUnit.BookId = null;
            SearchUnit.SectionId = Convert.ToInt16(SearchUnit.SearchValues?.Split(",")[1]);
            if (SearchUnit.SectionId == 0) SearchUnit.SectionId = null;
            if (SearchUnit.BookId != null)
            {
                //var AllUnits = (from a in _db.SubjectTeacherAllocations
                //                from b in _db.Books
                //                from c in _db.Units
                //                where a.EmployeeId == empId && b.BookId == a.BookId && c.BookId == a.BookId
                //                select new UnitList
                //                {
                //                    UnitName = c.UnitName,
                //                    UnitId = c.UnitId,
                //                    BookName = b.BookName
                //                }).Distinct();
                ViewBag.BookName = (await (from a in _db.Books
                                           where a.BookId == SearchUnit.BookId
                                           select a.BookName).FirstOrDefaultAsync());
                ViewBag.CalendarName = (await (from a in _db.years
                                               where a.YearId == YearId
                                               select a.YearName).FirstOrDefaultAsync());
                ViewBag.TermName = (await (from a in _db.terms
                                               where a.TermId == Id
                                               select a.TermName).FirstOrDefaultAsync());
                var AllUnits = (from b in _db.Books
                                from c in _db.Units
                                where b.BookId == SearchUnit.BookId && c.BookId == SearchUnit.BookId
                                select new UnitList
                                {
                                    UnitName = c.UnitName,
                                    UnitId = c.UnitId,
                                    BookName = b.BookName
                                }).Distinct();
                var AllocatedUnits = await _db.UnitAllocations.Where(x => x.TermId == Id && x.SectionId == SearchUnit.SectionId).ToListAsync();
                //var AllocatedChapters = await _db.UnitAllocations.Where(x => x.MonthId == Id).ToListAsync();
                //UnitAllocationVM UnitVM = new UnitAllocationVM();
                var month = await _repository.GetTermById(Id);
                ViewBag.Month = month.TermName;
                SearchUnit.MinDate = month.StartDate;
                SearchUnit.MaxDate = month.EndDate;
                //UnitVM.MonthId = Id;
                SearchUnit.Units = await AllUnits.Select(x => new UnitList { UnitId = x.UnitId, UnitName = x.UnitName, BookName = x.BookName }).ToListAsync();
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
            var UnitAllocationsToRemove = (await (from a in _db.UnitAllocations
                                                  from b in _db.Units
                                                  where a.TermId == data.TermId && b.UnitId == a.UnitId && b.BookId == data.BookId
                                                  select a).ToListAsync());
            _db.RemoveRange(UnitAllocationsToRemove);
            await _db.SaveChangesAsync();
            List<UnitAllocation> allocations = new List<UnitAllocation>();
            var selectedUnits = data.Units.Where(x => x.IsSelected == true).ToList();
            var unSelectedUnits = data.Units.Where(x => x.IsSelected == false).ToList();
            if (unSelectedUnits.Any())
            {
                var allocatedUnits = from a in unSelectedUnits
                                     from b in _db.UnitAllocations
                                     where b.UnitId == a.UnitId && b.TermId == data.TermId
                                     select b;
                var allocatedChapters = from a in unSelectedUnits
                                        from b in _db.ChapterAllocations
                                        where b.UnitId == a.UnitId && b.TermId == data.TermId
                                        select b;
                var allocatdTopics = from a in allocatedChapters
                                     from b in _db.TopicAllocations
                                     where b.ChapterId == a.ChapterId && b.TermId == data.TermId
                                     select b;
                var allocatedSubtopic = from a in allocatdTopics
                                        from b in _db.SubTopicAllocations
                                        where b.TopicId == a.TopicId && b.TermId == data.TermId
                                        select b;
                _db.RemoveRange(allocatedChapters);
                _db.RemoveRange(allocatdTopics);
                _db.RemoveRange(allocatedSubtopic);
                await _db.SaveChangesAsync();
            }
            foreach (var chapter in selectedUnits)
            {
                //var TbChapter = await _db.chapters.Where(x => x.ChapterId == chapter.ChapterId).FirstOrDefaultAsync();
                //TbChapter.IsAllocated = true;
                var newUnitAllocation = new UnitAllocation
                {
                    UnitId = chapter.UnitId,
                    StartDate = chapter.StartDate,
                    EndDate = chapter.EndDate,
                    TermId = data.TermId,
                    WorkBookId = chapter.WorkBookId,
                    WorkBookStartPage = chapter.WorkBookStartPage,
                    WorkBookEndPage = chapter.WorkBookEndPage,
                    SectionId = data.SectionId
                };
                allocations.Add(newUnitAllocation);
            }
            if (allocations.Any())
            {
                await _db.AddRangeAsync(allocations);
                if (await _repository.SaveChanges())
                {
                    //var OtherSectionBooks = from a in _db.SubjectTeacherAllocations
                    //                        from b in _db.Units
                    //                        from c in _db.UnitAllocations
                    //                        where a.EmployeeId == empId && a.BookId == data.BookId && a.SectionId != data.SectionId && b.BookId == data.BookId && c.UnitId == b.UnitId
                    //                        select c;
                    //var RecentPlannedUnit = await _db.UnitAllocations.OrderBy(x => x.UnitAllocationId).LastOrDefaultAsync();
                    //if (OtherSectionBooks.Any() == false)
                    //{
                    //    var autoPlanning = (await (from a in _db.SubjectTeacherAllocations
                    //                       from b in _db.Units
                    //                       from d in _db.UnitAllocations
                    //                       where a.EmployeeId == empId && a.BookId == data.BookId && a.SectionId != data.SectionId && b.BookId == data.BookId
                    //                       select a).Distinct().ToListAsync());
                    //    foreach (var autoUnit in autoPlanning)
                    //    {
                    //        var newUnitAllocation = new UnitAllocation
                    //        {
                    //            UnitId = RecentPlannedUnit?.UnitId,
                    //            StartDate = RecentPlannedUnit?.StartDate,
                    //            EndDate = RecentPlannedUnit?.EndDate,
                    //            TermId = data.TermId,
                    //            WorkBookId = RecentPlannedUnit?.WorkBookId,
                    //            WorkBookStartPage = RecentPlannedUnit?.WorkBookStartPage,
                    //            WorkBookEndPage = RecentPlannedUnit?.WorkBookEndPage,
                    //            SectionId = autoUnit.SectionId
                    //        };
                    //        await _repository.AddAsync(newUnitAllocation);
                    //    }
                    //    if (await _repository.SaveChanges())
                    //    {
                    //        return RedirectToAction("Term", new { Id = data.YearId });
                    //    }
                    //    ModelState.AddModelError("", "Error While Saving to Database");
                    //}
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
            SearchChapter.TermId = Id;
            SearchChapter.YearId = YearId;
            int empId = Convert.ToInt16(User.FindFirst(ClaimTypes.Sid)?.Value);
            //ViewBag.WorkBooks = await _db.Books.Where(x => x.IsWorkBook == true).ToListAsync();
            ViewBag.Units = (from a in _db.SubjectTeacherAllocations
                             from b in _db.Books
                             from c in _db.Units
                             from d in _db.UnitAllocations
                             where b.BookId == a.BookId && c.BookId == b.BookId && d.UnitId == c.UnitId && a.EmployeeId == empId && d.TermId == Id
                             select new
                             {
                                 UnitId = c.UnitId,
                                 UnitName = c.UnitName,
                                 BookName = b.BookName
                             }).Distinct();
            if (SearchChapter.UnitId != null)
            {
                ViewBag.UnitName = (await (from a in _db.Units
                                           where a.UnitId == SearchChapter.UnitId
                                           select a.UnitName).FirstOrDefaultAsync());
                ViewBag.BookName = (await (from a in _db.Units
                                           from b in _db.Books
                                           where a.UnitId == SearchChapter.UnitId && a.BookId == b.BookId
                                           select a.UnitName).FirstOrDefaultAsync());
                ViewBag.CalendarName = (await (from a in _db.years
                                               where a.YearId == YearId
                                               select a.YearName).FirstOrDefaultAsync());
                ViewBag.TermName = (await (from a in _db.terms
                                           where a.TermId == Id
                                           select a.TermName).FirstOrDefaultAsync());
                var AllChapters = (from a in _db.UnitAllocations
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
                                  }).Distinct();
                var month = await _repository.GetTermById(Id);
                //List < ChapterList > AllChapters = await _db.chapters.Select(x => new ChapterList { ChapterName = x.ChapterName, ChapterId = x.ChapterId }).ToListAsync();
                //ChapterAllocationVM ChapterVM = new ChapterAllocationVM();

                var ParentUnit = await _db.UnitAllocations.Where(x => x.TermId == Id && x.UnitId == SearchChapter.UnitId).FirstOrDefaultAsync();
                SearchChapter.MinDate = ParentUnit != null ? ParentUnit.StartDate : DateTime.Now;
                SearchChapter.MaxDate = ParentUnit != null ? ParentUnit.EndDate : DateTime.Now;
                SearchChapter.Chapters = await AllChapters.Select(x => new ChapterList { ChapterId = x.ChapterId, ChapterName = x.ChapterName, UnitId = x.UnitId, UnitName = x.UnitName, WBMinPage = x.WBMinPage, WBMaxPage = x.WBMaxPage }).Distinct().ToListAsync();
                var AllocatedChapters = await _db.ChapterAllocations.Where(x => x.TermId == Id).ToListAsync();
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
            .Where(p => p.TermId == data.TermId && p.UnitId == data.UnitId).ToList();
            _db.RemoveRange(result);
            await _db.SaveChangesAsync();
            List<ChapterAllocation> allocations = new List<ChapterAllocation>();
            var selectedChapters = data.Chapters.Where(x => x.IsSelected == true).ToList();
            var unSelectedChapters = data.Chapters.Where(x => x.IsSelected == false).ToList();
            if (unSelectedChapters.Any())
            {
                var allocatdTopics = from a in unSelectedChapters
                                     from b in _db.TopicAllocations
                                     where b.ChapterId == a.ChapterId
                                     select b;
                var allocatedSubtopic = from a in allocatdTopics
                                        from b in _db.SubTopicAllocations
                                        where b.TopicId == a.TopicId
                                        select b;
                _db.RemoveRange(allocatdTopics);
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
                    WorkBookEndPage = chapter.WorkBookEndPage
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
        public async Task<IActionResult> TopicAllocation(int Id, int YearId, TopicAllocationVM SearchTopics)
        {
            SearchTopics.TermId = Id;
            SearchTopics.YearId = YearId;
            int empId = Convert.ToInt16(User.FindFirst(ClaimTypes.Sid)?.Value);
            ViewBag.Chapters = (from a in _db.SubjectTeacherAllocations
                                from b in _db.Books
                                from c in _db.Units
                                from d in _db.chapters
                                from e in _db.ChapterAllocations
                                from f in _db.UnitAllocations
                                where e.TermId == Id && f.TermId == Id && a.EmployeeId == empId && b.BookId == a.BookId && c.BookId == a.BookId && d.UnitId == c.UnitId && e.ChapterId == d.ChapterId && e.UnitId == f.UnitId
                                select new
                                {
                                    ChapterId = e.ChapterId,
                                    ChapterName = d.ChapterName,
                                    UnitName = c.UnitName
                                }).Distinct();
            //ViewBag.WorkBooks = await _db.Books.Where(x => x.IsWorkBook == true).ToListAsync();
            if (SearchTopics.ChapterId != null)
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
                                           where c.ChapterId == SearchTopics.ChapterId && c.UnitId == b.UnitId && a.BookId == b.BookId
                                           select a.BookName).FirstOrDefaultAsync());
                ViewBag.UnitName = (await (from b in _db.Units
                                           from c in _db.chapters
                                           where c.ChapterId == SearchTopics.ChapterId && c.UnitId == b.UnitId
                                           select b.UnitName).FirstOrDefaultAsync());
                ViewBag.ChapterName = (await (from c in _db.chapters
                                           where c.ChapterId == SearchTopics.ChapterId
                                           select c.ChapterName).FirstOrDefaultAsync());
                var chapter = await _db.ChapterAllocations.Where(x => x.ChapterId == SearchTopics.ChapterId).FirstOrDefaultAsync();
                //List<TopicList> topics = new List<TopicList>();
                ////TopicAllocationVM TopicVM = new TopicAllocationVM();
                //foreach (var chapter in chapters)
                //{
                List<TopicList> ChapterTopics = await _db.topics.Where(x => x.ChapterId == SearchTopics.ChapterId).Select(x => new TopicList { TopicId = x.TopicId, TopicName = x.TopicName }).Distinct().ToListAsync();
                foreach (var each in ChapterTopics)
                {
                    each.WBMaxPage = chapter.WorkBookEndPage;
                    each.WBMinPage = chapter.WorkBookStartPage;
                }
                //topics.AddRange(ChapterTopics);
                //}
                SearchTopics.Topics = ChapterTopics;
                var allocatinos = from t in ChapterTopics
                                  from allo in _db.TopicAllocations
                                  where t.TopicId == allo.TopicId
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
                                  };
                foreach (var topic in ChapterTopics)
                {
                    var selectedTopic = await _db.TopicAllocations.Where(x => x.TopicId == topic.TopicId).FirstOrDefaultAsync();
                    if (selectedTopic != null)
                    {
                        var selectedChapter = await _db.ChapterAllocations.Where(x => x.ChapterId == selectedTopic.ChapterId).FirstOrDefaultAsync();
                        topic.ChapterStartDate = selectedChapter?.StartDate;
                        topic.ChapterEndDate = selectedChapter?.EndDate;
                    }
                    else
                    {
                        var nonSelectedTopic = await _db.topics.Where(x => x.TopicId == topic.TopicId).FirstOrDefaultAsync();
                        var nonSelectedChapter = await _db.ChapterAllocations.Where(x => x.ChapterId == nonSelectedTopic.ChapterId).FirstOrDefaultAsync();
                        topic.ChapterStartDate = nonSelectedChapter?.StartDate;
                        topic.ChapterEndDate = nonSelectedChapter?.EndDate;
                    }
                    foreach (var allo in allocatinos)
                    {
                        if (topic.TopicId == allo.topic)
                        {
                            topic.preAllocation = true;
                            topic.StartDate = allo.startDate;
                            topic.EndDate = allo.endDate;
                            topic.TeachingMethodologyId = allo.TeachingMethodologyId;
                            topic.TeachingMethodologyDesc = allo.TMethodDesc;
                            topic.WorkBookId = allo.WorkBookId;
                            topic.WorkBookStartPage = allo.WorkBookStartPage;
                            topic.WorkBookEndPage = allo.WorkBookEndPage;
                        }
                    }
                }
                var month = await _repository.GetTermById(Id);
                ViewBag.Month = month.TermName;
                var Dates = _db.ChapterAllocations.Where(x => x.TermId == Id && x.ChapterId == SearchTopics.ChapterId).Select(x => new { StartDate = x.StartDate, EndDate = x.EndDate }).ToList();
                //if (await _db.ChapterAllocations.ToListAsync().Result.Any()) ;
                var chapAllocation = await _db.ChapterAllocations.Where(x => x.TermId == Id).FirstOrDefaultAsync();
                SearchTopics.MinDate = chapAllocation != null ? chapAllocation.StartDate : DateTime.Now;
                SearchTopics.MaxDate = chapAllocation != null ? chapAllocation.EndDate : DateTime.Now;
                ViewBag.TMethods = await _db.TeachingMethodologies.ToListAsync();
                return View(SearchTopics);
            }
            return View(SearchTopics);
        }
        [Authorize(Policy = "Academic Planning")]
        [HttpPost]
        public async Task<IActionResult> TopicAllocation(TopicAllocationVM data)
        {
            var oldTopics = _db.TopicAllocations
            .Where(p => p.TermId.Equals(data.TermId) && p.ChapterId == data.ChapterId).ToList();
            _db.RemoveRange(oldTopics);
            await _db.SaveChangesAsync();
            var test = data.Topics.Where(x => x.IsSelected == true).ToList();
            var unSelectedTopics = data.Topics.Where(x => x.IsSelected == false).ToList();
            if (unSelectedTopics.Any())
            {
                var subTopics = from a in unSelectedTopics
                                from b in _db.SubTopicAllocations
                                where b.TopicId == a.TopicId
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
                bid.preAllocation = true;
                var topic = await _db.topics.Where(x => x.TopicId == bid.TopicId).FirstOrDefaultAsync();
                var chapter = await _db.ChapterAllocations.Where(x => x.ChapterId == topic.ChapterId).FirstOrDefaultAsync();
                var newTopic = new TopicAllocation
                {
                    TermId = data.TermId,
                    TopicId = bid.TopicId,
                    ChapterId = chapter.ChapterId,
                    StartDate = bid.StartDate,
                    EndDate = bid.EndDate,
                    TeachingMethodologyId = bid.TeachingMethodologyId,
                    TMethodDesc = bid.TeachingMethodologyDesc,
                    WorkBookId = bid.WorkBookId,
                    WorkBookEndPage = bid.WorkBookEndPage,
                    WorkBookStartPage = bid.WorkBookStartPage
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
            //ViewBag.WorkBooks = await _db.Books.Where(x => x.IsWorkBook == true).ToListAsync();
            SearchSubTopics.TermId = Id;
            SearchSubTopics.YearId = YearId;
            ViewBag.Topics = (from a in _db.SubjectTeacherAllocations
                             from b in _db.Books
                             from c in _db.Units
                             from d in _db.UnitAllocations
                             from f in _db.ChapterAllocations
                             from g in _db.chapters
                             from h in _db.TopicAllocations
                             from i in _db.topics
                             where a.EmployeeId == empId && b.BookId == a.BookId && c.BookId == b.BookId && d.UnitId == c.UnitId && f.UnitId == d.UnitId && h.ChapterId == f.ChapterId && i.TopicId == h.TopicId && g.ChapterId == f.ChapterId && d.TermId == Id && f.TermId == Id && h.TermId == Id
                             select new
                             {
                                 TopicId = i.TopicId,
                                 TopicName = i.TopicName,
                                 ChapterName = g.ChapterName
                             }).Distinct();
            if (SearchSubTopics.TopicId != null)
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
                var subTopics = (from a in _db.ChapterAllocations
                                from b in _db.TopicAllocations
                                from c in _db.subTopics
                                where a.TermId == Id && b.ChapterId == a.ChapterId && c.TopicId == b.TopicId && c.TopicId == SearchSubTopics.TopicId
                                select c).Distinct();
                //SubTopicAllocationVM subTopic = new SubTopicAllocationVM();
                if (subTopics.Any())
                {
                    var allocatedSubTopics = from a in _db.ChapterAllocations
                                             from b in _db.TopicAllocations
                                             from c in _db.SubTopicAllocations
                                             where a.TermId == Id && b.ChapterId == a.ChapterId && c.TopicId == b.TopicId && c.TopicId == SearchSubTopics.TopicId
                                             select c;
                    List<SubTopicList> subTopicList = await subTopics.Select(x => new SubTopicList { SubTopicId = x.SubTopicId, SubTopicName = x.SubTopicName }).ToListAsync();
                    foreach (var topic in subTopicList)
                    {
                        var selectedSubTopic = await _db.subTopics.Where(x => x.SubTopicId == topic.SubTopicId).FirstOrDefaultAsync();
                        var SelectedTopic = await _db.TopicAllocations.Where(x => x.TopicId == selectedSubTopic.TopicId).FirstOrDefaultAsync();
                        topic.TopicStartDate = SelectedTopic.StartDate;
                        topic.TopicEndDate = SelectedTopic.EndDate;
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
                    var ParentTopic = await _db.TopicAllocations.Where(x => x.TopicId == SearchSubTopics.TopicId && x.TermId == Id).FirstOrDefaultAsync();
                    SearchSubTopics.MinDate = ParentTopic.StartDate;
                    SearchSubTopics.MaxDate = ParentTopic.EndDate;
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
                               where a.TermId == data.TermId && b.ChapterId == a.ChapterId && c.TopicId == b.TopicId && c.TopicId == data.TopicId
                               select c;
            _db.RemoveRange(oldsubTopics);
            await _db.SaveChangesAsync();
            var test = data.SubTopics.Where(x => x.IsSelected == true).ToList();
            List<SubTopicAllocation> allocations = new List<SubTopicAllocation>();
            foreach (var bid in test)
            {
                bid.preAllocation = true;
                var SubTopic = await _db.subTopics.Where(x => x.SubTopicId == bid.SubTopicId).FirstOrDefaultAsync();
                var newSubTopic = new SubTopicAllocation
                {
                    TermId = data.TermId,
                    SubTopicId = bid.SubTopicId,
                    TopicId = SubTopic.TopicId,
                    StartDate = bid.StartDate,
                    EndDate = bid.EndDate,
                    WorkBookId = bid.WorkBookId,
                    WorkBookStartPage = bid.WorkBookStartPage,
                    WorkBookEndPage = bid.WorkBookEndPage
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

        #region Calendar
        //[Authorize(Policy = "Planner.Read")]
        [HttpGet]
        public async Task<IActionResult> Calendar(CalendarVM calender)
        {
            ViewBag.BookSelected = true;

            #region Filters

            //thinking as director academics

            int userId = Convert.ToInt16(this.User.FindFirst(ClaimTypes.Sid)?.Value);
            if (User.IsInRole("Director Academics") || User.IsInRole("Deputy Coordinator"))
            {
                ViewBag.Calendars = (from a in _db.years
                                     from b in _db.terms
                                     from c in _db.UnitAllocations
                                     where b.TermId == c.TermId && a.YearId == b.YearId
                                     select new
                                     {
                                         YearId = a.YearId,
                                         YearName = a.YearName
                                     }).Distinct();
                ViewBag.SchoolSections = (from a in _db.SchoolSections
                                          from b in _db.Employees
                                          from c in _db.SubjectTeacherAllocations
                                          from d in _db.Books
                                          from e in _db.Units
                                          from f in _db.UnitAllocations
                                          from g in _db.terms
                                          from h in _db.years
                                          where h.YearId == calender.YearId && g.YearId == h.YearId && f.TermId == g.TermId && f.UnitId == e.UnitId && e.BookId == d.BookId && d.BookId == c.BookId && c.EmployeeId == b.EmployeeId && a.SchoolSectionId == b.SchoolSectionId
                                          select a).Distinct();
            }
            if (User.IsInRole("Assistant Coordinator"))
            {
                var user = await _db.Employees.Where(x => x.EmployeeId == userId).FirstOrDefaultAsync();
                ViewBag.Grades = (from b in _db.Grades
                                  from c in _db.Books
                                  from d in _db.Units
                                  from e in _db.UnitAllocations
                                  where b.SchoolSectionId == user.SchoolSectionId && c.GradeId == b.GradeId && d.BookId == c.BookId && e.UnitId == d.UnitId
                                  select new
                                  {
                                      GradeId = b.GradeId,
                                      GradeName = b.GradeName
                                  }).Distinct();
            }
            else if (User.IsInRole("Grade Manager"))
            {
                ViewBag.Grades = (from b in _db.Grades
                                  from c in _db.Books
                                  from d in _db.Units
                                  from e in _db.UnitAllocations
                                  where b.GradeManagerId == userId && c.GradeId == b.GradeId && d.BookId == c.BookId && e.UnitId == d.UnitId
                                  select new
                                  {
                                      GradeId = b.GradeId,
                                      GradeName = b.GradeName
                                  }).Distinct();
            }
            else
            {
                ViewBag.Grades = (from b in _db.Grades
                                  from c in _db.Books
                                  from d in _db.Units
                                  from e in _db.UnitAllocations
                                  where c.GradeId == b.GradeId && d.BookId == c.BookId && e.UnitId == d.UnitId
                                  select new
                                  {
                                      GradeId = b.GradeId,
                                      GradeName = b.GradeName
                                  }).Distinct();
            }
            if (User.IsInRole("Subject Teacher"))
            {
                ViewBag.Subjects = (from a in _db.Books
                                    from b in _db.Subjects
                                    from c in _db.SubjectTeacherAllocations
                                    from d in _db.Units
                                    from e in _db.UnitAllocations
                                    where a.BookId == c.BookId && c.EmployeeId == userId && b.SubjectId == a.SubjectId && d.BookId == a.BookId && e.UnitId == d.UnitId
                                    select new
                                    {
                                        SubjectId = b.SubjectId,
                                        SubjectName = b.SubjectName
                                    }).Distinct();
            }
            else
            {
                ViewBag.Subjects = (from a in _db.Subjects
                                    from grade in _db.Grades
                                    from c in _db.Books
                                    from d in _db.Units
                                    from e in _db.UnitAllocations
                                    where grade.GradeId == calender.GradeId && c.GradeId == grade.GradeId && a.SubjectId == c.SubjectId && d.BookId == c.BookId && e.UnitId == d.UnitId
                                    select new
                                    {
                                        SubjectId = a.SubjectId,
                                        SubjectName = a.SubjectName
                                    }).Distinct();
            }
            ViewBag.Books = (from a in _db.Books
                             from b in _db.Units
                             from c in _db.UnitAllocations
                             from d in _db.Grades
                             from e in _db.Subjects
                             where a.SubjectId == calender.SubjectId && d.GradeId == calender.GradeId && a.GradeId == calender.GradeId && b.BookId == a.BookId && c.UnitId == b.UnitId
                             select a).Distinct();
            #endregion

            #region Comment
            //ViewBag.Grades = await _db.Grades.ToListAsync();
            //ViewBag.Subjects = await _db.Subjects.ToListAsync();
            //if (calender.BookId != null)
            //{
            //    var bookdata = from a in _db.Books
            //                   join c in _db.Grades on a.GradeId equals c.GradeId into bookGrade
            //                   from grade in bookGrade.DefaultIfEmpty()
            //                   join b in _db.Sections on grade.GradeId equals b.GradeId into bookClass
            //                   from CLASS in bookClass.DefaultIfEmpty()
            //                   from d in _db.SubjectTeacherAllocations
            //                   join e in _db.Employees on d.EmployeeId equals e.EmployeeId into BookTeacher
            //                   from teacher in BookTeacher.DefaultIfEmpty()
            //                   where a.BookId == calender.BookId && d.BookId == calender.BookId
            //                   select new
            //                   {
            //                       ClassName = grade.GradeName + CLASS.SectionName,
            //                       BookName = a.BookName,
            //                       TeacherName = teacher.FName + " " + teacher.LName
            //                   };

            //ViewBag.BookName = bookdata.FirstOrDefault()?.BookName;
            //ViewBag.ClassName = bookdata.FirstOrDefault()?.ClassName;
            //ViewBag.TeacherName = bookdata.FirstOrDefault()?.TeacherName;
            //}
            //else
            //{
            //    ViewBag.BookName = "Select Book Please!";
            //    ViewBag.ClassName = "Select Book Please!";
            //    ViewBag.TeacherName = "Select Book Please!";
            //}
            //ViewBag.Books = from a in _db.SubjectTeacherAllocations
            //                from b in _db.Books
            //                where a.EmployeeId == userId && b.BookId == a.BookId
            //                select b;
            #endregion

            int totalItems = 0;
            calender.years = await _db.years.Select(x => new YearList { YearName = x.YearName, EndDate = x.EndDate, Holidays = x.Holidays, IsLeapYear = x.IsLeapYear, StartDate = x.StartDate, TotalAssesWiseSchoolDays = x.TotalAssesWiseSchoolDays, TotalDays = x.TotalDays, TotalSatSundays = x.TotalSatSundays, TotalSchoolDays = x.TotalSchoolDays, YearId = x.YearId }).Where(x => x.YearId == calender.YearId).ToListAsync();

            #region NoFilters
            //if ((calender.BookId == null || calender.YearId == 0) && (calender.GradeId == null || calender.GradeId == 0) && (calender.SubjectId == null || calender.SubjectId == 0) && (calender.BookId == null || calender.BookId == 0))
            //{
            //    totalItems += calender.years.Count;
            //    foreach (var year in calender.years)
            //    {
            //        var terms = from term in _db.terms
            //                    where term.YearId == year.YearId
            //                    select new TermList
            //                    {
            //                        YearId = term.YearId,
            //                        EndDate = term.EndDate,
            //                        Holidays = term.TermHolidays,
            //                        StartDate = term.StartDate,
            //                        TermId = term.TermId,
            //                        TermName = term.TermName,
            //                        TotalDays = term.TotalDays,
            //                        TotalSatSundays = term.TotalSatSun,
            //                        TotalSchoolDays = term.TotalSchoolDays,
            //                        YearName = year.YearName,
            //                        AssesmentDays = term.AssesmentDays,
            //                        AssessmentWiseSchoolDays = term.AssesmentWiseTermDays
            //                    };
            //        year.terms = await terms.Select(x => new TermList { AssessmentWiseSchoolDays = x.AssessmentWiseSchoolDays, EndDate = x.EndDate, Holidays = x.Holidays, TotalDays = x.TotalDays, StartDate = x.StartDate, TermId = x.TermId, TermName = x.TermName, TotalSatSundays = x.TotalSatSundays, TotalSchoolDays = x.TotalSchoolDays, YearId = x.YearId, YearName = x.YearName, AssesmentDays = x.AssesmentDays }).ToListAsync();
            //        totalItems += year.terms.Count;
            //        foreach (var term in year.terms)
            //        {
            //            var books = await _db.Books.ToListAsync();
            //            term.Books = books.Select(x => new BookListVM { BookId = x.BookId, BookName = x.BookName }).ToList();
            //            totalItems += term.Books.Count;
            //            foreach (var book in term.Books)
            //            {
            //                var units = from allocatedUnit in _db.UnitAllocations
            //                            join un in _db.Units on allocatedUnit.UnitId equals un.UnitId into BookUnit
            //                            from bunit in BookUnit.DefaultIfEmpty()
            //                            from unit in _db.Units
            //                            from bok in _db.Books
            //                            where allocatedUnit.TermId == term.TermId && unit.UnitId == allocatedUnit.UnitId && bok.BookId == unit.BookId && bunit.BookId == book.BookId
            //                            select new UnitList
            //                            {
            //                                UnitId = (int)allocatedUnit.UnitId,
            //                                UnitName = unit.UnitName,
            //                                StartDate = allocatedUnit.StartDate,
            //                                EndDate = allocatedUnit.EndDate,
            //                                TermId = (int)allocatedUnit.TermId,
            //                                BookName = bok.BookName
            //                            };
            //                book.units = await units.Select(x => new UnitList { BookName = x.BookName, TermId = x.TermId, EndDate = x.EndDate, StartDate = x.StartDate, UnitId = x.UnitId, UnitName = x.UnitName }).ToListAsync();
            //                totalItems += term.units.Count;
            //                foreach (var unit in book.units)
            //                {
            //                    var chapters = from Allocatedchapter in _db.ChapterAllocations
            //                                   from chapter in _db.chapters
            //                                   from bok in _db.Units
            //                                   where Allocatedchapter.UnitId == unit.UnitId && chapter.ChapterId == Allocatedchapter.ChapterId && bok.UnitId == chapter.UnitId
            //                                   select new ChapterList
            //                                   {
            //                                       ChapterId = (int)Allocatedchapter.ChapterId,
            //                                       ChapterName = chapter.ChapterName,
            //                                       StartDate = Allocatedchapter.StartDate,
            //                                       EndDate = Allocatedchapter.EndDate,
            //                                       UnitId = (int)Allocatedchapter.UnitId,
            //                                       UnitName = bok.UnitName,
            //                                       TermId = (int)term.TermId
            //                                   };
            //                    unit.chapters = await chapters.Select(x => new ChapterList { ChapterId = (int)x.ChapterId, UnitId = x.UnitId, TermId = x.TermId, StartDate = x.StartDate, EndDate = x.EndDate, UnitName = x.UnitName, ChapterName = x.ChapterName }).ToListAsync();
            //                    totalItems += unit.chapters.Count;
            //                    foreach (var chapter in unit.chapters)
            //                    {
            //                        var topics = from AllocatedTopic in _db.TopicAllocations
            //                                     join TChapter in _db.chapters on AllocatedTopic.ChapterId equals TChapter.ChapterId into TopicChapter
            //                                     from chap in TopicChapter.DefaultIfEmpty()
            //                                     join topic in _db.topics on AllocatedTopic.TopicId equals topic.TopicId into AlloTopic
            //                                     from ATopic in AlloTopic.DefaultIfEmpty()
            //                                     join Methodology in _db.TeachingMethodologies on AllocatedTopic.TeachingMethodologyId equals Methodology.TeachingMethodologyId into TopicMethodology
            //                                     from method in TopicMethodology.DefaultIfEmpty()
            //                                     select new TopicList
            //                                     {
            //                                         ChapterId = AllocatedTopic.ChapterId,
            //                                         StartDate = AllocatedTopic.StartDate,
            //                                         EndDate = AllocatedTopic.EndDate,
            //                                         ChapterName = chap.ChapterName,
            //                                         TopicId = AllocatedTopic.TopicId,
            //                                         TopicName = ATopic.TopicName,
            //                                         TeachingMethodology = method.TMethodologyName,
            //                                         TeachingMethodologyDesc = AllocatedTopic.TMethodDesc
            //                                     };
            //                        chapter.topics = await topics.Select(x => new TopicList { ChapterId = (int)x.ChapterId, StartDate = x.StartDate, EndDate = x.EndDate, TopicId = (int)x.TopicId, ChapterName = x.ChapterName, TopicName = x.TopicName, TeachingMethodology = x.TeachingMethodology, TeachingMethodologyDesc = x.TeachingMethodologyDesc }).ToListAsync();
            //                        totalItems += chapter.topics.Count;
            //                        foreach (var topic in chapter.topics)
            //                        {
            //                            var subTopics = from AllocatedsubTopic in _db.SubTopicAllocations
            //                                            from subTopic in _db.subTopics
            //                                            from _topic in _db.topics
            //                                            where AllocatedsubTopic.TopicId == topic.TopicId && subTopic.SubTopicId == AllocatedsubTopic.SubTopicId && _topic.TopicId == AllocatedsubTopic.TopicId
            //                                            select new SubTopicList
            //                                            {
            //                                                EndDate = AllocatedsubTopic.EndDate,
            //                                                StartDate = AllocatedsubTopic.StartDate,
            //                                                TopicId = (int)AllocatedsubTopic.TopicId,
            //                                                SubTopicId = (int)AllocatedsubTopic.SubTopicId,
            //                                                SubTopicName = subTopic.SubTopicName,
            //                                                TopicName = _topic.TopicName

            //                                            };
            //                            topic.subTopics = await subTopics.Select(x => new SubTopicList { TopicId = (int)x.TopicId, EndDate = x.EndDate, StartDate = x.StartDate, TopicName = x.TopicName, SubTopicId = x.SubTopicId, SubTopicName = x.SubTopicName }).ToListAsync();
            //                            totalItems += topic.subTopics.Count;
            //                        }
            //                    }
            //                }

            //            }
            //        }

            //    }
            //}
            #endregion

            #region FilterCodeComment

            ////If the grade is choosed
            //#region GradeFilter
            //else if ((calender.GradeId != null || calender.GradeId > 0) && (calender.SubjectId == null || calender.SubjectId == 0) && (calender.BookId == null || calender.BookId == 0))
            //{
            //    totalItems += calender.years.Count;
            //    foreach (var year in calender.years)
            //    {
            //        var terms = from term in _db.terms
            //                    where term.YearId == year.YearId
            //                    select new TermList
            //                    {
            //                        YearId = term.YearId,
            //                        EndDate = term.EndDate,
            //                        Holidays = term.TermHolidays,
            //                        StartDate = term.StartDate,
            //                        TermId = term.TermId,
            //                        TermName = term.TermName,
            //                        TotalDays = term.TotalDays,
            //                        TotalSatSundays = term.TotalSatSun,
            //                        TotalSchoolDays = term.TotalSchoolDays,
            //                        YearName = year.YearName,
            //                        AssesmentDays = term.AssesmentDays,
            //                        AssessmentWiseSchoolDays = term.AssesmentWiseTermDays
            //                    };
            //        year.terms = await terms.Select(x => new TermList { AssessmentWiseSchoolDays = x.AssessmentWiseSchoolDays, EndDate = x.EndDate, Holidays = x.Holidays, TotalDays = x.TotalDays, StartDate = x.StartDate, TermId = x.TermId, TermName = x.TermName, TotalSatSundays = x.TotalSatSundays, TotalSchoolDays = x.TotalSchoolDays, YearId = x.YearId, YearName = x.YearName, AssesmentDays = x.AssesmentDays }).ToListAsync();
            //        totalItems += year.terms.Count;
            //        foreach (var term in year.terms)
            //        {
            //            var books = await _db.Books.Where(x => x.GradeId == calender.GradeId).ToListAsync();
            //            term.Books = books.Select(x => new BookListVM { BookId = x.BookId, BookName = x.BookName }).ToList();
            //            totalItems += term.Books.Count;
            //            foreach (var book in term.Books)
            //            {
            //                var units = from allcoatedUnit in _db.UnitAllocations
            //                            join un in _db.Units on allcoatedUnit.UnitId equals un.UnitId into BookUnit
            //                            from bunit in BookUnit.DefaultIfEmpty()
            //                            from unit in _db.Units
            //                            from bok in _db.Books
            //                            where bok.BookId == unit.BookId && allcoatedUnit.TermId == term.TermId && unit.UnitId == allcoatedUnit.UnitId && bunit.BookId == book.BookId
            //                            select new UnitList
            //                            {
            //                                UnitId = (int)allcoatedUnit.UnitId,
            //                                UnitName = unit.UnitName,
            //                                StartDate = allcoatedUnit.StartDate,
            //                                EndDate = allcoatedUnit.EndDate,
            //                                TermId = (int)allcoatedUnit.TermId,
            //                                BookName = book.BookName
            //                            };
            //                book.units = await units.Select(x => new UnitList { BookName = x.BookName, TermId = x.TermId, EndDate = x.EndDate, StartDate = x.StartDate, UnitId = x.UnitId, UnitName = x.UnitName }).ToListAsync();
            //                totalItems += term.units.Count;
            //                foreach (var unit in book.units)
            //                {
            //                    var chapters = from Allocatedchapter in _db.ChapterAllocations
            //                                   from chapter in _db.chapters
            //                                   from bok in _db.Units
            //                                   where Allocatedchapter.UnitId == unit.UnitId && chapter.ChapterId == Allocatedchapter.ChapterId && bok.UnitId == chapter.UnitId
            //                                   select new ChapterList
            //                                   {
            //                                       ChapterId = (int)Allocatedchapter.ChapterId,
            //                                       ChapterName = chapter.ChapterName,
            //                                       StartDate = Allocatedchapter.StartDate,
            //                                       EndDate = Allocatedchapter.EndDate,
            //                                       UnitId = (int)Allocatedchapter.UnitId,
            //                                       UnitName = bok.UnitName,
            //                                       TermId = (int)term.TermId
            //                                   };
            //                    unit.chapters = await chapters.Select(x => new ChapterList { ChapterId = (int)x.ChapterId, UnitId = x.UnitId, TermId = x.TermId, StartDate = x.StartDate, EndDate = x.EndDate, UnitName = x.UnitName, ChapterName = x.ChapterName }).ToListAsync();
            //                    totalItems += unit.chapters.Count;
            //                    foreach (var chapter in unit.chapters)
            //                    {
            //                        var topics = from AllocatedTopic in _db.TopicAllocations
            //                                     join TChapter in _db.chapters on AllocatedTopic.ChapterId equals TChapter.ChapterId into TopicChapter
            //                                     from chap in TopicChapter.DefaultIfEmpty()
            //                                     join topic in _db.topics on AllocatedTopic.TopicId equals topic.TopicId into AlloTopic
            //                                     from ATopic in AlloTopic.DefaultIfEmpty()
            //                                     join Methodology in _db.TeachingMethodologies on AllocatedTopic.TeachingMethodologyId equals Methodology.TeachingMethodologyId into TopicMethodology
            //                                     from method in TopicMethodology.DefaultIfEmpty()
            //                                     select new TopicList
            //                                     {
            //                                         ChapterId = AllocatedTopic.ChapterId,
            //                                         StartDate = AllocatedTopic.StartDate,
            //                                         EndDate = AllocatedTopic.EndDate,
            //                                         ChapterName = chap.ChapterName,
            //                                         TopicId = AllocatedTopic.TopicId,
            //                                         TopicName = ATopic.TopicName,
            //                                         TeachingMethodology = method.TMethodologyName,
            //                                         TeachingMethodologyDesc = AllocatedTopic.TMethodDesc
            //                                     };
            //                        chapter.topics = await topics.Select(x => new TopicList { ChapterId = (int)x.ChapterId, StartDate = x.StartDate, EndDate = x.EndDate, TopicId = (int)x.TopicId, ChapterName = x.ChapterName, TopicName = x.TopicName, TeachingMethodology = x.TeachingMethodology, TeachingMethodologyDesc = x.TeachingMethodologyDesc }).ToListAsync();
            //                        totalItems += chapter.topics.Count;
            //                        foreach (var topic in chapter.topics)
            //                        {
            //                            var subTopics = from AllocatedsubTopic in _db.SubTopicAllocations
            //                                            from subTopic in _db.subTopics
            //                                            from _topic in _db.topics
            //                                            where AllocatedsubTopic.TopicId == topic.TopicId && subTopic.SubTopicId == AllocatedsubTopic.SubTopicId && _topic.TopicId == AllocatedsubTopic.TopicId
            //                                            select new SubTopicList
            //                                            {
            //                                                EndDate = AllocatedsubTopic.EndDate,
            //                                                StartDate = AllocatedsubTopic.StartDate,
            //                                                TopicId = (int)AllocatedsubTopic.TopicId,
            //                                                SubTopicId = (int)AllocatedsubTopic.SubTopicId,
            //                                                SubTopicName = subTopic.SubTopicName,
            //                                                TopicName = _topic.TopicName

            //                                            };
            //                            topic.subTopics = await subTopics.Select(x => new SubTopicList { TopicId = (int)x.TopicId, EndDate = x.EndDate, StartDate = x.StartDate, TopicName = x.TopicName, SubTopicId = x.SubTopicId, SubTopicName = x.SubTopicName }).ToListAsync();
            //                            totalItems += topic.subTopics.Count;
            //                        }
            //                    }
            //                }

            //            }
            //        }

            //    }
            //}
            #endregion

            //If the grade and subject
            #region GradeAndSubjectFilter
            //else if ((calender.GradeId != null || calender.GradeId > 0) && (calender.SubjectId != null || calender.SubjectId > 0) && (calender.BookId == null || calender.BookId == 0))
            //{
            //    totalItems += calender.years.Count;
            //    foreach (var year in calender.years)
            //    {
            //        var terms = from term in _db.terms
            //                    where term.YearId == year.YearId
            //                    select new TermList
            //                    {
            //                        YearId = term.YearId,
            //                        EndDate = term.EndDate,
            //                        Holidays = term.TermHolidays,
            //                        StartDate = term.StartDate,
            //                        TermId = term.TermId,
            //                        TermName = term.TermName,
            //                        TotalDays = term.TotalDays,
            //                        TotalSatSundays = term.TotalSatSun,
            //                        TotalSchoolDays = term.TotalSchoolDays,
            //                        YearName = year.YearName,
            //                        AssesmentDays = term.AssesmentDays,
            //                        AssessmentWiseSchoolDays = term.AssesmentWiseTermDays
            //                    };
            //        year.terms = await terms.Select(x => new TermList { AssessmentWiseSchoolDays = x.AssessmentWiseSchoolDays, EndDate = x.EndDate, Holidays = x.Holidays, TotalDays = x.TotalDays, StartDate = x.StartDate, TermId = x.TermId, TermName = x.TermName, TotalSatSundays = x.TotalSatSundays, TotalSchoolDays = x.TotalSchoolDays, YearId = x.YearId, YearName = x.YearName, AssesmentDays = x.AssesmentDays }).ToListAsync();
            //        totalItems += year.terms.Count;
            //        foreach (var term in year.terms)
            //        {
            //            var books = await _db.Books.Where(x => x.GradeId == calender.GradeId && x.SubjectId == calender.SubjectId).ToListAsync();
            //            term.Books = books.Select(x => new BookListVM { BookId = x.BookId, BookName = x.BookName }).ToList();
            //            totalItems += term.Books.Count;
            //            foreach (var book in term.Books)
            //            {
            //                var units = from allcoatedUnit in _db.UnitAllocations
            //                            join u in _db.Units on allcoatedUnit.UnitId equals u.UnitId into BookUnit
            //                            from bunit in BookUnit.DefaultIfEmpty()
            //                            from unit in _db.Units
            //                            from bok in _db.Books
            //                            where allcoatedUnit.TermId == term.TermId && unit.UnitId == allcoatedUnit.UnitId && bok.BookId == unit.BookId && bunit.BookId == book.BookId
            //                            select new UnitList
            //                            {
            //                                UnitId = (int)allcoatedUnit.UnitId,
            //                                UnitName = unit.UnitName,
            //                                StartDate = allcoatedUnit.StartDate,
            //                                EndDate = allcoatedUnit.EndDate,
            //                                TermId = (int)allcoatedUnit.TermId,
            //                                BookName = book.BookName
            //                            };
            //                book.units = await units.Select(x => new UnitList { BookName = x.BookName, TermId = x.TermId, EndDate = x.EndDate, StartDate = x.StartDate, UnitId = x.UnitId, UnitName = x.UnitName }).ToListAsync();
            //                totalItems += term.units.Count;
            //                foreach (var unit in book.units)
            //                {
            //                    var chapters = from Allocatedchapter in _db.ChapterAllocations
            //                                   from chapter in _db.chapters
            //                                   from bok in _db.Units
            //                                   where Allocatedchapter.UnitId == unit.UnitId && chapter.ChapterId == Allocatedchapter.ChapterId && bok.UnitId == chapter.UnitId
            //                                   select new ChapterList
            //                                   {
            //                                       ChapterId = (int)Allocatedchapter.ChapterId,
            //                                       ChapterName = chapter.ChapterName,
            //                                       StartDate = Allocatedchapter.StartDate,
            //                                       EndDate = Allocatedchapter.EndDate,
            //                                       UnitId = (int)Allocatedchapter.UnitId,
            //                                       UnitName = bok.UnitName,
            //                                       TermId = (int)term.TermId
            //                                   };
            //                    unit.chapters = await chapters.Select(x => new ChapterList { ChapterId = (int)x.ChapterId, UnitId = x.UnitId, TermId = x.TermId, StartDate = x.StartDate, EndDate = x.EndDate, UnitName = x.UnitName, ChapterName = x.ChapterName }).ToListAsync();
            //                    totalItems += unit.chapters.Count;
            //                    foreach (var chapter in unit.chapters)
            //                    {
            //                        var topics = from AllocatedTopic in _db.TopicAllocations
            //                                     join TChapter in _db.chapters on AllocatedTopic.ChapterId equals TChapter.ChapterId into TopicChapter
            //                                     from chap in TopicChapter.DefaultIfEmpty()
            //                                     join topic in _db.topics on AllocatedTopic.TopicId equals topic.TopicId into AlloTopic
            //                                     from ATopic in AlloTopic.DefaultIfEmpty()
            //                                     join Methodology in _db.TeachingMethodologies on AllocatedTopic.TeachingMethodologyId equals Methodology.TeachingMethodologyId into TopicMethodology
            //                                     from method in TopicMethodology.DefaultIfEmpty()
            //                                     select new TopicList
            //                                     {
            //                                         ChapterId = AllocatedTopic.ChapterId,
            //                                         StartDate = AllocatedTopic.StartDate,
            //                                         EndDate = AllocatedTopic.EndDate,
            //                                         ChapterName = chap.ChapterName,
            //                                         TopicId = AllocatedTopic.TopicId,
            //                                         TopicName = ATopic.TopicName,
            //                                         TeachingMethodology = method.TMethodologyName,
            //                                         TeachingMethodologyDesc = AllocatedTopic.TMethodDesc
            //                                     };
            //                        chapter.topics = await topics.Select(x => new TopicList { ChapterId = (int)x.ChapterId, StartDate = x.StartDate, EndDate = x.EndDate, TopicId = (int)x.TopicId, ChapterName = x.ChapterName, TopicName = x.TopicName, TeachingMethodology = x.TeachingMethodology, TeachingMethodologyDesc = x.TeachingMethodologyDesc }).ToListAsync();
            //                        totalItems += chapter.topics.Count;
            //                        foreach (var topic in chapter.topics)
            //                        {
            //                            var subTopics = from AllocatedsubTopic in _db.SubTopicAllocations
            //                                            from subTopic in _db.subTopics
            //                                            from _topic in _db.topics
            //                                            where AllocatedsubTopic.TopicId == topic.TopicId && subTopic.SubTopicId == AllocatedsubTopic.SubTopicId && _topic.TopicId == AllocatedsubTopic.TopicId
            //                                            select new SubTopicList
            //                                            {
            //                                                EndDate = AllocatedsubTopic.EndDate,
            //                                                StartDate = AllocatedsubTopic.StartDate,
            //                                                TopicId = (int)AllocatedsubTopic.TopicId,
            //                                                SubTopicId = (int)AllocatedsubTopic.SubTopicId,
            //                                                SubTopicName = subTopic.SubTopicName,
            //                                                TopicName = _topic.TopicName

            //                                            };
            //                            topic.subTopics = await subTopics.Select(x => new SubTopicList { TopicId = (int)x.TopicId, EndDate = x.EndDate, StartDate = x.StartDate, TopicName = x.TopicName, SubTopicId = x.SubTopicId, SubTopicName = x.SubTopicName }).ToListAsync();
            //                            totalItems += topic.subTopics.Count;
            //                        }
            //                    }
            //                }

            //            }
            //        }

            //    }
            //}
            #endregion

            ////If All Filters Applied
            //#region GradeSubjectBookFilter
            //else if ((calender.GradeId != null || calender.GradeId > 0) && (calender.SubjectId != null || calender.SubjectId > 0) && (calender.BookId != null || calender.BookId > 0))
            //{
            #region Working Version
            totalItems += calender.years.Count;
            foreach (var year in calender.years)
            {
                var terms = from term in _db.terms
                            where term.YearId == year.YearId
                            select new TermList
                            {
                                YearId = term.YearId,
                                EndDate = term.EndDate,
                                Holidays = term.TermHolidays,
                                StartDate = term.StartDate,
                                TermId = term.TermId,
                                TermName = term.TermName,
                                TotalDays = term.TotalDays,
                                TotalSatSundays = term.TotalSatSun,
                                TotalSchoolDays = term.TotalSchoolDays,
                                YearName = year.YearName,
                                AssesmentDays = term.AssesmentDays,
                                AssessmentWiseSchoolDays = term.AssesmentWiseTermDays
                            };
                year.terms = await terms.Select(x => new TermList { AssessmentWiseSchoolDays = x.AssessmentWiseSchoolDays, EndDate = x.EndDate, Holidays = x.Holidays, TotalDays = x.TotalDays, StartDate = x.StartDate, TermId = x.TermId, TermName = x.TermName, TotalSatSundays = x.TotalSatSundays, TotalSchoolDays = x.TotalSchoolDays, YearId = x.YearId, YearName = x.YearName, AssesmentDays = x.AssesmentDays }).ToListAsync();
                totalItems += year.terms.Count;
                foreach (var term in year.terms)
                {
                    //var books = await _db.Books.Where(x => x.BookId == calender.BookId).ToListAsync();
                    //term.Books = books.Select(x => new BookListVM { BookId = x.BookId, BookName = x.BookName }).ToList();
                    //totalItems += term.Books.Count;
                    //foreach (var book in term.Books)
                    //{
                    if (calender.BookId == null)
                    {
                        ViewBag.BookSelected = false;
                        return View(calender);
                    }
                    var units = from allcoatedUnit in _db.UnitAllocations
                                join unit in _db.Units on allcoatedUnit.UnitId equals unit.UnitId into UnitDetails
                                from unitDetail in UnitDetails.DefaultIfEmpty()
                                join book in _db.Books on unitDetail.BookId equals book.BookId into unitBook
                                from uBook in unitBook.DefaultIfEmpty()
                                join wbook in _db.Books on allcoatedUnit.WorkBookId equals wbook.BookId into WorkBook
                                from workbook in WorkBook.DefaultIfEmpty()
                                where allcoatedUnit.UnitId == unitDetail.UnitId && uBook.BookId == calender.BookId && allcoatedUnit.TermId == term.TermId
                                select new UnitList
                                {
                                    UnitId = (int)allcoatedUnit.UnitId,
                                    UnitName = unitDetail.UnitName,
                                    StartDate = allcoatedUnit.StartDate,
                                    EndDate = allcoatedUnit.EndDate,
                                    TermId = (int)allcoatedUnit.TermId,
                                    BookName = workbook.BookName,
                                    WorkBookStartPage = allcoatedUnit.WorkBookStartPage,
                                    WorkBookEndPage = allcoatedUnit.WorkBookEndPage
                                };
                    term.units = await units.Select(x => new UnitList { WorkBookEndPage = x.WorkBookEndPage, WorkBookStartPage = x.WorkBookStartPage, BookName = x.BookName, TermId = x.TermId, EndDate = x.EndDate, StartDate = x.StartDate, UnitId = x.UnitId, UnitName = x.UnitName }).ToListAsync();
                    totalItems += term.units.Count;
                    foreach (var unit in term.units)
                    {
                        var chapters = from Allocatedchapter in _db.ChapterAllocations
                                       from chapter in _db.chapters
                                       from bok in _db.Units
                                       where Allocatedchapter.UnitId == unit.UnitId && chapter.ChapterId == Allocatedchapter.ChapterId && bok.UnitId == chapter.UnitId && Allocatedchapter.TermId == term.TermId
                                       select new ChapterList
                                       {
                                           ChapterId = (int)Allocatedchapter.ChapterId,
                                           ChapterName = chapter.ChapterName,
                                           StartDate = Allocatedchapter.StartDate,
                                           EndDate = Allocatedchapter.EndDate,
                                           UnitId = (int)Allocatedchapter.UnitId,
                                           UnitName = bok.UnitName,
                                           TermId = (int)term.TermId,
                                           WorkBookStartPage = Allocatedchapter.WorkBookStartPage,
                                           WorkBookEndPage = Allocatedchapter.WorkBookEndPage
                                       };
                        unit.chapters = await chapters.Select(x => new ChapterList { WorkBookEndPage = x.WorkBookEndPage, WorkBookStartPage = x.WorkBookStartPage, ChapterId = (int)x.ChapterId, UnitId = x.UnitId, TermId = x.TermId, StartDate = x.StartDate, EndDate = x.EndDate, UnitName = x.UnitName, ChapterName = x.ChapterName }).ToListAsync();
                        totalItems += unit.chapters.Count;
                        foreach (var chapter in unit.chapters)
                        {
                            var topics = from AllocatedTopic in _db.TopicAllocations
                                         join TChapter in _db.chapters on AllocatedTopic.ChapterId equals TChapter.ChapterId into TopicChapter
                                         from chap in TopicChapter.DefaultIfEmpty()
                                         join topic in _db.topics on AllocatedTopic.TopicId equals topic.TopicId into AlloTopic
                                         from ATopic in AlloTopic.DefaultIfEmpty()
                                         join Methodology in _db.TeachingMethodologies on AllocatedTopic.TeachingMethodologyId equals Methodology.TeachingMethodologyId into TopicMethodology
                                         from method in TopicMethodology.DefaultIfEmpty()
                                         where AllocatedTopic.TermId == term.TermId
                                         select new TopicList
                                         {
                                             ChapterId = AllocatedTopic.ChapterId,
                                             StartDate = AllocatedTopic.StartDate,
                                             EndDate = AllocatedTopic.EndDate,
                                             ChapterName = chap.ChapterName,
                                             TopicId = AllocatedTopic.TopicId,
                                             TopicName = ATopic.TopicName,
                                             TeachingMethodology = method.TMethodologyName,
                                             TeachingMethodologyDesc = AllocatedTopic.TMethodDesc,
                                             WorkBookStartPage = AllocatedTopic.WorkBookStartPage,
                                             WorkBookEndPage = AllocatedTopic.WorkBookEndPage
                                         };
                            chapter.topics = await topics.Select(x => new TopicList { WorkBookEndPage = x.WorkBookEndPage, WorkBookStartPage = x.WorkBookEndPage, ChapterId = (int)x.ChapterId, StartDate = x.StartDate, EndDate = x.EndDate, TopicId = (int)x.TopicId, ChapterName = x.ChapterName, TopicName = x.TopicName, TeachingMethodology = x.TeachingMethodology, TeachingMethodologyDesc = x.TeachingMethodologyDesc }).ToListAsync();
                            totalItems += chapter.topics.Count;
                            foreach (var topic in chapter.topics)
                            {
                                var subTopics = from AllocatedsubTopic in _db.SubTopicAllocations
                                                from subTopic in _db.subTopics
                                                from _topic in _db.topics
                                                where AllocatedsubTopic.TopicId == topic.TopicId && subTopic.SubTopicId == AllocatedsubTopic.SubTopicId && _topic.TopicId == AllocatedsubTopic.TopicId && AllocatedsubTopic.TermId == term.TermId
                                                select new SubTopicList
                                                {
                                                    EndDate = AllocatedsubTopic.EndDate,
                                                    StartDate = AllocatedsubTopic.StartDate,
                                                    TopicId = (int)AllocatedsubTopic.TopicId,
                                                    SubTopicId = (int)AllocatedsubTopic.SubTopicId,
                                                    SubTopicName = subTopic.SubTopicName,
                                                    TopicName = _topic.TopicName,
                                                    WorkBookEndPage = AllocatedsubTopic.WorkBookEndPage,
                                                    WorkBookStartPage = AllocatedsubTopic.WorkBookStartPage
                                                };
                                topic.subTopics = await subTopics.Select(x => new SubTopicList { WorkBookStartPage = x.WorkBookStartPage, WorkBookEndPage = x.WorkBookEndPage, TopicId = x.TopicId, EndDate = x.EndDate, StartDate = x.StartDate, TopicName = x.TopicName, SubTopicId = x.SubTopicId, SubTopicName = x.SubTopicName }).ToListAsync();
                                totalItems += topic.subTopics.Count;
                            }
                        }
                    }

                    //}
                }

            }
            //}
            #endregion

            #region Client-View

            var allocatedUnits = (from a in _db.UnitAllocations
                                  from b in _db.Units
                                  from c in _db.Books
                                  where b.UnitId == a.UnitId && b.BookId == c.BookId && c.BookId == calender.BookId
                                  select new
                                  {
                                      UnitName = b.UnitName,
                                      StartDate = a.StartDate,
                                      EndDate = a.EndDate,
                                      UnitId = a.UnitId,
                                      WBStartPage = a.WorkBookStartPage,
                                      WBEndPage = a.WorkBookEndPage,
                                      SLO = b.SLO
                                  }).Distinct();
            foreach (var unit in allocatedUnits)
            {
                var AllocatedChapters = from a in _db.ChapterAllocations
                                        join b in _db.chapters on a.ChapterId equals b.ChapterId into chapNames
                                        from names in chapNames.DefaultIfEmpty()
                                        where a.UnitId == unit.UnitId
                                        select new
                                        {
                                            ChapterName = names.ChapterName,
                                            StartDate = a.StartDate,
                                            EndDate = a.EndDate,
                                            ChapterId = a.ChapterId,
                                            WBStartPage = a.WorkBookStartPage,
                                            WBEndPage = a.WorkBookEndPage,
                                            SLO = names.SLO
                                        };
                if (!AllocatedChapters.Any())
                {
                    PLanList Unitplan = new PLanList() { StartDate = Convert.ToDateTime(unit.StartDate).ToString("dd-MMM-yyyy"), EndDate = Convert.ToDateTime(unit.EndDate).ToString("dd-MMM-yyyy"), UnitName = unit.UnitName /*, ChapterName = chapter.ChapterName, TopicName = topic.TopicName, TeachingMethodologyName = topic.TeachingMethodology,*/ , WbStartPage = unit.WBStartPage.ToString(), WbEndPage = unit.WBEndPage.ToString(), SLO = unit.SLO };
                    calender.Plan.Add(Unitplan);
                }
                foreach (var chapter in AllocatedChapters)
                {
                    var AllocatedTopics = from a in _db.TopicAllocations
                                          join b in _db.topics on a.TopicId equals b.TopicId into topNames
                                          from names in topNames.DefaultIfEmpty()
                                          join c in _db.TeachingMethodologies on a.TeachingMethodologyId equals c.TeachingMethodologyId into TMethods
                                          from TMethod in TMethods.DefaultIfEmpty()
                                          where a.ChapterId == chapter.ChapterId
                                          select new
                                          {
                                              TopicName = names.TopicName,
                                              StartDate = a.StartDate,
                                              EndDate = a.EndDate,
                                              TopicId = a.TopicId,
                                              TeachingMethodology = TMethod.TMethodologyName,
                                              WBStartPage = a.WorkBookStartPage,
                                              WBEndPage = a.WorkBookEndPage
                                          };
                    if (!AllocatedTopics.Any())
                    {
                        PLanList Chapterplan = new PLanList() { StartDate = Convert.ToDateTime(chapter.StartDate).ToString("dd-MMM-yyyy"), EndDate = Convert.ToDateTime(chapter.EndDate).ToString("dd-MMM-yyyy"), UnitName = unit.UnitName, ChapterName = chapter.ChapterName, /*TopicName = topic.TopicName, TeachingMethodologyName = topic.TeachingMethodology,*/ WbStartPage = chapter.WBStartPage.ToString(), WbEndPage = chapter.WBEndPage.ToString(), SLO = chapter.SLO };
                        calender.Plan.Add(Chapterplan);
                    }
                    else
                    {
                        foreach (var topic in AllocatedTopics)
                        {
                            var subTopics = from a in _db.SubTopicAllocations
                                            join b in _db.subTopics on a.SubTopicId equals b.SubTopicId into SubtopNames
                                            from names in SubtopNames.DefaultIfEmpty()
                                            where a.TopicId == topic.TopicId
                                            select new
                                            {
                                                TopicName = names.SubTopicName,
                                                StartDate = a.StartDate,
                                                EndDate = a.EndDate,
                                                WBStartPage = a.WorkBookStartPage,
                                                WBEndPage = a.WorkBookEndPage
                                            };
                            if (!subTopics.Any())
                            {
                                PLanList TopicPlan = new PLanList() { StartDate = Convert.ToDateTime(topic.StartDate).ToString("dd-MMM-yyyy"), EndDate = Convert.ToDateTime(topic.EndDate).ToString("dd-MMM-yyyy"), UnitName = unit.UnitName, ChapterName = chapter.ChapterName, TopicName = topic.TopicName, TeachingMethodologyName = topic.TeachingMethodology, WbStartPage = topic.WBStartPage.ToString(), WbEndPage = topic.WBEndPage.ToString() };
                                calender.Plan.Add(TopicPlan);
                            }
                            else
                            {
                                foreach (var stopic in subTopics)
                                {
                                    PLanList plan = new PLanList() { StartDate = Convert.ToDateTime(stopic.StartDate).ToString("dd-MMM-yyyy"), EndDate = Convert.ToDateTime(stopic.EndDate).ToString("dd-MMM-yyyy"), UnitName = unit.UnitName, ChapterName = chapter.ChapterName, TopicName = topic.TopicName, TeachingMethodologyName = topic.TeachingMethodology, WbStartPage = stopic.WBStartPage.ToString(), WbEndPage = stopic.WBEndPage.ToString(), SubTopicName = stopic.TopicName };
                                    calender.Plan.Add(plan);
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            calender.GradeName = (await (from a in _db.Books
                                         from b in _db.Grades
                                         where a.BookId == calender.BookId && b.GradeId == a.GradeId
                                         select b.GradeName).FirstOrDefaultAsync())?.ToString();

            calender.SubjectName = (await (from a in _db.Books
                                           join b in _db.Subjects on a.SubjectId equals b.SubjectId into bSubject
                                           from sub in bSubject
                                           where a.BookId == calender.BookId
                                           select sub.SubjectName).FirstOrDefaultAsync())?.ToString();
            calender.Textbook = _db.Books.Where(x => x.BookId == calender.BookId).FirstOrDefault()?.BookName;

            calender.Workbook = (await (from a in _db.Books
                                        from b in _db.Grades
                                        where a.BookId == calender.BookId && b.GradeId == a.BookId && a.IsWorkBook == true
                                        select a.BookName).FirstOrDefaultAsync())?.ToString();

            calender.TeacherName = (await (from a in _db.SubjectTeacherAllocations
                                           from d in _db.Employees
                                           where a.BookId == calender.BookId && d.EmployeeId == a.EmployeeId
                                           select d.FName + " " + d.LName).FirstOrDefaultAsync())?.ToString();

            calender.ClassName = (await (from a in _db.Books
                                         from b in _db.Grades
                                         from c in _db.Sections
                                         where a.BookId == calender.BookId && b.GradeId == a.GradeId && c.GradeId == b.GradeId
                                         select c.SectionName).FirstOrDefaultAsync())?.ToString();
            calender.totalItems = totalItems;
            return View(calender);
        }

        #region Comment
        //public async Task<JsonResult> MonthFreeDates(int TermId)
        //{
        //    var term = await _db.terms.Where(x => x.TermId == TermId).FirstOrDefaultAsync();
        //    var dates = from month in _db.months
        //                where month.TermId == TermId
        //                select new
        //                {
        //                    StartDate = month.StartDate,
        //                    EndDate = month.EndDate
        //                };
        //    var monthdates = dates.ToList();
        //    List<DateTime> alldates = new List<DateTime>();
        //    //foreach (var date in dates)
        //    //{
        //    //    var CurrentDate = date.EndDate.Value;
        //    //    while (CurrentDate != term.EndDate.Value)
        //    //    {
        //    //        var nextDate = CurrentDate.AddDays(1);
        //    //        alldates.Add(nextDate);
        //    //        CurrentDate = nextDate;
        //    //    }
        //    //}
        //    foreach (var date in dates)
        //    {
        //        var firstDate = date.StartDate.Value.AddDays(-1);
        //        var secondDate = date.EndDate.Value;
        //        var diff = (secondDate - firstDate).TotalDays;
        //        var nextDate = date.StartDate.Value;
        //        for (int i = 0; i < diff; i++)
        //        {
        //            alldates.Add(nextDate);
        //            nextDate = nextDate.AddDays(1);
        //        }

        //    }
        //for (int i = 0; i < monthdates.Count; i++)
        //{
        //    var diff = monthdates[i].EndDate - monthdates[i].StartDate;

        //    if (monthdates[i].StartDate.Value > term.StartDate.Value)
        //    {
        //        var termDate = term.StartDate.Value;
        //        while (termDate != monthdates[i].StartDate.Value)
        //        {
        //            var nextDate = termDate.AddDays(1);
        //            alldates.Add(nextDate);
        //            termDate = nextDate;
        //        }
        //    }
        //    else if (monthdates[i].EndDate < term.EndDate)
        //    {
        //        var mEndDate = monthdates[i].EndDate.Value;
        //        while (mEndDate != term.EndDate.Value)
        //        {
        //            var nextDate = mEndDate.AddDays(1);
        //            alldates.Add(nextDate);
        //            mEndDate = nextDate;
        //        }
        //    }
        //    else if (i > 0)
        //    {
        //        if (monthdates[i].StartDate.Value > monthdates[i - 1].EndDate.Value)
        //        {
        //            var NextMonthEDate = monthdates[i - 1].EndDate.Value;
        //            while (NextMonthEDate != monthdates[i].StartDate.Value)
        //            {
        //                var nextDate = NextMonthEDate.AddDays(1);
        //                alldates.Add(nextDate);
        //                NextMonthEDate = nextDate;
        //            }
        //        }
        //    }
        //    else if (i < (monthdates.Count - 1))
        //    {
        //        if (monthdates[i].EndDate.Value < monthdates[i + 1].StartDate.Value)
        //        {
        //            var mEndDate = monthdates[i].EndDate.Value;
        //            while (mEndDate != monthdates[i + 1].StartDate.Value)
        //            {
        //                var nextDate = mEndDate.AddDays(1);
        //                alldates.Add(nextDate);
        //                mEndDate = nextDate;
        //            }
        //        }
        //    }
        //}

        //    var json = JsonConvert.SerializeObject(alldates);
        //    return Json(json);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Calendars(CalendarVM calenderVM)
        //{
        //    calenderVM.years = await _db.years.Where(x => x.YearId == calenderVM.YearId).Select(x => new YearList { YearName = x.YearName, EndDate = x.EndDate, Holidays = x.Holidays, IsLeapYear = x.IsLeapYear, StartDate = x.StartDate, TotalAssesWiseSchoolDays = x.TotalAssesWiseSchoolDays, TotalDays = x.TotalDays, TotalSatSundays = x.TotalSatSundays, TotalSchoolDays = x.TotalSchoolDays, YearId = x.YearId }).ToListAsync();
        //    foreach (var year in calenderVM.years)
        //    {
        //        var terms = from term in _db.terms
        //                    where term.YearId == year.YearId
        //                    select new TermList
        //                    {
        //                        YearId = term.YearId,
        //                        EndDate = term.EndDate,
        //                        Holidays = term.TermHolidays,
        //                        StartDate = term.StartDate,
        //                        TermId = term.TermId,
        //                        TermName = term.TermName,
        //                        TotalDays = term.TotalDays,
        //                        TotalSatSundays = term.TotalSatSun,
        //                        TotalSchoolDays = term.TotalSchoolDays,
        //                        YearName = year.YearName,
        //                        AssesmentDays = term.AssesmentDays,
        //                        AssessmentWiseSchoolDays = term.AssesmentWiseTermDays
        //                    };
        //        year.terms = await terms.Select(x => new TermList { AssessmentWiseSchoolDays = x.AssessmentWiseSchoolDays, EndDate = x.EndDate, Holidays = x.Holidays, TotalDays = x.TotalDays, StartDate = x.StartDate, TermId = x.TermId, TermName = x.TermName, TotalSatSundays = x.TotalSatSundays, TotalSchoolDays = x.TotalSchoolDays, YearId = x.YearId, YearName = x.YearName, AssesmentDays = x.AssesmentDays }).ToListAsync();
        //        foreach (var term in year.terms)
        //        {
        //            var units = from allcoatedUnit in _db.UnitAllocations
        //                        from unit in _db.Units
        //                        from book in _db.Books
        //                        where allcoatedUnit.TermId == term.TermId && unit.UnitId == allcoatedUnit.UnitId && book.BookId == calenderVM.BookId && book.BookId == unit.BookId
        //                        select new UnitList
        //                        {
        //                            UnitId = (int)allcoatedUnit.UnitId,
        //                            UnitName = unit.UnitName,
        //                            StartDate = allcoatedUnit.StartDate,
        //                            EndDate = allcoatedUnit.EndDate,
        //                            TermId = (int)allcoatedUnit.TermId,
        //                            BookName = book.BookName
        //                        };
        //            term.units = await units.Select(x => new UnitList { BookName = x.BookName, TermId = x.TermId, EndDate = x.EndDate, StartDate = x.StartDate, UnitId = x.UnitId, UnitName = x.UnitName }).ToListAsync();

        //            foreach (var unit in term.units)
        //            {
        //                var chapters = from Allocatedchapter in _db.ChapterAllocations
        //                               from chapter in _db.chapters
        //                               from book in _db.Units
        //                               where Allocatedchapter.UnitId == unit.UnitId && chapter.ChapterId == Allocatedchapter.ChapterId && book.UnitId == chapter.UnitId
        //                               select new ChapterList
        //                               {
        //                                   ChapterId = (int)Allocatedchapter.ChapterId,
        //                                   ChapterName = chapter.ChapterName,
        //                                   StartDate = Allocatedchapter.StartDate,
        //                                   EndDate = Allocatedchapter.EndDate,
        //                                   UnitId = (int)Allocatedchapter.UnitId,
        //                                   UnitName = book.UnitName,
        //                                   TermId = (int)term.TermId
        //                               };
        //                unit.chapters = await chapters.Select(x => new ChapterList { ChapterId = (int)x.ChapterId, UnitId = x.UnitId, TermId = x.TermId, StartDate = x.StartDate, EndDate = x.EndDate, UnitName = x.UnitName, ChapterName = x.ChapterName }).ToListAsync();
        //                //totalItems += unit.chapters.Count;
        //                foreach (var chapter in unit.chapters)
        //                {
        //                    var topics = from AllocatedTopic in _db.TopicAllocations
        //                                 join TChapter in _db.chapters on AllocatedTopic.ChapterId equals TChapter.ChapterId into TopicChapter
        //                                 from chap in TopicChapter.DefaultIfEmpty()
        //                                 join topic in _db.topics on AllocatedTopic.TopicId equals topic.TopicId into AlloTopic
        //                                 from ATopic in AlloTopic.DefaultIfEmpty()
        //                                 join Methodology in _db.TeachingMethodologies on AllocatedTopic.TeachingMethodologyId equals Methodology.TeachingMethodologyId into TopicMethodology
        //                                 from method in TopicMethodology.DefaultIfEmpty()
        //                                 select new TopicList
        //                                 {
        //                                     ChapterId = AllocatedTopic.ChapterId,
        //                                     StartDate = AllocatedTopic.StartDate,
        //                                     EndDate = AllocatedTopic.EndDate,
        //                                     ChapterName = chap.ChapterName,
        //                                     TopicId = AllocatedTopic.TopicId,
        //                                     TopicName = ATopic.TopicName,
        //                                     TeachingMethodology = method.TMethodologyName,
        //                                     TeachingMethodologyDesc = AllocatedTopic.TMethodDesc
        //                                 };
        //                    chapter.topics = await topics.Select(x => new TopicList { ChapterId = (int)x.ChapterId, StartDate = x.StartDate, EndDate = x.EndDate, TopicId = (int)x.TopicId, ChapterName = x.ChapterName, TopicName = x.TopicName, TeachingMethodology = x.TeachingMethodology, TeachingMethodologyDesc = x.TeachingMethodologyDesc }).ToListAsync();

        //                    foreach (var topic in chapter.topics)
        //                    {
        //                        var subTopics = from AllocatedsubTopic in _db.SubTopicAllocations
        //                                        from subTopic in _db.subTopics
        //                                        from _topic in _db.topics
        //                                        where AllocatedsubTopic.TopicId == topic.TopicId && subTopic.SubTopicId == AllocatedsubTopic.SubTopicId && _topic.TopicId == AllocatedsubTopic.TopicId
        //                                        select new SubTopicList
        //                                        {
        //                                            EndDate = AllocatedsubTopic.EndDate,
        //                                            StartDate = AllocatedsubTopic.StartDate,
        //                                            TopicId = (int)AllocatedsubTopic.TopicId,
        //                                            SubTopicId = (int)AllocatedsubTopic.SubTopicId,
        //                                            SubTopicName = subTopic.SubTopicName,
        //                                            TopicName = _topic.TopicName

        //                                        };
        //                        topic.subTopics = await subTopics.Select(x => new SubTopicList { TopicId = (int)x.TopicId, EndDate = x.EndDate, StartDate = x.StartDate, TopicName = x.TopicName, SubTopicId = x.SubTopicId, SubTopicName = x.SubTopicName }).ToListAsync();

        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return RedirectToAction("Calendar", new { calender = calenderVM });
        //}

        //Function that does nothing and only to tackle the datatable ajax eroor

        public JsonResult AjaxError()
        {
            return Json("");
        }
        #endregion
        #endregion

        #region DinamicData

        public JsonResult GetSchoolSections(int YearId)
        {
            //if(User.IsInRole("Deputy Coordinator") || User.IsInRole("Director Academics"))
            //{
            //int userId = Convert.ToInt16(User.FindFirst(ClaimTypes.Sid)?.Value);
            var ss = (from a in _db.SchoolSections
                      from b in _db.Employees
                      from c in _db.SubjectTeacherAllocations
                      from d in _db.Books
                      from e in _db.Units
                      from f in _db.UnitAllocations
                      from g in _db.terms
                      from h in _db.years
                      where h.YearId == YearId && g.YearId == h.YearId && f.TermId == g.TermId && f.UnitId == e.UnitId && e.BookId == d.BookId && d.BookId == c.BookId && c.EmployeeId == b.EmployeeId && a.SchoolSectionId == b.SchoolSectionId
                      select a).Distinct();
            return Json(ss);
        }

        //public async Task<JsonResult> GetBooks(int Subjectid, int GradeId)
        //{
        //    var books = await _db.Books.Where(x => x.GradeId == GradeId && x.SubjectId == Subjectid).ToListAsync();
        //    return Json(books);
        public JsonResult GetGrades(int SchoolSectionId)
        {
            var grades = (from b in _db.Grades
                          from c in _db.Books
                          from d in _db.Units
                          from e in _db.UnitAllocations
                          where b.SchoolSectionId == SchoolSectionId && c.GradeId == b.GradeId && d.BookId == c.BookId && e.UnitId == d.UnitId
                          select new
                          {
                              GradeId = b.GradeId,
                              GradeName = b.GradeName
                          }).Distinct();
            return Json(grades);
        }

        public JsonResult GetSubjects(int GradeId)
        {
            var subjects = (from a in _db.Subjects
                            from grade in _db.Grades
                            from c in _db.Books
                            from d in _db.Units
                            from e in _db.UnitAllocations
                            where grade.GradeId == GradeId && c.GradeId == grade.GradeId && a.SubjectId == c.SubjectId && d.BookId == c.BookId && e.UnitId == d.UnitId
                            select new
                            {
                                SubjectId = a.SubjectId,
                                SubjectName = a.SubjectName
                            }).Distinct();
            return Json(subjects);
        }
        public JsonResult GetBooks(int SubjectId, int GradeId)
        {
            IQueryable classes;
            if (User.IsInRole("Subject Teacher"))
            {
                classes = (from a in _db.Books
                           from b in _db.Units
                           from c in _db.UnitAllocations
                           from e in _db.Subjects
                           where a.SubjectId == SubjectId && b.BookId == a.BookId && c.UnitId == b.UnitId
                           select a).Distinct();
            }
            else
            {
                classes = (from a in _db.Books
                           from b in _db.Units
                           from c in _db.UnitAllocations
                           from d in _db.Grades
                           from e in _db.Subjects
                           where a.SubjectId == SubjectId && d.GradeId == GradeId && a.GradeId == GradeId && b.BookId == a.BookId && c.UnitId == b.UnitId
                           select a).Distinct();
            }
            return Json(classes);
        }
        #endregion
    }
}
