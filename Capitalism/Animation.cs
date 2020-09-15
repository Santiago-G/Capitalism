using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capitalism
{
    public class Animation
    {
        private Texture2D tex;
        private Vector2 pos;

        int numFrames;
        int width;
        int height;
        int currentFrame;

        Rectangle dest;

        TimeSpan previousTime;

        uint delayMs;
        uint delayMsRestart;

        List<Rectangle> frames;
        int numRolls = 15;
        int currentRoll = 0;
        Random gen;

        public bool stopped = true;
        bool hasGone = false;

        public int DiceRollValue => currentFrame + 1;
        public Animation(Texture2D tex, Vector2 position, uint delay, Random random)
        {
            this.gen = random;
            this.tex = tex;
            this.pos = position;

            numFrames = 6;

            width = tex.Width / numFrames;
            height = tex.Height;

            delayMs = delay;
            delayMsRestart = delay;

            dest = new Rectangle((int)position.X, (int)position.Y, width, height);

            frames = new List<Rectangle>();

            //gen = new Random();

            for (int i = 0; i < numFrames; i++)
            {
                frames.Add(new Rectangle(width * i, 0, width, height));
            }

            ;
        }

        public void Restart()
        {
            stopped = false;
            delayMs = delayMsRestart;
            currentRoll = 0;
            currentFrame = 0;
        }

        internal void Update(GameTime gameTime, bool SlowDown)
        {

            if (gameTime.TotalGameTime - previousTime > TimeSpan.FromMilliseconds(delayMs))
            {
                if (currentRoll < numRolls)
                {
                    stopped = false;
                    hasGone = true;
                    currentFrame = gen.Next(frames.Count);
                    currentRoll++;
                    delayMs = (uint)(delayMs * 1.1);
                    previousTime = gameTime.TotalGameTime;
                }
                else if (hasGone)
                {
                    stopped = true;
                }
            }


            Console.WriteLine(currentFrame);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(tex, dest, frames[currentFrame], Color.White);
        }
    }
}
