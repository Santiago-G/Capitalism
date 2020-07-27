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
        Button SquarePlayersConfirmed;
        

        SpriteFont playerCountFont;
        SpriteFont startingScreenFont;

        Animation RedDice1;
        Animation RedDice2;

        bool realStartingScreen = true;
        bool PlayerScreen = false;
        bool diceScreen = false;

        int playerCount = 2;
        Random gen;
        public Starting_Screen(ContentManager Content)
        {
            gen = new Random();
            Logo = Content.Load<Texture2D>("MonopolyLogo");
            Texture2D Square1Image = Content.Load<Texture2D>("WhiteSquare");
            Texture2D RedDiceImage = Content.Load<Texture2D>("RedDice");

            SquareStart = new Button(Square1Image, new Vector2(30, 200), Color.Red);
            SquareRules = new Button(Square1Image, new Vector2(30, 350), Color.Red);
            SquareOptions = new Button(Square1Image, new Vector2(30, 500), Color.Red);

            SquarePlayerCount = new Button(Square1Image, new Vector2(150, 250), Color.Red);
            SquarePlayersConfirmed = new Button(Square1Image, new Vector2(300, 500), Color.Green);

            playerCountFont = Content.Load<SpriteFont>("PlayerCountFont");
            startingScreenFont = Content.Load<SpriteFont>("startingScreenFont");

            RedDice1 = new Animation(RedDiceImage, new Vector2(200, 200), 200, new Random(gen.Next()));
            RedDice2 = new Animation(RedDiceImage, new Vector2(400, 200), 200, new Random(gen.Next()));
        }

        public void Update(MouseState ms, GameTime gameTime)
        {
            ButtonState oldState = ButtonState.Released;

            SquareStart.Update(ms, true);
            SquareRules.Update(ms, true);
            SquareOptions.Update(ms, true);
            SquarePlayerCount.Update(ms, false);
            SquarePlayersConfirmed.Update(ms, false);

            if (SquareStart.Hitbox.Contains(ms.Position) && ms.LeftButton == ButtonState.Pressed && oldState == ButtonState.Released)
            {
                //**its gaming time**

                PlayerScreen = true;
                realStartingScreen = false;


            }

            if (PlayerScreen)
            {
                if (SquarePlayerCount.IsClicked)
                {
                    playerCount += 1;
                    
                    if (playerCount == 9)
                    {
                        playerCount = 2;
                    }
                }

                if (SquarePlayersConfirmed.IsClicked)
                {
                    //the number of players have been selected, time to make more ifs.

                    PlayerScreen = false;
                    diceScreen = true;
                }
            } // Selecting The Number Of Players

            if (diceScreen)
            {
                RedDice1.Update(gameTime, true);
                RedDice2.Update(gameTime, true);
            } //Choosing Who Goes First


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (realStartingScreen)
            {
                spriteBatch.Draw(Logo, new Vector2(30, 10), Color.White);
                SquareStart.Draw(spriteBatch);
                SquareRules.Draw(spriteBatch);
                SquareOptions.Draw(spriteBatch);

                spriteBatch.DrawString(startingScreenFont, "Start", new Vector2(300, 220), Color.Black);
                spriteBatch.DrawString(startingScreenFont, "Rules", new Vector2(295, 370), Color.Black);
                spriteBatch.DrawString(startingScreenFont, "Options", new Vector2(275, 520), Color.Black);
            }
            else if (PlayerScreen)
            {
                SquarePlayerCount.Draw(spriteBatch);
                SquarePlayersConfirmed.Draw(spriteBatch);

                spriteBatch.DrawString(playerCountFont, playerCount.ToString(), new Vector2(350, 250), Color.Black);
            }
            else if (diceScreen)
            {
                RedDice1.Draw(spriteBatch);
                RedDice2.Draw(spriteBatch);
            }
        }
    }
}
