using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDEaHTest1
{
    public class FileExtension
    {
        private string _extension;
        private string _name;

        public string Name => _name;
        public string Extension => _extension;

        public FileExtension(string extension, string name)
        {
            _extension = extension;
            _name = name;
        }
    }
}
