using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mic.Volo.AdoNetEx
{
    public class Image
    {
        public Image(int id, string fileName, string title, byte[] data)
        {
            Id = id;
            FileName = fileName;
            Title = title;
            Data = data;
        }

        public int Id { get;private set; }
        public string FileName { get; private set; }
        public string Title { get; private set; }
        public byte[] Data { get; private set; }

    }
}
