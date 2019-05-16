using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BookLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.DTO;
using Services.Interfaces;

namespace BookLibrary.Controllers
{
    public class ManageCommentsController : Controller
    {
        
        private readonly ICommentService _commentService;

        public ManageCommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }
        
        [HttpPost]
        public IActionResult EditComment(string id, string ownerId, string essenceId, string isBook, string time, string text)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(ownerId) || string.IsNullOrEmpty(essenceId) || string.IsNullOrEmpty(isBook) || string.IsNullOrEmpty(text) || string.IsNullOrEmpty(time))
            {
                RedirectToAction("Error");
            }

            if (!bool.TryParse(isBook, out bool isBookToParse))
            {
                RedirectToAction("Error");
            }
            if (!DateTime.TryParse(time, out DateTime dateTime))
            {
                RedirectToAction("Error");
            }

            CommentDTO updatedComment = new CommentDTO
            {
                Id = id,
                OwnerId = ownerId,
                CommentedEssenceId = essenceId,
                Text = text,
                Time = dateTime
            };
            _commentService.Update(updatedComment);
            if (isBookToParse)
            {
                return RedirectToAction("GetBookInfo", "Home", new { id = essenceId });
            }
            else
            {
                return RedirectToAction("GetAuthorInfo", "Home", new { id = essenceId });
            }
        }

        [HttpPost]
        public IActionResult DeleteComment(string comentId, string isBook, string essenceId)
        {
            if (string.IsNullOrEmpty(comentId) || string.IsNullOrEmpty(isBook) || string.IsNullOrEmpty(essenceId))
            {
                RedirectToAction("Error");
            }
            if (!bool.TryParse(isBook, out bool isBookToParse))
            {
                return RedirectToAction("Error");
            }


            if (_commentService.Get(comentId) == null)
            {
                RedirectToAction("Error");
            }
            _commentService.Remove(comentId);
            if (isBookToParse)
            {
                return RedirectToAction("GetBookInfo", "Home", new { id = essenceId });
            }
            else
            {
                return RedirectToAction("GetAuthorInfo", "Home", new { id = essenceId });
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddComment(string ownerId, string essenceId, string isBook, string text)
        {
            
            if (string.IsNullOrEmpty(ownerId) || string.IsNullOrEmpty(essenceId) || string.IsNullOrEmpty(isBook) || string.IsNullOrEmpty(text))
            {
                RedirectToAction("Error");
            }
            if (!bool.TryParse(isBook, out bool isBookToParse))
            {
                return RedirectToAction("Error");
            }

            CommentDTO newComment = new CommentDTO
            {
                OwnerId = ownerId,
                CommentedEssenceId = essenceId,
                Text = text,
                Time = DateTime.Now
            };
            _commentService.Add(newComment);
            if (isBookToParse)
            {
                return RedirectToAction("GetBookInfo", "Home", new { id = essenceId });
            }
            else
            {
                return RedirectToAction("GetAuthorInfo", "Home", new { id = essenceId });
            }
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}