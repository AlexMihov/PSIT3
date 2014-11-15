﻿using Quizio.Models;
using Quizio.Utilities;
using Quizio.ViewModels;
using Quizio.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Quizio
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void ApplicationStart(object sender, StartupEventArgs e)
        {
            //Disable shutdown when the dialog closes
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            var login = new Login();

            if (login.ShowDialog() == true && login.granted)
            {
                //data retrieve simulation (to be fetched from DB)
                string name = login.userName.Text;
                string currentStatus = "I love Quizio";

                Friend hans = new Friend("Hans", "Muster bedumtsch");
                Friend fritz = new Friend("Fritz", "Ritz bedumtsch");
                Friend rudolf = new Friend("Rudolf", "Liebt Golf bedumtsch");
                Friend michel = new Friend("Michel", "Mit der Sichel bedumtsch");

                List<Friend> friends = new List<Friend>();
                friends.Add(hans);
                friends.Add(fritz);
                friends.Add(rudolf);
                friends.Add(michel);

                User user = new User(name, currentStatus, friends);

                List<Notification> notifications = new List<Notification>();
                Notification first = new Notification("Hans hat gerade ein Quiz gegen Michel gewonnen.");
                Notification second = new Notification("Fritz hat dich zu einem Quiz herausgefordert!");
                Notification third = new Notification("Michel hat sein Status zu 'Sichelfetischist' geändert.");
                Notification fourth = new Notification("Glückwunsch, du bist gerade in den Rankings auf die Top 3 gestiegen!");
                notifications.Add(first);
                notifications.Add(second);
                notifications.Add(third);
                notifications.Add(fourth);

                List<Ranking> rankings = new List<Ranking>();
                Ranking r1 = new Ranking(1, "XXXPornoUser", 1065013205);
                Ranking r2 = new Ranking(2, name, 106501320);
                Ranking r3 = new Ranking(3, "KillaWieCam", 10650132);
                Ranking r4 = new Ranking(4, "BeschteWosGiz", 10013205);
                Ranking r5 = new Ranking(5, "BinMoslemWeisch_aKaBMW", 6513205);
                rankings.Add(r1);
                rankings.Add(r2);
                rankings.Add(r3);
                rankings.Add(r4);
                rankings.Add(r5);

                List<Answer> answers = new List<Answer>();
                Answer ans1 = new Answer("25", true);
                Answer ans2 = new Answer("20", false);
                Answer ans3 = new Answer("200", false);
                Answer ans4 = new Answer("15", false);
                answers.Add(ans1);
                answers.Add(ans2);
                answers.Add(ans3);
                answers.Add(ans4);

                List<Question> questions = new List<Question>();
                Question qs1 = new Question(answers, "x quadratisch", "Eine Zahl mit sich selbst Multipliziert ergibt in der Quersumme 7");
                questions.Add(qs1);

                List<Quiz> quizies1 = new List<Quiz>();
                Quiz qu1 = new Quiz("Gleichungen", questions);
                Quiz qu2 = new Quiz("Satzaufgaben", questions);
                Quiz qu3 = new Quiz("Prosa", questions);
                Quiz qu4 = new Quiz("Relationale Algebra", questions);
                quizies1.Add(qu1);
                quizies1.Add(qu2);
                quizies1.Add(qu3);
                quizies1.Add(qu4);

                List<Quiz> quizies2 = new List<Quiz>();
                Quiz qu11 = new Quiz("Thesaurus", questions);
                Quiz qu22 = new Quiz("Vokabulary", questions);
                Quiz qu33 = new Quiz("Grammar", questions);
                Quiz qu44 = new Quiz("True or false", questions);
                quizies2.Add(qu11);
                quizies2.Add(qu22);
                quizies2.Add(qu33);
                quizies2.Add(qu44);

                CategoryDAO catDao = new CategoryDAO();
                List<Category> categories = catDao.loadCategories();
                //Category ct1 = new Category("Mathematik",quizies1);
                //Category ct2 = new Category("English", quizies2);
                //categories.Add(ct1);
                //categories.Add(ct2);

                MainViewModel mvm = new MainViewModel(user, categories, notifications, rankings);
                //data retrieve simulation end

                var mainWindow = new MainWindow(mvm);
                //Re-enable normal shutdown mode.
                Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                Current.MainWindow = mainWindow;
                mainWindow.Show();
            }
            else
            {
                Current.Shutdown(-1);
            }
        }
    }
}
