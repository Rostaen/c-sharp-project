using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Descent_2e_Co_Op
{
    public class OverlayBG
    {
        #region Fields

        Rectangle drawRectangle = new Rectangle(0, 0, 100, 100);
        int drawX = (GameConstants.WINDOW_WIDTH / 100), drawY = GameConstants.WINDOW_HEIGHT / 100;
        Texture2D sprite;                

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        public OverlayBG(ContentManager content)
        {
            sprite = content.Load<Texture2D>("Misc/black overlay");
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The method to draw the background
        /// </summary>
        /// <param name="sprite">The spritebach that will draw the images onto the gaming field</param>
        public void Draw(SpriteBatch spriteBatch, int mAlphaValue)
        {
            //spriteBatch.Draw(sprite, drawRectangle, Color.White);
            for (int y = 0; y < drawY; y++)
            {
                for (int x = 0; x < drawX; x++)
                {
                    spriteBatch.Draw(sprite, drawRectangle, new Color(255, 255, 255, (byte)MathHelper.Clamp(mAlphaValue, 0, 255)));
                    drawRectangle.X = 100 * (x+1);
                }
                drawRectangle.X = 0;
                drawRectangle.Y = 100 * y;
            }
        }

        #endregion
    }
}
