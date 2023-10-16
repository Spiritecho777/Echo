using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Screenmate.Classe
{
    [Serializable]
    public class CmdVoc
    {
        public string Nom { get;set; }
        public List<string> Phrase {  get;set; }
        public string ActionMethodName { get; set; }

        [NonSerialized]
        public Action Action;

        public void SetAction(Action action)
        {
            Action = action;
        }

        public void ExecuteAction()
        {
            Action?.Invoke();
        }

        public void SaveCmdVoc(List<CmdVoc> commands, string filePath)
        {
            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                formatter.Serialize(stream, commands);
            }
        }

        public static List<CmdVoc> LoadCmdVoc(string filePath)
        {
            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                return (List<CmdVoc>)formatter.Deserialize(stream);
            }
        }
    }
}
