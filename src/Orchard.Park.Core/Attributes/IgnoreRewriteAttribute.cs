using System;

namespace Orchard.Park.Core.Attributes
{
    /// <summary>
    /// 忽略Api重写返回值
    /// </summary>
    public class IgnoreRewriteAttribute : Attribute
    {
        public IgnoreRewriteAttribute(bool outputLog = false)
        {
            OutputLog = outputLog;
        }

        public bool OutputLog { get; set; }
    }
}