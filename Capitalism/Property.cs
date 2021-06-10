using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Capitalism
{
    public enum PropertyColor
    { 
        purple,
        lightblue,
        pink,
        orange,
        red,
        yellow,
        green,
        blue,
        nully,
    }


    public class Property
    {
        public Texture2D Image;
        public Vector2 Position;
        public Color Tint;
        public Rectangle Hitbox;

        public float Rotation = -0.5f;

        public int Cost;
        public int Rent;
        public int With1House;
        public int With2House;
        public int With3House;
        public int With4House;
        public int WithHotel;
        public int houseCounter = 0;
        public int hotelCounter = 0;

        public int HouseCost;
        public int HotelCost;

        int originalRent;

        public bool expanded = false;
        public bool isRailroad = false;
        public bool isUtility = false;
        public bool isMortgaged = false;

        public List<HighlightButton> houses = new List<HighlightButton>();

        public List<HighlightButton> hotels = new List<HighlightButton>();
        //Add Mortgage

        public  string Color(PropertyColor color)
        {
            string currString = "";

            switch (color)
            {
                case PropertyColor.purple:
                    currString = "purple";
                    break;
                case PropertyColor.lightblue:
                    currString = "lightblue";
                    break;
                case PropertyColor.pink:
                    currString = "pink";
                    break;
                case PropertyColor.orange:
                    currString = "orange";
                    break;
                case PropertyColor.red:
                    currString = "red";
                    break;
                case PropertyColor.yellow:
                    currString = "yellow";
                    break;
                case PropertyColor.green:
                    currString = "green";
                    break;
                case PropertyColor.blue:
                    currString = "blue";
                    break;

                default:
                    break;
            }
            return currString;
        }

        int originalWidth;
        int originalHeight;

        public PropertyColor PropColor;

        public Property(Texture2D image, Rectangle hitbox, Color tint,int cost, int rent, bool Railroad, bool Utillity, int rentWHouse1, int rentWHouse2, int rentWHouse3, int rentWHouse4, int rentWHotel, int houseCost, int hotel, PropertyColor propColor)
        {
            Image = image;
            Tint = tint;
            Hitbox = hitbox;
            Position = hitbox.Location.ToVector2();

            Cost = cost;
            Rent = rent;
            With1House = rentWHouse1;
            With2House = rentWHouse2;
            With3House = rentWHouse3;
            With4House = rentWHouse4;
            WithHotel = rentWHotel;
            HouseCost = houseCost;
            HotelCost = hotel;
            PropColor = propColor;
            isRailroad = Railroad;
            isUtility = Utillity;
            //fix utillities 
            originalRent = rent;
        }



        public void Expand()
        {
            originalWidth = Hitbox.Width;
            originalHeight = Hitbox.Height;
            Hitbox.Width = (int)((double)(Hitbox.Width) * 1.75);
            Hitbox.Height = (int)((double)(Hitbox.Height) * 1.75);

            expanded = true;
        }

        public void Shrink()
        {
            Hitbox.Width = originalWidth;
            Hitbox.Height = originalHeight;

            expanded = false;
   
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(Image, Hitbox, null, Tint, Rotation, new Vector2(Hitbox.Width/2, Hitbox.Height/2-Hitbox.Height/3-Hitbox.Height/4), SpriteEffects.None, 1);
            //batch.Draw(Image, Hitbox, Color.Red);
        }

    }
}
