using AspireApp.AppHost;
using AspireApp.AppHost.OpenTelemetryCollector;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache")
    .WithRedisInsight(c => c.PublishAsContainer());


var prometheus = builder.AddContainer("prometheus", "prom/prometheus")
    .WithBindMount("../AspireApp.ApiService/wwwroot/Files/prometheus", "/etc/prometheus", isReadOnly: true)
    .WithArgs("--web.enable-otlp-receiver", "--config.file=/etc/prometheus/prometheus.yml")
    .WithHttpEndpoint(targetPort: 9090, name: "http");

var grafana = builder.AddContainer("grafana", "grafana/grafana")
    .WithBindMount("../AspireApp.ApiService/wwwroot/Files/grafana/config", "/etc/grafana", isReadOnly: true)
    .WithBindMount("../AspireApp.ApiService/wwwroot/Files/grafana/dashboards", "/var/lib/grafana/dashboards", isReadOnly: true)
    .WithEnvironment("PROMETHEUS_ENDPOINT", prometheus.GetEndpoint("http"))
    .WithHttpEndpoint(targetPort: 3000, name: "http");

builder.AddOpenTelemetryCollector("otelcollector", "../AspireApp.ApiService/wwwroot/Files/otelcollector/config.yaml")
    .WithEnvironment("PROMETHEUS_ENDPOINT", $"{prometheus.GetEndpoint("http")}/api/v1/otlp");


var apiService = builder.AddProject<Projects.AspireApp_ApiService>("apiservice")
    .WithScalar()
    .WithSwagger()
    .WaitFor(cache)
    .WithReference(cache);

builder.AddProject<Projects.AspireApp_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);


builder.Build().Run();