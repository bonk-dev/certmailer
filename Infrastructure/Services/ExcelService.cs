using System.Globalization;
using CertMailer.Application.Interfaces;
using CertMailer.Application.Models;
using CertMailer.Domain.Entities;
using OfficeOpenXml;

namespace CertMailer.Infrastructure.Services;

public class ExcelService : IExcelService
{
    private static readonly string[] ValidHeaderValues = ["Imię", "Nazwisko", "Email", "Kurs", "Data ukonczenia"];

    public const string DefaultSheetName = "Sheet1";
    
    public async Task<IResult<IEnumerable<Participant>>> ParseAsync(Stream stream) =>
        await ParseAsync(stream, DefaultSheetName);

    public async Task<IResult<IEnumerable<Participant>>> ParseAsync(Stream stream, string sheetName)
    {
        using var package = new ExcelPackage();
        await package
            .LoadAsync(stream)
            .ConfigureAwait(false);

        return Parse(package, sheetName);
    }

    private IResult<IEnumerable<Participant>> Parse(ExcelPackage package, string sheetName)
    {
        var ws = package.Workbook.Worksheets.SingleOrDefault(w => w.Name == sheetName);
        if (ws == null)
        {
            return Result<IEnumerable<Participant>>.Fail([$"Sheet {sheetName} not found"]);
        }

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
                return Result<IEnumerable<Participant>>.Fail([$"Invalid row in sheet {sheetName} (i: {i})"]);
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
        if (ws.Dimension.End.Column < ValidHeaderValues.Length)
        {
            return Result.Fail([$"Not enough columns in {ws.Name}"]);
        }

        if (ws.Dimension.End.Row < 1)
        {
            return Result.Fail([$"Sheet {ws.Name} is empty"]);
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