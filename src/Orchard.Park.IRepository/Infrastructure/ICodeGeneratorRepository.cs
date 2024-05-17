﻿using SqlSugar;

namespace Orchard.Park.IRepository.Infrastructure
{
    public interface ICodeGeneratorRepository
    {
        /// <summary>
        /// 获取所有的表
        /// </summary>
        /// <returns></returns>
        List<DbTableInfo> GetDbTables();

        /// <summary>
        /// 获取表下面所有的字段
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        List<DbColumnInfo> GetDbTablesColumns(string tableName);

        /// <summary>
        /// 自动生成全部代码
        /// </summary>
        /// <returns></returns>
        byte[] CodeGen(string tableName, string fileType);

        /// <summary>
        /// 自动生成类型的所有数据库代码
        /// </summary>
        /// <returns></returns>
        byte[] CodeGenByAll(string fileType);
    }
}