using Microsoft.Extensions.Diagnostics.HealthChecks;

using System.Diagnostics;

namespace AspireApp.AppHost
{
    internal static class ResourceBuilderExtensions
    {
        internal static IResourceBuilder<T> WithScalar<T>(this IResourceBuilder<T> builder)
            where T : IResourceWithEndpoints

        {
           return builder.WithOpenApiDocs("scalar-docs", "Scalar Docs", "scalar/v1");
        }  
        
        internal static IResourceBuilder<T> WithSwagger<T>(this IResourceBuilder<T> builder)
            where T : IResourceWithEndpoints

        {
           return builder.WithOpenApiDocs("swagger-docs", "Swagger Docs", "swagger");
        }
        private static IResourceBuilder<T> WithOpenApiDocs<T>(this IResourceBuilder<T> builder
            , string name, string displayName, string openApiPath)
        where T : IResourceWithEndpoints
        {
            return builder.WithCommand(name, displayName
                , executeCommand: async _ =>
                {
                    try
                    {
                        var endpoint = builder.GetEndpoint("https");
                        var url = $"{endpoint.Url}/{openApiPath}";
                        Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                        return new ExecuteCommandResult { Success = true };

                    }
                    catch (Exception e)
                    {
                        return new ExecuteCommandResult { Success = true, ErrorMessage = e.ToString() };
                    }
                }, con => con.ResourceSnapshot.HealthStatus ==
                       HealthStatus.Healthy ? ResourceCommandState.Enabled
                    : ResourceCommandState.Disabled,
                iconName: "Document",
                iconVariant: IconVariant.Filled

                );
        }
    }
}
