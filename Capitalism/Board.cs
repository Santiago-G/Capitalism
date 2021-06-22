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
        bool mortgaging = false;
        bool cardDouble = false;
        bool getOutOfJailGlow = false;

        bool houseMenuUp = false;
        bool houseMenuStage2 = false;

        bool mortMenuStage1 = false;
        bool mortMenuStage2 = false;
        bool mortMenuStage3 = false;

        bool unmortMenuStage1 = false;
        bool unmortMenuStage2 = false;
        bool unmortMenuStage3 = false;

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
        bool houseToRotate = false;
        bool hotelToRotate = false;
        bool tearDownTheWall = false;

        bool mortGlow = false;
        #endregion

        string theTrial = "";
        int bricksInTheWall = 0;
        HighlightButton theWall = null;

        int selectedMortgageOption = 0;

        ChanceCards chanceCard;
        CommunityChests communityCards;
        Property prop;
        Queue<ChanceCards> chanceCards = new Queue<ChanceCards>();
        Queue<CommunityChests> chestCards = new Queue<CommunityChests>();

        Property temp;
        Property temp2;
        Property temp3;

        int propRowCounter = 0;

        int rollValue = 0;
        int currentPlayerIndex = 0;
        int doubleCounter = 0;
        int propColorCounter = 0;
        int propCounter = 0;

        int mortColorSelected;
        public int playerCount => SelectingPlayers.playerCount;

        Property selectedPropToBuildOn;
        Property selectedPropToMortgage;

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

        public Vector2 housePositions(string color, int houseNumber, Property prop, int propNumb, bool rotate)
        {
            bool sideways = false;
            bool upsideDown = false;
            bool gotFlipedTurnedUpsideDown = false;
            Vector2 position = new Vector2(0);
            color = color.ToLower();
            switch (color)
            {
                case "purple":
                    if (propNumb == 1)
                    {
                        position = new Vector2(1200, 869);
                    }
                    else
                    {
                        position = new Vector2(1048, 869);
                    }

                    break;
                case "lightblue":

                    if (propNumb == 1)
                    {
                        position = new Vector2(819, 869);
                    }
                    else if (propNumb == 2)
                    {
                        position = new Vector2(668, 869);
                    }
                    else
                    {
                        position = new Vector2(593, 869);
                    }

                    break;
                case "pink":
                    sideways = true;

                    if (propNumb == 1)
                    {
                        position = new Vector2(603, 799);
                    }
                    else if (propNumb == 2)
                    {
                        position = new Vector2(603, 648);
                    }
                    else
                    {
                        position = new Vector2(603, 572);
                    }

                    break;
                case "orange":
                    sideways = true;

                    if (propNumb == 1)
                    {
                        position = new Vector2(603, 419);
                    }
                    else if (propNumb == 2)
                    {
                        position = new Vector2(603, 268);
                    }
                    else
                    {
                        position = new Vector2(603, 192);
                    }

                    break;
                case "red":
                    upsideDown = true;

                    if (propNumb == 1)
                    {
                        position = new Vector2(643, 160);
                    }
                    else if (propNumb == 2)
                    {
                        position = new Vector2(794, 160);
                    }
                    else
                    {
                        position = new Vector2(869, 160);
                    }

                    break;
                case "yellow":
                    upsideDown = true;

                    if (propNumb == 1)
                    {
                        position = new Vector2(1022, 160);
                    }
                    else if (propNumb == 2)
                    {
                        position = new Vector2(1098, 160);
                    }
                    else
                    {
                        position = new Vector2(1250, 160);
                    }

                    break;
                case "green":
                    sideways = true;
                    gotFlipedTurnedUpsideDown = true;

                    if (propNumb == 1)
                    {
                        //1274, 249
                        position = new Vector2(1311, 243);
                    }
                    else if (propNumb == 2)
                    {
                        position = new Vector2(1311, 320);
                    }
                    else
                    {
                        position = new Vector2(1311, 472);
                    }

                    break;
                case "blue":
                    sideways = true;
                    gotFlipedTurnedUpsideDown = true;

                    if (propNumb == 1)
                    {
                        position = new Vector2(1311, 699);
                    }
                    else
                    {
                        position = new Vector2(1311, 850);
                    }

                    break;
            }

            if (position != Vector2.Zero)
            {
                if (sideways)
                {
                    houseToRotate = true;

                    if (!gotFlipedTurnedUpsideDown)
                    {
                        position.Y += (20 * (houseNumber - 1));

                        position.X -= 20;

                        return position;
                    }

                    position.Y -= (20 * (houseNumber - 1));

                    position.X -= 20;

                    return position;

                }
                else if (upsideDown)
                {
                    position.X -= (20 * (houseNumber - 1));

                }
                else
                {
                    houseToRotate = false;
                    position.X += (20 * (houseNumber - 1));
                }


                return position;
            }

            return new Vector2(-1);
        }

        public Vector2 hotelPositions(string color, Property prop, int propNumb, bool rotate)
        {
            bool sideways = false;
            Vector2 position = new Vector2(0);
            color = color.ToLower();
            switch (color)
            {
                //add 22
                case "purple":
                    if (propNumb == 1)
                    {
                        position = new Vector2(1220, 869);
                    }
                    else
                    {
                        position = new Vector2(1068, 869);
                    }

                    break;
                case "lightblue":

                    if (propNumb == 1)
                    {
                        position = new Vector2(841, 869);
                    }
                    else if (propNumb == 2)
                    {
                        position = new Vector2(690, 869);
                    }
                    else
                    {
                        position = new Vector2(614, 869);
                    }

                    break;
                case "pink":
                    sideways = true;
                    //603 799
                    if (propNumb == 1)
                    {
                        position = new Vector2(577, 830);
                    }
                    else if (propNumb == 2)
                    {
                        position = new Vector2(577, 679);
                    }
                    else
                    {
                        position = new Vector2(577, 603);
                    }

                    break;
                case "orange":
                    sideways = true;

                    if (propNumb == 1)
                    {
                        position = new Vector2(577, 450);
                    }
                    else if (propNumb == 2)
                    {
                        position = new Vector2(577, 299);
                    }
                    else
                    {
                        position = new Vector2(577, 223);
                    }

                    break;
                case "red":
                    if (propNumb == 1)
                    {
                        position = new Vector2(613, 158);
                    }
                    else if (propNumb == 2)
                    {
                        position = new Vector2(764, 158);
                    }
                    else
                    {
                        position = new Vector2(839, 158);
                    }

                    break;
                case "yellow":

                    if (propNumb == 1)
                    {
                        position = new Vector2(992, 158);
                    }
                    else if (propNumb == 2)
                    {
                        position = new Vector2(1068, 158);
                    }
                    else
                    {
                        position = new Vector2(1220, 158);
                    }

                    break;
                case "green":
                    sideways = true;

                    if (propNumb == 1)
                    {
                        //1274, 249
                        position = new Vector2(1287, 223);
                    }
                    else if (propNumb == 2)
                    {
                        position = new Vector2(1287, 300);
                    }
                    else
                    {
                        position = new Vector2(1287, 452);
                    }

                    break;
                case "blue":
                    sideways = true;

                    if (propNumb == 1)
                    {
                        position = new Vector2(1287, 679);
                    }
                    else
                    {
                        position = new Vector2(1287, 830);
                    }

                    break;
            }
            if (position != Vector2.Zero)
            {
                if (sideways)
                {
                    hotelToRotate = true;
                    return position;
                }
                else
                {
                    hotelToRotate = false;
                }


                return position;
            }
            return new Vector2(-1);
        }

        public bool isReadyToSellHouse(Property selectedPropToMortgage)
        {
            List<Property> tempProps = getAllPropColor(selectedPropToMortgage.Color(selectedPropToMortgage.PropColor));


            int maxHouseCounter = tempProps[0].houseCounter;
            bool e = false;


            foreach (var prop in tempProps)
            {
                if (prop.houses.Count > maxHouseCounter)
                {
                    maxHouseCounter = prop.houses.Count;
                    e = true;
                    if (prop == selectedPropToMortgage)
                    {
                        return true;
                    }
                }
                if (prop.houses.Count < maxHouseCounter && prop == selectedPropToMortgage)
                {
                    return false;
                }
            }

            if (!e)
            {
                return true;
            }

            return false;
        }
        //unmortgage bugs

        static float Lerp(float start_value, float end_value, float pct)
        {
            return (start_value + (end_value - start_value) * pct);
        }

        public bool hasHouse(Property anotherTempInTheWall)
        {
            if (anotherTempInTheWall == null)
            {
                return false;
            }

            return anotherTempInTheWall.houseCounter != 0;
        }

        public bool hasHotel(Property weDontNeedNoTempControl)
        {
            if (weDontNeedNoTempControl == null)
            {
                return false;
            }

            return weDontNeedNoTempControl.hotelCounter != 0;
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

        Texture2D inGameHouseIcon;
        Texture2D inGameHotelIcon;

        SpriteFont font;
        SpriteFont mediumSizeFont;
        #endregion

        #region HighlightButtons
        HighlightButton yesButton;
        HighlightButton noButton;
        HighlightButton breakOutOfJailButton;
        HighlightButton exitHouseMenu;
        HighlightButton exitMortgageMenu;
        NormalButton diceOnBoard1;
        HighlightButton house;
        HighlightButton mortgageYesButton;

        HighlightButton PurpleProp;
        HighlightButton LightBlueProp;
        HighlightButton PinkProp;
        HighlightButton OrangeProp;
        HighlightButton RedProp;
        HighlightButton YellowProp;
        HighlightButton GreenProp;
        HighlightButton BlueProp;

        List<HighlightButton> MortProps = new List<HighlightButton>();

        HighlightButton hotelIcon;
        HighlightButton houseIcony;

        HighlightButton mortHotelIcon;
        HighlightButton mortHouseIcon;

        HighlightButton getOutOfJailFree;
        HighlightButton getOutOfJailFree2;

        HighlightButton buyHouses;
        HighlightButton mortgageProps;
        HighlightButton unmortgageYesButton;

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
            uint delay = 10;
            dice1 = new Animation(RedDice, new Vector2(800, 430), delay, new Random(gen.Next()));
            dice2 = new Animation(RedDice, new Vector2(600, 430), delay, new Random(gen.Next()));
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

            houseIcon = Content.Load<Texture2D>("pieceGreen_border07");
            house = new HighlightButton(houseIcon, new Vector2(50, 890), Color.White, new Vector2(2));

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

            mortHotelIcon = new HighlightButton(Content.Load<Texture2D>("monopolyHotel"), new Vector2(950, 680), Color.White, new Vector2(0.15f));
            mortHouseIcon = new HighlightButton(Content.Load<Texture2D>("housey2"), new Vector2(700, 700), Color.White, Vector2.One);

            PurpleProp = new HighlightButton(PurplePropSprite, new Vector2(380, 270), Color.White, Vector2.One);
            LightBlueProp = new HighlightButton(LightBluePropSprite, new Vector2(660, 270), Color.White, Vector2.One);
            PinkProp = new HighlightButton(PinkPropSprite, new Vector2(940, 270), Color.White, Vector2.One);
            OrangeProp = new HighlightButton(OrangePropSprite, new Vector2(1220, 270), Color.White, Vector2.One);

            RedProp = new HighlightButton(RedPropSprite, new Vector2(380, 600), Color.White, Vector2.One);
            YellowProp = new HighlightButton(YellowPropSprite, new Vector2(660, 600), Color.White, Vector2.One);
            GreenProp = new HighlightButton(GreenPropSprite, new Vector2(940, 600), Color.White, Vector2.One);
            BlueProp = new HighlightButton(BluePropSprite, new Vector2(1220, 600), Color.White, Vector2.One);


            MortProps.Add(new HighlightButton(PurplePropSprite, new Vector2(380, 270), Color.White, Vector2.One));
            MortProps.Add(new HighlightButton(LightBluePropSprite, new Vector2(660, 270), Color.White, Vector2.One));
            MortProps.Add(new HighlightButton(PinkPropSprite, new Vector2(940, 270), Color.White, Vector2.One));
            MortProps.Add(new HighlightButton(OrangePropSprite, new Vector2(1220, 270), Color.White, Vector2.One));

            MortProps.Add(new HighlightButton(RedPropSprite, new Vector2(380, 600), Color.White, Vector2.One));
            MortProps.Add(new HighlightButton(YellowPropSprite, new Vector2(660, 600), Color.White, Vector2.One));
            MortProps.Add(new HighlightButton(GreenPropSprite, new Vector2(940, 600), Color.White, Vector2.One));
            MortProps.Add(new HighlightButton(BluePropSprite, new Vector2(1220, 600), Color.White, Vector2.One));


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

            propertySprites.Add("purple1", new HighlightButton(Content.Load<Texture2D>("MediterraneanAve"), new Vector2(535, 300), Color.White, Vector2.One));
            propertySprites.Add("purple2", new HighlightButton(Content.Load<Texture2D>("BalticAve"), new Vector2(1085, 300), Color.White, Vector2.One));

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

            propertySprites.Add("blue1", new HighlightButton(Content.Load<Texture2D>("ParkPlace"), new Vector2(535, 300), Color.White, Vector2.One));
            propertySprites.Add("blue2", new HighlightButton(Content.Load<Texture2D>("Boardwalk"), new Vector2(1085, 300), Color.White, Vector2.One));

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

            inGameHouseIcon = Content.Load<Texture2D>("inGameHouse");
            inGameHotelIcon = Content.Load<Texture2D>("inGameHotel3");

            noButton = new HighlightButton(No, new Vector2(326, 662), Color.White, Vector2.One);
            yesButton = new HighlightButton(Yes, new Vector2(356, 662), Color.White, Vector2.One);
            breakOutOfJailButton = new HighlightButton(Yes, new Vector2(365, 755), Color.White, new Vector2(1.2f));
            exitHouseMenu = new HighlightButton(No, new Vector2(1496, 55), Color.White, new Vector2(1.5f));
            exitMortgageMenu = new HighlightButton(No, new Vector2(1496, 55), Color.White, new Vector2(1.5f));
            buyHouses = new HighlightButton(Yes, new Vector2(880, 870), Color.White, new Vector2(3f));
            mortgageProps = new HighlightButton(Content.Load<Texture2D>("mortgageIcon"), new Vector2(230, 900), Color.White, new Vector2(0.32f));
            mortgageYesButton = new HighlightButton(Yes, new Vector2(890, 870), Color.White, new Vector2(3f));
            unmortgageYesButton = new HighlightButton(Yes, new Vector2(1080, 195), Color.White, new Vector2(1.5f));

            Bounds = bounds;
        }

        ////////////UPDATE\\\\\\\\\\\\
        public void Update(MouseState ms, GameTime gameTime)
        {
            int iterations = 1;
            for (int dskaijdisajd = 0; dskaijdisajd < iterations; dskaijdisajd++)
            {
                if (tearDownTheWall)
                {
                    tearDownTheWall = !(ms.LeftButton == ButtonState.Released);

                }

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
                    Console.WriteLine($"X: {ms.X}, Y: {ms.Y}");
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
                if (((house.IsClicked && (!diceMoving && !diceRolling && !characterMoving)) || houseMenuUp) && !mortgaging)
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

                        foreach (var prop in propertySprites)
                        {
                            prop.Value.Tint = Color.White;
                            prop.Value.stayHighlighted = false;
                            prop.Value.stopBeingHighlighted = true;
                        }

                        hotelIcon.Tint = Color.Gray;
                        houseIcony.Tint = Color.Gray;

                        hotelIcon.stopBeingHighlighted = true;
                        houseIcony.stopBeingHighlighted = true;
                    }
                }

                #endregion

                if ((mortgageProps.IsClicked && (!diceMoving && !diceRolling && !characterMoving)) && !buyingHouses)
                {
                    mortgaging = true;
                }

                if (mortgaging)
                {
                    darkenScreen = true;

                    mortMenuStage1 = ((!mortMenuStage2 && !mortMenuStage3) && !unmortMenuStage1);
                    if (mortMenuStage3)
                    {
                        mortMenuStage2 = false;
                    }



                    if (mortMenuStage1)
                    {
                        for (int i = 0; i < MortProps.Count; i++)
                        {
                            if (MortProps[i].IsClicked && CurrentPlayer.oneOfOneColor(i))
                            {
                                mortColorSelected = i;
                                mortMenuStage2 = true;
                                tearDownTheWall = true;
                            }
                        }

                        if (unmortgageYesButton.IsClicked)
                        {
                            unmortMenuStage1 = true;
                        }
                    }
                    else if (mortMenuStage3)
                    {
                        if (mortgageYesButton.IsClicked && selectedMortgageOption > 0)
                        {
                            if (selectedMortgageOption == 1)
                            {
                                //house

                                if (isReadyToSellHouse(selectedPropToMortgage))
                                {
                                    selectedPropToMortgage.houses.RemoveAt(selectedPropToMortgage.houses.Count - 1);
                                    CurrentPlayer.Money += (selectedPropToMortgage.HouseCost / 2);
                                    selectedPropToMortgage.houseCounter--;
                                }

                            }
                            else if (selectedMortgageOption == 2)
                            {
                                selectedPropToMortgage.hotels.RemoveAt(0);
                                CurrentPlayer.Money += (selectedPropToMortgage.HotelCost / 2);
                                //add 4 houses
                            }
                            else
                            {
                                selectedPropToMortgage.isMortgaged = true;
                                CurrentPlayer.Money += (selectedPropToMortgage.Cost / 2);
                                selectedPropToMortgage.Tint = Color.Gray;
                                if (selectedPropToMortgage.houseCounter > 0)
                                {
                                    CurrentPlayer.Money += selectedPropToMortgage.houseCounter * (selectedPropToMortgage.HouseCost / 2);
                                    selectedPropToMortgage.houses.Clear();
                                    selectedPropToMortgage.houseCounter = 0;
                                }
                            }

                            mortgaging = false;
                            darkenScreen = false;
                            mortMenuStage1 = false;
                            mortMenuStage2 = false;
                            mortMenuStage3 = false;
                            selectedPropToMortgage = null;

                            bricksInTheWall = 0;
                            selectedMortgageOption = 0;
                        }
                    }

                    if (unmortMenuStage1)
                    {
                        for (int i = 0; i < MortProps.Count; i++)
                        {
                            if (MortProps[i].IsClicked && CurrentPlayer.oneOfOneColorMort(i))
                            {
                                mortColorSelected = i;
                                unmortMenuStage2 = true;
                                unmortMenuStage1 = false;
                                tearDownTheWall = true;
                            }
                        }
                    }


                    if (exitMortgageMenu.IsClicked)
                    {
                        mortgaging = false;
                        darkenScreen = false;
                        mortMenuStage1 = false;
                        mortMenuStage2 = false;
                        mortMenuStage3 = false;

                        unmortMenuStage1 = false;
                        unmortMenuStage2 = false;
                        unmortMenuStage3 = false;

                        bricksInTheWall = 0;
                        selectedMortgageOption = 0;

                        //remove all tints
                        #region remove Tints
                        foreach (var prop in propertySprites)
                        {
                            prop.Value.Tint = Color.White;
                        }

                        #endregion
                        //exit
                    }
                }

                #region Updates 
                CurrentPlayer.Update();
                noButton.Update(ms, true);
                yesButton.Update(ms, true);
                breakOutOfJailButton.Update(ms, getOutOfJailGlow);
                buyHouses.Update(ms, readyToBuy);
                mortgageProps.Update(ms, true);
                mortgageYesButton.Update(ms, true);
                unmortgageYesButton.Update(ms, true);

                exitHouseMenu.Update(ms, true);

                PurpleProp.Update(ms, CurrentPlayer.allOfOneColor("purple"));
                LightBlueProp.Update(ms, CurrentPlayer.allOfOneColor("lightBlue"));
                PinkProp.Update(ms, CurrentPlayer.allOfOneColor("pink"));
                OrangeProp.Update(ms, CurrentPlayer.allOfOneColor("orange"));
                RedProp.Update(ms, CurrentPlayer.allOfOneColor("red"));
                YellowProp.Update(ms, CurrentPlayer.allOfOneColor("yellow"));
                GreenProp.Update(ms, CurrentPlayer.allOfOneColor("green"));
                BlueProp.Update(ms, CurrentPlayer.allOfOneColor("blue"));

                if (mortMenuStage1)
                {
                    for (int i = 0; i < MortProps.Count; i++)
                    {
                        MortProps[i].Update(ms, CurrentPlayer.oneOfOneColor(i));
                    }
                }
                else if (unmortMenuStage1)
                {
                    for (int i = 0; i < MortProps.Count; i++)
                    {
                        MortProps[i].Update(ms, CurrentPlayer.oneOfOneColorMort(i));
                    }
                }


                house.Update(ms, glowingHouse);
                getOutOfJailFree.Update(ms, getOutOfJailGlow);
                getOutOfJailFree2.Update(ms, getOutOfJailGlow);

                propertySprites[$"purple1"].Update(ms, false);
                propertySprites[$"purple2"].Update(ms, false);
                propertySprites[$"lightBlue1"].Update(ms, false);
                propertySprites[$"lightBlue2"].Update(ms, false);
                propertySprites[$"lightBlue3"].Update(ms, false);
                propertySprites[$"pink1"].Update(ms, false);
                propertySprites[$"pink2"].Update(ms, false);
                propertySprites[$"pink3"].Update(ms, false);
                propertySprites[$"orange1"].Update(ms, false);
                propertySprites[$"orange2"].Update(ms, false);
                propertySprites[$"orange3"].Update(ms, false);
                propertySprites[$"red1"].Update(ms, false);
                propertySprites[$"red2"].Update(ms, false);
                propertySprites[$"red3"].Update(ms, false);
                propertySprites[$"yellow1"].Update(ms, false);
                propertySprites[$"yellow2"].Update(ms, false);
                propertySprites[$"yellow3"].Update(ms, false);
                propertySprites[$"green1"].Update(ms, false);
                propertySprites[$"green2"].Update(ms, false);
                propertySprites[$"green3"].Update(ms, false);
                propertySprites[$"blue1"].Update(ms, false);
                propertySprites[$"blue2"].Update(ms, false);

                exitMortgageMenu.Update(ms, true);

                hotelIcon.Update(ms, readyForHotel);
                houseIcony.Update(ms, isReadyForHouse(selectedPropToBuildOn));

                mortHotelIcon.Update(ms, hasHotel(selectedPropToMortgage));
                if (!hasHotel(selectedPropToMortgage))
                {
                    mortHotelIcon.Tint = Color.Gray;
                }
                else
                {
                    mortHotelIcon.Tint = Color.White;
                }

                mortHouseIcon.Update(ms, (hasHouse(selectedPropToMortgage)) && (isReadyToSellHouse(selectedPropToMortgage)));

                if (!hasHouse(selectedPropToMortgage))
                {
                    mortHouseIcon.Tint = Color.Gray;
                }
                else
                {
                    mortHouseIcon.Tint = Color.White;
                }

                foreach (var Player in Players)
                {
                    foreach (var property in Player.properties)
                    {
                        foreach (var house in property.houses)
                        {
                            house.Update(ms, false);
                        }

                        foreach (var hotel in property.hotels)
                        {
                            hotel.Update(ms, false);
                        }
                    }
                }

                theWall?.Update(ms, true);
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

                    if (diceOnBoard1.IsClicked(ms) && (!buyingHouses && (!mortMenuStage1 && !mortMenuStage2 && !mortMenuStage3)))
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
                            if (CurrentPlayer.currentTileIndex == 1)
                            {
                                target = 2;
                            }
                            else if (CurrentPlayer.currentTileIndex == 2)
                            {
                                target = 4;
                            }
                            else if (CurrentPlayer.currentTileIndex == 4)
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

                    else if (CurrentPlayer.currentTileIndex == 39 && !moneyStolenOnce)
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
                                        if ((BoughtProperties.ContainsKey(CurrentPlayer.Position) && Players[i].properties[j] == BoughtProperties[CurrentPlayer.Position] && Rent))
                                        {
                                            if (BoughtProperties[CurrentPlayer.Position].isUtility && !Players[i].properties[j].isMortgaged)
                                            {
                                                CurrentPlayer.Money -= BoughtProperties[CurrentPlayer.Position].Rent * rollValue;
                                                Players[i].Money += BoughtProperties[CurrentPlayer.Position].Rent * rollValue; ;
                                            }
                                            else if (!Players[i].properties[j].isMortgaged)
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
            mortgageProps.Draw(batch);

            if (Players[0] != null)
            {
                foreach (var Player in Players)
                {
                    foreach (var property in Player.properties)
                    {
                        foreach (var house in property.houses)
                        {
                            house.Draw(batch);
                        }

                        foreach (var hotel in property.hotels)
                        {
                            hotel.Draw(batch);
                        }
                    }
                }
            }
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

                        if (propertySprites[$"purple1"].IsClicked && !findProp("MediterraneanAve").isMortgaged)
                        {
                            propertySprites[$"purple1"].stayHighlighted = true;

                            selectedPropToBuildOn = findProp("MediterraneanAve");

                            propertySprites[$"purple2"].stayHighlighted = false;
                            propertySprites[$"purple2"].stopBeingHighlighted = true;
                            propColorCounter = 1;
                            propCounter = 1;
                        }
                        else if (findProp("MediterraneanAve").isMortgaged)
                        {
                            propertySprites[$"purple1"].stayHighlighted = false;
                            propertySprites[$"purple1"].stopBeingHighlighted = true;
                            propertySprites[$"purple1"].Tint = Color.Red;
                        }
                        if (propertySprites[$"purple2"].IsClicked && !findProp("BalticAve").isMortgaged)
                        {
                            propertySprites[$"purple2"].stayHighlighted = true;

                            selectedPropToBuildOn = findProp("BalticAve");

                            propertySprites[$"purple1"].stayHighlighted = false;
                            propertySprites[$"purple1"].stopBeingHighlighted = true;
                            propColorCounter = 2;
                            propCounter = 2;
                        }
                        else if (findProp("BalticAve").isMortgaged)
                        {
                            propertySprites[$"purple2"].stayHighlighted = false;
                            propertySprites[$"purple2"].stopBeingHighlighted = true;
                            propertySprites[$"purple2"].Tint = Color.Red;
                        }
                    }
                    else if (lightBluePropMenu)
                    {
                        propertySprites[$"lightBlue1"].Draw(batch);
                        propertySprites[$"lightBlue2"].Draw(batch);
                        propertySprites[$"lightBlue3"].Draw(batch);

                        if (propertySprites[$"lightBlue1"].IsClicked && !findProp("OrientalAve").isMortgaged)
                        {
                            propertySprites[$"lightBlue1"].stayHighlighted = true;

                            selectedPropToBuildOn = findProp("OrientalAve");

                            propertySprites[$"lightBlue2"].stayHighlighted = false;
                            propertySprites[$"lightBlue2"].stopBeingHighlighted = true;

                            propertySprites[$"lightBlue3"].stayHighlighted = false;
                            propertySprites[$"lightBlue3"].stopBeingHighlighted = true;
                            propColorCounter = 1;
                            propCounter = 3;
                        }
                        else if (findProp("OrientalAve").isMortgaged)
                        {
                            propertySprites[$"lightBlue1"].stayHighlighted = false;
                            propertySprites[$"lightBlue1"].stopBeingHighlighted = true;
                            propertySprites[$"lightBlue1"].Tint = Color.Red;
                        }
                        if (propertySprites[$"lightBlue2"].IsClicked && !findProp("VermontAve").isMortgaged)
                        {
                            propertySprites[$"lightBlue2"].stayHighlighted = true;

                            selectedPropToBuildOn = findProp("VermontAve");

                            propertySprites[$"lightBlue1"].stayHighlighted = false;
                            propertySprites[$"lightBlue1"].stopBeingHighlighted = true;

                            propertySprites[$"lightBlue3"].stayHighlighted = false;
                            propertySprites[$"lightBlue3"].stopBeingHighlighted = true;
                            propColorCounter = 2;
                            propCounter = 4;
                        }
                        else if (findProp("VermontAve").isMortgaged)
                        {
                            propertySprites[$"lightBlue2"].stayHighlighted = false;
                            propertySprites[$"lightBlue2"].stopBeingHighlighted = true;
                            propertySprites[$"lightBlue2"].Tint = Color.Red;
                        }
                        if (propertySprites[$"lightBlue3"].IsClicked && !findProp("ConnecticutAve").isMortgaged)
                        {
                            propertySprites[$"lightBlue3"].stayHighlighted = true;

                            selectedPropToBuildOn = findProp("ConnecticutAve");

                            propertySprites[$"lightBlue1"].stayHighlighted = false;
                            propertySprites[$"lightBlue1"].stopBeingHighlighted = true;

                            propertySprites[$"lightBlue2"].stayHighlighted = false;
                            propertySprites[$"lightBlue2"].stopBeingHighlighted = true;
                            propColorCounter = 3;
                            propCounter = 5;
                        }
                        else if (findProp("ConnecticutAve").isMortgaged)
                        {
                            propertySprites[$"lightBlue3"].stayHighlighted = false;
                            propertySprites[$"lightBlue3"].stopBeingHighlighted = true;
                            propertySprites[$"lightBlue3"].Tint = Color.Red;
                        }
                    }
                    else if (pinkPropMenu)
                    {
                        propertySprites[$"pink1"].Draw(batch);
                        propertySprites[$"pink2"].Draw(batch);
                        propertySprites[$"pink3"].Draw(batch);

                        if (propertySprites[$"pink1"].IsClicked && !findProp("StCharlesPlace").isMortgaged)
                        {
                            propertySprites[$"pink1"].stayHighlighted = true;

                            selectedPropToBuildOn = findProp("StCharlesPlace");

                            propertySprites[$"pink2"].stayHighlighted = false;
                            propertySprites[$"pink2"].stopBeingHighlighted = true;

                            propertySprites[$"pink3"].stayHighlighted = false;
                            propertySprites[$"pink3"].stopBeingHighlighted = true;
                            propColorCounter = 1;
                            propCounter = 6;
                        }
                        else if (findProp("StCharlesPlace").isMortgaged)
                        {
                            propertySprites[$"pink1"].stayHighlighted = false;
                            propertySprites[$"pink1"].stopBeingHighlighted = true;
                            propertySprites[$"pink1"].Tint = Color.Red;
                        }
                        if (propertySprites[$"pink2"].IsClicked && !findProp("StatesAve").isMortgaged)
                        {
                            propertySprites[$"pink2"].stayHighlighted = true;

                            selectedPropToBuildOn = findProp("StatesAve");

                            propertySprites[$"pink1"].stayHighlighted = false;
                            propertySprites[$"pink1"].stopBeingHighlighted = true;

                            propertySprites[$"pink3"].stayHighlighted = false;
                            propertySprites[$"pink3"].stopBeingHighlighted = true;
                            propColorCounter = 2;
                            propCounter = 7;
                        }
                        else if (findProp("StatesAve").isMortgaged)
                        {
                            propertySprites[$"pink2"].stayHighlighted = false;
                            propertySprites[$"pink2"].stopBeingHighlighted = true;
                            propertySprites[$"pink2"].Tint = Color.Red;
                        }
                        if (propertySprites[$"pink3"].IsClicked && !findProp("VirginiaAve").isMortgaged)
                        {
                            propertySprites[$"pink3"].stayHighlighted = true;

                            selectedPropToBuildOn = findProp("VirginiaAve");

                            propertySprites[$"pink1"].stayHighlighted = false;
                            propertySprites[$"pink1"].stopBeingHighlighted = true;

                            propertySprites[$"pink2"].stayHighlighted = false;
                            propertySprites[$"pink2"].stopBeingHighlighted = true;
                            propColorCounter = 3;
                            propCounter = 8;
                        }
                        else if (findProp("VirginiaAve").isMortgaged)
                        {
                            propertySprites[$"pink3"].stayHighlighted = false;
                            propertySprites[$"pink3"].stopBeingHighlighted = true;
                            propertySprites[$"pink3"].Tint = Color.Red;
                        }
                    }
                    else if (orangePropMenu)
                    {
                        propertySprites[$"orange1"].Draw(batch);
                        propertySprites[$"orange2"].Draw(batch);
                        propertySprites[$"orange3"].Draw(batch);

                        if (propertySprites[$"orange1"].IsClicked && !findProp("StJamesPlace").isMortgaged)
                        {
                            propertySprites[$"orange1"].stayHighlighted = true;

                            selectedPropToBuildOn = findProp("StJamesPlace");

                            propertySprites[$"orange2"].stayHighlighted = false;
                            propertySprites[$"orange2"].stopBeingHighlighted = true;

                            propertySprites[$"orange3"].stayHighlighted = false;
                            propertySprites[$"orange3"].stopBeingHighlighted = true;
                            propColorCounter = 1;
                            propCounter = 9;
                        }
                        else if (findProp("StJamesPlace").isMortgaged)
                        {
                            propertySprites[$"orange1"].stayHighlighted = false;
                            propertySprites[$"orange1"].stopBeingHighlighted = true;
                            propertySprites[$"orange1"].Tint = Color.Red;
                        }
                        if (propertySprites[$"orange2"].IsClicked && !findProp("TennesseeAve").isMortgaged)
                        {
                            propertySprites[$"orange2"].stayHighlighted = true;

                            selectedPropToBuildOn = findProp("TennesseeAve");

                            propertySprites[$"orange1"].stayHighlighted = false;
                            propertySprites[$"orange1"].stopBeingHighlighted = true;

                            propertySprites[$"orange3"].stayHighlighted = false;
                            propertySprites[$"orange3"].stopBeingHighlighted = true;
                            propColorCounter = 2;
                            propCounter = 10;
                        }
                        else if (findProp("TennesseeAve").isMortgaged)
                        {
                            propertySprites[$"orange2"].stayHighlighted = false;
                            propertySprites[$"orange2"].stopBeingHighlighted = true;
                            propertySprites[$"orange2"].Tint = Color.Red;
                        }
                        if (propertySprites[$"orange3"].IsClicked && !findProp("NewYorkAve").isMortgaged)
                        {
                            propertySprites[$"orange3"].stayHighlighted = true;

                            selectedPropToBuildOn = findProp("NewYorkAve");

                            propertySprites[$"orange1"].stayHighlighted = false;
                            propertySprites[$"orange1"].stopBeingHighlighted = true;

                            propertySprites[$"orange2"].stayHighlighted = false;
                            propertySprites[$"orange2"].stopBeingHighlighted = true;
                            propColorCounter = 3;
                            propCounter = 11;
                        }
                        else if (findProp("NewYorkAve").isMortgaged)
                        {
                            propertySprites[$"orange3"].stayHighlighted = false;
                            propertySprites[$"orange3"].stopBeingHighlighted = true;
                            propertySprites[$"orange3"].Tint = Color.Red;
                        }
                    }
                    else if (redPropMenu)
                    {
                        propertySprites[$"red1"].Draw(batch);
                        propertySprites[$"red2"].Draw(batch);
                        propertySprites[$"red3"].Draw(batch);

                        if (propertySprites[$"red1"].IsClicked && !findProp("KentuckyAve").isMortgaged)
                        {
                            propertySprites[$"red1"].stayHighlighted = true;

                            selectedPropToBuildOn = findProp("KentuckyAve");

                            propertySprites[$"red2"].stayHighlighted = false;
                            propertySprites[$"red2"].stopBeingHighlighted = true;

                            propertySprites[$"red3"].stayHighlighted = false;
                            propertySprites[$"red3"].stopBeingHighlighted = true;
                            propColorCounter = 1;
                            propCounter = 12;
                        }
                        else if (findProp("KentuckyAve").isMortgaged)
                        {
                            propertySprites[$"red1"].stayHighlighted = false;
                            propertySprites[$"red1"].stopBeingHighlighted = true;
                            propertySprites[$"red1"].Tint = Color.Red;
                        }
                        if (propertySprites[$"red2"].IsClicked && !findProp("IndianaAve").isMortgaged)
                        {
                            propertySprites[$"red2"].stayHighlighted = true;

                            selectedPropToBuildOn = findProp("IndianaAve");

                            propertySprites[$"red1"].stayHighlighted = false;
                            propertySprites[$"red1"].stopBeingHighlighted = true;

                            propertySprites[$"red3"].stayHighlighted = false;
                            propertySprites[$"red3"].stopBeingHighlighted = true;
                            propColorCounter = 2;
                            propCounter = 13;
                        }
                        else if (findProp("IndianaAve").isMortgaged)
                        {
                            propertySprites[$"red2"].stayHighlighted = false;
                            propertySprites[$"red2"].stopBeingHighlighted = true;
                            propertySprites[$"red2"].Tint = Color.Red;
                        }
                        if (propertySprites[$"red3"].IsClicked && !findProp("IllinoisAve").isMortgaged)
                        {
                            propertySprites[$"red3"].stayHighlighted = true;

                            selectedPropToBuildOn = findProp("IllinoisAve");

                            propertySprites[$"red1"].stayHighlighted = false;
                            propertySprites[$"red1"].stopBeingHighlighted = true;

                            propertySprites[$"red2"].stayHighlighted = false;
                            propertySprites[$"red2"].stopBeingHighlighted = true;
                            propColorCounter = 3;
                            propCounter = 14;
                        }
                        else if (findProp("IllinoisAve").isMortgaged)
                        {
                            propertySprites[$"red3"].stayHighlighted = false;
                            propertySprites[$"red3"].stopBeingHighlighted = true;
                            propertySprites[$"red3"].Tint = Color.Red;
                        }
                    }
                    else if (yellowPropMenu)
                    {
                        propertySprites[$"yellow1"].Draw(batch);
                        propertySprites[$"yellow2"].Draw(batch);
                        propertySprites[$"yellow3"].Draw(batch);

                        if (propertySprites[$"yellow1"].IsClicked && !findProp("AtlanticAve").isMortgaged)
                        {
                            propertySprites[$"yellow1"].stayHighlighted = true;

                            selectedPropToBuildOn = findProp("AtlanticAve");

                            propertySprites[$"yellow2"].stayHighlighted = false;
                            propertySprites[$"yellow2"].stopBeingHighlighted = true;

                            propertySprites[$"yellow3"].stayHighlighted = false;
                            propertySprites[$"yellow3"].stopBeingHighlighted = true;
                            propColorCounter = 1;
                            propCounter = 15;
                        }
                        else if (findProp("AtlanticAve").isMortgaged)
                        {
                            propertySprites[$"yellow1"].stayHighlighted = false;
                            propertySprites[$"yellow1"].stopBeingHighlighted = true;
                            propertySprites[$"yellow1"].Tint = Color.Red;
                        }
                        if (propertySprites[$"yellow2"].IsClicked && !findProp("VentnorAve").isMortgaged)
                        {
                            propertySprites[$"yellow2"].stayHighlighted = true;

                            selectedPropToBuildOn = findProp("VentnorAve");

                            propertySprites[$"yellow1"].stayHighlighted = false;
                            propertySprites[$"yellow1"].stopBeingHighlighted = true;

                            propertySprites[$"yellow3"].stayHighlighted = false;
                            propertySprites[$"yellow3"].stopBeingHighlighted = true;
                            propColorCounter = 2;
                            propCounter = 16;
                        }
                        else if (findProp("VentnorAve").isMortgaged)
                        {
                            propertySprites[$"yellow2"].stayHighlighted = false;
                            propertySprites[$"yellow2"].stopBeingHighlighted = true;
                            propertySprites[$"yellow2"].Tint = Color.Red;
                        }
                        if (propertySprites[$"yellow3"].IsClicked && !findProp("MarvinGardens").isMortgaged)
                        {
                            propertySprites[$"yellow3"].stayHighlighted = true;

                            selectedPropToBuildOn = findProp("MarvinGardens");

                            propertySprites[$"yellow1"].stayHighlighted = false;
                            propertySprites[$"yellow1"].stopBeingHighlighted = true;

                            propertySprites[$"yellow2"].stayHighlighted = false;
                            propertySprites[$"yellow2"].stopBeingHighlighted = true;
                            propColorCounter = 3;
                            propCounter = 17;
                        }
                        else if (findProp("MarvinGardens").isMortgaged)
                        {
                            propertySprites[$"yellow3"].stayHighlighted = false;
                            propertySprites[$"yellow3"].stopBeingHighlighted = true;
                            propertySprites[$"yellow3"].Tint = Color.Red;
                        }
                    }
                    else if (greenPropMenu)
                    {
                        propertySprites[$"green1"].Draw(batch);
                        propertySprites[$"green2"].Draw(batch);
                        propertySprites[$"green3"].Draw(batch);

                        if (propertySprites[$"green1"].IsClicked && !findProp("PacificAve").isMortgaged)
                        {
                            propertySprites[$"green1"].stayHighlighted = true;

                            selectedPropToBuildOn = findProp("PacificAve");

                            propertySprites[$"green2"].stayHighlighted = false;
                            propertySprites[$"green2"].stopBeingHighlighted = true;

                            propertySprites[$"green3"].stayHighlighted = false;
                            propertySprites[$"green3"].stopBeingHighlighted = true;
                            propColorCounter = 1;
                            propCounter = 18;
                        }
                        else if (findProp("PacificAve").isMortgaged)
                        {
                            propertySprites[$"green1"].stayHighlighted = false;
                            propertySprites[$"green1"].stopBeingHighlighted = true;
                            propertySprites[$"green1"].Tint = Color.Red;
                        }
                        if (propertySprites[$"green2"].IsClicked && !findProp("NoCarolinaAve").isMortgaged)
                        {
                            propertySprites[$"green2"].stayHighlighted = true;

                            selectedPropToBuildOn = findProp("NoCarolinaAve");

                            propertySprites[$"green1"].stayHighlighted = false;
                            propertySprites[$"green1"].stopBeingHighlighted = true;

                            propertySprites[$"green3"].stayHighlighted = false;
                            propertySprites[$"green3"].stopBeingHighlighted = true;
                            propColorCounter = 2;
                            propCounter = 19;
                        }
                        else if (findProp("NoCarolinaAve").isMortgaged)
                        {
                            propertySprites[$"green2"].stayHighlighted = false;
                            propertySprites[$"green2"].stopBeingHighlighted = true;
                            propertySprites[$"green1"].Tint = Color.Red;
                        }
                        if (propertySprites[$"green3"].IsClicked && !findProp("PennsylvaniaAve").isMortgaged)
                        {
                            propertySprites[$"green3"].stayHighlighted = true;

                            selectedPropToBuildOn = findProp("PennsylvaniaAve");

                            propertySprites[$"green1"].stayHighlighted = false;
                            propertySprites[$"green1"].stopBeingHighlighted = true;

                            propertySprites[$"green2"].stayHighlighted = false;
                            propertySprites[$"green2"].stopBeingHighlighted = true;
                            propColorCounter = 3;
                            propCounter = 20;
                        }
                        else if (findProp("PennsylvaniaAve").isMortgaged)
                        {
                            propertySprites[$"green3"].stayHighlighted = false;
                            propertySprites[$"green3"].stopBeingHighlighted = true;
                            propertySprites[$"green3"].Tint = Color.Red;
                        }
                    }
                    else if (bluePropMenu)
                    {
                        propertySprites[$"blue1"].Draw(batch);
                        propertySprites[$"blue2"].Draw(batch);

                        if (propertySprites[$"blue1"].IsClicked && !findProp("ParkPlace").isMortgaged)
                        {
                            propertySprites[$"blue1"].stayHighlighted = true;

                            selectedPropToBuildOn = findProp("ParkPlace");

                            propertySprites[$"blue2"].stayHighlighted = false;
                            propertySprites[$"blue2"].stopBeingHighlighted = true;
                            propColorCounter = 1;
                            propCounter = 21;
                        }
                        else if (findProp("ParkPlace").isMortgaged)
                        {
                            propertySprites[$"blue1"].stayHighlighted = false;
                            propertySprites[$"blue1"].stopBeingHighlighted = true;
                            propertySprites[$"blue1"].Tint = Color.Red;
                        }
                        if (propertySprites[$"blue2"].IsClicked && !findProp("Boardwalk").isMortgaged)
                        {
                            propertySprites[$"blue2"].stayHighlighted = true;

                            selectedPropToBuildOn = findProp("Boardwalk");

                            propertySprites[$"blue1"].stayHighlighted = false;
                            propertySprites[$"blue1"].stopBeingHighlighted = true;
                            propColorCounter = 2;
                            propCounter = 22;
                        }
                        else if (findProp("Boardwalk").isMortgaged)
                        {
                            propertySprites[$"blue2"].stayHighlighted = false;
                            propertySprites[$"blue2"].stopBeingHighlighted = true;
                            propertySprites[$"blue2"].Tint = Color.Red;
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
                            hotelIcon.stayHighlighted = false;
                            hotelIcon.stopBeingHighlighted = true;
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

                                selectedPropToBuildOn.houses.Clear();

                                HighlightButton tempy2 = new HighlightButton(inGameHotelIcon, hotelPositions(selectedPropToBuildOn.Color(selectedPropToBuildOn.PropColor), selectedPropToBuildOn, propColorCounter, true), Color.White, new Vector2(0.3f));

                                if (hotelToRotate)
                                {
                                    tempy2.rotation = 1.5708f;
                                    tempy2.origin = new Vector2((inGameHotelIcon.Width / 2), (inGameHotelIcon.Height / 2));
                                }

                                selectedPropToBuildOn.hotels.Add(tempy2);
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

                                HighlightButton tempy = new HighlightButton(inGameHouseIcon, housePositions(selectedPropToBuildOn.Color(selectedPropToBuildOn.PropColor), selectedPropToBuildOn.houseCounter, selectedPropToBuildOn, propColorCounter, true), Color.White, new Vector2(0.13f));

                                if (houseToRotate)
                                {
                                    tempy.rotation = 1.5708f;
                                    tempy.origin = new Vector2((inGameHotelIcon.Width / 2), (inGameHotelIcon.Height / 2));
                                }

                                selectedPropToBuildOn.houses.Add(tempy);

                            }
                            houseMenuStage2 = false;
                            imDone = true;
                            selectedPropToBuildOn = null;
                            readyToBuy = false;

                            hotelIcon.Tint = Color.Gray;
                            houseIcony.Tint = Color.Gray;

                            hotelIcon.stopBeingHighlighted = true;
                            houseIcony.stopBeingHighlighted = true;
                            houseIcony.stayHighlighted = false;
                            hotelIcon.stayHighlighted = false;
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

            if (mortgaging)
            {
                batch.Draw(houseBuyingUI, new Vector2(330, 55), Color.White);
                if (!unmortMenuStage1 && !unmortMenuStage2 && !unmortMenuStage3)
                {
                    batch.DrawString(mediumSizeFont, "Select which property you want to mortgage.", new Vector2(560, 110), Color.Black);
                }
                else
                {
                    batch.DrawString(mediumSizeFont, "Select which property you want to unmortgage.", new Vector2(550, 110), Color.Black);
                }

                #region Normal Mortgage Menu
                if (mortMenuStage1)
                {
                    batch.DrawString(font, "Press here to unmortgage", new Vector2(750, 190), Color.Black);

                    unmortgageYesButton.Draw(batch);

                    for (int i = 0; i < MortProps.Count; i++)
                    {
                        if (CurrentPlayer.oneOfOneColor(i))
                        {
                            MortProps[i].Tint = Color.White;
                        }
                        else
                        {
                            MortProps[i].Tint = Color.Gray;
                        }

                        MortProps[i].Draw(batch);
                    }
                }

                else if (mortMenuStage2)
                {
                    if (!tearDownTheWall)
                    {
                        switch (mortColorSelected)
                        {
                            case 0:
                                propertySprites[$"purple1"].Draw(batch);
                                propertySprites[$"purple2"].Draw(batch);

                                temp = findProp("MediterraneanAve");
                                temp2 = findProp("BalticAve");
                                //if you mortgage it will let you re-mortgage fix this

                                if (temp != null && !temp.isMortgaged)
                                {
                                    if (propertySprites[$"purple1"].IsClicked)
                                    {
                                        selectedPropToMortgage = findProp("MediterraneanAve");
                                        theTrial = "purple1";

                                        propColorCounter = 1;
                                        propCounter = 1;

                                        mortMenuStage3 = true;
                                    }
                                    else
                                    {
                                        mortGlow = true;
                                        propertySprites[$"purple1"].Tint = Color.White;
                                    }
                                }
                                else
                                {
                                    propertySprites[$"purple1"].Tint = Color.Gray;
                                    mortGlow = false;
                                    propertySprites[$"purple1"].stopBeingHighlighted = true;
                                    propertySprites[$"purple1"].stayHighlighted = false;
                                }

                                if (temp2 != null && !temp2.isMortgaged)
                                {
                                    if (propertySprites[$"purple2"].IsClicked)
                                    {
                                        selectedPropToMortgage = findProp("BalticAve");
                                        theTrial = "purple2";

                                        propColorCounter = 2;
                                        propCounter = 2;

                                        mortMenuStage3 = true;
                                    }
                                    else
                                    {
                                        mortGlow = true;
                                        propertySprites[$"purple2"].Tint = Color.White;
                                    }
                                }
                                else
                                {
                                    propertySprites[$"purple2"].Tint = Color.Gray;
                                    mortGlow = false;
                                    propertySprites[$"purple2"].stopBeingHighlighted = true;
                                    propertySprites[$"purple2"].stayHighlighted = false;
                                }

                                break;
                            case 1:
                                propertySprites[$"lightBlue1"].Draw(batch);
                                propertySprites[$"lightBlue2"].Draw(batch);
                                propertySprites[$"lightBlue3"].Draw(batch);

                                temp = findProp("OrientalAve");
                                temp2 = findProp("VermontAve");
                                temp3 = findProp("ConnecticutAve");
                                if (temp != null && !temp.isMortgaged)
                                {
                                    if (propertySprites[$"lightBlue1"].IsClicked && temp != null)
                                    {
                                        selectedPropToMortgage = findProp("OrientalAve");
                                        theTrial = "lightBlue1";

                                        propCounter = 3;

                                        mortMenuStage3 = true;
                                    }
                                    else
                                    {
                                        mortGlow = true;
                                        propertySprites[$"lightBlue1"].Tint = Color.White;
                                    }
                                }
                                else
                                {
                                    propertySprites[$"lightBlue1"].Tint = Color.Gray;
                                    mortGlow = false;
                                    propertySprites[$"lightBlue1"].stopBeingHighlighted = true;
                                    propertySprites[$"lightBlue1"].stayHighlighted = false;
                                }

                                if (temp2 != null && !temp2.isMortgaged)
                                {
                                    if (propertySprites[$"lightBlue2"].IsClicked && temp2 != null)
                                    {
                                        selectedPropToMortgage = findProp("VermontAve");
                                        theTrial = "lightBlue2";

                                        propColorCounter = 2;
                                        propCounter = 4;

                                        mortMenuStage3 = true;
                                    }
                                    else
                                    {
                                        mortGlow = true;
                                        propertySprites[$"lightBlue2"].Tint = Color.White;
                                    }
                                }
                                else
                                {
                                    propertySprites[$"lightBlue2"].Tint = Color.Gray;
                                    mortGlow = false;
                                    propertySprites[$"lightBlue2"].stopBeingHighlighted = true;
                                    propertySprites[$"lightBlue2"].stayHighlighted = false;
                                }

                                if (temp3 != null && !temp3.isMortgaged)
                                {
                                    if (propertySprites[$"lightBlue3"].IsClicked && temp3 != null)
                                    {
                                        selectedPropToMortgage = findProp("ConnecticutAve");
                                        theTrial = "lightBlue3";

                                        propColorCounter = 3;
                                        propCounter = 5;

                                        mortMenuStage3 = true;
                                    }
                                    else
                                    {
                                        mortGlow = true;
                                        propertySprites[$"lightBlue3"].Tint = Color.White;
                                    }
                                }
                                else
                                {
                                    propertySprites[$"lightBlue3"].Tint = Color.Gray;
                                    mortGlow = false;
                                    propertySprites[$"lightBlue3"].stopBeingHighlighted = true;
                                    propertySprites[$"lightBlue3"].stayHighlighted = false;
                                }

                                break;
                            case 2:

                                propertySprites[$"pink1"].Draw(batch);
                                propertySprites[$"pink2"].Draw(batch);
                                propertySprites[$"pink3"].Draw(batch);

                                temp = findProp("StCharlesPlace");
                                temp2 = findProp("StatesAve");
                                temp3 = findProp("VirginiaAve");

                                if (temp != null && !temp.isMortgaged)
                                {
                                    if (propertySprites[$"pink1"].IsClicked && temp != null)
                                    {
                                        selectedPropToMortgage = findProp("StCharlesPlace");
                                        theTrial = "pink1";

                                        propColorCounter = 1;
                                        propCounter = 6;

                                        mortMenuStage3 = true;
                                    }
                                    else
                                    {
                                        mortGlow = true;
                                        propertySprites[$"pink1"].Tint = Color.White;
                                    }
                                }
                                else
                                {
                                    propertySprites[$"pink1"].Tint = Color.Gray;
                                    mortGlow = false;
                                    propertySprites[$"pink1"].stopBeingHighlighted = true;
                                    propertySprites[$"pink1"].stayHighlighted = false;
                                }

                                if (temp2 != null && !temp2.isMortgaged)
                                {
                                    if (propertySprites[$"pink2"].IsClicked && temp2 != null)
                                    {
                                        selectedPropToMortgage = findProp("StatesAve");
                                        theTrial = "pink2";

                                        propColorCounter = 2;
                                        propCounter = 7;

                                        mortMenuStage3 = true;
                                    }
                                    else
                                    {
                                        mortGlow = true;
                                        propertySprites[$"pink2"].Tint = Color.White;
                                    }
                                }
                                else
                                {
                                    propertySprites[$"pink2"].Tint = Color.Gray;
                                    mortGlow = false;
                                    propertySprites[$"pink2"].stopBeingHighlighted = true;
                                    propertySprites[$"pink2"].stayHighlighted = false;
                                }

                                if (temp3 != null && !temp3.isMortgaged)
                                {
                                    if (propertySprites[$"pink3"].IsClicked && temp3 != null)
                                    {
                                        selectedPropToMortgage = findProp("VirginiaAve");
                                        theTrial = "pink3";

                                        propColorCounter = 3;
                                        propCounter = 8;

                                        mortMenuStage3 = true;
                                    }
                                    else
                                    {
                                        mortGlow = true;
                                        propertySprites[$"pink3"].Tint = Color.White;
                                    }
                                }
                                else
                                {
                                    propertySprites[$"pink3"].Tint = Color.Gray;
                                    mortGlow = false;
                                    propertySprites[$"pink3"].stopBeingHighlighted = true;
                                    propertySprites[$"pink3"].stayHighlighted = false;
                                }


                                break;
                            case 3:

                                propertySprites[$"orange1"].Draw(batch);
                                propertySprites[$"orange2"].Draw(batch);
                                propertySprites[$"orange3"].Draw(batch);

                                temp = findProp("StJamesPlace");
                                temp2 = findProp("TennesseeAve");
                                temp3 = findProp("NewYorkAve");

                                if (temp != null && !temp.isMortgaged)
                                {
                                    if (propertySprites[$"orange1"].IsClicked && temp != null)
                                    {
                                        selectedPropToMortgage = findProp("StJamesPlace");
                                        theTrial = "orange1";

                                        propColorCounter = 1;
                                        propCounter = 9;

                                        mortMenuStage3 = true;
                                    }
                                    else
                                    {
                                        mortGlow = true;
                                        propertySprites[$"orange1"].Tint = Color.White;
                                    }
                                }
                                else
                                {
                                    propertySprites[$"orange1"].Tint = Color.Gray;
                                    mortGlow = false;
                                    propertySprites[$"orange1"].stopBeingHighlighted = true;
                                    propertySprites[$"orange1"].stayHighlighted = false;
                                }

                                if (temp2 != null && !temp2.isMortgaged)
                                {
                                    if (propertySprites[$"orange2"].IsClicked && temp2 != null)
                                    {
                                        selectedPropToMortgage = findProp("TennesseeAve");
                                        theTrial = "orange2";

                                        propColorCounter = 2;
                                        propCounter = 10;

                                        mortMenuStage3 = true;
                                    }
                                    else
                                    {
                                        mortGlow = true;
                                        propertySprites[$"orange2"].Tint = Color.White;
                                    }
                                }
                                else
                                {
                                    propertySprites[$"orange2"].Tint = Color.Gray;
                                    mortGlow = false;
                                    propertySprites[$"orange2"].stopBeingHighlighted = true;
                                    propertySprites[$"orange2"].stayHighlighted = false;
                                }

                                if (temp3 != null && !temp3.isMortgaged)
                                {
                                    if (propertySprites[$"orange3"].IsClicked && temp3 != null)
                                    {
                                        selectedPropToMortgage = findProp("NewYorkAve");
                                        theTrial = "orange3";

                                        propColorCounter = 3;
                                        propCounter = 11;

                                        mortMenuStage3 = true;
                                    }
                                    else
                                    {
                                        mortGlow = true;
                                        propertySprites[$"orange3"].Tint = Color.White;
                                    }
                                }
                                else
                                {
                                    propertySprites[$"orange3"].Tint = Color.Gray;
                                    mortGlow = false;
                                    propertySprites[$"orange3"].stopBeingHighlighted = true;
                                    propertySprites[$"orange3"].stayHighlighted = false;
                                }

                                break;
                            case 4:
                                propertySprites[$"red1"].Draw(batch);
                                propertySprites[$"red2"].Draw(batch);
                                propertySprites[$"red3"].Draw(batch);

                                temp = findProp("KentuckyAve");
                                temp2 = findProp("IndianaAve");
                                temp3 = findProp("IllinoisAve");

                                if (temp != null && !temp.isMortgaged)
                                {
                                    if (propertySprites[$"red1"].IsClicked && temp != null)
                                    {
                                        selectedPropToMortgage = findProp("KentuckyAve");
                                        theTrial = "red1";

                                        propColorCounter = 1;
                                        propCounter = 12;

                                        mortMenuStage3 = true;
                                    }
                                    else
                                    {
                                        mortGlow = true;
                                        propertySprites[$"red1"].Tint = Color.White;
                                    }
                                }
                                else
                                {
                                    propertySprites[$"red1"].Tint = Color.Gray;
                                    mortGlow = false;
                                    propertySprites[$"red1"].stopBeingHighlighted = true;
                                    propertySprites[$"red1"].stayHighlighted = false;
                                }

                                if (temp2 != null && !temp2.isMortgaged)
                                {
                                    if (propertySprites[$"red2"].IsClicked && temp2 != null)
                                    {
                                        selectedPropToMortgage = findProp("IndianaAve");
                                        theTrial = "red2";

                                        propColorCounter = 2;
                                        propCounter = 13;

                                        mortMenuStage3 = true;
                                    }
                                    else
                                    {
                                        mortGlow = true;
                                        propertySprites[$"red2"].Tint = Color.White;
                                    }
                                }
                                else
                                {
                                    propertySprites[$"red2"].Tint = Color.Gray;
                                    mortGlow = false;
                                    propertySprites[$"red2"].stopBeingHighlighted = true;
                                    propertySprites[$"red2"].stayHighlighted = false;
                                }

                                if (temp3 != null && !temp3.isMortgaged)
                                {
                                    if (propertySprites[$"red3"].IsClicked && temp3 != null)
                                    {
                                        selectedPropToMortgage = findProp("IllinoisAve");
                                        theTrial = "red3";

                                        propColorCounter = 3;
                                        propCounter = 14;

                                        mortMenuStage3 = true;
                                    }
                                    else
                                    {
                                        mortGlow = true;
                                        propertySprites[$"red3"].Tint = Color.White;
                                    }
                                }
                                else
                                {
                                    propertySprites[$"red3"].Tint = Color.Gray;
                                    mortGlow = false;
                                    propertySprites[$"red3"].stopBeingHighlighted = true;
                                    propertySprites[$"red3"].stayHighlighted = false;
                                }

                                break;
                            case 5:
                                propertySprites[$"yellow1"].Draw(batch);
                                propertySprites[$"yellow2"].Draw(batch);
                                propertySprites[$"yellow3"].Draw(batch);

                                temp = findProp("AtlanticAve");
                                temp2 = findProp("VentnorAve");
                                temp3 = findProp("MarvinGardens");

                                if (temp != null && !temp.isMortgaged)
                                {
                                    if (propertySprites[$"yellow1"].IsClicked && temp != null)
                                    {
                                        selectedPropToMortgage = findProp("AtlanticAve");
                                        theTrial = "yellow1";

                                        propColorCounter = 1;
                                        propCounter = 15;

                                        mortMenuStage3 = true;
                                    }
                                    else
                                    {
                                        mortGlow = true;
                                        propertySprites[$"yellow1"].Tint = Color.White;
                                    }
                                }
                                else
                                {
                                    propertySprites[$"yellow1"].Tint = Color.Gray;
                                    mortGlow = false;
                                    propertySprites[$"yellow1"].stopBeingHighlighted = true;
                                    propertySprites[$"yellow1"].stayHighlighted = false;
                                }

                                if (temp2 != null && !temp2.isMortgaged)
                                {
                                    if (propertySprites[$"yellow2"].IsClicked && temp2 != null)
                                    {
                                        selectedPropToMortgage = findProp("VentnorAve");
                                        theTrial = "yellow2";

                                        propColorCounter = 2;
                                        propCounter = 16;

                                        mortMenuStage3 = true;
                                    }
                                    else
                                    {
                                        mortGlow = true;
                                        propertySprites[$"yellow2"].Tint = Color.White;
                                    }
                                }
                                else
                                {
                                    propertySprites[$"yellow2"].Tint = Color.Gray;
                                    mortGlow = false;
                                    propertySprites[$"yellow2"].stopBeingHighlighted = true;
                                    propertySprites[$"yellow2"].stayHighlighted = false;
                                }

                                if (temp3 != null && !temp3.isMortgaged)
                                {
                                    if (propertySprites[$"yellow3"].IsClicked && temp3 != null)
                                    {
                                        selectedPropToMortgage = findProp("MarvinGardens");
                                        theTrial = "yellow3";

                                        propColorCounter = 3;
                                        propCounter = 17;

                                        mortMenuStage3 = true;
                                    }
                                    else
                                    {
                                        mortGlow = true;
                                        propertySprites[$"yellow3"].Tint = Color.White;
                                    }
                                }
                                else
                                {
                                    propertySprites[$"yellow3"].Tint = Color.Gray;
                                    mortGlow = false;
                                    propertySprites[$"yellow3"].stopBeingHighlighted = true;
                                    propertySprites[$"yellow3"].stayHighlighted = false;
                                }

                                break;
                            case 6:
                                propertySprites[$"green1"].Draw(batch);
                                propertySprites[$"green2"].Draw(batch);
                                propertySprites[$"green3"].Draw(batch);

                                temp = findProp("PacificAve");
                                temp2 = findProp("NoCarolinaAve");
                                temp3 = findProp("PennsylvaniaAve");

                                if (temp != null && !temp.isMortgaged)
                                {
                                    if (propertySprites[$"green1"].IsClicked && temp != null)
                                    {
                                        selectedPropToMortgage = findProp("PacificAve");
                                        theTrial = "green1";

                                        propColorCounter = 1;
                                        propCounter = 18;

                                        mortMenuStage3 = true;
                                    }
                                    else
                                    {
                                        mortGlow = true;
                                        propertySprites[$"green1"].Tint = Color.White;
                                    }
                                }
                                else
                                {
                                    propertySprites[$"green1"].Tint = Color.Gray;
                                    mortGlow = false;
                                    propertySprites[$"green1"].stopBeingHighlighted = true;
                                    propertySprites[$"green1"].stayHighlighted = false;
                                }

                                if (temp2 != null && !temp2.isMortgaged)
                                {
                                    if (propertySprites[$"green2"].IsClicked && temp2 != null)
                                    {
                                        selectedPropToMortgage = findProp("NoCarolinaAve");
                                        theTrial = "green2";

                                        propColorCounter = 2;
                                        propCounter = 19;

                                        mortMenuStage3 = true;
                                    }
                                    else
                                    {
                                        mortGlow = true;
                                        propertySprites[$"green2"].Tint = Color.White;
                                    }
                                }
                                else
                                {
                                    propertySprites[$"green2"].Tint = Color.Gray;
                                    mortGlow = false;
                                    propertySprites[$"green2"].stopBeingHighlighted = true;
                                    propertySprites[$"green2"].stayHighlighted = false;
                                }

                                if (temp3 != null && !temp3.isMortgaged)
                                {
                                    if (propertySprites[$"green3"].IsClicked && temp3 != null)
                                    {
                                        selectedPropToMortgage = findProp("PennsylvaniaAve");
                                        theTrial = "green3";

                                        propColorCounter = 3;
                                        propCounter = 20;

                                        mortMenuStage3 = true;
                                    }
                                    else
                                    {
                                        mortGlow = true;
                                        propertySprites[$"green3"].Tint = Color.White;
                                    }
                                }
                                else
                                {
                                    propertySprites[$"green3"].Tint = Color.Gray;
                                    mortGlow = false;
                                    propertySprites[$"green3"].stopBeingHighlighted = true;
                                    propertySprites[$"green3"].stayHighlighted = false;
                                }

                                break;
                            case 7:
                                propertySprites[$"blue1"].Draw(batch);
                                propertySprites[$"blue2"].Draw(batch);

                                temp = findProp("ParkPlace");
                                temp2 = findProp("Boardwalk");

                                if (temp != null && !temp.isMortgaged)
                                {
                                    if (propertySprites[$"blue1"].IsClicked && temp != null)
                                    {
                                        selectedPropToMortgage = findProp("ParkPlace");
                                        theTrial = "blue1";

                                        propColorCounter = 1;
                                        propCounter = 21;

                                        mortMenuStage3 = true;
                                    }
                                    else
                                    {
                                        mortGlow = true;
                                        propertySprites[$"blue1"].Tint = Color.White;
                                    }
                                }
                                else
                                {
                                    propertySprites[$"blue1"].Tint = Color.Gray;
                                    mortGlow = false;
                                    propertySprites[$"blue1"].stopBeingHighlighted = true;
                                    propertySprites[$"blue1"].stayHighlighted = false;
                                }

                                if (temp2 != null && !temp2.isMortgaged)
                                {
                                    if (propertySprites[$"blue2"].IsClicked && temp2 != null)
                                    {
                                        selectedPropToMortgage = findProp("Boardwalk");
                                        theTrial = "blue2";

                                        propColorCounter = 2;
                                        propCounter = 22;

                                        mortMenuStage3 = true;
                                    }
                                    else
                                    {
                                        mortGlow = true;
                                        propertySprites[$"blue2"].Tint = Color.White;
                                    }
                                }
                                else
                                {
                                    propertySprites[$"blue2"].Tint = Color.Gray;
                                    mortGlow = false;
                                    propertySprites[$"blue2"].stopBeingHighlighted = true;
                                    propertySprites[$"blue2"].stayHighlighted = false;
                                }
                                break;
                        }
                    }
                }

                else if (mortMenuStage3)
                {
                    if (bricksInTheWall == 0)
                    {
                        theWall = propertySprites[theTrial].Copy();
                        theWall.stopBeingHighlighted = false;
                        bricksInTheWall++;
                    }
                    theWall.Position = new Vector2(810, 300);
                    theWall.Draw(batch);

                    mortHotelIcon.Draw(batch);
                    mortHouseIcon.Draw(batch);
                    if ((mortHouseIcon.IsClicked && hasHouse(selectedPropToMortgage)) && isReadyToSellHouse(selectedPropToMortgage))
                    {
                        mortHouseIcon.stayHighlighted = true;
                        mortHouseIcon.stopBeingHighlighted = false;

                        mortHotelIcon.stayHighlighted = false;
                        mortHotelIcon.stopBeingHighlighted = true;

                        theWall.stayHighlighted = false;
                        theWall.stopBeingHighlighted = true;

                        selectedMortgageOption = 1;
                    }
                    else if (!isReadyToSellHouse(selectedPropToMortgage))
                    {
                        mortHouseIcon.Tint = Color.Gray;
                    }
                    if (mortHotelIcon.IsClicked && hasHotel(selectedPropToMortgage))
                    {
                        mortHotelIcon.stayHighlighted = true;
                        mortHotelIcon.stopBeingHighlighted = false;

                        mortHouseIcon.stayHighlighted = false;
                        mortHouseIcon.stopBeingHighlighted = true;

                        theWall.stayHighlighted = false;
                        theWall.stopBeingHighlighted = true;

                        selectedMortgageOption = 2;
                    }
                    if (theWall.IsClicked)
                    {
                        theWall.stayHighlighted = true;
                        theWall.stopBeingHighlighted = false;

                        mortHouseIcon.stayHighlighted = false;
                        mortHouseIcon.stopBeingHighlighted = true;

                        mortHotelIcon.stayHighlighted = false;
                        mortHotelIcon.stopBeingHighlighted = true;

                        selectedMortgageOption = 3;
                    }

                    mortgageYesButton.Draw(batch);
                }
                #endregion

                #region Not-Normal-Un-Mortage Menu
                if (unmortMenuStage1)
                {
                    for (int i = 0; i < MortProps.Count; i++)
                    {
                        if (CurrentPlayer.oneOfOneColorMort(i))
                        {
                            MortProps[i].Tint = Color.White;
                        }
                        else
                        {
                            MortProps[i].Tint = Color.Gray;
                        }

                        MortProps[i].Draw(batch);
                    }
                }

                else if (unmortMenuStage2)
                {
                    if (!tearDownTheWall)
                    {
                        switch (mortColorSelected)
                        {
                            case 0:
                                propertySprites[$"purple1"].Draw(batch);
                                propertySprites[$"purple2"].Draw(batch);

                                temp = findProp("MediterraneanAve");
                                temp2 = findProp("BalticAve");

                                if (temp != null && temp.isMortgaged)
                                {
                                    if (propertySprites[$"purple1"].IsClicked)
                                    {
                                        selectedPropToMortgage = findProp("MediterraneanAve");
                                        selectedPropToMortgage.Tint = Color.White;
                                        theTrial = "purple1";

                                        propColorCounter = 1;
                                        propCounter = 1;

                                        selectedPropToMortgage.isMortgaged = false;
                                        CurrentPlayer.Money -= (int)((selectedPropToMortgage.Cost / 2) * .2f);
                                    }
                                    else
                                    {
                                        mortGlow = true;
                                        propertySprites[$"purple1"].Tint = Color.White;
                                    }
                                }
                                else
                                {
                                    propertySprites[$"purple1"].Tint = Color.Gray;
                                    mortGlow = false;
                                    propertySprites[$"purple1"].stopBeingHighlighted = true;
                                    propertySprites[$"purple1"].stayHighlighted = false;
                                }

                                if (temp2 != null && temp2.isMortgaged)
                                {
                                    if (propertySprites[$"purple2"].IsClicked)
                                    {
                                        selectedPropToMortgage = findProp("BalticAve");
                                        theTrial = "purple2";

                                        propColorCounter = 2;
                                        propCounter = 2;

                                        unmortMenuStage3 = true;
                                    }
                                    else
                                    {
                                        mortGlow = true;
                                        propertySprites[$"purple2"].Tint = Color.White;
                                    }
                                }
                                else
                                {
                                    propertySprites[$"purple2"].Tint = Color.Gray;
                                    mortGlow = false;
                                    propertySprites[$"purple2"].stopBeingHighlighted = true;
                                    propertySprites[$"purple2"].stayHighlighted = false;
                                }

                                break;
                            case 1:
                                propertySprites[$"lightBlue1"].Draw(batch);
                                propertySprites[$"lightBlue2"].Draw(batch);
                                propertySprites[$"lightBlue3"].Draw(batch);

                                temp = findProp("OrientalAve");
                                temp2 = findProp("VermontAve");
                                temp3 = findProp("ConnecticutAve");
                                if (temp != null && temp.isMortgaged)
                                {
                                    if (propertySprites[$"lightBlue1"].IsClicked && temp != null)
                                    {
                                        selectedPropToMortgage = findProp("OrientalAve");
                                        theTrial = "lightBlue1";

                                        propCounter = 3;

                                        unmortMenuStage3 = true;
                                    }
                                    else
                                    {
                                        mortGlow = true;
                                        propertySprites[$"lightBlue1"].Tint = Color.White;
                                    }
                                }
                                else
                                {
                                    propertySprites[$"lightBlue1"].Tint = Color.Gray;
                                    mortGlow = false;
                                    propertySprites[$"lightBlue1"].stopBeingHighlighted = true;
                                    propertySprites[$"lightBlue1"].stayHighlighted = false;
                                }

                                if (temp2 != null && temp2.isMortgaged)
                                {
                                    if (propertySprites[$"lightBlue2"].IsClicked && temp2 != null)
                                    {
                                        selectedPropToMortgage = findProp("VermontAve");
                                        theTrial = "lightBlue2";

                                        propColorCounter = 2;
                                        propCounter = 4;

                                        unmortMenuStage3 = true;
                                    }
                                    else
                                    {
                                        mortGlow = true;
                                        propertySprites[$"lightBlue2"].Tint = Color.White;
                                    }
                                }
                                else
                                {
                                    propertySprites[$"lightBlue2"].Tint = Color.Gray;
                                    mortGlow = false;
                                    propertySprites[$"lightBlue2"].stopBeingHighlighted = true;
                                    propertySprites[$"lightBlue2"].stayHighlighted = false;
                                }

                                if (temp3 != null && temp3.isMortgaged)
                                {
                                    if (propertySprites[$"lightBlue3"].IsClicked && temp3 != null)
                                    {
                                        selectedPropToMortgage = findProp("ConnecticutAve");
                                        theTrial = "lightBlue3";

                                        propColorCounter = 3;
                                        propCounter = 5;

                                        unmortMenuStage3 = true;
                                    }
                                    else
                                    {
                                        mortGlow = true;
                                        propertySprites[$"lightBlue3"].Tint = Color.White;
                                    }
                                }
                                else
                                {
                                    propertySprites[$"lightBlue3"].Tint = Color.Gray;
                                    mortGlow = false;
                                    propertySprites[$"lightBlue3"].stopBeingHighlighted = true;
                                    propertySprites[$"lightBlue3"].stayHighlighted = false;
                                }

                                break;
                            case 2:

                                propertySprites[$"pink1"].Draw(batch);
                                propertySprites[$"pink2"].Draw(batch);
                                propertySprites[$"pink3"].Draw(batch);

                                temp = findProp("StCharlesPlace");
                                temp2 = findProp("StatesAve");
                                temp3 = findProp("VirginiaAve");

                                if (temp != null && temp.isMortgaged)
                                {
                                    if (propertySprites[$"pink1"].IsClicked && temp != null)
                                    {
                                        selectedPropToMortgage = findProp("StCharlesPlace");
                                        theTrial = "pink1";

                                        propColorCounter = 1;
                                        propCounter = 6;

                                        unmortMenuStage3 = true;
                                    }
                                    else
                                    {
                                        mortGlow = true;
                                        propertySprites[$"pink1"].Tint = Color.White;
                                    }
                                }
                                else
                                {
                                    propertySprites[$"pink1"].Tint = Color.Gray;
                                    mortGlow = false;
                                    propertySprites[$"pink1"].stopBeingHighlighted = true;
                                    propertySprites[$"pink1"].stayHighlighted = false;
                                }

                                if (temp2 != null && temp2.isMortgaged)
                                {
                                    if (propertySprites[$"pink2"].IsClicked && temp2 != null)
                                    {
                                        selectedPropToMortgage = findProp("StatesAve");
                                        theTrial = "pink2";

                                        propColorCounter = 2;
                                        propCounter = 7;

                                        unmortMenuStage3 = true;
                                    }
                                    else
                                    {
                                        mortGlow = true;
                                        propertySprites[$"pink2"].Tint = Color.White;
                                    }
                                }
                                else
                                {
                                    propertySprites[$"pink2"].Tint = Color.Gray;
                                    mortGlow = false;
                                    propertySprites[$"pink2"].stopBeingHighlighted = true;
                                    propertySprites[$"pink2"].stayHighlighted = false;
                                }

                                if (temp3 != null && temp3.isMortgaged)
                                {
                                    if (propertySprites[$"pink3"].IsClicked && temp3 != null)
                                    {
                                        selectedPropToMortgage = findProp("VirginiaAve");
                                        theTrial = "pink3";

                                        propColorCounter = 3;
                                        propCounter = 8;

                                        unmortMenuStage3 = true;
                                    }
                                    else
                                    {
                                        mortGlow = true;
                                        propertySprites[$"pink3"].Tint = Color.White;
                                    }
                                }
                                else
                                {
                                    propertySprites[$"pink3"].Tint = Color.Gray;
                                    mortGlow = false;
                                    propertySprites[$"pink3"].stopBeingHighlighted = true;
                                    propertySprites[$"pink3"].stayHighlighted = false;
                                }


                                break;
                            case 3:

                                propertySprites[$"orange1"].Draw(batch);
                                propertySprites[$"orange2"].Draw(batch);
                                propertySprites[$"orange3"].Draw(batch);

                                temp = findProp("StJamesPlace");
                                temp2 = findProp("TennesseeAve");
                                temp3 = findProp("NewYorkAve");

                                if (temp != null && temp.isMortgaged)
                                {
                                    if (propertySprites[$"orange1"].IsClicked && temp != null)
                                    {
                                        selectedPropToMortgage = findProp("StJamesPlace");
                                        theTrial = "orange1";

                                        propColorCounter = 1;
                                        propCounter = 9;

                                        unmortMenuStage3 = true;
                                    }
                                    else
                                    {
                                        mortGlow = true;
                                        propertySprites[$"orange1"].Tint = Color.White;
                                    }
                                }
                                else
                                {
                                    propertySprites[$"orange1"].Tint = Color.Gray;
                                    mortGlow = false;
                                    propertySprites[$"orange1"].stopBeingHighlighted = true;
                                    propertySprites[$"orange1"].stayHighlighted = false;
                                }

                                if (temp2 != null && temp2.isMortgaged)
                                {
                                    if (propertySprites[$"orange2"].IsClicked && temp2 != null)
                                    {
                                        selectedPropToMortgage = findProp("TennesseeAve");
                                        theTrial = "orange2";

                                        propColorCounter = 2;
                                        propCounter = 10;

                                        unmortMenuStage3 = true;
                                    }
                                    else
                                    {
                                        mortGlow = true;
                                        propertySprites[$"orange2"].Tint = Color.White;
                                    }
                                }
                                else
                                {
                                    propertySprites[$"orange2"].Tint = Color.Gray;
                                    mortGlow = false;
                                    propertySprites[$"orange2"].stopBeingHighlighted = true;
                                    propertySprites[$"orange2"].stayHighlighted = false;
                                }

                                if (temp3 != null && temp3.isMortgaged)
                                {
                                    if (propertySprites[$"orange3"].IsClicked && temp3 != null)
                                    {
                                        selectedPropToMortgage = findProp("NewYorkAve");
                                        theTrial = "orange3";

                                        propColorCounter = 3;
                                        propCounter = 11;

                                        unmortMenuStage3 = true;
                                    }
                                    else
                                    {
                                        mortGlow = true;
                                        propertySprites[$"orange3"].Tint = Color.White;
                                    }
                                }
                                else
                                {
                                    propertySprites[$"orange3"].Tint = Color.Gray;
                                    mortGlow = false;
                                    propertySprites[$"orange3"].stopBeingHighlighted = true;
                                    propertySprites[$"orange3"].stayHighlighted = false;
                                }

                                break;
                            case 4:
                                propertySprites[$"red1"].Draw(batch);
                                propertySprites[$"red2"].Draw(batch);
                                propertySprites[$"red3"].Draw(batch);

                                temp = findProp("KentuckyAve");
                                temp2 = findProp("IndianaAve");
                                temp3 = findProp("IllinoisAve");

                                if (temp != null && temp.isMortgaged)
                                {
                                    if (propertySprites[$"red1"].IsClicked && temp != null)
                                    {
                                        selectedPropToMortgage = findProp("KentuckyAve");
                                        theTrial = "red1";

                                        propColorCounter = 1;
                                        propCounter = 12;

                                        unmortMenuStage3 = true;
                                    }
                                    else
                                    {
                                        mortGlow = true;
                                        propertySprites[$"red1"].Tint = Color.White;
                                    }
                                }
                                else
                                {
                                    propertySprites[$"red1"].Tint = Color.Gray;
                                    mortGlow = false;
                                    propertySprites[$"red1"].stopBeingHighlighted = true;
                                    propertySprites[$"red1"].stayHighlighted = false;
                                }

                                if (temp2 != null && temp2.isMortgaged)
                                {
                                    if (propertySprites[$"red2"].IsClicked && temp2 != null)
                                    {
                                        selectedPropToMortgage = findProp("IndianaAve");
                                        theTrial = "red2";

                                        propColorCounter = 2;
                                        propCounter = 13;

                                        unmortMenuStage3 = true;
                                    }
                                    else
                                    {
                                        mortGlow = true;
                                        propertySprites[$"red2"].Tint = Color.White;
                                    }
                                }
                                else
                                {
                                    propertySprites[$"red2"].Tint = Color.Gray;
                                    mortGlow = false;
                                    propertySprites[$"red2"].stopBeingHighlighted = true;
                                    propertySprites[$"red2"].stayHighlighted = false;
                                }

                                if (temp3 != null && temp3.isMortgaged)
                                {
                                    if (propertySprites[$"red3"].IsClicked && temp3 != null)
                                    {
                                        selectedPropToMortgage = findProp("IllinoisAve");
                                        theTrial = "red3";

                                        propColorCounter = 3;
                                        propCounter = 14;

                                        unmortMenuStage3 = true;
                                    }
                                    else
                                    {
                                        mortGlow = true;
                                        propertySprites[$"red3"].Tint = Color.White;
                                    }
                                }
                                else
                                {
                                    propertySprites[$"red3"].Tint = Color.Gray;
                                    mortGlow = false;
                                    propertySprites[$"red3"].stopBeingHighlighted = true;
                                    propertySprites[$"red3"].stayHighlighted = false;
                                }

                                break;
                            case 5:
                                propertySprites[$"yellow1"].Draw(batch);
                                propertySprites[$"yellow2"].Draw(batch);
                                propertySprites[$"yellow3"].Draw(batch);

                                temp = findProp("AtlanticAve");
                                temp2 = findProp("VentnorAve");
                                temp3 = findProp("MarvinGardens");

                                if (temp != null && temp.isMortgaged)
                                {
                                    if (propertySprites[$"yellow1"].IsClicked && temp != null)
                                    {
                                        selectedPropToMortgage = findProp("AtlanticAve");
                                        theTrial = "yellow1";

                                        propColorCounter = 1;
                                        propCounter = 15;

                                        unmortMenuStage3 = true;
                                    }
                                    else
                                    {
                                        mortGlow = true;
                                        propertySprites[$"yellow1"].Tint = Color.White;
                                    }
                                }
                                else
                                {
                                    propertySprites[$"yellow1"].Tint = Color.Gray;
                                    mortGlow = false;
                                    propertySprites[$"yellow1"].stopBeingHighlighted = true;
                                    propertySprites[$"yellow1"].stayHighlighted = false;
                                }

                                if (temp2 != null && temp2.isMortgaged)
                                {
                                    if (propertySprites[$"yellow2"].IsClicked && temp2 != null)
                                    {
                                        selectedPropToMortgage = findProp("VentnorAve");
                                        theTrial = "yellow2";

                                        propColorCounter = 2;
                                        propCounter = 16;

                                        unmortMenuStage3 = true;
                                    }
                                    else
                                    {
                                        mortGlow = true;
                                        propertySprites[$"yellow2"].Tint = Color.White;
                                    }
                                }
                                else
                                {
                                    propertySprites[$"yellow2"].Tint = Color.Gray;
                                    mortGlow = false;
                                    propertySprites[$"yellow2"].stopBeingHighlighted = true;
                                    propertySprites[$"yellow2"].stayHighlighted = false;
                                }

                                if (temp3 != null && temp3.isMortgaged)
                                {
                                    if (propertySprites[$"yellow3"].IsClicked && temp3 != null)
                                    {
                                        selectedPropToMortgage = findProp("MarvinGardens");
                                        theTrial = "yellow3";

                                        propColorCounter = 3;
                                        propCounter = 17;

                                        unmortMenuStage3 = true;
                                    }
                                    else
                                    {
                                        mortGlow = true;
                                        propertySprites[$"yellow3"].Tint = Color.White;
                                    }
                                }
                                else
                                {
                                    propertySprites[$"yellow3"].Tint = Color.Gray;
                                    mortGlow = false;
                                    propertySprites[$"yellow3"].stopBeingHighlighted = true;
                                    propertySprites[$"yellow3"].stayHighlighted = false;
                                }

                                break;
                            case 6:
                                propertySprites[$"green1"].Draw(batch);
                                propertySprites[$"green2"].Draw(batch);
                                propertySprites[$"green3"].Draw(batch);

                                temp = findProp("PacificAve");
                                temp2 = findProp("NoCarolinaAve");
                                temp3 = findProp("PennsylvaniaAve");

                                if (temp != null && temp.isMortgaged)
                                {
                                    if (propertySprites[$"green1"].IsClicked && temp != null)
                                    {
                                        selectedPropToMortgage = findProp("PacificAve");
                                        theTrial = "green1";

                                        propColorCounter = 1;
                                        propCounter = 18;

                                        unmortMenuStage3 = true;
                                    }
                                    else
                                    {
                                        mortGlow = true;
                                        propertySprites[$"green1"].Tint = Color.White;
                                    }
                                }
                                else
                                {
                                    propertySprites[$"green1"].Tint = Color.Gray;
                                    mortGlow = false;
                                    propertySprites[$"green1"].stopBeingHighlighted = true;
                                    propertySprites[$"green1"].stayHighlighted = false;
                                }

                                if (temp2 != null && temp2.isMortgaged)
                                {
                                    if (propertySprites[$"green2"].IsClicked && temp2 != null)
                                    {
                                        selectedPropToMortgage = findProp("NoCarolinaAve");
                                        theTrial = "green2";

                                        propColorCounter = 2;
                                        propCounter = 19;

                                        unmortMenuStage3 = true;
                                    }
                                    else
                                    {
                                        mortGlow = true;
                                        propertySprites[$"green2"].Tint = Color.White;
                                    }
                                }
                                else
                                {
                                    propertySprites[$"green2"].Tint = Color.Gray;
                                    mortGlow = false;
                                    propertySprites[$"green2"].stopBeingHighlighted = true;
                                    propertySprites[$"green2"].stayHighlighted = false;
                                }

                                if (temp3 != null && temp3.isMortgaged)
                                {
                                    if (propertySprites[$"green3"].IsClicked && temp3 != null)
                                    {
                                        selectedPropToMortgage = findProp("PennsylvaniaAve");
                                        theTrial = "green3";

                                        propColorCounter = 3;
                                        propCounter = 20;

                                        unmortMenuStage3 = true;
                                    }
                                    else
                                    {
                                        mortGlow = true;
                                        propertySprites[$"green3"].Tint = Color.White;
                                    }
                                }
                                else
                                {
                                    propertySprites[$"green3"].Tint = Color.Gray;
                                    mortGlow = false;
                                    propertySprites[$"green3"].stopBeingHighlighted = true;
                                    propertySprites[$"green3"].stayHighlighted = false;
                                }

                                break;
                            case 7:
                                propertySprites[$"blue1"].Draw(batch);
                                propertySprites[$"blue2"].Draw(batch);

                                temp = findProp("ParkPlace");
                                temp2 = findProp("Boardwalk");

                                if (temp != null && temp.isMortgaged)
                                {
                                    if (propertySprites[$"blue1"].IsClicked && temp != null)
                                    {
                                        selectedPropToMortgage = findProp("ParkPlace");
                                        theTrial = "blue1";

                                        propColorCounter = 1;
                                        propCounter = 21;

                                        unmortMenuStage3 = true;
                                    }
                                    else
                                    {
                                        mortGlow = true;
                                        propertySprites[$"blue1"].Tint = Color.White;
                                    }
                                }
                                else
                                {
                                    propertySprites[$"blue1"].Tint = Color.Gray;
                                    mortGlow = false;
                                    propertySprites[$"blue1"].stopBeingHighlighted = true;
                                    propertySprites[$"blue1"].stayHighlighted = false;
                                }

                                if (temp2 != null && temp2.isMortgaged)
                                {
                                    if (propertySprites[$"blue2"].IsClicked && temp2 != null)
                                    {
                                        selectedPropToMortgage = findProp("Boardwalk");
                                        theTrial = "blue2";

                                        propColorCounter = 2;
                                        propCounter = 22;

                                        unmortMenuStage3 = true;
                                    }
                                    else
                                    {
                                        mortGlow = true;
                                        propertySprites[$"blue2"].Tint = Color.White;
                                    }
                                }
                                else
                                {
                                    propertySprites[$"blue2"].Tint = Color.Gray;
                                    mortGlow = false;
                                    propertySprites[$"blue2"].stopBeingHighlighted = true;
                                    propertySprites[$"blue2"].stayHighlighted = false;
                                }
                                break;
                        }
                    }
                }
                #endregion

                exitMortgageMenu.Draw(batch);
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
