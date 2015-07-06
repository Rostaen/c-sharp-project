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
    public class HeroToken
    {
        #region Fields

        // Graphics and drawing info
        Texture2D sprite;
        Rectangle drawRectangle;

        // Hero Stats
        string heroName = "";
        bool active = true;

        // Token sound effects
        SoundEffect heroMovementSound = null, heroAttackSound = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a hero token for the selected hero
        /// </summary>
        /// <param name="contentManager">The content manager for loading content</param>
        /// <param name="sprite">The image of the selected hero token</param>
        /// <param name="nameOfHero">Name of the selected hero</param>
        /// <param name="spriteName">Name of the sprint image to use</param>
        /// <param name="heroNumber">Number of the current hero for initial board placement</param>
        public HeroToken(ContentManager contentManager, Texture2D sprite, string nameOfHero, string spriteName, int heroNumber)
        {
            LoadContent(contentManager, spriteName, heroNumber);
            heroName = nameOfHero; this.sprite = sprite;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets and sets whether or not the hero is active
        /// </summary>
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        public Rectangle DrawRectangle
        {
            get { return drawRectangle; }
            set { drawRectangle = value; }
        }

        /// <summary>
        /// Sets the X locatoin of the center of the hero token
        /// </summary>
        public int X
        {
            set { drawRectangle.X = value - drawRectangle.Width / 2; }
        }

        /// <summary>
        /// Sets the Y locatoin of the center of the hero token
        /// </summary>
        public int Y
        {
            set { drawRectangle.Y = value - drawRectangle.Height / 2; }
        }

        /// <summary>
        /// Gets the movement sound effect for hero tokens moving
        /// </summary>
        public SoundEffect HeroMovementSound
        {
            get { return heroMovementSound; }
        }

        /// <summary>
        /// Gets and sets the hero attack sound, melee/ranged/magic/thrown
        /// </summary>
        public SoundEffect HeroAttackSound
        {
            get { return heroAttackSound; }
            set { heroAttackSound = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Draws the hero token
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to use</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, drawRectangle, Color.White);
        }

        /// <summary>
        /// Updates the hero token around the board based on where the mouse is clicked
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="mouse">The current state of the mouse (to get X/Y position for movement)</param>
        public void Update(GameTime gameTime, MouseState mouse)
        {
            if (active)
            {

            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Loads the content for the given hero token
        /// </summary>
        /// <param name="contentManager">the content manager to use</param>
        /// <param name="spriteName">the name of the sprite for the hero token</param>
        /// <param name="X">the X location of the center of the hero token</param>
        /// <param name="Y">the Y location of the center of the hero token</param>
        /// <param name="heroNumber">The current hero number for initial board placement</param>
        private void LoadContent(ContentManager contentManager, string spriteName, int heroNumber)
        {
            int tempX, tempY; // used to place the hero token on the board
            switch (heroNumber)
            {
                case 1: tempX = HalfWindowWidth() - 32; tempY = HalfWindowHeight() - 32; break;
                case 2: tempX = HalfWindowWidth() + 32; tempY = HalfWindowHeight() - 32; break;
                case 3: tempX = HalfWindowWidth() - 32; tempY = HalfWindowHeight() + 32; break;
                case 4: tempX = HalfWindowWidth() + 32; tempY = HalfWindowHeight() + 32; break;
                default: tempX = HalfWindowWidth(); tempY = HalfWindowHeight(); break;
            }
            sprite = contentManager.Load<Texture2D>(spriteName);

            drawRectangle = new Rectangle(64, 64, sprite.Width, sprite.Height);
        }

        /// <summary>
        /// Returns ½ the window width
        /// </summary>
        /// <returns>½ Window Width</returns>
        private int HalfWindowWidth()
        {
            return GameConstants.WINDOW_WIDTH / 2;
        }

        /// <summary>
        /// Returns ½ the window height
        /// </summary>
        /// <returns>½ Window Height</returns>
        private int HalfWindowHeight()
        {
            return GameConstants.WINDOW_HEIGHT / 2;
        }

        #endregion
    }
}
