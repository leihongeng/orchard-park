using Orchard.Park.IRepository.Infrastructure;
using Orchard.Park.IService.CodeGenerator;
using SqlSugar;

namespace Orchard.Park.Service.CodeGenerator
{
    public class CodeGeneratorService(ICodeGeneratorRepository codeGeneratorRepo) : ICodeGeneratorService
    {
        /// <summary>
        /// 获取所有的表
        /// </summary>
        /// <returns></returns>
        public List<DbTableInfo> GetDbTables()
        {
            return codeGeneratorRepo.GetDbTables();
        }

        /// <summary>
        /// 获取表下面所有的字段
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public List<DbColumnInfo> GetDbTablesColumns(string tableName)
        {
            return codeGeneratorRepo.GetDbTablesColumns(tableName);
        }

        /// <summary>
        /// 自动生成代码
        /// </summary>
        /// <returns></returns>
        public byte[] CodeGen(string tableName, string fileType)
        {
            return codeGeneratorRepo.CodeGen(tableName, fileType);
        }

        /// <summary>
        /// 自动生成类型的所有数据库代码
        /// </summary>
        /// <returns></returns>
        public byte[] CodeGenByAll(string fileType)
        {
            return codeGeneratorRepo.CodeGenByAll(fileType);
        }
    }
}