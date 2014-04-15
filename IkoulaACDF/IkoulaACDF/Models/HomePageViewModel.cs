using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IkoulaACDF.Models
{
    public class HomePageViewModel
    {
        public IEnumerable<PhotoViewModel> Photos { get; set; }
        public IEnumerable<AcdfGuessBook> GuessBooks { get; set; }
        public IEnumerable<AcdfGuessBook> LastFiveGuessBooks { get; set; }
        public IEnumerable<AspNetUser> AspNetUsers { get; set; }
        public IDictionary<AspNetUser, int> BirthDays { get; set; }
        public HomePageViewModel(IEnumerable<PhotoViewModel> photos, IEnumerable<AcdfGuessBook> guessBooks, IEnumerable<AcdfGuessBook> lastFiveGuessBooks, IEnumerable<AspNetUser> aspnetUsers, IEnumerable<AspNetUser> birthdays)
        {
            Photos = photos;

            GuessBooks = guessBooks;

            LastFiveGuessBooks = lastFiveGuessBooks;

            AspNetUsers = aspnetUsers;

            BirthDays = new Dictionary<AspNetUser, int>();
            foreach (var b in birthdays)
            {
                BirthDays.Add(b, DateTime.Now.Year - b.BirthDate.Value.Year);
            }
        }
    }

    // BirthDays with ViewBag
    //public class HomePageViewModel
    //{
    //    public HomePageViewModel()
    //    {
    //    }
    //    public IEnumerable<PhotoViewModel> HomePagePhotoViewModel { get; set; }
    //    public IEnumerable<AcdfGuessBook> HomePageGuessBookViewModel { get; set; }
    //    public IEnumerable<AspNetUser> HomePageAspNetUsersViewModel { get; set; }
    //    public IEnumerable<AspNetUser> HomePageBirthDaysViewModel { get; set; }
    //}
}