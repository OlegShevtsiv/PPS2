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
    public class CommentServiceTests
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

        [Fact]
        public void GetByFilterTest()
        {
            //Arange
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Comment>> repositoryMock = new Mock<IRepository<Comment>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<Comment, bool>>>())).Returns(_comments.AsQueryable);
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<Comment>()).Returns(repositoryMock.Object);
            CommentService _commentService = new CommentService(unitOfWorkMock.Object);
            CommentFilter _commentFilter = new CommentFilter();

            //Act
            IEnumerable<CommentDTO> _commentsDto = _commentService.Get(_commentFilter);

            //Assert
            Assert.NotNull(_commentsDto);
            Assert.NotEmpty(_commentsDto);
            Assert.Equal(3, _commentsDto.Count());
        }
    }
}
