using System.Diagnostics;
using CertMailer.ExcelParser.Application.Interfaces;
using CertMailer.Shared.Domain.Entities;

namespace CertMailer.ExcelParser.Application.Tests.Services;

public class ExcelServiceTests
{
    [Test]
    public async Task TestParseValid()
    {
        var testFilePath = Path.Combine(Directory.GetCurrentDirectory(), "TestFiles", "participants_valid.xlsx");
        Debug.WriteLine("TestParseValid: {0}", testFilePath);

        await using var stream = File.OpenRead(testFilePath);
        var service = Testing.GetRequiredService<IExcelService>();
        var result = await service.ParseAsync(stream, "Participants");
        
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.True, "Parsing was not successful");
            Assert.That(result.Errors, Is.Empty, "Parsing ended with errors: " + string.Join(';', result.Errors));
        });
        
        var expected = new Participant[]
        {
            new("Aleksander", "Pietrzak", "aleksander.pietrzak@poczta.fm",
                "Programowanie Java", new DateTime(2024, 10, 8)),
            new("Robert", "Wiśniewski", "robert518@gmail.com",
                "Administracja Biurowa", new DateTime(2025, 4, 4)),
            new("Joanna", "Jabłoński", "joannajablonski@poczta.fm",
                "Prezentacje Multimedialne", new DateTime(2025, 1, 21)),
            new("Dorota", "Wojciechowski", "dorota.wojciechowski@onet.pl",
                "Logistyka i Transport", new DateTime(2025, 1, 19)),
            new("Aleksandra", "Tomaszewski", "aleksandra81@onet.pl",
                "Język Hiszpański - Konwersacje", new DateTime(2024, 8, 27)),
            new("Renata", "Jankowski", "r.jankowski@gmail.com",
                "Logistyka i Transport", new DateTime(2025, 3, 17)),
            new("Paweł", "Krawczyk", "p.krawczyk@onet.pl",
                "Język Angielski - Poziom B1", new DateTime(2024, 8, 5)),
            new("Weronika", "Baran", "weronika176@poczta.fm", "Prawo Pracy",
                new DateTime(2025, 4, 22)),
            new("Małgorzata", "Krawczyk", "m.krawczyk@gmail.com", "Prawo Pracy",
                new DateTime(2025, 6, 28)),
            new("Małgorzata", "Krawczyk", "malgorzatakrawczyk@poczta.fm",
                "Informatyka dla Seniorów", new DateTime(2024, 12, 24))
        };
        
        Assert.That(result.Data, Is.EquivalentTo(expected));
    }
    
    [Test]
    public async Task TestParseInvalidHeader()
    {
        var testFilePath = Path.Combine(Directory.GetCurrentDirectory(), "TestFiles", "participants_invalid_header.xlsx");
        Debug.WriteLine("TestParseInvalidHeader: {0}", testFilePath);

        await using var stream = File.OpenRead(testFilePath);
        var service = Testing.GetRequiredService<IExcelService>();
        var result = await service.ParseAsync(stream, "Participants");
        
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False, "Parsing was successful");
            Assert.That(result.Errors, Is.Not.Empty, "Parsing ended without errors");
        });
        
        Assert.That(result.Errors, 
            Has.Some.EqualTo("Invalid header value in Participants (expected: Imię, was: Nazwisko)"));
    }
    
    [Test]
    public async Task TestParseEmpty()
    {
        var testFilePath = Path.Combine(Directory.GetCurrentDirectory(), "TestFiles", "participants_empty.xlsx");
        Debug.WriteLine("TestEmpty: {0}", testFilePath);

        await using var stream = File.OpenRead(testFilePath);
        var service = Testing.GetRequiredService<IExcelService>();
        var result = await service.ParseAsync(stream, "Sheet1");
        
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False, "Parsing was successful");
            Assert.That(result.Errors, Is.Not.Empty, "Parsing ended without errors");
        });
        
        Assert.That(result.Errors, 
            Has.Some.EqualTo("Sheet Sheet1 is empty"));
    }
    
    [Test]
    public async Task TestParseInvalidColumns()
    {
        var testFilePath = Path.Combine(Directory.GetCurrentDirectory(), "TestFiles", "participants_invalid_columns.xlsx");
        Debug.WriteLine("TestParseInvalidColumns: {0}", testFilePath);

        await using var stream = File.OpenRead(testFilePath);
        var service = Testing.GetRequiredService<IExcelService>();
        var result = await service.ParseAsync(stream, "Participants");
        
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False, "Parsing was successful");
            Assert.That(result.Errors, Is.Not.Empty, "Parsing ended without errors");
        });
        
        Assert.That(result.Errors, 
            Has.Some.EqualTo("Not enough columns in Participants"));
    }
}