namespace PSAM.Exceptions
{
    public class AccountExceptions : Exception
    {
        public AccountExceptions(string message) : base(message) { }
    }

    public class LoginTakenException : AccountExceptions
    {
        public LoginTakenException() : base("This login is already taken.") { }
    }
    public class UsernameTakenException : AccountExceptions
    {
        public UsernameTakenException() : base("This username is already taken.") { }
    }

    public class AccountDoesntExistException : AccountExceptions
    {
        public AccountDoesntExistException() : base("This account doesn't exist.") { }
    }

    public class AccountHasTechException : AccountExceptions
    {
        public AccountHasTechException() : base("This account already has that technology.") { }
    }
    
}
