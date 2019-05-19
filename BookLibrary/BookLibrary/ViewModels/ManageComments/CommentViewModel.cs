using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.ViewModels.ManageComments
{
    public class CommentViewModel
    {
        public string Id { get; set; }
        public DateTime Time { get; set; }
        public string OwnerId { get; set; }
        public string EssenceId { get; set; }
        public bool IsBook { get; set; }
        public string Text { get; set; }
    }
}
