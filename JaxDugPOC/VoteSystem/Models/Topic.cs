using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VoteSystem.Models;

namespace VoteSystem.Models
{
    public class Topic
    {
        public int TopicId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public DateTime TopicCreated { get; set; }
        public ResponseFormat Type { get; set; }

        public virtual List<Suggestion> Suggestions { get; set; }
        public virtual List<Comment> Comments { get; set; }
    }

    public enum ResponseFormat
    {
        Date = 0,
        Hierarchtical = 1
    }
}