using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VoteSystem.Models
{
    public class Suggestion
    {
        public int SuggestionId { get; set; }
        public int TopicId { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
        public DateTime SuggestionCreated { get; set; }
        public bool AllowMultipleTopics { get; set; }

        public virtual Topic Topic { get; set; }
        public virtual List<Vote> Votes { get; set; }
        public virtual List<Comment> Comments { get; set; }
    }
}