namespace Orchard.Park.Model.Messages.Basics;

public class PageRequest
{
    private int _pageIndex;

    /// <summary>
    /// 当前页页码
    /// </summary>
    /// <example>1</example>
    public virtual int PageIndex
    {
        get => _pageIndex <= 0 ? 1 : _pageIndex;
        set => _pageIndex = value;
    }

    private int _pageSize;

    /// <summary>
    /// 每页条数
    /// </summary>
    /// <example>20</example>
    public virtual int PageSize
    {
        get => _pageSize <= 0 ? 10 : _pageSize;
        set => _pageSize = value;
    }
}