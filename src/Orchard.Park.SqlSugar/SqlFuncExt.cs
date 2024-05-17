using System;

namespace Orchard.Park.SqlSugar
{
    /// <summary>
    /// 扩展SqlFunc函数
    /// </summary>
    public static class SqlFuncExt
    {
        /// <summary>
        /// 获取两个经纬度之间的距离
        /// </summary>
        /// <param name="latBegin"></param>
        /// <param name="lngBegin"></param>
        /// <param name="latEnd"></param>
        /// <param name="lngEnd"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public static double GetDistance(double latBegin, double lngBegin, double latEnd, double lngEnd)
        {
            //这里不能写任何实现代码，需要在配置中实现
            throw new NotSupportedException("Can only be used in expressions");
        }

        /// <summary>
        /// 获取两个经纬度之间的距离
        /// </summary>
        /// <param name="latBegin"></param>
        /// <param name="lngBegin"></param>
        /// <param name="latEnd"></param>
        /// <param name="lngEnd"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public static double GetDistance(string latBegin, string lngBegin, string latEnd, string lngEnd)
        {
            //这里不能写任何实现代码，需要在配置中实现
            throw new NotSupportedException("Can only be used in expressions");
        }
    }
}