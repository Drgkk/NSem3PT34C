using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSem3PT34.Classes.Util
{
    public class SpellChecker
    {
        private static SpellChecker instance;
        private HashSet<string> dictionary = new HashSet<string>();

        private SpellChecker() {}

        private static readonly object _lock = new object();
        public static SpellChecker GetInstance()
        {
            if (instance == null)
            {
                lock(_lock) {
                    if (instance == null){
                        instance = new SpellChecker();
                    }
                }
            }
		
            return instance;
        }
	
        public void LoadDictionary(string dictionaryPath)
        {
            try
            {
                foreach (var rawLine in File.ReadLines(dictionaryPath, Encoding.UTF8))
                {
                    var word = rawLine?.Trim();
                    if (!string.IsNullOrEmpty(word))
                        dictionary.Add(word);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public bool IsMisspelled(string word)
        {
            if (string.IsNullOrEmpty(word)) return true;
            return !dictionary.Contains(word);
        }
    }
}
