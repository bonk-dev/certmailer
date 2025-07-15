namespace CertMailer.Shared.Application.Dto;

public class ParticipantDto
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string CourseName { get; init; }
    public required DateTime CompletionDate { get; init; }
}
