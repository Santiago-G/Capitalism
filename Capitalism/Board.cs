using Capitalism.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Capitalism
{
    public class Board
    {
        Random gen = new Random();

        #region bools
        bool rollDice = true;
        bool showingDice = false;
        bool diceRolling = false;
        bool diceMoving = false;
        bool darkenScreen = false;
        bool characterMoving = false;
        bool itsMoneyTime = false;
        bool bean = true;
        bool diceFlashing = true;
        bool moneyStolenOnce = false;
        bool RedEatsNoTerrarian = true;
        bool drawedACard = false;
        bool drawChanceCards = false;
        bool drawCommunityCards = false;
        bool agagagagaga = true;
        //bool firstLap = true;
        #endregion

        ChanceCards chanceCard;
        CommunityChests communityCards;
        Property prop;
        Queue<ChanceCards> chanceCards = new Queue<ChanceCards>();
        Queue<CommunityChests> chestCards = new Queue<CommunityChests>();

        int rollValue = 0;
        int currentPlayerIndex = 0;
        public int playerCount => SelectingPlayers.playerCount;

        public Rectangle Bounds { get; set; }

        Vector2[] charPostitions;
        Vector2[] goPositions;

        Player[] Players;

        Dictionary<string, (Player player, HighlightButton button)> playerDict = ChoosingCharacters.players;
        Player CurrentPlayer;

        bool shouldMove;

        #region Functions

        //FUNCTIONS\\
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

        static public Queue<T> ShuffleQueue<T>(Queue<T> queue)
        {
            return new Queue<T>(queue.Shuffle());
        }

        static private CardTypes GetCardType(string filename)
        {
            // CardTypes cardType;

            if (filename.Contains("goToGo"))
            {
                return CardTypes.GoToGo;
            }
            else if (filename.Contains("goToJail"))
            {
                return CardTypes.GoInJail;
            }
            else if (filename.Contains("goTo"))
            {
                return CardTypes.Advance;
            }
            else if (filename.Contains("goBack3"))
            {
                return CardTypes.GoBack3;
            }
            else if (filename.Contains("bankPays") || filename.Contains("building&Loan") || filename.Contains("payPoorTax"))
            {
                return CardTypes.BankMoney;
            }
            else if (filename.Contains("generalRepairs"))
            {
                return CardTypes.HouseRepair;
            }
            else if (filename.Contains("chairman"))
            {
                return CardTypes.GiveToOthers;
            }
            else if (filename.Contains("getOutOfJail"))
            {
                return CardTypes.GetOutOfJail;
            }


            return CardTypes.Invalid;
            //var cardType = Enum.Parse(typeof(CardTypes), filename);

            // return cardType;
        }

        static private CommunityCardTypes GetCardType2(string filename)
        {
            // CardTypes cardType;

            if (filename.Contains("AdvanceToGo"))
            {
                return CommunityCardTypes.GoToGo;
            }
            else if (filename.Contains("GoToJail"))
            {
                return CommunityCardTypes.GoInJail;
            }
            else if (filename.Contains("StreetRepairs"))
            {
                return CommunityCardTypes.HouseRepair;
            }
            else if (filename.Contains("chairman"))
            {
                return CommunityCardTypes.GetFromOthers;
            }
            else if (filename.Contains("getOutOfJail"))
            {
                return CommunityCardTypes.GetOutOfJail;
            }
            else
            {
                return CommunityCardTypes.BankMoney;
            }

            return CommunityCardTypes.Invalid;
            //var cardType = Enum.Parse(typeof(CardTypes), filename);

            // return cardType;
        }

        static private Vector2 Destination(CardTypes cardTypes, string filename, Vector2[] positions)
        {
            if (cardTypes == CardTypes.Advance)
            {
                string propertyName = null;

                if (filename.Contains("Boardwalk"))
                {
                    return positions[39];
                }
                else if (filename.Contains("Illinois"))
                {
                    return positions[24];
                }
                else if (filename.Contains("NearestRail"))
                {
                    //good luck
                }
                else if (filename.Contains("NearestUtillity"))
                {
                    //good luck
                }
                else if (filename.Contains("ReadingRailroad"))
                {
                    return positions[5];
                }
                else if (filename.Contains("StCharles"))
                {
                    return positions[11];
                }
            }

            return Vector2.Zero;
        }

        static private int CommunityMoney(CommunityCardTypes cardTypes, string filename)
        {
            if (cardTypes == CommunityCardTypes.BankMoney)
            {
                if (filename.Contains("BankError"))
                {
                    return 200;
                }
                else if (filename.Contains("DoctorsFee"))
                {
                    return 50;
                }
                else if (filename.Contains("IncomeTaxRefund"))
                {
                    return 20;
                }
                else if (filename.Contains("LifeInsuranceMatures") || filename.Contains("xmasFundMatures") || filename.Contains("YouInherit100"))
                {
                    return 100;
                }
                else if (filename.Contains("PayHospital") || filename.Contains("PaySchoolTax"))
                {
                    return -150;
                }
                else if (filename.Contains("ReceiveForServices"))
                {
                    return 25;
                }
                else if (filename.Contains("SaleOfStocks"))
                {
                    return 45;
                }
                else if (filename.Contains("SecondPrizeBeautyContest"))
                {
                    return 10;
                }
            }

            if (cardTypes == CommunityCardTypes.GetFromOthers)
            {
                return 50;
            }

            return 0;
        }

        static private int ChestMoney(CardTypes cardTypes, string filename)
        {
            if (cardTypes == CardTypes.BankMoney)
            {
                if (filename.Contains("payPoorTax"))
                {
                    return -15;
                }
                else if (filename.Contains("bankPays50"))
                {
                    return 50;
                }
                else if (filename.Contains("building&Loan"))
                {
                    return 150;
                }
            }

            if (cardTypes == CardTypes.GiveToOthers)
            {
                return -50;
            }

            return 0;
        }

        static float Lerp(float start_value, float end_value, float pct)
        {
            return (start_value + (end_value - start_value) * pct);
        }
        //FUNCTIONS\\

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
        Texture2D PlayerTitle;

        Texture2D boatFrame;
        Texture2D bootFrame;
        Texture2D carFrame;
        Texture2D catFrame;
        Texture2D dogFrame;
        Texture2D duckFrame;
        Texture2D hatFrame;

        Texture2D advanceToGo;
        Texture2D bankError;
        Texture2D doctorsFee;
        Texture2D getOutOfJail;
        Texture2D goToJail;
        Texture2D grandOperaOpening;
        Texture2D incomingTaxRefund;
        Texture2D lifeInsuranceMatures;
        Texture2D payHospital;
        Texture2D paySchoolTax;
        Texture2D receiveForServices;
        Texture2D saleOfStocks;
        Texture2D secondPrizeBeautyContest;
        Texture2D streetRepairs;
        Texture2D xmasFundMatures;
        Texture2D youInherit100;

        SpriteFont font;
        #endregion

        HighlightButton yesButton;
        HighlightButton noButton;
        NormalButton diceOnBoard1;
        NormalButton diceOnBoard2;


        Animation dice1;
        Animation dice2;

        Dictionary<Vector2, Property> Properties = new Dictionary<Vector2, Property>();
        Dictionary<Vector2, Property> BoughtProperties = new Dictionary<Vector2, Property>();

        PropertyNames? selectedValue = null;

        MouseState lms;

        Vector2[] listOfPositions = new Vector2[40];

        TimeSpan previousTime = TimeSpan.Zero;
        TimeSpan tokenMovingTime = TimeSpan.Zero;
        TimeSpan diceGlowInterval = TimeSpan.FromSeconds(1);
        TimeSpan tokenInterval = TimeSpan.FromMilliseconds(700);
        TimeSpan chanceCardPrevTime = TimeSpan.Zero;

        Property LoadContent(string Name, int x, int y, bool fliped, int cost, int rent, bool isRailroad, int rentH1, int rentH2, int rentH3, int rentH4, int rentHotel, int houseCost, int hotelCost, ContentManager Content)
        {
            if (fliped)
            {
                return new Property(Content.Load<Texture2D>(Name), new Rectangle(x, y, 70, 100), Color.White, cost, rent, isRailroad, rentH1, rentH2, rentH3, rentH4, rentHotel, houseCost, hotelCost);
            }
            else
            {
                return new Property(Content.Load<Texture2D>(Name), new Rectangle(x, y, 100, 70), Color.White, cost, rent, isRailroad, rentH1, rentH2, rentH3, rentH4, rentHotel, houseCost, hotelCost);
            }
        }
        Sprite LoadImage(Texture2D image, Vector2 Position, Color Tint)
        {
            return new PropertySprite(image, Position, Tint);
        }


        int target;

        public Board(ContentManager Content, Rectangle bounds)
        {
            //Testing Testing Testing
            charPostitions = MakingPositions();
            goPositions = MakingGoPositions();
            //Testing Testing Testing

            Players = new Player[playerCount];

            PlayerTitle = Content.Load<Texture2D>("PlayerTitle2");
            pixel = Content.Load<Texture2D>("FFFFFF-1");
            pixel2 = Content.Load<Texture2D>("pixel");
            diceOnBoard1 = new NormalButton(pixel, Vector2.Zero, Color.Yellow * 0.2f, new Rectangle(682, 618, 48, 44));
            RedDice = Content.Load<Texture2D>("RedDice");
            dice1 = new Animation(RedDice, new Vector2(800, 430), 100, new Random(gen.Next()));
            dice2 = new Animation(RedDice, new Vector2(600, 430), 100, new Random(gen.Next()));
            itsBeanTime = Content.Load<Texture2D>("bean");
            font = Content.Load<SpriteFont>("smallSize");

            boatFrame = Content.Load<Texture2D>("boatFrame");
            bootFrame = Content.Load<Texture2D>("bootFrame");
            carFrame = Content.Load<Texture2D>("carFrame");
            catFrame = Content.Load<Texture2D>("catFrame");
            dogFrame = Content.Load<Texture2D>("dogFrame");
            duckFrame = Content.Load<Texture2D>("duckFrame");
            hatFrame = Content.Load<Texture2D>("hatFrame");


            #region Properties

            Properties.Add(charPostitions[1], LoadContent("MediterraneanAve", 1199, 895, true, 60, 2, false, 10, 30, 90, 160, 250, 50, 50, Content));
            Properties.Add(charPostitions[3], LoadContent("BalticAve", 1049, 895, true, 60, 4, false, 20, 60, 180, 320, 450, 50, 50, Content));

            Properties.Add(charPostitions[6], LoadContent("OrientalAve", 820, 895, true, 100, 6, false, 30, 90, 270, 400, 550, 50, 50, Content));
            Properties.Add(charPostitions[8], LoadContent("VermontAve", 668, 895, true, 100, 6, false, 30, 90, 270, 400, 550, 50, 50, Content));
            Properties.Add(charPostitions[9], LoadContent("ConnecticutAve", 592, 895, true, 120, 8, false, 40, 100, 300, 450, 600, 50, 50, Content));

            Properties.Add(charPostitions[11], LoadContent("StCharlesPlace", 458, 794, false, 140, 10, false, 40, 100, 300, 450, 600, 50, 50, Content));
            Properties.Add(charPostitions[13], LoadContent("StatesAve", 458, 642, false, 140, 10, false, 40, 100, 300, 450, 600, 50, 50, Content));
            Properties.Add(charPostitions[14], LoadContent("VirginiaAve", 458, 567, false, 160, 12, false, 40, 100, 300, 450, 600, 50, 50, Content));

            Properties.Add(charPostitions[16], LoadContent("StJamesPlace", 458, 413, false, 180, 14, false, 40, 100, 300, 450, 600, 50, 50, Content));
            Properties.Add(charPostitions[18], LoadContent("TennesseeAve", 458, 262, false, 180, 14, false, 40, 100, 300, 450, 600, 50, 50, Content));
            Properties.Add(charPostitions[19], LoadContent("NewYorkAve", 458, 186, false, 200, 16, false, 40, 100, 300, 450, 600, 50, 50, Content));

            Properties.Add(charPostitions[21], LoadContent("KentuckyAve", 592, 54, true, 220, 18, false, 40, 100, 300, 450, 600, 50, 50, Content));
            Properties.Add(charPostitions[23], LoadContent("IndianaAve", 742, 54, true, 220, 18, false, 40, 100, 300, 450, 600, 50, 50, Content));
            Properties.Add(charPostitions[24], LoadContent("IllinoisAve", 818, 54, true, 240, 20, false, 40, 100, 300, 450, 600, 50, 50, Content));

            Properties.Add(charPostitions[26], LoadContent("AtlanticAve", 970, 54, true, 260, 10, false, 40, 100, 300, 450, 600, 50, 50, Content));
            Properties.Add(charPostitions[27], LoadContent("VentnorAve", 1045, 54, true, 260, 22, false, 40, 100, 300, 450, 600, 50, 50, Content));
            Properties.Add(charPostitions[29], LoadContent("MarvinGardens", 1196, 54, true, 280, 24, false, 40, 100, 300, 450, 600, 50, 50, Content));

            Properties.Add(charPostitions[31], LoadContent("PacificAve", 1196, 54, true, 300, 26, false, 40, 100, 300, 450, 600, 50, 50, Content));
            Properties.Add(charPostitions[32], LoadContent("NoCarolinaAve", 1196, 54, true, 300, 26, false, 40, 100, 300, 450, 600, 50, 50, Content));
            Properties.Add(charPostitions[34], LoadContent("PennsylvaniaAve", 1196, 54, true, 300, 28, false, 40, 100, 300, 450, 600, 50, 50, Content));

            Properties.Add(charPostitions[37], LoadContent("ParkPlace", 1196, 54, true, 300, 35, false, 40, 100, 300, 450, 600, 50, 50, Content));
            Properties.Add(charPostitions[39], LoadContent("Boardwalk", 1196, 54, true, 300, 50, false, 40, 100, 300, 450, 600, 50, 50, Content));

            Properties.Add(charPostitions[5], LoadContent("ReadingRailroad", 11, 11, true, 200, 25, true, 25, 50, 100, 200, 0, 0, 0, Content));
            Properties.Add(charPostitions[15], LoadContent("PennsylvaniaRR", 11, 11, false, 200, 25, true, 25, 50, 100, 200, 0, 0, 0, Content));
            Properties.Add(charPostitions[25], LoadContent("B&ORailroad", 11, 11, true, 200, 25, true, 25, 50, 100, 200, 0, 0, 0, Content));
            Properties.Add(charPostitions[35], LoadContent("ShortLineRR", 11, 11, false, 200, 25, true, 25, 50, 100, 200, 0, 0, 0, Content));

            //Properties.Add(PropertiesEnum.ElectricComp, Content.Load<Texture2D>("ElectricCompany"));
            //Properties.Add(PropertiesEnum.WaterWorks, Content.Load<Texture2D>("WaterWorks"));

            #endregion

            #region Chance Cards

            string chancePath = Directory.GetCurrentDirectory();
            int binFolderIndex = chancePath.IndexOf("bin");

            string basePath = "";

            for (int i = 0; i < binFolderIndex; i++)
            {
                basePath += chancePath[i];
            }

            var path = Path.Combine(basePath, @"Content\\Chance");
            var files = Directory.GetFiles(path);

            string[] chanceFileNames = new string[files.Length];
            string extension = ".png";
            int count = 0;
            foreach (var filePath in files)
            {
                for (int i = path.Length + 1; i < filePath.Length - extension.Length; i++)
                {
                    chanceFileNames[count] += filePath[i];
                }
                count++;
            }



            for (int i = 0; i < chanceFileNames.Length; i++)
            {
                string filename = chanceFileNames[i];
                Texture2D text = Content.Load<Texture2D>(filename);

                CardTypes cardType = GetCardType(filename);

                Property property = null;

                chanceCards.Enqueue(new ChanceCards(text, new Rectangle(300, 300, 290 / 4, 160 / 4), Color.White, cardType, Destination(cardType, filename, charPostitions), ChestMoney(cardType, filename)  /*ask about order after*/));
                ;
            }

            chanceCards = ShuffleQueue(chanceCards);
            ;
            #endregion

            #region CommunityChest
            string chestPath = Directory.GetCurrentDirectory();
            int binFolderIndex2 = chancePath.IndexOf("bin");

            string basePath2 = "";

            for (int i = 0; i < binFolderIndex2; i++)
            {
                basePath2 += chestPath[i];
            }

            var path2 = Path.Combine(basePath2, @"Content\\Community");
            var files2 = Directory.GetFiles(path2);

            string[] chestFileNames = new string[files2.Length];
            string extension2 = ".png";
            int count2 = 0;
            foreach (var filePath in files2)
            {
                for (int i = path2.Length + 1; i < filePath.Length - extension2.Length; i++)
                {
                    chestFileNames[count2] += filePath[i];
                }
                count2++;
            }



            for (int i = 0; i < chestFileNames.Length; i++)
            {
                string filename2 = "Community\\"+chestFileNames[i];

                Texture2D text = Content.Load<Texture2D>(filename2);

                CommunityCardTypes cardType = GetCardType2(filename2);

                Property property = null;

                chestCards.Enqueue(new CommunityChests(text, new Rectangle(300, 300, 290 / 4, 160 / 4), Color.White, cardType, CommunityMoney(cardType, filename2)));

                ;
            }

            chestCards = ShuffleQueue(chestCards);
            #endregion

            Frame = Content.Load<Texture2D>("Frame");
            Yes = Content.Load<Texture2D>("Yes");
            No = Content.Load<Texture2D>("No");
            Purchase = Content.Load<Texture2D>("PurchaseThisProp");

            noButton = new HighlightButton(No, new Vector2(326, 662), Color.White);
            yesButton = new HighlightButton(Yes, new Vector2(356, 662), Color.White);
            Bounds = bounds;
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
                    Players[i].currentTileIndex = 1;
                }

                CurrentPlayer = Players[0];

                Players[0].Tint = Color.Purple;
                ;
                bean = false;
            }

            #region Property/Testing

            if (ms.LeftButton == ButtonState.Released && lms.LeftButton == ButtonState.Pressed)
            {
                Debug.WriteLine($"X: {ms.X}, Y: {ms.Y}");
            }

            #endregion

            for (int i = 0; i < Players.Length; i++)
            {
                if (Players[i].Money == 0)
                {
                    //game over
                }
            }

            CurrentPlayer.Update();
            noButton.Update(ms, true);
            yesButton.Update(ms, true);

            if (gameTime.TotalGameTime - previousTime >= diceGlowInterval && diceFlashing)
            {
                diceOnBoard1.Tint = diceOnBoard1.Tint == Color.White * 0.0f ? Color.Yellow * 0.2f : Color.White * 0.0f;
                previousTime = gameTime.TotalGameTime;
            }

            #region DiceRolling
            if (rollDice)
            {

                if (diceMoving)
                {
                    ;
                    if (dice2.dest.X != 24 && !CurrentPlayer.inJail)
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
                        diceFlashing = false;
                        rollDice = false;
                        agagagagaga = true;

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
                    diceFlashing = false;
                    showingDice = true;
                    dice1.Update(gameTime, true);
                    dice2.Update(gameTime, true);

                    rollValue = (dice1.DiceRollValue) + (dice2.DiceRollValue);
                    target = rollValue + (CurrentPlayer.currentTileIndex);
                    //target = 8;

                    if (dice1.stopped)
                    {
                        if (CurrentPlayer.inJail)
                        {
                            if (dice1.DiceRollValue != dice2.DiceRollValue)
                            {
                                rollValue = 0;
                                target = rollValue + (CurrentPlayer.currentTileIndex);

                                if (agagagagaga)
                                {
                                    CurrentPlayer.jailTimer++;
                                    agagagagaga = false;
                                }

                                diceMoving = true;
                            }
                            else
                            {
                                CurrentPlayer.inJail = false;
                                CurrentPlayer.jailTimer = 0;

                            }
                        }
                        else
                        {
                            diceMoving = true;
                        }
                    }
                }


            }
            #endregion

            #region Movement
            if (characterMoving)
            {
                if (target >= 40)
                {
                    target %= 40;
                    shouldMove = true;
                }

                if (CurrentPlayer.currentTileIndex >= 40)
                {
                    CurrentPlayer.currentTileIndex %= 40;
                }

                if (CurrentPlayer.currentTileIndex < target || shouldMove)
                {
                    Console.WriteLine($"Roll Val + Pos = {target}");
                    Console.WriteLine($"Roll Val = {rollValue}");
                    Console.WriteLine($"Pos = {CurrentPlayer.currentTileIndex}");

                    if (gameTime.TotalGameTime - tokenMovingTime >= tokenInterval)
                    {
                        //CurrentPlayer.Position = Vector2.Lerp(CurrentPlayer.Position, val, .1f);

                        CurrentPlayer.Position = charPostitions[CurrentPlayer.currentTileIndex];
                        CurrentPlayer.currentTileIndex++;
                        tokenMovingTime = gameTime.TotalGameTime;
                    }
                }
                else
                {
                    if (dice1.DiceRollValue == dice2.DiceRollValue)
                    {
                        diceFlashing = true;
                        characterMoving = false;
                        rollDice = true;
                        showingDice = false;

                        tokenMovingTime = TimeSpan.Zero;
                        shouldMove = false;
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
                    }
                    else
                    {
                        characterMoving = false;
                        itsMoneyTime = true;
                        showingDice = false;

                        tokenMovingTime = TimeSpan.Zero;
                        shouldMove = false;
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
                    }
                }

                if (CurrentPlayer.currentTileIndex == target)
                {
                    shouldMove = false;
                }
            }
            #endregion

            #region Financial stuff 

            if (itsMoneyTime)
            {
                //taxes
                if (CurrentPlayer.currentTileIndex == 5 && !moneyStolenOnce)
                {
                    CurrentPlayer.Money -= 200;
                    moneyStolenOnce = true;
                }

                else if (CurrentPlayer.currentTileIndex == 38 && !moneyStolenOnce)
                {
                    CurrentPlayer.Money -= 100;
                    moneyStolenOnce = true;
                }
                //taxes

                //jail
                if (CurrentPlayer.currentTileIndex == 31)
                {
                    CurrentPlayer.inJail = true;
                    CurrentPlayer.jailTimer++;
                }
                //jail

                #region Buying
                if (Properties.ContainsKey(CurrentPlayer.Position))
                {
                    if (noButton.IsClicked)
                    {
                        if (currentPlayerIndex + 1 < playerCount)
                        {
                            currentPlayerIndex++;
                        }
                        else
                        {
                            currentPlayerIndex = 0;
                        }

                        CurrentPlayer = Players[currentPlayerIndex];

                        rollDice = true;
                        itsMoneyTime = false;
                        diceFlashing = true;
                        moneyStolenOnce = false;
                        drawedACard = false;
                    }

                    if (yesButton.IsClicked)
                    {
                        CurrentPlayer.Money = CurrentPlayer.Money - Properties[CurrentPlayer.Position].Cost;
                        CurrentPlayer.properties.Add(Properties[CurrentPlayer.Position]);

                        int i = CurrentPlayer.properties.Count - 1;
                        BoughtProperties.Add(CurrentPlayer.Position, Properties[CurrentPlayer.Position]);
                        Properties.Remove(CurrentPlayer.Position);

                        #region Railroads
                        if (CurrentPlayer.mostRecentPurchase().isRailroad)
                        {
                            CurrentPlayer.railroadCounter++;

                            for (int j = 0; j < CurrentPlayer.properties.Count; j++)
                            {
                                if (CurrentPlayer.properties[j].isRailroad)
                                {
                                    if (CurrentPlayer.railroadCounter < 3)
                                    {
                                        CurrentPlayer.properties[j].Rent = 25 * CurrentPlayer.railroadCounter;
                                    }
                                    else
                                    {
                                        CurrentPlayer.properties[j].Rent = 100 * (CurrentPlayer.railroadCounter - 2);
                                    }
                                }
                            }
                        }
                        #endregion

                        #region Utility

                        if (CurrentPlayer.mostRecentPurchase().isUtility)
                        {
                            CurrentPlayer.utillityCounter++;

                            if (CurrentPlayer.utillityCounter < 2)
                            {
                                CurrentPlayer.mostRecentPurchase().Rent = 4;
                            }
                            else
                            {
                                for (int k = 0; k < CurrentPlayer.properties.Count; k++)
                                {
                                    if (CurrentPlayer.properties[k].isUtility)
                                    {
                                        CurrentPlayer.properties[k].Rent = 10;
                                    }
                                }
                            }
                        }

                        #endregion

                        #region Property Placement
                        if (CurrentPlayer.properties.Count == 1)
                        {
                            CurrentPlayer.properties[0].Hitbox = new Rectangle(1500, 720, CurrentPlayer.properties[0].Image.Width / 2, CurrentPlayer.properties[0].Image.Height / 2);
                        }
                        else
                        {
                            int temp = 1500;
                            int temp2 = 690;

                            if (CurrentPlayer.properties.Count % 6 == 0)
                            {
                                temp2 = CurrentPlayer.properties[i - 1].Hitbox.Y + 100;
                                temp = 1500;
                                CurrentPlayer.properties[i].Rotation = CurrentPlayer.properties[0].Rotation;
                            }


                            temp += ((CurrentPlayer.properties.Count % 5 - 1) * 65);
                            if (CurrentPlayer.properties.Count % 5 < 4)
                            {
                                temp2 -= (CurrentPlayer.properties.Count % 5 - 1) * 10;
                            }
                            else
                            {
                                temp2 += (CurrentPlayer.properties.Count % 5 - 4) * 10;
                            }

                            CurrentPlayer.properties[i].Hitbox = new Rectangle(temp, temp2, (CurrentPlayer.properties[i].Image.Width / 2), CurrentPlayer.properties[i].Image.Height / 2);
                            CurrentPlayer.properties[i].Rotation = (CurrentPlayer.properties[i - 1].Rotation + 0.25f);
                        }

                        #endregion

                        if (currentPlayerIndex + 1 < playerCount)
                        {
                            currentPlayerIndex++;
                        }
                        else
                        {
                            currentPlayerIndex = 0;
                        }

                        CurrentPlayer = Players[currentPlayerIndex];

                        rollDice = true;
                        itsMoneyTime = false;
                        diceFlashing = true;
                        moneyStolenOnce = false;
                        drawedACard = false;


                    }

                }

                #endregion

                #region Rent
                else if (BoughtProperties.ContainsKey(CurrentPlayer.Position))
                {
                    Property temp = BoughtProperties[CurrentPlayer.Position];

                    bool temp2 = false;

                    for (int i = 0; i < CurrentPlayer.properties.Count; i++)
                    {
                        if (temp == CurrentPlayer.properties[i])
                        {
                            RedEatsNoTerrarian = true;
                            if (currentPlayerIndex + 1 < playerCount)
                            {
                                currentPlayerIndex++;
                            }
                            else
                            {
                                currentPlayerIndex = 0;
                            }

                            CurrentPlayer = Players[currentPlayerIndex];

                            rollDice = true;
                            itsMoneyTime = false;
                            diceFlashing = true;
                            moneyStolenOnce = false;
                            temp2 = true;
                            drawedACard = false;
                        }
                    }
                    if (!temp2)
                    {
                        for (int i = 0; i < Players.Length; i++)
                        {
                            if (Players[i] != CurrentPlayer && RedEatsNoTerrarian)
                            {
                                for (int j = 0; j < Players[i].properties.Count; j++)
                                {
                                    if (BoughtProperties.ContainsKey(CurrentPlayer.Position) && Players[i].properties[j] == BoughtProperties[CurrentPlayer.Position] && RedEatsNoTerrarian)
                                    {
                                        if (BoughtProperties[CurrentPlayer.Position].isUtility)
                                        {
                                            CurrentPlayer.Money -= BoughtProperties[CurrentPlayer.Position].Rent * rollValue;
                                            Players[i].Money += BoughtProperties[CurrentPlayer.Position].Rent * rollValue; ;
                                        }
                                        else
                                        {
                                            CurrentPlayer.Money -= BoughtProperties[CurrentPlayer.Position].Rent;
                                            Players[i].Money += BoughtProperties[CurrentPlayer.Position].Rent;
                                        }

                                        RedEatsNoTerrarian = false;

                                        if (currentPlayerIndex + 1 < playerCount)
                                        {
                                            currentPlayerIndex++;
                                        }
                                        else
                                        {
                                            currentPlayerIndex = 0;
                                        }

                                        CurrentPlayer = Players[currentPlayerIndex];

                                        rollDice = true;
                                        itsMoneyTime = false;
                                        diceFlashing = true;
                                        moneyStolenOnce = false;
                                        drawedACard = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                #endregion

                #region Chance Card

                if ((CurrentPlayer.currentTileIndex == 8 || CurrentPlayer.currentTileIndex == 23 || CurrentPlayer.currentTileIndex == 36) && !drawedACard)
                {
                    chanceCard = chanceCards.Dequeue();

                    drawChanceCards = true;

                    if (chanceCard.money != 0)
                    {
                        CurrentPlayer.Money += chanceCard.money;
                    }
                    else if (chanceCard.destination != Vector2.Zero || chanceCard.cardTypes == CardTypes.GoToGo)
                    {
                        CurrentPlayer.Position = chanceCard.destination;
                    }
                    else if (chanceCard.cardTypes == CardTypes.GetOutOfJail)
                    {
                        //out of jail
                    }
                    else if (chanceCard.cardTypes == CardTypes.GiveToOthers)
                    {
                        for (int i = 0; i < playerCount; i++)
                        {
                            if (Players[i] != CurrentPlayer)
                            {
                                Players[i].Money += 50;
                            }
                        }
                    }
                    else if (chanceCard.cardTypes == CardTypes.GoBack3)
                    {
                        CurrentPlayer.Position = charPostitions[CurrentPlayer.currentTileIndex - 4];
                    }
                    else if (chanceCard.cardTypes == CardTypes.GoInJail)
                    {
                        CurrentPlayer.inJail = true;
                    }
                    else if (chanceCard.cardTypes == CardTypes.HouseRepair)
                    {
                        //houses
                    }

                    chanceCard.Hitbox.X = 1100;
                    chanceCard.Hitbox.Y = 680;

                    chanceCards.Enqueue(chanceCard);
                    drawedACard = true;
                }

                if (drawChanceCards)
                {

                    if (chanceCard.rotation < 8 * 3.14f)
                    {
                        chanceCard.rotation += .2f;
                        chanceCard.Hitbox.X = (int)Lerp(chanceCard.Hitbox.X, Bounds.Width / 2, .03f);
                        chanceCard.Hitbox.Y = (int)Lerp(chanceCard.Hitbox.Y, Bounds.Height / 2, .03f);

                        chanceCard.Hitbox.Width = (int)Lerp(chanceCard.Hitbox.Width, 190, .05f);
                        chanceCard.Hitbox.Height = (int)Lerp(chanceCard.Hitbox.Height, 150, .05f);

                        Console.WriteLine($"{chanceCard.Hitbox}");
                    }
                    else
                    {
                        chanceCardPrevTime += gameTime.ElapsedGameTime;
                        if (chanceCardPrevTime >= TimeSpan.FromSeconds(3))
                        {
                            chanceCard.rotation = 0;
                            chanceCardPrevTime = TimeSpan.Zero;
                            drawChanceCards = false;
                            chanceCard = null;
                        }
                    }
                }
                #endregion

                #region CommunityChest Cards

                if ((CurrentPlayer.currentTileIndex == 3 || CurrentPlayer.currentTileIndex == 18 || CurrentPlayer.currentTileIndex == 33) && !drawedACard)
                {
                    communityCards = chestCards.Dequeue();

                    drawCommunityCards = true;

                    if (communityCards.money != 0)
                    {
                        CurrentPlayer.Money += communityCards.money;
                    }
                    else if (communityCards.cardTypes == CommunityCardTypes.GetOutOfJail)
                    {
                        //out of jail
                    }
                    else if (communityCards.cardTypes == CommunityCardTypes.GetFromOthers)
                    {
                        for (int i = 0; i < playerCount; i++)
                        {
                            if (Players[i] != CurrentPlayer)
                            {
                                Players[i].Money -= 50;
                            }
                        }
                    }
                    else if (communityCards.cardTypes == CommunityCardTypes.GoInJail)
                    {
                        CurrentPlayer.inJail = true;
                    }
                    else if (communityCards.cardTypes == CommunityCardTypes.HouseRepair)
                    {
                        //houses
                    }

                    communityCards.Hitbox.X = 1100;
                    communityCards.Hitbox.Y = 680;

                    chestCards.Enqueue(communityCards);
                    drawedACard = true;
                }

                if (drawCommunityCards)
                {

                    if (chanceCard.rotation < 8 * 3.14f)
                    {
                        chanceCard.rotation += .2f;
                        chanceCard.Hitbox.X = (int)Lerp(chanceCard.Hitbox.X, Bounds.Width / 2, .03f);
                        chanceCard.Hitbox.Y = (int)Lerp(chanceCard.Hitbox.Y, Bounds.Height / 2, .03f);

                        chanceCard.Hitbox.Width = (int)Lerp(chanceCard.Hitbox.Width, 190, .05f);
                        chanceCard.Hitbox.Height = (int)Lerp(chanceCard.Hitbox.Height, 150, .05f);

                        Console.WriteLine($"{chanceCard.Hitbox}");
                    }
                    else
                    {
                        chanceCardPrevTime += gameTime.ElapsedGameTime;
                        //if (gameTime.TotalGameTime - chanceCardPrevTime > TimeSpan.FromSeconds(3))
                        if (chanceCardPrevTime >= TimeSpan.FromSeconds(3))
                        {
                            chanceCard.rotation = 0;
                            chanceCardPrevTime = TimeSpan.Zero;
                            drawChanceCards = false;
                            chanceCard = null;
                        }
                        //else 
                        //{
                        //    //drawChanceCards = false;
                        //    //chanceCard = null;
                        //}
                    }
                }

                #endregion

                else
                {
                    RedEatsNoTerrarian = true;
                    if (currentPlayerIndex + 1 < playerCount)
                    {
                        currentPlayerIndex++;
                    }
                    else
                    {
                        currentPlayerIndex = 0;
                    }

                    CurrentPlayer = Players[currentPlayerIndex];

                    rollDice = true;
                    itsMoneyTime = false;
                    diceFlashing = true;
                    moneyStolenOnce = false;
                    drawedACard = false;
                }

                // loop over all properties



                for (int i = 0; i < CurrentPlayer.properties.Count; i++)
                {
                    if (CurrentPlayer.properties[i].Hitbox.Contains(ms.Position))
                    {
                        // mouse was inside the hitbox for property
                        //Is any other property that the player owns expanded



                        if (!CurrentPlayer.properties[i].expanded)
                        {
                            bool expand = true;
                            foreach (Property prop in CurrentPlayer.properties)
                            {
                                if (prop.expanded)
                                {
                                    expand = false;
                                }
                            }

                            if (expand)
                            {
                                CurrentPlayer.properties[i].Expand();
                            }
                        }

                    }
                    else
                    {
                        if (CurrentPlayer.properties[i].expanded)
                        {
                            CurrentPlayer.properties[i].Shrink();
                        }
                        //reset the height and width if it goes from true to false
                    }

                }


            }
            #endregion

            lms = ms;
        }

        public void DarkenScreen(SpriteBatch spritebatch)
        {
            spritebatch.Draw(pixel, new Rectangle(0, 0, 130000, 478150), new Color(Color.Black, 0.5f));
        }



        public void Draw(SpriteBatch batch)
        {
            batch.Draw(Purchase, new Vector2(27, 640), Color.White);

            batch.Draw(PlayerTitle, new Vector2(1506, 0), Color.White);
            noButton.Draw(batch);
            yesButton.Draw(batch);

            batch.Draw(Frame, position: new Vector2(25, 200), color: Color.White);

            if (CurrentPlayer != null && Properties.ContainsKey(CurrentPlayer.Position))
            {
                prop = Properties[CurrentPlayer.Position];
            }
            else
            {
                prop = null;
            }

            if (prop != null)
            {
                batch.Draw(prop.Image, new Rectangle(91, 265, 226, 279), Color.White);
            }

            if (CurrentPlayer != null)
            {
                for (int i = 0; i < CurrentPlayer.properties.Count; i++)
                {
                    CurrentPlayer.properties[i].Draw(batch);
                }

                foreach (Property prop in CurrentPlayer.properties)
                {
                    if (prop.expanded)
                    {
                        prop.Draw(batch);
                    }
                }

                //properties
                if (CurrentPlayer.Token == "Car")
                {
                    batch.Draw(carFrame, new Vector2(1496, 200), Color.White);
                }
                else if (CurrentPlayer.Token == "Boat")
                {
                    batch.Draw(boatFrame, new Vector2(1515, 200), Color.White);
                }
                else if (CurrentPlayer.Token == "Boot")
                {
                    batch.Draw(bootFrame, new Vector2(1515, 200), Color.White);
                }
                else if (CurrentPlayer.Token == "Cat")
                {
                    batch.Draw(catFrame, new Vector2(1515, 200), Color.White);
                }
                else if (CurrentPlayer.Token == "Dog")
                {
                    batch.Draw(dogFrame, new Vector2(1475, 200), Color.White);
                }
                else if (CurrentPlayer.Token == "Duck")
                {
                    batch.Draw(duckFrame, new Vector2(1506, 200), Color.White);
                }
                else if (CurrentPlayer.Token == "Hat")
                {
                    batch.Draw(hatFrame, new Vector2(1515, 200), Color.White);
                }

                batch.DrawString(font, $"M : {CurrentPlayer.Money}", new Vector2(1610, 610), Color.White);

            }

            CurrentPlayer?.Draw(batch);

            for (int i = 0; i < Players.Length; i++)
            {

                Players[i]?.Draw(batch);
            }
            ;
            diceOnBoard1.Draw(batch);


            if (darkenScreen)
            {
                DarkenScreen(batch);
            }

            if (showingDice)
            {
                dice1.Draw(batch);
                dice2.Draw(batch);
            }

            if (drawChanceCards)
            {
                chanceCard.Draw(batch);
            }
        }
    }
}
