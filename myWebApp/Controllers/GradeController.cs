using Entities.Models;
using Entities.ViewModels;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using myWebApp.ViewModels.AcademicCalendar;
using myWebApp.ViewModels.BookAllocation;
using myWebApp.ViewModels.Grade;
using System.IO;
using myWebApp.ViewModels.Setups;
using static System.Collections.Specialized.BitVector32;
using NuGet.Protocol.Plugins;
using Microsoft.AspNetCore.Mvc.Razor.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace myWebApp.Controllers
{
    [Authorize]
    public class GradeController : Controller
    {
        private readonly IEFRepository _repository;
        private readonly SchoolDbContext _db;
        private readonly IWebHostEnvironment _Environment;
        public GradeController(IEFRepository repository, SchoolDbContext db, IWebHostEnvironment Environment)
        {
            _repository = repository;
            _db = db;
            _Environment = Environment;
        }


        #region Subject
        [Authorize(Policy = "Subject.Read")]
        public async Task<IActionResult> Subject()
        {
            var subjects = await _repository.GetSubjects();
            return View(subjects);
        }
        [Authorize(Policy = "Subject.Create")]
        [HttpGet]
        public IActionResult AddSubject()
        {
            return View();
        }
        [Authorize(Policy = "Subject.Create")]
        [HttpPost]
        public async Task<IActionResult> AddSubject(SubjectVM subject)
        {
            var newSubject = new Subject
            {
                SubjectName = subject.SubjectName
            };
            await _repository.AddAsync(newSubject);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Subject");
            }
            else
            {
                ModelState.AddModelError("", "Error While Saving to Database");
            }
            return View(subject);
        }
        [Authorize(Policy = "Subject.Update")]
        [HttpGet]
        public async Task<IActionResult> UpdateSubject(int id)
        {
            var temp = await _repository.GetSubjectById(id);
            int active = 0;
            if (temp == null)
            {
                return NotFound();
            }
            if (temp.IsActive == true)
            {
                active = 1;
            }
            var Department = new UpdateSubjectVM
            {
                SubjectId = temp.SubjectId,
                SubjectName = temp.SubjectName,
                IsActive = active
            };
            return View(Department);
        }
        [Authorize(Policy = "Subject.Update")]
        [HttpPost]
        public async Task<IActionResult> UpdateSubject(UpdateSubjectVM subject)
        {
            bool active = false;
            var temp = await _repository.GetSubjectById(subject.SubjectId);
            if (temp == null)
            {
                return NotFound();
            }
            if (subject.IsActive == 1)
            {
                active = true;
            }
            else
            {
                var books = await _repository.GetBooks();
                foreach (var book in books)
                {
                    book.IsActive = false;
                }
                active = false;
            }
            temp.SubjectName = subject.SubjectName;
            temp.IsActive = active;
            await _repository.UpdateAsync(temp);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Subject");
            }
            ModelState.AddModelError("", "Error While Saving to Database");
            return View(subject);
        }
        [Authorize(Policy = "Subject.Delete")]
        [HttpGet]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            var temp = await _repository.GetSubjectById(id);
            if (temp == null)
            {
                return NotFound();
            }
            await _repository.Delete(temp);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Subject");
            }
            return RedirectToAction("Subject");
        }
        #endregion

        #region Book
        [Authorize(Policy = "Book.Read")]
        public async Task<IActionResult> Book(int sectionId)
        {
            
            //var s = User.Identity.Name;
            //var name = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //var UserId = this.User.FindFirst(ClaimTypes.Sid)?.Value;
            var DepId = this.User.FindFirst("DepartmentId")?.Value;
              var books = from b in _db.Books
                        join c in _db.Subjects on b.SubjectId equals c.SubjectId into BookSubject
                        from subject in BookSubject.DefaultIfEmpty()
                        //join d in _db.Sections on b.GradeId equals d.GradeId into BookSection
                        //from section in BookSection.DefaultIfEmpty()
                        join e in _db.Grades on b.GradeId equals e.GradeId into BookGrade
                        from grade in BookGrade.DefaultIfEmpty()
                        join f in _db.ResourceNoteBooks on b.ResourceNoteBookId equals f.ResourceNoteBookId into BookNotebook
                        from NoteBook in BookNotebook.DefaultIfEmpty()
                        select new
                        {
                            BookId = b.BookId,
                            BookName = b.BookName,
                            SubjectName = subject.SubjectName,
                            Grade = grade.GradeName,
                            Publisher = b.Publisher,
                            PublishDate = b.PublishDate,
                            IsActive = b.IsActive,
                            ResourceBook = b.ResourceBook,
                            ResourceNoteBook = NoteBook.NoteBookName,
                            ResouceBookURL = b.ResourceBookPath,
                            IsWorkBook = b.IsWorkBook
                        };
            BookVM bookVM = new BookVM();
            bookVM.books = await books.Select(x => new BookViewList {IsWorkBook = x.IsWorkBook,GardeName = x.Grade,BookName = x.BookName, ResourceBookURL = x.ResouceBookURL, BookId = x.BookId, IsActive = x.IsActive, PublishDate = x.PublishDate, Publisher = x.Publisher, SubjectName = x.SubjectName, ResourceBook = x.ResourceBook,ResourceNoteBook = x.ResourceNoteBook }).ToListAsync();
            //ViewBag.NoteBook = await _db.ResourceNoteBooks.ToListAsync();
            return View(bookVM);
        }
        [Authorize(Policy = "Book.Create")]
        [HttpGet]
        public async Task<IActionResult> AddBook()
        {
            
            ViewBag.Subjects = await _repository.GetSubjects();
            ViewBag.Grades = await _db.Grades.ToListAsync();
            ViewBag.NoteBooks = await _db.ResourceNoteBooks.ToListAsync();
            return View();
        }
        [Authorize(Policy = "Book.Create")]
        [HttpPost]
        public async Task<IActionResult> AddBook(BookVM book)
        {
            var newBook = new Book();
            newBook.BookName = book.BookName;
            newBook.Author = book.Author;
            newBook.Publisher = book.Publisher;
            newBook.PublishDate = book.PublishDate;
            newBook.SubjectId = book.SubjectId;
            newBook.GradeId = book.GradeId;
            newBook.ResourceBook = book.ResourceBook;
            newBook.ResourceNoteBookId = book.ResourceNoteBookId;
            newBook.IsWorkBook = book.IsWorkBook;
            if (book.ResourceBookURL != null)
            {
                newBook.ResourceBookPath = FileSaver(book);
            }
            await _repository.AddAsync(newBook);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Book");
            }
            else
            {
                ModelState.AddModelError("", "Error While Saving to Database");
            }
            return View(book);
        }
        [Authorize(Policy = "Book.Update")]
        [HttpGet]
        public async Task<IActionResult> UpdateBook(int id)
        {
            ViewBag.Subjects = await _repository.GetSubjects();
            ViewBag.Grades = await _repository.GetGrades();
            ViewBag.NoteBooks = await _db.ResourceNoteBooks.ToListAsync();
            var temp = await _repository.GetBookById(id);
            if (temp == null)
            {
                return NotFound();
            }
            var Department = new UpdateBookVM
            {
                BookId = temp.BookId,
                BookName = temp.BookName,
                Author = temp.Author,
                Publisher = temp.Publisher,
                PublishDate = temp.PublishDate,
                SubjectId = temp.SubjectId,
                IsActive = (bool)temp.IsActive,
                GradeId = (int)temp.GradeId,
                ResourceBook = temp.ResourceBook,
                ResourceNoteBookId = temp.ResourceNoteBookId,
                ResourceBookURL = temp.ResourceBookPath,
                IsWorkBook = (bool)temp.IsWorkBook
            };
            return View(Department);
        }
        [Authorize(Policy = "Book.Update")]
        [HttpPost]
        public async Task<IActionResult> Updatebook(UpdateBookVM book)
        {
            var temp = await _repository.GetBookById(book.BookId);
            if (temp == null)
            {
                return NotFound();
            }
            //if (book.IsActive == 1)
            //{
            //    active = true;
            //}
            temp.BookName = book.BookName;
            temp.Author = book.Author;
            temp.Publisher = book.Publisher;
            temp.PublishDate = book.PublishDate;
            temp.SubjectId = book.SubjectId;
            temp.IsActive = book.IsActive;
            temp.GradeId = book.GradeId;
            temp.ResourceNoteBookId = book.ResourceNoteBookId;
            temp.ResourceBook = book.ResourceBook;
            temp.IsWorkBook = book.IsWorkBook;
            if (book.NewResourceBookURL != null)
            {
                var oldBook = new FileInfo(book.ResourceBookURL);
                oldBook.Delete();
                FileInfo fileInfo = new FileInfo(book.NewResourceBookURL.FileName);
                string path = Path.Combine(this._Environment.WebRootPath, "ResourceBooks/");
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                string fileName = book.ResourceBook + fileInfo.Extension;
                string fileNameWithPath = Path.Combine(path, fileName);
                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    book.NewResourceBookURL.CopyTo(stream);
                }
                temp.ResourceBookPath = fileNameWithPath;
            }
            await _repository.UpdateAsync(temp);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Book");
            }
            ModelState.AddModelError("", "Error While Saving to Database");
            return View(book);
        }
        [Authorize(Policy = "Book.Delete")]
        [HttpGet]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var temp = await _repository.GetBookById(id);
            if (temp == null)
            {
                return NotFound();
            }
            await _repository.Delete(temp);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Book");
            }
            return RedirectToAction("Book");
        }

        public IActionResult ViewResourceBook(string path)
        {
            Stream stream = new FileStream(path, FileMode.Open);
            if (stream != null)
            {
                return new FileStreamResult(stream, "application/pdf");
            }
            else
            {
                return NotFound();
            }
        }

        public string FileSaver(BookVM book)
        {
            FileInfo fileInfo = new FileInfo(book.ResourceBookURL.FileName);
            string path = Path.Combine(this._Environment.WebRootPath, "ResourceBooks/");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            string fileName = book.ResourceBook + fileInfo.Extension;
            string fileNameWithPath = Path.Combine(path, fileName);
            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                book.ResourceBookURL.CopyTo(stream);
            }
            return fileNameWithPath;
        }
        #endregion

        #region Unit
        [Authorize(Policy = "Unit.Read")]
        [HttpGet]
        public async Task<IActionResult> Unit(int Id)
        {
            UnitVM Unit = new UnitVM();
            var book = await _db.Books.Where(x => x.BookId == Id).FirstOrDefaultAsync();
            ViewBag.Book = book.BookName;
            Unit.BookId = Id;
            var Units = _db.Units.Where(x => x.BookId == Id).ToList();
            ViewBag.Units = Units;
            return View(Unit);
        }
        [Authorize(Policy = "Unit.Create")]
        [HttpPost]
        public async Task<IActionResult> Unit(UnitVM unit)
        {
            var newUnit = new Unit
            {
                BookId = unit.BookId,
                UnitName = unit.UnitName,
                StartPage = unit.StartPage,
                EndPage = unit.EndPage,
                SLO = unit.SLO
            };
            await _repository.AddAsync(newUnit);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Unit", new {Id = unit.BookId});
            }
            else
            {
                ModelState.AddModelError("", "Error While Saving to Database");
            }
            return View(unit);
        }
        [Authorize(Policy = "Unit.Update")]
        [HttpGet]
        public IActionResult UpdateUnit(int id)
        {
            var temp = _db.Units.Where(x => x.UnitId == id).FirstOrDefault();

            if (temp == null)
            {
                return NotFound();
            }
            var Book = _db.Books.Where(x => x.BookId == temp.BookId).FirstOrDefault();
            ViewBag.Book = Book.BookName;
            var unit = new UnitVM
            {
                UnitId = temp.UnitId,
                UnitName = temp.UnitName,
                StartPage = (int)temp.StartPage,
                EndPage = (int)temp.EndPage,
                IsActive = temp.IsActive,
                BookId = (int)temp.BookId,
                SLO = temp.SLO
            };

            return View(unit);
        }
        [Authorize(Policy = "Unit.Update")]
        [HttpPost]
        public async Task<IActionResult> UpdateUnit(UnitVM unit)
        {
            var temp = _db.Units.Where(x => x.UnitId == unit.UnitId).FirstOrDefault();
            if (temp == null)
            {
                return NotFound();
            }
            temp.UnitName = unit.UnitName;
            temp.StartPage = unit.StartPage;
            temp.EndPage = unit.EndPage;
            temp.IsActive = (bool)unit.IsActive;
            temp.SLO = unit.SLO;
            //if ((bool)temp.IsActive)
            //{
            //    v InActive all the related Topics here too
            //}
            await _repository.UpdateAsync(temp);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Unit", new { Id = unit.BookId });
            }
            ModelState.AddModelError("", "Error While Saving to Database");
            return View(unit);
        }
        [Authorize(Policy = "Unit.Delete")]
        [HttpGet]
        public async Task<IActionResult> DeleteUnit(int id,int BookId)
        {
            var temp = _db.Units.Where(x => x.UnitId == id).FirstOrDefault();
            if (temp == null)
            {
                return NotFound();
            }
            await _repository.Delete(temp);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Unit",new {Id = BookId});
            }
            return RedirectToAction("Unit", new { Id = BookId });
        }
        #endregion

        #region Chapter
        [Authorize(Policy = "Chapter.Read")]
        [HttpGet]
        public IActionResult Chapter(int Id)
        {
            ChapterVM chapter = new ChapterVM();
            var unit = _db.Units.Where(x => x.UnitId == Id).FirstOrDefault();
            chapter.MinPage = unit.StartPage;
            chapter.MaxPage = unit.EndPage;
            ViewBag.Unit = unit.UnitName;
            ViewBag.UStartPage = unit.StartPage;
            ViewBag.UEndPage = unit.EndPage;
            chapter.BookId = Id;
            var chapters = _db.chapters.Where(x => x.UnitId == Id).ToList();
            ViewBag.Chapters = chapters;
            return View(chapter);
        }
        [Authorize(Policy = "Chapter.Create")]
        [HttpPost]
        public async Task<IActionResult> Chapter(ChapterVM chapter)
        {
            var newChapter = new Chapter
            {
                UnitId = chapter.BookId,
                ChapterName = chapter.ChapterName,
                StartPage = chapter.StartPage,
                EndPage = chapter.EndPage,
                SLO = chapter.SLO
            };
            await _repository.AddAsync(newChapter);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Chapter", new {Id = chapter.BookId});
            }
            else
            {
                ModelState.AddModelError("", "Error While Saving to Database");
            }
            return View(chapter);
        }
        [Authorize(Policy = "Chapter.Update")]
        [HttpGet]
        public async Task<IActionResult> UpdateChapter(int id)
        {
            var temp = _db.chapters.Where(x => x.ChapterId == id).FirstOrDefault();
            if (temp == null)
            {
                return NotFound();
            }
            var Unit = _db.Units.Where(x => x.UnitId == temp.UnitId).FirstOrDefault();
            ViewBag.UStartPage = Unit.StartPage;
            ViewBag.UEndPage = Unit.EndPage;
            var chapter = new UpdateChapterVM
            {
                ChapterId = temp.ChapterId,
                ChapterName = temp.ChapterName,
                StartPage = (int)temp.StartPage,
                EndPage = (int)temp.EndPage,
                UnitId = (int)temp.UnitId,
                MinPage = Unit.StartPage,
                MaxPage = Unit.EndPage,
                SLO = temp.SLO,
                IsActive = temp.IsActive
            };
            ViewBag.Units = await _db.Units.ToListAsync();
            return View(chapter);
        }
        [Authorize(Policy = "Chapter.Update")]
        [HttpPost]
        public async Task<IActionResult> UpdateChapter(UpdateChapterVM chapter)
        {
            var temp = _db.chapters.Where(x => x.ChapterId == chapter.ChapterId).FirstOrDefault();
            if (temp == null)
            {
                return NotFound();
            }
            temp.ChapterName = chapter.ChapterName;
            temp.StartPage = chapter.StartPage;
            temp.EndPage = chapter.EndPage;
            temp.IsActive = (bool)chapter.IsActive;
            temp.UnitId = chapter.UnitId;
            temp.SLO = chapter.SLO;
            await _repository.UpdateAsync(temp);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Chapter", new {Id = temp.UnitId});
            }
            ModelState.AddModelError("", "Error While Saving to Database");
            return View(chapter);
        }
        [Authorize(Policy = "Chapter.Delete")]
        [HttpGet]
        public async Task<IActionResult> DeleteChapter(int id, int UnitId)
        {
            var temp = _db.chapters.Where(x => x.ChapterId == id).FirstOrDefault();
            if (temp == null)
            {
                return NotFound();
            }
            await _repository.Delete(temp);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Chapter", new {Id = UnitId});
            }
            return RedirectToAction("Chapter", new {Id = UnitId});
        }
        #endregion

        #region Questions
        [HttpGet]
        public async Task<IActionResult> Questions(int ChapterId, int UnitId)
        {
            ChapterQuestionsVM questions= new ChapterQuestionsVM();
            var Chapter = await _db.chapters.Where(p => p.ChapterId == ChapterId).FirstOrDefaultAsync();
            ViewBag.Chapter = Chapter.ChapterName;
            questions.UnitId = UnitId;
            questions.ChapterId = ChapterId;
            return View(questions);
        }
        [HttpPost]
        public async Task<IActionResult> Questions(ChapterQuestionsVM question)
        {
            if (ModelState.IsValid)
            {
                ChapterQuestions NewQuestion;
                if (question.QuestionType == "TrueFalse")
                {
                    NewQuestion = new ChapterQuestions
                    {
                        ChapterId = question.ChapterId,
                        Topic = question.Topic,
                        QuestionType = question.QuestionType,
                    };
                    await _repository.AddAsync(NewQuestion);
                    if (await _repository.SaveChanges())
                    {
                        var quest = await _db.ChapterQuestions.OrderBy(p => p.QuestionId).LastOrDefaultAsync();
                        var answer = new ChapterAnswers
                        {
                            IsTrue = question.TrueOrFalse,
                            QuestionId = quest.QuestionId
                        };
                        await _repository.AddAsync(answer);
                        if (await _repository.SaveChanges())
                        {
                            return RedirectToAction("Chapter", new { Id = question.UnitId });
                        }
                    }
                }
                else
                {
                    NewQuestion = new ChapterQuestions
                    {
                        ChapterId = question.ChapterId,
                        Topic = question.Topic,
                        QuestionType = question.QuestionType,
                    };
                    await _repository.AddAsync(NewQuestion);
                    if (await _repository.SaveChanges())
                    {
                        var quest = await _db.ChapterQuestions.OrderBy(p => p.QuestionId).LastOrDefaultAsync();
                        foreach (var ans in question.Answers)
                        {
                            var answer = new ChapterAnswers
                            {
                                IsTrue = ans.IsTrue,
                                QuestionId = quest.QuestionId,
                                Choice = ans.Choice
                            };
                            await _repository.AddAsync(answer);
                        }
                        if (await _repository.SaveChanges())
                        {
                            return RedirectToAction("Chapter", new { Id = question.UnitId });
                        }
                    }
                }
                await _repository.AddAsync(NewQuestion);
                if (await _repository.SaveChanges())
                {
                    return RedirectToAction("Chapter", new { Id = question.UnitId });
                }
                else
                {
                    ModelState.AddModelError("", "Error While Saving to Database");
                }
                return View(question);
            }
            return View(question);
        }
        //[HttpGet]
        //public async Task<IActionResult> UpdateChapter(int id)
        //{
        //    var temp = _db.chapters.Where(x => x.ChapterId == id).FirstOrDefault();
        //    if (temp == null)
        //    {
        //        return NotFound();
        //    }
        //    var Unit = _db.Units.Where(x => x.UnitId == temp.UnitId).FirstOrDefault();
        //    ViewBag.UStartPage = Unit.StartPage;
        //    ViewBag.UEndPage = Unit.EndPage;
        //    var chapter = new UpdateChapterVM
        //    {
        //        ChapterId = temp.ChapterId,
        //        ChapterName = temp.ChapterName,
        //        StartPage = (int)temp.StartPage,
        //        EndPage = (int)temp.EndPage,
        //        UnitId = (int)temp.UnitId,
        //        MinPage = Unit.StartPage,
        //        MaxPage = Unit.EndPage,
        //        SLO = temp.SLO,
        //        IsActive = temp.IsActive
        //    };
        //    ViewBag.Units = await _db.Units.ToListAsync();
        //    return View(chapter);
        //}
        //[HttpPost]
        //public async Task<IActionResult> UpdateChapter(UpdateChapterVM chapter)
        //{
        //    var temp = _db.chapters.Where(x => x.ChapterId == chapter.ChapterId).FirstOrDefault();
        //    if (temp == null)
        //    {
        //        return NotFound();
        //    }
        //    temp.ChapterName = chapter.ChapterName;
        //    temp.StartPage = chapter.StartPage;
        //    temp.EndPage = chapter.EndPage;
        //    temp.IsActive = (bool)chapter.IsActive;
        //    temp.UnitId = chapter.UnitId;
        //    temp.SLO = chapter.SLO;
        //    await _repository.UpdateAsync(temp);
        //    if (await _repository.SaveChanges())
        //    {
        //        return RedirectToAction("Chapter", new { Id = temp.UnitId });
        //    }
        //    ModelState.AddModelError("", "Error While Saving to Database");
        //    return View(chapter);
        //}
        //[HttpGet]
        //public async Task<IActionResult> DeleteChapter(int id, int UnitId)
        //{
        //    var temp = _db.chapters.Where(x => x.ChapterId == id).FirstOrDefault();
        //    if (temp == null)
        //    {
        //        return NotFound();
        //    }
        //    await _repository.Delete(temp);
        //    if (await _repository.SaveChanges())
        //    {
        //        return RedirectToAction("Chapter", new { Id = UnitId });
        //    }
        //    return RedirectToAction("Chapter", new { Id = UnitId });
        //}
        #endregion

        #region Topic
        [Authorize(Policy = "Topic.Read")]
        [HttpGet]
        public async Task<IActionResult> Topic(int Id)
        {
            TopicVM Topic = new TopicVM();
            var Chapter = _db.chapters.Where(x => x.ChapterId == Id).FirstOrDefault();
            ViewBag.CStartPage = Chapter.StartPage;
            ViewBag.CEndPage = Chapter.EndPage;
            ViewBag.TMethods = _db.TeachingMethodologies.ToList();
            ViewBag.Chapter = Chapter.ChapterName;
            Topic.ChapterId = Id;
            Topic.MinPage = Chapter.StartPage;
            Topic.MaxPage = Chapter.EndPage;
            var Topics = await _db.topics.Where(x => x.ChapterId == Id).ToListAsync();
            ViewBag.Topics = Topics;
            return View(Topic);
        }
        [Authorize(Policy = "Topic.Create")]
        [HttpPost]
        public async Task<IActionResult> Topic(TopicVM Topic)
        {
            var newTopic = new Topic
            {
                ChapterId = Topic.ChapterId,
                TopicName = Topic.TopicName,
                EndPage = Topic.EndPage,
                StartPage = Topic.StartPage
            };
            await _repository.AddAsync(newTopic);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Topic", new {Id = Topic.ChapterId});
            }
            else
            {
                ModelState.AddModelError("", "Error While Saving to Database");
            }
            return View(Topic);
        }
        [Authorize(Policy = "Topic.Update")]
        [HttpGet]
        public IActionResult UpdateTopic(int id)
        {
            var temp = _db.topics.Where(x => x.TopicId == id).FirstOrDefault();
            if (temp == null)
            {
                return NotFound();
            }
            var Chapter = _db.chapters.Where(x => x.ChapterId == temp.ChapterId).FirstOrDefault();
            ViewBag.CStartPage = Chapter.StartPage;
            ViewBag.CEndPage = Chapter.EndPage;
            ViewBag.Chapter = Chapter.ChapterName;
            var chapter = new UpdateTopicVM
            {
                TopicId = temp.TopicId,
                ChapterId = (int)temp.ChapterId,
                TopicName = temp.TopicName,
                EndPage = (int)temp.EndPage,
                StartPage = (int)temp.StartPage,
                IsActive = temp.IsActive,
                MinPage = Chapter.StartPage,
                MaxPage = Chapter.EndPage
            };
            return View(chapter);
        }
        [Authorize(Policy = "Topic.Update")]
        [HttpPost]
        public async Task<IActionResult> UpdateTopic(UpdateTopicVM Topic)
        {
            var temp = _db.topics.Where(x => x.TopicId == Topic.TopicId).FirstOrDefault();
            if (temp == null)
            {
                return NotFound();
            }
            //temp.ChapterId = Topic.chap
            temp.TopicName = Topic.TopicName;
            temp.StartPage = Topic.StartPage;
            temp.EndPage = Topic.EndPage;
            temp.IsActive = (bool)Topic.IsActive;
            //temp.TeachingMethodologyId = Topic.TeachingMethodologyId;
            //temp.TeachingMethodDesc = Topic.TeachingMethodologyDesc;
            await _repository.UpdateAsync(temp);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Topic", new { Id = Topic.ChapterId });
            }
            ModelState.AddModelError("", "Error While Saving to Database");
            return View(Topic);
        }
        [Authorize(Policy = "Topic.Delete")]
        [HttpGet]
        public async Task<IActionResult> DeleteTopic(int id)
        {
            var temp = _db.topics.Where(x => x.TopicId == id).FirstOrDefault();
            if (temp == null)
            {
                return NotFound();
            }
            await _repository.Delete(temp);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Topic", new {Id = temp.ChapterId});
            }
            return RedirectToAction("Topic", new {Id = temp.ChapterId});
        }
        #endregion

        #region Sub-Topic
        [Authorize(Policy = "SubTopic.Read")]
        [HttpGet]
        public IActionResult SubTopic(int Id)
        {
            SubTopicVM SubTopic = new SubTopicVM();
            var Topic = _db.topics.Where(x => x.TopicId == Id).FirstOrDefault();
            ViewBag.TStartPage = Topic.StartPage;
            ViewBag.TEndPage = Topic.EndPage;
            ViewBag.Topic = Topic.TopicName;
            SubTopic.TopicId = Id;
            SubTopic.MinPage = Topic.StartPage;
            SubTopic.MaxPage = Topic.EndPage;
            var Topics = _db.subTopics.Where(x => x.TopicId == Id).ToList();
            ViewBag.Topics = Topics;
            return View(SubTopic);
        }
        [Authorize(Policy = "SubTopic.Create")]
        [HttpPost]
        public async Task<IActionResult> SubTopic(SubTopicVM Topic)
        {
            var newTopic = new SubTopic
            {
                TopicId = Topic.TopicId,
                SubTopicName = Topic.SubTopicName,
                EndPage = Topic.EndPage,
                StartPage = Topic.StartPage
            };
            await _repository.AddAsync(newTopic);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("SubTopic", new { Id = Topic.TopicId });
            }
            else
            {
                ModelState.AddModelError("", "Error While Saving to Database");
            }
            return View(Topic);
        }
        [Authorize(Policy = "SubTopic.Update")]
        [HttpGet]
        public IActionResult UpdateSubTopic(int id)
        {
            var temp = _db.subTopics.Where(x => x.SubTopicId == id).FirstOrDefault();
            if (temp == null)
            {
                return NotFound();
            }
            var Topic = _db.topics.Where(x => x.TopicId == temp.TopicId).FirstOrDefault();
            ViewBag.TStartPage = Topic.StartPage;
            ViewBag.TEndPage = Topic.EndPage;
            ViewBag.Topic = Topic.TopicName;
            var chapter = new UpdateSubTopicVM
            {
                TopicId = (int)temp.TopicId,
                SubTopicName = temp.SubTopicName,
                SubTopicId = temp.SubTopicId,
                EndPage = (int)temp.EndPage,
                StartPage = (int)temp.StartPage,
                IsActive = temp.IsActive,
                MinPage = Topic.StartPage,
                MaxPage = Topic.EndPage
            };
            return View(chapter);
        }
        [Authorize(Policy = "SubTopic.Update")]
        [HttpPost]
        public async Task<IActionResult> UpdateSubTopic(UpdateSubTopicVM Topic)
        {
            var temp = _db.subTopics.Where(x => x.SubTopicId == Topic.SubTopicId).FirstOrDefault();
            if (temp == null)
            {
                return NotFound();
            }
            //temp.ChapterId = Topic.chap
            temp.SubTopicName = Topic.SubTopicName;
            temp.TopicId = Topic.TopicId;
            temp.StartPage = Topic.StartPage;
            temp.EndPage = Topic.EndPage;
            temp.IsActive = (bool)Topic.IsActive;
            await _repository.UpdateAsync(temp);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("SubTopic", new { Id = Topic.TopicId });
            }
            ModelState.AddModelError("", "Error While Saving to Database");
            return View(Topic);
        }
        [Authorize(Policy = "SubTopic.Delete")]
        [HttpGet]
        public async Task<IActionResult> DeleteSubTopic(int id, int TopicId)
        {
            var temp = _db.subTopics.Where(x => x.SubTopicId == id).FirstOrDefault();
            if (temp == null)
            {
                return NotFound();
            }
            await _repository.Delete(temp);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("SubTopic", new { Id = TopicId });
            }
            return RedirectToAction("Topic", new { Id = TopicId });
        }
        #endregion

        #region Grade
        [Authorize(Policy = "Grade.Read")]
        [HttpGet]
        public IActionResult Grade()
        {
            var userId = Convert.ToInt16(User.FindFirst(ClaimTypes.Sid)?.Value);
            //var user = _db.Employees.Where(x => x.EmployeeId == userId).FirstOrDefault();
            if(User.IsInRole("Grade Manager"))
            {
                ViewBag.Grades = from a in _db.Grades
                                 join b in _db.SchoolSections on a.SchoolSectionId equals b.SchoolSectionId into GradeSchoolSection
                                 from schoolSection in GradeSchoolSection.DefaultIfEmpty()
                                 join c in _db.Employees on a.GradeManagerId equals c.EmployeeId into GradeManager
                                 from GM in GradeManager.DefaultIfEmpty()
                                 where schoolSection.SchoolSectionId == a.SchoolSectionId
                                 select new
                                 {
                                     GradeId = a.GradeId,
                                     GradeName = a.GradeName,
                                     SchoolSection = schoolSection.SectionName,
                                     IsActive = a.IsActive,
                                     GradeManager = GM.FName + " " + GM.LName,
                                     ShowDeleteUpdate = a.GradeManagerId == userId ? true : false
                                 };
            }
            else if(!User.IsInRole("Subject Teacher"))
            {
                ViewBag.Grades = from a in _db.Grades
                                 join b in _db.SchoolSections on a.SchoolSectionId equals b.SchoolSectionId into GradeSchoolSection
                                 from schoolSection in GradeSchoolSection.DefaultIfEmpty()
                                 join c in _db.Employees on a.GradeManagerId equals c.EmployeeId into GradeManager
                                 from GM in GradeManager.DefaultIfEmpty()
                                 where schoolSection.SchoolSectionId == a.SchoolSectionId
                                 select new
                                 {
                                     GradeId = a.GradeId,
                                     GradeName = a.GradeName,
                                     SchoolSection = schoolSection.SectionName,
                                     IsActive = a.IsActive,
                                     GradeManager = GM.FName + " " + GM.LName,
                                     ShowDeleteUpdate = true
                                 };
            }
            return View();
        }
        [Authorize(Policy = "Grade.Create")]
        [HttpGet]
        public async Task<IActionResult> AddGrade()
        {
            ViewBag.SchoolSections = await _db.SchoolSections.ToListAsync();
            ViewBag.GradeManagers = from a in _db.Employees
                                    join b in _db.Roles on a.RoleId equals b.RoleId into empRole
                                    from role in empRole.DefaultIfEmpty()
                                    where role.RollName == "Grade Manager"
                                    select new
                                    {
                                        FName = a.FName,
                                        EmployeeId = a.EmployeeId,
                                        LName = a.LName
                                    };
            return View();
        }
        [Authorize(Policy = "Grade.Create")]
        [HttpPost]
        public async Task<IActionResult> AddGrade(GradeVM grade)
        {
            var newGrade = new Grade
            {
                GradeName = grade.GradeName,
                SchoolSectionId = grade.SchoolSectionId,
                GradeManagerId = grade.GradeManagerId
            };
            await _repository.AddAsync(newGrade);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Grade");
            }
            else
            {
                ModelState.AddModelError("", "Error While Saving to Database");
            }
            return View(grade);
        }
        [Authorize(Policy = "Grade.Update")]
        [HttpGet]
        public async Task<IActionResult> UpdateGrade(int id)
        {
            ViewBag.SchoolSections = await _db.SchoolSections.ToListAsync();
            ViewBag.GradeManagers = from a in _db.Employees
                                    join b in _db.Roles on a.RoleId equals b.RoleId into empRole
                                    from role in empRole.DefaultIfEmpty()
                                    where role.RollName == "Grade Manager"
                                    select new
                                    {
                                        FName = a.FName,
                                        EmployeeId = a.EmployeeId,
                                        LName = a.LName
                                    };
            var temp = await _repository.GetGradeById(id);
            if (temp == null)
            {
                return NotFound();
            }
            var grade = new UpdateGradeVM
            {
                GradeId = temp.GradeId,
                GradeName = temp.GradeName,
                IsActive = (bool)temp.IsActive,
                SchoolSectionId = (int)temp.SchoolSectionId,
                GradeManagerId = (int)temp.GradeManagerId
            };
            return View(grade);
        }
        [Authorize(Policy = "Grade.Update")]
        [HttpPost]
        public async Task<IActionResult> UpdateGrade(UpdateGradeVM grade)
        {
            var temp = await _repository.GetGradeById(grade.GradeId);
            if (temp == null)
            {
                return NotFound();
            }
            //else
            //{
            //    var sections = await _repository.GetSections();
            //    foreach (var eachSection in sections)
            //    {
            //        eachSection.IsActive = false;
            //    }
            //    temp.IsActive = false;
            //}
            temp.GradeName = grade.GradeName;
            temp.IsActive = grade.IsActive;
            temp.SchoolSectionId = grade.SchoolSectionId;
            temp.GradeManagerId = grade.GradeManagerId;
            await _repository.UpdateAsync(temp);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Grade");
            }
            ModelState.AddModelError("", "Error While Saving to Database");
            return View(grade);
        }
        [Authorize(Policy = "Grade.Delete")]
        [HttpGet]
        public async Task<IActionResult> DeleteGrade(int id)
        {
            var temp = await _repository.GetGradeById(id);
            if (temp == null)
            {
                return NotFound();
            }
            await _repository.Delete(temp);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Grade");
            }
            return RedirectToAction("Grade");
        }
        #endregion

        #region Section
        [Authorize(Policy = "Section.Read")]
        [HttpGet]
        public async Task<IActionResult> Section()
        {
            int userId = Convert.ToInt16(User.FindFirst(ClaimTypes.Sid)?.Value);
            if(User.IsInRole("Grade Manager"))
            {
                ViewBag.Sections = (await (from a in _db.Grades
                                           from b in _db.Sections
                                           join c in _db.Employees on b.ClassTeacherId equals c.EmployeeId into ClassTeacher
                                           from CT in ClassTeacher.DefaultIfEmpty()
                                           where b.GradeId == a.GradeId && b.IsActive == true
                                           select new
                                           {
                                               SectionName = b.SectionName,
                                               SectionId = b.SectionId,
                                               IsActive = b.IsActive,
                                               GradeName = a.GradeName,
                                               ShowDeleteUpdate = a.GradeManagerId == userId ? true : false,
                                               ClassTeacher = CT.FName + " " + CT.LName

                                           }).ToListAsync());
            }
            else if(!User.IsInRole("Subject Teacher"))
            {
                ViewBag.Sections = (await (from a in _db.Grades
                                           from b in _db.Sections
                                           join c in _db.Employees on b.ClassTeacherId equals c.EmployeeId into ClassTeacher
                                           from CT in ClassTeacher.DefaultIfEmpty()
                                           where b.GradeId == a.GradeId
                                           select new
                                           {
                                               SectionName = b.SectionName,
                                               SectionId = b.SectionId,
                                               IsActive = b.IsActive,
                                               GradeName = a.GradeName,
                                               ShowDeleteUpdate = true,
                                               ClassTeacher = CT.FName + " " + CT.LName

                                           }).ToListAsync());
            }
            return View();
        }
        [Authorize(Policy = "Section.Create")]
        [HttpGet]
        public async Task<IActionResult> AddSection()
        {
            ViewBag.Grades = await _repository.GetGrades();
            return View();
        }
        [Authorize(Policy = "Section.Create")]
        [HttpPost]
        public async Task<IActionResult> AddSection(SectionVM section)
        {
            var newSection = new Entities.Models.Section
            {
                SectionName = section.SectionName,
                GradeId = section.GradeId,
                ClassTeacherId = section.ClassTeacherId
            };
            await _repository.AddAsync(newSection);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Section");
            }
            else
            {
                ModelState.AddModelError("", "Error While Saving to Database");
            }
            return View(section);
        }
        [Authorize(Policy = "Section.Update")]
        [HttpGet]
        public async Task<IActionResult> UpdateSection(int id)
        {
            ViewBag.Grades = await _repository.GetGrades();
            var temp = await _repository.GetSectionById(id);
            if (temp == null)
            {
                return NotFound();
            }
            var section = new UpdateSectionVM
            {
                SectionId = temp.SectionId,
                SectionName = temp.SectionName,
                GradeId = (int)temp.GradeId,
                IsActive = (bool)temp.IsActive,
                ClassTeacherId = (int)temp.ClassTeacherId
            };
            return View(section);
        }
        [Authorize(Policy = "Section.Update")]
        [HttpPost]
        public async Task<IActionResult> UpdateSection(UpdateSectionVM section)
        {
            var temp = await _repository.GetSectionById(section.SectionId);
            if (temp == null)
            {
                return NotFound();
            }
            //else
            //{
            //    var sections = await _repository.GetSections();
            //    foreach (var eachSection in sections)
            //    {
            //        eachSection.IsActive = false;
            //    }
            //    active = false;
            //}
            temp.SectionName = section.SectionName;
            temp.GradeId = section.GradeId;
            temp.IsActive = section.IsActive;
            temp.ClassTeacherId = section.ClassTeacherId;
            await _repository.UpdateAsync(temp);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Section");
            }
            ModelState.AddModelError("", "Error While Saving to Database");
            return View(section);
        }
        [Authorize(Policy = "Section.Delete")]
        [HttpGet]
        public async Task<IActionResult> DeleteSection(int id)
        {
            var temp = await _repository.GetSectionById(id);
            if (temp == null)
            {
                return NotFound();
            }
            temp.IsActive = false;
            await _repository.UpdateAsync(temp);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Section");
            }
            return RedirectToAction("Section");
        }
        #endregion

        #region SubjectAllocation

        [HttpGet]
        public async Task<IActionResult> SubjectAllocation(int Id)
        {
            var AllocatedSubjects = await _db.SubjectAllocations.Where(x => x.SectionId == Id).ToListAsync();
            var subjects = await _db.Subjects.Select(x => new SubjectList { SubjectId = x.SubjectId, SubjectName = x.SubjectName }).ToListAsync();
            SubjectAllocationVm subjectAllocationVM = new SubjectAllocationVm();
            subjectAllocationVM.SectionId = Id;
            subjectAllocationVM.Subjects = subjects.Select(x => new SubjectList { SubjectName = x.SubjectName, SubjectId = x.SubjectId }).ToList();
            foreach (var subject in subjectAllocationVM.Subjects)
            {
                foreach (var ASubject in AllocatedSubjects)
                {
                    if (subject.SubjectId == ASubject.SubjectId)
                    {
                        subject.preAllocation = true;
                    }
                }
            }
            var className = from sec in _db.Sections
                            join clas in _db.Grades on sec.GradeId equals clas.GradeId
                            select new
                            {
                                ClassName = clas.GradeName + "(" + sec.SectionName + ")"
                            };
            ViewBag.Section = className.FirstOrDefault().ToString();
            return View(subjectAllocationVM);
        }

        // 09 jan 2023 night work till here completed
        [HttpPost]
        public async Task<IActionResult> SubjectAllocation(SubjectAllocationVm data)
        {
            var result = await _db.SubjectAllocations.Where(x => x.SectionId == data.SectionId).ToListAsync();
            _db.RemoveRange(result);
            await _db.SaveChangesAsync();
            var test = data.Subjects.Where(x => x.IsSelected == true).ToList();
            int[] SubIds = new int[test.Count()];
            int i = -1;
            foreach (var bid in test)
            {
                i++;
                SubIds[i] = bid.SubjectId;
            }
            SubjectAllocation[] allocations = new SubjectAllocation[SubIds.Length];
            for (int book = 0; book < allocations.Length; book++)
            {
                var newBook = new SubjectAllocation
                {
                    SectionId = data.SectionId,
                    SubjectId = SubIds[book]
                };
                allocations[book] = newBook;
            }
            await _db.AddRangeAsync(allocations);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Section");
            }
            ModelState.AddModelError("", "Error While Saving to Database");
            return View(data);
        }
        #endregion

        #region BookALlocation

        [HttpGet]
        public async Task<IActionResult> BookAllocation(int Id)
        {
            var AllocatedBooks = await _db.bookAllocations.Where(x => x.SectionId == Id).ToListAsync();
            var Books = from a in _db.Sections
                        from b in _db.Grades
                        from c in _db.Books
                        from d in _db.Subjects
                        where a.SectionId == Id && b.GradeId == a.GradeId && c.GradeId == b.GradeId && d.SubjectId == c.SubjectId
                        select new
                        {
                            BookId = c.BookId,
                            BookName = c.BookName,
                            SubjectName = d.SubjectName
                        };
            BookAllocationVm bookAllocationVM = new BookAllocationVm();
            bookAllocationVM.SectionId = Id;
            bookAllocationVM.Books = await Books.Select(x => new BookList { BookId = x.BookId, BookName = x.BookName, SubjectName = x.SubjectName }).ToListAsync();
            foreach (var book in bookAllocationVM.Books)
            {
                foreach (var ABook in AllocatedBooks)
                {
                    if (book.BookId == ABook.BookId)
                    {
                        book.preAllocation = true;
                    }
                }
            }
            return View(bookAllocationVM);
        }
        [HttpPost]
        public async Task<IActionResult> BookAllocation(BookAllocationVm data)
        {
            var result = _db.bookAllocations
            .Where(p => p.SectionId.Equals(data.SectionId)).ToList();
            _db.RemoveRange(result);
            await _db.SaveChangesAsync();
            var test = data.Books.Where(x => x.IsSelected == true).ToList();
            int[] bookIds = new int[test.Count()];
            int i = -1;
            foreach (var bid in test)
            {
                i++;
                bookIds[i] = bid.BookId;
            }
            BookAllocation[] allocations = new BookAllocation[bookIds.Length];
            for (int book = 0; book < allocations.Length; book++)
            {
                var newBook = new BookAllocation
                {
                    SectionId = data.SectionId,
                    BookId = bookIds[book]
                };
                allocations[book] = newBook;
            }
            await _db.AddRangeAsync(allocations);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("Section");
            }
            ModelState.AddModelError("", "Error While Saving to Database");
            return View(data);
        }
        #endregion

        #region ResouceNoteBook
        [Authorize(Policy = "Notebook.Read")]
        [HttpGet]
        public async Task<IActionResult> ResourceNoteBook()
        {
            ResourceNoteBookVm noteBookVm = new ResourceNoteBookVm();
            var books = await _db.ResourceNoteBooks.ToListAsync();
            noteBookVm.NoteBooks.AddRange(books.Select(x => new ResourceNoteBookVm {IsActive = (bool)x.IsActive, ResourceNoteBookId = x.ResourceNoteBookId, ResourceNoteBookName = x.NoteBookName }).ToList());
            return View(noteBookVm);
        }
        //[HttpGet]
        //public async IActionResult AddResourceNoteBook()
        //{
        //    return View();
        //}
        [Authorize(Policy = "Notebook.Create")] 
        [HttpPost]
        public async Task<IActionResult> AddResourceNoteBook(ResourceNoteBookVm rBook)
        {
            var newRBook = new ResourceNoteBook
            {
                NoteBookName = rBook.ResourceNoteBookName
            };
            await _repository.AddAsync(newRBook);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("ResourceNoteBook");
            }
            else
            {
                ModelState.AddModelError("", "Error While Saving to Database");
            }
            return View(rBook);
        }
        [Authorize(Policy = "Notebook.Update")]
        [HttpGet]
        public async Task<IActionResult> UpdateResourceNoteBook(int id)
        {
            var temp = await _db.ResourceNoteBooks.Where(x => x.ResourceNoteBookId == id).FirstOrDefaultAsync();
            var RNBook = new ResourceNoteBookVm
            {
                ResourceNoteBookId = temp.ResourceNoteBookId,
                ResourceNoteBookName = temp.NoteBookName,
                IsActive = (bool)temp.IsActive
            };
            return View(RNBook);
        }
        [Authorize(Policy = "Notebook.Update")]
        [HttpPost]
        public async Task<IActionResult> UpdateResourceNoteBook(ResourceNoteBookVm rBook)
        {
            var temp = await _db.ResourceNoteBooks.Where(x => x.ResourceNoteBookId == rBook.ResourceNoteBookId).FirstOrDefaultAsync();
            temp.NoteBookName = rBook.ResourceNoteBookName;
            temp.IsActive = rBook.IsActive;
            await _repository.UpdateAsync(temp);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("ResourceNoteBook");
            }
            ModelState.AddModelError("", "Error While Saving to Database");
            return View(rBook);
        }
        [Authorize(Policy = "Notebook.Delete")]
        [HttpGet]
        public async Task<IActionResult> DeleteResourceNoteBook(int id)
        {
            var temp = await _db.ResourceNoteBooks.Where(x => x.ResourceNoteBookId == id).FirstOrDefaultAsync();
            if (temp == null)
            {
                return NotFound();
            }
            await _repository.Delete(temp);
            if (await _repository.SaveChanges())
            {
                return RedirectToAction("ResourceNoteBook");
            }
            return RedirectToAction("ResourceNoteBook");
        }
        #endregion

        #region Book-Details
        //[Authorize(Policy = "BookDetails.Read")]
        [HttpGet]
        public async Task<IActionResult> BookDetails(int BookId)
        {
            BookViewList bookDetailss = new BookViewList();
            var book = from bok in _db.Books
                       where bok.BookId == BookId
                       select new BookVM
                       {
                           BookName = bok.BookName,
                           Author = bok.Author,
                           GradeId = bok.BookId
                       };
            bookDetailss.BookId = (int)await book.Select(x => x.GradeId).FirstOrDefaultAsync();
            bookDetailss.BookName = await book.Select(x => x.BookName).FirstOrDefaultAsync();
            bookDetailss.Author = await book.Select(x => x.Author).FirstOrDefaultAsync();
            var units = from b in book
                        from u in _db.Units
                        where u.BookId == b.GradeId
                        select new UnitVM
                        {
                            StartPage = (int)u.StartPage,
                            EndPage = (int)u.EndPage,
                            UnitName = u.UnitName,
                            UnitId = u.UnitId,
                            BookId = (int)u.BookId

                        };
            bookDetailss.units.AddRange(await units.Select(x => new UnitVM { UnitName = x.UnitName, EndPage = x.EndPage, StartPage = x.StartPage, UnitId = x.UnitId, BookId = x.BookId }).ToListAsync());
            foreach (var unit in bookDetailss.units)
            {
                var chapters = from chapter in _db.chapters
                               where chapter.UnitId == unit.UnitId
                               select new ChapterVM
                               {
                                   BookId = chapter.ChapterId,
                                   ChapterName = chapter.ChapterName,
                                   EndPage = (int)chapter.EndPage,
                                   StartPage = (int)chapter.StartPage,
                                   UnitId = (int)chapter.UnitId
                               };
                unit.Chapters.AddRange(await chapters.Select(x => new ChapterVM { BookId = x.BookId, ChapterName = x.ChapterName, EndPage = x.EndPage, StartPage = x.StartPage, UnitId = x.UnitId }).ToListAsync());
                foreach (var chapter in unit.Chapters)
                {
                    var topcis = from topic in _db.topics
                                 where topic.ChapterId == chapter.BookId
                                 select new TopicVM
                                 {
                                     TopicName = topic.TopicName,
                                     EndPage = (int)topic.EndPage,
                                     StartPage = (int)topic.StartPage,
                                     ChapterId = (int)topic.ChapterId,
                                     TopicId = topic.TopicId
                                 };
                    chapter.topics.AddRange(await topcis.Select(x => new TopicVM { ChapterId = x.ChapterId, EndPage = x.EndPage, StartPage = x.StartPage, TopicName = x.TopicName, TopicId = x.TopicId }).ToListAsync());
                    foreach (var topic in chapter.topics)
                    {
                        var subTopics = from sTopic in _db.subTopics
                                        where sTopic.TopicId == topic.TopicId
                                        select new SubTopicVM
                                        {
                                            SubTopicName = sTopic.SubTopicName,
                                            StartPage = (int)sTopic.StartPage,
                                            EndPage = (int)sTopic.EndPage,
                                            TopicId = (int)sTopic.TopicId
                                        };
                        topic.SubTopics.AddRange(await subTopics.Select(x => new SubTopicVM { EndPage = x.EndPage, StartPage = x.StartPage, SubTopicName = x.SubTopicName, TopicId = x.TopicId }).ToListAsync());
                    }
                }
            }
            return View(bookDetailss);
        }
        #endregion

        #region Dynamic-Data

        public JsonResult GetClassTeachers(int GradeId)
        {
            var subjects = (from a in _db.Grades
                            from c in _db.Employees
                            where a.GradeId == GradeId && c.SchoolSectionId == a.SchoolSectionId
                            select new
                            {
                                EmployeeId = c.EmployeeId,
                                FName = c.FName,
                                LName = c.LName
                            }).Distinct().ToList();
            return Json(subjects);
        }
        #endregion
    }
}