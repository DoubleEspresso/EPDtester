using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace epdTester
{
    public class Settings
    {
        static SettingsFile param = new SettingsFile();
        static string CustomPath = null;
        static public string SettingsPath
        {
            get
            {
                return (CustomPath == null) ? Application.LocalUserAppDataPath : CustomPath;
            }
            set
            {
                CustomPath = value;
            }
        }
        static public string FileName
        {
            get
            {
                return string.Format("{0}\\settings.txt", SettingsPath);
            }
        }
        static bool modified = false;
        public Settings() { }
        static public void save()
        {
            if (modified)
            {
                lock (param)
                {
                    param.save(FileName);
                }
            }
        }
        public static bool Read()
        {
            lock(param)
            {
                if (!File.Exists(FileName))
                {
                    // create new
                    File.Create(FileName);
                    Log.WriteLine("..creating new settings file in {0}", FileName);
                }
                param.read(FileName);
                modified = false;
            }
            return true; 
        }
        static public void set(string name, string value)
        {
            lock (param) { param.set(name, value); modified = true; }
        }
        static public void get(string name, ref string value)
        {
            lock (param) { value = param.get(name);}
        }
        static public void set(string name, bool value)
        {
            lock (param) { param.set(name, value); modified = true; }
        }
        static public bool get(string name, ref bool value)
        {
            lock (param) { return param.get(name, ref value); }
        }
        static public void set(string name, int value)
        {
            lock (param) { param.set(name, value); modified = true; }
        }
        static public bool get(string name, ref int value)
        {
            lock (param) { return param.get(name, ref value); }
        }       
    }

    public class SettingsFile
    {
        public Dictionary<string, string> entries = new Dictionary<string, string>();
        public void Clear()
        {
            entries.Clear();
        }
        public bool read(Stream ss)
        {
            using (StreamReader txt = new StreamReader(ss))
            {
                string line = txt.ReadLine();
                while (line != null)
                {
                    if (line.StartsWith("#")) { } // this is a comment
                    else
                    {
                        int idx = line.IndexOf(':');
                        int len = line.Length;
                        if ((idx > 0) && (len - idx - 1 > 0))
                        {
                            string key = line.Substring(0, idx);
                            string value = line.Substring(idx + 1, len - idx - 1);
                            entries[key] = value;
                        }
                    }
                    line = txt.ReadLine();
                }
            }
            return true;
        }
        public bool exists(string name)
        {
            return entries.ContainsKey(name);
        }
        public bool read(string filename)
        {
            if (!File.Exists(filename)) return false;
            try
            {
                using (FileStream fstr = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
                    read(fstr);
                return true;
            }
            catch (Exception e)
            {
                Log.WriteLine("Error reading settings [{0}]: {1}", filename, e.Message);
            }
            return false;
        }
        public string get(string name)
        {
            try
            {
                if (exists(name)) return entries[name];
            }
            catch (Exception) { }
            return null;
        }
        public int getInt(string name)
        {
            try
            {
                return int.Parse(entries[name]);
            }
            catch (Exception) { }
            return 0;
        }
        public bool get(string name, ref int value)
        {
            if (exists(name))
            {
                value = getInt(name);
                return true;
            }
            return false;
        }
        public void set(string name, string value)
        {
            entries[name] = value;
        }
        public void unset(string name)
        {
            entries.Remove(name);
        }
        public void set(string name, int value)
        {
            set(name, value.ToString());
        }
        public void set(string name, bool value)
        {
            set(name, value.ToString());
        }
        public bool getBool(string name)
        {
            try
            {
                return bool.Parse(entries[name]);
            }
            catch (Exception) { }
            return false;
        }
        public bool get(string name, ref bool value)
        {
            if (exists(name))
            {
                value = getBool(name);
                return true;
            }
            return false;
        }
        public bool save(string filename)
        {
            try
            {
                if (entries.Count <= 0) return true; 
                List<string> all_entries = new List<string>();
                foreach (KeyValuePair<string, string> e in entries)
                {
                    all_entries.Add(string.Format("{0}:{1}", e.Key, e.Value));
                }
                all_entries.Sort();
                using (StreamWriter fout = new StreamWriter(filename))
                {
                    fout.WriteLine("# EPD tester settings file");
                    foreach (string str in all_entries) fout.WriteLine(str);
                }
                return true;
            }
            catch (Exception any)
            {
                Log.WriteLine("Error saving settings entry to file [{0}]: {1}", filename, any.Message);
            }
            return false;
        }
    }
}
