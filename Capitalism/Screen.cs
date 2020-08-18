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
    public abstract class Screen
    {
        public string Name;
        public bool EndScreen;

        public Screen(string name)
        {
            Name = name;
        }

        

        public abstract void LoadContent(ContentManager Content);


        public abstract void Update(GameTime gameTime);


        public abstract void Draw(SpriteBatch spritebatch);

    }
}
