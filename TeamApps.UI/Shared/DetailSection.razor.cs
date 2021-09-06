using System.Linq;
using System.Threading.Tasks;
using Core.Utilities;
using Microsoft.AspNetCore.Components;
using TeamApps.Shared;

namespace TeamApps.UI
{
    /// <summary>
    /// Section page
    /// </summary>
    public class DetailSectionBase : ComponentBase
    {
        /// <summary>
        /// Gets or sets page model
        /// </summary>
        [Parameter]
        public DetailSectionPage DetailSectionPage { get; set; }
            = new DetailSectionPage();

        /// <summary>
        /// Gets or sets eventcallback to parent
        /// </summary>
        [Parameter]
        public EventCallback<KeyValue> OnUpdate { get; set; }

        /// <summary>
        /// On init
        /// </summary>
        protected override void OnInitialized()
        {
            DetailSectionPage.EditedSection.KeyValues = EntityHelper<KeyValue>
                .AssignIdsIfNotExists(DetailSectionPage.EditedSection.KeyValues);
        }

        /// <summary>
        /// Toggles mode
        /// </summary>
        /// <param name="mode">Mode type</param>
        /// <param name="isUpdate">Flag for update</param>
        /// <returns></returns>
        protected async Task ToggleModeAsync(Modes mode, bool isUpdate = false)
        {
            // Clean data for update
            if (isUpdate)
            {
                var valids = DetailSectionPage
                    .EditedSection
                    .KeyValues
                    .Where(i => !string.IsNullOrEmpty(i.Name))
                    .Select(j =>
                    {
                        var filterOut = j.KeyValues?
                            .Where(k => !string.IsNullOrEmpty(k.Name))
                            .Select(l =>
                            {
                                var filterInner = l.KeyValues?
                                    .Where(m => !string.IsNullOrEmpty(m.Name))?
                                    .ToList();

                                l.KeyValues = CommonUtils.IsValidCollection(filterInner) ? filterInner : Enumerable.Empty<KeyValue>();

                                return l;
                            })
                            .ToList();

                        j.KeyValues = CommonUtils.IsValidCollection(filterOut) ? filterOut : Enumerable.Empty<KeyValue>();

                        return j;
                    })
                    .ToList();

                DetailSectionPage.EditedSection.KeyValues = valids;

                await OnUpdate.InvokeAsync(DetailSectionPage.EditedSection);
            }

            DetailSectionPage.Mode = mode;
        }

        /// <summary>
        /// Update section and parent
        /// </summary>
        /// <param name="updateDetails">Update details</param>
        protected void UpdateSectionPushToParent(KeyValueUpdate updateDetails)
        {
            if (updateDetails.KeyValue is KeyValue && updateDetails.ParentId > 0)
            {
                DetailSectionPage.EditedSection.KeyValues = DetailSectionPage
                    .EditedSection
                    .KeyValues
                    .Select(i =>
                    {
                        if (i.Id == updateDetails.ParentId)
                        {
                            i.Description = updateDetails.KeyValue.Description;
                            i.Flag = updateDetails.KeyValue.Flag;
                            i.IsActive = updateDetails.KeyValue.IsActive;
                            i.KeyValues = updateDetails.KeyValue.KeyValues;
                            i.Name = updateDetails.KeyValue.Name;
                            i.SubType = updateDetails.KeyValue.SubType;
                            i.Type = updateDetails.KeyValue.Type;
                            i.Value = updateDetails.KeyValue.Value;
                            i.Values = updateDetails.KeyValue.Values;
                        }

                        return i;
                    });
            }
        }
    }
}
