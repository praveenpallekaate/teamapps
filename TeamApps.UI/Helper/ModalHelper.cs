namespace TeamApps.UI
{
    /// <summary>
    /// Modal helper
    /// </summary>
    public class ModalHelper
    {
        /// <summary>
        /// Toggles modal
        /// </summary>
        /// <param name="customModalDetail">Modal details</param>
        /// <param name="title">Modal title</param>
        /// <param name="isUpdate">Flag for update</param>
        /// <returns>Modal details</returns>
        public static AppModalDetail ToggleModal(
            AppModalDetail customModalDetail,
            string title = "",
            bool isUpdate = true)
        {
            if (!customModalDetail.Open)
            {
                customModalDetail = new AppModalDetail
                {
                    Open = true,
                    IsUpdate = isUpdate,
                    Title = title,
                };
            }
            else
            {
                customModalDetail = new AppModalDetail();
            }

            return customModalDetail;
        }
    }
}
