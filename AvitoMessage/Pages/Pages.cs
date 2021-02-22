using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvitoMessage.Pages
{
    public class Page
    {
        public string link { protected set; get; }

        public Page(string link)
        {
            this.link = link;
        }       
    }
}
