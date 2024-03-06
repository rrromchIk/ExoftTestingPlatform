namespace TestingApi.Models;

public class User : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string ProfilePictureFilePath { get; set; } = null!;
    public UserRole UserRole { get; set; }
    
    public ICollection<UserTest> UserTests { get; set; } = null!;
}