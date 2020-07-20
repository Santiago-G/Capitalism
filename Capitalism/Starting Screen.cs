using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capitalism
{
    public class Starting_Screen
    {
        Texture2D Logo;

        Button SquareStart;
        Button SquareRules;
        Button SquareOptions;

        Button SquarePlayerCount;

        SpriteFont playerCountFont;

        bool realStartingScreen = true;
        bool PlayerScreen = false;

        int playerCount = 2;
        public Starting_Screen(ContentManager Content)
        {
            Logo = Content.Load<Texture2D>("MonopolyLogo");
            Texture2D Square1Image = Content.Load<Texture2D>("WhiteSquare");

            SquareStart = new Button(Square1Image, new Vector2(30, 200), Color.Red);
            SquareRules = new Button(Square1Image, new Vector2(30, 350), Color.Red);
            SquareOptions = new Button(Square1Image, new Vector2(30, 500), Color.Red);

            SquarePlayerCount = new Button(Square1Image, new Vector2(150, 250), Color.Red);

            playerCountFont = Content.Load<SpriteFont>("PlayerCountFont");
        }

        public void Update(MouseState ms)
        {
            SquareStart.Update(ms, true);
            SquareRules.Update(ms, true);
            SquareOptions.Update(ms, true);
            SquarePlayerCount.Update(ms, false);

            if (SquareStart.Hitbox.Contains(ms.Position) && ms.LeftButton == ButtonState.Pressed)
            {
                //**its gaming time**

                PlayerScreen = true;
                realStartingScreen = false;
                
           

            }

            if (SquarePlayerCount.Hitbox.Contains(ms.Position) && ms.LeftButton == ButtonState.Pressed)
            {
                playerCount += 1;
                ;
                if (playerCount == 9)
                {
                    playerCount = 2;
                }

            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (realStartingScreen)
            {
                spriteBatch.Draw(Logo, new Vector2(30, 10), Color.White);
                SquareStart.Draw(spriteBatch);
                SquareRules.Draw(spriteBatch);
                SquareOptions.Draw(spriteBatch);
            }
            else if (PlayerScreen)
            {
                SquarePlayerCount.Draw(spriteBatch);

                spriteBatch.DrawString(playerCountFont, playerCount.ToString(), new Vector2(350, 250), Color.Black);
            }

        }
    }
}
