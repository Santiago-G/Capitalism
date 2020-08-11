using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capitalism
{
    public class ScreenManager
    {

        public ScreenManager()
        {
            
        }

        Stack<Screen> Screens = new Stack<Screen>();

        public void AddScreen(string Name)
        {
            Screens.Push(Name);
        }


    }
}
