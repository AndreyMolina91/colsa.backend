namespace COLSA.Infraestructure.Dtos
{
    public class UserRegisterDto
    {
        public string UserName { get; set; }
        public string EmailAddress { get; set; }

        public string UserCountry { get; set; }

        public string UserDateOfBirth { get; set; }

        public string InGameName { get; set; }

        public string Password { get; set; }

    }
}