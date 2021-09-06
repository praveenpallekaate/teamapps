using System.Collections.Generic;
using System.Linq;
using Core.Utilities;
using TeamApps.Shared;

namespace TeamApps.UI
{
    /// <summary>
    /// Entity helper
    /// </summary>
    /// <typeparam name="T"><see cref="T"/></typeparam>
    public static class EntityHelper<T>
        where T : IModel
    {
        /// <summary>
        /// Assigns collection ids if not exists
        /// </summary>
        /// <param name="items">Collection</param>
        /// <returns>Updated collection</returns>
        public static IEnumerable<T> AssignIdsIfNotExists(IEnumerable<T> items)
        {
            var result = items;

            if (CommonUtils.IsValidCollection(items))
            {
                var searchFilter = items.Where(i => i.Id < 1);
                bool hasAnyUnassigned = CommonUtils.IsValidCollection(searchFilter);

                if (hasAnyUnassigned)
                {
                    int maxId = items.Max(j => j.Id);

                    maxId = maxId == 0 ? 1 : (maxId + 1);

                    result = items
                        .Select(i =>
                        {
                            if (!(i.Id > 0))
                            {
                                i.Id = maxId;

                                maxId += 1;
                            }

                            return i;
                        })
                        .ToList();
                }
            }

            return result;
        }
    }
}
