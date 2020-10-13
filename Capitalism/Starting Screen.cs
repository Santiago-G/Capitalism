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
        ChoosingCharacters characterChoosing = new ChoosingCharacters("Choosing Characters");

        public bool StartingScreenFinished = false;

        public static int PlayerCount => SelectingPlayers.playerCount;

        public Starting_Screen(ContentManager Content)
        {
            startingScreens.AddScreen(mainMenu);
            startingScreens.LoadContent(Content);
            startingScreens.AddScreen(selectingPlayers);
            startingScreens.LoadContent(Content);
            //startingScreens.AddScreen(rollingOfTheDice);
            //startingScreens.LoadContent(Content);
            startingScreens.AddScreen(characterChoosing);
            startingScreens.LoadContent(Content);
        }

        public void Update(MouseState ms, GameTime gameTime)
        {
            Game1.TitleBarString = "";

            if (startingScreens.currentScreen == characterChoosing && startingScreens.currentScreen.EndScreen == true)
            {
                StartingScreenFinished = true;
            }

            startingScreens.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Dictionary[CurrentScreen].Draw()

            startingScreens.Draw(spriteBatch);
        }
    }
}
