using System.IO;
using System.Threading.Tasks;

namespace Rocket.Libraries.Delta.FileSystem
{
    public interface IFileSystemAccessor
    {
        Task<string> GetAllTextAsync (string filename);
        Task WriteAllText(string filename, string text);
    }

    public class FileSystemAccessor : IFileSystemAccessor
    {
        public async Task<string> GetAllTextAsync (string filename)
        {
            using (var fileStream = new FileStream (filename, FileMode.Open))
            {
                using (var streamReader = new StreamReader (fileStream))
                {
                    return await streamReader.ReadToEndAsync ();
                }
            }
        }

        public async Task WriteAllText (string filename, string text)
        {
            await File.WriteAllTextAsync (filename, text);
        }

    }
}