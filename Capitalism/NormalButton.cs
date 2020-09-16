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
    public class NormalButton : Sprite
    {
        public override Rectangle Hitbox { get; set; }

        public NormalButton(Texture2D image, Vector2 position, Color tint) : base(image, position, tint)
        {
        }

        public bool IsClicked(MouseState ms)
        {
            if (Hitbox.Contains(ms.Position) && ms.LeftButton == ButtonState.Pressed)
            {
                return true;
            }

            return false;
        }

        public void Update(MouseState ms)
        {
            IsClicked(ms);
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(this.Image, Hitbox, Tint);
        }
    }
}
