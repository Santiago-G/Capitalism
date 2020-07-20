using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;

namespace Capitalism
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D spriteSheet;
        Texture2D BankThing;

        //thy placeholders.

        Board TheBoard;
        Starting_Screen TheStartingScreen;
        Player Players;

        bool StartingScreen = true;

        //List<Screen> screens;
        //Screen currentScreen;
        bool setMainScreenResolution = false;


        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {

            // TODO: Add your initialization logic here

            IsMouseVisible = true;

            //ChangeResolution(1470, 1048);
            ChangeResolution(700, 700);

            //ScreenManager manager = new ScreenManager();
            //manager.Add(new Screen("Loading Screen"))


            // currentScreen = screens[0];


            base.Initialize();
        }

        public void ChangeResolution(int width, int height)
        {
            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
            graphics.ApplyChanges();
        }


        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            spriteSheet = Content.Load<Texture2D>("TheCapitalistBoard");
            BankThing = Content.Load<Texture2D>("BankTitle");


            TheBoard = new Board(Content);
            TheStartingScreen = new Starting_Screen(Content);



            //string[] filecontents = File.ReadAllLines("rules.txt");

            

            // TODO: use this.Content to load your game content here
        }



        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }



        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                
               Exit();
            }


            MouseState Ms = Mouse.GetState();

            //if (Ms.LeftButton == ButtonState.Pressed)
            //{
            //    setMainScreenResolution = true;
            //}

            //currentScreen.Update();

            if (StartingScreen)
            {
                TheStartingScreen.Update(Ms);

                if (setMainScreenResolution == true)
                {
                    ChangeResolution(1470, 1048);

                    setMainScreenResolution = false;
                    StartingScreen = false;
                }
            }
            else
            {
                TheBoard.Update(Ms);
            }


            // TODO: Add your update logic here

            base.Update(gameTime);
        }



        protected override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here
            spriteBatch.Begin();

            //currentScreen.Draw();

            if (StartingScreen)
            {
                GraphicsDevice.Clear(Color.Beige);

                TheStartingScreen.Draw(spriteBatch);



            }

            else
            {
                GraphicsDevice.Clear(Color.FromNonPremultiplied(109, 109, 109, 155));

                spriteBatch.Draw(spriteSheet, new Vector2(410, 0), Color.White);
                spriteBatch.Draw(BankThing, new Vector2(45, 0), Color.White);
                TheBoard.Draw(spriteBatch);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
