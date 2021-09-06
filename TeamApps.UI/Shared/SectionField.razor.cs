using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TeamApps.Shared;

namespace TeamApps.UI
{
    /// <summary>
    /// Section field
    /// </summary>
    public class SectionFieldBase : ComponentBase
    {
        /// <summary>
        /// Gets or sets layout page model
        /// </summary>
        [CascadingParameter]
        public MainLayoutPage MainLayoutPageDetails { get; set; }

        /// <summary>
        /// Gets or sets jsruntime
        /// </summary>
        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        /// <summary>
        /// Gets or sets class
        /// </summary>
        [Parameter]
        public string Class { get; set; }

        /// <summary>
        /// Gets or sets parent id
        /// </summary>
        [Parameter]
        public int ParentId { get; set; }

        /// <summary>
        /// Gets or sets details
        /// </summary>
        [Parameter]
        public KeyValue Detail { get; set; }
            = new KeyValue();

        /// <summary>
        /// Gets or sets mode
        /// </summary>
        [Parameter]
        public Modes Mode { get; set; }
            = Modes.read;

        /// <summary>
        /// Gets or sets update callback
        /// </summary>
        [Parameter]
        public EventCallback<KeyValueUpdate> OnUpdate { get; set; }

        /// <summary>
        /// Gets or sets edited detail
        /// </summary>
        protected KeyValue EditedDetail { get; set; }
            = new KeyValue();

        /// <summary>
        /// Gets or sets new name
        /// </summary>
        protected string NewName { get; set; }

        /// <summary>
        /// Gets or sets new value
        /// </summary>
        protected string NewValue { get; set; }

        /// <summary>
        /// On init
        /// </summary>
        protected override void OnInitialized()
        {
            EditedDetail = Detail.Copy();
        }

        /// <summary>
        /// Updated parent
        /// </summary>
        /// <returns></returns>
        protected async Task UpdateParentAsync()
        {
            await OnUpdate.InvokeAsync(new KeyValueUpdate
            {
                ItemId = Detail.Id,
                KeyValue = EditedDetail,
                ParentId = ParentId,
            });
        }

        /// <summary>
        /// Open url in new tab
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected async Task OpenUrlAsync(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                await JSRuntime.InvokeAsync<object>("open", url, "_blank");
            }
        }

        /// <summary>
        /// Update selected option
        /// </summary>
        /// <param name="value">Selected flag</param>
        /// <param name="item">Item detail</param>
        /// <param name="editedDetail">Field detail</param>
        /// <returns></returns>
        protected async Task UpdateSelectedOptionsAsync(bool? value, KeyValue item, KeyValue editedDetail)
        {
            editedDetail.KeyValues = editedDetail
                .KeyValues
                .Select(i =>
                {
                    if (i.Name == item.Name)
                    {
                        if (value.HasValue && value.Value)
                        {
                            i.Flag = value.Value;
                        }
                        else
                        {
                            i.Flag = false;
                        }
                    }

                    return i;
                });

            editedDetail.Values = editedDetail
                .KeyValues
                .Where(j => (j.Flag.HasValue && j.Flag.Value == true))
                .Select(k => k.Name)
                .ToArray();

            await UpdateParentAsync();
        }

        /// <summary>
        /// Add new item
        /// </summary>
        protected void AddItemToList()
        {
            var items = CommonUtils.IsValidCollection(EditedDetail.KeyValues) ? EditedDetail.KeyValues.ToList() : new List<KeyValue>();
            int maxId = items.Count > 0 ? items.Max(i => i.Id) : 1;

            items.Add(new KeyValue { Id = maxId + 1 });

            EditedDetail.KeyValues = items;
        }

        /// <summary>
        /// Add tag
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected async Task AddTagAsync(KeyValue item)
        {
            string tag = item.Description;

            if (!string.IsNullOrEmpty(tag))
            {
                var items = EditedDetail
                    .KeyValues
                    .Select(i =>
                    {
                        if (i.Id == item.Id)
                        {
                            var tags = CommonUtils.IsValidCollection(i.KeyValues) ? i.KeyValues.ToList() : new List<KeyValue>();
                            int maxId = tags.Count > 0 ? tags.Max(j => j.Id) : 1;

                            tags.Add(new KeyValue { Id = maxId + 1, Name = tag });

                            i.KeyValues = tags;

                            // Reset description
                            i.Description = string.Empty;
                        }

                        return i;
                    })
                    .ToList();

                EditedDetail.KeyValues = items;

                await UpdateParentAsync();
            }
        }

        /// <summary>
        /// Add Comment
        /// </summary>
        /// <returns></returns>
        protected async Task AddCommentAsync()
        {
            if (!string.IsNullOrEmpty(NewValue))
            {
                var keyValues = CommonUtils.IsValidCollection(EditedDetail.KeyValues)
                    ? EditedDetail.KeyValues.ToList()
                    : new List<KeyValue>();
                int maxId = keyValues.Count > 0 ? keyValues.Max(i => i.Id) : 1;

                keyValues.Add(new KeyValue
                {
                    Id = maxId + 1,
                    Name = MainLayoutPageDetails.LoggedUser.Name,
                    Description = DateTime.Now.ToFormattedShortDate(),
                    Value = NewValue,
                });

                EditedDetail.KeyValues = keyValues;

                NewValue = string.Empty;

                await UpdateParentAsync();
            }
        }
    }
}
