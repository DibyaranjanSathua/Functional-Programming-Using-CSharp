namespace Immutability;

/// <summary>
/// Audit manager that keep tracks of all the visitors of an organisation. It uses flat text file as the underlying
/// storage mechanism.
/// 1; Dibyaranjan Sathua; 2021-09-01T12:00:00
/// 2; Monalisa Sahu; 2021-09-01T12:30:00
/// 3; Nibedita Sahu; 2021-09-01T12:40:00
///
/// Requirements
/// 1. Class should be able to add new records.
/// 2. There should be a limit to number records the file can contain. So the audit manager should handle a overflow
/// case. Each time a new record is added, it should check if the file has reached the limit. If it has reached the
/// limit it should create a new record.
/// 3. Should be able to delete a record.
///
/// This class conflicts two responsibilities.
/// 1. Log file processing. Contains logic to read and write to the file.
/// 2. Operating files on the disk. Remove log files. (Side effects)
/// Separate this class into two classes. One for log file processing and another for operating files on the disk.
/// 1. Immutable Core --> Log file processing
/// 2. Mutable Shell --> Operating files on the disk
///
/// Currently this class is not unit testable. It has side effects. It directly works with the files.
/// </summary>
public class AuditManagerMutable(int maxEntriesPerFile)
{
    public void AddRecord(string currentFile, string visitorName, DateTime timeOfVisit)
    {
        string[] lines = File.ReadAllLines(currentFile);
        if (lines.Length < maxEntriesPerFile)
        {
            int lastIndex = int.Parse(lines[^1].Split(';')[0]);
            string newLine = $"{lastIndex + 1}; {visitorName}; {timeOfVisit:s}";
            File.AppendAllLines(currentFile, new[] { newLine });
        }
        else
        {
            string newLine = $"1; {visitorName}; {timeOfVisit:s}";
            string newFileName = GetNewFileName(currentFile);
            File.WriteAllLines(newFileName, new[] { newLine });
        }
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

    public void RemoveMentionsAbout(string visitorName, string directoryName)
    {
        foreach (string fileName in Directory.GetFiles(directoryName))
        {
            string tempFile = Path.GetRandomFileName();
            List<string> linesToKeep = File
                .ReadLines(fileName)
                .Where(line => !line.Contains(visitorName))
                .ToList();

            if (linesToKeep.Count == 0)
            {
                File.Delete(fileName);
            }
            else
            {
                File.WriteAllLines(tempFile, linesToKeep);
                File.Delete(fileName);
                File.Move(tempFile, fileName);
            }
        }
    }
}
