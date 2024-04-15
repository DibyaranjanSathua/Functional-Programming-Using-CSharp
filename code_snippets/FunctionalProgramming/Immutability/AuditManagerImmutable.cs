namespace Immutability;

public class AuditManagerImmutable(int maxEntriesPerFile)
{
    // In order to make AddRecord immutable, we need to ensure that it does not read or write to the file.
    // We should pass all the required data as arguments to the method.
    // We need to pass both the file name and its contents as input.
    // And returns the result of the operation as output. An instruction that indicates what should happen to the file.
    public FileAction AddRecord(FileContent currentFile, string visitorName, DateTime timeOfVisit)
    {
        List<AuditEntry> entries = Parse(currentFile.Content);
        if (entries.Count < maxEntriesPerFile)
        {
            entries.Add(new AuditEntry(entries.Count + 1, visitorName, timeOfVisit));
            string[] newContent = Serialize(entries);
            return new FileAction(currentFile.FileName, ActionType.Update, newContent);
        }
        else
        {
            AuditEntry entry = new(1, visitorName, timeOfVisit);
            string[] newContent = Serialize([entry]);
            string newFileName = GetNewFileName(currentFile.FileName);
            return new FileAction(newFileName, ActionType.Create, newContent);
        }
    }

    private string[] Serialize(List<AuditEntry> entries)
    {
        return entries
            .Select(entry => $"{entry.Number}; {entry.VisitorName}; {entry.TimeOfVisit:s}")
            .ToArray();
    }

    private List<AuditEntry> Parse(string[] content)
    {
        List<AuditEntry> result = [];
        foreach (string line in content)
        {
            string[] parts = line.Split(';');
            int number = int.Parse(parts[0]);
            string visitorName = parts[1];
            DateTime timeOfVisit = DateTime.Parse(parts[2]);
            result.Add(new AuditEntry(number, visitorName, timeOfVisit));
        }

        return result;
    }

    private string GetNewFileName(string existingFileName)
    {
        string[] parts = existingFileName.Split('.');
        string fileName = parts[0];
        int index = int.Parse(fileName.Split('_')[1]);
        string extension = parts[1];
        string newFileName = $"{fileName}_{index + 1}.{extension}";
        return newFileName;
    }

    public IReadOnlyList<FileAction> RemoveMentionsAbout(string visitorName, FileContent[] directoryFiles)
    {
        // List<FileAction> result = [];
        // foreach (FileContent file in directoryFiles)
        // {
        //     FileAction action = RemoveMentionsIn(file, visitorName);
        //     result.Add(action);
        // }
        //
        // return result;

        return directoryFiles
            .Select(file => RemoveMentionsIn(file, visitorName))
            .Where(action => action is not null)
            .Select(action => action!.Value)
            .ToList();
    }

    private FileAction? RemoveMentionsIn(FileContent file, string visitorName)
    {
        List<AuditEntry> entries = Parse(file.Content);
        // Select clause will re-assign the index to the entries.
        List<AuditEntry> entriesToKeep = entries
            .Where(entry => entry.VisitorName != visitorName)
            .Select((entry, index) => new AuditEntry(index + 1, entry.VisitorName, entry.TimeOfVisit))
            .ToList();

        if (entriesToKeep.Count == entries.Count)
        {
            return null;
        }

        if (entriesToKeep.Count == 0)
        {
            return new FileAction(file.FileName, ActionType.Delete, []);
        }
        else
        {
            string[] newContent = Serialize(entriesToKeep);
            return new FileAction(file.FileName, ActionType.Update, newContent);
        }
    }
}


public struct FileContent(string fileName, string[] content)
{
    public readonly string FileName = fileName;
    public readonly string[] Content = content;
}


public struct FileAction(string fileName, ActionType type, string[] content)
{
    public readonly string FileName = fileName;
    public readonly string[] Content = content;
    public readonly ActionType Type = type;
}


public enum ActionType
{
    Create,
    Update,
    Delete
}


public struct AuditEntry(int number, string visitorName, DateTime timeOfVisit)
{
    public readonly int Number = number;
    public readonly string VisitorName = visitorName;
    public readonly DateTime TimeOfVisit = timeOfVisit;
}
