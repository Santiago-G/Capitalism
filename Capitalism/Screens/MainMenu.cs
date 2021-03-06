﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Capitalism.Screens
{
    public class MainMenu : Screen
    {
        HighlightButton SquareStart;
        HighlightButton SquareRules;
        HighlightButton SquareOptions;

        SpriteFont startingScreenFont;

        Texture2D Logo;

        public MainMenu(string Name) : base (Name)
        { 
            
        }

        public override void LoadContent(ContentManager Content)
        {
            this.EndScreen = false;

            Texture2D Square1Image = Content.Load<Texture2D>("WhiteSquare");
            Logo = Content.Load<Texture2D>("MonopolyLogo");

            startingScreenFont = Content.Load<SpriteFont>("startingScreenFont");

            SquareStart = new HighlightButton(Square1Image, new Vector2(30, 200), Color.Red, Vector2.One);
            SquareRules = new HighlightButton(Square1Image, new Vector2(30, 350), Color.Red, Vector2.One);
            SquareOptions = new HighlightButton(Square1Image, new Vector2(30, 500), Color.Red, Vector2.One);
        }

        public override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();

            SquareStart.Update(ms, true);
            SquareRules.Update(ms, true);
            SquareOptions.Update(ms, true);

            ButtonState oldState = ButtonState.Released;

            if (SquareStart.Hitbox.Contains(ms.Position) && ms.LeftButton == ButtonState.Pressed && oldState == ButtonState.Released)
            {
                //**its gaming time**

                this.EndScreen = true;

            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Logo, new Vector2(30, 10), Color.White);

            SquareStart.Draw(spriteBatch);
            SquareRules.Draw(spriteBatch);
            SquareOptions.Draw(spriteBatch);

            spriteBatch.DrawString(startingScreenFont, "Start", new Vector2(300, 220), Color.Black);
            spriteBatch.DrawString(startingScreenFont, "Rules", new Vector2(295, 370), Color.Black);
            spriteBatch.DrawString(startingScreenFont, "Options", new Vector2(275, 520), Color.Black);
        }
    }


}
