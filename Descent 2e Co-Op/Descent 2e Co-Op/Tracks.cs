using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Descent_2e_Co_Op
{
    public class Tracks
    {
        #region Fields
        Texture2D sprite;
        Rectangle drawRectangle;
        bool lootTrackOpen = false, targetReached = true, negX = false, active = false;
        Vector2 direction = Vector2.Zero, location, target = Vector2.Zero;
        #endregion

        #region Constructor

        public Tracks(ContentManager content, Texture2D sprite, int xLoc, int yLoc)
        {
            this.sprite = sprite;
            drawRectangle = new Rectangle(xLoc, yLoc, this.sprite.Width, this.sprite.Height);
            location.X = drawRectangle.X; location.Y = drawRectangle.Y;
        }

        #endregion

        #region Properties

        public Rectangle DrawRectangle { get { return drawRectangle; } set { drawRectangle = value; } }
        public bool LootTrackOpen { get { return lootTrackOpen; } set { lootTrackOpen = value; } }
        public bool Active { get { return active; } set { active = value; } }

        #endregion

        #region Public Methods

        public void Update(GameTime gameTime)
        {
            if (!targetReached)
            {
                location += direction * GameConstants.LOOT_MOVE_SPEED * gameTime.ElapsedGameTime.Milliseconds;
                drawRectangle.X = (int)location.X;
                //if (direction.X - location.X <= 0) targetReached = true;
                if (negX) { if (location.X - target.X <= 0) { targetReached = true; negX = false; } }
                else { if (target.X - location.X <= 0) targetReached = true; }
            }
        }

        public void SetTarget(float xDir)
        {
            targetReached = false;
            target.X = xDir;
            if (xDir < 0) negX = true;
            direction.X = xDir;
            direction.Normalize();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, drawRectangle, Color.White);
        }

        #endregion
    }
}
