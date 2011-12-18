using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VoteSystem.Models
{
    public class Comment
    {
        public int CommentId {get;set;}
        public int UserId {get;set;}
        public int SuggestionId { get; set; }
        public string Content { get; set; }
        public bool Flagged { get; set; }
        public DateTime CommentCreated  {get;set;}

        public virtual Comment ParentComment { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual Suggestion Suggestion { get; set; }

    }
}