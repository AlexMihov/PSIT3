using Quizio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.ViewModels
{
    public class MainViewModel
    {
        public User CurrentUser { get; private set; }

        public IEnumerable<Notification> Notifications { get; set; }

        public IEnumerable<Category> Categories { get; set; }

        public IEnumerable<Ranking> Rankings { get; set; }

        public RegularGameViewModel RegularGameViewModel { get; set; }

        public MainViewModel(User user, List<Category> categories,  List<Notification> notifications, List<Ranking> rankings)
        {
            this.CurrentUser = user;
            this.Categories = categories;
            this.Notifications = notifications;
            this.Rankings = rankings;
            this.RegularGameViewModel = new RegularGameViewModel(user, categories);
        }
    }
}
