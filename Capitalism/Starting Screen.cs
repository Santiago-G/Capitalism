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
        Texture2D Square2Image;
        Texture2D Crown;

        Button SquareStart;
        Button SquareRules;
        Button SquareOptions;

        Button SquarePlayerCount;
        Button SquarePlayersConfirmed;
        Button SquareDiceRoll;

        SpriteFont playerCountFont;
        SpriteFont startingScreenFont;
        SpriteFont mediumSizeFont;
        SpriteFont smallSizeFont;

        Animation RedDice1;
        Animation RedDice2;

        bool realStartingScreen = true;
        bool PlayerScreen = false;
        bool diceScreen = false;
        bool ictoastiwdteuswoaiwynictoatmhtohyswoatlwy = true;

        int currentPlayer = 1;
        int currentHighestPlayer = 0;

        bool rollDice = false;

        public int playerCount = 2;

        int greenSquareX = 110;
        int highestDiceRoll = 0;
        int greenSquareY = 0;


        Random gen;
        public Starting_Screen(ContentManager Content)
        {
            gen = new Random();
            Logo = Content.Load<Texture2D>("MonopolyLogo");
            Texture2D Square1Image = Content.Load<Texture2D>("WhiteSquare");
            Square2Image = Content.Load<Texture2D>("SmallerWhiteSquare");
            Texture2D RedDiceImage = Content.Load<Texture2D>("RedDice");
            Crown = Content.Load<Texture2D>("SmallCrown");


            SquareStart = new Button(Square1Image, new Vector2(30, 200), Color.Red);
            SquareRules = new Button(Square1Image, new Vector2(30, 350), Color.Red);
            SquareOptions = new Button(Square1Image, new Vector2(30, 500), Color.Red);

            SquarePlayerCount = new Button(Square1Image, new Vector2(150, 250), Color.Red);
            SquarePlayersConfirmed = new Button(Square1Image, new Vector2(300, 500), Color.Green);
            SquareDiceRoll = new Button(Square1Image, new Vector2(315, 400), Color.Green);

            playerCountFont = Content.Load<SpriteFont>("PlayerCountFont");
            startingScreenFont = Content.Load<SpriteFont>("startingScreenFont");
            mediumSizeFont = Content.Load<SpriteFont>("MediumSize");
            smallSizeFont = Content.Load<SpriteFont>("smallSize");

            RedDice1 = new Animation(RedDiceImage, new Vector2(200, 200), 100, new Random(gen.Next()));
            RedDice2 = new Animation(RedDiceImage, new Vector2(400, 200), 100, new Random(gen.Next()));

        }

        public void Update(MouseState ms, GameTime gameTime)
        {
            ButtonState oldState = ButtonState.Released;

            Game1.TitleBarString = "";

            SquareStart.Update(ms, true);
            SquareRules.Update(ms, true);
            SquareOptions.Update(ms, true);
            SquarePlayerCount.Update(ms, true);
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
                if (currentPlayer <= playerCount)
                {
                    if (SquareDiceRoll.IsClicked && RedDice1.stopped)
                    {
                        rollDice = true;
                        ictoastiwdteuswoaiwynictoatmhtohyswoatlwy = true;
                        SquareDiceRoll.Update(ms, false);

                        RedDice1.Restart();
                        RedDice2.Restart();
                    }
                    else 
                    {
                        SquareDiceRoll.Update(ms, false);
                    }


                    if (rollDice && ictoastiwdteuswoaiwynictoatmhtohyswoatlwy)
                    {
                        RedDice1.Update(gameTime, true);
                        RedDice2.Update(gameTime, true);

                        if (RedDice1.stopped && ictoastiwdteuswoaiwynictoatmhtohyswoatlwy)
                        {

                                greenSquareY += 1;
       

                            rollDice = false;

                            int diceRollValue = (RedDice1.DiceRollValue + 1) + (RedDice2.DiceRollValue + 1);

                            highestDiceRoll = Math.Max(diceRollValue, highestDiceRoll);

                            if (highestDiceRoll == diceRollValue)
                            {
                                currentHighestPlayer = currentPlayer - 1;
                            }

                            currentPlayer++;
                            ictoastiwdteuswoaiwynictoatmhtohyswoatlwy = false;

                        }
                    }
                }

            } //Choosing Who Goes First


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Dictionary[CurrentScreen].Draw()
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
                spriteBatch.DrawString(mediumSizeFont, "Press the red button to select", new Vector2(130, 100), Color.Black);
                spriteBatch.DrawString(mediumSizeFont, "how many players will be playing", new Vector2(100, 140), Color.Black);
            }
            else if (diceScreen)
            {
                int distance = 50;
                int y = distance - 40;
                for (int i = 0; i < playerCount; i++)
                {
                    spriteBatch.DrawString(smallSizeFont, $"Player {i + 1}", new Vector2(10, y), Color.Black);
                    y += distance;
                }

                spriteBatch.Draw(Square2Image, new Vector2(greenSquareX, 12 + (53 * greenSquareY)), Color.Green);
                spriteBatch.Draw(Crown, new Vector2(150, 10 + (53 * currentHighestPlayer)), Color.White);

                SquareDiceRoll.Draw(spriteBatch);
                RedDice1.Draw(spriteBatch);
                RedDice2.Draw(spriteBatch);
            }
        }
    }
}
