using AutoMapper;
using Blog.Domain;
using Blog.Services;
using Blog.Services.Administrator;
using Blog.Utils;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Blog.Tests.Services.Administrator
{
    [Trait("Category", "Integration")]
    public class ServiceTests
    {
        public ServiceTests()
        {
            _options = Db.CreateOptions();

            using (var seed = new BlogContext(_options))
            {
                seed.Database.EnsureCreated();
                seed.Infos.Add(new PostInfo
                {
                    Id = 1,
                    Language = Language.English,
                    PublishDate = new DateTime(2019, 1, 1),
                    Summary = "Learning FP in Javascript",
                    Tags = "JS, FP, Node.js",
                    Title = "Javascript FP"
                });
                seed.Infos.Add(new PostInfo
                {
                    Id = 2,
                    Language = Language.English,
                    PublishDate = DateTime.Now,
                    Summary = "Learning OOP in C#",
                    Tags = "OOP, C#",
                    Title = "Object Oriented C#"
                });
                seed.Infos.Add(new PostInfo
                {
                    Id = 3,
                    Language = Language.Farsi,
                    PublishDate = DateTime.Now,
                    Summary = "استفاده از جاوا در ویندوز",
                    Tags = "Java",
                    Title = "جاوا و ویندوز"
                });
                seed.Drafts.Add(new Draft
                {
                    Id = 1,
                    Content = "<p>JS Functional Programming</p>"
                });
                seed.Drafts.Add(new Draft
                {
                    Id = 2,
                    Content = "<p>Object Oriented C#</p>"
                });
                seed.Drafts.Add(new Draft
                {
                    Id = 3,
                    Content = "<p>جاوا و ویندوز</p>"
                });
                seed.Posts.Add(new Post
                {
                    Id = 1,
                    Url = "Javascript-FP"
                });
                seed.Posts.Add(new Post
                {
                    Id = 2,
                    Url = "Object-Oriented-Csharp"
                });
                seed.SaveChanges();
            }

            var context = new BlogContext(_options);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<PostProfile>();
            });
            _imageContext = new Mock<IImageContext>();
            _services = new Service(context, config.CreateMapper(), _imageContext.Object);
        }

        private readonly DbContextOptions _options;
        private readonly Mock<IImageContext> _imageContext;
        private readonly Service _services;

        [Fact]
        public void New_post_date_is_set_to_today()
        {
            _services
                .Create()
                .PublishDate
                .Should()
                .HaveDay(DateTime.Now.Day)
                .And
                .HaveMonth(DateTime.Now.Month)
                .And
                .HaveYear(DateTime.Now.Year);
        }

        [Fact]
        public void GetDrafts_get_all_of_them()
        {
            var drafts = _services.GetDrafts();

            drafts.Should().HaveCount(3);

            drafts
                .First()
                .Should()
                .BeEquivalentTo(new
                {
                    Id = 1,
                    Title = "Javascript FP",
                    Published = true
                });

            drafts
                .ElementAt(1)
                .Should()
                .BeEquivalentTo(new
                {
                    Id = 2,
                    Title = "Object Oriented C#",
                    Published = true
                });

            drafts
                .ElementAt(2)
                .Should()
                .BeEquivalentTo(new
                {
                    Id = 3,
                    Title = "جاوا و ویندوز",
                    Published = false
                });
        }

        [Fact]
        public void Get()
        {
            var draft = _services.Get(1);

            draft.Content.Should().Be("<p>JS Functional Programming</p>");
            draft.Id.Should().Be(1);
            draft.Language.Should().Be(Language.English);
            draft.PublishDate.Should().Be(new DateTime(2019, 1, 1));
            draft.Summary.Should().Be("Learning FP in Javascript");
            draft.Tags.Should().Be("JS, FP, Node.js");
            draft.Title.Should().Be("Javascript FP");
        }

        [Fact]
        public void Dont_save_duplicate_titles()
        {
            var entry = new DraftEntry
            {
                Title = "Javascript FP"
            };

            _services
                .Invoking(x => x.Save(entry))
                .Should()
                .Throw<ValidationException>()
                .WithMessage("This title already exists in the database.");
        }

        [Fact]
        public void Add_new_drafts()
        {
            var entry = new DraftEntry
            {
                Content = "<h1>Content</h1>",
                Language = Language.English,
                PublishDate = new DateTime(2019, 1, 1),
                Summary = "Summary",
                Tags = "tagA, tagB",
                Title = "Title"
            };

            _services.Save(entry);

            entry.Id = 4;
            _services
                .Get(4)
                .Should()
                .BeEquivalentTo(entry);
        }

        [Fact]
        public void Update_existing_drafts()
        {
            _services.Save(new DraftEntry
            {
                Id = 1,
                Content = "<h1>FP</h1>",
                Title = "FP",
                Summary = "Summary",
                Tags = "Tags"
            });

            _services
                .Get(1)
                .Content
                .Should()
                .Be("<h1>FP</h1>");
        }

        [Fact]
        public void Create_new_post_of_a_draft()
        {
            _services.Save(new DraftEntry
            {
                Content = "<h1>Content</h1>",
                Language = Language.English,
                PublishDate = new DateTime(2019, 1, 1),
                Publish = true,
                Summary = "Summary",
                Tags = "Tags",
                Title = "Title"
            });

            using (var ctx = new BlogContext(_options))
            {
                ctx
                    .Posts
                    .Find(4)
                    .Should()
                    .BeEquivalentTo(new
                    {
                        Content = "<h1>Content</h1>"
                    });
            }
        }

        [Fact]
        public void Update_old_publish_of_a_draft()
        {
            _services.Save(new DraftEntry
            {
                Id = 1,
                Content = "<p>New Content</p>",
                Language = Language.English,
                PublishDate = new DateTime(2019, 1, 1),
                Publish = true,
                Summary = "Summary",
                Tags = "Tags",
                Title = "New Content"
            });

            using (var context = new BlogContext(_options))
            {
                context
                    .Posts
                    .Find(1)
                    .Should()
                    .BeEquivalentTo(new
                    {
                        Url = "New-Content",
                        Content = "<p>New Content</p>"
                    });
            }
        }

        [Fact]
        public void Delete_post_when_draft_publish_is_not_checked()
        {
            _services.Save(new DraftEntry
            {
                Id = 1,
                Title = "New Title",
                Content = "<p>New Content</p>",
                Summary = "SUMMARY",
                Tags = "Tags",
                Publish = false
            });

            using (var context = new BlogContext(_options))
            {
                context
                    .Posts
                    .SingleOrDefault(x => x.Id == 1)
                    .Should()
                    .BeNull();
            }
        }

        [Fact]
        public void Save_images()
        {
            _services.Save(new DraftEntry
            {
                Content = "<figure><img data-filename=\"pic.jpeg\" src=\"data:image/jpeg;base64,DATA\"></figure>",
                Title = "the post",
                Summary = "SUMMARY"
            });

            _imageContext.Verify(x => x.SaveChanges(It.IsAny<IEnumerable<Image>>()));
        }

        [Fact]
        public void Delete_draft()
        {
            _services.Delete(3);

            using (var ctx = new BlogContext(_options))
            {
                ctx.Infos
                    .Should()
                    .HaveCount(2);

                ctx.Drafts
                    .Should()
                    .HaveCount(2);

                ctx.Posts
                    .Should()
                    .HaveCount(2);
            }
        }

        [Fact]
        public void Delete_post()
        {
            _services.Delete(1);

            using (var ctx = new BlogContext(_options))
            {
                ctx.Infos
                    .Should()
                    .HaveCount(2);

                ctx.Drafts
                  .Should()
                  .HaveCount(2);

                ctx.Posts
                  .Should()
                  .HaveCount(1);
            }
        }
    }
}
