using System.Collections.Generic;
using System.Threading.Tasks;
using TeamApps.Shared;

namespace TeamApps.API
{
    /// <summary>
    /// Read services
    /// </summary>
    /// <typeparam name="T1">Data model</typeparam>
    /// <typeparam name="T2">View model</typeparam>
    public interface IReadService<T1, T2>
        where T1 : IDataModel
        where T2 : IViewModel
    {
        /// <summary>
        /// Fetch <see cref="T1"/> details item
        /// </summary>
        /// <param name="id">Item id</param>
        /// <returns><see cref="T2"/> item</returns>
        Task<T2> GetItemDetailsAsync(int id);

        /// <summary>
        /// Fetch <see cref="T1"/> details items
        /// </summary>
        /// <param name="ids">Item ids</param>
        /// <returns><see cref="T2"/> items</returns>
        Task<IEnumerable<T2>> GetItemsDetailsAsync(int[] ids);
    }
}
