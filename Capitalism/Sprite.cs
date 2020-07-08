using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Capitalism
{
    public abstract class Sprite
    {
        public Texture2D Image;
        public Vector2 Position;
        public Color Tint;
        public Rectangle Hitbox => new Rectangle((int)Position.X, (int)Position.Y, Image.Width, Image.Height);

        public Sprite(Texture2D image, Vector2 position, Color tint)
        {
            Image = image;
            Position = position;
            Tint = tint;
            //Hitbox = hitbox;
        }

        public virtual void Draw(SpriteBatch batch)
        {
            batch.Draw(Image, Position, Tint);
        }
    }
}