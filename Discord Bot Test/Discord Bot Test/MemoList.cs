using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Discord_Bot_Test
{
    class MemoList
    {
        private string file;
        private Dictionary<string, string> dic;
        public MemoList()
        {
            file = "";
            dic = new Dictionary<string, string>();
        }
        
        public void initialise(string _file)
        {
            file = _file;
            StreamReader reader = new StreamReader(file);
            while(reader.Peek() > -1)
            {
                string reference = reader.ReadLine();
                string memo = reader.ReadLine();
                dic[reference] = memo;
            }
            reader.Close();
        }

        public void cleanup()
        {
            StreamWriter writer = new StreamWriter(file, false);
            foreach(KeyValuePair<string, string> i in dic)
            {
                writer.WriteLine(i.Key);
                writer.WriteLine(i.Value);
            }
            writer.Close();
        }

        public string handleMemoUpdate(string param1, string param2 = "")
        {
            string result = "";
            if (param2 == "")
            {
                if (dic.ContainsKey(param1))
                {
                    result = dic[param1];
                }
                else
                {
                    result = "I don't know " + param1 + ".";
                }
            }
            else
            {
                bool replacingOld = false;
                if (dic.ContainsKey(param1))
                {
                    replacingOld = true;
                }
                if(replacingOld)
                {
                    result = "You have overwritten a memo:\n" + dic[param1];
                }
                else
                {
                    result = "I'll remember that.";
                }
                dic[param1] = param2;
            }
            return result;
        }

        public string getList()
        {
            string list = "";
            foreach(KeyValuePair<string, string> i in dic)
            {
                list += i.Key + ", ";
            }
            list = list.Substring(0, list.Length-2);
            list += ".";
            return list;
        }
    }
}
