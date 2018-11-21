using System.Threading.Tasks;

namespace HttpLogExtensions
{
    /// <summary>
    /// 定義事件紀錄儲存方法的實作介面
    /// </summary>
    public interface IHttpLogStore
    {
        Task Add(HttpLog log);
    }
}
