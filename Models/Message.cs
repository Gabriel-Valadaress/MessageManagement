using System;

namespace MessageManagement.Models;
public class Message
{
    public int Id { get; set; }
    public string? Email { get; set; }
    public string? Text { get; set; }
    public DateTime Date { get; set; }
}