using Microsoft.Extensions.Options;
using TeamApps.Shared;

namespace TeamApps.API
{
    /// <summary>
    /// Repository base
    /// </summary>
    /// <typeparam name="T">Data model</typeparam>
    public class RepositoryBase<T>
        where T : IDataModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryBase{T}"/> class.
        /// ctor
        /// </summary>
        /// <param name="options"></param>
        public RepositoryBase(IOptions<AppSettings> options)
        {
            LiteDbHelper<T>.Initialize(options);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            LiteDbHelper<T>.Dispose();
        }
    }
}
