using System.ComponentModel.DataAnnotations;

namespace UserApi;

public class User
{
    public string Id { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 4)]
    public string Name { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 4)]
    public string Surname { get; set; }

    [Required]
    [Range(12, 99)]
    public int Age { get; set; }

    public User(string name, string surname, int age)
    {
        Name = name;
        Surname = surname;
        Age = age;
    }
}
