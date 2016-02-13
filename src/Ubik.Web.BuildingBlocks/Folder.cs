using System;
using System.IO;
using System.Linq;
using Ubik.Web.BuildingBlocks.Ext;

namespace Ubik.Web.BuildingBlocks
{
    public class Folder
    {
        protected readonly DirectoryInfo _dir;

        public Folder(string root)
        {
            _dir = root.StartsWith(@"~/") ? new DirectoryInfo(Path.GetFullPath(root)) : new DirectoryInfo(root);
        }

        internal Folder(DirectoryInfo direcory)
        {
            _dir = direcory;
        }

        public string RelativePath { get; private set; }

        public string AbsolutePath { get; private set; }
    }

    public interface IAssetsFolder
    {
        string FullAssetPath(int id, string fileName);
    }

    public class AssetsFolder : Folder, IAssetsFolder
    {
        private readonly Func<int, string> _assetFolderNamePredicate;

        public AssetsFolder(string root, Func<int, string> assetFolderNamePredicate)
            : base(root)
        {
            _assetFolderNamePredicate = assetFolderNamePredicate;
        }

        protected string WriteAssetToDisc(int id, string fileName, byte[] data)
        {
            var assetFolderName = _assetFolderNamePredicate.Invoke(id);
            var assetFolderPath = Path.Combine(_dir.FullName, assetFolderName);
            if (!Directory.Exists(assetFolderPath)) Directory.CreateDirectory(assetFolderPath);
            var assetFolderDirectory = new DirectoryInfo(assetFolderPath);
            fileName = FileNameAdjuster.AdjustFileNameIfFileWithSameNameExists(fileName, assetFolderDirectory);
            File.WriteAllBytes(Path.Combine(assetFolderDirectory.FullName, fileName), data);
            return fileName;
        }

        public string FullAssetPath(int id, string fileName)
        {
            var assetFolderName = _assetFolderNamePredicate.Invoke(id);
            return Path.Combine(_dir.FullName, assetFolderName, fileName);
        }
    }

    public interface IPictureFolder : IAssetsFolder
    {
        string SavePicture(int id, string fileName, byte[] data);
    }

    public class PictureFolder : AssetsFolder, IPictureFolder
    {
        public PictureFolder(string root, Func<int, string> assetFolderNamePredicate)
            : base(root, assetFolderNamePredicate)
        {
        }

        public PictureFolder(string root)
            : base(root, x => (x.RoundOff(100) + 100).ToString("######"))
        {
        }

        public string SavePicture(int id, string fileName, byte[] data)
        {
            return WriteAssetToDisc(id, fileName, data);
        }
    }

    internal class FileNameAdjuster
    {
        public static string AdjustFileNameIfFileWithSameNameExists(string fileName, DirectoryInfo folder)
        {
            if (folder.GetFiles(fileName).Any())
                fileName = //if there is a file with the same name prefix the new file name
                    string.Format("{0}_{1}", NowPrefix(), fileName);
            return fileName;
        }

        private static string NowPrefix()
        {
            return DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        }
    }
}