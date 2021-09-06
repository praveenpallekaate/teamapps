using System;

namespace TeamApps.Shared
{
    /// <summary>
    /// User data model
    /// </summary>
    public class User : IDataModel
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
