using System.ComponentModel.DataAnnotations;

namespace LiveGroupChat.Models.Entities;

public class User
{
    public int Id { get; set; }
    
    [StringLength(15)]
    public string? Nickname { get; set; } }