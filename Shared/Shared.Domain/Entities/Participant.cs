namespace CertMailer.Shared.Domain.Entities;

public record Participant(string FirstName, string LastName, string Email, string CourseName, DateTime CompletionDate);