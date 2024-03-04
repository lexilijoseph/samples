namespace HierarchyData
{
    public class FolderHierarchy
    {
      //  public FolderModel ParentFolder { get; set; }
        public string ParentFolder { get; set; }
        public List<FolderHierarchy> ChildFolders { get; set; }
    }
}
