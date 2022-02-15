using System.Text;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace CompanyEmployees.Extensions;

public class CsvOutputFormatter:TextOutputFormatter
{
    public CsvOutputFormatter()
    {
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);
    }

    protected override bool CanWriteType(Type? type)
    {
        return typeof(IEnumerable<CompanyDto>).IsAssignableFrom(type) && base.CanWriteType(type);
    }

    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
    {
        var response = context.HttpContext.Response;
        var buffer = new StringBuilder();
        if (context.Object is IEnumerable<CompanyDto> companies)
        {
            foreach (var company in companies)
            {
                FormatCsv(buffer,company);
            }
        }
        else
        {
            FormatCsv(buffer,(CompanyDto)context.Object!);
        }

        await response.WriteAsync(buffer.ToString());
    }

    private void FormatCsv(StringBuilder buffer, CompanyDto? companyDto)
    {
        buffer.AppendLine($"{companyDto!.Id},\"{companyDto.Name}\"{companyDto.FullAddress}\"");
    }
}