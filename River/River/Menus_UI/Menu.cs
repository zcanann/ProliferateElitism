using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace River
{
    class Menu
    {
        protected MenuSideType MenuType;

        public Menu(MenuSideType MenuSideType)
        {
            this.MenuType = MenuSideType;
        }

        public MenuSideType GetMenuType()
        {
            return MenuType;
        }

        //Simply here to be overwritten
        public virtual void Open() { }
        public virtual void Update(GameTime GameTime) { }
        public virtual void Draw(SpriteBatch SpriteBatch) { }

    }
}
