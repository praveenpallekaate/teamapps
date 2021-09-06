using System;

namespace TeamApps.Shared
{
    /// <summary>
    /// App model
    /// </summary>
    public interface IModel
    {
        int Id { get; set; }
        bool IsActive { get; set; }
    }
}
