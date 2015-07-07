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
	public class Card
	{
		#region Fields
        Rectangle drawRectangle, sourceRectangle;

		// Search Card Values
		string name; int value;

		// Peril Card Values
		bool hasMonsterSide = true, active = false;
		string emptyFlavorText, emptyCondition, monsterFlavorText = "", monsterCondition = "";
		#endregion

        // Shop Card Values
        string type = "", trait = "", bonus1 = "", bonus2 = "", surge1 = "", surge2 = "", surge3="";
        int attackDie1, attackDie2, attackDie3, defenseDie1, defenseDie2, numOfHands = 1;

        // Encounter Room Values
        int timer = 0;

        // 

		#region Constructors
		/// <summary>
		/// Constructs a search card
		/// </summary>
		/// <param name="content">Content manager</param>
		/// <param name="spriteName">Name of the sprite to draw</param>
		/// <param name="name">Name of search card</param>
		/// <param name="value">Value of search card</param>
		public Card(string name, int value, Rectangle source)
		{
            sourceRectangle = source;
			this.name = name; this.value = value;
            drawRectangle = new Rectangle(GameConstants.HALF_WINDOW_WIDTH() - source.Width, GameConstants.HALF_WINDOW_HEIGHT() - source.Height, source.Width, source.Height);
		}

		/// <summary>
		/// Constructs a peril card
		/// </summary>
		/// <param name="content">Content manager</param>
		/// <param name="spriteName">Name of the sprite to draw</param>
		/// <param name="hasMonsterSide">Determines if there is a monster effect</param>
		/// <param name="emptyFlavorText">Flavor text for empty rooms</param>
		/// <param name="emptyCondition">Condition to effect heroes in empty room</param>
		/// <param name="monsterFlavorText">Flavor text when monsters in room</param>
		/// <param name="monsterCondition">Condition to effect heroes when monsters present</param>
		public Card(bool hasMonsterSide, string emptyFlavorText, string emptyCondition, string monsterFlavorText, string monsterCondition, Rectangle source)
        {
            sourceRectangle = source;
            drawRectangle = new Rectangle(GameConstants.HALF_WINDOW_WIDTH() - source.Width, GameConstants.HALF_WINDOW_HEIGHT() - source.Height, source.Width, source.Height);
            if (hasMonsterSide)
            {
                this.monsterFlavorText = monsterFlavorText;
                this.monsterCondition = monsterCondition;
            }
            this.emptyFlavorText = emptyFlavorText;
            this.emptyCondition = emptyCondition;
            this.hasMonsterSide = hasMonsterSide;
        }
        
        /// <summary>
        /// Constructs a shop card
        /// </summary>
        /// <param name="content">Content Manager</param>
        /// <param name="spriteName">Sprite path/name</param>
        /// <param name="name">Name of card</param>
        /// <param name="type">Type: melee, range, armor, trinket, helm, belt, etc</param>
        /// <param name="trait">Exoit, slashing, heavy armor, etc</param>
        /// <param name="bonus1">Bonus trait one</param>
        /// <param name="bonus2">Bonus trait two</param>
        /// <param name="surge1">Surge 1</param>
        /// <param name="surge2">Surge 2</param>
        /// <param name="surge3">Surge 3</param>
        /// <param name="numbOfHands">Number of hands the weapon uses</param>
        /// <param name="die1">Attack die color 1: blue 0, red 1, yellow 2</param>
        /// <param name="die2">Attack die color 2: blue 0, red 1, yellow 2</param>
        /// <param name="die3">Attack die color 3: blue 0, red 1, yellow 2</param>
        /// <param name="def1">Defense die color 1: brown 3, gray 4, black 5</param>
        /// <param name="def2">Defense die color 2: brown 3, gray 4, black 5</param>
        public Card(Rectangle source, string name, string type, string trait, string bonus1, string bonus2, string surge1, string surge2, string surge3,
                    int numbOfHands, int die1, int die2, int die3, int def1, int def2)
        {
            sourceRectangle = source;
            this.name = name; this.type = type; this.trait = trait; this.bonus1 = bonus1; this.bonus2 = bonus2; this.surge1 = surge1; this.surge2 = surge2; this.surge3 = surge3;
            this.numOfHands = numbOfHands; attackDie1 = die1; attackDie2 = die2; attackDie3 = die3; defenseDie1 = def1; defenseDie2 = def2;
        }

        /// <summary>
        /// Constructor for an encounter room card
        /// </summary>
        /// <param name="source">The source off the sprite sheet</param>
        /// <param name="drawRectangle">The location of where to draw the card</param>
        /// <param name="timer">How long the heroes have to manage in the room</param>
        public Card(Rectangle source, Rectangle drawRectangle, int timer)
        {
            this.sourceRectangle = source; this.drawRectangle = drawRectangle; this.timer = timer;
        }

		#endregion

		#region Properties
        public Rectangle DrawRectangle { get { return drawRectangle; } set { drawRectangle = value; } }
        public Rectangle SourceRectangle { get { return sourceRectangle; } }
        public string Name { get { return name; } set { name = value; } }
		public int Value { get { return value; } }
		public bool Active { get { return active; } set { active = value; } }
		public bool HasMonsterSide { get { return hasMonsterSide; } }
		public string EmptyFlavorText { get { return emptyFlavorText; } }
		public string EmptyCondition { get { return EmptyCondition; } }
		public string MonsterFlavorText { get { return monsterFlavorText; } }
		public string MonsterCondition { get { return monsterCondition; } }
        public string Type { get { return type; } }
        public string Trait { get { return trait; } }
        public string Bonus1 { get { return bonus1; } }
        public string Bonus2 { get { return bonus2; } }
        public string Surge1 { get { return surge1; } }
        public string Surge2 { get { return surge2; } }
        public string Surge3 { get { return surge3; } }
        public int Attack1 { get { return attackDie1; } }
        public int Attack2 { get { return attackDie2; } }
        public int Attack3 { get { return attackDie3; } }
        public int Defense1 { get { return defenseDie1; } }
        public int Defense2 { get { return defenseDie2; } }
        public int NumOfHands { get { return numOfHands; } }
        public int Timer { get { return timer; } set { timer = value; } }
		#endregion

		#region Public Methods
		public void Draw(SpriteBatch spriteBatch, Texture2D sprite) { spriteBatch.Draw(sprite, drawRectangle, sourceRectangle, Color.White); }
		#endregion

		#region Private Methods
		#endregion
	}
}
