using System.Globalization;
using CertMailer.ExcelParser.Application.Interfaces;
using CertMailer.ExcelParser.Application.Models;
using CertMailer.Shared.Domain.Entities;
using CommunityToolkit.HighPerformance;
using OfficeOpenXml;

namespace CertMailer.ExcelParser.Infrastructure.Services;

public class ExcelService : IExcelService
{
    private static readonly string[] ValidHeaderValues = ["Imię", "Nazwisko", "Email", "Kurs", "Data ukonczenia"];

    public const string DefaultSheetName = "Sheet1";
    
    public async Task<IResult<IEnumerable<Participant>>> ParseAsync(Stream stream) =>
        await ParseAsync(stream, DefaultSheetName);

    public async Task<IResult<IEnumerable<Participant>>> ParseAsync(Stream stream, string sheetName)
    {
        using var package = new ExcelPackage();

        try
        {
            await package
                .LoadAsync(stream)
                .ConfigureAwait(false);
        }
        catch (InvalidDataException ex)
        {
            return Result<IEnumerable<Participant>>.Fail([$"Invalid XLSX file: {ex.Message}"]);
        }

        return Parse(package, sheetName);
    }

    public IResult<IEnumerable<Participant>> Parse(Memory<byte> buffer)
    {
        using var package = new ExcelPackage();

        try
        {
            package.Load(buffer.AsStream());
        }
        catch (InvalidDataException ex)
        {
            return Result<IEnumerable<Participant>>.Fail([$"Invalid XLSX file: {ex.Message}"]);
        }

        return Parse(package);
    }

    public IResult<IEnumerable<Participant>> Parse(Memory<byte> buffer, string sheetName)
    {
        using var package = new ExcelPackage();
        try
        {
            package.Load(buffer.AsStream());
        }
        catch (InvalidDataException ex)
        {
            return Result<IEnumerable<Participant>>.Fail([$"Invalid XLSX file: {ex.Message}"]);
        }
        return Parse(package, sheetName);
    }

    private IResult<IEnumerable<Participant>> Parse(ExcelPackage package, string sheetName)
    {
        var ws = package.Workbook.Worksheets.SingleOrDefault(w => w.Name == sheetName);
        if (ws == null)
        {
            return Result<IEnumerable<Participant>>.Fail([$"Sheet {sheetName} not found"]);
        }

        return Parse(ws);
    }
    
    private IResult<IEnumerable<Participant>> Parse(ExcelPackage package)
    {
        var ws = package.Workbook.Worksheets.FirstOrDefault();
        if (ws == null)
        {
            return Result<IEnumerable<Participant>>.Fail(["Not sheets not found"]);
        }

        return Parse(ws);
    }

    private IResult<IEnumerable<Participant>> Parse(ExcelWorksheet ws)
    {
        var r = ValidateWorksheet(ws);
        if (!r.Success)
        {
            return Result<IEnumerable<Participant>>.Fail(r.Errors);
        }

        var rows = ws.Dimension.End.Row;
        var participants = new List<Participant>();
        // skip header
        for (var i = 2; i <= rows; ++i)
        {
            var cells = ws.Cells[i, 1, i, ValidHeaderValues.Length];
            if (cells == null)
            {
                return Result<IEnumerable<Participant>>.Fail([$"Invalid row in sheet {ws.Name} (i: {i})"]);
            }

            var name = cells.GetCellValue<string>();
            var lastName = cells.GetCellValue<string>(columnOffset: 1);
            var email = cells.GetCellValue<string>(columnOffset: 2);
            var course = cells.GetCellValue<string>(columnOffset: 3);
            var completionDateRaw = cells.GetCellValue<string>(columnOffset: 4);

            const string dateFormat = "yyyy-MM-dd";
            if (!DateTime.TryParseExact(
                    completionDateRaw, 
                    dateFormat, 
                    CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var completionDate))
            {
                return Result<IEnumerable<Participant>>.Fail([
                    $"Invalid date format in row {i}. Valid format: {dateFormat}"]);
            }

            participants.Add(new Participant(name, lastName, email, course, completionDate));
        }

        return Result<IEnumerable<Participant>>.Ok(participants);
    }

    private Result ValidateWorksheet(ExcelWorksheet ws)
    {
        if (ws.Dimension == null || ws.Dimension.End.Row < 1)
        {
            return Result.Fail([$"Sheet {ws.Name} is empty"]);
        }
        
        if (ws.Dimension.End.Column < ValidHeaderValues.Length)
        {
            return Result.Fail([$"Not enough columns in {ws.Name}"]);
        }

        for (var i = 1; i <= ValidHeaderValues.Length; ++i)
        {
            var hCell = ws.Cells[1, i];
            if (hCell == null)
            {
                return Result.Fail([$"Could not read header cell from {ws.Name} (col: {i})"]);
            }

            var cellValue = hCell.GetValue<string>();
            var validValue = ValidHeaderValues[i - 1];
            if (!validValue.Equals(cellValue, StringComparison.InvariantCultureIgnoreCase))
            {
                return Result.Fail([$"Invalid header value in {ws.Name} (expected: {validValue}, was: {cellValue})"]);
            }
        }
        
        return Result.Ok();
    }
}