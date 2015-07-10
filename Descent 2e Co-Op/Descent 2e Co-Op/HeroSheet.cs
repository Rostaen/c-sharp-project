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
    /// <summary>
    /// This is the hero sheet clas. This class holds all the information regarding the hero sheet for each player, including hit points, stamina, movement, defense dice, etc.
    /// </summary>
    public class HeroSheet
    {
        #region Fields

        // Graphics and drawing info
        Rectangle drawRectangle, sourceRectangle;

        // Hero Stats
        string heroName = "", archetype = "", heroicAbility = "", heroicFeat = "";
        int hitPoints = 0, stamina = 0, movement = 0, defense = 0, willPower = 0, strength = 0, knowledge = 0, awareness = 0, searchRange = 1;
        private int maxHitPoints = 0, maxStamina = 0, fatigueOverflow = 0, actionPoints = 2, maxMovement = 0;
        bool[] conditions = { false, false, false, false }; // Conditions are as such: Poison, Disease, Stun, Immobilized
        // Heroic ability will be given a string indicator
        // Heroic feats will be labeled as either passive or active
        bool heroicFeetUsed = false, knockedOut = false, dead = false, active = true, activeSheet = false, resting = false;

        List<Dice> defenseDicePool = new List<Dice>();
        
        HeroClass pickedClass;


        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a HeroSheet for the selected hero
        /// </summary>
        /// <param name="classPicked">String of the picked class</param>
        /// <param name="classSheet">Sprite sheet containing the cards for the selected class</param>
        /// <param name="archetype">String of the archetype</param>
        /// <param name="heroName">Name of the chosen hero</param>
        /// <param name="heroSheet">The name of the sprite image to use</param>
        /// <param name="spriteSheet">Spritesheet containing dice images</param>
        /// <param name="hp">Hit points of the selected hero</param>
        /// <param name="sta">Stamina of the selected hero</param>
        /// <param name="move">Movement of the selected hero</param>
        /// <param name="def">Defense value of the selected hero (3 = brown, 4 = gray, 5 = black)</param>
        /// <param name="wp">Willpower of the selected hero</param>
        /// <param name="str">Strength of the selected hero</param>
        /// <param name="know">Knowledge of the selected hero</param>
        /// <param name="aware">Awareness of the selected hero</param>
        /// <param name="heroAbil">Type of hero ability: status, surge, etc</param>
        /// <param name="heroFeat">Type of hero feat: activation, passive</param>
        /// <param name="X">The X location of the center of the hero sheet</param>
        /// <param name="Y">The Y location of the center of the hero sheet</param>
        public HeroSheet(string classPicked, string archetype, string heroName, int hp, int sta, int move, int def, int wp, int str, int know, int aware, string heroAbil, string heroFeat, int X, int Y)
        {
            this.archetype = archetype;
            LoadContent(heroName, X, Y);
            this.heroName = heroName; maxHitPoints = hp; maxStamina = sta; maxMovement = move;
            hitPoints = hp; stamina = sta; movement = move; defense = def; willPower = wp; strength = str; knowledge = know; awareness = aware;
            heroicAbility = heroAbil; heroicFeat = heroFeat;
            defenseDicePool.Add(new Dice(def));
            pickedClass = new HeroClass(classPicked);
			int die2Color = pickedClass.CurrentWeapon.Dice2;
        }

        #endregion

        #region Properties

        public bool Resting { get { return resting; } set { resting = value; } }
        public HeroClass PickedClass { get { return pickedClass; } }
		public int SearchRange { get { return searchRange; } set { searchRange = value; } }
		public int MaxHP { get { return maxHitPoints; } }
		public int MaxStam { get { return maxStamina; } }
		public int MaxMovement { get { return maxMovement; } }
        public string Archetype { get { return archetype; } }
        public string Name { get { return heroName; } }

        /// <summary>
        /// Gets and sets whether or not the hero is active
        /// </summary>
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        /// <summary>
        /// Gets/sets if the hero sheet is active based on clicked token
        /// </summary>
        public bool ActiveSheet
        {
            get { return activeSheet; }
            set { activeSheet = value; }
        }

        /// <summary>
        /// Gets and sets the draw rectangle for the hero sheet
        /// </summary>
        public Rectangle DrawRectangle
        {
            get { return drawRectangle; }
            set { drawRectangle = value; }
        }

        /// <summary>
        /// Sets the X locatoin of the center of the hero sheet
        /// </summary>
        public int X
        {
            set { drawRectangle.X = value - drawRectangle.Width / 2; }
        }

        /// <summary>
        /// Sets the Y locatoin of the center of the hero sheet
        /// </summary>
        public int Y
        {
            set { drawRectangle.Y = value - drawRectangle.Height / 2; }
        }

        /// <summary>
        /// Gets and sets the current hit points of the hero
        /// </summary>
        public int Health
        {
            get { return hitPoints; }
            set
            {
                hitPoints = value;
                if (hitPoints > maxHitPoints) hitPoints = maxHitPoints;
                if (hitPoints < 0) hitPoints = 0;
            }
        }

        /// <summary>
        /// Gets and sets the stamina for hero. If stamina is healed over the max stamina, stamina is set to it's current max. If stamina goes below zero, the negative valuse is applied to health as damage
        /// </summary>
        public int Stamina
        {
            get { return stamina; }
            set
            {
                stamina = value;
                if (stamina > maxStamina) stamina = maxStamina;
                if (stamina < 0)
                {
                    fatigueOverflow = stamina;
                    stamina = 0;
                    Health += fatigueOverflow;
                }
            }
        }

        /// <summary>
        /// Get's the heroes movement amount to move on the board
        /// </summary>
        public int Movement
        {
            get { return movement; }
        }

        /// <summary>
        /// Gets and sets the defense value for the hero
        /// 1: Brown Defense Die
        /// 2: Gray Defense Die
        /// 3: Black Defense Die
        /// </summary>
        public int Defense
        {
            get { return defense; }
            set { defense = value; }
        }

        /// <summary>
        /// Gets and sets the heroes willpower stat for skill checks
        /// </summary>
        public int Willpower
        {
            get { return willPower; }
            set { willPower = value; }
        }

        /// <summary>
        /// Gets and sets the heroes strength stat for skill checks
        /// </summary>
        public int Strength
        {
            get { return strength; }
            set { strength = value; }
        }

        /// <summary>
        /// Gets and sets the heroes knowledge stat for skill checks
        /// </summary>
        public int Knowledge
        {
            get { return knowledge; }
            set { knowledge = value; }
        }

        /// <summary>
        /// Gets and sets the heroes awareness stat for skill checks
        /// </summary>
        public int Awareness
        {
            get { return awareness; }
            set { awareness = value; }
        }

        /// <summary>
        /// Gets the heroes heroic ability. A status is used to termine how ability works form a switch statement:
        /// status, surge, etc.
        /// </summary>
        public string HeroicAbility
        {
            get { return heroicAbility; }
        }

        /// <summary>
        /// Gets the heroes heroic feat. A status is returned declaring either activation or passive
        /// </summary>
        public string HeroicFeat
        {
            get { return heroicFeat; }
        }

        /// <summary>
        /// Gets/sets if the heoric feat has been used
        /// </summary>
        public bool HeroicFeatUsed
        {
            get { return heroicFeetUsed; }
            set { heroicFeetUsed = value; }
        }

        /// <summary>
        /// Gets and sets if the hero has been knocked out or not
        /// </summary>
        public bool KnockedOut
        {
            get { return knockedOut; }
            set { knockedOut = value; }
        }

        /// <summary>
        /// Gets sets whether the hero is dead or not. Only used during the final encounter
        /// </summary>
        public bool Dead
        {
            get { return dead; }
            set { dead = value; }
        }

        /// <summary>
        /// Gets and sets the actions points for the hero
        /// </summary>
        public int ActionPoints
        {
            get { return actionPoints; }
            set { actionPoints = value; }
        }

        /// <summary>
        /// Gets and sets the conditions affecting a hero
        /// </summary>
        public bool[] Conditions
        {
            get { return conditions; }
            set { conditions = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Draws the hero sheet
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to use</param>
        public void Draw(SpriteBatch spriteBatch, Texture2D spriteSheet, Texture2D classSheet, Texture2D shopSheet)
        {
            spriteBatch.Draw(spriteSheet, drawRectangle, sourceRectangle, Color.White);
            pickedClass.DrawSkillEquipment(spriteBatch, classSheet, shopSheet);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Loads the content for the given hero sheet
        /// </summary>
        /// <param name="contentManager">the content manager to use</param>
        /// <param name="spriteSheet">the name of the sprite for the hero sheet</param>
        /// <param name="X">the X location of the center of the hero sheet</param>
        /// <param name="Y">the Y location of the center of the hero sheet</param>
        private void LoadContent(string heroName, int X, int Y)
        {
            Rectangle sourceRect = new Rectangle();
            switch (heroName)
            {
                case "Ashrian": sourceRect = new Rectangle(0, 0, 320, 256); break;
                case "Avric": sourceRect = new Rectangle(0, 256, 320, 256); break;
                case "Leoric": sourceRect = new Rectangle(0, 512, 320, 256); break;
                case "Tarha": sourceRect = new Rectangle(0, 768, 320, 256); break;
                case "Jain": sourceRect = new Rectangle(0, 0, 320, 256); break;
                case "Tomble": sourceRect = new Rectangle(0, 256, 320, 256); break;
                case "Grisban": sourceRect = new Rectangle(0, 512, 320, 256); break;
                case "Syndrael": sourceRect = new Rectangle(0, 768, 320, 256); break;
                default: break;
            }
            drawRectangle = new Rectangle(X - sourceRect.Width / 2, Y - sourceRect.Height, sourceRect.Width, sourceRect.Height);
            this.sourceRectangle = sourceRect;
        }

        #endregion
    }
}
