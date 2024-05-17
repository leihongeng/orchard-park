using SqlSugar;

namespace Orchard.Park.SqlSugar
{
    public class SqlSugarConnection
    {
        public int ConfigId { get; set; }

        public string Connection { get; set; }

        public DbType DbType { get; set; }
    }
}