namespace TeamApps.Shared
{
    /// <summary>
    /// Collections in application database
    /// </summary>
    public enum AppCollections
    {
        AppLookups,
        Teams,
        Applications,
        AdditionalInformations,
        Users,
        Activities,
        Notifications,
        Resources,
    }

    /// <summary>
    /// AppLookup types
    /// </summary>
    public enum AppLookupTypes
    {
        team,
    }

    /// <summary>
    /// KeyValue types
    /// </summary>
    public enum KeyValueTypes
    {
        section,
        person,
        boolean,
        array,
        list,
        help,
        link,
        clicklink,
        options,
        tagitem,
        textsummaryitem,
        item,
        comment,
        timeline,
    }

    /// <summary>
    /// Edit modes
    /// </summary>
    public enum Modes
    {
        add,
        edit,
        read,
        prompt,
    }

    /// <summary>
    /// Chart types
    /// </summary>
    public enum ChartTypes
    {
        pie,
        donut,
        bar
    }

    /// <summary>
    /// App navigation pages
    /// </summary>
    public enum NavigationPages
    {
        home,
        resources,
        health,
        jobs,
    }

    /// <summary>
    /// Input types
    /// </summary>
    public enum InputTypes
    {
        fieldvalue,
        text,
        date,
        select,
        multilinetext
    }

    /// <summary>
    /// App themes
    /// </summary>
    public enum AppThemes
    {
        dark,
        light,
    }

    /// <summary>
    /// Modal types
    /// </summary>
    public enum ModalTypes
    {
        addteam,
        saveteam,
        addapplication,
        saveapplication,
        addresource,
        saveresource,
        addallocation,
        saveallocation,
        close,
    }
}
