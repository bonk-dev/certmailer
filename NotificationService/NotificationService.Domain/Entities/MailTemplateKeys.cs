namespace NotificationService.Domain.Entities;

public static class MailTemplateKeys
{
    public const string FirstName = "{FirstName}";
    public const string CourseName = "{CourseName}";

    public static IReadOnlyCollection<string> AllKeys { get; } =
    [
        FirstName,
        CourseName
    ];
}