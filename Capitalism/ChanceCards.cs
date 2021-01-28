﻿using Microsoft.Xna.Framework;
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
        BankMoney,
        GiveToOthers,
        Advance,
        GoBack3,
        GetOutOfJail,
        GoInJail,
        HouseRepair,
        GoToGo,
        Invalid
    }

    public class ChanceCards
    {
        public Texture2D Image;
        public Vector2 Position;
        public Color Tint;
        public Rectangle Hitbox;

        public CardTypes cardTypes;

        public Vector2 destination;
        public int money;
        public float rotation = 0f;

        public ChanceCards(Texture2D image, Rectangle hitbox, Color tint, CardTypes CardTypes, Vector2 destination, int money=0)
        {
            Image = image;
            Hitbox = hitbox;
            Tint = tint;
            Position = hitbox.Location.ToVector2();
            cardTypes = CardTypes;
            this.destination = destination;
            this.money = money;
        }



        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(Image, Hitbox, null, Tint, 0.0f, new Vector2(0,0) , null, 0);
            spriteBatch.Draw(Image, Hitbox, null, Tint, rotation, new Vector2(Hitbox.X + (Image.Width/2), Hitbox.Y + (Image.Height/2)), SpriteEffects.None, 1);
        }

    }
}
