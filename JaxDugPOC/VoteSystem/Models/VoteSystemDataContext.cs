using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace VoteSystem.Models
{
    public class VoteSystemDataContext : DbContext
    {
        public DbSet<Vote> Vote { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Topic> Topic { get; set; }
        public DbSet<Suggestion> Suggestion { get; set; }
        //public DbSet<User> User { get; set; }

        public VoteSystemDataContext()
            : base("VoteSystem")
        {
            Database.SetInitializer(new Initializer<VoteSystemDataContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }

    public class Initializer<TContext> : IDatabaseInitializer<TContext> where TContext : VoteSystemDataContext
    {
        public void InitializeDatabase(TContext context)
        {
            var exists = context.Database.Exists();
            //if (exists && context.Database.CompatibleWithModel(true))
            //{
            //    return;
            //}

            if (exists)
            {
                //Console.WriteLine("The database exist but the model does not march");
                //Console.Write("Do you wish to drop and recreate the database (Y/N)");
                //var answer = Console.ReadKey();
                //Console.WriteLine();
                //if (answer.KeyChar == 'N') return;
                context.Database.Delete();
            }

            context.Database.Create();
            Seed(context);

        }

        protected void Seed(VoteSystemDataContext context)
        {
            const int baseId = 00000;

            #region MockUser
            //var user = new List<User>
            //                     {
            //                         new User
            //                             {
            //                                 UserId = baseId + 1,
            //                                 FirstName = "Tony",
            //                                 LastName = "Rose",
            //                                 Email = "xecilamx@gmail.com",
            //                                 UserName = "lookitstony",
            //                                 IsAdmin = true
            //                             },
            //                         new User
            //                             {
            //                                 UserId = baseId + 2,
            //                                 FirstName = "Joe",
            //                                 LastName = "Rose",
            //                                 Email = "joezrose@gmail.com",
            //                                 UserName = "joezrose",
            //                                 IsAdmin = false
            //                             },
            //                         new User
            //                             {
            //                                 UserId = baseId + 1,
            //                                 FirstName = "Anon",
            //                                 LastName = "YMox",
            //                                 Email = "subzero@gmail.com",
            //                                 UserName = "Mker",
            //                                 IsAdmin = true
            //                             }

            //                     };
            //user.ForEach(u => context.User.Add(u));
            #endregion
            #region MockTopics
            var topics = new List<Topic>
                                 {
                                     new Topic
                                         {
                                             TopicId = baseId + 1,
                                             Name = "Website Features",
                                             TopicCreated = DateTime.Now.Subtract(TimeSpan.FromDays(28)),
                                             UserId = 00001
                                         },
                                     new Topic
                                         {
                                             TopicId = baseId + 2,
                                             Name = "Presentations",
                                             TopicCreated = DateTime.Now.Subtract(TimeSpan.FromDays(12)),
                                             UserId = 00002
                                         }
                                 };
            topics.ForEach(t => context.Topic.Add(t));
            #endregion
            #region MockSuggestions

            var suggest = new List<Suggestion>()
                              {
                                  new Suggestion()
                                      {
                                          SuggestionId = baseId + 1,
                                          UserId = baseId + 1,
                                          TopicId = baseId +1,
                                          Content = "Build a custom voting system from scratch.",
                                          SuggestionCreated = DateTime.Now.Subtract(TimeSpan.FromDays(23)),
                                          AllowMultipleTopics = false
                                      },

                                  new Suggestion()
                                      {
                                          SuggestionId = baseId + 2,
                                          UserId = baseId + 2,
                                          TopicId= baseId +2,
                                          Content = "Make the site Orange.",
                                          SuggestionCreated = DateTime.Now.Subtract(TimeSpan.FromDays(10)),
                                          AllowMultipleTopics = false
                                      },

                                  new Suggestion()
                                      {
                                          SuggestionId = baseId + 3,
                                          UserId = baseId + 1,
                                          TopicId = baseId +1,
                                          Content = "Solar powered robots should clean the windows at night.",
                                          SuggestionCreated = DateTime.Now.Subtract(TimeSpan.FromDays(3)),
                                          AllowMultipleTopics = false
                                      }
                              };

            suggest.ForEach(s => context.Suggestion.Add(s));

            #endregion
            #region MockVote

            var vote = new List<Vote>()
                           {
                               new Vote()
                                   {
                                       VoteId = baseId + 1,
                                       UserId = baseId + 1,
                                       SuggestionId = baseId +1,
                                       Descision = Descision.Plus,
                                       VoteCreated = DateTime.Now.Subtract(TimeSpan.FromDays(23))
                                      },

                                  new Vote()
                                      {
                                       VoteId = baseId + 2,
                                       UserId = baseId + 1,
                                       SuggestionId = baseId +2,
                                       Descision = Descision.Negative,
                                       VoteCreated = DateTime.Now.Subtract(TimeSpan.FromDays(13))
                                      },

                                  new Vote()
                                      {
                                          VoteId = baseId + 3,
                                          UserId = baseId + 1,
                                          SuggestionId = baseId +2,
                                          Descision = Descision.Plus,
                                          VoteCreated = DateTime.Now.Subtract(TimeSpan.FromDays(3))
                                      },
                                  new Vote()
                                   {
                                       VoteId = baseId + 4,
                                       UserId = baseId + 1,
                                       SuggestionId = baseId +3,
                                       Descision = Descision.Plus,
                                       VoteCreated = DateTime.Now.Subtract(TimeSpan.FromDays(23))
                                      },

                                  new Vote()
                                      {
                                       VoteId = baseId + 5,
                                       UserId = baseId + 2,
                                       SuggestionId = baseId +2,
                                       Descision = Descision.Negative,
                                       VoteCreated = DateTime.Now.Subtract(TimeSpan.FromDays(13))
                                      },

                                  new Vote()
                                      {
                                          VoteId = baseId + 6,
                                          UserId = baseId + 3,
                                          SuggestionId = baseId +3,
                                          Descision = Descision.Plus,
                                          VoteCreated = DateTime.Now.Subtract(TimeSpan.FromDays(3))
                                      }
                              };

            vote.ForEach(v => context.Vote.Add(v));

            #endregion
            context.SaveChanges();
        }
    }
}