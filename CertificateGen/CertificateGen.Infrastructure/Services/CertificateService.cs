using CertMailer.CertificateGen.Application.Interfaces;
using CertMailer.CertificateGen.Application.Models;
using CertMailer.Shared.Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace CertMailer.CertificateGen.Infrastructure.Services;

public class CertificateService : ICertificateService
{
    public void GeneratePdf(Participant participant, Stream stream) => 
        GeneratePdf(participant, stream, CertificateOptions.Default);

    public void GeneratePdf(Participant participant, Stream stream, CertificateOptions options)
    {
        // TODO: Use options.BackgroundDocument
        var document = CreateQuestPdfDocument(participant, options);
        document.GeneratePdf(stream);
    }

    private Document CreateQuestPdfDocument(Participant participant, CertificateOptions options)
    {
        // TODO: Add templated layouts etc.
        return Document.Create(d =>
        {
            d.Page(p =>
            {
                p.Size(PageSizes.A5.Landscape());
                p.Content()
                    .AlignCenter()
                    .AlignMiddle()
                    .Column(c =>
                    {
                        c
                            .Item()
                            .AlignTop()
                            .Text(options.Title)
                            .AlignCenter()
                            .FontSize(18f);
                        c
                            .Item()
                            .Text(options.Subtitle)
                            .AlignCenter();
                        c
                            .Item()
                            .Text(participant.FirstName + ' ' + participant.LastName)
                            .AlignCenter()
                            .FontSize(20f);
                        c
                            .Item()
                            .Text(
                                string.Format(
                                    options.DescriptionFormat, 
                                    participant.CourseName,
                                    participant.CompletionDate.ToShortDateString()))
                            .AlignCenter();
                    });
            });
        });
    }
}