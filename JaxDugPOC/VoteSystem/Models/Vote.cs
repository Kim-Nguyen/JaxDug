using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace VoteSystem.Models
{
    public class Vote
    {
        public int VoteId { get; set; }
        public int UserId { get; set; }
        public int SuggestionId { get; set; }
        public Descision Descision { get; set; }
        public DateTime VoteCreated { get; set; }
     
    }
    public enum Descision
    {
        Plus = 1,
        Negative = 0
    }
}