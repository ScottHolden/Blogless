using System.IO;

namespace Blogless
{
	public class EmbededFileManager : IFileManager
	{
        public bool TryGetFileStream(string path, out Stream stream)
        {
            stream = typeof(EmbededFileManager).Assembly.GetManifestResourceStream(TransformToResourcePath(path));
            return stream != null;
        }

        private static string TransformToResourcePath(string path)
            => $"{nameof(Blogless)}.www.{path.Replace('/', '.').Replace('\\', '.')}";
    }
}
