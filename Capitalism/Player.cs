using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capitalism
{
    public class Player
    {
        public Texture2D Image;
        public Vector2 Position;
        public Color Tint;
        public Rectangle Hitbox => new Rectangle((int)Position.X, (int)Position.Y, Image.Width, Image.Height);

        //yet to be decided if I use a set of bills, or I just do the number.
        public string Token = "";

        public Player(Texture2D image, Vector2 position, Color tint, string token)
        {
            Position = position;
            Image = image;
            Tint = tint;
            Token = token;
        }

        public void Update()
        { 
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, Position, Tint);
        }
    }
}
