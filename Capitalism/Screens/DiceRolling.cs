using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capitalism.Screens
{
    public class DiceRolling : Screen
    {
        Texture2D Square2Image;
        Texture2D Crown;

        Button SquarePlayersConfirmed;
        Button SquareDiceRoll;

        SpriteFont smallSizeFont;

        Animation RedDice1;
        Animation RedDice2;


        int currentPlayer = 1;
        int currentHighestPlayer = 0;

        bool rollDice = false;
        bool ictoastiwdteuswoaiwynictoatmhtohyswoatlwy = true;

        public bool ScreenEnded = false;

        public int playerCount = 2;

        int greenSquareX = 110;
        int highestDiceRoll = 0;
        int greenSquareY = 0;

        Random gen;

        public DiceRolling(string Name) : base(Name)
        { 
        
        }

        public override void LoadContent(ContentManager Content)
        {
            gen = new Random();

            Texture2D Square1Image = Content.Load<Texture2D>("WhiteSquare");

            Square2Image = Content.Load<Texture2D>("SmallerWhiteSquare");
            Texture2D RedDiceImage = Content.Load<Texture2D>("RedDice");
            Crown = Content.Load<Texture2D>("SmallCrown");

            SquareDiceRoll = new Button(Square1Image, new Vector2(315, 400), Color.Green);

            smallSizeFont = Content.Load<SpriteFont>("smallSize");

            RedDice1 = new Animation(RedDiceImage, new Vector2(200, 200), 100, new Random(gen.Next()));
            RedDice2 = new Animation(RedDiceImage, new Vector2(400, 200), 100, new Random(gen.Next()));

        }

        public override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();

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
                        if (currentPlayer < playerCount)
                        {
                            greenSquareY += 1;
                        }
                        else
                        {
                            ScreenEnded = true;
                        }


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
        }

        public override void Draw(SpriteBatch spriteBatch)
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
