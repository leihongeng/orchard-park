using Orchard.Park.CodeGenerator;
using Orchard.Park.IRepository.Infrastructure;
using SqlSugar;

namespace Orchard.Park.Repository.Infrastructure
{
    public class CodeGeneratorRepository(IUnitOfWork unitOfWork)
        : BaseRepository<object>(unitOfWork), ICodeGeneratorRepository
    {
        /// <summary>
        /// 获取所有的表
        /// </summary>
        /// <returns></returns>
        public List<DbTableInfo> GetDbTables()
        {
            var tables = DbClient.DbMaintenance.GetTableInfoList(false).OrderBy(p => p.Name).ToList();
            var views = DbClient.DbMaintenance.GetViewInfoList(false).OrderBy(p => p.Name).ToList();
            if (!views.Any()) return tables;
            var newList = tables.Union(views).ToList();
            return newList;
        }

        /// <summary>
        /// 获取表下面所有的字段
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public List<DbColumnInfo> GetDbTablesColumns(string tableName)
        {
            var columns = DbClient.DbMaintenance.GetColumnInfosByTableName(tableName, false);
            return columns;
        }

        /// <summary>
        /// 自动生成代码
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public byte[] CodeGen(string tableName, string fileType)
        {
            var tables = DbClient.DbMaintenance.GetTableInfoList(false);
            var views = DbClient.DbMaintenance.GetViewInfoList(false);
            var tb = tables.Find(p => p.Name == tableName) ?? views.Find(p => p.Name == tableName);
            if (tb == null)
            {
                return null;
            }

            var columns = DbClient.DbMaintenance.GetColumnInfosByTableName(tb.Name, false);
            return GeneratorCodeHelper.CodeGenerator(tb.Name, tb.Description, columns, fileType);
        }

        /// <summary>
        /// 自动生成类型的所有数据库代码
        /// </summary>
        /// <returns></returns>
        public byte[] CodeGenByAll(string fileType)
        {
            var tables = DbClient.DbMaintenance.GetTableInfoList(false);
            var views = DbClient.DbMaintenance.GetViewInfoList(false);
            var newList = tables;
            if (views.Any()) newList = tables.Union(views).ToList();

            var allDb = new List<DbTableInfoAndColumns>();
            newList.ForEach(p =>
            {
                var model = new DbTableInfoAndColumns
                {
                    Name = p.Name,
                    DbObjectType = p.DbObjectType,
                    Description = p.Description,
                    columns = DbClient.DbMaintenance.GetColumnInfosByTableName(p.Name, false)
                };
                allDb.Add(model);
            });

            if (!allDb.Any())
                return null;

            return GeneratorCodeHelper.CodeGeneratorAll(allDb, fileType);
        }
    }
}