using System.IO;

namespace Blogless
{
	public interface IFileManager
    {
        bool TryGetFileStream(string path, out Stream stream);
	}
}
