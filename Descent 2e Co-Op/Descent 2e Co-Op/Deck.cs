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
	public class Deck
	{
		#region Fields

		List<Card> cardDeck = new List<Card>();
		List<Card> discardDeck = new List<Card>();
		string type, effectCondition;
		//bool linguringEffect = false;

		#endregion

		#region Constructors

		/// <summary>
		/// Constructs a deck based of the type passed in.
		/// </summary>
		/// <param name="content">The content manager</param>
        /// <param name="type">The type of deck to construct: Search, Peril, Shop 1, Shop 2, Activation, Room</param>
		public Deck(ContentManager content, string type, string adventureName)
		{
            this.type = type;
            if (type == "Search")
            {
                for (int x = 0; x < 3; x++) { cardDeck.Add(new Card("Health Potion", 25, new Rectangle(x * 128, 960, 128, 192))); }
                for (int x = 0; x < 3; x++) { cardDeck.Add(new Card("Stamina Potion", 25, new Rectangle(384 + (x * 128), 960, 128, 192))); }
                cardDeck.Add(new Card("Curse Doll", 50, new Rectangle(768, 960, 128, 192))); 
                cardDeck.Add(new Card("Fire Flask", 50, new Rectangle(896, 960, 128, 192))); 
                cardDeck.Add(new Card("Nothing", 0, new Rectangle(1024, 960, 128, 192))); 
                cardDeck.Add(new Card("Power Potion", 50, new Rectangle(1152, 960, 128, 192))); 
                cardDeck.Add(new Card("Treasure Chest", 0, new Rectangle(0, 960, 128, 192))); 
                cardDeck.Add(new Card("Warding Talisman", 50, new Rectangle(128, 960, 128, 192))); 
            }
            else if (type == "Peril")
            {

                cardDeck.Add(new Card(false, GameConstants.EMPTY_FLAVOR_01, GameConstants.EMPTY_COND_01, "", "", new Rectangle(0, 606, 179, 275)));
                cardDeck.Add(new Card(false, GameConstants.EMPTY_FLAVOR_02, GameConstants.EMPTY_COND_02, "", "", new Rectangle(179, 606, 179, 275)));
                cardDeck.Add(new Card(true, GameConstants.EMPTY_FLAVOR_03, GameConstants.EMPTY_COND_03, GameConstants.MON_FLAVOR_03, GameConstants.MON_COND_03, new Rectangle(358, 303, 179, 275)));
                cardDeck.Add(new Card(true, GameConstants.EMPTY_FLAVOR_04, GameConstants.EMPTY_COND_04, GameConstants.MON_FLAVOR_04, GameConstants.MON_COND_04, new Rectangle(537, 303, 179, 275)));
                cardDeck.Add(new Card(true, GameConstants.EMPTY_FLAVOR_05, GameConstants.EMPTY_COND_05, GameConstants.MON_FLAVOR_05, GameConstants.MON_COND_05, new Rectangle(716, 303, 179, 275)));
                cardDeck.Add(new Card(true, GameConstants.EMPTY_FLAVOR_06, GameConstants.EMPTY_COND_06, GameConstants.MON_FLAVOR_06, GameConstants.MON_COND_06, new Rectangle(895, 303, 179, 275)));
                cardDeck.Add(new Card(true, GameConstants.EMPTY_FLAVOR_07, GameConstants.EMPTY_COND_07, GameConstants.MON_FLAVOR_07, GameConstants.MON_COND_07, new Rectangle(1074, 303, 179, 275)));
                cardDeck.Add(new Card(false, GameConstants.EMPTY_FLAVOR_08, GameConstants.EMPTY_COND_08, "", "", new Rectangle(1253, 303, 179, 275)));
                cardDeck.Add(new Card(true, GameConstants.EMPTY_FLAVOR_09, GameConstants.EMPTY_COND_09, GameConstants.MON_FLAVOR_09, GameConstants.MON_COND_09, new Rectangle(1432, 303, 179, 275)));
                cardDeck.Add(new Card(true, GameConstants.EMPTY_FLAVOR_10, GameConstants.EMPTY_COND_10, GameConstants.MON_FLAVOR_10, GameConstants.MON_COND_10, new Rectangle(1611, 303, 179, 275)));
            }
            else if (type == "Shop 1")
            {
                cardDeck.Add(new Card(new Rectangle(0, 0, 128, 192), "Chainmail", "armor", "Heavy Armor", "Can't equip Runes", "Speed <= 4", "", "", "", 0, -1, -1, -1, 4, -1));
                cardDeck.Add(new Card(new Rectangle(128, 0, 128, 192), "Crossbow", "range", "Bow, Exotic", "pierce1", "", "2dmg", "1dmg1push", "", 1, 0, 2, -1, -1, -1));
                cardDeck.Add(new Card(new Rectangle(256, 0, 128, 192), "Elm Greatbow", "range", "Bow", "Adjacent not block LoS", "", "2dmg", "2range", "", 2, 0, 2, -1, -1, -1));
                cardDeck.Add(new Card(new Rectangle(384, 0, 128, 192), "Heavy Cloak", "armor", "Cloak", "Cancel 1 surge", "", "", "", "", 0, -1, -1, -1, -1, -1));
                cardDeck.Add(new Card(new Rectangle(512, 0, 128, 192), "Immolation", "range", "Magic, Rune", "pierce1", "", "1dmg", "1range", "", 2, 0, 1, -1, -1, -1));
                cardDeck.Add(new Card(new Rectangle(640, 0, 128, 192), "Iron Battleaxe", "melee", "Axe", "pierce1", "", "2dmg", "pierce1", "", 2, 0, 1, -1, -1, -1));
                cardDeck.Add(new Card(new Rectangle(768, 0, 128, 192), "Iron Shield", "shield", "Shield", "reroll1or1def", "", "", "", "", 1, -1, -1, -1, -1, -1));
                cardDeck.Add(new Card(new Rectangle(896, 0, 128, 192), "Iron Shield", "shield", "Shield", "reroll1or1def", "", "", "", "", 1, -1, -1, -1, -1, -1));
                cardDeck.Add(new Card(new Rectangle(1024, 0, 128, 192), "Iron Spear", "melee", "Exotic", "reach", "", "1dmg", "pierce1", "", 1, 0, 2, -1, -1, -1));
                cardDeck.Add(new Card(new Rectangle(1152, 0, 128, 192), "Leather Armor", "armor", "Light Armor", "+1hp", "", "", "", "", 0, -1, -1, -1, -1, -1));
                cardDeck.Add(new Card(new Rectangle(0, 192, 128, 192), "Leather Armor", "armor", "Light Armor", "+1hp", "", "", "", "", 0, -1, -1, -1, -1, -1));
                cardDeck.Add(new Card(new Rectangle(128, 192, 128, 192), "Light Hammer", "melee", "Hammer", "", "", "2dmg", "stun", "", 1, 0, 2, -1, -1, -1));
                cardDeck.Add(new Card(new Rectangle(256, 192, 128, 192), "Lucky Charm", "trinket", "Trinket", "rerollAttribute", "", "", "", "", 0, -1, -1, -1, -1, -1));
                cardDeck.Add(new Card(new Rectangle(384, 192, 128, 192), "Magic Staff", "range", "Magic, Staff", "", "", "1dmg>1-3sp", "1range", "", 2, 0, 1, -1, -1, -1));
                cardDeck.Add(new Card(new Rectangle(512, 192, 128, 192), "Mana Weave", "trinket", "Rune", "1surge", "", "", "", "", 0, -1, -1, -1, -1, -1));
                cardDeck.Add(new Card(new Rectangle(640, 192, 128, 192), "Ring of Power", "trinket", "Ring", "1sta", "", "", "", "", 0, -1, -1, -1, -1, -1));
                cardDeck.Add(new Card(new Rectangle(768, 192, 128, 192), "Scorpion Helm", "trinket", "Helmet", "1range", "limit1", "", "", "", 0, -1, -1, -1, -1, -1));
                cardDeck.Add(new Card(new Rectangle(896, 192, 128, 192), "Sling", "range", "Exotic", "", "", "1dmg1rng", "stun", "", 1, 0, 2, -1, -1, -1));
                cardDeck.Add(new Card(new Rectangle(1024, 192, 128, 192), "Steel Broadsword", "melee", "Blade", "rerollred", "", "1dmg", "", "", 1, 0, 1, -1, -1, -1));
                cardDeck.Add(new Card(new Rectangle(1152, 192, 128, 192), "Sunburst", "range", "Magic, Rune", "", "", "stun", "2dmg", "", 2, 0, 2, -1, -1, -1));
            }
            else if (type == "Shop 2")
            {
                cardDeck.Add(new Card(new Rectangle(0, 384, 128, 192), "Demonhide Leather", "armor", "Light Armor", "2move-1sta", "", "", "", "", 0, -1, -1, -1, 4, -1));
                cardDeck.Add(new Card(new Rectangle(128, 384, 128, 192), "Dragontooth Hammer", "melee", "Hammer", "pierce1", "emptyOff1dmg", "pierce2", "", "", 1, 0, 1, 1, -1, -1));
                cardDeck.Add(new Card(new Rectangle(256, 384, 128, 192), "Dwarven Firebomb", "range", "Exotic", "", "", "1dmg1rng", "blast", "stun", 1, 0, 1, 2, -1, -1));
                cardDeck.Add(new Card(new Rectangle(384, 384, 128, 192), "Elven Cloak", "armor", "Cloak", "replaceDef>Awareness", "", "", "", "", 0, -1, -1, -1, -1, -1));
                cardDeck.Add(new Card(new Rectangle(512, 384, 128, 192), "Grinding Axe", "melee", "Axe", "", "", "1dmg", "5dmg", "", 2, 0, 1, 1, -1, -1));
                cardDeck.Add(new Card(new Rectangle(640, 384, 128, 192), "Heavy Steel Shield", "shield", "Shield", "reroll1+1def", "", "", "", "", 0, -1, -1, -1, -1, -1));
                cardDeck.Add(new Card(new Rectangle(768, 384, 128, 192), "Ice Storm", "range", "Magic, Rune", "", "", "imm", "2dmg", "", 2, 0, 2, 2, -1, -1));
                cardDeck.Add(new Card(new Rectangle(896, 384, 128, 192), "Iron-Bound Ring", "trinket", "Ring", "2hp", "1black1def", "", "", "", 0, -1, -1, -1, -1, -1));
                cardDeck.Add(new Card(new Rectangle(1024, 384, 128, 192), "Latari Longbow", "range", "Bow", "pierce1", "", "2range", "2dmg", "", 2, 0, 2, 2, -1, -1));
                cardDeck.Add(new Card(new Rectangle(1152, 384, 128, 192), "Lightning Strike", "range", "Magic, Rune", "", "", "blast", "2dmg", "2dmg", 2, 0, 2, 2, -1, -1));
                cardDeck.Add(new Card(new Rectangle(0, 576, 128, 192), "Mace of Kellos", "melee", "Hammer", "", "", "adj1", "recover1", "", 1, 0, 1, 2, -1, -1));
                cardDeck.Add(new Card(new Rectangle(128, 576, 128, 192), "Platemail", "armor", "Heavy Armor", "Can't equip Runes", "Speed <= 3", "", "", "", 0, -1, -1, -1, 5, -1));
                cardDeck.Add(new Card(new Rectangle(256, 576, 128, 192), "Steel Greatsword", "melee", "Blade", "reroll1powerdie", "", "pierce2", "1dmg", "", 2, 0, 1, 2, -1, -1));
                cardDeck.Add(new Card(new Rectangle(384, 576, 128, 192), "Tival Crystal", "trinket", "Trinket", "action:exhaustroll1red", "", "", "", "", 0, -1, -1, -1, -1, -1));
            }
		}

		#endregion

		#region Properties

		public List<Card> DiscardDeck { get { return discardDeck; } }
		public List<Card> CardDeck { get { return cardDeck; } }
		public bool Empty { get { return cardDeck.Count == 0; } }

		#endregion

		#region Public Methods

		/// <summary>
		/// Shuffles the deck at hand
		/// </summary>
		/// <param name="rand">The random number generator</param>
		public void Shuffle(Random rand)
		{
			for (int i = cardDeck.Count - 1; i > 0; i--)
			{
				int randomIndex = rand.Next(i + 1);
				Card tempCard = cardDeck[i];
				cardDeck[i] = cardDeck[randomIndex];
				cardDeck[randomIndex] = tempCard;
			}
		}

        /// <summary>
        /// Pulls a search card from the search deck
        /// </summary>
        /// <param name="searchingHero">Hero that performed the search</param>
        /// <param name="act">Current act of the game, used only when the treasure chest is found</param>
		public void pullSearchCard(HeroSheet searchingHero)
		{
			discardDeck.Add(cardDeck[0]);
			discardDeck[0].Active = true;
            if (!(cardDeck[0].Name == "Nothing")) { searchingHero.PickedClass.AddSearchCard(cardDeck[0]); }
			cardDeck.RemoveAt(0);			
		}

		public void pullPerilCard(Random rand)
		{
			int randomIndex = rand.Next(cardDeck.Count + 1);
		}

        public Card pullShopCard(HeroSheet currHero)
        {
            if (cardDeck.Count > 0)
            {
                Card tempCard = cardDeck[0];
                cardDeck.RemoveAt(0);
                return tempCard;
            }
            else return null;
        }

        /// <summary>
        /// Puts a card back into the deck. Primarily used for Shop 1/2 decks
        /// </summary>
        /// <param name="cardBack">Card being returned to the deck</param>
        public void putCardBack(Card cardBack) { cardDeck.Add(cardBack); }

		#endregion

		#region Private Methods
		#endregion
	}
}
