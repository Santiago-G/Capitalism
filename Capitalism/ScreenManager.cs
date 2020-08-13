using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capitalism
{
    public class ScreenManager
    {

        Stack<Screen> Screens;
        public Screen currentScreen => Screens.Peek();
        public ScreenManager()
        {
            Screens = new Stack<Screen>();
        }

        public void AddScreen(Screen newScreen)
        {
            Screens.Push(newScreen);
        }

        public void NextScreen()
        {
            Screens.Pop();
        }


        #region Load, Update, and Draw

        public void LoadContent(ContentManager Content)
        {
            currentScreen.LoadContent(Content);
        }

        public void Update(GameTime gameTime)
        {
            currentScreen.Update(gameTime);

            if (currentScreen.e)
            { 
            
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            currentScreen.Draw(spriteBatch);
        }

        #endregion
    }
}
