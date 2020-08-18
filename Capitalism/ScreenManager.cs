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

        Queue<Screen> Screens;
        public Screen currentScreen => Screens.Peek();
        public Screen lastScreen => Screens.Last();
        public ScreenManager()
        {
            Screens = new Queue<Screen>();
        }

        public void AddScreen(Screen newScreen)
        {
            Screens.Enqueue(newScreen);
        }

        public void NextScreen()
        {
            Screens.Dequeue();
        }

        public Screen GetScreenByName(string name)
        {
            foreach (var screen in Screens)
            {
                if (screen.Name == name)
                {
                    return screen;
                }
            }

            return null;
        }


        #region Load, Update, and Draw

        public void LoadContent(ContentManager Content)
        {
            lastScreen.LoadContent(Content);
        }

        public void Update(GameTime gameTime)
        {
            currentScreen.Update(gameTime);

            if (currentScreen.EndScreen)
            {
                NextScreen();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            currentScreen.Draw(spriteBatch);
        }

        #endregion
    }
}
