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
        bool surge1Used = false, surge2Used = false, surge3Used = false, offHandExhausted = false;
        int cost = 0, hands = 1, dice1 = 0, dice2 = 0, dice3 = 0, shieldType = 0;
        Rectangle equipmentSpriteRect;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for the weapon currently being used
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
        public Equipment(string name, string type, string trait1, string trait2, string bonus, string surge1, string surge2, string surge3, int cost, int hands, int dice1, int dice2, int dice3)
        {
            this.name = name; this.type = type; this.trait1 = trait1; this.trait2 = trait2; this.bonus1 = bonus; this.surge1 = surge1; this.surge2 = surge2; this.surge3 = surge3;
            this.cost = cost; this.hands = hands; this.dice1 = dice1; this.dice2 = dice2; this.dice3 = dice3;
        }

        /// <summary>
        /// Constructor for the shield currently being used in the offhand
        /// </summary>
        /// <param name="name">Name of the shield</param>
        /// <param name="shieldType">Number indicating the shields "strength": 0 = wooden, 1 = Iron Shield, 2 = Heavy Steel Shield</param>
        public Equipment(string name, int shieldType)
        {
			this.name = name; type = "shield"; this.trait1 = "Shield"; this.shieldType = shieldType; this.hands = 1;
        }

        /// <summary>
        /// Constructor for the trinket currently being used
        /// </summary>
        /// <param name="name">Name of the trinket</param>
        /// <param name="bonus1">The first bonus of the trinket</param>
        /// <param name="bonus2">The second bonus of the trinket, if any</param>
        public Equipment(string name, string bonus1, string bonus2)
        {
			this.name = name; type = "trinket"; this.trait1 = "Trinket"; this.bonus1 = bonus1; this.bonus2 = bonus2;
        }

        #endregion

        #region Properties

        public Rectangle EquipmentSpriteRect
        {
            get { return equipmentSpriteRect; }
            set { equipmentSpriteRect = value; }
        }

        #region String Properties

        public string Name { get { return name; } }
        public string Type { get { return type; } }
        public string Trait1 { get { return trait1; } }
        public string Trait2 { get { return trait2; } } 
        public string Bonus1 { get { return bonus1; } }
        public string Bonus2 { get { return bonus2; } }
        public string Surge1 { get { return surge1; } } 
        public string Surge2 { get { return surge2; } }
        public string Surge3 { get { return surge3; } }

        #endregion

        #region Int Properties

        public int Cost
        {
            get { return cost; }
        }

        public int Hands
        {
            get { return hands; }
        }

        public int Dice1
        {
            get { return dice1; }
        }

        public int Dice2
        {
            get { return dice2; }
        }

        public int Dice3
        {
            get { return dice3; }
        }

        #endregion

        #region Bool Properties

        public bool Surge1Used
        {
            get { return surge1Used; }
            set { surge1Used = value; }
        }

        public bool Surge2Used
        {
            get { return surge2Used; }
            set { surge2Used = value; }
        }

        public bool Surge3Used
        {
            get { return surge3Used; }
            set { surge3Used = value; }
        }

        public bool OffHandExhausted
        {
            get { return offHandExhausted; }
            set { offHandExhausted = value; }
        }

        #endregion

        #endregion

        #region Public Classes

        #endregion

        #region Private Classes

        #endregion
    }
}
