using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Screenmate.Classe
{
    [Serializable]
    public class EventSave
    {
        public string Date {  get; set; }
        public string Content { get; set; }
        public bool Annuel { get; set; }

        public static void SaveEventSave(List<EventSave> Date, string filePath)
        {
            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                formatter.Serialize(stream, Date);
            }
        }

        public static List<EventSave> LoadEventSave(string filePath)
        {
            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                return (List<EventSave>)formatter.Deserialize(stream);
            }
        }
    }
}
