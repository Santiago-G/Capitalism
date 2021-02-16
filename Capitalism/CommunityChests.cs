using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capitalism
{
    public enum CommunityCardTypes
    {
        BankMoney,
        GetFromOthers,
        GetOutOfJail,
        GoInJail,
        HouseRepair,
        GoToGo,
        Invalid
    }

    public class CommunityChests
    {


        public Texture2D Image;
        public Vector2 Position;
        public Color Tint;
        public Rectangle Hitbox;

        public CommunityCardTypes cardTypes;

        public int money;
        public float rotation = 0f;

        public CommunityChests(Texture2D image, Rectangle hitbox, Color tint, CommunityCardTypes CardTypes, int money = 0)
        {
            Image = image;
            Hitbox = hitbox;
            Tint = tint;
            Position = hitbox.Location.ToVector2();
            cardTypes = CardTypes;
            this.money = money;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(Image, Hitbox, null, Tint, 0.0f, new Vector2(0,0) , null, 0);
            spriteBatch.Draw(Image, Hitbox, null, Tint, rotation, new Vector2((Image.Width / 2), (Image.Height / 2)), SpriteEffects.None, 1);
        }
    }
}
