using System.Threading.Tasks;
using TeamApps.Shared;

namespace TeamApps.API
{
    /// <summary>
    /// Write service
    /// </summary>
    /// <typeparam name="T1">Data model</typeparam>
    /// <typeparam name="T2">View model</typeparam>
    public interface IWriteService<T1, T2>
        where T1 : IDataModel
        where T2 : IViewModel
    {
        /// <summary>
        /// Insert item to collection
        /// </summary>
        /// <param name="item"><see cref="T2"/> item</param>
        /// <returns>Result</returns>
        Task<int> InsertItemAsync(T2 item);

        /// <summary>
        /// Update item
        /// </summary>
        /// <param name="item"><see cref="T2"/> item</param>
        /// <returns>Result</returns>
        Task<bool> UpdateItemAsync(T2 item);
    }
}
