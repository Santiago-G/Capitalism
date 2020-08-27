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

        List<Player> Players = new List<Player>();

        // LIST OF PLAYERS  - Car, Ship, Top Hat, Cat, Dog, WheelBarrow, Boot, Plane, Hamburger, duck

        Dictionary<string, (Player player, Button button)> players = new Dictionary<string, (Player, Button)>();

        public ChoosingCharacters(string Name) : base(Name)
        {

        }

        public override void LoadContent(ContentManager Content)
        {

            for (int i = 0; i < alreadySelected.Length; i++)
            {
                alreadySelected[i] = true;
            }

            playerFont = Content.Load<SpriteFont>("startingScreenFont");



            #region tokens 

            VroomVroom = Content.Load<Texture2D>("car");
            AAAANNNNNNNNNDDDDDDDDD_THE_TITANIC_HAS_SUNK_LADIES_AND_GENTLEMEN = Content.Load<Texture2D>("boat");
            good_day_sir = Content.Load<Texture2D>("hat");
            ew = Content.Load<Texture2D>("cat");
            discount_cart = Content.Load<Texture2D>("wheelbarrow");
            the_shape_of_italy = Content.Load<Texture2D>("boot");


            weakling = Content.Load<Texture2D>("duck");

            Car = new Button(VroomVroom, new Vector2(50, 120), Color.White);
            Boat = new Button(AAAANNNNNNNNNDDDDDDDDD_THE_TITANIC_HAS_SUNK_LADIES_AND_GENTLEMEN, new Vector2(260, 100), Color.White);
            Hat = new Button(good_day_sir, new Vector2(480, 130), Color.White);
            Cat = new Button(ew, new Vector2(50, 260), Color.White);
            Wheelbarrow = new Button(discount_cart, new Vector2(260, 320), Color.White);
            Boot = new Button(the_shape_of_italy, new Vector2(50, 470), Color.White);


            Duck = new Button(weakling, new Vector2(480, 470), Color.White);
            #endregion

            players.Add("Car", (Players[0], new Button(VroomVroom, new Vector2(50, 120), Color.White)));
            players.Add("Ship", (Players[1], new Button(AAAANNNNNNNNNDDDDDDDDD_THE_TITANIC_HAS_SUNK_LADIES_AND_GENTLEMEN, new Vector2(260, 100), Color.White)));
            players.Add("Hat", (Players[2], new Button(VroomVroom, new Vector2(50, 120), Color.White)));

        }


        public override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();

            //foreach (var pair in players)
            //{
            //    pair.Value.Update();
            //}


            Car.Update(ms, alreadySelected[0]);
            Boat.Update(ms, alreadySelected[1]);
            Hat.Update(ms, alreadySelected[2]);
            Cat.Update(ms, alreadySelected[3]);
            Wheelbarrow.Update(ms, alreadySelected[5]);
            Boot.Update(ms, alreadySelected[6]);


            Duck.Update(ms, alreadySelected[8]);



            if (Car.IsClicked)
            {
                Players.Add(new Player(VroomVroom, new Vector2(1000), Color.White, "Car"));
                alreadySelected[0] = false;
            }

            if (Boat.IsClicked)
            {
                Players.Add(new Player(VroomVroom, new Vector2(1000), Color.White, "Car"));
                alreadySelected[0] = false;
            }

            if (Hat.IsClicked)
            {
                Players.Add(new Player(VroomVroom, new Vector2(1000), Color.White, "Car"));
                alreadySelected[0] = false;
            }

            if (Cat.IsClicked)
            {
                Players.Add(new Player(VroomVroom, new Vector2(1000), Color.White, "Car"));
                alreadySelected[0] = false;
            }

            if (Wheelbarrow.IsClicked)
            {
                Players.Add(new Player(VroomVroom, new Vector2(1000), Color.White, "Car"));
                alreadySelected[0] = false;
            }

            if (Boot.IsClicked)
            {
                Players.Add(new Player(VroomVroom, new Vector2(1000), Color.White, "Car"));
                alreadySelected[0] = false;
            }

            if (Car.IsClicked)
            {
                Players.Add(new Player(VroomVroom, new Vector2(1000), Color.White, "Car"));
                alreadySelected[0] = false;
            }

            if (Car.IsClicked)
            {
                Players.Add(new Player(VroomVroom, new Vector2(1000), Color.White, "Car"));
                alreadySelected[0] = false;
            }

            if (Duck.IsClicked)
            {
                Players.Add(new Player(VroomVroom, new Vector2(1000), Color.White, "Car"));
                alreadySelected[0] = false;
            }


        }
        public override void Draw(SpriteBatch spritebatch)
        {
            spritebatch.DrawString(playerFont, $"Player {currentPlayer}'s turn", new Vector2(200, 30), Color.Black);
            Car.Draw(spritebatch);
            Boat.Draw(spritebatch);
            Hat.Draw(spritebatch);
            Cat.Draw(spritebatch);

            Wheelbarrow.Draw(spritebatch);
            Boot.Draw(spritebatch);

            Duck.Draw(spritebatch);
        }

    }
}
