using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Capitalism.Screens
{
    public class ChoosingCharacters : Screen
    {
        SpriteFont playerFont;

        Texture2D VroomVroom;
        Texture2D AAAANNNNNNNNNDDDDDDDDD_THE_TITANIC_HAS_SUNK_LADIES_AND_GENTLEMEN;
        Texture2D good_day_sir;
        Texture2D ew;
        Texture2D yes;
        Texture2D discount_cart;
        Texture2D the_shape_of_italy;
        Texture2D black_magic;
        Texture2D F_O_O_D;
        Texture2D weakling;

        public int playerCount => SelectingPlayers.playerCount;
        int currentPlayer = 1;

        // LIST OF PLAYERS  - Car, Ship, Top Hat, Cat, Dog, WheelBarrow, Boot, Plane, Hamburger, duck

        public ChoosingCharacters(string Name) : base(Name)
        { 
        
        }

        public override void LoadContent(ContentManager Content)
        {
            playerFont = Content.Load<SpriteFont>("startingScreenFont");

            VroomVroom = Content.Load<Texture2D>("car");
            AAAANNNNNNNNNDDDDDDDDD_THE_TITANIC_HAS_SUNK_LADIES_AND_GENTLEMEN = Content.Load<Texture2D>("boat");

            weakling = Content.Load<Texture2D>("duck");

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
