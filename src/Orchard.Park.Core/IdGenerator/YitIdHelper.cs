/*
 * 版权属于：yitter(yitter@126.com)
 * 开源地址：https://github.com/yitter/idgenerator
 * 版权协议：MIT
 * 版权说明：只要保留本版权，你可以免费使用、修改、分发本代码。
 * 免责条款：任何因为本代码产生的系统、法律、政治、宗教问题，均与版权所有者无关。
 *
 */

using Orchard.Park.Core.IdGenerator.Contract;
using System;
using System.Runtime.InteropServices;

namespace Orchard.Park.Core.IdGenerator;

public class YitIdHelper
{
    private static IIdGenerator _IdGenInstance;

    //定义dll路径
    public const string RegWorkerId_Windows = "IdGenerator//lib//workeridgo-windows.dll";

    //定义dll路径
    public const string RegWorkerId_Linux = "IdGenerator//lib//workeridgo-linux.dll";

    /// <summary>
    /// 机器码
    /// </summary>
    public static ushort WorkerId = 1;

    /// <summary>
    /// 注册一个 WorkerId，会先注销所有本机已注册的记录
    /// </summary>
    /// <param name="server">redis 服务器地址</param>
    /// <param name="password">redis 访问密码，可为空字符串</param>
    /// <param name="db">Redis指定存储库，示例：1</param>
    /// <param name="sentinelMasterName">Redis 哨兵模式下的服务名称，示例：mymaster，非哨兵模式传入空字符串即可</param>
    /// <param name="minWorkerId">WorkerId 最小值，示例：30</param>
    /// <param name="maxWorkerId">WorkerId 最大值，示例：63</param>
    /// <param name="lifeTimeSeconds">WorkerId缓存时长（秒，3的倍数），推荐值15</param>
    /// <returns></returns>
    [DllImport(RegWorkerId_Windows, EntryPoint = "RegisterOne", CallingConvention = CallingConvention.Cdecl, ExactSpelling = false)]
    private static extern ushort RegisterOne_Win(string server, string password, int db, string sentinelMasterName, int minWorkerId, int maxWorkerId, int lifeTimeSeconds);

    /// <summary>
    /// 注销本机已注册的 WorkerId
    /// </summary>
    [DllImport(RegWorkerId_Windows, EntryPoint = "UnRegister", CallingConvention = CallingConvention.Cdecl, ExactSpelling = false)]
    private static extern void UnRegister_Win();

    /// <summary>
    /// 注册一个 WorkerId，会先注销所有本机已注册的记录
    /// </summary>
    /// <param name="server">redis 服务器地址</param>
    /// <param name="password">redis 访问密码，可为空字符串</param>
    /// <param name="db">Redis指定存储库，示例：1</param>
    /// <param name="sentinelMasterName">Redis 哨兵模式下的服务名称，示例：mymaster，非哨兵模式传入空字符串即可</param>
    /// <param name="minWorkerId">WorkerId 最小值，示例：30</param>
    /// <param name="maxWorkerId">WorkerId 最大值，示例：63</param>
    /// <param name="lifeTimeSeconds">WorkerId缓存时长（秒，3的倍数），推荐值15</param>
    /// <returns></returns>
    [DllImport(RegWorkerId_Linux, EntryPoint = "RegisterOne", CallingConvention = CallingConvention.Cdecl, ExactSpelling = false)]
    private static extern ushort RegisterOne_Linux(string server, string password, int db, string sentinelMasterName, int minWorkerId, int maxWorkerId, int lifeTimeSeconds);

    /// <summary>
    /// 注销本机已注册的 WorkerId
    /// </summary>
    [DllImport(RegWorkerId_Linux, EntryPoint = "UnRegister", CallingConvention = CallingConvention.Cdecl, ExactSpelling = false)]
    private static extern void UnRegister_Linux();

    /// <summary>
    /// 初始化
    /// </summary>
    public static void Initialize(string redisConnectionString)
    {
        if (string.IsNullOrEmpty(redisConnectionString))
            throw new ArgumentException("redisConnectionString不能为空");

        var connectionParams = redisConnectionString.Split(",");
        var server = connectionParams[0];
        var passwordParams = connectionParams[1].Split("=");
        var password = passwordParams[1];

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            WorkerId = RegisterOne_Linux(server, password, 0, "", 10, 63, 15);
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            WorkerId = RegisterOne_Win(server, password, 0, "", 10, 63, 15);
        else
            throw new ArgumentException($"当前雪花Id库暂不支持{RuntimeInformation.OSDescription}平台");

        Console.WriteLine($"雪花Id机器码：workerId:{WorkerId}");
        var options = new IdGeneratorOptions(WorkerId);
        _IdGenInstance = new DefaultIdGenerator(options);
    }

    /// <summary>
    /// 设置参数，建议程序初始化时执行一次
    /// </summary>
    /// <param name="options"></param>
    public static void SetIdGenerator(IdGeneratorOptions options)
    {
        _IdGenInstance = new DefaultIdGenerator(options);
    }

    /// <summary>
    /// 生成新的Id
    /// 调用本方法前，请确保调用了 SetIdGenerator 方法做初始化。
    /// </summary>
    /// <returns></returns>
    public static long NextId()
    {
        if (_IdGenInstance == null) throw new ArgumentException("Please initialize Yitter.IdGeneratorOptions first.");

        return _IdGenInstance.NewLong();
    }
}