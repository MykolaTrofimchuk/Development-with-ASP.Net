using JobPortal.Views.ViewModels;
using System.Collections.Generic;

namespace JobPortal.Models.ViewModels
{
    public class UsersListViewModel
    {
        public IEnumerable<User> Users { get; set; } = Enumerable.Empty<User>();
        public PagingInfo PagingInfo { get; set; } = new PagingInfo();
    }
}