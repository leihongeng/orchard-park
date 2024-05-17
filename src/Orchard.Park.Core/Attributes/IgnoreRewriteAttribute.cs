using System;

namespace Orchard.Park.Core.Attributes
{
    /// <summary>
    /// 忽略Api重写返回值
    /// </summary>
    public class IgnoreRewriteAttribute(bool outputLog = false) : Attribute
    {
        public bool OutputLog { get; set; } = outputLog;
    }
}