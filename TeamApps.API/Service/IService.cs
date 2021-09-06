using System.Collections.Generic;
using System.Threading.Tasks;
using TeamApps.Shared;

namespace TeamApps.API
{
    /// <summary>
    /// App services
    /// </summary>
    /// <typeparam name="T">Data model</typeparam>
    public interface IService<T>
        where T : IDataModel
    {
        /// <summary>
        /// Fetch items for ids
        /// </summary>
        /// <param name="ids">Item ids</param>
        /// <returns><see cref="T"/> collection</returns>
        Task<IEnumerable<T>> GetItemsAsync(int[] ids);

        /// <summary>
        /// Fetch item for id
        /// </summary>
        /// <param name="id">Item id</param>
        /// <returns><see cref="T"/> item</returns>
        Task<T> GetItemAsync(int id);
    }
}
