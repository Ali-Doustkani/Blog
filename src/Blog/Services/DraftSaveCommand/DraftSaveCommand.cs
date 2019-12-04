﻿using MediatR;

namespace Blog.Services.DraftSaveCommand
{
   public class DraftSaveCommand : IRequest<Result>
   {
      public int Id { get; set; }
      public string Title { get; set; }
      public string Content { get; set; }
   }
}
