using DataAccess.Interfaces;
using DataAccess.Models;
using Moq;
using Services.DTO;
using Services.Filters;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace Services.Tests
{
    class CommentServiceTests
    {
        private readonly List<Comment> _comments;

        public CommentServiceTests()
        {
            String _text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.";

            _comments = new List<Comment>
            {
                new Comment { Id = "id1", CommentedEssenceId = "ceId1", Time = new DateTime(2019, 01, 01), Text = _text},
                new Comment { Id = "id2", CommentedEssenceId = "ceId2", Time = new DateTime(2019, 02, 02), Text = _text},
                new Comment { Id = "id3", CommentedEssenceId = "ceId3", Time = new DateTime(2019, 03, 03), Text = _text}
            };
        }
    }
}
