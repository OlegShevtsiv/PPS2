using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Services.DTO;
using BookLibrary.ViewModels.ManageLibrary;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using BookLibrary.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Services.Filters;

namespace BookLibrary.Controllers
{
    [Authorize(Roles = "library admin")]
    public class ManageLibraryController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IAuthorService _authorService;
        private readonly IRateService _rateService;
        private readonly ICommentService _commentService;
        public ManageLibraryController(IBookService bookService, IAuthorService authorService, IRateService rateService, ICommentService commentService)
        {
            _bookService = bookService;
            _authorService = authorService;
            _commentService = commentService;
            _rateService = rateService;
        }

        [HttpGet]
        public IActionResult Index() => View(_bookService.GetAll().ToList().OrderByDescending(b => b.Rate));

        [HttpGet]
        public IActionResult AddBook()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddBook(AddBookViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                {
                    return RedirectToAction("Error");
                }
                BookDTO newBook = new BookDTO
                {
                    Title = model.Title.Trim(),
                    AuthorId = model.AuthorId,
                    Genre = model.Genre.Trim(),
                    Rate = model.Rate,
                    Description = model.Description,
                    Year = model.Year,
                    RatesAmount = model.RatesAmount
                };
                if (model.Image != null && model.FileBook != null)
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(model.Image.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)model.Image.Length);
                    }
                    newBook.Image = imageData;

                    byte[] fileData = null;
                    using (var binaryReader = new BinaryReader(model.FileBook.OpenReadStream()))
                    {
                        fileData = binaryReader.ReadBytes((int)model.FileBook.Length);
                    }
                    newBook.FileBook = fileData;
                }
                else
                {
                    return RedirectToAction("Error");
                }

                _bookService.Add(newBook);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditBook(string id)
        {
            //ViewBag.Authors = _authorService.GetAll().ToList();

            BookDTO getedBook = _bookService.Get(id);
            if (getedBook == null)
            {
                return RedirectToAction("Error");
            }
            EditBookViewModel model = new EditBookViewModel {
                Id = getedBook.Id,
                Title = getedBook.Title.Trim(),
                AuthorId = getedBook.AuthorId,
                Rate = getedBook.Rate,
                Year = getedBook.Year,
                Description = getedBook.Description,
                Genre = getedBook.Genre,
                RatesAmount = getedBook.RatesAmount
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult EditBook(EditBookViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                {
                    return RedirectToAction("Error");
                }
                BookDTO editedBook = new BookDTO
                {
                    Id = model.Id,
                    Title = model.Title.Trim(),
                    AuthorId = model.AuthorId,
                    Genre = model.Genre,
                    Rate = model.Rate,
                    Description = model.Description,
                    Year = model.Year,
                    RatesAmount = model.RatesAmount
                };
                if (model.Image != null && model.FileBook != null)
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(model.Image.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)model.Image.Length);
                    }
                    editedBook.Image = imageData;

                    byte[] fileData = null;
                    using (var binaryReader = new BinaryReader(model.FileBook.OpenReadStream()))
                    {
                        fileData = binaryReader.ReadBytes((int)model.FileBook.Length);
                    }
                    editedBook.FileBook = fileData;
                }
                else
                {
                    return RedirectToAction("Error");
                }

                _bookService.Update(editedBook);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteBook(string id)
        {
            foreach (var c in _commentService.Get(new CommentFilter { CommentedEssenceId =  id}))
            {
                _commentService.Remove(c.Id);
            }
            foreach (var r in _rateService.Get(new RateFilterByBookId { BookId = id }))
            {
                _rateService.Remove(r.Id);
            }
            _bookService.Remove(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AuthorsList() => View(_authorService.GetAll().ToList());

        [HttpGet]
        public IActionResult AddAuthor()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddAuthor(AddAuthorViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (model == null)
                {
                    return RedirectToAction("Error");
                }

                AuthorDTO newAuthor = new AuthorDTO
                {
                    Name = model.Name.Trim(),
                    Description = model.Description,
                    Surname = model.Surname.Trim()
                };
                if (model.Image != null)
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(model.Image.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)model.Image.Length);
                    }
                    newAuthor.Image = imageData;
                }
                else
                {
                    return RedirectToAction("Error");
                }
                _authorService.Add(newAuthor);
            }
            
            return RedirectToAction("AuthorsList");
        }

        [HttpGet]
        public IActionResult EditAuthor(string id)
        {
            AuthorDTO getedAuthor = _authorService.Get(id);
            if (getedAuthor == null)
            {
                return RedirectToAction("Error");
            }
            EditAuthorViewModel model = new EditAuthorViewModel
            {
                Name = getedAuthor.Name.Trim(),
                Surname = getedAuthor.Surname.Trim(),
                Description = getedAuthor.Description
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult EditAuthor(EditAuthorViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                {
                    throw new Exception();
                }

                AuthorDTO newAuthor = new AuthorDTO
                {
                    Id = model.Id,
                    Name = model.Name.Trim(),
                    Description = model.Description,
                    Surname = model.Surname.Trim()
                };
                if (model.Image != null)
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(model.Image.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)model.Image.Length);
                    }
                    newAuthor.Image = imageData;
                }
                else
                {
                    throw new Exception();
                }
                _authorService.Update(newAuthor);
            }
            return RedirectToAction("AuthorsList");
        }

        [HttpPost]
        public IActionResult DeleteAuthor(string id)
        {
            foreach (var c in _commentService.Get(new CommentFilter { CommentedEssenceId = id }))
            {
                _commentService.Remove(c.Id);
            }
            _authorService.Remove(id);
            return RedirectToAction("AuthorsList");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}