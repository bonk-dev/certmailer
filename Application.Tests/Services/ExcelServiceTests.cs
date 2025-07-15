using System.Diagnostics;
using CertMailer.Application.Interfaces;
using CertMailer.Domain.Entities;

namespace CertMailer.Application.Tests.Services;

public class ExcelServiceTests
{
    [Test]
    public async Task TestParseValid()
    {
        var testFilePath = Path.Combine(Directory.GetCurrentDirectory(), "TestFiles", "participants_valid.xlsx");
        Debug.WriteLine("TestParseValid: {0}", args: testFilePath);

        await using var stream = File.OpenRead(testFilePath);
        var service = Testing.GetRequiredService<IExcelService>();
        var result = await service.ParseAsync(stream, sheetName: "Participants");
        
        Assert.Multiple(() =>
        {
            Assert.IsTrue(result.Success, message: "Parsing was not successful");
            Assert.IsEmpty(result.Errors, message: "Parsing ended with errors: " + string.Join(';', result.Errors));
        });
        
        var expected = new Participant[]
        {
            new(FirstName: "Aleksander", LastName: "Pietrzak", Email: "aleksander.pietrzak@poczta.fm",
                CourseName: "Programowanie Java", CompletionDate: new DateTime(2024, 10, 8)),
            new(FirstName: "Robert", LastName: "Wiśniewski", Email: "robert518@gmail.com",
                CourseName: "Administracja Biurowa", CompletionDate: new DateTime(2025, 4, 4)),
            new(FirstName: "Joanna", LastName: "Jabłoński", Email: "joannajablonski@poczta.fm",
                CourseName: "Prezentacje Multimedialne", CompletionDate: new DateTime(2025, 1, 21)),
            new(FirstName: "Dorota", LastName: "Wojciechowski", Email: "dorota.wojciechowski@onet.pl",
                CourseName: "Logistyka i Transport", CompletionDate: new DateTime(2025, 1, 19)),
            new(FirstName: "Aleksandra", LastName: "Tomaszewski", Email: "aleksandra81@onet.pl",
                CourseName: "Język Hiszpański - Konwersacje", CompletionDate: new DateTime(2024, 8, 27)),
            new(FirstName: "Renata", LastName: "Jankowski", Email: "r.jankowski@gmail.com",
                CourseName: "Logistyka i Transport", CompletionDate: new DateTime(2025, 3, 17)),
            new(FirstName: "Paweł", LastName: "Krawczyk", Email: "p.krawczyk@onet.pl",
                CourseName: "Język Angielski - Poziom B1", CompletionDate: new DateTime(2024, 8, 5)),
            new(FirstName: "Weronika", LastName: "Baran", Email: "weronika176@poczta.fm", CourseName: "Prawo Pracy",
                CompletionDate: new DateTime(2025, 4, 22)),
            new(FirstName: "Małgorzata", LastName: "Krawczyk", Email: "m.krawczyk@gmail.com", CourseName: "Prawo Pracy",
                CompletionDate: new DateTime(2025, 6, 28)),
            new(FirstName: "Małgorzata", LastName: "Krawczyk", Email: "malgorzatakrawczyk@poczta.fm",
                CourseName: "Informatyka dla Seniorów", CompletionDate: new DateTime(2024, 12, 24))
        };
        
        Assert.That(result.Data, Is.EquivalentTo(expected));
    }
}