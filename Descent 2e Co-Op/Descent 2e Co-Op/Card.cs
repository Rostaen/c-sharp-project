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
		Texture2D sprite;
		Rectangle drawRectangle;

		// Search Card Values
		string name; int value;

		// Peril Card Values
		bool hasMonsterSide = true, active = false;
		string emptyFlavorText, emptyCondition, monsterFlavorText = "", monsterCondition = "";
		#endregion

        // Shop Card Values
        string type = "", trait = "", bonus1 = "", bonus2 = "", surge1 = "", surge2 = "", surge3="";
        Dice attackDie1, attackDie2, attackDie3, defenseDie1, defenseDie2;
        int numOfHands = 1;


		#region Constructors
		/// <summary>
		/// Constructs a search card
		/// </summary>
		/// <param name="content">Content manager</param>
		/// <param name="spriteName">Name of the sprite to draw</param>
		/// <param name="name">Name of search card</param>
		/// <param name="value">Value of search card</param>
		public Card(ContentManager content, string spriteName, string name, int value)
		{
			this.name = name; this.value = value;
			sprite = content.Load<Texture2D>(spriteName);
			drawRectangle = new Rectangle(GameConstants.HALF_WINDOW_WIDTH() - sprite.Width, GameConstants.HALF_WINDOW_HEIGHT() - sprite.Height, sprite.Width, sprite.Height);
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
		public Card(ContentManager content, string spriteName, bool hasMonsterSide, string emptyFlavorText, string emptyCondition, string monsterFlavorText, string monsterCondition)
        {
            this.sprite = content.Load<Texture2D>(spriteName);
            drawRectangle = new Rectangle(GameConstants.HALF_WINDOW_WIDTH() - sprite.Width, GameConstants.HALF_WINDOW_HEIGHT() - sprite.Height, sprite.Width, sprite.Height);
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
        public Card(ContentManager content, string spriteName, string name, string type, string trait, string bonus1, string bonus2, string surge1, string surge2, string surge3,
                    int numbOfHands, int die1, int die2, int die3, int def1, int def2)
        {
            this.sprite = content.Load<Texture2D>(spriteName);
            this.name = name; this.type = type; this.trait = trait; this.bonus1 = bonus1; this.bonus2 = bonus2; this.surge1 = surge1; this.surge2 = surge2; this.surge3 = surge3;
            this.numOfHands = numbOfHands; attackDie1 = new Dice(die1); attackDie2 = new Dice(die2); attackDie3 = new Dice(die3); defenseDie1 = new Dice(def1); defenseDie2 = new Dice(def2);
        }
		#endregion

		#region Properties
		public Rectangle DrawRectangle { get { return drawRectangle; } }
		public string Name { get { return name; } }
		public int Value { get { return value; } }
		public bool Active { get { return active; } set { active = value; } }
		public bool HasMonsterSide { get { return hasMonsterSide; } }
		public string EmptyFlavorText { get { return emptyFlavorText; } }

		public string EmptyCondition
		{
			get { return EmptyCondition; }
		}

		public string MonsterFlavorText
		{
			get { return monsterFlavorText; }
		}

		public string MonsterCondition
		{
			get { return monsterCondition; }
		}
		#endregion

		#region Public Methods
		public void Draw(SpriteBatch spriteBatch) { spriteBatch.Draw(sprite, drawRectangle, Color.White); }
		#endregion

		#region Private Methods
		#endregion
	}
}
