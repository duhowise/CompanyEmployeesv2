using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace CompanyEmployees.Extensions;

public class ConfigureSwaggerOptions:IConfigureNamedOptions<SwaggerOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }
    public void Configure(SwaggerOptions options)
    {
       
    }

    public void Configure(string name, SwaggerOptions options)
    {
        
    }
}