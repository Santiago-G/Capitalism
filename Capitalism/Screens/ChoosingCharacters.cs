using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Capitalism.Screens
{
    public class ChoosingCharacters : Screen
    {
        SpriteFont playerFont;
        Texture2D test;
        Texture2D pixel;
        Texture2D yesTexture;
        Texture2D noTexture;

        HighlightButton Yes;
        HighlightButton No;

        #region Token Setup
        Texture2D VroomVroom;
        Texture2D titanic;
        Texture2D good_day_sir;
        Texture2D ew;
        Texture2D yes;
        Texture2D discount_cart;
        Texture2D the_shape_of_italy;
        Texture2D weakling;

        HighlightButton Car;
        HighlightButton Boat;
        HighlightButton Hat;
        HighlightButton Cat;
        HighlightButton Dog;
        HighlightButton Wheelbarrow;
        HighlightButton Boot;
        HighlightButton Duck;

        bool[] alreadySelected = new bool[9];

        #endregion
        public int playerCount => SelectingPlayers.playerCount;
        int currentPlayer = 1;

        bool why = false;
        bool not = true;
        bool areYouDone = false;
        

        // LIST OF PLAYERS  - Car, Ship, Top Hat, Cat, Dog, WheelBarrow, Boot, Plane, duck

        public static Dictionary<string, (Player player, HighlightButton button)> players = new Dictionary<string, (Player, HighlightButton)>();
        
        public ChoosingCharacters(string Name) : base(Name)
        {
        }

        public override void LoadContent(ContentManager Content)
        {
            this.EndScreen = false;
            for (int i = 0; i < alreadySelected.Length; i++)
            {
                alreadySelected[i] = false;
            }

            playerFont = Content.Load<SpriteFont>("startingScreenFont");

            test = Content.Load<Texture2D>("Test");
            pixel = Content.Load<Texture2D>("pixel");

            yesTexture = Content.Load<Texture2D>("YesTwo");
            Yes = new HighlightButton(yesTexture, new Vector2(280,350), Color.White, Vector2.One);
            noTexture = Content.Load<Texture2D>("NoTwo");
            No = new HighlightButton(noTexture, new Vector2(370, 350), Color.White, Vector2.One);

            #region tokens 

            VroomVroom = Content.Load<Texture2D>("car");
            titanic = Content.Load<Texture2D>("boat");
            good_day_sir = Content.Load<Texture2D>("hat");
            ew = Content.Load<Texture2D>("cat");
            yes = Content.Load<Texture2D>("dog");
            discount_cart = Content.Load<Texture2D>("wheelbarrow");
            the_shape_of_italy = Content.Load<Texture2D>("boot");
            //Plane
            weakling = Content.Load<Texture2D>("duck");


            Car = new HighlightButton(VroomVroom, new Vector2(50, 120), Color.White, Vector2.One);
            Boat = new HighlightButton(titanic, new Vector2(260, 100), Color.White, Vector2.One);
            Hat = new HighlightButton(good_day_sir, new Vector2(480, 130), Color.White, Vector2.One);
            Cat = new HighlightButton(ew, new Vector2(50, 260), Color.White, Vector2.One);
            Dog = new HighlightButton(yes, new Vector2(260, 260), Color.White, Vector2.One);
            Wheelbarrow = new HighlightButton(discount_cart, new Vector2(480, 320), Color.White, Vector2.One);
            Boot = new HighlightButton(the_shape_of_italy, new Vector2(50, 470), Color.White, Vector2.One);
            //Plane

            Duck = new HighlightButton(weakling, new Vector2(480, 470), Color.White, Vector2.One);
            #endregion
        }


        public override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();

            //foreach (var pair in players)
            //{
            //    pair.Value.Update();
            //}


            Car.Update(ms, !alreadySelected[0]);
            Boat.Update(ms, !alreadySelected[1]);
            Hat.Update(ms, !alreadySelected[2]);
            Cat.Update(ms, !alreadySelected[3]);
            Dog.Update(ms, !alreadySelected[4]);
            Wheelbarrow.Update(ms, !alreadySelected[5]);
            Boot.Update(ms, !alreadySelected[6]);


            Duck.Update(ms, !alreadySelected[8]);


            if (currentPlayer <= playerCount && not)
            {

                if (Car.IsClicked && !alreadySelected[0])
                {

                    players.Add($"Player {currentPlayer}", (new Player(VroomVroom, new Vector2(1000), Color.White, "Car", 1, 1500), Car));
                    alreadySelected[0] = true;

                    why = true;
                }

                if (Boat.IsClicked && !alreadySelected[1])
                {
                    players.Add($"Player {currentPlayer}", (new Player(titanic, new Vector2(1000), Color.White, "Boat", 1, 1500), Boat));
                    alreadySelected[1] = true;

                    why = true;
                }

                if (Hat.IsClicked && !alreadySelected[2])
                {
                    players.Add($"Player {currentPlayer}", (new Player(good_day_sir, new Vector2(1000), Color.White, "Hat", 1, 1500), Hat));
                    alreadySelected[2] = true;

                    why = true;
                }

                if (Cat.IsClicked && !alreadySelected[3])
                {
                    players.Add($"Player {currentPlayer}", (new Player(ew, new Vector2(1000), Color.White, "Cat", 1, 1500), Cat));
                    alreadySelected[3] = true;

                    why = true;
                }

                if (Dog.IsClicked && !alreadySelected[4])
                {
                    players.Add($"Player {currentPlayer}", (new Player(yes, new Vector2(1000), Color.White, "Dog", 1, 1500), Dog));
                    alreadySelected[4] = true;

                    why = true;
                }

                if (Wheelbarrow.IsClicked && !alreadySelected[5])
                {
                    players.Add($"Player {currentPlayer}", (new Player(discount_cart, new Vector2(1000), Color.White, "Wheelbarrow", 1, 1500), Wheelbarrow));
                    alreadySelected[5] = true;

                    why = true;
                }

                if (Boot.IsClicked && !alreadySelected[6])
                {
                    players.Add($"Player {currentPlayer}", (new Player(the_shape_of_italy, new Vector2(1000), Color.White, "Boot", 1, 1500), Boot));
                    alreadySelected[6] = true;

                    why = true;
                }

                if (Duck.IsClicked && !alreadySelected[8])
                {
                    players.Add($"Player {currentPlayer}", (new Player(weakling, new Vector2(1000), Color.White, "Duck", 1, 1500), Duck));
                    alreadySelected[8] = true;

                    why = true;
                }

                if (why && currentPlayer < playerCount)
                {
                    currentPlayer++;
                    why = false;
                }
                else if (why && currentPlayer == playerCount)
                {
                    not = false;
                }

                
            }
            else
            {
                Yes.Update(ms, false);
                No.Update(ms, false);

                if (Yes.IsClicked)
                {
                    this.EndScreen = true;
                }
                if (No.IsClicked)
                { 
                    //sinners
                }

                areYouDone = true;
                for (int i = 0; i < alreadySelected.Length; i++)
                {
                    alreadySelected[i] = true;
                }
            }


        }
        public override void Draw(SpriteBatch spritebatch)
        {
            spritebatch.DrawString(playerFont, $"Player {currentPlayer}'s turn", new Vector2(200, 30), Color.Black);
            Car.Draw(spritebatch);
            Boat.Draw(spritebatch);
            Hat.Draw(spritebatch);
            Cat.Draw(spritebatch);
            Dog.Draw(spritebatch);
            Wheelbarrow.Draw(spritebatch);
            Boot.Draw(spritebatch);

            Duck.Draw(spritebatch);

            if (areYouDone)
            {

                spritebatch.Draw(pixel, new Rectangle(0, 0, 700, 700), new Color(Color.DarkGreen, 0.5f));
                spritebatch.Draw(test, new Vector2(100, 250), Color.White);
                spritebatch.DrawString(playerFont, "Ready to Start?", new Vector2(200, 280), Color.Black);
                Yes.Draw(spritebatch);
                No.Draw(spritebatch);
            }



        }

    }
}
