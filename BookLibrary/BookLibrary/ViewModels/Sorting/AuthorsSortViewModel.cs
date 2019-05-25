using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLibrary.ViewModels.Sorting.States;

namespace BookLibrary.ViewModels.Sorting
{
    public class AuthorsSortViewModel
    {
        public AuthorsSort NameSort { get; private set; }
        public AuthorsSort SurnameSort { get; private set; }
        public AuthorsSort Current { get; private set; }

        public AuthorsSortViewModel(AuthorsSort sortOrder)
        {
            NameSort = sortOrder == AuthorsSort.NAME_ASC ? AuthorsSort.NAME_DESC : AuthorsSort.NAME_ASC;
            SurnameSort = sortOrder == AuthorsSort.SURNAME_ASC ? AuthorsSort.SURNAME_DESC : AuthorsSort.SURNAME_ASC;

            Current = sortOrder;
        }

    }
}
