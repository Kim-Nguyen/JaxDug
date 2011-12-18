using System.Data.Entity;
using System.Linq;
using System.Web;

namespace VoteSystem.Models
{
    public class VoteService
    {
        private const string _connectionString = @"\\localhost\";   
        private DbContext VoteSystem = new DbContext(_connectionString);
        

    }


    
}