﻿using Capitalism.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Capitalism
{
    public class Board
    {


        Random gen = new Random();

        bool showingDice = false;
        bool diceRolling = false;
        bool diceMoving = false;
        bool darkenScreen = false;
        bool characterMoving = false;
        bool bean = true;

        int rollValue = 0;
        int currentPlayerIndex = 0;
        public int playerCount => SelectingPlayers.playerCount;

        Vector2[] charPostitions;
        Vector2[] goPositions;

        Player[] Players;

        Dictionary<string, (Player player, HighlightButton button)> playerDict = ChoosingCharacters.players;
        Player CurrentPlayer;



        #region Functions

        static public Vector2[] MakingPositions()
        {
            //there are 40 positions
            Vector2[] Positions = new Vector2[40];

            float tempPlayer1X = 1295;
            float tempPlayer1Y = 920;

            for (int i = 0; i < 10; i++)
            {
                if (i != 0)
                {
                    tempPlayer1X = tempPlayer1X - 75.7f;
                }

                Positions[i] = new Vector2(tempPlayer1X, tempPlayer1Y);   
            }

            Positions[10] = new Vector2(545, 970);
            tempPlayer1X = 500;
            tempPlayer1Y = 890.7f;

            for (int i = 11; i < 20; i++)
            {
                tempPlayer1Y = tempPlayer1Y - 75.7f;
                Positions[i] = new Vector2(tempPlayer1X, tempPlayer1Y);
            }

            Positions[20] = new Vector2(510, 110);
            tempPlayer1Y = 100;
            tempPlayer1X += 35;

            for (int i = 21; i < 30; i++)
            {
                tempPlayer1X = tempPlayer1X + 75.7f;
                Positions[i] = new Vector2(tempPlayer1X, tempPlayer1Y);
            }

            Positions[30] = new Vector2(1317, 100);
            tempPlayer1X = 1317;
            tempPlayer1Y = 130;

            for (int i = 31; i < 40; i++)
            {
                tempPlayer1Y = tempPlayer1Y + 75.7f;
                Positions[i] = new Vector2(tempPlayer1X, tempPlayer1Y);
            }
            return Positions;
        }

        static public Vector2[] MakingGoPositions()
        {
            Vector2[] GoPostions = new Vector2[8];

            GoPostions[0] = new Vector2(1284, 895);
            GoPostions[1] = new Vector2(1284, 935);

            GoPostions[2] = new Vector2(1320, 950);
            GoPostions[3] = new Vector2(1320, 875);
            GoPostions[4] = new Vector2(1320, 915);

            GoPostions[5] = new Vector2(1360, 950);
            GoPostions[6] = new Vector2(1360, 875);
            GoPostions[7] = new Vector2(1360, 915);

            return GoPostions;
        }

        static public Player CreatePlayer(Player player, Texture2D image, int playerNumb, Vector2[] goPositions)
        {
            player.Size = .3f;
            player.Image = image;
            player.Position = goPositions[playerNumb];

            return player;
        }

        static public void RestartingDice()
        { 
        
        }
        #endregion

        #region Textures
        Texture2D Frame;
        Texture2D Yes;
        Texture2D No;
        Texture2D Purchase;
        Texture2D RedDice;
        Texture2D pixel;
        Texture2D pixel2;
        Texture2D itsBeanTime;

        #endregion


        Player player;
        HighlightButton noButton;
        NormalButton diceOnBoard1;
        NormalButton diceOnBoard2;

        Animation dice1;
        Animation dice2;
        
        Dictionary<PropertyNames, Property> Properties = new Dictionary<PropertyNames, Property>();

        PropertyNames? selectedValue = null;

        MouseState lms;

        Vector2[] listOfPositions = new Vector2[40];

        TimeSpan previousTime = TimeSpan.Zero;
        TimeSpan TESTINGpreviousTime = TimeSpan.Zero;
        TimeSpan tokenMovingTime = TimeSpan.Zero;

        TimeSpan diceGlowInterval = TimeSpan.FromSeconds(1);
        TimeSpan TESTINGinterval = TimeSpan.FromMilliseconds(500);
        TimeSpan tokenInterval = TimeSpan.FromMilliseconds(700);

        Property LoadContent(string Name, int x, int y, bool fliped, int rent, int rentH1, int rentH2, int rentH3, int rentH4, int rentHotel, int houseCost, int hotelCost, ContentManager Content)
        {
            if (fliped)
            {
                return new Property(Content.Load<Texture2D>(Name), new Rectangle(x, y, 70, 100), Color.White, rent, rentH1, rentH2, rentH3, rentH4, rentHotel, houseCost, hotelCost);
            }
            else
            {
                return new Property(Content.Load<Texture2D>(Name), new Rectangle(x, y, 100, 70), Color.White, rent, rentH1, rentH2, rentH3, rentH4, rentHotel, houseCost, hotelCost);
            }
        }

        public Board(ContentManager Content)
        {
            //Testing Testing Testing
            charPostitions = MakingPositions();
            goPositions = MakingGoPositions();
            ;
            //Testing Testing Testing
           
            Players = new Player[playerCount];

            pixel = Content.Load<Texture2D>("FFFFFF-1");
            pixel2 = Content.Load<Texture2D>("pixel");
            diceOnBoard1 = new NormalButton(pixel, Vector2.Zero , Color.Yellow * 0.2f, new Rectangle(682, 618, 48, 44));
            RedDice = Content.Load<Texture2D>("RedDice");
            dice1 = new Animation(RedDice, new Vector2(800, 430), 100, new Random(gen.Next()));
            dice2 = new Animation(RedDice, new Vector2(600, 430), 100, new Random(gen.Next()));
            itsBeanTime = Content.Load<Texture2D>("bean");

            #region Properties

            Properties.Add(PropertyNames.Mediterranean, LoadContent("MediterraneanAve", 1199, 895, true, 2, 10, 30, 90, 160, 250, 50, 50, Content));
            ;
            Properties.Add(PropertyNames.Baltic, LoadContent("BalticAve", 1049, 895, true, 4, 20, 60, 180, 320, 450, 50, 50, Content));
            Properties.Add(PropertyNames.Oriental, LoadContent("OrientalAve", 820, 895, true, 6, 30, 90, 270, 400, 550, 50, 50, Content));
            Properties.Add(PropertyNames.Vermont, LoadContent("VermontAve", 668, 895, true, 6, 30, 90, 270, 400, 550, 50, 50, Content));
            Properties.Add(PropertyNames.Connecticut, LoadContent("ConnecticutAve", 592, 895, true, 8, 40, 100, 300, 450, 600, 50, 50, Content));

            Properties.Add(PropertyNames.StCharles, LoadContent("StCharlesPlace", 458, 794, false, 8, 40, 100, 300, 450, 600, 50, 50, Content));
            Properties.Add(PropertyNames.State, LoadContent("StatesAve", 458, 642, false, 10, 40, 100, 300, 450, 600, 50, 50, Content));
            Properties.Add(PropertyNames.Virginia, LoadContent("VirginiaAve", 458, 567, false, 10, 40, 100, 300, 450, 600, 50, 50, Content));
            Properties.Add(PropertyNames.StJames, LoadContent("StJamesPlace", 458, 413, false, 10, 40, 100, 300, 450, 600, 50, 50, Content));
            Properties.Add(PropertyNames.Tennessee, LoadContent("TennesseeAve", 458, 262, false, 10, 40, 100, 300, 450, 600, 50, 50, Content));
            Properties.Add(PropertyNames.NewYork, LoadContent("NewYorkAve", 458, 186, false, 10, 40, 100, 300, 450, 600, 50, 50, Content));
            //592 54, 742, 818, 970, 1045, 1196
            Properties.Add(PropertyNames.Kentucky, LoadContent("KentuckyAve", 592, 54 , true, 10, 40, 100, 300, 450, 600, 50, 50, Content));
            Properties.Add(PropertyNames.Indiana, LoadContent("IndianaAve", 742, 54, true, 10, 40, 100, 300, 450, 600, 50, 50, Content));
            Properties.Add(PropertyNames.Illinois, LoadContent("IllinoisAve", 818, 54, true, 10, 40, 100, 300, 450, 600, 50, 50, Content));
            Properties.Add(PropertyNames.Atlantic, LoadContent("AtlanticAve", 970, 54, true, 10, 40, 100, 300, 450, 600, 50, 50, Content));
            Properties.Add(PropertyNames.Ventnor, LoadContent("VentnorAve", 1045, 54, true, 10, 40, 100, 300, 450, 600, 50, 50, Content));
            Properties.Add(PropertyNames.Marvin, LoadContent("MarvinGardens", 1196, 54, true, 10, 40, 100, 300, 450, 600, 50, 50, Content));

            ;

            //Properties.Add(PropertiesEnum.Pacific, Content.Load<Texture2D>("PacificAve"));
            //Properties.Add(PropertiesEnum.NorthCarolina, Content.Load<Texture2D>("NoCarolinaAve"));
            //Properties.Add(PropertiesEnum.Pennsylvania, Content.Load<Texture2D>("PennsylvaniaAve"));
            //Properties.Add(PropertiesEnum.Park, Content.Load<Texture2D>("ParkPlace"));
            //Properties.Add(PropertiesEnum.Boardwalk, Content.Load<Texture2D>("Boardwalk"));

            //Properties.Add(PropertiesEnum.ReadingR, Content.Load<Texture2D>("ReadingRailroad"));
            //Properties.Add(PropertiesEnum.PennsylvaniaR, Content.Load<Texture2D>("PennsylvaniaRR"));
            //Properties.Add(PropertiesEnum.BOR, Content.Load<Texture2D>("B&ORailroad"));
            //Properties.Add(PropertiesEnum.ShortLineR, Content.Load<Texture2D>("ShortLineRR"));
            //Properties.Add(PropertiesEnum.ElectricComp, Content.Load<Texture2D>("ElectricCompany"));
            //Properties.Add(PropertiesEnum.WaterWorks, Content.Load<Texture2D>("WaterWorks"));

            #endregion

            Frame = Content.Load<Texture2D>("Frame");
            Yes = Content.Load<Texture2D>("Yes");
            No = Content.Load<Texture2D>("No");
            Purchase = Content.Load<Texture2D>("PurchaseThisProp");
            //WhatAboutTheDroidAttackOnTheWookies = Content.Load<Texture2D>("WhatAboutTheDroidAttackOnTheWookies");

            noButton = new HighlightButton(No, new Vector2(356, 662), Color.White);

            //player = new Player(WhatAboutTheDroidAttackOnTheWookies, new Vector2(Player1X, Player1Y), Color.White, "no");
        }

        public void Update(MouseState ms, GameTime gameTime)
        {
            if (bean)
            {
                if (playerCount != Players.Length)
                {
                    Players = new Player[playerCount];
                }
                for (int i = 0; i < playerCount; i++)
                {
                    Players[i] = CreatePlayer(playerDict[$"Player {i + 1}"].player, itsBeanTime, i, goPositions);
                    Players[i].PositionArea = charPostitions;
                }

                CurrentPlayer = Players[0];

                ;
                bean = false;
            }

            #region Property/Testing

            if (ms.LeftButton == ButtonState.Released && lms.LeftButton == ButtonState.Pressed)
            {
                Debug.WriteLine($"X: {ms.X}, Y: {ms.Y}");
            }

            //if (gameTime.TotalGameTime - TESTINGpreviousTime >= TESTINGinterval)
            //{
            //    CurrentPlayer.Position = charPostitions[TESTINGCounter];
            //    TESTINGCounter++;
            //    TESTINGpreviousTime = gameTime.TotalGameTime;
            //}

            #endregion

            #region DiceRolling

            if (diceMoving)
            {
                ;
                if (dice2.dest.X != 24)
                {
                    dice1.dest.Width = (int)Vector2.Lerp(new Vector2(dice1.dest.Width), new Vector2(dice1.dest.Width / 1.1f), .1f).X;
                    dice1.dest.Height = (int)Vector2.Lerp(new Vector2(dice1.dest.Height), new Vector2(dice1.dest.Height / 1.1f), .1f).X;

                    dice2.dest.Width = (int)Vector2.Lerp(new Vector2(dice2.dest.Width), new Vector2(dice2.dest.Width / 1.1f), .1f).X;
                    dice2.dest.Height = (int)Vector2.Lerp(new Vector2(dice2.dest.Height), new Vector2(dice2.dest.Height / 1.1f), .1f).X;


                    dice2.dest.X = (int)Vector2.Lerp(new Vector2(dice2.dest.X, dice1.dest.Y), new Vector2(24, 962), .1f).X;
                    dice2.dest.Y = (int)Vector2.Lerp(new Vector2(dice2.dest.X, dice1.dest.Y), new Vector2(24, 962), .1f).Y;

                    dice1.dest.X = (int)Vector2.Lerp(new Vector2(dice1.dest.X, dice1.dest.Y), new Vector2(104, 962), .1f).X;
                    dice1.dest.Y = (int)Vector2.Lerp(new Vector2(dice1.dest.X, dice1.dest.Y), new Vector2(104, 962), .1f).Y;
                }
                else 
                {
                    diceMoving = false;
                    darkenScreen = false;
                    diceRolling = false;
                    characterMoving = true;
                }
            }

            diceOnBoard1.Update(ms);

            if (diceOnBoard1.IsClicked(ms))
            {
                diceRolling = true;

                darkenScreen = true;
            }

            if (diceRolling)
            {
                showingDice = true;
                dice1.Update(gameTime, true);
                dice2.Update(gameTime, true);

                rollValue = (dice1.DiceRollValue ) + (dice2.DiceRollValue );
                
                if (dice1.stopped)
                {
                    diceMoving = true;
                }
            }
            else 
            {
                if (gameTime.TotalGameTime - previousTime >= diceGlowInterval)
                {
                    diceOnBoard1.Tint = diceOnBoard1.Tint == Color.White * 0.0f ? Color.Yellow * 0.2f : Color.White * 0.0f;
                    previousTime = gameTime.TotalGameTime;
                }
            }

            #endregion

            if (characterMoving)
            {
                if (CurrentPlayer.currentTileIndex <= rollValue + CurrentPlayer.currentTileIndex)
                {
                    if (gameTime.TotalGameTime - tokenMovingTime >= tokenInterval)
                    {
                        CurrentPlayer.Position = charPostitions[CurrentPlayer.currentTileIndex];
                        CurrentPlayer.currentTileIndex++;
                        tokenMovingTime = gameTime.TotalGameTime;
                    }
                }
                else 
                {
                    rollValue = 0;
                    characterMoving = false;
                    showingDice = false;

                    tokenMovingTime = TimeSpan.Zero;

                    dice1.Restart();
                    dice2.Restart();

                    dice1.dest.Width = 128;
                    dice1.dest.Height = 128;
                    dice1.dest.X = 800;
                    dice1.dest.Y = 430;

                    dice2.dest.X = 600;
                    dice2.dest.Y = 430;
                    dice2.dest.Width = dice1.dest.Width;
                    dice2.dest.Height = dice1.dest.Height;

                    if (currentPlayerIndex + 1 < playerCount)
                    {
                        currentPlayerIndex++;
                    }
                    else 
                    {
                        currentPlayerIndex = 0;
                    }

                    CurrentPlayer = Players[currentPlayerIndex];
                }
            }

            lms = ms;
        }

        public void DarkenScreen(SpriteBatch spritebatch)
        {
            spritebatch.Draw(pixel, new Rectangle(0, 0, 1500, 1400), new Color(Color.Black, 0.5f));
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(Purchase, new Vector2(27, 640), Color.White);
            batch.Draw(Yes, new Vector2(324, 662), Color.White);
            noButton.Draw(batch);

            CurrentPlayer?.Draw(batch);

            for (int i = 0; i < Players.Length; i++)
            {
                
                Players[i]?.Draw(batch);
            }
            ;
            diceOnBoard1.Draw(batch);

            batch.Draw(Frame, position: new Vector2(25, 200), color: Color.White);


            if (darkenScreen)
            {
                DarkenScreen(batch);
            }

            if (showingDice)
            {
                dice1.Draw(batch);
                dice2.Draw(batch);
            }


            if (selectedValue.HasValue)
            {
                //Properties[selectedValue.Value]
            }
        }
    }
}
