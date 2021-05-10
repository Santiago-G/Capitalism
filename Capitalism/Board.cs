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
        bool Rent = true;
        bool drawedACard = false;
        bool drawChanceCards = false;
        bool drawCommunityCards = false;
        bool agagagagaga = true;
        bool glowingHouse = false;
        bool buyingHouses = false;
        bool cardDouble = false;
        bool getOutOfJailGlow = false;

        bool houseMenuUp = false;
        bool houseMenuStage2 = false;

        bool redPropMenu = false;
        bool orangePropMenu = false;
        bool yellowPropMenu = false;
        bool greenPropMenu = false;
        bool lightBluePropMenu = false;
        bool bluePropMenu = false;
        bool pinkPropMenu = false;
        bool purplePropMenu = false;

        bool tempi = true;
        bool readyForHotel = false;
        bool readyToBuy = false;
        bool imDone = false;
        //bool firstLap = true;
        #endregion

        ChanceCards chanceCard;
        CommunityChests communityCards;
        Property prop;
        Queue<ChanceCards> chanceCards = new Queue<ChanceCards>();
        Queue<CommunityChests> chestCards = new Queue<CommunityChests>();

        int propRowCounter = 0;

        int rollValue = 0;
        int currentPlayerIndex = 0;
        int doubleCounter = 0;
        public int playerCount => SelectingPlayers.playerCount;

        Property selectedPropToBuildOn;

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
            else if (filename.Contains("GetOutOfJail"))
            {
                return CommunityCardTypes.GetOutOfJail;
            }
            else if (filename.Contains("GrandOperaOpening"))
            {
                return CommunityCardTypes.GetFromOthers;
            }
            else
            {
                return CommunityCardTypes.BankMoney;
            }

            return CommunityCardTypes.Invalid;
            //var cardType = Enum.Parse(typeof(CardTypes), filename);

            // return cardType;
        }

        static private (Vector2 position, int tileNumber) Destination(CardTypes cardTypes, string filename, Vector2[] positions)
        {
            if (cardTypes == CardTypes.Advance)
            {

                if (filename.Contains("Boardwalk"))
                {
                    return (positions[39], 39);
                }
                else if (filename.Contains("Illinois"))
                {
                    return (positions[24], 24);
                }
                else if (filename.Contains("NearestRail"))
                {
                    return (Vector2.One, 1000);
                }
                else if (filename.Contains("NearestUtillity"))
                {
                    return (new Vector2(2, 2), 1001);
                }
                else if (filename.Contains("ReadingRailroad"))
                {
                    return (positions[5], 5);
                }
                else if (filename.Contains("StCharles"))
                {
                    return (positions[11], 11);
                }
            }

            if (cardTypes == CardTypes.GoToGo)
            {
                return (positions[0], 0);
            }

            return (Vector2.Zero, -1);
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

        static private int ChanceMoney(CardTypes cardTypes, string filename)
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
        Texture2D houseIcon;
        Texture2D houseBuyingUI;

        Texture2D PurplePropSprite;
        Texture2D LightBluePropSprite;
        Texture2D PinkPropSprite;
        Texture2D OrangePropSprite;
        Texture2D RedPropSprite;
        Texture2D YellowPropSprite;
        Texture2D GreenPropSprite;
        Texture2D BluePropSprite;

        SpriteFont font;
        SpriteFont mediumSizeFont;
        #endregion

        #region HighlightButtons
        HighlightButton yesButton;
        HighlightButton noButton;
        HighlightButton breakOutOfJailButton;
        HighlightButton exitHouseMenu;
        NormalButton diceOnBoard1;
        HighlightButton house;

        HighlightButton PurpleProp;
        HighlightButton LightBlueProp;
        HighlightButton PinkProp;
        HighlightButton OrangeProp;
        HighlightButton RedProp;
        HighlightButton YellowProp;
        HighlightButton GreenProp;
        HighlightButton BlueProp;

        HighlightButton hotelIcon;
        HighlightButton houseIcony;

        HighlightButton getOutOfJailFree;
        HighlightButton getOutOfJailFree2;

        HighlightButton buyHouses;
        #endregion

        Animation dice1;
        Animation dice2;

        Dictionary<Vector2, Property> Properties = new Dictionary<Vector2, Property>();
        Dictionary<Vector2, Property> BoughtProperties = new Dictionary<Vector2, Property>();
        Dictionary<string, HighlightButton> propertySprites = new Dictionary<string, HighlightButton>();

        MouseState lms;

        Vector2[] listOfPositions = new Vector2[40];

        TimeSpan previousTime = TimeSpan.Zero;
        TimeSpan tokenMovingTime = TimeSpan.Zero;
        TimeSpan diceGlowInterval = TimeSpan.FromSeconds(1);
        TimeSpan tokenInterval = TimeSpan.FromMilliseconds(700);
        TimeSpan chanceCardPrevTime = TimeSpan.Zero;
        TimeSpan communityChestPrevTime = TimeSpan.Zero;

        Property LoadContent(string Name, int x, int y, bool fliped, int cost, int rent, bool isRailroad, bool isUtillity, int rentH1, int rentH2, int rentH3, int rentH4, int rentHotel, int houseCost, int hotelCost, ContentManager Content, PropertyColor propColor)
        {
            if (fliped)
            {
                return new Property(Content.Load<Texture2D>(Name), new Rectangle(x, y, 70, 100), Color.White, cost, rent, isRailroad, isUtillity, rentH1, rentH2, rentH3, rentH4, rentHotel, houseCost, hotelCost, propColor);
            }
            else
            {
                return new Property(Content.Load<Texture2D>(Name), new Rectangle(x, y, 100, 70), Color.White, cost, rent, isRailroad, isUtillity, rentH1, rentH2, rentH3, rentH4, rentHotel, houseCost, hotelCost, propColor);
            }
        }
        Sprite LoadImage(Texture2D image, Vector2 Position, Color Tint)
        {
            return new PropertySprite(image, Position, Tint);
        }

        int target;

        public Property findProp(string imageName)
        {
            for (int i = 0; i < CurrentPlayer.properties.Count; i++)
            {
                if (CurrentPlayer.properties[i].Image.Name == imageName)
                {
                    return CurrentPlayer.properties[i];
                }
            }

            return null;
        }

        public bool hasAll4Houses(string imageName)
        {
            return (findProp(imageName).hotelCounter == 4);
        }
        public List<Property> getAllPropColor(string propColor)
        {
            List<Property> list = new List<Property>();

            for (int i = 0; i < CurrentPlayer.properties.Count; i++)
            {
                if (CurrentPlayer.properties[i].Color(CurrentPlayer.properties[i].PropColor) == propColor)
                {
                    list.Add(CurrentPlayer.properties[i]);
                }
            }


            if (list.Count == 3 || (list.Count == 2 && (propColor == "purple" || propColor == "blue")))
            {
                return list;
            }

            return null;
        }

        public bool isReadyForHouse(Property pendingProp)
        {
            if (pendingProp == null || pendingProp.houseCounter == 4 || (CurrentPlayer.Money - pendingProp.HouseCost) < 0)
            {
                return false;
            }

            List<Property> tempProps = getAllPropColor(pendingProp.Color(pendingProp.PropColor));
            int highestHouseCount = 0;
            int lowestHouseCount = 100;
            int savedIndex = 0;
            int oldHouseCount = 0;

            for (int i = 0; i < tempProps.Count; i++)
            {
                if (tempProps[i] == pendingProp)
                {
                    oldHouseCount = tempProps[i].houseCounter;
                    tempProps[i].houseCounter++;
                    savedIndex = i;
                }

                if (tempProps[i].houseCounter > highestHouseCount)
                {
                    highestHouseCount = tempProps[i].houseCounter;
                }

                if (tempProps[i].houseCounter < lowestHouseCount)
                {
                    lowestHouseCount = tempProps[i].houseCounter;
                }
            }

            tempProps[savedIndex].houseCounter = oldHouseCount;

            if (highestHouseCount - lowestHouseCount > 1)
            {
                return false;
            }

            return true;
        }

        public bool isReadyForHotel(Property pendingProp)
        {
            if (pendingProp == null || pendingProp.houseCounter < 4 || pendingProp.hotelCounter == 1 || (CurrentPlayer.Money - pendingProp.HotelCost) < 0)
            {
                return false;
            }

            List<Property> tempProps = getAllPropColor(pendingProp.Color(pendingProp.PropColor));

            for (int i = 0; i < tempProps.Count; i++)
            {
                if (tempProps[i].houseCounter < 4)
                {
                    return false;
                }
            }

            return true;
        }

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
            mediumSizeFont = Content.Load<SpriteFont>("MediumSize");

            boatFrame = Content.Load<Texture2D>("boatFrame");
            bootFrame = Content.Load<Texture2D>("bootFrame");
            carFrame = Content.Load<Texture2D>("carFrame");
            catFrame = Content.Load<Texture2D>("catFrame");
            dogFrame = Content.Load<Texture2D>("dogFrame");
            duckFrame = Content.Load<Texture2D>("duckFrame");
            hatFrame = Content.Load<Texture2D>("hatFrame");

            houseIcon = Content.Load<Texture2D>("housey2");
            house = new HighlightButton(houseIcon, new Vector2(50, 900), Color.White, Vector2.One);
            houseBuyingUI = Content.Load<Texture2D>("template");

            PurplePropSprite = Content.Load<Texture2D>("PurpleProp");
            LightBluePropSprite = Content.Load<Texture2D>("LightBlueProp");
            PinkPropSprite = Content.Load<Texture2D>("PinkProp");
            OrangePropSprite = Content.Load<Texture2D>("Orange");
            RedPropSprite = Content.Load<Texture2D>("RedProp");
            YellowPropSprite = Content.Load<Texture2D>("YellowProp");
            GreenPropSprite = Content.Load<Texture2D>("GreenProp");
            BluePropSprite = Content.Load<Texture2D>("BlueProp");

            hotelIcon = new HighlightButton(Content.Load<Texture2D>("monopolyHotel"), new Vector2(950, 680), Color.White, new Vector2(0.15f));
            houseIcony = new HighlightButton(Content.Load<Texture2D>("housey2"), new Vector2(700, 700), Color.White, Vector2.One);

            PurpleProp = new HighlightButton(PurplePropSprite, new Vector2(380, 270), Color.White, Vector2.One);
            LightBlueProp = new HighlightButton(LightBluePropSprite, new Vector2(660, 270), Color.White, Vector2.One);
            PinkProp = new HighlightButton(PinkPropSprite, new Vector2(940, 270), Color.White, Vector2.One);
            OrangeProp = new HighlightButton(OrangePropSprite, new Vector2(1220, 270), Color.White, Vector2.One);

            RedProp = new HighlightButton(RedPropSprite, new Vector2(380, 600), Color.White, Vector2.One);
            YellowProp = new HighlightButton(YellowPropSprite, new Vector2(660, 600), Color.White, Vector2.One);
            GreenProp = new HighlightButton(GreenPropSprite, new Vector2(940, 600), Color.White, Vector2.One);
            BlueProp = new HighlightButton(BluePropSprite, new Vector2(1220, 600), Color.White, Vector2.One);


            getOutOfJailFree = new HighlightButton(Content.Load<Texture2D>("getOutOfJailFree"), new Vector2(50, 760), Color.White, new Vector2(.5f, .5f));
            getOutOfJailFree2 = new HighlightButton(Content.Load<Texture2D>("GetOutOfJail2"), new Vector2(50, 760), Color.White, new Vector2(1f, 1f));

            #region Properties

            //finish adding house rent and prop

            Properties.Add(charPostitions[1], LoadContent("MediterraneanAve", 1199, 895, true, 60, 2, false, false, 10, 30, 90, 160, 250, 50, 50, Content, PropertyColor.purple));
            Properties.Add(charPostitions[3], LoadContent("BalticAve", 1049, 895, true, 60, 4, false, false, 20, 60, 180, 320, 450, 50, 50, Content, PropertyColor.purple));

            Properties.Add(charPostitions[6], LoadContent("OrientalAve", 820, 895, true, 100, 6, false, false, 30, 90, 270, 400, 550, 50, 50, Content, PropertyColor.lightblue));
            Properties.Add(charPostitions[8], LoadContent("VermontAve", 668, 895, true, 100, 6, false, false, 30, 90, 270, 400, 550, 50, 50, Content, PropertyColor.lightblue));
            Properties.Add(charPostitions[9], LoadContent("ConnecticutAve", 592, 895, true, 120, 8, false, false, 40, 100, 300, 450, 600, 50, 50, Content, PropertyColor.lightblue));

            Properties.Add(charPostitions[11], LoadContent("StCharlesPlace", 458, 794, false, 140, 10, false, false, 50, 150, 450, 625, 750, 100, 100, Content, PropertyColor.pink));
            Properties.Add(charPostitions[13], LoadContent("StatesAve", 458, 642, false, 140, 10, false, false, 50, 150, 450, 625, 750, 100, 100, Content, PropertyColor.pink));
            Properties.Add(charPostitions[14], LoadContent("VirginiaAve", 458, 567, false, 160, 12, false, false, 60, 180, 500, 700, 900, 100, 100, Content, PropertyColor.pink));

            Properties.Add(charPostitions[16], LoadContent("StJamesPlace", 458, 413, false, 180, 14, false, false, 70, 200, 550, 750, 950, 100, 100, Content, PropertyColor.orange));
            Properties.Add(charPostitions[18], LoadContent("TennesseeAve", 458, 262, false, 180, 14, false, false, 70, 200, 550, 750, 950, 100, 100, Content, PropertyColor.orange));
            Properties.Add(charPostitions[19], LoadContent("NewYorkAve", 458, 186, false, 200, 16, false, false, 80, 220, 600, 800, 1000, 100, 100, Content, PropertyColor.orange));

            Properties.Add(charPostitions[21], LoadContent("KentuckyAve", 592, 54, true, 220, 18, false, false, 90, 250, 700, 875, 1050, 150, 150, Content, PropertyColor.red));
            Properties.Add(charPostitions[23], LoadContent("IndianaAve", 742, 54, true, 220, 18, false, false, 90, 250, 700, 875, 1050, 150, 150, Content, PropertyColor.red));
            Properties.Add(charPostitions[24], LoadContent("IllinoisAve", 818, 54, true, 240, 20, false, false, 100, 300, 750, 925, 1100, 150, 150, Content, PropertyColor.red));

            Properties.Add(charPostitions[26], LoadContent("AtlanticAve", 970, 54, true, 260, 22, false, false, 110, 330, 800, 975, 1150, 150, 150, Content, PropertyColor.yellow));
            Properties.Add(charPostitions[27], LoadContent("VentnorAve", 1045, 54, true, 260, 22, false, false, 110, 330, 800, 975, 1150, 150, 150, Content, PropertyColor.yellow));
            Properties.Add(charPostitions[29], LoadContent("MarvinGardens", 1196, 54, true, 280, 24, false, false, 120, 360, 850, 1025, 1200, 150, 150, Content, PropertyColor.yellow));

            Properties.Add(charPostitions[31], LoadContent("PacificAve", 1196, 54, true, 300, 26, false, false, 130, 390, 900, 1100, 1275, 150, 150, Content, PropertyColor.green));
            Properties.Add(charPostitions[32], LoadContent("NoCarolinaAve", 1196, 54, true, 300, 26, false, false, 130, 390, 900, 1100, 1275, 150, 150, Content, PropertyColor.green));
            Properties.Add(charPostitions[34], LoadContent("PennsylvaniaAve", 1196, 54, true, 300, 28, false, false, 150, 450, 1000, 1200, 1400, 200, 200, Content, PropertyColor.green));

            Properties.Add(charPostitions[37], LoadContent("ParkPlace", 1196, 54, true, 300, 35, false, false, 175, 500, 1100, 1300, 1500, 200, 200, Content, PropertyColor.blue));
            Properties.Add(charPostitions[39], LoadContent("Boardwalk", 1196, 54, true, 300, 50, false, false, 200, 600, 1400, 1700, 2000, 200, 200, Content, PropertyColor.blue));

            Properties.Add(charPostitions[5], LoadContent("ReadingRailroad", 11, 11, true, 200, 25, true, false, 25, 50, 100, 200, 0, 0, 0, Content, PropertyColor.nully));
            Properties.Add(charPostitions[15], LoadContent("PennsylvaniaRR", 11, 11, false, 200, 25, true, false, 25, 50, 100, 200, 0, 0, 0, Content, PropertyColor.nully));
            Properties.Add(charPostitions[25], LoadContent("B&ORailroad", 11, 11, true, 200, 25, true, false, 25, 50, 100, 200, 0, 0, 0, Content, PropertyColor.nully));
            Properties.Add(charPostitions[35], LoadContent("ShortLineRR", 11, 11, false, 200, 25, true, false, 25, 50, 100, 200, 0, 0, 0, Content, PropertyColor.nully));

            Properties.Add(charPostitions[12], LoadContent("ElectricCompany", 12, 12, false, 150, 0, false, true, 0, 0, 0, 0, 0, 0, 0, Content, PropertyColor.nully));
            Properties.Add(charPostitions[28], LoadContent("WaterWorks", 12, 12, true, 150, 0, false, true, 0, 0, 0, 0, 0, 0, 0, Content, PropertyColor.nully));//PropertiesEnum.WaterWorks, Content.Load<Texture2D>("WaterWorks"));

            //////

            propertySprites.Add("purple1", new HighlightButton(Content.Load<Texture2D>("MediterraneanAve"), new Vector2(100, 300), Color.White, Vector2.One));
            propertySprites.Add("purple2", new HighlightButton(Content.Load<Texture2D>("BalticAve"), new Vector2(100, 300), Color.White, Vector2.One));

            propertySprites.Add("lightBlue1", new HighlightButton(Content.Load<Texture2D>("OrientalAve"), new Vector2(535, 300), Color.White, Vector2.One));
            propertySprites.Add("lightBlue2", new HighlightButton(Content.Load<Texture2D>("VermontAve"), new Vector2(810, 300), Color.White, Vector2.One));
            propertySprites.Add("lightBlue3", new HighlightButton(Content.Load<Texture2D>("ConnecticutAve"), new Vector2(1085, 300), Color.White, Vector2.One));

            propertySprites.Add("pink1", new HighlightButton(Content.Load<Texture2D>("StCharlesPlace"), new Vector2(535, 300), Color.White, Vector2.One));
            propertySprites.Add("pink2", new HighlightButton(Content.Load<Texture2D>("StatesAve"), new Vector2(810, 300), Color.White, Vector2.One));
            propertySprites.Add("pink3", new HighlightButton(Content.Load<Texture2D>("VirginiaAve"), new Vector2(1085, 300), Color.White, Vector2.One));

            propertySprites.Add("orange1", new HighlightButton(Content.Load<Texture2D>("StJamesPlace"), new Vector2(535, 300), Color.White, Vector2.One));
            propertySprites.Add("orange2", new HighlightButton(Content.Load<Texture2D>("TennesseeAve"), new Vector2(810, 300), Color.White, Vector2.One));
            propertySprites.Add("orange3", new HighlightButton(Content.Load<Texture2D>("NewYorkAve"), new Vector2(1085, 300), Color.White, Vector2.One));

            propertySprites.Add("red1", new HighlightButton(Content.Load<Texture2D>("KentuckyAve"), new Vector2(535, 300), Color.White, Vector2.One));
            propertySprites.Add("red2", new HighlightButton(Content.Load<Texture2D>("IndianaAve"), new Vector2(810, 300), Color.White, Vector2.One));
            propertySprites.Add("red3", new HighlightButton(Content.Load<Texture2D>("IllinoisAve"), new Vector2(1085, 300), Color.White, Vector2.One));

            propertySprites.Add("yellow1", new HighlightButton(Content.Load<Texture2D>("AtlanticAve"), new Vector2(535, 300), Color.White, Vector2.One));
            propertySprites.Add("yellow2", new HighlightButton(Content.Load<Texture2D>("VentnorAve"), new Vector2(810, 300), Color.White, Vector2.One));
            propertySprites.Add("yellow3", new HighlightButton(Content.Load<Texture2D>("MarvinGardens"), new Vector2(1085, 300), Color.White, Vector2.One));

            propertySprites.Add("green1", new HighlightButton(Content.Load<Texture2D>("PacificAve"), new Vector2(535, 300), Color.White, Vector2.One));
            propertySprites.Add("green2", new HighlightButton(Content.Load<Texture2D>("NoCarolinaAve"), new Vector2(810, 300), Color.White, Vector2.One));
            propertySprites.Add("green3", new HighlightButton(Content.Load<Texture2D>("PennsylvaniaAve"), new Vector2(1085, 300), Color.White, Vector2.One));

            propertySprites.Add("blue1", new HighlightButton(Content.Load<Texture2D>("ParkPlace"), new Vector2(100, 300), Color.White, Vector2.One));
            propertySprites.Add("blue2", new HighlightButton(Content.Load<Texture2D>("Boardwalk"), new Vector2(100, 300), Color.White, Vector2.One));

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
                var result = Destination(cardType, filename, charPostitions);

                chanceCards.Enqueue(new ChanceCards(text, new Rectangle(300, 300, 290 / 4, 160 / 4), Color.White, cardType, result.position, result.tileNumber, ChanceMoney(cardType, filename)  /*ask about order after*/));
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
                string filename2 = "Community\\" + chestFileNames[i];

                Texture2D text = Content.Load<Texture2D>(filename2);

                CommunityCardTypes cardType = GetCardType2(filename2);

                chestCards.Enqueue(new CommunityChests(text, new Rectangle(300, 300, 290 / 4, 160 / 4), Color.White, cardType, CommunityMoney(cardType, filename2)));
            }

            chestCards = ShuffleQueue(chestCards);
            #endregion

            Frame = Content.Load<Texture2D>("Frame");
            Yes = Content.Load<Texture2D>("Yes");
            No = Content.Load<Texture2D>("No");
            Purchase = Content.Load<Texture2D>("PurchaseThisProp");

            noButton = new HighlightButton(No, new Vector2(326, 662), Color.White, Vector2.One);
            yesButton = new HighlightButton(Yes, new Vector2(356, 662), Color.White, Vector2.One);
            breakOutOfJailButton = new HighlightButton(Yes, new Vector2(365, 755), Color.White, new Vector2(1.2f));
            exitHouseMenu = new HighlightButton(No, new Vector2(1496, 55), Color.White, new Vector2(1.5f));
            buyHouses = new HighlightButton(Yes, new Vector2(880, 870), Color.White, new Vector2(3f));

            Bounds = bounds;
        }

        ////////////UPDATE\\\\\\\\\\\\
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

            #region Jail
            if (CurrentPlayer.inJail)
            {
                if (!diceRolling)
                {
                    getOutOfJailGlow = true;
                }
                else
                {
                    getOutOfJailGlow = false;
                }

                if (breakOutOfJailButton.IsClicked)
                {
                    CurrentPlayer.inJail = false;
                    CurrentPlayer.Money -= 50;
                    CurrentPlayer.Position = charPostitions[10];
                }
            }

            #endregion

            #region House/Hotels
            glowingHouse = true;
            if ((house.IsClicked && (!diceMoving && !diceRolling && !characterMoving)) || houseMenuUp)
            {
                buyingHouses = true;
                darkenScreen = true;
                houseMenuUp = true;

                #region if the props are clicked
                if (RedProp.IsClicked && CurrentPlayer.allOfOneColor("red") && tempi)
                {
                    tempi = false;
                    redPropMenu = true;
                    houseMenuStage2 = true;
                }
                else if (OrangeProp.IsClicked && CurrentPlayer.allOfOneColor("orange") && tempi)
                {
                    orangePropMenu = true;
                    houseMenuStage2 = true;
                    tempi = false;
                }
                else if (YellowProp.IsClicked && CurrentPlayer.allOfOneColor("yellow") && tempi)
                {
                    yellowPropMenu = true;
                    houseMenuStage2 = true;
                    tempi = false;
                }
                else if (GreenProp.IsClicked && CurrentPlayer.allOfOneColor("green") && tempi)
                {
                    greenPropMenu = true;
                    houseMenuStage2 = true;
                    tempi = false;
                }
                else if (LightBlueProp.IsClicked && CurrentPlayer.allOfOneColor("lightBlue") && tempi)
                {
                    lightBluePropMenu = true;
                    houseMenuStage2 = true;
                    tempi = false;
                }
                else if (BlueProp.IsClicked && CurrentPlayer.allOfOneColor("blue") && tempi)
                {
                    bluePropMenu = true;
                    houseMenuStage2 = true;
                    tempi = false;
                }
                else if (PurpleProp.IsClicked && CurrentPlayer.allOfOneColor("purple") && tempi)
                {
                    purplePropMenu = true;
                    houseMenuStage2 = true;
                    tempi = false;
                }
                else if (PinkProp.IsClicked && CurrentPlayer.allOfOneColor("pink") && tempi)
                {
                    pinkPropMenu = true;
                    houseMenuStage2 = true;
                    tempi = false;
                }
                #endregion

                if (houseMenuStage2)
                {

                }

                //then use propSprite dict to draw the props

                if (exitHouseMenu.IsClicked || imDone)
                {
                    buyingHouses = false;
                    darkenScreen = false;
                    houseMenuUp = false;

                    houseMenuStage2 = false;

                    redPropMenu = false;
                    orangePropMenu = false;
                    yellowPropMenu = false;
                    greenPropMenu = false;
                    lightBluePropMenu = false;
                    bluePropMenu = false;
                    pinkPropMenu = false;
                    purplePropMenu = false;
                    tempi = true;
                    imDone = false;
                }
            }

            #endregion

            #region Updates 
            CurrentPlayer.Update();
            noButton.Update(ms, true);
            yesButton.Update(ms, true);
            breakOutOfJailButton.Update(ms, getOutOfJailGlow);
            buyHouses.Update(ms, readyToBuy);

            exitHouseMenu.Update(ms, true);

            PurpleProp.Update(ms, CurrentPlayer.allOfOneColor("purple"));
            LightBlueProp.Update(ms, CurrentPlayer.allOfOneColor("lightBlue"));
            PinkProp.Update(ms, CurrentPlayer.allOfOneColor("pink"));
            OrangeProp.Update(ms, CurrentPlayer.allOfOneColor("orange"));
            RedProp.Update(ms, CurrentPlayer.allOfOneColor("red"));
            YellowProp.Update(ms, CurrentPlayer.allOfOneColor("yellow"));
            GreenProp.Update(ms, CurrentPlayer.allOfOneColor("green"));
            BlueProp.Update(ms, CurrentPlayer.allOfOneColor("blue"));

            house.Update(ms, glowingHouse);
            getOutOfJailFree.Update(ms, getOutOfJailGlow);
            getOutOfJailFree2.Update(ms, getOutOfJailGlow);

            propertySprites[$"purple1"].Update(ms, true);
            propertySprites[$"purple2"].Update(ms, true);
            propertySprites[$"lightBlue1"].Update(ms, true);
            propertySprites[$"lightBlue2"].Update(ms, true);
            propertySprites[$"lightBlue3"].Update(ms, true);
            propertySprites[$"pink1"].Update(ms, true);
            propertySprites[$"pink2"].Update(ms, true);
            propertySprites[$"pink3"].Update(ms, true);
            propertySprites[$"orange1"].Update(ms, true);
            propertySprites[$"orange2"].Update(ms, true);
            propertySprites[$"orange3"].Update(ms, true);
            propertySprites[$"red1"].Update(ms, true);
            propertySprites[$"red2"].Update(ms, true);
            propertySprites[$"red3"].Update(ms, true);
            propertySprites[$"yellow1"].Update(ms, true);
            propertySprites[$"yellow2"].Update(ms, true);
            propertySprites[$"yellow3"].Update(ms, true);
            propertySprites[$"green1"].Update(ms, true);
            propertySprites[$"green2"].Update(ms, true);
            propertySprites[$"green3"].Update(ms, true);
            propertySprites[$"blue1"].Update(ms, true);
            propertySprites[$"blue2"].Update(ms, true);

            hotelIcon.Update(ms, readyForHotel);
            houseIcony.Update(ms, isReadyForHouse(selectedPropToBuildOn));

            #endregion

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

                    //target = rollValue + CurrentPlayer.currentTileIndex;

                    if (CurrentPlayer.Token != "Boat")
                    {
                        //if (CurrentPlayer.currentTileIndex == 1)
                        //{
                        //    target = 2;
                        //}
                        //else if (CurrentPlayer.currentTileIndex == 2)
                        //{
                        //    target = 4;
                        //}
                        if (CurrentPlayer.currentTileIndex == 1)
                        {
                            target = 7;
                        }
                        else if (CurrentPlayer.currentTileIndex == 7)
                        {
                            target = 9;
                        }
                        else if (CurrentPlayer.currentTileIndex == 9)
                        {
                            target = 10;
                        }
                        else if (CurrentPlayer.currentTileIndex == 10)
                        {
                            target = 12;
                        }
                        else if (CurrentPlayer.currentTileIndex == 12)
                        {
                            target = 14;
                        }
                        else if (CurrentPlayer.currentTileIndex == 14)
                        {
                            target = 15;
                        }
                        else if (CurrentPlayer.currentTileIndex == 15)
                        {
                            target = 17;
                        }
                        else if (CurrentPlayer.currentTileIndex == 17)
                        {
                            target = 19;
                        }
                        else if (CurrentPlayer.currentTileIndex == 19)
                        {
                            target = 20;
                        }
                        else if (CurrentPlayer.currentTileIndex == 20)
                        {
                            target = 22;
                        }
                        else if (CurrentPlayer.currentTileIndex == 22)
                        {
                            target = 24;
                        }
                        else if (CurrentPlayer.currentTileIndex == 24)
                        {
                            target = 25;
                        }
                        else if (CurrentPlayer.currentTileIndex == 25)
                        {
                            target = 27;
                        }
                        else if (CurrentPlayer.currentTileIndex == 27)
                        {
                            target = 28;
                        }
                        else if (CurrentPlayer.currentTileIndex == 28)
                        {
                            target = 30;
                        }
                        else if (CurrentPlayer.currentTileIndex == 30)
                        {
                            target = 32;
                        }
                        else if (CurrentPlayer.currentTileIndex == 32)
                        {
                            target = 33;
                        }
                        else if (CurrentPlayer.currentTileIndex == 33)
                        {
                            target = 35;
                        }
                        else if (CurrentPlayer.currentTileIndex == 35)
                        {
                            target = 38;
                        }
                        else if (CurrentPlayer.currentTileIndex == 38)
                        {
                            target = 40;
                        }


                    }
                    else
                    {
                        target = 1;
                    }

                    /*
                    FOR TESTING BUYING PROPS 
                     
                    if (CurrentPlayer.Token != "Boat")
                    {
                        target = 3 + CurrentPlayer.currentTileIndex;
                    }
                    else
                    {
                        target = 1;
                    }
                    */

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
                else
                {
                    if (CurrentPlayer.inJail && (getOutOfJailFree.IsClicked || getOutOfJailFree2.IsClicked))
                    {
                        CurrentPlayer.inJail = false;
                        CurrentPlayer.Position = charPostitions[10];
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


                    if (gameTime.TotalGameTime - tokenMovingTime >= tokenInterval)
                    {
                        CurrentPlayer.Position = charPostitions[CurrentPlayer.currentTileIndex];
                        CurrentPlayer.currentTileIndex++;
                        tokenMovingTime = gameTime.TotalGameTime;
                    }
                }
                else
                {
                    if (dice1.DiceRollValue == dice2.DiceRollValue)
                    {
                        cardDouble = true;
                        doubleCounter++;

                        if (doubleCounter == 3)
                        {
                            CurrentPlayer.inJail = true;
                        }
                    }
                    else
                    {
                        cardDouble = false;
                        doubleCounter = 0;
                    }

                    characterMoving = false;
                    itsMoneyTime = true;
                    Rent = true;
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

                if (CurrentPlayer.currentTileIndex == target)
                {
                    shouldMove = false;
                }
            }
            #endregion

            #region Financial stuff 

            if (itsMoneyTime)
            {

                //jail
                if (CurrentPlayer.currentTileIndex == 31)
                {
                    CurrentPlayer.inJail = true;
                    CurrentPlayer.jailTimer++;
                    CurrentPlayer.Position = new Vector2(523, 885);
                }
                //jail

                if (buyingHouses)
                {

                }

                //if the player clicks on the house/hotel icon on the bottom left, open a menu that shows all the colored properties divided by colors
                //if they own all of 1 color property, the properties will be highlighted, otherwise they will be darkened
                //when they click on the property 

                #region Chance Card

                if ((CurrentPlayer.currentTileIndex == 8 || CurrentPlayer.currentTileIndex == 23 || CurrentPlayer.currentTileIndex == 37) && !drawedACard)
                {
                    chanceCard = chanceCards.Dequeue();
                    bool dontPutBack = false;
                    drawChanceCards = true;

                    if (chanceCard.money != 0)
                    {
                        CurrentPlayer.Money += chanceCard.money;
                    }
                    else if (chanceCard.destination != Vector2.Zero || chanceCard.cardTypes == CardTypes.GoToGo)
                    {
                        if (chanceCard.destination == Vector2.One)
                        {
                            //Nearest Railroad.

                            int county = CurrentPlayer.currentTileIndex;

                            while (true)
                            {
                                if ((Properties.ContainsKey(charPostitions[county]) && Properties[charPostitions[county]].isRailroad) || (BoughtProperties.ContainsKey(charPostitions[county]) && BoughtProperties[charPostitions[county]].isRailroad))
                                {
                                    chanceCard.destination = charPostitions[county];
                                    chanceCard.tileNumber = county + 1;
                                    break;
                                }
                                else if (county == 39)
                                {
                                    county = 0;
                                }


                                county++;
                            }
                        }

                        else if (chanceCard.destination == new Vector2(2, 2))
                        {
                            //Nearest Utility

                            int thingy = CurrentPlayer.currentTileIndex;

                            while (true)
                            {
                                if ((Properties.ContainsKey(charPostitions[thingy]) && Properties[charPostitions[thingy]].isUtility) || (BoughtProperties.ContainsKey(charPostitions[thingy]) && BoughtProperties[charPostitions[thingy]].isUtility))
                                {
                                    chanceCard.destination = charPostitions[thingy];
                                    chanceCard.tileNumber = thingy + 1;
                                    break;
                                }
                                else if (thingy == 39)
                                {
                                    thingy = 0;
                                }

                                thingy++;
                            }
                        }

                        if (CurrentPlayer.currentTileIndex > chanceCard.tileNumber && !moneyStolenOnce)
                        {
                            CurrentPlayer.Money += 200;
                            moneyStolenOnce = true;
                        }

                        CurrentPlayer.Position = chanceCard.destination;
                        CurrentPlayer.currentTileIndex = chanceCard.tileNumber;
                    }
                    else if (chanceCard.cardTypes == CardTypes.GetOutOfJail)
                    {
                        dontPutBack = true;
                        CurrentPlayer.GetOutOfJailFree = true;

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

                        if (CurrentPlayer.currentTileIndex == 5)
                        {
                            CurrentPlayer.Money -= 200;
                        }
                    }
                    else if (chanceCard.cardTypes == CardTypes.GoInJail)
                    {
                        CurrentPlayer.inJail = true;
                        CurrentPlayer.Position = new Vector2(523, 885);
                    }
                    else if (chanceCard.cardTypes == CardTypes.HouseRepair)
                    {
                        //houses
                    }

                    chanceCard.Hitbox.X = 1100;
                    chanceCard.Hitbox.Y = 680;

                    if (!dontPutBack)
                    {
                        chanceCards.Enqueue(chanceCard);
                    }

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

                if ((CurrentPlayer.currentTileIndex == 3 || CurrentPlayer.currentTileIndex == 18 || CurrentPlayer.currentTileIndex == 34) && !drawedACard)
                {
                    communityCards = chestCards.Dequeue();

                    drawCommunityCards = true;
                    bool dontPutBack = false;
                    if (communityCards.money != 0)
                    {
                        CurrentPlayer.Money += communityCards.money;
                    }


                    switch (communityCards.cardTypes)
                    {
                        case CommunityCardTypes.BankMoney:
                            break;

                        case CommunityCardTypes.GetFromOthers:
                            for (int i = 0; i < playerCount; i++)
                            {
                                if (Players[i] != CurrentPlayer)
                                {
                                    Players[i].Money -= 50;
                                }
                            }
                            break;

                        case CommunityCardTypes.GetOutOfJail:
                            CurrentPlayer.GetOutOfJailFree2 = true;
                            dontPutBack = true;
                            break;

                        case CommunityCardTypes.GoInJail:
                            CurrentPlayer.inJail = true;
                            break;

                        case CommunityCardTypes.HouseRepair:
                            //house
                            break;

                        case CommunityCardTypes.GoToGo:
                            CurrentPlayer.Position = charPostitions[0];
                            CurrentPlayer.currentTileIndex = 0;
                            break;

                        case CommunityCardTypes.Invalid:
                            break;
                        default:
                            break;
                    }

                    communityCards.Hitbox.X = 1100;
                    communityCards.Hitbox.Y = 680;

                    if (!dontPutBack)
                    {
                        chestCards.Enqueue(communityCards);
                    }

                    drawedACard = true;
                }

                if (drawCommunityCards)
                {
                    if (communityCards.rotation < 8 * 3.14f)
                    {
                        communityCards.rotation += .2f;
                        communityCards.Hitbox.X = (int)Lerp(communityCards.Hitbox.X, Bounds.Width / 2, .03f);
                        communityCards.Hitbox.Y = (int)Lerp(communityCards.Hitbox.Y, Bounds.Height / 2, .03f);

                        communityCards.Hitbox.Width = (int)Lerp(communityCards.Hitbox.Width, 190, .05f);
                        communityCards.Hitbox.Height = (int)Lerp(communityCards.Hitbox.Height, 150, .05f);
                    }
                    else
                    {
                        communityChestPrevTime += gameTime.ElapsedGameTime;
                        if (communityChestPrevTime >= TimeSpan.FromSeconds(3))
                        {
                            communityCards.rotation = 0;
                            communityChestPrevTime = TimeSpan.Zero;
                            drawCommunityCards = false;
                            communityCards = null;
                        }
                    }
                }

                #endregion

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

                #region Buying
                if (Properties.ContainsKey(CurrentPlayer.Position) && !drawChanceCards)
                {
                    if (noButton.IsClicked)
                    {
                        if (!cardDouble)
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

                        }
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
                        int propCount = CurrentPlayer.properties.Count;
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
                        if (propCount == 1)
                        {
                            CurrentPlayer.properties[0].Hitbox = new Rectangle(1475, 700, CurrentPlayer.properties[0].Image.Width / 3, CurrentPlayer.properties[0].Image.Height / 3);
                        }
                        else
                        {
                            int x = 0;
                            int y = 0;
                            switch (propRowCounter)
                            {
                                case 0:
                                    x = 1475;
                                    y = 690;
                                    break;
                                case 1:
                                    x = 1680;
                                    y = 690;
                                    propCount = propCount - 5;
                                    break;
                                case 2:
                                    x = 1475;
                                    y = 860;
                                    propCount = propCount - 10;
                                    break;
                                case 3:
                                    x = 1680;
                                    y = 860;
                                    propCount = propCount - 15;
                                    break;

                                default:
                                    throw new Exception("Counter is off");
                            }

                            int[] xCoordinates = new int[] { 1475, 1680 };
                            int[] yCoordinates = new int[] { 690, 700, 870, 870 };

                            if (propCount % 6 == 0)
                            {
                                propRowCounter++;

                                CurrentPlayer.properties[i].Rotation = CurrentPlayer.properties[0].Rotation;

                                CurrentPlayer.properties[i].Hitbox = new Rectangle(xCoordinates[propRowCounter % 2], yCoordinates[propRowCounter], CurrentPlayer.properties[0].Image.Width / 3, CurrentPlayer.properties[0].Image.Height / 3);
                            }
                            else
                            {
                                x += ((propCount % 6 - 1) * 33);

                                if (propCount % 5 < 4)
                                {
                                    y -= ((propCount % 5 - 1) * 10);
                                }

                                if (propCount == 4)
                                {
                                    y -= 20;
                                }

                                if (propCount == 5)
                                {
                                    y -= 20;
                                }


                                CurrentPlayer.properties[i].Hitbox = new Rectangle(x, y, (CurrentPlayer.properties[i].Image.Width / 3), CurrentPlayer.properties[i].Image.Height / 3);
                                CurrentPlayer.properties[i].Rotation = (CurrentPlayer.properties[i - 1].Rotation + 0.25f);
                            }



                        }

                        #endregion

                        if (!cardDouble)
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

                        }

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
                            Rent = true;
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
                            if (Players[i] != CurrentPlayer && Rent)
                            {
                                for (int j = 0; j < Players[i].properties.Count; j++)
                                {
                                    if (BoughtProperties.ContainsKey(CurrentPlayer.Position) && Players[i].properties[j] == BoughtProperties[CurrentPlayer.Position] && Rent)
                                    {
                                        if (BoughtProperties[CurrentPlayer.Position].isUtility)
                                        {
                                            CurrentPlayer.Money -= BoughtProperties[CurrentPlayer.Position].Rent * rollValue;
                                            Players[i].Money += BoughtProperties[CurrentPlayer.Position].Rent * rollValue; ;
                                        }
                                        else
                                        {
                                            if (Players[i].allOfOneColor(BoughtProperties[CurrentPlayer.Position].Color((BoughtProperties[CurrentPlayer.Position].PropColor))))
                                            {
                                                CurrentPlayer.Money -= BoughtProperties[CurrentPlayer.Position].Rent * 2;
                                                Players[i].Money += BoughtProperties[CurrentPlayer.Position].Rent * 2;
                                            }
                                            else
                                            {
                                                CurrentPlayer.Money -= BoughtProperties[CurrentPlayer.Position].Rent;
                                                Players[i].Money += BoughtProperties[CurrentPlayer.Position].Rent;
                                            }
                                        }

                                        Rent = false;

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

                else if (!drawChanceCards && !drawCommunityCards)
                {
                    if (!cardDouble)
                    {
                        Rent = true;
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

                    rollDice = true;
                    itsMoneyTime = false;
                    diceFlashing = true;
                    moneyStolenOnce = false;
                    drawedACard = false;
                    glowingHouse = false;
                    cardDouble = false;
                }

                #region Growing Effect on Properties 
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
                #endregion

            }
            #endregion

            lms = ms;
        }
        ////////////UPDATE\\\\\\\\\\\\

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

                #region tokens
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
                #endregion

                batch.DrawString(font, $"M : {CurrentPlayer.Money}", new Vector2(1610, 610), Color.White);

                #region jail

                if (CurrentPlayer.inJail)
                {
                    batch.DrawString(font, "Break Out?", new Vector2(220, 710), Color.White);
                    batch.DrawString(font, "Costs 50M", new Vector2(220, 750), Color.White);
                    breakOutOfJailButton.Draw(batch);

                    getOutOfJailFree.Tint = Color.White;
                    getOutOfJailFree2.Tint = Color.White;
                }
                else
                {
                    getOutOfJailFree.Tint = Color.DarkGray;
                    getOutOfJailFree2.Tint = Color.DarkGray;
                }

                if (CurrentPlayer.GetOutOfJailFree)
                {
                    getOutOfJailFree.Draw(batch);
                }
                if (CurrentPlayer.GetOutOfJailFree2)
                {
                    getOutOfJailFree2.Draw(batch);
                }

                #endregion
            }

            CurrentPlayer?.Draw(batch);

            for (int i = 0; i < Players.Length; i++)
            {

                Players[i]?.Draw(batch);
            }
            ;
            diceOnBoard1.Draw(batch);

            house.Draw(batch);


            ///////////////////

            if (darkenScreen)
            {
                DarkenScreen(batch);
            }

            if (buyingHouses)
            {
                batch.Draw(houseBuyingUI, new Vector2(330, 55), Color.White);
                batch.DrawString(mediumSizeFont, "Select which property you want to build on.", new Vector2(560, 140), Color.Black);

                if (houseMenuStage2)
                {
                    buyHouses.Draw(batch);

                    if (purplePropMenu)
                    {
                        propertySprites[$"purple1"].Draw(batch);
                        propertySprites[$"purple2"].Draw(batch);

                        if (propertySprites[$"purple1"].IsClicked)
                        {
                            propertySprites[$"purple1"].stayHighlighted = true;

                            selectedPropToBuildOn = findProp("MediterraneanAve");

                            propertySprites[$"purple2"].stayHighlighted = false;
                            propertySprites[$"purple2"].stopBeingHighlighted = true;
                        }
                        if (propertySprites[$"purple2"].IsClicked)
                        {
                            propertySprites[$"purple2"].stayHighlighted = true;

                            selectedPropToBuildOn = findProp("BalticAve");

                            propertySprites[$"purple1"].stayHighlighted = false;
                            propertySprites[$"purple1"].stopBeingHighlighted = true;
                        }
                    }
                    else if (lightBluePropMenu)
                    {
                        propertySprites[$"lightBlue1"].Draw(batch);
                        propertySprites[$"lightBlue2"].Draw(batch);
                        propertySprites[$"lightBlue3"].Draw(batch);

                        if (propertySprites[$"lightBlue1"].IsClicked)
                        {
                            propertySprites[$"lightBlue1"].stayHighlighted = true;

                            selectedPropToBuildOn = findProp("OrientalAve");

                            propertySprites[$"lightBlue2"].stayHighlighted = false;
                            propertySprites[$"lightBlue2"].stopBeingHighlighted = true;

                            propertySprites[$"lightBlue3"].stayHighlighted = false;
                            propertySprites[$"lightBlue3"].stopBeingHighlighted = true;
                        }
                        if (propertySprites[$"lightBlue2"].IsClicked)
                        {
                            propertySprites[$"lightBlue2"].stayHighlighted = true;

                            selectedPropToBuildOn = findProp("VermontAve");

                            propertySprites[$"lightBlue1"].stayHighlighted = false;
                            propertySprites[$"lightBlue1"].stopBeingHighlighted = true;

                            propertySprites[$"lightBlue3"].stayHighlighted = false;
                            propertySprites[$"lightBlue3"].stopBeingHighlighted = true;
                        }
                        if (propertySprites[$"lightBlue3"].IsClicked)
                        {
                            propertySprites[$"lightBlue3"].stayHighlighted = true;

                            selectedPropToBuildOn = findProp("ConnecticutAve");

                            propertySprites[$"lightBlue1"].stayHighlighted = false;
                            propertySprites[$"lightBlue1"].stopBeingHighlighted = true;

                            propertySprites[$"lightBlue2"].stayHighlighted = false;
                            propertySprites[$"lightBlue2"].stopBeingHighlighted = true;
                        }
                    }
                    else if (pinkPropMenu)
                    {
                        propertySprites[$"pink1"].Draw(batch);
                        propertySprites[$"pink2"].Draw(batch);
                        propertySprites[$"pink3"].Draw(batch);

                        if (propertySprites[$"pink1"].IsClicked)
                        {
                            propertySprites[$"pink1"].stayHighlighted = true;

                            selectedPropToBuildOn = findProp("StCharlesPlace");

                            propertySprites[$"pink2"].stayHighlighted = false;
                            propertySprites[$"pink2"].stopBeingHighlighted = true;

                            propertySprites[$"pink3"].stayHighlighted = false;
                            propertySprites[$"pink3"].stopBeingHighlighted = true;
                        }
                        if (propertySprites[$"pink2"].IsClicked)
                        {
                            propertySprites[$"pink2"].stayHighlighted = true;

                            selectedPropToBuildOn = findProp("StatesAve");

                            propertySprites[$"pink1"].stayHighlighted = false;
                            propertySprites[$"pink1"].stopBeingHighlighted = true;

                            propertySprites[$"pink3"].stayHighlighted = false;
                            propertySprites[$"pink3"].stopBeingHighlighted = true;
                        }
                        if (propertySprites[$"pink3"].IsClicked)
                        {
                            propertySprites[$"pink3"].stayHighlighted = true;

                            selectedPropToBuildOn = findProp("VirginiaAve");

                            propertySprites[$"pink1"].stayHighlighted = false;
                            propertySprites[$"pink1"].stopBeingHighlighted = true;

                            propertySprites[$"pink2"].stayHighlighted = false;
                            propertySprites[$"pink2"].stopBeingHighlighted = true;
                        }
                    }
                    else if (orangePropMenu)
                    {
                        propertySprites[$"orange1"].Draw(batch);
                        propertySprites[$"orange2"].Draw(batch);
                        propertySprites[$"orange3"].Draw(batch);

                        if (propertySprites[$"orange1"].IsClicked)
                        {
                            propertySprites[$"orange1"].stayHighlighted = true;

                            selectedPropToBuildOn = findProp("StJamesPlace");

                            propertySprites[$"orange2"].stayHighlighted = false;
                            propertySprites[$"orange2"].stopBeingHighlighted = true;

                            propertySprites[$"orange3"].stayHighlighted = false;
                            propertySprites[$"orange3"].stopBeingHighlighted = true;
                        }
                        if (propertySprites[$"orange2"].IsClicked)
                        {
                            propertySprites[$"orange2"].stayHighlighted = true;

                            selectedPropToBuildOn = findProp("TennesseeAve");

                            propertySprites[$"orange1"].stayHighlighted = false;
                            propertySprites[$"orange1"].stopBeingHighlighted = true;

                            propertySprites[$"orange3"].stayHighlighted = false;
                            propertySprites[$"orange3"].stopBeingHighlighted = true;
                        }
                        if (propertySprites[$"orange3"].IsClicked)
                        {
                            propertySprites[$"orange3"].stayHighlighted = true;

                            selectedPropToBuildOn = findProp("NewYorkAve");

                            propertySprites[$"orange1"].stayHighlighted = false;
                            propertySprites[$"orange1"].stopBeingHighlighted = true;

                            propertySprites[$"orange2"].stayHighlighted = false;
                            propertySprites[$"orange2"].stopBeingHighlighted = true;
                        }
                    }
                    else if (redPropMenu)
                    {
                        propertySprites[$"red1"].Draw(batch);
                        propertySprites[$"red2"].Draw(batch);
                        propertySprites[$"red3"].Draw(batch);

                        if (propertySprites[$"red1"].IsClicked)
                        {
                            propertySprites[$"red1"].stayHighlighted = true;

                            selectedPropToBuildOn = findProp("KentuckyAve");

                            propertySprites[$"red2"].stayHighlighted = false;
                            propertySprites[$"red2"].stopBeingHighlighted = true;

                            propertySprites[$"red3"].stayHighlighted = false;
                            propertySprites[$"red3"].stopBeingHighlighted = true;
                        }
                        if (propertySprites[$"red2"].IsClicked)
                        {
                            propertySprites[$"red2"].stayHighlighted = true;

                            selectedPropToBuildOn = findProp("IndianaAve");

                            propertySprites[$"red1"].stayHighlighted = false;
                            propertySprites[$"red1"].stopBeingHighlighted = true;

                            propertySprites[$"red3"].stayHighlighted = false;
                            propertySprites[$"red3"].stopBeingHighlighted = true;
                        }
                        if (propertySprites[$"red3"].IsClicked)
                        {
                            propertySprites[$"red3"].stayHighlighted = true;

                            selectedPropToBuildOn = findProp("IllinoisAve");

                            propertySprites[$"red1"].stayHighlighted = false;
                            propertySprites[$"red1"].stopBeingHighlighted = true;

                            propertySprites[$"red2"].stayHighlighted = false;
                            propertySprites[$"red2"].stopBeingHighlighted = true;
                        }
                    }
                    else if (yellowPropMenu)
                    {
                        propertySprites[$"yellow1"].Draw(batch);
                        propertySprites[$"yellow2"].Draw(batch);
                        propertySprites[$"yellow3"].Draw(batch);

                        if (propertySprites[$"yellow1"].IsClicked)
                        {
                            propertySprites[$"yellow1"].stayHighlighted = true;

                            selectedPropToBuildOn = findProp("AtlanticAve");

                            propertySprites[$"yellow2"].stayHighlighted = false;
                            propertySprites[$"yellow2"].stopBeingHighlighted = true;

                            propertySprites[$"yellow3"].stayHighlighted = false;
                            propertySprites[$"yellow3"].stopBeingHighlighted = true;
                        }
                        if (propertySprites[$"yellow2"].IsClicked)
                        {
                            propertySprites[$"yellow2"].stayHighlighted = true;

                            selectedPropToBuildOn = findProp("VentnorAve");

                            propertySprites[$"yellow1"].stayHighlighted = false;
                            propertySprites[$"yellow1"].stopBeingHighlighted = true;

                            propertySprites[$"yellow3"].stayHighlighted = false;
                            propertySprites[$"yellow3"].stopBeingHighlighted = true;
                        }
                        if (propertySprites[$"yellow3"].IsClicked)
                        {
                            propertySprites[$"yellow3"].stayHighlighted = true;

                            selectedPropToBuildOn = findProp("MarvinGardens");

                            propertySprites[$"yellow1"].stayHighlighted = false;
                            propertySprites[$"yellow1"].stopBeingHighlighted = true;

                            propertySprites[$"yellow2"].stayHighlighted = false;
                            propertySprites[$"yellow2"].stopBeingHighlighted = true;
                        }
                    }
                    else if (greenPropMenu)
                    {
                        propertySprites[$"green1"].Draw(batch);
                        propertySprites[$"green2"].Draw(batch);
                        propertySprites[$"green3"].Draw(batch);

                        if (propertySprites[$"green1"].IsClicked)
                        {
                            propertySprites[$"green1"].stayHighlighted = true;

                            selectedPropToBuildOn = findProp("PacificAve");

                            propertySprites[$"green2"].stayHighlighted = false;
                            propertySprites[$"green2"].stopBeingHighlighted = true;

                            propertySprites[$"green3"].stayHighlighted = false;
                            propertySprites[$"green3"].stopBeingHighlighted = true;
                        }
                        if (propertySprites[$"green2"].IsClicked)
                        {
                            propertySprites[$"green2"].stayHighlighted = true;

                            selectedPropToBuildOn = findProp("NoCarolinaAve");

                            propertySprites[$"green1"].stayHighlighted = false;
                            propertySprites[$"green1"].stopBeingHighlighted = true;

                            propertySprites[$"green3"].stayHighlighted = false;
                            propertySprites[$"green3"].stopBeingHighlighted = true;
                        }
                        if (propertySprites[$"green3"].IsClicked)
                        {
                            propertySprites[$"green3"].stayHighlighted = true;

                            selectedPropToBuildOn = findProp("PennsylvaniaAve");

                            propertySprites[$"green1"].stayHighlighted = false;
                            propertySprites[$"green1"].stopBeingHighlighted = true;

                            propertySprites[$"green2"].stayHighlighted = false;
                            propertySprites[$"green2"].stopBeingHighlighted = true;
                        }
                    }
                    else if (bluePropMenu)
                    {
                        propertySprites[$"blue1"].Draw(batch);
                        propertySprites[$"blue2"].Draw(batch);

                        if (propertySprites[$"blue1"].IsClicked)
                        {
                            propertySprites[$"blue1"].stayHighlighted = true;

                            selectedPropToBuildOn = findProp("ParkPlace");

                            propertySprites[$"blue2"].stayHighlighted = false;
                            propertySprites[$"blue2"].stopBeingHighlighted = true;
                        }
                        if (propertySprites[$"blue2"].IsClicked)
                        {
                            propertySprites[$"blue2"].stayHighlighted = true;

                            selectedPropToBuildOn = findProp("Boardwalk");

                            propertySprites[$"blue1"].stayHighlighted = false;
                            propertySprites[$"blue1"].stopBeingHighlighted = true;
                        }
                    }

                    hotelIcon.Draw(batch);
                    houseIcony.Draw(batch);

                    if (selectedPropToBuildOn != null)
                    {
                        //house rules. You can never have more than a one-house difference 

                        //CurrentPlayer.properties[0].houseCounter = 1;
                        //CurrentPlayer.properties[1].houseCounter = 1;
                        //CurrentPlayer.properties[2].houseCounter = 2;

                        #region House Icon Logic
                        if (isReadyForHouse(selectedPropToBuildOn))
                        {
                            houseIcony.Tint = Color.White;

                            if (houseIcony.IsClicked)
                            {
                                readyToBuy = true;
                                houseIcony.stayHighlighted = true;

                                hotelIcon.stayHighlighted = false;
                                hotelIcon.stopBeingHighlighted = true;
                            }
                        }
                        else 
                        {
                            if (!isReadyForHotel(selectedPropToBuildOn))
                            {
                                readyToBuy = false;
                            }

                            houseIcony.Tint = Color.Gray;
                            houseIcony.stayHighlighted = false;
                            houseIcony.stopBeingHighlighted = false;
                        }
                        #endregion

                        #region Hotel Icon Logic
                        if (isReadyForHotel(selectedPropToBuildOn))
                        {
                            hotelIcon.Tint = Color.White;

                            if (hotelIcon.IsClicked)
                            {
                                readyToBuy = true;
                                hotelIcon.stayHighlighted = true;

                                houseIcony.stayHighlighted = false;
                                houseIcony.stopBeingHighlighted = true;
                            }
                        }
                        else
                        {
                            if (!isReadyForHouse(selectedPropToBuildOn))
                            {
                                readyToBuy = false;
                            }

                            hotelIcon.Tint = Color.Gray;
                        }
                        #endregion
                        ;
                        if (readyToBuy && buyHouses.IsClicked)
                        { 

                            if (hotelIcon.stayHighlighted)
                            {
                                selectedPropToBuildOn.hotelCounter++;
                                CurrentPlayer.Money -= selectedPropToBuildOn.HotelCost;
                                selectedPropToBuildOn.Rent = selectedPropToBuildOn.WithHotel;
                            }
                            else 
                            {
                                selectedPropToBuildOn.houseCounter++;
                                CurrentPlayer.Money -= selectedPropToBuildOn.HouseCost;

                                switch (selectedPropToBuildOn.houseCounter)
                                {
                                    case 1:
                                        selectedPropToBuildOn.Rent = selectedPropToBuildOn.With1House;
                                        break;
                                    case 2:
                                        selectedPropToBuildOn.Rent = selectedPropToBuildOn.With2House;
                                        break;
                                    case 3:
                                        selectedPropToBuildOn.Rent = selectedPropToBuildOn.With3House;
                                        break;
                                    case 4:
                                        selectedPropToBuildOn.Rent = selectedPropToBuildOn.With4House;
                                        break;
                                }

                            }
                            houseMenuStage2 = false;
                            imDone = true;
                            selectedPropToBuildOn = null;
                            readyToBuy = false;
                        }
                    }
                    else
                    {
                        hotelIcon.Tint = Color.Gray;
                        houseIcony.Tint = Color.Gray;
                    }
                }
                else
                {
                    if (!CurrentPlayer.allOfOneColor("purple"))
                    {
                        PurpleProp.Tint = Color.Gray;
                    }
                    else
                    {
                        PurpleProp.Tint = Color.White;
                    }

                    if (!CurrentPlayer.allOfOneColor("lightBlue"))
                    {
                        LightBlueProp.Tint = Color.Gray;
                    }
                    else
                    {
                        LightBlueProp.Tint = Color.White;
                    }

                    if (!CurrentPlayer.allOfOneColor("pink"))
                    {
                        PinkProp.Tint = Color.Gray;
                    }
                    else
                    {
                        PinkProp.Tint = Color.White;
                    }

                    if (!CurrentPlayer.allOfOneColor("orange"))
                    {
                        OrangeProp.Tint = Color.Gray;
                    }
                    else
                    {
                        OrangeProp.Tint = Color.White;
                    }

                    if (!CurrentPlayer.allOfOneColor("red"))
                    {
                        RedProp.Tint = Color.Gray;
                    }
                    else
                    {
                        RedProp.Tint = Color.White;
                    }

                    if (!CurrentPlayer.allOfOneColor("yellow"))
                    {
                        YellowProp.Tint = Color.Gray;
                    }
                    else
                    {
                        YellowProp.Tint = Color.White;
                    }

                    if (!CurrentPlayer.allOfOneColor("green"))
                    {
                        GreenProp.Tint = Color.Gray;
                    }
                    else
                    {
                        GreenProp.Tint = Color.White;
                    }

                    if (!CurrentPlayer.allOfOneColor("blue"))
                    {
                        BlueProp.Tint = Color.Gray;
                    }
                    else
                    {
                        BlueProp.Tint = Color.White;
                    }


                    PurpleProp.Draw(batch);
                    LightBlueProp.Draw(batch);
                    PinkProp.Draw(batch);
                    OrangeProp.Draw(batch);
                    RedProp.Draw(batch);
                    YellowProp.Draw(batch);
                    GreenProp.Draw(batch);
                    BlueProp.Draw(batch);
                }
                exitHouseMenu.Draw(batch);
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

            if (drawCommunityCards)
            {
                communityCards.Draw(batch);
            }
        }
        //screaming issue - THE CARDS FREEZE WHEN THE PLAYER HAS TO PAY RENT
        //do these first - houses

        /*   THINGS THAT ARE BROKEN
         * 
         * Community Chest doesn't work with doubles.
         * After you pull a moving card, when you move again it pulls another card
         * doubles not working when char is paying rent
         */
    }
}
