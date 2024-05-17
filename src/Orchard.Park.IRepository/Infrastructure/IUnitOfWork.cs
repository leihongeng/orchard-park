using SqlSugar;

namespace Orchard.Park.IRepository.Infrastructure;

public interface IUnitOfWork
{
    SqlSugarScope GetDbClient();

    void BeginTran();

    void CommitTran();

    void RollbackTran();

    /// <summary>
    /// 使用嵌套事物
    /// 嵌套事物，有外部事务，内部事务用外部事务
    /// </summary>
    /// <returns></returns>
    Task UseNestedTranAsync(Func<Task> action);
}