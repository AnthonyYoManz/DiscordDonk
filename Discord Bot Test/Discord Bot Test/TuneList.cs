using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Discord_Bot_Test
{
    class TuneList
    {
        static Random rng;
        private string file;
        private List<string> list;
        public TuneList()
        {
            file = "";
            rng = new Random(DateTime.Now.Millisecond + DateTime.Now.Minute * 600);
            list = new List<string>();
        }

        public void initialise(string _file)
        {
            file = _file;
            StreamReader reader = new StreamReader(file);
            while (reader.Peek() > -1)
            {
                string tune = reader.ReadLine();
                list.Add(tune);
            }
            reader.Close();
        }

        public void cleanup()
        {
            StreamWriter writer = new StreamWriter(file, false);
            foreach (string i in list)
            {
                writer.WriteLine(i);
            }
            writer.Close();
        }

        public string handleTuneUpdate(string _tune)
        {
            string result = "";
            if (_tune.Contains("&"))
            {
                _tune = _tune.Substring(0, _tune.IndexOf('&'));
            }
            if (list.Contains(_tune))
            {
                result = "This tune has already been registered.";
            }
            else
            {
                if (_tune.Contains("youtube.com/watch?v="))
                {
                    list.Add(_tune);
                    result = "Thank you for your contribution.";
                }
                else
                {
                    result = "Tunes must be provided in the form of a YouTube link like the following:\n'https://www.youtube.com/watch?v=MG157tElj2M'";
                }
            }
            return result;
        }

        public int getCount()
        {
            return list.Count;
        }

        public string getTune(int id)
        {
            string tune = "";
            if(id >=0 && id < list.Count)
            {
                tune = list[id];
            }
            return tune;
        }

        public string getRandomTune(int depth = -1)
        {
            string tune = "";
            if(depth < 1 || depth > getCount())
            {
                depth = getCount(); 
            }
            int tuneID = getCount() - rng.Next(depth);
            Console.WriteLine("Getting tune at " + tuneID);
            tune = getTune(tuneID-1);
            return tune;
        }
    }
}
