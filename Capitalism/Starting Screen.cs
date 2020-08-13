using Capitalism.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capitalism
{
    public class Starting_Screen
    {
        ScreenManager startingScreens = new ScreenManager();

        MainMenu mainMenu = new MainMenu("Main Menu");
        SelectingPlayers selectingPlayers = new SelectingPlayers("Selecting Players");
        DiceRolling rollingOfTheDice = new DiceRolling("Dice Rolling");


        bool realStartingScreen = true;
        bool PlayerScreen = false;
        bool diceScreen = false;


        public Starting_Screen(ContentManager Content)
        {
            startingScreens.AddScreen(mainMenu);
            startingScreens.AddScreen(selectingPlayers);
            startingScreens.AddScreen(rollingOfTheDice);


            startingScreens.LoadContent(Content);

        }

        public void Update(MouseState ms, GameTime gameTime)
        {

            Game1.TitleBarString = "";

            mainMenu.Update(gameTime);
            selectingPlayers.Update(gameTime);
            rollingOfTheDice.Update(gameTime);

            if (mainMenu.ScreenEnded)
            {
                realStartingScreen = false;
                PlayerScreen = true;
            }
            if (selectingPlayers.ScreenEnded)
            {
                PlayerScreen = false;
                diceScreen = true;
            }
            if (rollingOfTheDice.ScreenEnded)
            {
                diceScreen = false;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Dictionary[CurrentScreen].Draw()
            if (realStartingScreen)
            {
                mainMenu.Draw(spriteBatch);
            }
            else if (PlayerScreen)
            {
                selectingPlayers.Draw(spriteBatch);
            }

            else if (diceScreen)
            {
                rollingOfTheDice.Draw(spriteBatch);
            }
        }
    }
}
