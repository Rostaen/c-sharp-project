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
    public class Dice
    {
        #region Fields
        /// <summary>
        /// 2 demension array for dice image sides [color die (blue, red, yellow, brown, gray, black), die side (1-6)]
        /// </summary>
        Rectangle[] diceSidesSourceRect = new Rectangle[6];
        int range = 0, attack = 0, surge = 0, defense = 0, dieColor = -1, randomSide = -1;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor to make a die to be rolled in the game, either attack or defense die
        /// </summary>
        /// <param name="content">Content manager being sent in</param>
        /// <param name="dieColor">Die number: 0 = blue, 1 = red, 2 = yellow, 3 = brown, 4 = gray, 5 = black </param>
        public Dice(int dieColor) 
        {
            this.dieColor = dieColor;
            switch (dieColor)
            {
                // blue
                case 0: for (int x = 0; x < 6; x++) diceSidesSourceRect[x] = new Rectangle((32*x), 320, 32, 32); break;
                // red
                case 1:
                    diceSidesSourceRect[0] = new Rectangle(416, 320, 32, 32);
                    for (int y = 1; y < 4; y++) diceSidesSourceRect[y] = new Rectangle(384, 320, 32, 32); 
                    diceSidesSourceRect[4] = new Rectangle(480, 320, 32, 32);
                    diceSidesSourceRect[5] = new Rectangle(448, 320, 32, 32);
                    break;
                // yellow
                case 2: for (int z = 0; z < 6; z++) diceSidesSourceRect[z] = new Rectangle(192+(32 * z), 320, 32, 32); break;
                // brown
                case 3:
                    for (int r = 0; r < 3; r++) diceSidesSourceRect[r] = new Rectangle(512, 320, 32, 32); 
                    for (int r = 3; r < 5; r++) diceSidesSourceRect[r] = new Rectangle(544, 320, 32, 32); 
                    diceSidesSourceRect[5] = new Rectangle(576, 320, 32, 32); 
                    break;
                // gray
                case 4:
                    diceSidesSourceRect[0] = new Rectangle(640, 320, 32, 32); 
                    for (int s = 1; s < 4; s++) diceSidesSourceRect[s] = new Rectangle(608, 320, 32, 32); 
                    diceSidesSourceRect[4] = new Rectangle(704, 320, 32, 32); 
                    diceSidesSourceRect[5] = new Rectangle(672, 320, 32, 32); 
                    break;
                // black
                case 5:
                    diceSidesSourceRect[0] = new Rectangle(800, 320, 32, 32); 
                    for (int t = 1; t < 4; t++) diceSidesSourceRect[t] = new Rectangle(736, 320, 32, 32); 
                    diceSidesSourceRect[4] = new Rectangle(768, 320, 32, 32); 
                    diceSidesSourceRect[5] = new Rectangle(832, 320, 32, 32); 
                    break;
                default: break;
            }
        }

        #endregion

        #region Properties

		public Rectangle[] DiceSides { get { return diceSidesSourceRect; } }
		public int RandomSide { get { return randomSide; } }
		public int DieColor { get { return dieColor; } set { dieColor = value; } }

        /// <summary>
        /// Returns the total range from attack result
        /// </summary>
        public int Range
        {
            get { return range; }
            set { range = value; }
        }
        /// <summary>
        /// Returns the total attack from attack result
        /// </summary>
        public int Attack
        {
            get { return attack; }
            set { attack = value; }
        }
        /// <summary>
        /// Returns the total range from surge result
        /// </summary>
        public int Surge
        {
            get { return surge; }
            set { surge = value; }
        }
        /// <summary>
        /// Returns the total defense from attack result
        /// </summary>
        public int Defense
        {
            get { return defense; }
            set { defense = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Rolls a die each time the method is called the adds the dice to a list of dice used for drawing later on.
        /// </summary>
        /// <param name="dieColor">The color of the die to use: 0 = blue, 1 = red, 2 = yellow, 3 = brown, 4 = gray, 5 = black</param>
        /// <param name="randomSide">A sent random number to pick the side of the dice to use, 0 - 5</param>
        public void RollDie(int randomSide)
        {
            this.randomSide = randomSide;
            if (dieColor < 3)
            {
                range += GameConstants.DICE_SIDES_ATTACK[dieColor, randomSide, 0];
                attack += GameConstants.DICE_SIDES_ATTACK[dieColor, randomSide, 1];
                surge += GameConstants.DICE_SIDES_ATTACK[dieColor, randomSide, 2];
            }
            else defense += GameConstants.DICE_SIDES_DEFENSE[dieColor - 3, randomSide];
        }

        /// <summary>
        /// Draws the dice being rolled
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to draw sprites</param>
        /// <param name="dieRolled">The number position of the die being rolled, 1st, 2nd, 3rd, etc</param>
        public void Draw(SpriteBatch spriteBatch, int dieRolled, Texture2D spriteSheet)
        {
            if (dieColor < 3) spriteBatch.Draw(spriteSheet, new Rectangle(GameConstants.DICE_DRAW_START_X + (dieRolled * GameConstants.DICE_DRAW_BUFFER_X), GameConstants.DICE_DRAW_START_Y, 32, 32), diceSidesSourceRect[randomSide], Color.White);
            else spriteBatch.Draw(spriteSheet, new Rectangle(GameConstants.DICE_DEF_START_X + (dieRolled * GameConstants.DICE_DRAW_BUFFER_X), GameConstants.DICE_DRAW_START_Y, 32, 32), diceSidesSourceRect[randomSide], Color.White);
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
