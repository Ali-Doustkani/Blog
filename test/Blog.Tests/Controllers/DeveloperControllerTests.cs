using Blog.Domain.DeveloperStory;
using FluentAssertions;
using FluentAssertions.Json;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Blog.Tests.Controllers
{
   public class DeveloperControllerTests : IClassFixture<ApiTestContext>
   {
      public DeveloperControllerTests(ApiTestContext fixture)
      {
         fixture.InitializeDatabase();
         _client = fixture.Client;
      }

      private readonly HttpClient _client;

      [Fact]
      public async Task Return_204_when_no_developer_is_available()
      {
         using (var response = await _client.GetAsync("/api/developer"))
         {
            response.StatusCode
               .Should()
               .Be(HttpStatusCode.NoContent);
         }
      }

      [Fact]
      public async Task Return_201_when_new_developer_is_added()
      {
         var developer = new
         {
            summary = "a web programmer",
            skills = "C#, HTML"
         };

         using (var response = await _client.PutAsJsonAsync("/api/developer", developer))
         {
            response.StatusCode
               .Should()
               .Be(HttpStatusCode.Created);
         }
      }

      [Fact]
      public async Task Return_200_when_developer_is_updated()
      {
         await _client.PutAsJsonAsync("/api/developer", new
         {
            summary = "a web programmer",
            skills = "C#, HTML"
         });

         var update = new
         {
            summary = "a passionate web developer",
            skills = "C#"
         };

         using (var response = await _client.PutAsJsonAsync("/api/developer", update))
         {
            response.StatusCode
               .Should()
               .Be(HttpStatusCode.OK);
         }
      }

      [Fact]
      public async Task Return_developer_when_its_available()
      {
         var developer = new
         {
            summary = "The best developer ever!",
            skills = "C#, Javascript, React",
            experiences = new[]
            {
               new
               {
                  id = "a",
                  company = "Parmis",
                  position = "C# Developer",
                  startDate = "2016-1-1",
                  endDate = "2017-1-1",
                  content = "System Architect"
               }
            },
            sideProjects = new[]
            {
               new
               {
                  id = "a",
                  title = "Richtext Editor",
                  content = "A simple richtext for web"
               }
            },
            educations = new[]
            {
               new
               {
                  id = "a",
                  degree = "BS",
                  university = "S&C",
                  startDate = "2010-1-1",
                  endDate = "2011-1-1"
               }
            }
         };

         await _client.PutAsJsonAsync("/api/developer", developer);

         using (var response = await _client.GetAsync("/api/developer"))
         {
            var result = JsonConvert.DeserializeObject<DeveloperUpdateCommand>(await response.Content.ReadAsStringAsync());
            result.Should().BeEquivalentTo(new
            {
               Summary = "The best developer ever!",
               Skills = "C#, Javascript, React",
               Experiences = new[]
               {
                  new
                  {
                     Company = "Parmis",
                     Position= "C# Developer",
                     Content = "System Architect",
                     StartDate="2016-01-01",
                     EndDate="2017-01-01"
                  }
               },
               SideProjects = new[]
               {
                  new
                  {
                     Title="Richtext Editor",
                     Content="A simple richtext for web"
                  }
               },
               Educations = new[]
               {
                  new
                  {
                     Degree = "BS",
                     University = "S&C",
                     StartDate = "2010-01-01",
                     EndDate = "2011-01-01"
                  }
               }
            });
         }
      }

      [Fact]
      public async Task Add_when_there_is_no_developer_available()
      {
         var add1 = new
         {
            summary = "Cool guy!",
            skills = "C#, Javascript, React",
            experiences = new[]
            {
               new
               {
                  id = "abc-123-efg",
                  company = "Microsoft",
                  content = "as backend developer",
                  startDate = "2016-02-23",
                  endDate = "2017-01-02",
                  position = "Lead Developer"
               }
            },
            sideProjects = new[]
            {
               new
               {
                  id = "abc-123-efg",
                  title = "Richtext Editor",
                  content = "A simple richtext for web"
               }
            },
            educations = new[]
            {
               new
               {
                  id = "frg-234",
                  degree = "BS",
                  university = "S&C",
                  startDate = "2010-1-1",
                  endDate = "2011-1-1"
               }
            }
         };

         using (var response = await _client.PutAsJsonAsync("/api/developer", add1))
         {
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var result = JToken.Parse(await response.Content.ReadAsStringAsync());
            var expected = JsonConvert.SerializeObject(new
            {
               experiences = new[] { 1 },
               sideProjects = new[] { 1 },
               educations = new[] { 1 }
            });
            result.Should().BeEquivalentTo(expected);
         }

         var add2 = new
         {
            summary = "Cool guy!",
            skills = "C#, Javascript, React",
            experiences = new[]
            {
               new
               {
                  id = "a",
                  company = "Microsoft",
                  content = "as backend developer",
                  startDate = "2016-02-23",
                  endDate = "2017-01-02",
                  position = "Lead Developer"
               }
            },
            sideProjects = new[]
            {
               new
               {
                  id = "a",
                  title = "Richtext Editor",
                  content = "A simple richtext for web"
               }
            },
            educations = new[]
            {
               new
               {
                  id = "a",
                  degree = "BS",
                  university = "S&C",
                  startDate = "2010-01-01",
                  endDate = "2011-01-01"
               }
            }
         };

         using (var response = await _client.PutAsJsonAsync("/api/developer", add2))
         {
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = JToken.Parse(await response.Content.ReadAsStringAsync());
            var expected = JsonConvert.SerializeObject(new
            {
               experiences = new[] { 2 },
               sideProjects = new[] { 2 },
               educations = new[] { 2 }
            });
            result.Should().BeEquivalentTo(expected);
         }
      }

      [Fact]
      public async Task Update_when_there_is_already_a_developer_available()
      {
         var add = new
         {
            summary = "So Cool!",
            skills = "ES7, Node.js",
            experiences = new[]
            {
               new
               {
                  id = "a",
                  company = "Lodgify",
                  position = "C# Developer",
                  startDate = "2016-2-23",
                  endDate = "2017-1-2",
                  content = "as backend developer"
               }
            },
            sideProjects = new[]
            {
               new
               {
                  id = "a",
                  title = "Richtext Editor",
                  content = "A simple richtext for web"
               }
            },
            educations = new[]
            {
               new
               {
                  id = "a",
                  degree = "BS",
                  university = "S&C",
                  startDate = "2010-1-1",
                  endDate = "2011-1-1"
               }
            }
         };

         await _client.PutAsJsonAsync("/api/developer", add);

         var update = new
         {
            summary = "Not so cool",
            skills = "ES7, Node.js",
            experiences = new[]
            {
               new
               {
                  id = "1",
                  company = "Lodgify",
                  content = "as backend developer",
                  startDate = "2016-02-23",
                  endDate = "2017-01-02",
                  position = "C# Developer"
               }
            },
            sideProjects = new[]
            {
               new
               {
                  id = "1",
                  title = "Richtext Editor",
                  content = "A simple richtext for web"
               }
            },
            educations = new[]
            {
               new
               {
                  id = "1",
                  degree = "B.S.",
                  university = "Science and Culture",
                  startDate = "2009-01-01",
                  endDate = "2010-01-01"
               }
            }
         };

         using (var response = await _client.PutAsJsonAsync("/api/developer", update))
         {
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = JToken.Parse(await response.Content.ReadAsStringAsync());
            var expected = JsonConvert.SerializeObject(new
            {
               experiences = new[] { 1 },
               sideProjects = new[] { 1 },
               educations = new[] { 1 }
            });
            result.Should().BeEquivalentTo(expected);
         }

         using (var response = await _client.GetAsync("/api/developer"))
         {
            var result = JToken.Parse(await response.Content.ReadAsStringAsync());
            var expected = JsonConvert.SerializeObject(new
            {
               summary = "Not so cool",
               skills = "ES7, Node.js",
               experiences = new[]
               {
                  new
                  {
                     id = "1",
                     company ="Lodgify",
                     content ="as backend developer",
                     startDate ="2016-02-23",
                     endDate ="2017-01-02",
                     position ="C# Developer"
                  }
               },
               sideProjects = new[]
               {
                  new
                  {
                     id = "1",
                     title = "Richtext Editor",
                     content = "A simple richtext for web"
                  }
               },
               educations = new[]
               {
                  new
                  {
                     id = "1",
                     degree = "B.S.",
                     university = "Science and Culture",
                     startDate = "2009-01-01",
                     endDate = "2010-01-01"
                  }
               }
            });

            result.Should().BeEquivalentTo(expected);
         }
      }

      [Fact]
      public async Task Update_experiences()
      {
         var add = new
         {
            summary = "Not so cool",
            skills = "ES7, Node.js",
            experiences = new[]
            {
               new
               {
                  id = "a",
                  company = "Lodgify",
                  position = "C# Developer",
                  startDate = "2016-2-23",
                  endDate = "2017-1-2",
                  content = "as backend developer"
               },
               new
               {
                  id = "b",
                  company = "Parmis",
                  position = "Java Developer",
                  startDate = "2017-2-23",
                  endDate = "2018-1-2",
                  content = "as the team lead"
               }
            }
         };
         await _client.PutAsJsonAsync("/api/developer", add);

         var update = new
         {
            summary = "Not so cool",
            skills = "ES7, Node.js",
            experiences = new[]
            {
               new
               {
                  id = "2",
                  company = "Parmis",
                  content = "as the team lead",
                  startDate = "2014-01-01",
                  endDate = "2015-01-01",
                  position = "Not Java Developer"
               },
               new
               {
                  id = "ddd",
                  company = "Bellin",
                  content = "agile c# developer",
                  startDate = "2019-01-01",
                  endDate = "2022-01-01",
                  position = "Tester"
               }
            }
         };

         using (var response = await _client.PutAsJsonAsync("/api/developer", update))
         {
            var result = JToken.Parse(await response.Content.ReadAsStringAsync());
            var expected = JsonConvert.SerializeObject(new
            {
               experiences = new[] { 2, 3 },
               sideProjects = Enumerable.Empty<int>(),
               educations = Enumerable.Empty<int>()
            });
            result.Should().BeEquivalentTo(expected);
         }

         using (var response = await _client.GetAsync("/api/developer"))
         {
            var result = JToken.Parse(await response.Content.ReadAsStringAsync());
            var expected = JsonConvert.SerializeObject(new
            {
               summary = "Not so cool",
               skills = "ES7, Node.js",
               experiences = new[]
               {
                  new
                  {
                     id = "3",
                     company = "Bellin",
                     position = "Tester",
                     startDate = "2019-01-01",
                     endDate = "2022-01-01",
                     content = "agile c# developer"
                  },
                  new
                  {
                     id = "2",
                     company = "Parmis",
                     position = "Not Java Developer",
                     startDate = "2014-01-01",
                     endDate = "2015-01-01",
                     content = "as the team lead"
                  }
               },
               sideProjects = Enumerable.Empty<SideProjectEntry>(),
               educations = Enumerable.Empty<EducationEntry>()
            });
            result.Should().BeEquivalentTo(expected);
         }
      }

      [Fact]
      public async Task Update_sideProjects()
      {
         var add = new
         {
            summary = "Cool guy!",
            skills = "C#, SQL",
            sideProjects = new[]
            {
               new
               {
                  id = "a",
                  title = "Richtext Editor",
                  content = "A simple web richtext"
               },
               new
               {
                  id = "b",
                  title = "CodePrac",
                  content = "A simple app for practice coding"
               }
            }
         };

         await _client.PutAsJsonAsync("/api/developer", add);

         var update = new
         {
            summary = "Cool guy!",
            skills = "C#, SQL",
            sideProjects = new[]
            {
               new
               {
                  id = "2",
                  title = "CodePrac V2",
                  content = "other description"
               },
               new
               {
                  id = "a",
                  title = "Blog",
                  content = "A developers blog"
               }
            }
         };

         using (var response = await _client.PutAsJsonAsync("/api/developer", update))
         {
            var result = JToken.Parse(await response.Content.ReadAsStringAsync());
            var expected = JsonConvert.SerializeObject(new
            {
               experiences = Enumerable.Empty<int>(),
               sideProjects = new[] { 2, 3 },
               educations = Enumerable.Empty<int>()
            });
         }

         using (var response = await _client.GetAsync("/api/developer"))
         {
            var result = JToken.Parse(await response.Content.ReadAsStringAsync());
            var expected = JsonConvert.SerializeObject(new
            {
               summary = "Cool guy!",
               skills = "C#, SQL",
               experiences = Enumerable.Empty<ExperienceEntry>(),
               educations = Enumerable.Empty<EducationEntry>(),
               sideProjects = new[]
               {
                  new
                  {
                     id = "2",
                     title = "CodePrac V2",
                     content = "other description"
                  },
                  new
                  {
                     id = "3",
                     title = "Blog",
                     content = "A developers blog"
                  },
               }
            });
            result.Should().BeEquivalentTo(expected);
         }
      }

      [Fact]
      public async Task Update_educations()
      {
         var add = new
         {
            summary = "Cool guy!",
            skills = "C#, SQL",
            educations = new[]
            {
               new
               {
                  id = "a",
                  degree = "AS",
                  university = "College1",
                  startDate = "2010-1-1",
                  endDate = "2011-1-1"
               },
               new
               {
                  id = "b",
                  degree = "BS",
                  university = "S&C",
                  startDate = "2011-1-1",
                  endDate = "2012-1-1"
               }
            }
         };

         await _client.PutAsJsonAsync("/api/developer", add);

         var update = new
         {
            summary = "Cool guy!",
            skills = "C#, SQL",
            educations = new[]
            {
               new
               {
                  id = "2",
                  degree = "B.S.",
                  university = "Science & Culture",
                  startDate = "2015-01-01",
                  endDate = "2016-01-01"
               },
               new
               {
                  id = "a",
                  degree = "M.S.",
                  university = "Tehran",
                  startDate = "2017-01-01",
                  endDate = "2018-01-01"
               }
            }
         };

         using (var response = await _client.PutAsJsonAsync("/api/developer", update))
         {
            var result = JToken.Parse(await response.Content.ReadAsStringAsync());
            var expected = JsonConvert.SerializeObject(new
            {
               experiences = Enumerable.Empty<int>(),
               sideProjects = Enumerable.Empty<int>(),
               educations = new[] { 2, 3 }
            });
            result.Should().BeEquivalentTo(expected);
         }

         using (var response = await _client.GetAsync("/api/developer"))
         {
            var result = JToken.Parse(await response.Content.ReadAsStringAsync());
            var expected = JsonConvert.SerializeObject(new
            {
               summary = "Cool guy!",
               skills = "C#, SQL",
               experiences = Enumerable.Empty<ExperienceEntry>(),
               sideProjects = Enumerable.Empty<SideProjectEntry>(),
               educations = new[]
               {
                  new
                  {
                     id = "3",
                     degree = "M.S.",
                     university = "Tehran",
                     startDate = "2017-01-01",
                     endDate = "2018-01-01"
                  },
                  new
                  {
                     id = "2",
                     degree = "B.S.",
                     university = "Science & Culture",
                     startDate = "2015-01-01",
                     endDate = "2016-01-01"
                  }
               }
            });
            result.Should().BeEquivalentTo(expected);
         }
      }

      [Fact]
      public async Task Validate_input()
      {
         var developer = new
         {
            experiences = new[]
            {
               new { Id = "abc-123" }
            }
         };

         using (var response = await _client.PutAsJsonAsync("/api/developer", developer))
         {
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var result = JToken.Parse(await response.Content.ReadAsStringAsync());

            var dic = new Dictionary<string, object>
            {
               { "Skills", new[] {"'Skills' must not be empty."}},
               { "Summary", new[] {"'Summary' must not be empty."}},
               { "Experiences[0].Company", new[] {"'Company' must not be empty."}},
               { "Experiences[0].Content", new[] {"'Content' must not be empty."}},
               { "Experiences[0].EndDate", new[] {"Date string is invalid"}},
               { "Experiences[0].Position", new[] {"'Position' must not be empty."}},
               { "Experiences[0].StartDate", new[] {"Date string is invalid"}},
            };

            result.Should().BeEquivalentTo(JsonConvert.SerializeObject(dic));
         }
      }
   }
}
