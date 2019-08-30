using MediatR;
using System.Collections.Generic;

namespace Blog.Services.DraftListQuery
{
   public class DraftListQuery : IRequest<IEnumerable<DraftItem>>
   { }
}
