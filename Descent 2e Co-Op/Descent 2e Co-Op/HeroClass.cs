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
    public class HeroClass
    {
        #region Fields

        //Texture2D startingSkill1, startingskill2;
        Rectangle[] skillSpriteSourceRect = new Rectangle[8];
        int currentExp = 0, numOfTrinketsUsed = 0;
        string pickedClass;
		Rectangle currentWeaponRect, offHandRect, armorRect, trinket1Rect, trinket2Rect;
        List<Token> allSkillCards = new List<Token>();
        List<Token> skillList = new List<Token>();

        Equipment currentWeapon, offHand, armor, trinket1, trinket2;
        List<Equipment> backPack = new List<Equipment>();
		List<Card> searchCards = new List<Card>();
		Dice mainDie1, mainDie2, mainDie3, offDie1, offDie2, offDie3;
		int mainAttackRange = 0, offAttackRange = 0;


        /// <summary>
        /// SkillOwned represents the bought skills for the picked class. Represents the skills 1-8 as 0-7 in the array 
        /// </summary>
        bool[] skillsOwned = { false, false, false, false, false, false, false, false };

        bool[] skillsExhausted = { false, false, false, false, false, false, false, false, false };

        #endregion

        #region Constructors

        public HeroClass(string pickedClass)
        {
            loadStartingGear(pickedClass);
            mainDie1 = new Dice(currentWeapon.Dice1);
            mainDie2 = new Dice(currentWeapon.Dice2);
            mainDie3 = new Dice(currentWeapon.Dice3);
            this.pickedClass = pickedClass;
			string weaponType = CurrentWeapon.Type;
			if (weaponType == "Melee")
			{
				if (CurrentWeapon.Bonus1 == "reach") mainAttackRange = 2;
				else mainAttackRange = 1;
			}
			else
			{
				if (CurrentWeapon.Dice2 == 1) mainAttackRange = 6;
				else mainAttackRange = 8;
			}
        }

        #endregion

        #region Properties

        public bool[] SkillsExhausted { get { return skillsExhausted; } set { skillsExhausted = value; } }
        public bool[] SkillsOwned { get { return skillsOwned; } set { skillsOwned = value; } }
		public int MainAttackRange { get { return mainAttackRange; } set { mainAttackRange = value; } }
		public int OffAttackRange { get { return offAttackRange; } set { offAttackRange = value; } }
        public Rectangle[] SkillSprites { get { return skillSpriteSourceRect; } set { skillSpriteSourceRect = value; } }
		public Rectangle CurrentWeaponRect { get { return currentWeaponRect; } }
		public Rectangle OffHandRect { get { return offHandRect; } }
		public Rectangle ArmorRect { get { return armorRect; } }
		public Rectangle Trinket1Rect { get { return trinket1Rect; } }
		public Rectangle Trinket2Rect { get { return trinket2Rect; } }
		public string ClassName { get { return pickedClass; } }
		public Dice MainDie1 { get { return mainDie1; } }
		public Dice MainDie2 { get { return mainDie2; } }
		public Dice MainDie3 { get { return mainDie3; } }
		public Dice OffDie1 { get { return offDie1; } }
		public Dice OffDie2 { get { return offDie2; } }
		public Dice OffDie3 { get { return offDie3; } }
		public Equipment CurrentWeapon { get { return currentWeapon; } set { currentWeapon = value; } }
		public Equipment OffHand { get { return offHand; } set { offHand = value; } }
		public Equipment Armor { get { return armor; } set { armor = value; } }
		public Equipment Trinket1 { get { return trinket1; } set { trinket1 = value; } }
		public Equipment Trinket2 { get { return trinket2; } set { trinket2 = value; } }
		public List<Equipment> BackPack { get { return backPack; } }
		public List<Card> SearchCards { get { return searchCards; } }
        public List<Token> SkillList { get { return skillList; } }
        public List<Token> AllSkillCards { get { return allSkillCards; } }
        public int CurrentExp { get { return currentExp; } set { currentExp += value; } }

        #endregion

        #region Public Methods

		public void AddSearchCard(Card searchCard)
		{
			searchCards.Add(searchCard);
		}

        public void AddToBackPack(Card shopCard)
        {
            if (shopCard.Type == "melee" || shopCard.Type == "range")
            {
                backPack.Add(new Equipment(shopCard.SourceRectangle, shopCard.Name, shopCard.Type, shopCard.Trait, "", shopCard.Bonus1, shopCard.Surge1, shopCard.Surge2, shopCard.Surge3, 0, shopCard.NumOfHands,
                    shopCard.Attack1, shopCard.Attack2, shopCard.Attack3));
                if (shopCard.Bonus2 != "") backPack[backPack.Count].Bonus2 = shopCard.Bonus2;
            }
            else if (shopCard.Type == "armor")
            {
                backPack.Add(new Equipment(shopCard.SourceRectangle, shopCard.Name, shopCard.Trait, shopCard.Bonus1, shopCard.Bonus2, shopCard.Defense1));
            }
            else if (shopCard.Type == "trinket")
            {
                backPack.Add(new Equipment(shopCard.SourceRectangle, shopCard.Name, shopCard.Bonus1, shopCard.Bonus2));
            }
            else if (shopCard.Type == "shield")
            {
                backPack.Add(new Equipment(shopCard.SourceRectangle, shopCard.Name, shopCard.Bonus1));
            }
        }

		public void AddNewWeapon(ContentManager content, Equipment weaponCard, string handPosition)
		{
			if (handPosition == "main")
			{
				backPack.Add(currentWeapon);
				currentWeapon = weaponCard;
				mainDie1 = new Dice(currentWeapon.Dice1); mainDie2 = new Dice(currentWeapon.Dice2); mainDie3 = new Dice(currentWeapon.Dice3);
			}
			else
			{
				if (offHand != null)
				{
					backPack.Add(offHand);
					offHand = weaponCard;
					offDie1 = new Dice(currentWeapon.Dice1); offDie2 = new Dice(currentWeapon.Dice2); offDie3 = new Dice(currentWeapon.Dice3);
				}
			}
		}

        public void DrawSkillEquipment(SpriteBatch spriteBatch, Texture2D spriteSheet)
        {   
            int skillEquipYPos = GameConstants.WINDOW_HEIGHT - skillSpriteSourceRect[0].Height;
            currentWeaponRect = new Rectangle(GameConstants.HAND_1_LOC_X, skillEquipYPos, 128, 192);
            offHandRect = new Rectangle(GameConstants.HAND_1_LOC_X + GameConstants.EQUIPMENT_BUFFER_X, skillEquipYPos, 128, 192);
            armorRect = new Rectangle(GameConstants.HAND_1_LOC_X + (GameConstants.EQUIPMENT_BUFFER_X * 2), skillEquipYPos, 128, 192);
            trinket1Rect = new Rectangle(GameConstants.HAND_1_LOC_X + (GameConstants.EQUIPMENT_BUFFER_X * 3), skillEquipYPos, 128, 192);
            trinket2Rect = new Rectangle(GameConstants.HAND_1_LOC_X + (GameConstants.EQUIPMENT_BUFFER_X * 4), skillEquipYPos, 128, 192);
            int skillCount = CountSkillsOwned();
            foreach (Token skill in skillList) skill.Draw(spriteBatch, spriteSheet);
            spriteBatch.Draw(spriteSheet, currentWeaponRect, currentWeapon.EquipmentSpriteRect, Color.White);
            if (offHand != null) spriteBatch.Draw(spriteSheet, offHandRect, offHand.EquipmentSpriteRect, Color.White);
            if (armor != null) spriteBatch.Draw(spriteSheet, armorRect, armor.EquipmentSpriteRect, Color.White);
            if (trinket1 != null) spriteBatch.Draw(spriteSheet, trinket1Rect, trinket1.EquipmentSpriteRect, Color.White);
            if (trinket2 != null) spriteBatch.Draw(spriteSheet, trinket2Rect, trinket2.EquipmentSpriteRect, Color.White);
        }

        #endregion 

        #region Private Methods

        private int CountSkillsOwned()
        {
            int count = 0;
            for (int x = 0; x < skillsOwned.Length; x++) if (skillsOwned[x] == true) count++;
            return count;
        }

        /// <summary>
        /// Gets the starting weapon sprite
        /// </summary>
        /// <param name="content">A string to determin the chosen class</param>
        private void loadStartingGear(string pickedClass)
        {
            Rectangle startingSkillRect = new Rectangle(0, 0, GameConstants.WINDOW_WIDTH, GameConstants.WINDOW_HEIGHT);
            int xLoc = GameConstants.MAIN_SKILL_LOC_X, yLoc = GameConstants.WINDOW_HEIGHT - 192,
                xPos = 0, yPos = 0, counter = 0;
            switch (pickedClass)
            {
                case "disciple": 
                    currentWeapon = GameConstants.DISCIPLE_WEAPON;
                    currentWeapon.EquipmentSpriteRect = new Rectangle(0, 0, 128, 192);
                    offHand = GameConstants.DISCIPLE_SHIELD;
                    offHand.EquipmentSpriteRect = new Rectangle(128, 0, 128, 192);
                    xPos = 256; yPos = 0;
                    for (int x = 1; x <= 8; x++) { skillSpriteSourceRect[x - 1] = new Rectangle(xPos, yPos, 128, 192); xPos += 128; }
                    skillList.Add(new Token(startingSkillRect, 0, xLoc, yLoc, new Rectangle(256, 0, 128, 192)));
                    break;
                case "spirit speaker": 
                    currentWeapon = GameConstants.SPIRIT_SPEAKER_WEAPON;
                    currentWeapon.EquipmentSpriteRect = new Rectangle(0, 192, 128, 192);
                    xPos = 128; yPos = 192;
                    for (int x = 1; x <= 8; x++) { skillSpriteSourceRect[x - 1] = new Rectangle(xPos, yPos, 128, 192); xPos += 128; }
                    skillList.Add(new Token(startingSkillRect, 0, xLoc, yLoc, new Rectangle(512, 192, 128, 192)));
                    break;
                case "necromancer": 
                    currentWeapon = GameConstants.NECROMANCER_WEAPON;
                    currentWeapon.EquipmentSpriteRect = new Rectangle(0, 384, 128, 192);
                    xPos = 384; yPos = 384;
                    for (int x = 1; x <= 8; x++) { skillSpriteSourceRect[x - 1] = new Rectangle(xPos, yPos, 128, 192); xPos += 128; }
                    skillList.Add(new Token(startingSkillRect, 0, xLoc, yLoc, new Rectangle(768, 384, 128, 192)));
                    skillList.Add(new Token(startingSkillRect, -1, xLoc + GameConstants.MAIN_SKILL_BUFFER_X, yLoc, new Rectangle(256, 384, 128, 192)));
                    break;
                case "runemaster": 
                    currentWeapon = GameConstants.RUNEMASTER_WEAPON;
                    currentWeapon.EquipmentSpriteRect = new Rectangle(0, 576, 128, 192);
                    xPos = 128; yPos = 576;
                    for (int x = 1; x <= 8; x++) { skillSpriteSourceRect[x - 1] = new Rectangle(xPos, yPos, 128, 192); xPos += 128; }
                    skillList.Add(new Token(startingSkillRect, 0, xLoc, yLoc, new Rectangle(128, 0, 128, 192)));
                    break;
                case "thief": 
                    currentWeapon = GameConstants.THIEF_WEAPON;
                    currentWeapon.EquipmentSpriteRect = new Rectangle(0, 0, 128, 192);
                    trinket1 = GameConstants.THIEF_TRINKET;
                    trinket1.EquipmentSpriteRect = new Rectangle(128, 0, 128, 192);
                    numOfTrinketsUsed++;
                    xPos = 256; yPos = 0;
                    for (int x = 1; x <= 8; x++) { skillSpriteSourceRect[x - 1] = new Rectangle(xPos, yPos, 128, 192); xPos += 128; }
                    skillList.Add(new Token(startingSkillRect, 0, xLoc, yLoc, new Rectangle(512, 192, 128, 192)));
                    break;
                case "wildlander": 
                    currentWeapon = GameConstants.WILDLANDER_WEAPON;
                    currentWeapon.EquipmentSpriteRect = new Rectangle(0, 192, 128, 192);
                    xPos = 128; yPos = 192;
                    for (int x = 1; x <= 8; x++) { skillSpriteSourceRect[x - 1] = new Rectangle(xPos, yPos, 128, 192); xPos += 128; }
                    skillList.Add(new Token(startingSkillRect, 0, xLoc, yLoc, new Rectangle(768, 384, 128, 192)));
                    break;
                case "berserker": 
                    currentWeapon = GameConstants.BERSERKER_WEAPON;
                    currentWeapon.EquipmentSpriteRect = new Rectangle(0, 384, 128, 192);
                    xPos = 128; yPos = 384;
                    for (int x = 1; x <= 8; x++) { skillSpriteSourceRect[x - 1] = new Rectangle(xPos, yPos, 128, 192); xPos += 128; }
                    skillList.Add(new Token(startingSkillRect, 0, xLoc, yLoc, new Rectangle(128, 0, 128, 192)));
                    break;
                case "knight": 
                    currentWeapon = GameConstants.KNIGHT_WEAPON;
                    currentWeapon.EquipmentSpriteRect = new Rectangle(0, 576, 128, 192);
                    offHand = GameConstants.KNIGHT_SHIELD;
                    offHand.EquipmentSpriteRect = new Rectangle(128, 576, 128, 192);
                    xPos = 256; yPos = 576;
                    for (int x = 1; x <= 8; x++) { skillSpriteSourceRect[x - 1] = new Rectangle(xPos, yPos, 128, 192); xPos += 128; }
                    skillList.Add(new Token(startingSkillRect, 0, xLoc, yLoc, new Rectangle(512, 192, 128, 192)));
                    break;
                default: break;
            }
            for (int y = 0; y < 2; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    allSkillCards.Add(new Token(new Rectangle(GameConstants.HALF_WINDOW_WIDTH() - 280 + (140 * x), GameConstants.HALF_WINDOW_HEIGHT() - 200 + (200 * y), 128, 192), counter+1, 0, 0, skillSpriteSourceRect[counter]));
                    counter++;
                }
            }
            foreach (Token skillCard in allSkillCards) skillCard.Active = false;
        }

        #endregion

    }
}
