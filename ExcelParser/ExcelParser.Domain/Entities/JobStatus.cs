namespace ExcelParser.Domain.Entities;

public class JobStatus
{
    private int _participantsParsed;
    private int _certificatesGenerated;
    private int _mailsSent;
    public string ParserState { get; set; } = "uploaded";

    public int ParticipantsParsed
    {
        get => _participantsParsed;
        set => _participantsParsed = value;
    }

    public int CertificatesGenerated
    {
        get => _certificatesGenerated;
        set => _certificatesGenerated = value;
    }

    public int MailsSent
    {
        get => _mailsSent;
        set => _mailsSent = value;
    }

    /// <summary>
    /// Atomically increment one of the status properties
    /// </summary>
    /// <param name="prop"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void Increment(Prop prop)
    {
        switch (prop)
        {
            case Prop.Parsed:
                Interlocked.Increment(ref _participantsParsed);
                break;
            case Prop.Certs:
                Interlocked.Increment(ref _certificatesGenerated);
                break;
            case Prop.Emails:
                Interlocked.Increment(ref _mailsSent);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(prop), prop, null);
        }
    }

    public enum Prop
    {
        Parsed,
        Certs,
        Emails
    }
}