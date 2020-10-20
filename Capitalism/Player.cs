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
        public Vector2[] PositionArea;
        public int currentTileIndex;
        public int Money;

        public Rectangle Hitbox => new Rectangle((int)Position.X, (int)Position.Y, (int)(Image.Width * Size), (int)(Image.Height * Size));

        public string Token = "";

        public Player(Texture2D image, Vector2 position, Color tint, string token, int size, int cash)
        {
            Position = position;
            Image = image;
            Tint = tint;
            Token = token;
            Size = size;
            Money = cash;
        }

        public void Update()
        {
            if (Money == 0)
            { 
                //game over
            }

            //if ()
            //{
            //    Money += 200;
            //}
        }

        public Vector2 GetTilePosition()
        {
            for (int i = 0; i < PositionArea.Length; i++)
            {
                if (Position == PositionArea[i])
                {
                    return PositionArea[i];
                }
            }

            return Vector2.Zero;
        }

        public int GetTileIndex()
        {
            for (int i = 0; i < PositionArea.Length; i++)
            {
                if (Position == PositionArea[i])
                {
                    return i;
                }
            }

            return 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture: Image,destinationRectangle: Hitbox, color: Tint, rotation: 0, origin: new Vector2(Hitbox.Width/2, Hitbox.Height/2), effects: default, layerDepth: default);
        }//(int)(Image.Width * Size)/ 2, (int)(Image.Height * Size)/ 2
    }
}
