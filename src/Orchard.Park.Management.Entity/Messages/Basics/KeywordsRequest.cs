namespace Orchard.Park.Model.Messages.Basics;

public class KeywordsRequest : PageRequest
{
    /// <summary>
    /// 搜索关键词
    /// </summary>
    public string Keywords { get; set; }
}