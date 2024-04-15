namespace Immutability;

public class ApplicationService(string directoryName)
{
    private readonly string _directoryName = directoryName;
    private readonly AuditManagerImmutable _auditManager;
    private readonly Persister _persister;

    public void RemoveMentionsAbout(string visitorName)
    {
        FileContent[] directoryFiles = _persister.ReadDirectory(_directoryName);
        IReadOnlyList<FileAction> actions = _auditManager.RemoveMentionsAbout(visitorName, directoryFiles);
        _persister.ApplyChanges(actions);
    }

    public void AddRecord(string visitorName, DateTime timeOfVisit)
    {
        FileInfo fileInfo = new DirectoryInfo(_directoryName)
            .GetFiles()
            .OrderByDescending(x => x.LastWriteTime)
            .First();
        FileContent file = _persister.ReadFile(fileInfo.Name);
       FileAction action = _auditManager.AddRecord(file, visitorName, timeOfVisit);
        _persister.ApplyChange(action);
    }
}
