namespace JaxDug.Models
{
    public class UserState
    {
        public string FirstName = string.Empty;
        public string LastName = string.Empty;
        public string Email = string.Empty;
        public string OpenId = string.Empty;
 
        /// <summary>
        /// Exports a short string list of Id, Email, Name separated by |
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Join("|", new[] { OpenId, FirstName, LastName, Email});
        }
 
        /// <summary>
        /// Imports Id, Email and Name from a | separated string
        /// </summary>
        /// <param name="itemString"></param>
        public bool FromString(string itemString)
        {
            if (string.IsNullOrEmpty(itemString))
                return false;
 
            var strings = itemString.Split('|');
            if (strings.Length < 3)
                return false;
 
            OpenId = strings[0];
            FirstName = strings[1];
            LastName = strings[2];
            Email = strings[3];
 
            return true;
        }
    }
}
