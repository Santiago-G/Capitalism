using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capitalism
{
    public class HighlightButton : Sprite
    {

        public Rectangle HighlightedHitbox
        {
            get
            {
                double scaleValue =0.1;
                int newSize = (int)(this.Hitbox.Width * scaleValue);

                //int 
                //// NOTE: MOVE THE BOX BACK BY THE NEW SIZE AMOUNT SO ITS BASED OFF THE CENTER

                return new Rectangle(this.Hitbox.X - (int)(this.Hitbox.Height * 0.05), this.Hitbox.Y - (int)(this.Hitbox.Width * 0.05), (int)(this.Hitbox.Width * 1.1), (int)(this.Hitbox.Height * 1.1));
                
            }
        }


        public override Rectangle Hitbox 
        { 
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, (int)(Image.Width * scale.X), (int)(Image.Height * scale.Y));
            }
        }

        public Rectangle CurrentHitbox;
        public Color CurrentTint;

        MouseState mouseState;

        Vector2 scale { get; set; } = new Vector2(1,1);

        public bool IsClicked { get; private set; }

        public HighlightButton(Texture2D image, Vector2 position, Color tint, Vector2 Scale) : base(image, position, tint)
        {
            scale = Scale;
        }

        public void Update(MouseState ms, bool toHighlight)
        {
            IsClicked = false;

            if (toHighlight)
            {
                if (Hitbox.Contains(ms.Position)) 
                {
                    CurrentHitbox = HighlightedHitbox;
                    CurrentTint = Color.Gold;
                }
                else
                {
                    CurrentHitbox = Hitbox;
                    CurrentTint = Tint;
                }
            }
            else
            {
                CurrentHitbox = Hitbox;
                CurrentTint = Tint;
            }

            if (CurrentHitbox.Contains(ms.Position))
            {
                if ((ms != mouseState && ms.LeftButton == ButtonState.Pressed)) // click logic goes in here
                {
                    IsClicked = true;
                }
                else 
                {
                    IsClicked = false;
                }
            }

            mouseState = ms;
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(this.Image, CurrentHitbox, CurrentTint);
            //base.Draw(batch);
        }
    }
}
