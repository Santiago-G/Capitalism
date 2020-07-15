using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capitalism
{
    public class Starting_Screen
    {
        Texture2D Logo;

        Button Square1;
        Button Square2;
        Button Square3;



        public Starting_Screen(ContentManager Content)
        {
            Logo = Content.Load<Texture2D>("MonopolyLogo");
            Texture2D Square1Image = Content.Load<Texture2D>("WhiteSquare");
            Texture2D Square2Image = Content.Load<Texture2D>("WhiteSquare");
            Texture2D Square3Image = Content.Load<Texture2D>("WhiteSquare");

            Square1 = new Button(Square1Image, new Vector2(30, 200), Color.Red);
            Square2 = new Button(Square2Image, new Vector2(30, 350), Color.Red);
            Square3 = new Button(Square3Image, new Vector2(30, 500), Color.Red);
        }

        public void Update(MouseState ms)
        {
            Square1.Update(ms);
            Square2.Update(ms);
            Square3.Update(ms);

            // && ms.LeftButton == ButtonState.Pressed
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Logo, new Vector2(30, 10), Color.White);
            Square1.Draw(spriteBatch);
            Square2.Draw(spriteBatch);
            Square3.Draw(spriteBatch);

        }
    }
}
