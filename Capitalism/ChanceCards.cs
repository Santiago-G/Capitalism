using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capitalism
{

    public enum CardTypes
    {
        BankCollect,
        BankStealing,
        GainFromOthers,
        GiveToOthers,
        Advance,
        GoBack3,
        GetOutOfJail,
        GoInJail,
        HouseRepair
    }

    public class ChanceCards
    {
        public Texture2D Image;
        public Vector2 Position;
        public Color Tint;
        public Rectangle Hitbox;

        CardTypes cardTypes;

        public Property destination;
        public int money;

        public ChanceCards(Texture2D image, Rectangle hitbox, Color tint, CardTypes cardTypes, Property destination=null, int money=0)
        {
            Image = image;
            Hitbox = hitbox;
            Tint = tint;
            Position = hitbox.Location.ToVector2();

            
        }



        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, Hitbox, Tint);
        }

    }
}
