using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Screenmate.Classe
{
    [Serializable]
    public class FolderStructure
    {
        public Dictionary<string, Dictionary<string, List<string>>> Folders { get; set; }
        public Dictionary<string, string> ImagePaths { get; set; }
    }

    public class FileManager
    {
        public void SaveFolderStructure(FolderStructure folderStructure, string filePath)
        {
            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                formatter.Serialize(stream, folderStructure);
            }
        }

        public FolderStructure LoadFolderStructure(string filePath)
        {
            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                return (FolderStructure)formatter.Deserialize(stream);
            }
        }
    }
}
