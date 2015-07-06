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
    public class FillBG
    {
        #region Fields

        private Texture2D backgaroundImage;
        private int screenHeight, screenWidth;
        private Vector2 origin, nextSpot;

        #endregion

        #region Public Methods

        /// <summary>
        /// This method loads the selected background image to the proper areas and begins loading the screen
        /// </summary>
        /// <param name="device">The graphics device</param>
        /// <param name="incTexture">The loaded image to be used as the background</param>
        public void Load(GraphicsDevice device, Texture2D incTexture)
        {
            screenHeight = device.Viewport.Height;
            screenWidth = device.Viewport.Width;
            backgaroundImage = incTexture;
            origin = new Vector2(0f, 0f);
            if (backgaroundImage.Width < screenWidth) nextSpot = new Vector2(backgaroundImage.Width, 0);
        }

        /// <summary>
        /// The method to draw the background
        /// </summary>
        /// <param name="sprite">The spritebach that will draw the images onto the gaming field</param>
        public void Draw(SpriteBatch sprite)
        {
            sprite.Draw(backgaroundImage, origin, Color.White);
            if (nextSpot != null) sprite.Draw(backgaroundImage, nextSpot, Color.White);
            if (backgaroundImage.Height < screenHeight)
            {
                sprite.Draw(backgaroundImage, new Vector2(0, backgaroundImage.Height), Color.White);
                sprite.Draw(backgaroundImage, new Vector2(backgaroundImage.Width, backgaroundImage.Height), Color.White);
            }
        }

        #endregion
    }
}
