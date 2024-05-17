using Orchard.Park.IRepository.Infrastructure;
using SqlSugar;

namespace Orchard.Park.Repository.Infrastructure;

public class UnitOfWork(ISqlSugarClient sqlSugarClient) : IUnitOfWork
{
    /// <summary>
    ///     获取DB，保证唯一性
    /// </summary>
    /// <returns></returns>
    public SqlSugarScope GetDbClient()
    {
        // 必须要as，后边会用到切换数据库操作
        return sqlSugarClient as SqlSugarScope;
    }

    public void BeginTran()
    {
        sqlSugarClient.AsTenant().BeginTran();
    }

    public void CommitTran()
    {
        try
        {
            sqlSugarClient.AsTenant().CommitTran(); //
        }
        catch (Exception)
        {
            sqlSugarClient.AsTenant().RollbackTran();
            throw;
        }
    }

    public void RollbackTran()
    {
        sqlSugarClient.AsTenant().RollbackTran();
    }

    /// <summary>
    /// 使用嵌套事物
    /// 嵌套事物，有外部事务，内部事务用外部事务
    /// </summary>
    /// <returns></returns>
    public async Task UseNestedTranAsync(Func<Task> action)
    {
        //嵌套事物，有外部事务，内部事务用外部事务
        using var uow = sqlSugarClient.CreateContext(sqlSugarClient.Ado.IsNoTran());

        await action();

        uow.Commit();
    }
}