namespace api.DTOs;
public class GetProfileDto
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string[] roles { get; set; }
    public byte[]? Avatar { get; set; }
}