namespace Immutability;

public class Persister
{
    public FileContent ReadFile(string fileName)
    {
        string[] content = File.ReadAllLines(fileName);
        return new FileContent(fileName, content);
    }

    public FileContent[] ReadDirectory(string directoryName)
    {
        return Directory
            .GetFiles(directoryName)
            .Select(ReadFile)
            .ToArray();
    }

    public void ApplyChanges(IReadOnlyList<FileAction> actions)
    {
        foreach (FileAction action in actions)
        {
            switch (action.Type)
            {
                case ActionType.Create:
                    File.WriteAllLines(action.FileName, action.Content);
                    break;
                case ActionType.Update:
                    File.WriteAllLines(action.FileName, action.Content);
                    break;
                case ActionType.Delete:
                    File.Delete(action.FileName);
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }
    }

    public void ApplyChange(FileAction action) => ApplyChanges([action]);
}
