namespace Orchard.Park.SqlSugar
{
    public static class SqlWithExt
    {
        /// <summary>
        /// 排他锁
        /// </summary>
        public const string XLock = "WITH(XLOCK)";

        /// <summary>
        /// 行级排他锁
        /// </summary>
        public const string XLock_RowLock = "WITH(XLOCK,ROWLOCK)";
    }
}