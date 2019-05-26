using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLibrary.ViewModels.Sorting.States;

namespace BookLibrary.ViewModels.Sorting
{
    public class AuthorsSortViewModel
    {
        public SortEnum NameSort { get; private set; }
        public SortEnum SurnameSort { get; private set; }
        public SortEnum Current { get; private set; }
        public bool Up { get; set; }
        public AuthorsSortViewModel(SortEnum sortOrder)
        {
            NameSort = sortOrder == SortEnum.NAME_ASC ? SortEnum.NAME_DESC : SortEnum.NAME_ASC;
            SurnameSort = sortOrder == SortEnum.SURNAME_ASC ? SortEnum.SURNAME_DESC : SortEnum.SURNAME_ASC;
            
            Up = true;

            if (sortOrder == SortEnum.NAME_DESC || sortOrder == SortEnum.SURNAME_DESC)
            {
                Up = false;
            }
            Current = sortOrder;
            //switch (sortOrder)
            //{
            //    case SortEnum.NAME_DESC:
            //        Current = SortEnum.NAME_ASC;
            //        break;
            //    case SortEnum.SURNAME_ASC:
            //        Current = SortEnum.SURNAME_DESC;
            //        break;
            //    case SortEnum.SURNAME_DESC:
            //        Current = SortEnum.SURNAME_ASC;
            //        break;
            //    default:
            //        Current = SortEnum.NAME_DESC;
            //        break;
            //}
        }
    }
}
