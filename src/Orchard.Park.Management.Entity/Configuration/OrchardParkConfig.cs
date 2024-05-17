namespace Orchard.Park.Model.Configuration;

public class OrchardParkConfig
{
    public ConnectionStringOptions ConnectionStrings { get; set; }
}

public class ConnectionStringOptions
{
    /// <summary>
    /// 数据库连接字符串
    /// </summary>
    public string MySql { get; set; }

    /// <summary>
    /// Redis连接字符串
    /// </summary>
    public string Redis { get; set; }
}