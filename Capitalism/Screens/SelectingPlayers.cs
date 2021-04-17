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
    public class SelectingPlayers : Screen
    {
        HighlightButton SquarePlayerCount;
        HighlightButton SquarePlayersConfirmed;

        SpriteFont playerCountFont;
        SpriteFont mediumSizeFont;

        public static int playerCount = 2;

        public SelectingPlayers(string Name) : base(Name)
        {

        }

        public override void LoadContent(ContentManager Content)
        {
            this.EndScreen = false;

            Texture2D Square1Image = Content.Load<Texture2D>("WhiteSquare");

            playerCountFont = Content.Load<SpriteFont>("PlayerCountFont");
            mediumSizeFont = Content.Load<SpriteFont>("MediumSize");

            SquarePlayerCount = new HighlightButton(Square1Image, new Vector2(150, 250), Color.Red, Vector2.One);
            SquarePlayersConfirmed = new HighlightButton(Square1Image, new Vector2(300, 500), Color.Green, Vector2.One);
        }

        public override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();

            SquarePlayerCount.Update(ms, true);
            SquarePlayersConfirmed.Update(ms, true);

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

                this.EndScreen = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            SquarePlayerCount.Draw(spriteBatch);
            SquarePlayersConfirmed.Draw(spriteBatch);

            spriteBatch.DrawString(playerCountFont, playerCount.ToString(), new Vector2(350, 250), Color.Black);
            spriteBatch.DrawString(mediumSizeFont, "Press the red button to select", new Vector2(130, 100), Color.Black);
            spriteBatch.DrawString(mediumSizeFont, "how many players will be playing", new Vector2(100, 140), Color.Black);
        }
    }
}
