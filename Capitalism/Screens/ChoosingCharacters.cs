using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Capitalism.Screens
{
    public class ChoosingCharacters : Screen
    {
        SpriteFont playerFont;

        public int playerCount => SelectingPlayers.playerCount;
        int currentPlayer = 1;

        // LIST OF PLAYERS  - Car, Ship, Top Hat, Ice Skate, Cat, Dog, WheelBarrow, Boot, Plane, Hamburger.

        public ChoosingCharacters(string Name) : base(Name)
        { 
        
        }

        public override void LoadContent(ContentManager Content)
        {
            playerFont = Content.Load<SpriteFont>("startingScreenFont");

        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
        public override void Draw(SpriteBatch spritebatch)
        {
            spritebatch.DrawString(playerFont, $"{currentPlayer}'s turn", new Vector2(100, 100), Color.Black);
        }

    }
}
