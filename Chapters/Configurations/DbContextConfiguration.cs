﻿using Chapters;
using Chapters.Enums;
using Microsoft.EntityFrameworkCore;
using Npgsql;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class DbContextConfiguration
{
    private static readonly ICollection<string> RetriedPostgresExceptions = new[]
    {
        PostgresErrorCodes.ConnectionException,
        PostgresErrorCodes.ConnectionFailure
    };

    public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Postgres");
        var maxDelay = TimeSpan.FromSeconds(1);
        
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        dataSourceBuilder.MapEnums();
        var dataSource = dataSourceBuilder.Build();
        
        services.AddDbContext<ApplicationDbContext>((options) =>
        {
            options.UseNpgsql(dataSource,
                npgsqlOptions =>
                {
                    npgsqlOptions.EnableRetryOnFailure(2, maxDelay, RetriedPostgresExceptions);
                    npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                });
        });

        return services;
    }

    private static NpgsqlDataSourceBuilder MapEnums(this NpgsqlDataSourceBuilder dataSourceBuilder)
    {
        dataSourceBuilder.MapEnum<BookStatus>();
        return dataSourceBuilder;
    }
}