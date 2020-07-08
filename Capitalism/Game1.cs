using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Capitalism
{
    // TODO LIST
    /*
     * Make Sprites for the characters
     * IF the Sprites for the cards aren't fixed, FIX THEM
     * Finish the positions with the right Sprites
     * Either make a loading screen asking how many players there are
       OR add the money system
     * 
     * 
     * 
     * 
     */
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D spriteSheet;
        Texture2D BankThing;

        Board TheBoard;
        Player Players;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {

            // TODO: Add your initialization logic here

            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 1470;
            graphics.PreferredBackBufferHeight = 1048;
            graphics.ApplyChanges();

            base.Initialize();
        }


        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            spriteSheet = Content.Load<Texture2D>("TheCapitalistBoard");
            BankThing = Content.Load<Texture2D>("BankTitle");
            TheBoard = new Board(Content);

            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            MouseState Ms = Mouse.GetState();
            TheBoard.Update(Ms);

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.FromNonPremultiplied(109, 109, 109, 155));

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            spriteBatch.Draw(spriteSheet, new Vector2(410, 0), Color.White);
            spriteBatch.Draw(BankThing, new Vector2(45, 0), Color.White);
            TheBoard.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
