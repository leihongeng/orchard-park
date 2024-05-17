namespace Orchard.Park.Model.Messages.Basics;

public class PageResponse<T> where T : class
{
    /// <summary>
    /// 返回内容主体
    /// </summary>
    public List<T> List { get; set; } = new();

    /// <summary>
    /// 总条数
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// 当前页
    /// </summary>
    public int PageIndex { get; set; }

    /// <summary>
    /// 分页大小
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// 总页数
    /// </summary>
    public int TotalPages { get; set; }
}

public static class PageResponseExt
{
    public static PageResponse<T> ToPageResponse<T>(this IPageList<T> page) where T : class
    {
        return new PageResponse<T>
        {
            TotalCount = page.TotalCount,
            List = page.ToList(),
            PageIndex = page.PageIndex,
            PageSize = page.PageSize,
            TotalPages = page.TotalPages
        };
    }

    public static IPageList<T> ToPageList<T>(this List<T> data, PageRequest req, int totalCount)
    {
        return new PageList<T>(data, req.PageIndex, req.PageSize, totalCount);
    }
}