namespace AnnoRDA.DataArchives
{
    internal interface IDataArchive
    {
        bool IsValid { get; }
        string Path { get; }

        IEnumerable<string> Files { get; }
        IEnumerable<string> FilesFor(params string[] extensions);

        Stream? OpenRead(string filePath);
        IEnumerable<string> Find(string pattern);

        //this should go vanish itself into the builder
        Task LoadAsync(params string[] forEndings);
    }
}
