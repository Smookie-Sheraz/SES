using Entities.Models;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myWebApp.ViewModels.AcademicCalendar;
using Newtonsoft.Json;
using System.Data;
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
            ViewBag.Year = year.YearName;
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
            term.Terms = await AllTerms.Select(x => new TermList {IsActive = x.IsActive, TermId = x.TermId,AssessmentWiseSchoolDays = x.AssessmentWiseSchoolDays, AssesmentDays = x.AssesmentDays, EndDate = x.EndDate, Holidays = x.Holidays, StartDate = x.StartDate, TermName = x.TermName, TotalDays = x.TotalDays, TotalSatSundays = x.TotalSatSundays, TotalSchoolDays = x.TotalSchoolDays,YearName = x.YearName }).ToListAsync();
            return View(term);
        }
        [Authorize(Policy = "Term.Create")]
        [HttpPost]
        public async Task<IActionResult> Term(TermVM term)
        {
            var totalDays = term.TermEndDate - term.TermStartDate;
            var SatSun = CountWeekEnds(term.TermStartDate,term.TermEndDate);
            int Days = (int)totalDays.TotalDays + 1;
            term.AssessmentDays = term.AssessmentDays == null? 0: term.AssessmentDays;
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
                if(Parentyear.TotalSchoolDays == null)
                {
                    Parentyear.TotalSchoolDays = 0;
                    Parentyear.TotalSchoolDays += LastTerm.TotalSchoolDays;
                }
                else
                {
                    Parentyear.TotalSchoolDays += LastTerm.TotalSchoolDays;
                }
                if(Parentyear.AssesmentDays == null)
                {
                    Parentyear.AssesmentDays = 0;
                    Parentyear.AssesmentDays += LastTerm.AssesmentDays;
                }
                else
                {
                    Parentyear.AssesmentDays += LastTerm.AssesmentDays;
                }
                if(Parentyear.TotalAssesWiseSchoolDays == null)
                {
                    Parentyear.TotalAssesWiseSchoolDays = 0;
                    Parentyear.TotalAssesWiseSchoolDays += LastTerm.AssesmentWiseTermDays;
                }
                else
                {
                    Parentyear.TotalAssesWiseSchoolDays += LastTerm.AssesmentWiseTermDays;
                }
                if(Parentyear.TotalDays == null)
                {
                    Parentyear.TotalDays = 0;
                    Parentyear.TotalDays += LastTerm.TotalDays;
                }
                else
                {
                    Parentyear.TotalDays += LastTerm.TotalDays;
                }
                await _repository.UpdateAsync(Parentyear);
                if(await _repository.SaveChanges())
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
            var SatSun = CountWeekEnds(Uterm.StartDate,Uterm.EndDate);
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
                ParentYear.TotalDays-= term.TotalDays;
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

        //#region Month
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

        //#endregion

        #region Holidays
        [Authorize(Policy = "Holidays.Read")]
        [HttpGet]
        public async Task<IActionResult> Holidays(int Id,int YearId)
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
                if (term.TermHolidays != null) { 
                    term.TermHolidays += LastHoliday.NoOfHolidays;
                }
                else
                {
                    term.TermHolidays = 0;
                    term.TermHolidays += LastHoliday.NoOfHolidays;
                }
                term.TotalSchoolDays -= LastHoliday.NoOfHolidays;
                term.AssesmentWiseTermDays -= LastHoliday.NoOfHolidays;
                if(year.Holidays != null)
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
                if(await _repository.SaveChanges())
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
        [HttpGet]
        public async Task<IActionResult> UnitAllocation(int Id, int YearId)
        {
            ViewBag.WorkBooks = await _db.Books.Where(x => x.IsWorkBook == true).ToListAsync();
            int empId = Convert.ToInt16(User.FindFirst(ClaimTypes.Sid)?.Value);
            var AllUnits = from a in _db.SubjectTeacherAllocations
                           join b in _db.Books on a.BookId equals b.BookId into TeacherBooks
                           from c in TeacherBooks
                           join d in _db.Units on c.BookId equals d.BookId into BookUnits
                           from Unit in BookUnits
                           where a.EmployeeId == empId
                           select new UnitList
                           {
                               UnitName = Unit.UnitName,
                               UnitId = Unit.UnitId,
                               BookName = c.BookName
                           };
            var AllocatedUnits = await _db.UnitAllocations.Where(x => x.TermId == Id).ToListAsync();
            //var AllocatedChapters = await _db.UnitAllocations.Where(x => x.MonthId == Id).ToListAsync();
            UnitAllocationVM UnitVM = new UnitAllocationVM();
            var month = await _repository.GetTermById(Id);
            ViewBag.Month = month.TermName;
            UnitVM.MinDate = month.StartDate;
            UnitVM.MaxDate = month.EndDate;
            //UnitVM.MonthId = Id;
            UnitVM.Units = await AllUnits.Select(x => new UnitList { UnitId = x.UnitId, UnitName = x.UnitName, BookName = x.BookName }).ToListAsync();
            foreach (var unit in UnitVM.Units)
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
            UnitVM.TermId = Id;
            UnitVM.YearId = YearId;
            return View(UnitVM);
        }
        [HttpPost]
        public async Task<IActionResult> UnitAllocation(UnitAllocationVM data)
        {
            var result = _db.UnitAllocations
            .Where(p => p.TermId.Equals(data.TermId)).ToList();
            _db.RemoveRange(result);
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
                    WorkBookEndPage = chapter.WorkBookEndPage
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

        [HttpGet]
        public async Task<IActionResult> ChapterAllocation(int Id, int YearId)
        {
            ViewBag.WorkBooks = await _db.Books.Where(x => x.IsWorkBook == true).ToListAsync();
            var AllChapters = from a in _db.UnitAllocations
                              from chapter in _db.chapters
                              from unit in _db.Units
                              where chapter.UnitId == a.UnitId && unit.UnitId == a.UnitId && a.TermId == Id
                              select new ChapterList
                              {
                                  ChapterName = chapter.ChapterName,
                                  ChapterId = chapter.ChapterId,
                                  UnitId = (int)chapter.UnitId,
                                  UnitName = unit.UnitName,
                                  WBMaxPage = a.WorkBookEndPage,
                                  WBMinPage = a.WorkBookStartPage
                              };
            var month = await _repository.GetTermById(Id);
            //List < ChapterList > AllChapters = await _db.chapters.Select(x => new ChapterList { ChapterName = x.ChapterName, ChapterId = x.ChapterId }).ToListAsync();
            ChapterAllocationVM ChapterVM = new ChapterAllocationVM();
            ChapterVM.TermId = Id;
            var ParentUnit = await _db.UnitAllocations.Where(x => x.TermId == Id).FirstOrDefaultAsync();
            ChapterVM.MinDate = ParentUnit != null ? ParentUnit.StartDate : DateTime.Now;
            ChapterVM.MaxDate = ParentUnit != null ? ParentUnit.EndDate : DateTime.Now;
            ChapterVM.Chapters = await AllChapters.Select(x => new ChapterList { ChapterId = x.ChapterId, ChapterName = x.ChapterName, UnitId = x.UnitId, UnitName = x.UnitName,WBMinPage = x.WBMinPage, WBMaxPage = x.WBMaxPage }).ToListAsync();
            var AllocatedChapters = await _db.ChapterAllocations.Where(x => x.TermId == Id).ToListAsync();
            foreach (var chapter in ChapterVM.Chapters)
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
            ChapterVM.YearId = YearId;
            return View(ChapterVM);
        }
        [HttpPost]
        public async Task<IActionResult> ChapterAllocation(ChapterAllocationVM data)
        {
            var result = _db.ChapterAllocations
            .Where(p => p.TermId == data.TermId).ToList();
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

        [HttpGet]
        public async Task<IActionResult> TopicAllocation(int Id, int YearId)
        {
            ViewBag.WorkBooks = await _db.Books.Where(x => x.IsWorkBook == true).ToListAsync();
            var chapters = await _db.ChapterAllocations.Where(x => x.TermId == Id).ToListAsync();
            List<TopicList> topics = new List<TopicList>();
            TopicAllocationVM TopicVM = new TopicAllocationVM();
            foreach (var chapter in chapters)
            {
                List<TopicList> topic = await _db.topics
                    .Where(x => x.ChapterId == chapter.ChapterId)
                    .Select(x => new TopicList
                    {
                        TopicId = x.TopicId,
                        TopicName = x.TopicName
                    }).ToListAsync();
                foreach (var each in topic)
                {
                    each.WBMaxPage = chapter.WorkBookEndPage;
                    each.WBMinPage = chapter.WorkBookStartPage;
                }
                topics.AddRange(topic);
            }
            TopicVM.Topics = topics;
            var allocatinos = from t in topics
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
            foreach (var topic in topics)
            {
                var selectedTopic = await _db.TopicAllocations.Where(x => x.TopicId == topic.TopicId).FirstOrDefaultAsync();
                if (selectedTopic != null)
                {
                    var chapter = await _db.ChapterAllocations.Where(x => x.ChapterId == selectedTopic.ChapterId).FirstOrDefaultAsync();
                    topic.ChapterStartDate = chapter?.StartDate;
                    topic.ChapterEndDate = chapter?.EndDate;
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
            TopicVM.TermId = Id;
            var Dates = _db.ChapterAllocations.Where(x => x.TermId == Id).Select(x => new { StartDate = x.StartDate, EndDate = x.EndDate }).ToList();
            //if (await _db.ChapterAllocations.ToListAsync().Result.Any()) ;
            var chapAllocation = await _db.ChapterAllocations.Where(x => x.TermId == Id).FirstOrDefaultAsync();
            TopicVM.MinDate = chapAllocation != null ? chapAllocation.StartDate : DateTime.Now;
            TopicVM.MaxDate = chapAllocation != null ? chapAllocation.EndDate : DateTime.Now;
            ViewBag.TMethods = await _db.TeachingMethodologies.ToListAsync();
            TopicVM.YearId = YearId;
            return View(TopicVM);
        }
        [HttpPost]
        public async Task<IActionResult> TopicAllocation(TopicAllocationVM data)
        {
            var oldTopics = _db.TopicAllocations
            .Where(p => p.TermId.Equals(data.TermId)).ToList();
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

        [HttpGet]
        public async Task<IActionResult> SubTopicAllocation(int Id, int YearId)
        {
            ViewBag.WorkBooks = await _db.Books.Where(x => x.IsWorkBook == true).ToListAsync();
            var subTopics = from a in _db.ChapterAllocations
                            from b in _db.TopicAllocations
                            from c in _db.subTopics
                            where a.TermId == Id && b.ChapterId == a.ChapterId && c.TopicId == b.TopicId
                            select c;
            SubTopicAllocationVM subTopic = new SubTopicAllocationVM();
            subTopic.TermId = Id;
            subTopic.YearId = YearId;
            if (subTopics.Any())
            {
                var allocatedSubTopics = from a in _db.ChapterAllocations
                                         from b in _db.TopicAllocations
                                         from c in _db.SubTopicAllocations
                                         where a.TermId == Id && b.ChapterId == a.ChapterId && c.TopicId == b.TopicId
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
                subTopic.SubTopics = subTopicList;
                var ParentTopic = await _db.TopicAllocations.Where(x => x.TopicId == subTopics.FirstOrDefault().TopicId).FirstOrDefaultAsync();
                subTopic.MinDate = ParentTopic.StartDate;
                subTopic.MaxDate = ParentTopic.EndDate;
                return View(subTopic);
            }
            else
            {
                List<SubTopicList> subTopicList = new List<SubTopicList>();
                subTopic.SubTopics = subTopicList;
                subTopic.MinDate = DateTime.Now;
                subTopic.MaxDate = DateTime.Now;
                return View(subTopic);
            }
        }
        [HttpPost]
        public async Task<IActionResult> SubTopicAllocation(SubTopicAllocationVM data)
        {
            var oldsubTopics = from a in _db.ChapterAllocations
                               from b in _db.TopicAllocations
                               from c in _db.SubTopicAllocations
                               where a.TermId == data.TermId && b.ChapterId == a.ChapterId && c.TopicId == b.TopicId
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
            
            int userId = Convert.ToInt16(this.User.FindFirst(ClaimTypes.Sid)?.Value);
            ViewBag.Years = await _db.years.ToListAsync();
            ViewBag.SchoolSections = await _db.SchoolSections.ToListAsync();
            ViewBag.Classes = from a in _db.SubjectTeacherAllocations
                              from b in _db.Grades
                              from c in _db.Sections
                              where a.SectionId == c.SectionId && c.SectionId == a.SectionId && b.GradeId == c.GradeId && a.EmployeeId == userId
                              select new
                              {
                                  SectionId = c.SectionId,
                                  ClassName = b.GradeName + c.SectionName
                              };
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
            int totalItems = 0;
            calender.years = await _db.years.Select(x => new YearList { YearName = x.YearName, EndDate = x.EndDate, Holidays = x.Holidays, IsLeapYear = x.IsLeapYear, StartDate = x.StartDate, TotalAssesWiseSchoolDays = x.TotalAssesWiseSchoolDays, TotalDays = x.TotalDays, TotalSatSundays = x.TotalSatSundays, TotalSchoolDays = x.TotalSchoolDays, YearId = x.YearId }).ToListAsync();

            //#region NoFilters
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
            //#endregion

            //#region FilterCodeComment

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
            //#endregion

            ////If the grade and subject
            //#region GradeAndSubjectFilter
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
            //#endregion

            ////If All Filters Applied
            //#region GradeSubjectBookFilter
            //else if ((calender.GradeId != null || calender.GradeId > 0) && (calender.SubjectId != null || calender.SubjectId > 0) && (calender.BookId != null || calender.BookId > 0))
            //{
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
                            term.units = await units.Select(x => new UnitList { WorkBookEndPage = x.WorkBookEndPage, WorkBookStartPage = x.WorkBookStartPage,BookName = x.BookName, TermId = x.TermId, EndDate = x.EndDate, StartDate = x.StartDate, UnitId = x.UnitId, UnitName = x.UnitName }).ToListAsync();
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
                                unit.chapters = await chapters.Select(x => new ChapterList { WorkBookEndPage = x.WorkBookEndPage, WorkBookStartPage = x.WorkBookStartPage,ChapterId = (int)x.ChapterId, UnitId = x.UnitId, TermId = x.TermId, StartDate = x.StartDate, EndDate = x.EndDate, UnitName = x.UnitName, ChapterName = x.ChapterName }).ToListAsync();
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
                                    chapter.topics = await topics.Select(x => new TopicList { WorkBookEndPage = x.WorkBookEndPage, WorkBookStartPage = x.WorkBookEndPage,ChapterId = (int)x.ChapterId, StartDate = x.StartDate, EndDate = x.EndDate, TopicId = (int)x.TopicId, ChapterName = x.ChapterName, TopicName = x.TopicName, TeachingMethodology = x.TeachingMethodology, TeachingMethodologyDesc = x.TeachingMethodologyDesc }).ToListAsync();
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
                                        topic.subTopics = await subTopics.Select(x => new SubTopicList {WorkBookStartPage = x.WorkBookStartPage, WorkBookEndPage = x.WorkBookEndPage,TopicId = x.TopicId, EndDate = x.EndDate, StartDate = x.StartDate, TopicName = x.TopicName, SubTopicId = x.SubTopicId, SubTopicName = x.SubTopicName }).ToListAsync();
                                        totalItems += topic.subTopics.Count;
                                    }
                                }
                            }

                        //}
                    }

                }
            //}
            #endregion
            calender.totalItems = totalItems;
            return View(calender);
        }

        #region DinamicData
        public async Task<JsonResult> GetBooks(int Subjectid, int GradeId)
        {
            var books = await _db.Books.Where(x => x.GradeId == GradeId && x.SubjectId == Subjectid).ToListAsync();
            return Json(books);
        }
        public JsonResult GetSubjects(int GradeId)
        {
            var subjects = from a in _db.Books
                        from b in _db.Subjects
                        //let subNames = from sub in booksubjects
                        //               select new
                        //               {
                        //                   id = sub.SubjectId,
                        //                   name = sub.SubjectName
                        //               }
                        where b.SubjectId == a.SubjectId && a.GradeId == GradeId
                        select b;
            //var subjects = books.GroupBy(x => x.SubjectId,);
            return Json(subjects);
        }
        public JsonResult GetGrades(int SchoolSectionId)

        {
            var grades = from b in _db.Grades
                           from c in _db.SchoolSections
                           where c.SchoolSectionId == SchoolSectionId && b.SchoolSectionId == c.SchoolSectionId
                           select b;
            return Json(grades);
        }

        public JsonResult GetClassBooks(int ClassId)

        {
            int userId = Convert.ToInt16(User.FindFirst(ClaimTypes.Sid)?.Value);
            var classes = from a in _db.Books
                         from d in _db.SubjectTeacherAllocations
                         where d.BookId == a.BookId && d.EmployeeId == userId && d.SectionId == ClassId && a.IsWorkBook == false
                         select a;
            return Json(classes);
        }
        #endregion

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
    }
}
