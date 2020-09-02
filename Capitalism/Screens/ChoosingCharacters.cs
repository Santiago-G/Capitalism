using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Capitalism.Screens
{
    public class ChoosingCharacters : Screen
    {
        SpriteFont playerFont;
        Texture2D test;
        Texture2D pixel;
        Texture2D yesTexture;
        Texture2D noTexture;

        Button Yes;
        Button No;


        #region Token Setup
        Texture2D VroomVroom;
        Texture2D AAAANNNNNNNNNDDDDDDDDD_THE_TITANIC_HAS_SUNK_LADIES_AND_GENTLEMEN;
        Texture2D good_day_sir;
        Texture2D ew;
        Texture2D yes;
        Texture2D discount_cart;
        Texture2D the_shape_of_italy;
        Texture2D black_magic;
        Texture2D F_O_O_D;
        Texture2D weakling;

        Button Car;
        Button Boat;
        Button Hat;
        Button Cat;
        Button Dog;
        Button Wheelbarrow;
        Button Boot;
        Button Plane;
        Button Duck;

        bool[] alreadySelected = new bool[9];

        #endregion
        public int playerCount => SelectingPlayers.playerCount;
        int currentPlayer = 1;

        bool why = false;
        bool not = true;
        bool areYouDone = false;

        // LIST OF PLAYERS  - Car, Ship, Top Hat, Cat, Dog, WheelBarrow, Boot, Plane, duck

        Dictionary<string, (Player player, Button button)> players = new Dictionary<string, (Player, Button)>();


        public ChoosingCharacters(string Name) : base(Name)
        {
        }

        public override void LoadContent(ContentManager Content)
        {

            for (int i = 0; i < alreadySelected.Length; i++)
            {
                alreadySelected[i] = false;
            }

            playerFont = Content.Load<SpriteFont>("startingScreenFont");

            test = Content.Load<Texture2D>("Test");
            pixel = Content.Load<Texture2D>("pixel");

            yesTexture = Content.Load<Texture2D>("YesTwo");
            Yes = new Button(yesTexture, new Vector2(280,350), Color.White);
            noTexture = Content.Load<Texture2D>("NoTwo");
            No = new Button(noTexture, new Vector2(370, 350), Color.White);

            #region tokens 

            VroomVroom = Content.Load<Texture2D>("car");
            AAAANNNNNNNNNDDDDDDDDD_THE_TITANIC_HAS_SUNK_LADIES_AND_GENTLEMEN = Content.Load<Texture2D>("boat");
            good_day_sir = Content.Load<Texture2D>("hat");
            ew = Content.Load<Texture2D>("cat");
            yes = Content.Load<Texture2D>("dog");
            discount_cart = Content.Load<Texture2D>("wheelbarrow");
            the_shape_of_italy = Content.Load<Texture2D>("boot");
            //Plane
            weakling = Content.Load<Texture2D>("duck");


            Car = new Button(VroomVroom, new Vector2(50, 120), Color.White);
            Boat = new Button(AAAANNNNNNNNNDDDDDDDDD_THE_TITANIC_HAS_SUNK_LADIES_AND_GENTLEMEN, new Vector2(260, 100), Color.White);
            Hat = new Button(good_day_sir, new Vector2(480, 130), Color.White);
            Cat = new Button(ew, new Vector2(50, 260), Color.White);
            Dog = new Button(yes, new Vector2(260, 260), Color.White);
            Wheelbarrow = new Button(discount_cart, new Vector2(480, 320), Color.White);
            Boot = new Button(the_shape_of_italy, new Vector2(50, 470), Color.White);
            //Plane

            Duck = new Button(weakling, new Vector2(480, 470), Color.White);
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
                    players.Add($"Player {currentPlayer}, Car", (new Player(VroomVroom, new Vector2(1000), Color.White, "Car"), Car));
                    alreadySelected[0] = true;

                    why = true;
                }

                if (Boat.IsClicked && !alreadySelected[1])
                {
                    players.Add($"Player {currentPlayer}, Boat", (new Player(AAAANNNNNNNNNDDDDDDDDD_THE_TITANIC_HAS_SUNK_LADIES_AND_GENTLEMEN, new Vector2(1000), Color.White, "Boat"), Boat));
                    alreadySelected[1] = true;

                    why = true;
                }

                if (Hat.IsClicked && !alreadySelected[2])
                {
                    players.Add($"Player {currentPlayer}, Hat", (new Player(good_day_sir, new Vector2(1000), Color.White, "Hat"), Hat));
                    alreadySelected[2] = true;

                    why = true;
                }

                if (Cat.IsClicked && !alreadySelected[3])
                {
                    players.Add($"Player {currentPlayer}, Cat", (new Player(ew, new Vector2(1000), Color.White, "Cat"), Cat));
                    alreadySelected[3] = true;

                    why = true;
                }

                if (Dog.IsClicked && !alreadySelected[4])
                {
                    players.Add($"Player {currentPlayer}, Dog", (new Player(yes, new Vector2(1000), Color.White, "Dog"), Dog));
                    alreadySelected[4] = true;

                    why = true;
                }

                if (Wheelbarrow.IsClicked && !alreadySelected[5])
                {
                    players.Add($"Player {currentPlayer}, Wheelbarrow", (new Player(discount_cart, new Vector2(1000), Color.White, "Wheelbarrow"), Wheelbarrow));
                    alreadySelected[5] = true;

                    why = true;
                }

                if (Boot.IsClicked && !alreadySelected[6])
                {
                    players.Add($"Player {currentPlayer}, Boot", (new Player(the_shape_of_italy, new Vector2(1000), Color.White, "Boot"), Boot));
                    alreadySelected[6] = true;

                    why = true;
                }

                if (Duck.IsClicked && !alreadySelected[8])
                {
                    players.Add($"Player {currentPlayer}, Duck", (new Player(weakling, new Vector2(1000), Color.White, "Duck"), Duck));
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
