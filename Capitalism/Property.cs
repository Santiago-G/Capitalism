using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Capitalism
{
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

        public bool expanded = false;
        public bool isRailroad = false;
        public bool isUtility = false;
        //Add Mortgage

        int originalWidth;
        int originalHeight;

        public Property(Texture2D image, Rectangle hitbox, Color tint,int cost, int rent, bool Railroad, int rentWHouse1, int rentWHouse2, int rentWHouse3, int rentWHouse4, int rentWHotel, int houseCost, int hotel)
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

        }



        public void Expand()
        {
            originalWidth = Hitbox.Width;
            originalHeight = Hitbox.Height;
            Hitbox.Width = (int)((double)(Hitbox.Width) * 1.25);
            Hitbox.Height = (int)((double)(Hitbox.Width) * 1.25);

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
