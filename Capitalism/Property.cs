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

        public int Cost;
        public int Rent;
        public int With1House;
        public int With2House;
        public int With3House;
        public int With4House;
        public int WithHotel;

        public int HouseCost;
        public int HotelCost;

        //Add Mortgage


        public Property(Texture2D image, Rectangle hitbox, Color tint,int cost, int rent, int rentWHouse1, int rentWHouse2, int rentWHouse3, int rentWHouse4, int rentWHotel, int houseCost, int hotel)
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



        public void Draw(SpriteBatch batch)
        {
            batch.Draw(Image, Position, Tint);
        }

    }
}
