﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capitalism
{
    public class Player
    {
        public Texture2D Image;
        public Vector2 Position;
        public float Size;
        public Color Tint;
        public Vector2[] PositionArea;
        public int currentTileIndex;
        public int Money;
        public int railroadCounter = 0;
        public int utillityCounter = 0;

        public int jailTimer = 0;
        public bool inJail = false;

        public bool GetOutOfJailFree = false;
        public bool GetOutOfJailFree2 = false;

        bool goMoney = true;
        bool gotMoneyOnce = false;

        public Rectangle Hitbox => new Rectangle((int)Position.X, (int)Position.Y, (int)(Image.Width * Size), (int)(Image.Height * Size));

        public string Token = "";

        public List<Property> properties = new List<Property>();

        public Player(Texture2D image, Vector2 position, Color tint, string token, int size, int cash)
        {
            Position = position;
            Image = image;
            Tint = tint;
            Token = token;
            Size = size;
            Money = cash;
        }

        public bool allOfOneColor(string colorName)
        {
            colorName = colorName.ToLower();
            int count = 0;

            for (int i = 0; i < properties.Count; i++)
            {
                if (Enum.GetName(typeof(PropertyColor), properties[i].PropColor) == colorName)
                {
                    count++;

                    if (((colorName == "purple" || colorName == "blue") && count == 2) || count == 3)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool oneOfOneColor(int numb)
        {
            string colorName = "";

            switch (numb)
            {
                case 0:
                    colorName = "purple";
                    break;
                case 1:
                    colorName = "lightblue";
                    break;
                case 2:
                    colorName = "pink";
                    break;
                case 3:
                    colorName = "orange";
                    break;
                case 4:
                    colorName = "red";
                    break;
                case 5:
                    colorName = "yellow";
                    break;
                case 6:
                    colorName = "green";
                    break;
                case 7:
                    colorName = "blue";
                    break;
            }

            for (int i = 0; i < properties.Count; i++)
            {
                if (Enum.GetName(typeof(PropertyColor), properties[i].PropColor) == colorName)
                {
                    return true;
                }
            }

            return false;
        }

        public bool oneOfOneColorMort(int numb)
        {
            string colorName = "";

            switch (numb)
            {
                case 0:
                    colorName = "purple";
                    break;
                case 1:
                    colorName = "lightblue";
                    break;
                case 2:
                    colorName = "pink";
                    break;
                case 3:
                    colorName = "orange";
                    break;
                case 4:
                    colorName = "red";
                    break;
                case 5:
                    colorName = "yellow";
                    break;
                case 6:
                    colorName = "green";
                    break;
                case 7:
                    colorName = "blue";
                    break;
            }

            for (int i = 0; i < properties.Count; i++)
            {
                if ((Enum.GetName(typeof(PropertyColor), properties[i].PropColor) == colorName) && properties[i].isMortgaged)
                {
                    return true;
                }
            }

            return false;
        }

        public void Update()
        {
            if (Money == 0)
            {
                //game over
            }
            //if GetTilePosition != Position 0 then reset goMoney to false
            Vector2 temp = GetTilePosition();

            if (temp != PositionArea[0])
            {
                goMoney = false;
            }

            if (currentTileIndex == 1 && goMoney == false)
            {
                Money += 200;
                goMoney = true;
                gotMoneyOnce = true;
            }

            if (inJail)
            {
                Position = new Vector2(523, 885);
                currentTileIndex = 11;
                if (jailTimer == 3)
                {
                    inJail = false;
                    jailTimer = 0;
                    Money -= 50;
                }
            }
        }


        public Property mostRecentPurchase()
        {
            return properties[properties.Count - 1];
        }

        public Vector2 GetTilePosition()
        {
            for (int i = 0; i < PositionArea.Length; i++)
            {
                if (Position == PositionArea[i])
                {
                    return PositionArea[i];
                }
            }



            if (Position == new Vector2(1284, 895) || Position == new Vector2(1284, 935) || Position == new Vector2(1320, 950) || Position == new Vector2(1320, 875) || Position == new Vector2(1320, 915) || Position == new Vector2(1360, 950) || Position == new Vector2(1360, 875) || Position == new Vector2(1360, 915))
            {
                return PositionArea[0];
            }
            return Vector2.Zero;
        }

        public int GetTileIndex()
        {
            for (int i = 0; i < PositionArea.Length; i++)
            {
                if (Position == PositionArea[i])
                {
                    return i;
                }
            }

            return 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture: Image, destinationRectangle: Hitbox, color: Tint, rotation: 0, origin: new Vector2(Hitbox.Width / 2, Hitbox.Height / 2), effects: default, layerDepth: default);
        }//(int)(Image.Width * Size)/ 2, (int)(Image.Height * Size)/ 2
    }
}
