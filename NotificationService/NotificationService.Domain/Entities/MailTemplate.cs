namespace NotificationService.Domain.Entities;

public class MailTemplate
{
    public required int Id { get; init; }
    public required string Name { get; set; }
    public required string Template { get; set; }
}