using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace River
{
    class ExitMenu : Menu
    {
        public bool HasExited = false;


        public ExitMenu(MenuSideType MenuSideType)
            :base(MenuSideType)
        {

        }

        public override void Open() { HasExited = true; }
    }
}
