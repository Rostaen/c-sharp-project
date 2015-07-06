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
                for (int x = 0; x < 3; x++) { cardDeck.Add(new Card(content, "Search Deck/Health Potion", "Health Potion", 25)); }
                for (int x = 0; x < 3; x++) { cardDeck.Add(new Card(content, "Search Deck/Stamina Potion", "Stamina Potion", 25)); }
                cardDeck.Add(new Card(content, "Search Deck/Curse Doll", "Curse Doll", 50));
                cardDeck.Add(new Card(content, "Search Deck/Fire Flask", "Fire Flask", 50));
                cardDeck.Add(new Card(content, "Search Deck/Nothing", "Nothing", 0));
                cardDeck.Add(new Card(content, "Search Deck/Power Potion", "Power Potion", 50));
                cardDeck.Add(new Card(content, "Search Deck/Treasure Chest", "Treasure Chest", 0));
                cardDeck.Add(new Card(content, "Search Deck/Warding Talisman", "Warding Talisman", 50));
            }
            else if (type == "Peril")
            {

                cardDeck.Add(new Card(content, "Peril Deck/" + adventureName + "/Peril 1", false, GameConstants.EMPTY_FLAVOR_01, GameConstants.EMPTY_COND_01, "", ""));
                cardDeck.Add(new Card(content, "Peril Deck/" + adventureName + "/Peril 2", false, GameConstants.EMPTY_FLAVOR_02, GameConstants.EMPTY_COND_02, "", ""));
                cardDeck.Add(new Card(content, "Peril Deck/" + adventureName + "/Peril 3", true, GameConstants.EMPTY_FLAVOR_03, GameConstants.EMPTY_COND_03, GameConstants.MON_FLAVOR_03, GameConstants.MON_COND_03));
                cardDeck.Add(new Card(content, "Peril Deck/" + adventureName + "/Peril 4", true, GameConstants.EMPTY_FLAVOR_04, GameConstants.EMPTY_COND_04, GameConstants.MON_FLAVOR_04, GameConstants.MON_COND_04));
                cardDeck.Add(new Card(content, "Peril Deck/" + adventureName + "/Peril 5", true, GameConstants.EMPTY_FLAVOR_05, GameConstants.EMPTY_COND_05, GameConstants.MON_FLAVOR_05, GameConstants.MON_COND_05));
                cardDeck.Add(new Card(content, "Peril Deck/" + adventureName + "/Peril 6", true, GameConstants.EMPTY_FLAVOR_06, GameConstants.EMPTY_COND_06, GameConstants.MON_FLAVOR_06, GameConstants.MON_COND_06));
                cardDeck.Add(new Card(content, "Peril Deck/" + adventureName + "/Peril 7", true, GameConstants.EMPTY_FLAVOR_07, GameConstants.EMPTY_COND_07, GameConstants.MON_FLAVOR_07, GameConstants.MON_COND_07));
                cardDeck.Add(new Card(content, "Peril Deck/" + adventureName + "/Peril 8", false, GameConstants.EMPTY_FLAVOR_08, GameConstants.EMPTY_COND_08, "", ""));
                cardDeck.Add(new Card(content, "Peril Deck/" + adventureName + "/Peril 9", true, GameConstants.EMPTY_FLAVOR_09, GameConstants.EMPTY_COND_09, GameConstants.MON_FLAVOR_09, GameConstants.MON_COND_09));
                cardDeck.Add(new Card(content, "Peril Deck/" + adventureName + "/Peril 10", true, GameConstants.EMPTY_FLAVOR_10, GameConstants.EMPTY_COND_10, GameConstants.MON_FLAVOR_10, GameConstants.MON_COND_10));
            }
            else if (type == "Shop 1")
            {
                cardDeck.Add(new Card(content, "Shop 1/Chainmail", "Chainmail", "armor", "Heavy Armor", "Can't equip Runes", "Speed <= 4", "", "", "", 0, -1, -1, -1, 4, -1));
                cardDeck.Add(new Card(content, "Shop 1/Crossbow", "Crossbow", "range", "Bow, Exotic", "pierce1", "", "2dmg", "1dmg1push", "", 1, 0, 2, -1, -1, -1));
                cardDeck.Add(new Card(content, "Shop 1/Elm Greatbow", "Elm Greatbow", "range", "Bow", "Adjacent not block LoS", "", "2dmg", "2range", "", 2, 0, 2, -1, -1, -1));
                cardDeck.Add(new Card(content, "Shop 1/Heavy Cloak", "Heavy Cloak", "armor", "Cloak", "Cancel 1 surge", "", "", "", "", 0, -1, -1, -1, -1, -1));
                cardDeck.Add(new Card(content, "Shop 1/Immolation", "Immolation", "range", "Magic, Rune", "pierce1", "", "1dmg", "1range", "", 2, 0, 1, -1, -1, -1));
                cardDeck.Add(new Card(content, "Shop 1/Iron Battleaxe", "Iron Battleaxe", "melee", "Axe", "pierce1", "", "2dmg", "pierce1", "", 2, 0, 1, -1, -1, -1));
                cardDeck.Add(new Card(content, "Shop 1/Iron Shield 1", "Iron Shield", "off hand", "Shield", "reroll1or1def", "", "", "", "", 1, -1, -1, -1, -1, -1));
                cardDeck.Add(new Card(content, "Shop 1/Iron Shield 2", "Iron Shield", "off hand", "Shield", "reroll1or1def", "", "", "", "", 1, -1, -1, -1, -1, -1));
                cardDeck.Add(new Card(content, "Shop 1/Iron Spear", "Iron Spear", "melee", "Exotic", "reach", "", "1dmg", "pierce1", "", 1, 0, 2, -1, -1, -1));
                cardDeck.Add(new Card(content, "Shop 1/Leather Armor 1", "Leather Armor", "armor", "Light Armor", "+1hp", "", "", "", "", 0, -1, -1, -1, -1, -1));
                cardDeck.Add(new Card(content, "Shop 1/Leather Armor 2", "Leather Armor", "armor", "Light Armor", "+1hp", "", "", "", "", 0, -1, -1, -1, -1, -1));
                cardDeck.Add(new Card(content, "Shop 1/Light Hammer", "Light Hammer", "melee", "Hammer", "", "", "2dmg", "stun", "", 1, 0, 2, -1, -1, -1));
                cardDeck.Add(new Card(content, "Shop 1/Lucky Charm", "Lucky Charm", "trinket", "Trinket", "rerollAttribute", "", "", "", "", 0, -1, -1, -1, -1, -1));
                cardDeck.Add(new Card(content, "Shop 1/Magic Staff", "Magic Staff", "range", "Magic, Staff", "", "", "1dmg>1-3sp", "1range", "", 2, 0, 1, -1, -1, -1));
                cardDeck.Add(new Card(content, "Shop 1/Mana Weave", "Mana Weave", "trinket", "Rune", "1surge", "", "", "", "", 0, -1, -1, -1, -1, -1));
                cardDeck.Add(new Card(content, "Shop 1/Ring of Power", "Ring of Power", "trinket", "Ring", "1sta", "", "", "", "", 0, -1, -1, -1, -1, -1));
                cardDeck.Add(new Card(content, "Shop 1/Scorpion Helm", "Scorpion Helm", "trinket", "Helmet", "1range", "limit1", "", "", "", 0, -1, -1, -1, -1, -1));
                cardDeck.Add(new Card(content, "Shop 1/Sling", "Sling", "range", "Exotic", "", "", "1dmg1rng", "stun", "", 1, 0, 2, -1, -1, -1));
                cardDeck.Add(new Card(content, "Shop 1/Steel Broadsword", "Steel Broadsword", "melee", "Blade", "rerollred", "", "1dmg", "", "", 1, 0, 1, -1, -1, -1));
                cardDeck.Add(new Card(content, "Shop 1/Sunburst", "Sunburst", "range", "Magic, Rune", "", "", "stun", "2dmg", "", 2, 0, 2, -1, -1, -1));
            }
            else if (type == "Shop 2")
            {
                // TODO: Finish implementing Shop 2 deck then add code to award a shop item when enough monsters are defeated
                cardDeck.Add(new Card(content, "Shop 2/Demonhide Leather", "Demonhide Leather", "armor", "Light Armor", "2move-1sta", "", "", "", "", 0, -1, -1, -1, 4, -1));
                cardDeck.Add(new Card(content, "Shop 2/Dragontooth Hammer", "Dragontooth Hammer", "melee", "Hammer", "pierce1", "emptyOff1dmg", "pierce2", "", "", 1, 0, 1, 1, -1, -1));
                cardDeck.Add(new Card(content, "Shop 2/Dwarven Firebomb", "Dwarven Firebomb", "range", "Exotic", "", "", "1dmg1rng", "blast", "stun", 1, 0, 1, 2, -1, -1));
                cardDeck.Add(new Card(content, "Shop 2/Elven Cloak", "Elven Cloak", "armor", "Cloak", "replaceDef>Awareness", "", "", "", "", 0, -1, -1, -1, -1, -1));
                cardDeck.Add(new Card(content, "Shop 2/Grinding Axe", "Grinding Axe", "melee", "Axe", "", "", "1dmg", "5dmg", "", 2, 0, 1, 1, -1, -1));
                cardDeck.Add(new Card(content, "Shop 2/Heavy Steel Shield", "Heavy Steel Shield", "off hand", "Shield", "reroll1+1def", "", "", "", "", 0, -1, -1, -1, -1, -1));
                cardDeck.Add(new Card(content, "Shop 2/Ice Storm", "Ice Storm", "range", "Magic, Rune", "", "", "imm", "2dmg", "", 2, 0, 2, 2, -1, -1));
                cardDeck.Add(new Card(content, "Shop 2/Iron Bound Ring", "Iron-Bound Ring", "trinket", "Ring", "2hp", "1black1def", "", "", "", 0, -1, -1, -1, -1, -1));
                cardDeck.Add(new Card(content, "Shop 2/Latari Longbow", "Latari Longbow", "range", "Bow", "pierce1", "", "2range", "2dmg", "", 2, 0, 2, 2, -1, -1));
                cardDeck.Add(new Card(content, "Shop 2/Lightning Strike", "Lightning Strike", "range", "Magic, Rune", "", "", "blast", "2dmg", "2dmg", 2, 0, 2, 2, -1, -1));
                cardDeck.Add(new Card(content, "Shop 2/Mace of Kellos", "Mace of Kellos", "melee", "Hammer", "", "", "adj1", "recover1", "", 1, 0, 1, 2, -1, -1));
                cardDeck.Add(new Card(content, "Shop 2/Platemail", "Platemail", "armor", "Heavy Armor", "Can't equip Runes", "Speed <= 3", "", "", "", 0, -1, -1, -1, 5, -1));
                cardDeck.Add(new Card(content, "Shop 2/Steel Greatsword", "Steel Greatsword", "melee", "Blade", "reroll1powerdie", "", "pierce2", "1dmg", "", 2, 0, 1, 2, -1, -1));
                cardDeck.Add(new Card(content, "Shop 2/Tival Crystal", "Tival Crystal", "trinket", "Trinket", "action:exhaustroll1red", "", "", "", "", 0, -1, -1, -1, -1, -1));
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

		public void drawSearchCard(Random rand, HeroSheet searchingHero)
		{
			int randomIndex = rand.Next(0, cardDeck.Count);
			discardDeck.Add(cardDeck[randomIndex]);
			int discardSize = discardDeck.Count - 1;
			discardDeck[discardSize].Active = true;
			if (!(cardDeck[randomIndex].Name == "Nothing")) { searchingHero.PickedClass.AddSearchCard(cardDeck[randomIndex]); }
			cardDeck.RemoveAt(randomIndex);			
		}

		public void drawPerilCard(Random rand)
		{
			int randomIndex = rand.Next(cardDeck.Count + 1);
		}



		#endregion

		#region Private Methods
		#endregion
	}
}
