using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Orchard.Park.SqlSugar;

public static class ServiceCollectionExtension
{
    public static void AddSqlSugar(this IServiceCollection services, List<SqlSugarConnection> connectionList)
    {
        if (connectionList == null || connectionList.Count == 0)
            throw new ArgumentNullException($"请添加数据库连接配置:{nameof(List<SqlSugarConnection>)}");

        Console.WriteLine($"数据库连接字符串：{JsonConvert.SerializeObject(connectionList)}");

        //Create ext method
        var expMethods = new List<SqlFuncExternal>
        {
            new()
            {
                UniqueMethodName = "GetDistance",
                MethodValue = (expInfo, dbType, _) =>
                {
                    if (dbType == DbType.SqlServer)
                        return $"dbo.fnGetDistance({expInfo.Args[0].MemberName},{expInfo.Args[1].MemberName},{expInfo.Args[2].MemberName},{expInfo.Args[3].MemberName})";

                    throw new Exception("未实现");
                }
            }
        };

        var dbConfigList = new List<ConnectionConfig>();
        foreach (var config in connectionList.Select(connection => new ConnectionConfig
        {
            ConfigId = connection.ConfigId,
            DbType = connection.DbType,
            ConnectionString = connection.Connection,
            IsAutoCloseConnection = true
        }))
        {
            if (config.DbType == DbType.SqlServer)
            {
                config.ConfigureExternalServices = new ConfigureExternalServices
                {
                    SqlFuncServices = expMethods //set ext method
                };
            }

            dbConfigList.Add(config);
        }

        //非单例用ISqlSugarClient
        //单例用SqlSugarScope
        //https://www.donet5.com/Home/Doc?typeId=1181
        services.AddSingleton<ISqlSugarClient>(s =>
        {
            var logger = s.GetRequiredService<ILogger<ISqlSugarClient>>();
            var hostEnvironment = s.GetRequiredService<IHostEnvironment>();
            var sqlSugar = new SqlSugarScope(dbConfigList,
                db =>
                {
                    db.Aop.OnLogExecuted = (sql, pars) =>
                    {
                        if (hostEnvironment.IsProduction())
                        {
                            //获取原生SQL推荐 5.1.4.63  性能OK[官方推荐]
                            var nativeSql = UtilMethods.GetNativeSql(sql, pars);
                            logger.LogDebug($"[{nativeSql}]:[{db.Ado.SqlExecutionTime}]");
                        }
                        else
                        {
                            //获取无参数化SQL 影响性能只适合调试
                            var sqlString = UtilMethods.GetSqlString(db.CurrentConnectionConfig.DbType, sql, pars);
                            logger.LogInformation($"[SQL]:[{sqlString}]:[{db.Ado.SqlExecutionTime}]");
                        }
                    };
                    db.Aop.OnError = exp =>
                    {
                        logger.LogError($"{exp.Message} {exp.Source} {exp.Sql}");
                    };
                });

            return sqlSugar;
        });
    }
}