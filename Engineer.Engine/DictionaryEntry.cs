using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.Engine
{
    [Serializable]
    public class DictionaryEntry
    {
        private string _Key;
        private object _Value;
        public DictionaryEntry()
        {
        }
        public DictionaryEntry(string Key, object Value)
        {
            _Key = Key;
            _Value = Value;
        }
        public string Key { get => _Key; set => _Key = value; }
        public object Value { get => _Value; set => _Value = value; }
    }
}
