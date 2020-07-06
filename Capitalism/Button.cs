using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capitalism
{
    public class Button
    {

        public Texture2D Image;
        public Vector2 Position;
        public Color Tint;
        public Rectangle Hitbox => new Rectangle((int)Position.X, (int)Position.Y, Image.Width, Image.Height);
        

        public Button(Texture2D image, Vector2 position, Color Tint)
        {
            this.Image = image;
            this.Position = position;
            this.Tint = Tint;
        }



        public void Draw(SpriteBatch batch)
        {
            batch.Draw(Image, Position, Tint);
        }
    }
}
