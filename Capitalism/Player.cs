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
        public float Size;
        public Color Tint;
        public Rectangle Hitbox => new Rectangle((int)Position.X, (int)Position.Y, (int)(Image.Width * Size), (int)(Image.Height * Size));

        //yet to be decided if I use a set of bills, or I just do the number.
        public string Token = "";

        public Player(Texture2D image, Vector2 position, Color tint, string token, int size)
        {
            Position = position;
            Image = image;
            Tint = tint;
            Token = token;
            Size = size;
        }

        public void Update()
        { 
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture: Image,destinationRectangle: Hitbox, color: Tint, rotation: 0, origin: new Vector2(Hitbox.Width/2, Hitbox.Height/2), effects: default, layerDepth: default);
        }//(int)(Image.Width * Size)/ 2, (int)(Image.Height * Size)/ 2
    }
}
