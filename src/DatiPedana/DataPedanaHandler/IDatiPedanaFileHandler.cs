namespace DataPedanaHandler
{
    public interface IDatiPedanaFileHandler
    {
        IEnumerable<DatiPedanaRow> ReadPlatformA(string filePath);

        IEnumerable<DatiPedanaRow> ReadPlatformB(string filePath);

        void CreateCopy(string sourceFile, string destinationFile, DatiPedanaRow[] PlatformA, DatiPedanaRow[] PlatofrmB);
    }
}
