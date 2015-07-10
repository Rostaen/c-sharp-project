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
    public class Equipment
    {
        #region Fields

        string name = "", type = "", trait1 = "", trait2 = "", bonus1 = "", bonus2 = "", surge1 = "", surge2 = "", surge3 = "";
        bool offHandExhausted = false, startingGear = false, slotUsed = false;
        int cost = 0, hands = 1, dice1 = 0, dice2 = 0, dice3 = 0, def1 = -1;
        Rectangle equipmentSpriteRect, sourceRectangle;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for the starting weapon currently being used
        /// </summary>
        /// <param name="name">Name of the weapon</param>
        /// <param name="type">Type of the weapon: melee or range</param>
        /// <param name="trait1">Trait of the weapon: bow, sword, wand, rune, etc</param>
        /// <param name="trait2">Trait2 of the weapon: bow, sword, wand, rune, etc</param>
        /// <param name="bonus">Bonus applied to the weapon: Stun, +1 range, piece X, etc</param>
        /// <param name="surge1">Surge 1 useage: +1 damage, +1 range, etc</param>
        /// <param name="surge2">Surge 2 useage: +1 damage, +1 range, etc</param>
        /// <param name="surge3">Surge 3 useage: +1 damage, +1 range, etc</param>
        /// <param name="cost">Price of the weapon</param>
        /// <param name="hands">Number of hands the weapon uses: 1 or 2 handed</param>
        /// <param name="dice1">Dice 1 color: 0 = blue, 1 = red, 2 = yellow</param>
        /// <param name="dice2">Dice 2 color: 0 = blue, 1 = red, 2 = yellow</param>
        /// <param name="dice3">Dice 3 color: 0 = blue, 1 = red, 2 = yellow</param>
        public Equipment(Rectangle source, string name, string type, string trait1, string trait2, string bonus, string surge1, string surge2, string surge3, int cost, int hands, int dice1, int dice2, int dice3)
        {
            sourceRectangle = source;
            this.name = name; this.type = type; this.trait1 = trait1; this.trait2 = trait2; this.bonus1 = bonus; this.surge1 = surge1; this.surge2 = surge2; this.surge3 = surge3;
            this.cost = cost; this.hands = hands; this.dice1 = dice1; this.dice2 = dice2; this.dice3 = dice3;
        }

        /// <summary>
        /// Constructor for the shield currently being used in the offhand
        /// </summary>
        /// <param name="name">Name of the shield</param>
        /// <param name="shieldType"></param>
        public Equipment(Rectangle source, string name, string bonus)
        {
            sourceRectangle = source;
            this.name = name; type = "shield"; this.trait1 = "Shield"; this.hands = 1; this.bonus1 = bonus;
        }

        /// <summary>
        /// Constructor for the trinket currently being used
        /// </summary>
        /// <param name="name">Name of the trinket</param>
        /// <param name="bonus1">The first bonus of the trinket</param>
        /// <param name="bonus2">The second bonus of the trinket, if any</param>
        public Equipment(Rectangle source, string name, string bonus1, string bonus2)
        {
            sourceRectangle = source;
            this.name = name; type = "trinket"; this.trait1 = "Trinket"; this.bonus1 = bonus1; this.bonus2 = bonus2; 
        }

        /// <summary>
        /// Constructor for Armor
        /// </summary>
        /// <param name="name">Name of the armor</param>
        /// <param name="trait">The type of armor</param>
        /// <param name="bonus1">Bonus applied from armor</param>
        /// <param name="bonus2">2nd bonus applied from armor</param>
        /// <param name="die">Color of the die being assigned by armor: 3 = brown, 4 = gray, 5 = black</param>
        public Equipment(Rectangle source, string name, string trait, string bonus1, string bonus2, int die)
        {
            sourceRectangle = source;
            this.name = name; type = "shield"; this.trait1 = trait; this.bonus1 = bonus1; this.bonus2 = bonus2; def1 = die;
        }

        #endregion

        #region Properties

        #region Rectangles
        public Rectangle EquipmentSpriteRect { get { return equipmentSpriteRect; } set { equipmentSpriteRect = value; } }
        public Rectangle DrawRectangle { get { return equipmentSpriteRect; } set { equipmentSpriteRect = value; } }
        public Rectangle SourceRect { get { return sourceRectangle; } set { sourceRectangle = value; } }
        #endregion

        #region String Properties

        public string Name { get { return name; } set { name = value; } }
        public string Type { get { return type; } set { type = value; } }
        public string Trait1 { get { return trait1; } set { trait1 = value; } }
        public string Trait2 { get { return trait2; } set { trait2 = value; } }
        public string Bonus1 { get { return bonus1; } set { bonus1 = value; } }
        public string Bonus2 { get { return bonus2; } set { bonus2 = value; } }
        public string Surge1 { get { return surge1; } set { surge1 = value; } }
        public string Surge2 { get { return surge2; } set { surge2 = value; } }
        public string Surge3 { get { return surge3; } set { surge3 = value; } }

        #endregion

        #region Int Properties

        public int Cost { get { return cost; } }
        public int Hands { get { return hands; } set { hands = value; } }
        public int Dice1 { get { return dice1; } set { dice1 = value; } }
        public int Dice2 { get { return dice2; } set { dice2 = value; } }
        public int Dice3 { get { return dice3; } set { dice3 = value; } }

        #endregion

        #region Bool Properties
        public bool SlotUsed { get { return slotUsed; } set { slotUsed = value; } }
        public bool StartingGear { get { return startingGear; } set { startingGear = value; } }
        public bool OffHandExhausted { get { return offHandExhausted; } set { offHandExhausted = value; } }

        #endregion

        #endregion

        #region Public Classes

        public void Draw(SpriteBatch spriteBatch, Texture2D sprite)
        {
            spriteBatch.Draw(sprite, equipmentSpriteRect, Color.White);
        }

        #endregion

        #region Private Classes

        #endregion
    }
}
