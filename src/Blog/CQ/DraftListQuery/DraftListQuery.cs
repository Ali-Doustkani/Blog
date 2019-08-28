using MediatR;
using System.Collections.Generic;

namespace Blog.CQ.DraftListQuery
{
   public class DraftListQuery : IRequest<IEnumerable<DraftItem>>
   { }
}
