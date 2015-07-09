using System;
using System.Collections.Generic;
//using System.Windows;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Descent_2e_Co_Op
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region Fields
        
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public SpriteFont windlassFont6;
        public SpriteFont windlassFont14;
        public SpriteFont windlassFont23;
        public SpriteFont windlassFont36;
        SpriteFont Arial14;

        // Starting Game States
        static GameState currentGameState = GameState.CharacterCreation;
        static HeroState currentHeroState = HeroState.SelectHero;
        static HeroActionState currentHeroActionState = HeroActionState.ChooseAction;
        static OverlordState currentOLState = OverlordState.ActiveEffect;
        
        // For fading purposes
        int mAlphaValue = 1, mFadeIncrement = 3;
        double mFadeDelay = 0.015;

        // Sprite Sheets
        Texture2D spriteSheet1, classSheet1, classSheet2, heroSheet1, heroSheet2, shopSheet;

        // HP/Stam bar items
        Rectangle barBGSource, hpBarSource, stamBarSource,
                  bar1BGLocation, bar2BGLocation, hpBarLocation, stamBarLocation;
        Token bar1BG, bar2BG;

        // List of all items being manipulated to play the game
        List<HeroSheet> heroSheets = new List<HeroSheet>();
        List<MonsterSheet> monsterSheet = new List<MonsterSheet>();
        List<Token> heroTokens = new List<Token>();
        List<Token> drawingTokens = new List<Token>();
        List<Token> monsterTokens = new List<Token>();
        List<Token> searchTokens = new List<Token>();
        List<Token> objectiveTokens = new List<Token>();
        List<Token> floorHighlights = new List<Token>();
        List<Token> yesNoList = new List<Token>();
        List<Token> surgeListRect = new List<Token>();
        List<Token> hpBars = new List<Token>();
        List<Token> stamBars = new List<Token>();
        List<Tile> tiles = new List<Tile>();
        List<Tile> doors = new List<Tile>();
        List<Tile> endCaps = new List<Tile>();
        List<Message> messages = new List<Message>();
		List<Dice> attackDiceList = new List<Dice>();
		List<Dice> defenseDiceList = new List<Dice>();
        List<string> surgeList = new List<string>();
        List<Token> testing = new List<Token>();
        List<Card> awardedShopCards = new List<Card>();
        Deck searchDeck, perilDeck, shop1Deck, shop2Deck, roomDeck;
        //monsterActivationDeck, 

        List<Token> dots = new List<Token>();

        // Random number generator for dice rolls
        Random random = new Random();

        // Various Items
		bool heroTokenClicked = false, monsterTokenClicked = false, leftClickStarted = false, leftButtonReleased = true,
			 barghestFound = false, fleshFound = false, dragonFound = false, zombieFound = false, finishedSelectingHeroes = false, 
			 createdStep1 = false, loadOnce = false, loadSurgeOnce = false, heroMoving = false, searchTokenClicked = false, searchedOnce = false, weaponPicked = false,
			 calcAttack = true, attackHit = true, hasSurges = false, skillPicked = false, familiarActive = false, familiarActed = false, familiarAttacked = false, familiarMoved = false, 
             familiarChoosing = true, familiarActionSheetOn = true, usedSkillOnce = false, LoSFound = false, skillNotExhausted = false, awardingLoot = false;

        string currentRoom = "The Onset", selectedHeroName = "", weaponUsed = "", familiarAction ="", theClassName = "";
        int numHeroTurns = 2, numHeroesPlaying = 2, creatingHeroNumber = 1, currentHeroTurn = 1, numOfHealers = 0, numOfMages = 0, numOfScouts = 0, numOfWarriors = 0, attackRange = 0, heroNumPosition = -1,
            skillUsed = -1, timer = 0, heroPlacementCount = 0, buySkill = 0, loadTokensSheets = 0, masterKillCount = 1;

        Card awardedCardHold;

        Equipment attackingWeapon;

        // Message and other window items
        Vector2 centerWindowMessage = new Vector2(GameConstants.HALF_WINDOW_WIDTH(), GameConstants.WINDOW_HEIGHT * 0.25f);

        // Current act of the game, starts in Act 1
        int currentAct = 1;

        // Private variables
        private Texture2D background;
        private FillBG bgFill = new FillBG();
        private OverlayBG monsterOverlay;

        // Loot/Overlord Track Items
        private Token masterMonsterKillToken;
        private Tracks lootTrack, overlordTrack;
        List<Token> lootTrackTokens = new List<Token>(), overlordTokens = new List<Token>(), lootOLButtons = new List<Token>();
        List<Message> lootOLTrackMessages = new List<Message>();
        Rectangle staminaTokenSource, hpTokenSource;

        // Starting game creation
        private Texture2D creationBG;
        private Rectangle creationRec;
        private int creationStep = 1;
        List<Token> numOfPlayerTokens = new List<Token>();
        List<Token> chooseArchetype = new List<Token>();
        List<Token> chooseHero = new List<Token>();
        List<Token> chooseClass = new List<Token>();
        List<int> chosenHeroArchtype = new List<int>(4); // 1 = Healer, 2 = Mage, 3 = Scout, 4 = Warrior
        List<int> chosenHeroName = new List<int>(4); // 1 = Ashrian, 2 = Avric, 3 = Leoric, 4 = Tarha, 5 = Jain, 6 = Tomble, 7 = Grisban, 8 = Syndreal
        List<int> chosenHeroClass = new List<int>(4); // 1 = Disciple, 2 = Spirit Speaker, 3 = Necromancer, 4 = Runemaster, 5 = Thief, 6 = Wildlander, 7 = Berserker, 8 = Knight

        // Hero Action Selection Items
        private Texture2D actionChoiceSprite;
        private Rectangle actionChoiceRect;
        private List<Message> actionMessages = new List<Message>();
        List<Token> actionButtons = new List<Token>();
        private int selectedActionNumber = -1;
        private bool choosingAction = true, zeroMovementPoints = false;
		List<Token> movementButtons = new List<Token>();
		MonsterSheet targetMonsterSheet;
		Token targetMonsterToken, endToken;
        Vector2 actionWindowTop, actionWindowBottom;

		// Dice temporary dice placeholders for various items
		Dice attackDice1, attackDice2, attackDice3, defenseDice1, defenseDice2;
        Dice familiarDie1, familiarDie2, familiarDie3;

		// For Attacks
		int totalRange = 0, totalAttack = 0, totalSurge = 0, totalDefense = 0, removeSurgeNumber = -1;
		
		#endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferHeight = GameConstants.WINDOW_HEIGHT;
            graphics.PreferredBackBufferWidth = GameConstants.WINDOW_WIDTH;
            //graphics.IsFullScreen = true;
            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Loading Fonts
            windlassFont6 = Content.Load<SpriteFont>("Fonts/Windlass 6");
            windlassFont14 = Content.Load<SpriteFont>("Fonts/Windlass 14");
            windlassFont23 = Content.Load<SpriteFont>("Fonts/Windlass 23");
            windlassFont36 = Content.Load<SpriteFont>("Fonts/Windlass 36");
            Arial14 = Content.Load<SpriteFont>("Fonts/Arial18");

            // Loading background image
            endToken = new Token(new Rectangle(0, 0, GameConstants.WINDOW_WIDTH, GameConstants.WINDOW_HEIGHT), 0, GameConstants.WINDOW_WIDTH - 200, 50, new Rectangle(768, 712, 150, 64));
            background = Content.Load<Texture2D>("Misc/dirt bg");
            spriteSheet1 = Content.Load<Texture2D>("sprite sheet 1"); classSheet1 = Content.Load<Texture2D>("Classes/class sheet 1"); classSheet2 = Content.Load<Texture2D>("Classes/class sheet 2");
            heroSheet1 = Content.Load<Texture2D>("Hero_Items/hero sheets 1"); heroSheet2 = Content.Load<Texture2D>("Hero_Items/hero sheets 2");
            shopSheet = Content.Load<Texture2D>("shop sheet");
            barBGSource = new Rectangle(0, 996, 246, 28); hpBarSource = new Rectangle(0, 972, 244, 24); stamBarSource = new Rectangle(0, 948, 244, 24);
            bar1BGLocation = new Rectangle(GameConstants.HALF_WINDOW_WIDTH() - 411, GameConstants.WINDOW_HEIGHT - 225, 246, 28);
            hpBarLocation = new Rectangle(GameConstants.HALF_WINDOW_WIDTH() - 407, GameConstants.WINDOW_HEIGHT - 223, 244, 24);
            bar2BGLocation = new Rectangle(GameConstants.HALF_WINDOW_WIDTH() + 165, GameConstants.WINDOW_HEIGHT - 225, 246, 28);
            stamBarLocation = new Rectangle(GameConstants.HALF_WINDOW_WIDTH() + 169, GameConstants.WINDOW_HEIGHT - 224, 244, 24);
            bar1BG = new Token(bar1BGLocation, 0, 0, 0, barBGSource); bar2BG = new Token(bar2BGLocation, 0, 0, 0, barBGSource);

            // Adding Yes/No buttons for familiar activation
            yesNoList.Add(new Token(new Rectangle(0, 0, GameConstants.WINDOW_WIDTH, GameConstants.WINDOW_HEIGHT), 0, GameConstants.WINDOW_WIDTH / 2 - 25, 240, new Rectangle(360, 352, 40, 40)));
            yesNoList.Add(new Token(new Rectangle(0, 0, GameConstants.WINDOW_WIDTH, GameConstants.WINDOW_HEIGHT), 1, GameConstants.WINDOW_WIDTH / 2 + 25, 240, new Rectangle(400, 352, 40, 40)));

            // Adding Loot track and OL track items to the game
            Texture2D lootSprite = Content.Load<Texture2D>("Misc/Loot Tracker");
            lootTrack = new Tracks(Content, lootSprite, 10, GameConstants.WINDOW_HEIGHT / 2 - lootSprite.Height / 2);
            // TODO: Set for testing gaining equipment, delete after done testing
            int xPosition = GameConstants.HP_TOKEN_START_X + lootTrack.DrawRectangle.Height, yPosition = GameConstants.HP_TOKEN_START_Y + lootTrack.DrawRectangle.Height;
            for (int x = 0; x < 3; x++) lootTrackTokens.Add(new Token(lootTrack.DrawRectangle, lootTrackTokens.Count + 1, GameConstants.HP_TOKEN_START_X, yPosition + (GameConstants.HP_TOKEN_BUFFER_Y * lootTrackTokens.Count), hpTokenSource));
            //
            staminaTokenSource = new Rectangle(721, 224, 43, 40); hpTokenSource = new Rectangle(721, 264, 40, 36);
            masterMonsterKillToken = new Token(lootTrack.DrawRectangle, 1, lootTrack.DrawRectangle.Width - 110, lootTrack.DrawRectangle.Height - 58, staminaTokenSource);
            Texture2D overlordSprite = Content.Load<Texture2D>("Misc/Overlord Tracker");
            overlordTrack = new Tracks(Content, overlordSprite, GameConstants.WINDOW_WIDTH / 2 - overlordSprite.Width / 2, 0);
            overlordTokens.Add(new Token(new Rectangle(640, 256, 40, 40),"doom", overlordTrack, overlordSprite.Width - 105, overlordSprite.Height - 80));
            overlordTokens.Add(new Token(new Rectangle(680, 256, 40, 40), "fate", overlordTrack, 42, overlordSprite.Height - 80));
            lootOLButtons.Add(new Token(new Rectangle(0, 0, GameConstants.WINDOW_WIDTH, GameConstants.WINDOW_HEIGHT), 0, 10, 10, new Rectangle(480, 352, 40, 40)));
            lootOLButtons.Add(new Token(new Rectangle(0, 0, GameConstants.WINDOW_WIDTH, GameConstants.WINDOW_HEIGHT), 1, 60, 10, new Rectangle(440, 352, 40, 40)));
            lootOLTrackMessages.Add(new Message("Loot Track", windlassFont14, new Vector2(60, 20), 0));
            lootOLTrackMessages.Add(new Message("Overlord Track", windlassFont14, new Vector2(200, 20), 1));

            // Adding the background, initial tiles/endcap/doors and monster overlay
            bgFill.Load(GraphicsDevice, background);
            tiles.Add(new Tile(Content,
                (GameConstants.WINDOW_WIDTH / 2) - (GameConstants.TILE_SIZE[9,0] / 2), (GameConstants.WINDOW_HEIGHT / 2) - (GameConstants.TILE_SIZE[9,1] / 2), GameConstants.TILE_SIZE[9,0], GameConstants.TILE_SIZE[9,1],
                GameConstants.TILE_ENT_EXIT_REC[9, 0], GameConstants.TILE_ENT_EXIT_REC[9, 1], GameConstants.TILE_ENT_EXIT_REC[9, 2],
                GameConstants.TILE_NAMES[9, 2], GameConstants.TILE_NAMES[9, 3], GameConstants.TILE_NAMES[9, 4],
                GameConstants.TILE_NAMES[9, 0], false, 0));
            endCaps.Add(new Tile(Content, tiles[0].X + GameConstants.TILE_SIZE[9, 0], tiles[0].Y, GameConstants.TILE_SIZE[9, 0], GameConstants.TILE_SIZE[9, 1], 
                GameConstants.TILE_ENT_EXIT_REC[9, 2], GameConstants.TILE_NAMES[9, 2], 0));
            tiles.Add(new Tile(Content, tiles[0].X - 64, tiles[0].Y + 64, 64, 128, new Vector2(64f, 64f), new Vector2(0f, 64f), new Vector2(), "E", "W", "", "Tile - Conn b", false, 1)); 
            doors.Add(new Tile(Content, "Door Yellow", false, tiles[1].Exit1, tiles[1].ExitSide1, tiles[1].X, tiles[1].Y, tiles[1].Height, tiles[1].Width));
            monsterOverlay = new OverlayBG(Content);

            // Setting up background for character creation
            creationBG = Content.Load<Texture2D>("Start Up Items/selection bg");
            creationRec = new Rectangle(GameConstants.HALF_WINDOW_WIDTH() - creationBG.Width / 2, GameConstants.HALF_WINDOW_HEIGHT() - creationBG.Height / 2, creationBG.Width, creationBG.Height);

            // loading archetype tokens
            chooseArchetype.Add(new Token(creationRec, 1, (int)(creationRec.Width * 0.2f), (int)(creationRec.Height * 0.55f) - 50, new Rectangle(512, 64, 64, 64)));
            chooseArchetype.Add(new Token(creationRec, 2, (int)(creationRec.Width * 0.4f), (int)(creationRec.Height * 0.55f) - 50, new Rectangle(576, 64, 64, 64)));
            chooseArchetype.Add(new Token(creationRec, 3, (int)(creationRec.Width * 0.6f), (int)(creationRec.Height * 0.55f) - 50, new Rectangle(640, 64, 64, 64)));
            chooseArchetype.Add(new Token(creationRec, 4, (int)(creationRec.Width * 0.8f), (int)(creationRec.Height * 0.55f) - 50, new Rectangle(704, 64, 64, 64)));

            // Setting up action menu
            actionChoiceSprite = Content.Load<Texture2D>("Misc/hero action paper");
            actionChoiceRect = new Rectangle(GameConstants.WINDOW_WIDTH - (actionChoiceSprite.Width + 30), 30, actionChoiceSprite.Width, actionChoiceSprite.Height);
            addActionMessageButton("Attack", "Hero Actions/attack", 0); addActionMessageButton("Search", "Hero Actions/search", 1); addActionMessageButton("Move", "Hero Actions/move", 2); 
            addActionMessageButton("Special", "Hero Actions/special", 3); addActionMessageButton("Rest", "Hero Actions/rest", 4); addActionMessageButton("Stand Up", "Hero Actions/stand", 5);
            addActionMessageButton("Perform ability/skill", "Hero Actions/ability skill", 6); addActionMessageButton("Open or close a door", "Hero Actions/door", 7); addActionMessageButton("Revive a hero", "Hero Actions/revive", 8);

            // Adding vector2 info for the action window top/bottom message locations
            actionWindowTop = new Vector2((float)(actionChoiceRect.X + (actionChoiceRect.Width / 2)), (float)(actionChoiceRect.Y + 60));
            actionWindowBottom = new Vector2((float)(actionChoiceRect.X + (actionChoiceRect.Width / 2)), (float)(actionChoiceRect.Y + actionChoiceRect.Height - 60));

			// Setting up movement buttons
			Rectangle menuButtonRect = new Rectangle(GameConstants.WINDOW_WIDTH - 300, 50, 150, 300);
			movementButtons.Add(new Token(menuButtonRect, 1, 0, 0, new Rectangle(768, 584, 150, 64)));
            movementButtons.Add(new Token(menuButtonRect, 2, 0, 69, new Rectangle(768, 648, 150, 64)));
            movementButtons.Add(new Token(menuButtonRect, 3, 0, 138, new Rectangle(768, 712, 150, 64)));

			// Setting up Decks
			searchDeck = new Deck("Search", "");
			searchDeck.Shuffle(random);
			perilDeck = new Deck("Peril", "Forgotten Souls");
			perilDeck.Shuffle(random);
            shop1Deck = new Deck("Shop 1", "");
            shop1Deck.Shuffle(random);
            shop2Deck = new Deck("Shop 2", "");
            shop2Deck.Shuffle(random);
            roomDeck = new Deck("Room", "Forgotten Souls"); // no shuffling of this deck, it is done upon construction

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyBoard = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyBoard.IsKeyDown(Keys.Escape))
                this.Exit();

            switch (currentGameState)
            {
                #region Game State: Character Creation
                case GameState.CharacterCreation:
                    #region Choosing number of players/heroes/classes
                    #region Step 1: Select Number of Heroes
                    if (creationStep == 1) // Choosing the number of heroes to play the game with
                    {
                        messages.Add(new Message("Choose The", windlassFont36, new Vector2(creationRec.X + creationRec.Width / 2, (int)(creationRec.Y + creationRec.Height * 0.25))));
                        messages.Add(new Message("Number Of Heroes", windlassFont36, new Vector2(creationRec.X + creationRec.Width / 2, (int)(creationRec.Y + creationRec.Height * 0.35))));
                        if (!createdStep1)
                        {
                            numOfPlayerTokens.Add(new Token(creationRec, 2, creationRec.Width / 4 - 50, (int)(creationRec.Height * 0.55f) - 50, new Rectangle(918, 584, 100, 100)));
                            numOfPlayerTokens.Add(new Token(creationRec, 3, creationRec.Width / 2 - 50, (int)(creationRec.Height * 0.55f) - 50, new Rectangle(918, 684, 100, 100)));
                            numOfPlayerTokens.Add(new Token(creationRec, 4, (int)(creationRec.Width * 0.75f - 50), (int)(creationRec.Height * 0.55f) - 50, new Rectangle(918, 784, 100, 100)));
                            createdStep1 = true;
                        }
                        if (mouse.LeftButton == ButtonState.Pressed && leftButtonReleased) { leftClickStarted = true; leftButtonReleased = false; }
                        else if (mouse.LeftButton == ButtonState.Released)
                        {
                            leftButtonReleased = true;
                            if (leftClickStarted)
                            {
                                leftClickStarted = false;
                                foreach (Token numPlayers in numOfPlayerTokens) { if (numPlayers.DrawRectangle.Contains(mouse.X, mouse.Y)) { numHeroesPlaying = numPlayers.Variable; creationStep = 2; messages.Clear(); } }
                                chosenHeroArchtype.Add(-1); chosenHeroArchtype.Add(-1);
                                chosenHeroName.Add(-1); chosenHeroName.Add(-1); 
                                chosenHeroClass.Add(-1); chosenHeroClass.Add(-1);
                                if (numHeroesPlaying >= 3) { chosenHeroArchtype.Add(-1); chosenHeroName.Add(-1); chosenHeroClass.Add(-1); }
                                if (numHeroesPlaying == 4) { chosenHeroArchtype.Add(-1); chosenHeroName.Add(-1); chosenHeroClass.Add(-1); }

                                checkVariousItems(mouse);
                            }
                        }
                    }
                    #endregion
                    #region Step 2: Choose the Archetype
                    else if (creationStep == 2) // Choosing the Archtype to play the game with, repeated min 2 times, max 4 times
                    {
                        messages.Add(new Message("Choose An Archetype", windlassFont36, new Vector2(creationRec.X + creationRec.Width / 2, (int)(creationRec.Y + creationRec.Height * 0.25))));
                        messages.Add(new Message("For Hero " + creatingHeroNumber, windlassFont36, new Vector2(creationRec.X + creationRec.Width / 2, (int)(creationRec.Y + creationRec.Height * 0.35))));
                        foreach (Token choseArchetype in chooseArchetype) choseArchetype.Active = true;
                        if (numOfHealers == 2) { chooseArchetype[0].Active = false; chooseArchetype[0].SourceRectangleY = 128; }
                        if (numOfMages == 2) { chooseArchetype[1].Active = false; chooseArchetype[1].SourceRectangleY = 128; }
                        if (numOfScouts == 2) { chooseArchetype[2].Active = false; chooseArchetype[2].SourceRectangleY = 128; }
                        if (numOfWarriors == 2) { chooseArchetype[3].Active = false; chooseArchetype[3].SourceRectangleY = 128; }
                        if (mouse.LeftButton == ButtonState.Pressed && leftButtonReleased) { leftClickStarted = true; leftButtonReleased = false; }
                        else if (mouse.LeftButton == ButtonState.Released)
                        {
                            leftButtonReleased = true;
                            if (leftClickStarted)
                            {
                                leftClickStarted = false;
                                foreach (Token chosenArchetype in chooseArchetype) { 
									if (chosenArchetype.DrawRectangle.Contains(mouse.X, mouse.Y) && chosenArchetype.Active) {
										if (chosenArchetype.Variable == 1) numOfHealers++;
										else if (chosenArchetype.Variable == 2) numOfMages++;
										else if (chosenArchetype.Variable == 3) numOfScouts++;
										else numOfWarriors++;
										chosenHeroArchtype[creatingHeroNumber - 1] = chosenArchetype.Variable; 
										creationStep = 3; 
										messages.Clear(); 
										foreach (Token choseArchetype in chooseArchetype) choseArchetype.Active = false; 
									} 
								}
                            }
                        }
                    }
                    #endregion
                    #region Step 3: Choose the Hero
                    else if (creationStep == 3) // Choosing the Hero for the given Archtype, repeated min 2 times, max 4 times
                    {
                        string archetypeName = GetArchetypeName();
						bool ashPicked = false, avPicked = false, leoPicked = false, tarPicked = false, jainPicked = false, tomPicked = false, grisPicked = false, synPicked = false;
                        messages.Add(new Message("Choose A " + archetypeName + " Hero", windlassFont36, new Vector2(creationRec.X + creationRec.Width / 2, (int)(creationRec.Y + creationRec.Height * 0.25))));
                        if (!loadOnce)
                        {
							for (int x = 0; x < chosenHeroName.Count; x++)
							{
								switch (chosenHeroName[x])
								{
									case 1: ashPicked = true; break;
									case 2: avPicked = true; break;
									case 3: leoPicked = true; break;
									case 4: tarPicked = true; break;
									case 5: jainPicked = true; break;
									case 6: tomPicked = true; break;
									case 7: grisPicked = true; break;
									case 8: synPicked = true; break;
									default: break;
								}
							}
                            if (archetypeName == "Healer")
                            {
                                if(!ashPicked) chooseHero.Add(new Token(creationRec, 1, (int)(creationRec.Width * 0.15f), (int)(creationRec.Height * 0.333f), new Rectangle(0, 0, 320, 256)));
								else chooseHero.Add(new Token(creationRec, 1, (int)(creationRec.Width * 0.15f), (int)(creationRec.Height * 0.333f), new Rectangle(320, 0, 320, 256)));
								if (!avPicked) chooseHero.Add(new Token(creationRec, 2, (int)(creationRec.Width * 0.55f), (int)(creationRec.Height * 0.333f), new Rectangle(0, 256, 320, 256)));
								else chooseHero.Add(new Token(creationRec, 2, (int)(creationRec.Width * 0.55f), (int)(creationRec.Height * 0.333f), new Rectangle(320, 256, 320, 256)));
                            }
                            else if (archetypeName == "Mage")
                            {
                                if(!leoPicked) chooseHero.Add(new Token(creationRec, 3, (int)(creationRec.Width * 0.15f), (int)(creationRec.Height * 0.333f), new Rectangle(0, 512, 320, 256)));
								else chooseHero.Add(new Token(creationRec, 3, (int)(creationRec.Width * 0.15f), (int)(creationRec.Height * 0.333f), new Rectangle(320, 512, 320, 256)));
                                if(!tarPicked) chooseHero.Add(new Token(creationRec, 4, (int)(creationRec.Width * 0.55f), (int)(creationRec.Height * 0.333f), new Rectangle(0, 768, 320, 256)));
								else chooseHero.Add(new Token(creationRec, 4, (int)(creationRec.Width * 0.55f), (int)(creationRec.Height * 0.333f), new Rectangle(320, 768, 320, 256)));
                            }
                            else if (archetypeName == "Scout")
                            {
                                if (!jainPicked) chooseHero.Add(new Token(creationRec, 5, (int)(creationRec.Width * 0.15f), (int)(creationRec.Height * 0.333f), new Rectangle(0, 0, 320, 256)));
                                else chooseHero.Add(new Token(creationRec, 5, (int)(creationRec.Width * 0.15f), (int)(creationRec.Height * 0.333f), new Rectangle(320, 0, 320, 256)));
                                if (!tomPicked) chooseHero.Add(new Token(creationRec, 6, (int)(creationRec.Width * 0.55f), (int)(creationRec.Height * 0.333f), new Rectangle(0, 256, 320, 256)));
                                else chooseHero.Add(new Token(creationRec, 6, (int)(creationRec.Width * 0.55f), (int)(creationRec.Height * 0.333f), new Rectangle(320, 256, 320, 256)));
                            }
                            else if (archetypeName == "Warrior")
                            {
                                if (!grisPicked) chooseHero.Add(new Token(creationRec, 7, (int)(creationRec.Width * 0.15f), (int)(creationRec.Height * 0.333f), new Rectangle(0, 512, 320, 256)));
                                else chooseHero.Add(new Token(creationRec, 7, (int)(creationRec.Width * 0.15f), (int)(creationRec.Height * 0.333f), new Rectangle(320, 512, 320, 256)));
                                if (!synPicked) chooseHero.Add(new Token(creationRec, 8, (int)(creationRec.Width * 0.55f), (int)(creationRec.Height * 0.333f), new Rectangle(0, 768, 320, 256)));
                                else chooseHero.Add(new Token(creationRec, 8, (int)(creationRec.Width * 0.55f), (int)(creationRec.Height * 0.333f), new Rectangle(320, 768, 320, 256)));
                            }
                            loadOnce = true;
							foreach (Token hSheet in chooseHero) { foreach (int chosenHero in chosenHeroName) if (chosenHero == hSheet.Variable) hSheet.Active = false; }
                        }
                        if (mouse.LeftButton == ButtonState.Pressed && leftButtonReleased) { leftClickStarted = true; leftButtonReleased = false; }
                        else if (mouse.LeftButton == ButtonState.Released)
                        {
                            leftButtonReleased = true;
                            if (leftClickStarted)
                            {
                                leftClickStarted = false;
                                foreach (Token chosenHero in chooseHero) { if (chosenHero.DrawRectangle.Contains(mouse.X, mouse.Y) && chosenHero.Active) { chosenHeroName[creatingHeroNumber - 1] = chosenHero.Variable; creationStep = 4; messages.Clear(); } }
                                foreach (Token choseHero in chooseHero) choseHero.Active = false;
                                chooseHero.Clear(); loadOnce = false; 
                            }
                        }
                    }
                    #endregion
                    #region Step 4: Choose the Class
                    else if (creationStep == 4)
                    {
                        string archetypeName = GetArchetypeName();
						bool ssPicked = false, dePicked = false, rmPicked = false, necPicked = false, thPicked = false, wlPicked = false, bsPicked = false, knPicked = false;
                        messages.Add(new Message("Choose A " + archetypeName + " Class", windlassFont36, new Vector2(creationRec.X + creationRec.Width / 2, (int)(creationRec.Y + creationRec.Height * 0.25))));
                        #region Hero Classes
                        if (!loadOnce)
                        {
							for (int x = 0; x < chosenHeroClass.Count; x++)
							{
								switch (chosenHeroClass[x])
								{
									case 1: ssPicked = true; break;
									case 2: dePicked = true; break;
									case 3: necPicked = true; break;
									case 4: rmPicked = true; break;
									case 5: thPicked = true; break;
									case 6: wlPicked = true; break;
									case 7: bsPicked = true; break;
									case 8: knPicked = true; break;
									default: break;
								}
							}
                            if (archetypeName == "Healer")
                            {
								if(!ssPicked) chooseClass.Add(new Token(creationRec, 1, (int)(creationRec.Width * 0.15f), (int)(creationRec.Height * 0.3), new Rectangle(512, 64, 64, 64)));
                                else chooseClass.Add(new Token(creationRec, 1, (int)(creationRec.Width * 0.15f), (int)(creationRec.Height * 0.3), new Rectangle(512, 128, 64, 64)));
                                if (!dePicked) chooseClass.Add(new Token(creationRec, 2, (int)(creationRec.Width * 0.11f), (int)(creationRec.Height * 0.5), new Rectangle(512, 64, 64, 64)));
                                else chooseClass.Add(new Token(creationRec, 2, (int)(creationRec.Width * 0.11f), (int)(creationRec.Height * 0.5), new Rectangle(512, 128, 64, 64)));
                                messages.Add(new Message("Disciple - <insert class definition here>", windlassFont14, new Vector2(GameConstants.HALF_WINDOW_WIDTH(), creationRec.Height / 2)));
                                messages.Add(new Message("Spirit Speaker - <insert class definition here>", windlassFont14, new Vector2(GameConstants.HALF_WINDOW_WIDTH(), creationRec.Height * 0.7f)));
                            }
                            else if (archetypeName == "Mage")
                            {
                                if (!necPicked) chooseClass.Add(new Token(creationRec, 3, (int)(creationRec.Width * 0.15f), (int)(creationRec.Height * 0.3), new Rectangle(576, 64, 64, 64)));
                                else chooseClass.Add(new Token(creationRec, 3, (int)(creationRec.Width * 0.15f), (int)(creationRec.Height * 0.3), new Rectangle(576, 128, 64, 64)));
                                if (!rmPicked) chooseClass.Add(new Token(creationRec, 4, (int)(creationRec.Width * 0.11f), (int)(creationRec.Height * 0.5), new Rectangle(576, 64, 64, 64)));
                                else chooseClass.Add(new Token(creationRec, 4, (int)(creationRec.Width * 0.11f), (int)(creationRec.Height * 0.5), new Rectangle(576, 128, 64, 64)));
                                messages.Add(new Message("Necromancer - <insert class definition here>", windlassFont14, new Vector2(GameConstants.HALF_WINDOW_WIDTH(), creationRec.Height / 2)));
                                messages.Add(new Message("Runemaster - <insert class definition here>", windlassFont14, new Vector2(GameConstants.HALF_WINDOW_WIDTH(), creationRec.Height * 0.7f)));
                            }
                            else if (archetypeName == "Scout")
                            {
                                if (!thPicked) chooseClass.Add(new Token(creationRec, 5, (int)(creationRec.Width * 0.15f), (int)(creationRec.Height * 0.3), new Rectangle(640, 64, 64, 64)));
                                else chooseClass.Add(new Token(creationRec, 5, (int)(creationRec.Width * 0.15f), (int)(creationRec.Height * 0.3), new Rectangle(640, 128, 64, 64)));
                                if (!wlPicked) chooseClass.Add(new Token(creationRec, 6, (int)(creationRec.Width * 0.11f), (int)(creationRec.Height * 0.5), new Rectangle(640, 64, 64, 64)));
                                else chooseClass.Add(new Token(creationRec, 6, (int)(creationRec.Width * 0.11f), (int)(creationRec.Height * 0.5), new Rectangle(640, 128, 64, 64)));
                                messages.Add(new Message("Thief - <insert class definition here>", windlassFont14, new Vector2(GameConstants.HALF_WINDOW_WIDTH(), creationRec.Height / 2)));
                                messages.Add(new Message("Wildlander - <insert class definition here>", windlassFont14, new Vector2(GameConstants.HALF_WINDOW_WIDTH(), creationRec.Height * 0.7f)));
                            }
                            else if (archetypeName == "Warrior")
                            {
                                if (!bsPicked) chooseClass.Add(new Token(creationRec, 7, (int)(creationRec.Width * 0.15f), (int)(creationRec.Height * 0.3), new Rectangle(704, 64, 64, 64)));
                                else chooseClass.Add(new Token(creationRec, 7, (int)(creationRec.Width * 0.15f), (int)(creationRec.Height * 0.3), new Rectangle(704, 128, 64, 64)));
                                if (!knPicked) chooseClass.Add(new Token(creationRec, 8, (int)(creationRec.Width * 0.11f), (int)(creationRec.Height * 0.5), new Rectangle(704, 64, 64, 64)));
                                else chooseClass.Add(new Token(creationRec, 8, (int)(creationRec.Width * 0.11f), (int)(creationRec.Height * 0.5), new Rectangle(704, 128, 64, 64)));
                                messages.Add(new Message("Berserker - <insert class definition here>", windlassFont14, new Vector2(GameConstants.HALF_WINDOW_WIDTH(), creationRec.Height / 2)));
                                messages.Add(new Message("Knight - <insert class definition here>", windlassFont14, new Vector2(GameConstants.HALF_WINDOW_WIDTH(), creationRec.Height * 0.7f)));
                            }
                            loadOnce = true;
							foreach (Token pickClass in chooseClass) { foreach (int pickedClass in chosenHeroClass) if (pickedClass == pickClass.Variable) pickClass.Active = false; }
                        }
                        #endregion
                        if (mouse.LeftButton == ButtonState.Pressed && leftButtonReleased) { leftClickStarted = true; leftButtonReleased = false; }
                        else if (mouse.LeftButton == ButtonState.Released)
                        {
                            leftButtonReleased = true;
                            if (leftClickStarted)
                            {
                                leftClickStarted = false;
                                foreach (Token chosenClass in chooseClass) { if (chosenClass.DrawRectangle.Contains(mouse.X, mouse.Y) && chosenClass.Active) { chosenHeroClass[creatingHeroNumber - 1] = chosenClass.Variable; messages.Clear(); creatingHeroNumber++; creationStep = 2; } }
                                foreach (Token chosenClass in chooseClass) chosenClass.Active = false;
                                chooseClass.Clear(); loadOnce = false; 
                            }
                        }
                    }
                    #endregion
                    if (creatingHeroNumber > numHeroesPlaying) { finishedSelectingHeroes = true; numHeroTurns = numHeroesPlaying; creationStep = -1; }
                    #endregion
                    #region Adding Hero Sheets
                    if (finishedSelectingHeroes)
                    {
                        while (loadTokensSheets < numHeroesPlaying)
                        {
                            for (int x = 0; x < numHeroesPlaying; x++)
                            {
                                // variable for setting up selected heroes/classes
                                theClassName = GetClassName(chosenHeroClass[x]);
                                Rectangle drawingTokenRec = new Rectangle((int)((GameConstants.WINDOW_WIDTH / 2) - ((GameConstants.WINDOW_WIDTH * 0.333f) / 2)), (int)(GameConstants.DRAWING_TOKEN_START_Y), (int)(GameConstants.WINDOW_WIDTH * 0.333f), 64);
                                float multiplier;
                                if (numHeroesPlaying == 2) multiplier = 0.333f; else if (numHeroesPlaying == 3) multiplier = 0.25f; else multiplier = 0.2f;
                                int xLoc = (int)(((x + 1) * multiplier) * drawingTokenRec.Width); int yLoc = 0;
                                hpBars.Add(new Token(hpBarSource, hpBarLocation)); stamBars.Add(new Token(stamBarSource, stamBarLocation));
                                hpBars[x].Active = false; stamBars[x].Active = false;
                                switch (chosenHeroName[x] - 1)
                                {
                                    #region Creating sheets/tokens
                                    case 0: // Ashrian
                                        heroSheets.Add(new HeroSheet(theClassName, "healer", "Ashrian", 10, 4, 5, 4, 3, 2, 2, 4, "Status", "Activation", GameConstants.WINDOW_WIDTH / 2, GameConstants.WINDOW_HEIGHT));
                                        heroTokens.Add(new Token("Ashrian", x + 1, tiles[0], 10, 4, 5, new Rectangle(0, 0, 64, 64)));
                                        drawingTokens.Add(new Token(drawingTokenRec, x + 1, xLoc, yLoc, new Rectangle(0, 0, 64, 64)));
                                        break;
                                    case 1: // Avric
                                        heroSheets.Add(new HeroSheet(theClassName, "healer", "Avric", 12, 4, 4, 4, 4, 2, 3, 2, "Surge", "Activation", GameConstants.WINDOW_WIDTH / 2, GameConstants.WINDOW_HEIGHT));
                                        drawingTokens.Add(new Token(drawingTokenRec, x + 1, xLoc, yLoc, new Rectangle(64, 0, 64, 64)));
                                        heroTokens.Add(new Token("Avric", x + 1, tiles[0], 12, 4, 4, new Rectangle(64, 0, 64, 64)));
                                        break;
                                    case 2: // Leoric
                                        heroSheets.Add(new HeroSheet(theClassName, "mage", "Leoric", 8, 5, 4, 4, 2, 1, 5, 3, "Distance", "Activation", GameConstants.WINDOW_WIDTH / 2, GameConstants.WINDOW_HEIGHT));
                                        drawingTokens.Add(new Token(drawingTokenRec, x + 1, xLoc, yLoc, new Rectangle(128, 0, 64, 64)));
                                        heroTokens.Add(new Token("Leoric", x + 1, tiles[0], 8, 5, 4, new Rectangle(128, 0, 64, 64)));
                                        break;
                                    case 3: // Tarha
                                        heroSheets.Add(new HeroSheet(theClassName, "mage", "Tarha", 10, 4, 4, 4, 3, 2, 4, 2, "Reroll", "Activation", GameConstants.WINDOW_WIDTH / 2, GameConstants.WINDOW_HEIGHT));
                                        drawingTokens.Add(new Token(drawingTokenRec, x + 1, xLoc, yLoc, new Rectangle(192, 0, 64, 64)));
                                        heroTokens.Add(new Token("Tarha", x + 1, tiles[0], 10, 4, 4, new Rectangle(192, 0, 64, 64)));
                                        break;
                                    case 4: // Jain
                                        heroSheets.Add(new HeroSheet(theClassName, "scout", "Jain", 8, 5, 5, 4, 2, 2, 3, 4, "Damage", "Activation", GameConstants.WINDOW_WIDTH / 2, GameConstants.WINDOW_HEIGHT));
                                        drawingTokens.Add(new Token(drawingTokenRec, x + 1, xLoc, yLoc, new Rectangle(256, 0, 64, 64)));
                                        heroTokens.Add(new Token("Jain", x + 1, tiles[0], 8, 5, 5, new Rectangle(256, 0, 64, 64)));
                                        break;
                                    case 5: // Tomble
                                        heroSheets.Add(new HeroSheet(theClassName, "scout", "Tomble", 8, 5, 4, 4, 3, 1, 2, 5, "Adjacent", "Activation", GameConstants.WINDOW_WIDTH / 2, GameConstants.WINDOW_HEIGHT));
                                        drawingTokens.Add(new Token(drawingTokenRec, x + 1, xLoc, yLoc, new Rectangle(320, 0, 64, 64)));
                                        heroTokens.Add(new Token("Tomble", x + 1, tiles[0], 8, 5, 4, new Rectangle(320, 0, 64, 64)));
                                        break;
                                    case 6: // Grisban
                                        heroSheets.Add(new HeroSheet(theClassName, "warrior", "Grisban", 14, 4, 3, 4, 3, 5, 2, 1, "Recover", "Passive", GameConstants.WINDOW_WIDTH / 2, GameConstants.WINDOW_HEIGHT));
                                        drawingTokens.Add(new Token(drawingTokenRec, x + 1, xLoc, yLoc, new Rectangle(384, 0, 64, 64)));
                                        heroTokens.Add(new Token("Grisban", x + 1, tiles[0], 14, 4, 3, new Rectangle(384, 0, 64, 64)));
                                        break;
                                    case 7: // Syndrael
                                        heroSheets.Add(new HeroSheet(theClassName, "warrior", "Syndrael", 12, 4, 4, 4, 2, 4, 3, 2, "Stationary", "Passive", GameConstants.WINDOW_WIDTH / 2, GameConstants.WINDOW_HEIGHT));
                                        drawingTokens.Add(new Token(drawingTokenRec, x + 1, xLoc, yLoc, new Rectangle(448, 0, 64, 64)));
                                        heroTokens.Add(new Token("Syndrael", x + 1, tiles[0], 12, 4, 4, new Rectangle(448, 0, 64, 64)));
                                        break;
                                    default: break;
                                    #endregion
                                }
                                loadTokensSheets++;
                            }
                            if (loadTokensSheets >= numHeroesPlaying)
                            {
                                if (numHeroTurns == 3) { foreach (Token olToken in overlordTokens) if (olToken.Name == "doom") olToken.X = overlordTrack.DrawRectangle.X + overlordTrack.DrawRectangle.Width - 149; }
                                else if (numHeroTurns == 4) { foreach (Token olToken in overlordTokens) if (olToken.Name == "doom") olToken.X = overlordTrack.DrawRectangle.X + overlordTrack.DrawRectangle.Width - 225; }
                            }
                        }
                        if (!loadOnce)
                        {
                            messages.Add(new Message(heroSheets[buySkill].Name + " has 1 EXP to start the quest with. Either select\na skill to spend it OR hit End to store it.", windlassFont14, centerWindowMessage));
                            foreach (Token skillCard in heroSheets[buySkill].PickedClass.AllSkillCards) skillCard.Active = true;
                            loadOnce = true;
                        }
                        if (mouse.LeftButton == ButtonState.Pressed && leftButtonReleased) { leftClickStarted = true; leftButtonReleased = false; }
                        else if (mouse.LeftButton == ButtonState.Released)
                        {
                            leftButtonReleased = true;
                            if (leftClickStarted)
                            {
                                leftClickStarted = false;
                                
                                foreach (Token skillCard in heroSheets[buySkill].PickedClass.AllSkillCards)
                                {
                                    if(skillCard.DrawRectangle.Contains(mouse.X, mouse.Y)){
                                        int theVariable = skillCard.Variable;
                                        if (theVariable < 4)
                                        {
                                            Rectangle secondSkill;
                                            if (heroSheets[buySkill].PickedClass.ClassName == "necromancer") secondSkill = new Rectangle(GameConstants.MAIN_SKILL_LOC_X + (GameConstants.MAIN_SKILL_BUFFER_X * 2), GameConstants.WINDOW_HEIGHT - 192, 128, 192);
                                            else secondSkill = new Rectangle(GameConstants.MAIN_SKILL_LOC_X + GameConstants.MAIN_SKILL_BUFFER_X, GameConstants.WINDOW_HEIGHT - 192, 128, 192);
                                            heroSheets[buySkill].PickedClass.SkillList.Add(new Token(secondSkill, theVariable, 0, 0, skillCard.SourceRectangle));
                                            messages.Clear();
                                            buySkill++; loadOnce = false;
                                        }
                                        else messages.Add(new Message("That skill is to expensive to purchase right now, choose again.", windlassFont14, new Vector2(centerWindowMessage.X, centerWindowMessage.Y - 50)));
                                    }
                                }                                
                                if(endToken.DrawRectangle.Contains(mouse.X, mouse.Y)){
                                    messages.Clear();
                                    heroSheets[buySkill].PickedClass.CurrentExp++;
                                    messages.Add(new Message(heroSheets[buySkill].Name + " has stored the EXP for later.", windlassFont14, new Vector2(centerWindowMessage.X, centerWindowMessage.Y = 50)));
                                    buySkill++; loadOnce = false;                                    
                                }
                            }
                        }
                        if(buySkill == numHeroesPlaying) currentGameState = GameState.InitializeRoom;
                    }
                    #endregion
                    break;
                #endregion
                #region Game State: Initialize Room
                case GameState.InitializeRoom:
                    switch(currentRoom){
                        case "The Onset":
                            monsterTokens.Add(new Token("zombie", new Rectangle(960, 64, 64, 64), true, 0, tiles[0], GameConstants.MONSTER_LOC_19B[0, 0, 0], GameConstants.MONSTER_LOC_19B[0, 0, 1], GameConstants.ZOMBIE_FLESH_W, GameConstants.ZOMBIE_FLESH_BARGH_H, 0, 0, 0));
                            monsterTokens.Add(new Token("barghest", new Rectangle(513, 256, 128, 64), true, 3, tiles[0], GameConstants.MONSTER_LOC_19B[1, 0, 0], GameConstants.MONSTER_LOC_19B[1, 0, 1], GameConstants.BARGH_W, GameConstants.ZOMBIE_FLESH_BARGH_H, 0, 0, 0));
                            if (heroTokens.Count >= 3)
                            {
                                monsterTokens.Add(new Token("zombie", new Rectangle(896, 64, 64, 64), false, 0, tiles[0], GameConstants.MONSTER_LOC_19B[0, 1, 0], GameConstants.MONSTER_LOC_19B[0, 1, 1], GameConstants.ZOMBIE_FLESH_W, GameConstants.ZOMBIE_FLESH_BARGH_H, 0, 0, 0));
                                monsterTokens.Add(new Token("barghest", new Rectangle(512, 192, 128, 64), false, 3, tiles[0], GameConstants.MONSTER_LOC_19B[1, 1, 0], GameConstants.MONSTER_LOC_19B[1, 1, 1], GameConstants.BARGH_W, GameConstants.ZOMBIE_FLESH_BARGH_H, 0, 0, 0));
                            }
                            if (heroTokens.Count == 4)
                            {
                                monsterTokens.Add(new Token("zombie", new Rectangle(896, 64, 64, 64), false, 0, tiles[0], GameConstants.MONSTER_LOC_19B[0, 2, 0], GameConstants.MONSTER_LOC_19B[0, 2, 1], GameConstants.ZOMBIE_FLESH_W, GameConstants.ZOMBIE_FLESH_BARGH_H, 0, 0, 0));
                                monsterTokens.Add(new Token("barghest", new Rectangle(512, 192, 128, 64), false, 3, tiles[0], GameConstants.MONSTER_LOC_19B[1, 2, 0], GameConstants.MONSTER_LOC_19B[1, 2, 1], GameConstants.BARGH_W, GameConstants.ZOMBIE_FLESH_BARGH_H, 0, 0, 0));
                            }
                            searchTokens.Add(new Token(new Rectangle(960, 0, 64, 64), endCaps[0], 0, 64, 0));
                            break;
                        default: break;
                    }
                    foreach (Token mToken in monsterTokens)
                    {
                        if (mToken.Name == "barghest" && !barghestFound) { addMonsterSheet(mToken.Name); barghestFound = true; }
                        else if (mToken.Name == "flesh moulder" && !fleshFound) { addMonsterSheet(mToken.Name); fleshFound = true; }
                        else if (mToken.Name == "shadow dragon" && !dragonFound) { addMonsterSheet(mToken.Name); dragonFound = true; }
                        else if (mToken.Name == "zombie" && !zombieFound) { addMonsterSheet(mToken.Name); zombieFound = true; }
                        foreach (MonsterSheet mSheet in monsterSheet)
                        {
                            if (mToken.Name == mSheet.Name)
                            {
                                if (mToken.IsMaster) { mToken.HP = mSheet.MasHP; mToken.Movement = mSheet.MasMove; }
                                else { mToken.HP = mSheet.MinHP; mToken.Movement = mSheet.MinMove; }
                            }
                        }
                    }
                    currentGameState = GameState.StartingPositions;
                    break;
                #endregion
                #region Game State: Place Hero Starting Positions
                case GameState.StartingPositions:
                    int placeX = tiles[0].X + 128, placeY = tiles[0].Y;
                    if (!loadOnce)
                    {
                        for (int y = 0; y < 3; y++) { for (int x = 0; x < 2; x++) { floorHighlights.Add(new Token(new Rectangle(704, 0, 64, 64), new Rectangle(placeX + (x * 64), placeY + (y * 64), 64, 64))); } }
                        loadOnce = true;
                    }
                    messages.Add(new Message("Place " + heroSheets[heroPlacementCount].Name + " in a red starting square.", windlassFont23, centerWindowMessage));
                    if (mouse.LeftButton == ButtonState.Pressed && leftButtonReleased) { leftClickStarted = true; leftButtonReleased = false; }
                    else if (mouse.LeftButton == ButtonState.Released)
                    {
                        leftButtonReleased = true;
                        if (leftClickStarted)
                        {
                            leftClickStarted = false;
                            foreach (Token floorLight in floorHighlights)
                            {
                                if (floorLight.DrawRectangle.Contains(mouse.X, mouse.Y))
                                {
                                    heroTokens[heroPlacementCount].DrawRectangle = floorLight.DrawRectangle;
                                    heroTokens[heroPlacementCount].OriginalDrawRect = heroTokens[heroPlacementCount].DrawRectangle;
                                    heroTokens[heroPlacementCount].LocationX = heroTokens[heroPlacementCount].DrawRectangle.X + 32;
                                    heroTokens[heroPlacementCount].LocationY = heroTokens[heroPlacementCount].DrawRectangle.Y + 32;
                                    heroTokens[heroPlacementCount].OriginalLocation = heroTokens[heroPlacementCount].Location;
                                    heroPlacementCount++;
                                    messages.Clear();
                                }
                            }
                        }
                    }
                    if (heroPlacementCount >= numHeroesPlaying) { currentGameState = GameState.HeroTurn; loadOnce = false; floorHighlights.Clear(); }
                    break;
                #endregion
                #region Game State: Hero Turn
                case GameState.HeroTurn:
                    #region Fade Delay
                    mFadeDelay -= gameTime.ElapsedGameTime.TotalSeconds;
                    if (mFadeDelay <= 0)
                    {
                        mFadeDelay = 0.015;
                        mAlphaValue += mFadeIncrement;
                        // Below is used for a pulsing effect. 
                        // TODO: Make a 2nd series for a glowing ring on selected hero token
                        //if (mAlphaValue >= 225 || mAlphaValue <= 0) mFadeIncrement *= -1;
                    }
                    #endregion
                    switch(currentHeroState)
					{
						#region Selecting Hero
						case HeroState.SelectHero:
							if (!heroTokenClicked) { messages.Add(new Message("Click a hero token for turn #" + currentHeroTurn, windlassFont23, centerWindowMessage)); }
							if (mouse.LeftButton == ButtonState.Pressed && leftButtonReleased) { leftClickStarted = true; leftButtonReleased = false; }
							else if (mouse.LeftButton == ButtonState.Released)
							{
								leftButtonReleased = true;
								if (leftClickStarted)
								{
                                    leftClickStarted = false;
                                    checkVariousItems(mouse);
									foreach (Token drawingToken in drawingTokens)
									{
										if (drawingToken.DrawRectangle.Contains(mouse.X, mouse.Y))
										{
											heroNumPosition = drawingToken.Variable - 1;
											selectedHeroName = heroTokens[heroNumPosition].Name;
											heroTokenClicked = true;
											messages.Clear();
											heroSheets[heroNumPosition].ActiveSheet = true;
											currentHeroState = HeroState.StartTurnAbility;
											drawingToken.Active = false;
										}
									}
								}
							}
							break;
						#endregion
						#region Start of Turn abilities
						case HeroState.StartTurnAbility:
                            if (familiarActive && !familiarActed && heroSheets[heroNumPosition].PickedClass.ClassName == "necromancer")
                            {
                                messages.Add(new Message("Activate the reanimate before the necromancers turn?", windlassFont23, centerWindowMessage));
                            }
                            else { currentHeroState = HeroState.RefreshCards; if (familiarActive && familiarActed) familiarActed = false; }
                            if (mouse.LeftButton == ButtonState.Pressed && leftButtonReleased) { leftClickStarted = true; leftButtonReleased = false; }
                            else if (mouse.LeftButton == ButtonState.Released)
                            {
                                leftButtonReleased = true;
                                if (leftClickStarted)
                                {
                                    leftClickStarted = false;
                                    checkVariousItems(mouse);
                                    foreach (Token yesNo in yesNoList)
                                    {
                                        if (yesNo.DrawRectangle.Contains(mouse.X, mouse.Y))
                                        {
                                            messages.Clear();
                                            int answer = yesNo.Variable;
                                            if (answer == 0) currentHeroState = HeroState.FamiliarActions;
                                            else { familiarActed = true; }
                                        }
                                    }
                                }
                            }
                            break;
                        #endregion
                        #region Familiar Actions
                        case HeroState.FamiliarActions:
                            choosingAction = false;
                            Token target;
                            if (!loadOnce)
                            {
                                foreach (Token action in actionButtons) action.Active = false;
                                actionButtons[0].Active = true;
                                actionButtons[2].Active = true;
                                loadOnce = true;
                            }
                            else familiarAttacked = true;

                            if (heroSheets[heroNumPosition].PickedClass.ClassName == "necromancer" && familiarActionSheetOn) messages.Add(new Message("Reanimate actions.", windlassFont14, actionWindowTop));

                            if (familiarAction == "attacking") { familiarChoosing = true; messages.Add(new Message("Select a target to attack.", windlassFont23, centerWindowMessage)); }
                            else if(familiarAction == "moving") { familiarChoosing = true; messages.Add(new Message("Select an area to move too.", windlassFont23, centerWindowMessage)); }

                            if (mouse.LeftButton == ButtonState.Pressed && leftButtonReleased) { leftClickStarted = true; leftButtonReleased = false; }
                            else if (mouse.LeftButton == ButtonState.Released)
                            {
                                leftButtonReleased = true;
                                if (leftClickStarted)
                                {
                                    leftClickStarted = false;
                                    //if(!familiarAttacking || !familiarMoving) checkVariousItems(mouse);
                                    #region Checking for Action Click
                                    foreach (Token action in actionButtons)
                                    {
                                        messages.Clear();
                                        if (action.DrawRectangle.Contains(mouse.X, mouse.Y))
                                        {
                                            if (action.Variable == 1 && action.Active)
                                            {
                                                if (heroSheets[heroNumPosition].PickedClass.ClassName == "necromancer")
                                                {
                                                    familiarDie1 = new Dice(0);
                                                    familiarDie2 = new Dice(1);
                                                    if (heroSheets[heroNumPosition].PickedClass.SkillsOwned[5]) familiarDie3 = new Dice(2);
                                                }
                                                DetermineRanges(1, "familiar", heroTokens[heroTokens.Count - 1].DrawRectangle, "Familiar");
                                                familiarAction = "attacking";
                                            }
                                            else if (action.Variable == 3 && action.Active)
                                            {
                                                DetermineRanges(heroTokens[heroTokens.Count - 1].Movement, "familiar", heroTokens[heroTokens.Count - 1].DrawRectangle, "Familiar");
                                                familiarAction = "moving";
                                            }
                                            familiarActionSheetOn = false;
                                            action.Active = false;
                                        }
                                    }
                                    #endregion
                                    #region Checking for Floor Tile Click
                                    foreach (Token floorLight in floorHighlights)
                                    {
                                        if (floorLight.DrawRectangle.Contains(mouse.X, mouse.Y))
                                        {
                                            #region Attacking
                                            if (familiarAction == "attacking")
                                            {
                                                foreach (Token mToken in monsterTokens)
                                                {
                                                    if (mToken.DrawRectangle.Contains(mouse.X, mouse.Y))
                                                    {
                                                        familiarChoosing = false;
                                                        if(calcAttack)
                                                        {                                                        
                                                            target = mToken;
                                                            RollAttack(familiarDie1);
                                                            RollAttack(familiarDie2);
                                                            if (familiarDie3 != null && familiarDie3.DieColor >= 0) { RollAttack(familiarDie3); }
                                                            string monsterName = mToken.Name;
                                                            foreach (MonsterSheet mSheet in monsterSheet)
                                                            {
                                                                targetMonsterSheet = mSheet;
                                                                if (mSheet.Name == monsterName)
                                                                {
                                                                    int monDie1 = -1, monDie2 = -1;
                                                                    if (mToken.IsMaster) { monDie1 = mSheet.MasDef1; if (mSheet.MasDef2 >= 0) monDie2 = mSheet.MasDef2; }
                                                                    else { monDie1 = mSheet.MinDef1; if (mSheet.MinDef2 >= 0) monDie2 = mSheet.MinDef2; }
                                                                    defenseDice1 = new Dice(monDie1);
                                                                    RollDefense(defenseDice1);
                                                                    if (monDie2 >= 0) { defenseDice2 = new Dice(monDie2); RollDefense(defenseDice2); }
                                                                }
                                                            }
                                                            messages.Clear();
                                                            if(totalSurge > 0) totalAttack++;
                                                            int totalDamageDealt = totalAttack - totalDefense;
                                                            messages.Add(new Message("Reanimate dealt " + (totalDamageDealt) + " damage to the " + target.Name + ".", windlassFont23, centerWindowMessage));
                                                            if (totalSurge > 0) messages.Add(new Message("+1 Damage added from available surge", windlassFont14, new Vector2(centerWindowMessage.X, centerWindowMessage.Y + 30)));
                                                            if (totalDamageDealt > 0) mToken.HP -= totalDamageDealt;
                                                            if (mToken.HP <= 0)
                                                            {
                                                                mToken.Active = false;
                                                                messages.Add(new Message("The " + mToken.Name + " was defeated!", windlassFont14, new Vector2(centerWindowMessage.X, centerWindowMessage.Y + 60)));
                                                            }
                                                            familiarAttacked = true;
                                                            familiarAction = "";
                                                        }
                                                    }
                                                }
                                            }
                                            #endregion
                                            #region Moving
                                            else if(familiarAction == "moving")
                                            {
                                                familiarChoosing = false;
                                                familiarActionSheetOn = false;
                                                messages.Clear();
                                                if (floorLight.Active && !floorLight.Occupied) { heroTokens[heroTokens.Count - 1].NewSetTarget(new Vector2(floorLight.DrawRectangle.Center.X, floorLight.DrawRectangle.Center.Y)); heroMoving = true; }
                                                familiarMoved = true;
                                                familiarAction = "";                                                
                                            }
                                            #endregion
                                        }
                                    }
                                    #endregion
                                    #region Checking for End Button Click
                                    if (endToken.DrawRectangle.Contains(mouse.X, mouse.Y))
                                    {
                                        foreach (Tile tile in tiles) if (heroTokens[heroTokens.Count - 1].DrawRectangle.Intersects(tile.DrawRectangle)) heroTokens[heroTokens.Count - 1].adjustPosition(tile.DrawRectangle);
                                        foreach (Tile endCap in endCaps) if (heroTokens[heroTokens.Count - 1].DrawRectangle.Intersects(endCap.DrawRectangle)) heroTokens[heroTokens.Count - 1].adjustPosition(endCap.DrawRectangle);
                                        familiarActionSheetOn = true;
                                        floorHighlights.Clear();
                                        if (familiarAttacked && familiarMoved)
                                        {
                                            familiarActed = true;
                                            messages.Clear(); floorHighlights.Clear(); loadOnce = false; heroMoving = false;
                                            foreach (Token action in actionButtons) action.Active = true;
                                            ClearDiceRolls();
                                            checkActionPoints(heroSheets[heroNumPosition]);
                                        }
                                    }
                                    #endregion
                                }
                            }
                            if (familiarActed)
                            {
                                ResetFamiliar();
                                checkActionPoints(heroSheets[heroNumPosition]);
                            }
                            break;
                        #endregion
                        #region Refresh Cards
                        case HeroState.RefreshCards:
                            for (int x = 0; x < 9; x++) { heroSheets[heroNumPosition].PickedClass.SkillsExhausted[x] = false; }
                            currentHeroState = HeroState.EquipItems;
                            break;
                        #endregion
                        #region Equip Items
                        case HeroState.EquipItems:
                            if (heroSheets[heroNumPosition].PickedClass.BackPack.Count > 0)
                            {
                                if (!loadOnce)
                                {
                                    messages.Clear();
                                    messages.Add(new Message("Choose what you would like to equip. When you're\ndone, click exit to finish equipping", windlassFont14, centerWindowMessage));
                                }
                                // TODO: Add code here to see about swapping equipment
                            }
                            else currentHeroState = HeroState.SelectActions;
                            break;
                        #endregion
                        #region Selecting an Action
                        case HeroState.SelectActions:
                            if (heroTokenClicked && choosingAction)
                            {
                                messages.Add(new Message("Choose an Action", windlassFont23, actionWindowTop));
                                messages.Add(new Message(selectedHeroName + ": " + heroSheets[heroNumPosition].ActionPoints + " Action Point(s)", windlassFont14, actionWindowBottom));
                            } 
                            if (mouse.LeftButton == ButtonState.Pressed && leftButtonReleased) { leftClickStarted = true; leftButtonReleased = false; }
                            else if (mouse.LeftButton == ButtonState.Released)
                            {
                                leftButtonReleased = true;
                                if (leftClickStarted)
                                {
                                    leftClickStarted = false;
                                    checkVariousItems(mouse);
                                    foreach (Token actButton in actionButtons)
                                    {
                                        if (actButton.DrawRectangle.Contains(mouse.X, mouse.Y))
                                        {
                                            selectedActionNumber = actButton.Variable; loadOnce = false;
                                            if (selectedActionNumber == 1) { loadOnce = true; floorHighlights.Clear(); }
                                            GetHeroActionState(selectedActionNumber);
                                            choosingAction = false;
                                            currentHeroState = HeroState.PerformAction;
                                            if (familiarActive) familiarActed = false;
                                            messages.Clear();
                                        }
                                    }
                                }
                            }
                            break;
                        #endregion
                        #region Performing the Action
                        case HeroState.PerformAction:
							Rectangle tokenPosition = heroTokens[heroNumPosition].DrawRectangle;
                            HeroSheet thisHeroSheet = heroSheets[heroNumPosition];
                            HeroClass currentHeroClass = heroSheets[heroNumPosition].PickedClass;
                            Token currentHeroToken = heroTokens[heroNumPosition];
							string archetype = heroSheets[heroNumPosition].Archetype;
                            switch (currentHeroActionState)
							{
								#region Attack Action
								case HeroActionState.AttackAction:
                                    if (!weaponPicked) { floorHighlights.Clear(); messages.Add(new Message("Select a weapon to attack with.", windlassFont23, centerWindowMessage)); }
									if (!loadOnce)
                                    {
                                        #region Loading target select message and determining range
                                        messages.Clear();
                                        messages.Add(new Message("Select a target to attack.", windlassFont23, centerWindowMessage));
                                        DetermineRanges(attackRange, archetype, tokenPosition, "Attack"); loadOnce = true;
                                        #endregion
                                    }
                                    if (!calcAttack && attackHit && LoSFound)
                                    {
                                        #region Reviewing attack
                                        if (totalSurge > 0 && !loadSurgeOnce)
                                        {
                                            #region checking surges
                                            loadSurgeOnce = true;
											hasSurges = true;
											messages.Add(new Message("Number of surges to use: " + totalSurge, windlassFont14, actionWindowTop));
                                            if (removeSurgeNumber >= 0 && surgeList.Count > 0) { surgeList.RemoveAt(removeSurgeNumber); surgeList.Insert(removeSurgeNumber, ""); }
                                            #endregion
                                        }
                                        else if (totalSurge <= 0)
                                        {
                                            #region post surge check, finalizing attack
                                            attackHit = false; hasSurges = false; surgeList.Clear(); surgeListRect.Clear();
                                            int rangeToTarget = DetermineDistanceLong(heroTokens[heroNumPosition], targetMonsterToken);
                                            if (totalRange >= rangeToTarget)
                                            {
                                                if (totalDefense < 0) totalDefense = 0;
                                                int totalDmg = totalAttack - totalDefense;
                                                if (totalDmg > 0) targetMonsterToken.HP -= totalDmg;
                                                messages.Add(new Message(totalDmg.ToString(), windlassFont36, new Vector2(targetMonsterToken.DrawRectangleX + (targetMonsterToken.DrawRectangle.Width /2) , targetMonsterToken.DrawRectangleY + targetMonsterToken.DrawRectangle.Height / 2)));
                                                if (targetMonsterToken.HP <= 0)
                                                {
                                                    targetMonsterToken.Active = false;
                                                    checkWeaponBonus(targetMonsterToken, attackingWeapon);
                                                    messages.Add(new Message("The " + targetMonsterToken.Name + " has been defeated!", windlassFont14, new Vector2(centerWindowMessage.X, centerWindowMessage.Y + 30)));
                                                    UpdateLootTrackWindow(targetMonsterToken);
                                                }
                                            }
                                            else attackHit = false;
                                            #endregion
                                        }
                                        if(!awardingLoot) messages.Add(new Message("Rng: " + totalRange + " Dmg: " + totalAttack + " Srg: " + totalSurge + " Def: " + totalDefense, windlassFont23, centerWindowMessage));
                                        #endregion
                                    }
									if (!calcAttack && !attackHit && totalRange == 0) messages.Add(new Message("Attack missed...", windlassFont23, centerWindowMessage));
									if (mouse.LeftButton == ButtonState.Pressed && leftButtonReleased) { leftClickStarted = true; leftButtonReleased = false; }
									else if (mouse.LeftButton == ButtonState.Released)
									{
										leftButtonReleased = true;
										if (leftClickStarted)
										{
                                            leftClickStarted = false;
                                            //checkVariousItems(mouse);
                                            #region Checks for main hand weapon click
											if (currentHeroClass.CurrentWeaponRect.Contains(mouse.X, mouse.Y) && !weaponPicked) {
                                                addSurgeUsage(currentHeroClass.CurrentWeapon, heroSheets[heroNumPosition], heroTokens[heroNumPosition]);
												attackRange = currentHeroClass.MainAttackRange;
                                                weaponPicked = true; weaponUsed = "main"; loadOnce = false; attackingWeapon = currentHeroClass.CurrentWeapon;
												messages.Clear();
                                            }
                                            #endregion
                                            #region Checks for off hand weapon click
											if (currentHeroClass.OffHandRect.Contains(mouse.X, mouse.Y) && !weaponPicked && (currentHeroClass.OffHand.Type == "melee" || currentHeroClass.OffHand.Type == "range")){
                                                addSurgeUsage(currentHeroClass.OffHand, heroSheets[heroNumPosition], heroTokens[heroNumPosition]);
												attackRange = currentHeroClass.OffAttackRange;
                                                weaponPicked = true; weaponUsed = "off"; loadOnce = false; messages.Clear(); attackingWeapon = currentHeroClass.OffHand;
                                            }
                                            #endregion
                                            #region Checks for end button click
                                            if (endToken.DrawRectangle.Contains(mouse.X, mouse.Y))
                                            {
                                                weaponPicked = false; loadOnce = false; calcAttack = true; attackHit = true; hasSurges = false; loadSurgeOnce = false; LoSFound = false;
                                                ClearDiceRolls(); removeSurgeNumber = -1;
                                                messages.Clear(); floorHighlights.Clear();
                                                thisHeroSheet.ActionPoints--;
                                                checkActionPoints(heroSheets[heroNumPosition]);
                                            }
                                            #endregion
                                            #region Checks for surge button click
											foreach(Token surge in surgeListRect)
											{
												if (surge.DrawRectangle.Contains(mouse.X, mouse.Y))
												{
                                                    messages.Clear();
                                                    loadSurgeOnce = false;
													removeSurgeNumber = surge.Variable - 1;
                                                    if (surgeList[removeSurgeNumber].ToString() != "")
                                                    {
                                                        UseSurge(surgeList[removeSurgeNumber], targetMonsterToken);
                                                        totalSurge--;
                                                    }
												}
                                            }
                                            #endregion
                                            #region Checks for monster token click for attack
                                            // Checks each monster for click and rolls damage based on weapon used and defense of monster
											foreach (Token mToken in monsterTokens)
											{
												if (mToken.DrawRectangle.Contains(mouse.X, mouse.Y) && weaponPicked)
												{
                                                    messages.Clear();
                                                    if (attackingWeapon.Type == "melee") { calcAttack = true; LoSFound = true; }
                                                    else
                                                    {
                                                        dots.Clear();
                                                        #region Checking LoS on Ranged weapons
                                                        bool foundLoS;
                                                        List<Vector2> heroPoints = new List<Vector2>();
                                                        heroPoints.Add(new Vector2(currentHeroToken.DrawRectangle.X, currentHeroToken.DrawRectangle.Y));
                                                        heroPoints.Add(new Vector2(currentHeroToken.DrawRectangle.X + 64, currentHeroToken.DrawRectangle.Y));
                                                        heroPoints.Add(new Vector2(currentHeroToken.DrawRectangle.X, currentHeroToken.DrawRectangle.Y + 64));
                                                        heroPoints.Add(new Vector2(currentHeroToken.DrawRectangle.X + 64, currentHeroToken.DrawRectangle.Y + 64));
                                                        List<Vector2> monsterPoints = new List<Vector2>();
                                                        monsterPoints.Add(new Vector2(mToken.DrawRectangle.X, mToken.DrawRectangle.Y));
                                                        monsterPoints.Add(new Vector2(mToken.DrawRectangle.X + 64, mToken.DrawRectangle.Y));
                                                        monsterPoints.Add(new Vector2(mToken.DrawRectangle.X, mToken.DrawRectangle.Y + 64));
                                                        monsterPoints.Add(new Vector2(mToken.DrawRectangle.X + 64, mToken.DrawRectangle.Y + 64));
                                                        for (int y = 0; y < 4; y++)
                                                        {
                                                            for (int x = 0; x < 4; x++)
                                                            {
                                                                foundLoS = DetermineLineOfSight((int)heroPoints[y].X, (int)heroPoints[y].Y, (int)monsterPoints[x].X, (int)monsterPoints[x].Y);
                                                                if (foundLoS) { LoSFound = true; calcAttack = true; break; }
                                                            }
                                                            if (LoSFound) break;
                                                        }
                                                        if (!LoSFound)
                                                        {
                                                            messages.Add(new Message("Line of sight could not be found to the " + mToken.Name + ".\nSelect another target within range.", windlassFont14, centerWindowMessage));
                                                        }
                                                        #endregion
                                                    }
                                                    if (calcAttack && LoSFound)
													{
                                                        messages.Clear();
														targetMonsterToken = mToken;
														if (weaponUsed == "main") { attackDice1 = currentHeroClass.MainDie1; attackDice2 = currentHeroClass.MainDie2; attackDice3 = currentHeroClass.MainDie3; }
														else { attackDice1 = currentHeroClass.OffDie1; attackDice2 = currentHeroClass.OffDie2; attackDice3 = currentHeroClass.OffDie3; }
														RollAttack(attackDice1);
                                                        //messages.Add(new Message("Side: "+(attackDice1.RandomSide+1)+"Rng: "+attackDice1.Range+" Atk: "+attackDice1.Attack+" Srg: "+attackDice1.Surge, windlassFont14, new Vector2(300, 50)));
                                                        checkWeaponBonus(targetMonsterToken, attackingWeapon);
                                                        if (attackDice2.DieColor >= 0) { RollAttack(attackDice2);
                                                            //messages.Add(new Message("Side: " + (attackDice2.RandomSide + 1) + "Rng: " + attackDice2.Range + " Atk: " + attackDice2.Attack + " Srg: " + attackDice2.Surge, windlassFont14, new Vector2(300, 100)));
                                                        }
														if (attackDice3.DieColor >= 0) { RollAttack(attackDice3); }
														string monsterName = mToken.Name;
														foreach (MonsterSheet mSheet in monsterSheet)
														{
															targetMonsterSheet = mSheet;
															if (mSheet.Name == monsterName)
															{
																int monDie1 = -1, monDie2 = -1; 
																if (mToken.IsMaster) { monDie1 = mSheet.MasDef1; if (mSheet.MasDef2 >= 0) monDie2 = mSheet.MasDef2; }
																else { monDie1 = mSheet.MinDef1; if (mSheet.MinDef2 >= 0) monDie2 = mSheet.MinDef2; }
																defenseDice1 = new Dice(monDie1);
																RollDefense(defenseDice1);
																if (monDie2 >= 0) { defenseDice2 = new Dice(monDie2); RollDefense(defenseDice2); }
															}
														}
														calcAttack = false;
													}
												}
                                            }
                                            #endregion
                                            #region Checks for reward click
                                            if (awardingLoot)
                                            {
                                                bool lootTaken = false;
                                                foreach (Card loot in awardedShopCards)
                                                    if (loot.DrawRectangle.Contains(mouse.X, mouse.Y))
                                                    {
                                                        currentHeroClass.AddToBackPack(loot);
                                                        awardedCardHold = loot;
                                                        lootTaken = true;
                                                    }
                                                if (lootTaken)
                                                {
                                                    awardedShopCards.Remove(awardedCardHold);
                                                    foreach (Card loot in awardedShopCards)
                                                    {
                                                        if (currentAct == 1) { shop1Deck.putCardBack(loot); shop1Deck.Shuffle(random); }
                                                        else { shop2Deck.putCardBack(loot); shop2Deck.Shuffle(random); }
                                                    }
                                                    lootTaken = false; awardingLoot = false;
                                                    messages.Clear();
                                                }
                                            }
                                            #endregion
                                        }
									}
                                    break;
								#endregion
								#region Search Action
								case HeroActionState.SearchAction:
									int searchRange = thisHeroSheet.SearchRange;
                                    if (!weaponPicked) { messages.Add(new Message("Select a search token within range.", windlassFont23, centerWindowMessage)); }
									if (!loadOnce) { DetermineRanges(searchRange, archetype, tokenPosition, "Search"); loadOnce = true; }
									int searchCoinIndex = 0;
									if (mouse.LeftButton == ButtonState.Pressed && leftButtonReleased) { leftClickStarted = true; leftButtonReleased = false; }
									else if (mouse.LeftButton == ButtonState.Released)
									{
										leftButtonReleased = true;
										if (leftClickStarted)
										{
                                            leftClickStarted = false;
                                            //checkVariousItems(mouse);
											foreach (Token searchCoin in searchTokens)
											{
												if (searchCoin.DrawRectangle.Contains(mouse.X, mouse.Y))
												{
													if (!searchedOnce)
													{
                                                        weaponPicked = true;
                                                        messages.Clear();
														messages.Add(new Message(thisHeroSheet.Name + " found...", windlassFont23, centerWindowMessage));
                                                        searchDeck.pullSearchCard(thisHeroSheet);
														searchedOnce = true;
                                                        if (thisHeroSheet.PickedClass.SearchCards[thisHeroSheet.PickedClass.SearchCards.Count - 1].Name == "Treasure Chest")
                                                        {
                                                            thisHeroSheet.PickedClass.SearchCards.RemoveAt(thisHeroSheet.PickedClass.SearchCards.Count - 1);
                                                            if (currentAct == 1) shop1Deck.pullShopCard(thisHeroSheet);
                                                            else shop2Deck.pullShopCard(thisHeroSheet);
                                                        }
													}
													searchTokenClicked = true;
													searchCoinIndex = searchCoin.Variable;
												}
											}
											foreach (Card displayedCard in searchDeck.DiscardDeck)
											{
												if (displayedCard.DrawRectangle.Contains(mouse.X, mouse.Y))
												{
                                                    weaponPicked = false; displayedCard.Active = false; searchTokenClicked = false; loadOnce = false;
													messages.Clear();
                                                    searchTokens.RemoveAt(searchCoinIndex);
                                                    thisHeroSheet.ActionPoints--;
													checkActionPoints(heroSheets[heroNumPosition]);
												}
											}
										}
									}
                                    break;
								#endregion
								#region Move Action
								case HeroActionState.MoveAction:
                                    int movementPoints = heroTokens[heroNumPosition].Movement;
                                    if (movementPoints == 0) { movementButtons[0].Active = false; movementButtons[1].Active = false; zeroMovementPoints = true; }
                                    if (!loadOnce) { DetermineRanges(movementPoints, archetype, tokenPosition, "Move"); loadOnce = true; }
                                    if (mouse.LeftButton == ButtonState.Pressed && leftButtonReleased) { leftClickStarted = true; leftButtonReleased = false; }
                                    else if (mouse.LeftButton == ButtonState.Released)
                                    {
                                        leftButtonReleased = true;
                                        if (leftClickStarted)
                                        {
                                            leftClickStarted = false;
                                            //checkVariousItems(mouse);
                                            foreach (Token hToken in heroTokens)
                                            {
												foreach (Token floorLight in floorHighlights) 
                                                    if (floorLight.DrawRectangle.Contains(mouse.X, mouse.Y) && floorLight.Active && !floorLight.Occupied && selectedHeroName == hToken.Name) { 
                                                        hToken.NewSetTarget(new Vector2(floorLight.DrawRectangle.Center.X, floorLight.DrawRectangle.Center.Y)); 
                                                        heroMoving = true; 
                                                        messages.Add(new Message(hToken.Name + " used " + hToken.MovementUsed + " MP.", windlassFont14, new Vector2(20, 200), 0));
                                                    }                                                
                                            }
                                            #region Checking Movement Buttons
                                            foreach (Token moveButton in movementButtons)
											{
												if(moveButton.DrawRectangle.Contains(mouse.X, mouse.Y)){
													int moveValue = moveButton.Variable;
                                                    currentHeroToken.Active = true;
                                                    currentHeroToken.MovementPath.Clear();
                                                    heroMoving = false;
													if (moveValue == 1)
													{
                                                        currentHeroToken.Location = currentHeroToken.OriginalLocation;
                                                        currentHeroToken.DrawRectangle = currentHeroToken.OriginalDrawRect;
													}
													else if (moveValue == 2)
                                                    {
                                                        floorHighlights.Clear();
                                                        //currentHeroToken.OriginalLocation = currentHeroToken.Location;
                                                        //currentHeroToken.OriginalDrawRect = currentHeroToken.DrawRectangle;
                                                        choosingAction = true;
                                                        loadOnce = false;
                                                        foreach (Tile tile in tiles) if (currentHeroToken.DrawRectangle.Intersects(tile.DrawRectangle)) currentHeroToken.adjustPosition(tile.DrawRectangle);
                                                        foreach (Tile endCap in endCaps) if (currentHeroToken.DrawRectangle.Intersects(endCap.DrawRectangle)) currentHeroToken.adjustPosition(endCap.DrawRectangle);
                                                        currentHeroToken.Movement -= currentHeroToken.MovementUsed;
                                                        currentHeroActionState = HeroActionState.ChooseAction;
                                                        currentHeroState = HeroState.SelectActions;
													}
													else
                                                    {
                                                        floorHighlights.Clear();
                                                        //currentHeroToken.OriginalLocation = currentHeroToken.Location;
                                                        //currentHeroToken.OriginalDrawRect = currentHeroToken.DrawRectangle; 
                                                        loadOnce = false;
                                                        foreach (Tile tile in tiles) if (currentHeroToken.DrawRectangle.Intersects(tile.DrawRectangle)) currentHeroToken.adjustPosition(tile.DrawRectangle);
                                                        foreach (Tile endCap in endCaps) if (currentHeroToken.DrawRectangle.Intersects(endCap.DrawRectangle)) currentHeroToken.adjustPosition(endCap.DrawRectangle);
                                                        if (movementPoints == 0) { movementButtons[0].Active = true; movementButtons[1].Active = true; zeroMovementPoints = false; }
                                                        currentHeroToken.Movement = thisHeroSheet.MaxMovement;
                                                        currentHeroToken.MovementUsed = 0;
                                                        zeroMovementPoints = false;
                                                        thisHeroSheet.ActionPoints--;
														checkActionPoints(thisHeroSheet);
													}
												}
                                            }
                                            #endregion
                                        }
                                    }
                                    break;
								#endregion
								case HeroActionState.SpecialAction:
                                    currentHeroState = HeroState.SelectActions;
                                    break;
                                #region Rest Action
                                case HeroActionState.RestAction:
                                    if (!loadOnce)
                                    {
                                        thisHeroSheet.Resting = true; loadOnce = true;
                                        thisHeroSheet.ActionPoints--;
                                        messages.Clear();
                                        messages.Add(new Message(thisHeroSheet.Name + " is resting this round", windlassFont23, centerWindowMessage));
                                    }
                                    GeneralMouseClick(mouse);
                                    break;
                                #endregion
                                #region Stand Up Action
                                case HeroActionState.StandUpAction:
                                    if (!loadOnce)
                                    {
                                        Dice hpDie1 = new Dice(1), hpDie2 = new Dice(1);
                                        int totalHPRecovered = 0, totalStaminaRecovered = 0;
                                        hpDie1.RollDie(random.Next(0, 6));
                                        attackDiceList.Add(hpDie1);
                                        totalHPRecovered += hpDie1.Attack; totalStaminaRecovered += hpDie1.Surge;
                                        hpDie2.RollDie(random.Next(0, 6));
                                        attackDiceList.Add(hpDie2);
                                        totalHPRecovered += hpDie2.Attack; totalStaminaRecovered += hpDie2.Surge;
                                        messages.Clear();
                                        messages.Add(new Message(thisHeroSheet.Name + " has recovered " + totalHPRecovered + " hit points & " + totalStaminaRecovered + " stamina", windlassFont23, centerWindowMessage));
                                        thisHeroSheet.ActionPoints -= 2;
                                        loadOnce = true;
                                    }
                                    GeneralMouseClick(mouse);
                                    break;
                                #endregion
                                #region Ability & Skill Action
                                case HeroActionState.PerformArrowAbilitySkillAction:
                                    if (!skillPicked) { messages.Add(new Message("Select a skill to use.", windlassFont23, centerWindowMessage)); }
                                    if (skillNotExhausted)
                                    {
                                        #region Skill Number Check
                                        #region Starting Skill Check (0)
                                        if (skillUsed == 0)
                                        {
                                            if (currentHeroClass.ClassName == "disciple" && skillPicked)
                                            {
                                                DetermineRanges(1, archetype, tokenPosition, "Skill Use");
                                                messages.Add(new Message("Select a target to heal", windlassFont23, centerWindowMessage));
                                            }
                                            else if (currentHeroClass.ClassName == "necromancer")
                                            {
                                                DetermineRanges(1, archetype, tokenPosition, "Skill Use");
                                                messages.Add(new Message("Select a location for the reanimate", windlassFont23, centerWindowMessage));
                                            }
                                            else if (currentHeroClass.ClassName == "thief" && skillPicked)
                                            {
                                                thisHeroSheet.SearchRange = 3;
                                                if (!usedSkillOnce) { changeHpStaminaBar(currentHeroToken, -1, heroNumPosition, "stamina"); usedSkillOnce = true; }
                                                skillUsed = -1; skillPicked = false;
                                                currentHeroActionState = HeroActionState.SearchAction;
                                            }
                                            else if (currentHeroClass.ClassName == "berserker" && skillPicked)
                                            {
                                                totalAttack += 1; changeHpStaminaBar(currentHeroToken, -1, heroNumPosition, "stamina"); skillUsed = -1; skillPicked = false;
                                                currentHeroActionState = HeroActionState.AttackAction; messages.Clear(); loadOnce = true;
                                            }
                                            else if (currentHeroClass.ClassName == "knight" && skillPicked)
                                            {
                                                DetermineRanges(3, archetype, tokenPosition, "Skill Use");
                                                messages.Add(new Message("Select the closest empty location within range\nthat is adjacent to a hero token and to a monster", windlassFont23, centerWindowMessage));
                                            }
                                        }
                                        #endregion
                                        #region Skill 1 used
                                        else if (skillUsed == 1)
                                        {

                                        }
                                        #endregion
                                        #endregion
                                    }
                                    if (mouse.LeftButton == ButtonState.Pressed && leftButtonReleased) { leftClickStarted = true; leftButtonReleased = false; }
                                    else if (mouse.LeftButton == ButtonState.Released)
                                    {
                                        leftButtonReleased = true;
                                        if (leftClickStarted)
                                        {
                                            leftClickStarted = false;
                                            //checkVariousItems(mouse);
                                            #region Skill Card Click Check
                                            foreach (Token skills in currentHeroClass.SkillList) if (skills.DrawRectangle.Contains(mouse.X, mouse.Y))
                                                {

                                                    skillPicked = true;
                                                    skillUsed = skills.Variable;
                                                    if (thisHeroSheet.PickedClass.SkillsExhausted[skillUsed])
                                                    {
                                                        messages.Clear();
                                                        messages.Add(new Message("This skill is exhausted. Pick another or\nselect a new action.", windlassFont23, centerWindowMessage));
                                                        skillNotExhausted = false;
                                                    }
                                                    else
                                                    {
                                                        skillNotExhausted = true;
                                                        messages.Clear();
                                                    }
                                                }
                                            #endregion
                                            #region Hero Token Click Check
                                            foreach (Token hToken in heroTokens)
                                            {
                                                if (hToken.DrawRectangle.Contains(mouse.X, mouse.Y))
                                                {
                                                    switch (skillUsed)
                                                    {
                                                        case 0:
                                                            if (currentHeroClass.ClassName == "disciple")
                                                            {
                                                                thisHeroSheet.PickedClass.SkillsExhausted[0] = true;
                                                                skillPicked = false; changeHpStaminaBar(currentHeroToken, -1, heroNumPosition, "stamina"); messages.Clear();
                                                                Dice healing = new Dice(1);
                                                                RollAttack(healing); attackDiceList.Add(healing);
                                                                messages.Add(new Message(hToken.Name + " has been healed\nfor " + totalAttack + " damage.", windlassFont23, centerWindowMessage));
                                                                hToken.HP += totalAttack; floorHighlights.Clear();
                                                                thisHeroSheet.PickedClass.SkillsExhausted[0] = true;
                                                                thisHeroSheet.ActionPoints--;
                                                                totalAttack = 0; skillUsed = -1; checkActionPoints(thisHeroSheet);
                                                            }
                                                            break;
                                                        default: break;
                                                    }
                                                }
                                            }
                                            #endregion
                                            #region Floor Highlight Click Check
                                            foreach (Token floorLight in floorHighlights)
                                            {
                                                if (floorLight.DrawRectangle.Contains(mouse.X, mouse.Y))
                                                {
                                                    switch (skillUsed)
                                                    {
                                                        case 0:
                                                            if (currentHeroClass.ClassName == "necromancer")
                                                            {
                                                                skillPicked = false; skillUsed = -1;
                                                                changeHpStaminaBar(currentHeroToken, -1, heroNumPosition, "stamina");
                                                                heroTokens.Add(new Token(floorLight.DrawRectangle, 5, 0, 0, new Rectangle(768, 776, 64, 64)));
                                                                Token reanimate = heroTokens[heroTokens.Count - 1];
                                                                reanimate.HP = 4; reanimate.Movement = 3;
                                                                reanimate.DrawRectangle = floorLight.DrawRectangle; 
                                                                reanimate.OriginalDrawRect = reanimate.DrawRectangle;
                                                                reanimate.LocationX = reanimate.DrawRectangle.X + 32;
                                                                reanimate.LocationY = reanimate.DrawRectangle.Y + 32;
                                                                reanimate.OriginalLocation = reanimate.Location;
                                                                reanimate.HalfDrawRectWidth = reanimate.SourceRectangle.Width / 2;
                                                                reanimate.HalfDrawRectHeight = reanimate.SourceRectangle.Height / 2;
                                                                messages.Clear();
                                                                familiarActive = true;
                                                                thisHeroSheet.ActionPoints--;
                                                                checkActionPoints(thisHeroSheet);
                                                            }
                                                            else // Knight Advance
                                                            {
                                                                skillPicked = false;
                                                                currentHeroToken.DrawRectangle = floorLight.DrawRectangle;
                                                                skillUsed = -1; changeHpStaminaBar(currentHeroToken, -1, heroNumPosition, "stamina");
                                                                messages.Clear();
                                                                currentHeroActionState = HeroActionState.AttackAction;
                                                            }
                                                            break;
                                                        default: break;
                                                    }
                                                }
                                            }
                                            #endregion
                                            #region Reset button click
                                            if(movementButtons[0].DrawRectangle.Contains(mouse.X, mouse.Y)){
                                                skillPicked = false; skillNotExhausted = false;
                                                messages.Clear();
                                                checkActionPoints(thisHeroSheet);
                                            }
                                            #endregion
                                        }
                                    }
                                    break;
                                #endregion
                                case HeroActionState.OpenCloseDoorAction:
                                    currentHeroState = HeroState.SelectActions;
                                    break;
                                case HeroActionState.ReviveHeroAction:
                                    currentHeroState = HeroState.SelectActions;
                                    break;

                                default: break;
                            }
                            break;
                        #endregion
                        #region Ending Hero Turn
						case HeroState.EndTurn:
							heroTokenClicked = false;
                            heroSheets[heroNumPosition].ActiveSheet = false;
							if (currentHeroTurn > numHeroTurns)
							{
								currentHeroTurn = 1;
								foreach (HeroSheet hSheet in heroSheets) hSheet.ActionPoints = 2;
								foreach (Token drawToken in drawingTokens) drawToken.Active = true;
								currentHeroState = HeroState.SelectHero;
								currentGameState = GameState.OverlordTurn;
							}
							else currentHeroState = HeroState.SelectHero;

                            break;
                        #endregion
                        default: break;
                    }
                    break;
                #endregion
				#region Game State: Overlord Turn
				case GameState.OverlordTurn:
                    //messages.Add(new Message("Overlord turn coming soon!", windlassFont36, centerWindowMessage));
                    //if (timer > 5000) { currentGameState = GameState.HeroTurn; timer = 0; messages.Clear(); }
                    //else timer += gameTime.ElapsedGameTime.Milliseconds;
                    switch (currentOLState)
                    {
                        case OverlordState.ActiveEffect:
                            messages.Add(new Message("Room activation effects coming soon!", windlassFont36, centerWindowMessage));
                            if (timer > 5000) { currentOLState = OverlordState.Fate; timer = 0; messages.Clear(); }
                            else timer += gameTime.ElapsedGameTime.Milliseconds;
                            break;
                        case OverlordState.Fate:
                            messages.Add(new Message("Fate check coming soon!", windlassFont36, centerWindowMessage));
                            if (timer > 5000) { currentOLState = OverlordState.MonsterActivation; timer = 0; messages.Clear(); }
                            else timer += gameTime.ElapsedGameTime.Milliseconds;
                            break;
                        case OverlordState.MonsterActivation:
                            if (fleshFound)
                            {
                            }
                            if (dragonFound)
                            {
                            }
                            if (zombieFound)
                            {

                            }
                            if (barghestFound)
                            {

                            }
                            messages.Add(new Message("Monster activation coming soon!", windlassFont36, centerWindowMessage));
                            if (timer > 5000) { currentOLState = OverlordState.ActiveEffect; currentGameState = GameState.HeroTurn; timer = 0; messages.Clear(); }
                            else timer += gameTime.ElapsedGameTime.Milliseconds;
                            break;
                    }
					break;
				#endregion
				default: break;
            }

            // Clean out dead monster tokens
            for (int i = monsterTokens.Count - 1; i >= 0; i--) if (!monsterTokens[i].Active) monsterTokens.RemoveAt(i);

            // Update Game items
            if(heroMoving) foreach(Token hToken in heroTokens) if(selectedHeroName == hToken.Name) hToken.Update2(gameTime);
            //foreach(Token mToken in monsterTokens) mToken.Update(gameTime);
            //overlordTrack.Update(gameTime);
            //lootTrack.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Adds the action button and message to the hero page for choosing the next action for the selected hero
        /// </summary>
        /// <param name="actionType">Name of the action to perform</param>
        /// <param name="spriteString">The sprite to represent the action being performed</param>
        /// <param name="number">Number used in multipliers and system to designate an action with a number</param>
        private void addActionMessageButton(string actionType, string spriteString, int number)
        {
            actionMessages.Add(new Message(actionType, windlassFont14, new Vector2(actionChoiceRect.X + GameConstants.ACTION_MESSAGE_X_LEFT, actionChoiceRect.Y + GameConstants.ACTION_MESSAGE_Y_START + (GameConstants.ACTION_MESSAGE_Y_BUFFER * number)), number + 1));
            actionButtons.Add(new Token(actionChoiceRect, number + 1, (int)GameConstants.ACTION_TOKEN_X_LEFT, (int)(GameConstants.ACTION_TOKEN_Y_START + (GameConstants.ACTION_TOKEN_Y_BUFFER * number)), new Rectangle(number * 40, 352, 40, 40)));
        }

        /// <summary>
        /// Add monster information sheets to a list based on what monster tokens are on the board
        /// </summary>
        /// <param name="Content">The content manager</param>
        /// <param name="monsterName">Name of the monster's sheet to add</param>
        private void addMonsterSheet(string monsterName)
        {
            switch (monsterName)
            {
                case "barghest":
                    if (currentAct == 1) monsterSheet.Add(new MonsterSheet(GameConstants.MON_CARD_POS_X, GameConstants.MON_CARD_POS_Y, monsterName, "melee", 1, 4, 4, 4, -1, 0, 1, -1, "Howl", "surgeDmg1", "", "", 4, 6, 4, -1, 0, 1, -1, "Night Stalker", "Howl", "surgeDmg2", ""));
                    else monsterSheet.Add(new MonsterSheet(GameConstants.MON_CARD_POS_X, GameConstants.MON_CARD_POS_Y, monsterName, "melee", 2, 4, 6, 5, -1, 0, 1, -1, "Howl", "surgeDmg2", "", "", 4, 8, 5, -1, 0, 1, 2, "Night Stalker", "Howl", "surgeDmg2", ""));
                    break;
                case "flesh moulder":
                    if (currentAct == 1) monsterSheet.Add(new MonsterSheet(GameConstants.MON_CARD_POS_X, GameConstants.MON_CARD_POS_Y, monsterName, "range", 1, 4, 4, 4, -1, 0, 2, -1, "surgeMend1", "surgeDmg1", "", "", 4, 5, 4, -1, 0, 2, -1, "Heal", "surgeMend2", "surgeDmg2", ""));
                    else monsterSheet.Add(new MonsterSheet(GameConstants.MON_CARD_POS_X, GameConstants.MON_CARD_POS_Y, monsterName, "range", 2, 4, 5, 4, 3, 0, 2, -1, "surgeMend2", "surgeDmg2", "", "", 4, 7, 4, 3, 0, 2, 2, "Heal", "surgeMend3", "surgeDmg3", ""));
                    break;
                case "shadow dragon":
                    if (currentAct == 1) monsterSheet.Add(new MonsterSheet(GameConstants.MON_CARD_POS_X, GameConstants.MON_CARD_POS_Y, monsterName, "melee", 1, 3, 6, 4, 4, 0, 1, -1, "Shadow", "surgeDmg1", "", "", 3, 9, 4, 4, 0, 1, -1, "Shadow", "surgeFireBreath", "surgeDmg2", ""));
                    else monsterSheet.Add(new MonsterSheet(GameConstants.MON_CARD_POS_X, GameConstants.MON_CARD_POS_Y, monsterName, "melee", 2, 3, 8, 5, 4, 0, 1, 1, "Shadow", "surgeDmg2", "", "", 3, 10, 5, 4, 0, 1, 1, "Shadow", "surgeFireBreath", "surgeDmg3", ""));
                    break;
                case "zombie":
                    if (currentAct == 1) monsterSheet.Add(new MonsterSheet(GameConstants.MON_CARD_POS_X, GameConstants.MON_CARD_POS_Y, monsterName, "melee", 1, 3, 3, 3, -1, 0, 2, -1, "Shambling", "surgeDisease", "surgeDmg1", "", 3, 6, 3, -1, 0, 2, -1, "Shambling", "Grab", "surgeDisease", "surgeDmg1"));
                    else monsterSheet.Add(new MonsterSheet(GameConstants.MON_CARD_POS_X, GameConstants.MON_CARD_POS_Y, monsterName, "melee", 2, 3, 5, 3, -1, 0, 2, -1, "Shambling", "surgeDisease", "surgeDmg2", "", 3, 9, 3, -1, 0, 1, 2, "Shambling", "Grab", "surgeDisease", "surgeDmg2"));
                    break;
                default: break;
            }
        }

        /// <summary>
        /// Determines what surges are available for the current hero to use
        /// </summary>
        /// <param name="equipment">Weapon being used with surge options</param>
        /// <param name="heroSheet">Used to determine if specific hero sheets are active for extra surge uses</param>
        /// <param name="heroToken">The hero token using the surge</param>
        private void addSurgeUsage(Equipment equipment, HeroSheet heroSheet, Token heroToken)
        {
            surgeList.Add("Restore 1 Stamina");
            if (heroSheet.MaxHP != heroToken.HP) foreach (Token hToken in heroTokens) if (hToken.Name == "Avric") { int avricRange = DetermineDistanceLong(heroToken, hToken); if (avricRange <= 3) surgeList.Add("Restore 1 Damage"); }
            if (heroSheet.PickedClass.ClassName == "runemaster") surgeList.Add("Suffer 1 Stamina to\ngain +2 Damage");
            string surge1 = equipment.Surge1, surge2 = equipment.Surge2, surge3 = equipment.Surge3;
            surgeList.Add(surge1 = GetSurgeName(surge1));
            if (surge2 != "") surgeList.Add(surge2 = GetSurgeName(surge2));
            if (surge3 != "") surgeList.Add(surge3 = GetSurgeName(surge3));
            for (int x = 0; x < surgeList.Count; x++)
                surgeListRect.Add(new Token(actionChoiceRect, x + 1, (int)(GameConstants.ACTION_MESSAGE_X_LEFT) - 35, (int)(GameConstants.ACTION_TOKEN_Y_START + (GameConstants.ACTION_TOKEN_Y_BUFFER * x) + 11), new Rectangle(0, 884, 250, 23)));
        }

        // TODO: Create code for the Surge: Blast
        private void blastSurge(Token targetToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Changes the amount of the heroes stamina based on using amount and updates the stamina bar's width
        /// </summary>
        /// <param name="heroToken">The affected hero token</param>
        /// <param name="changingAmount">The amount changing</param>
        /// <param name="heroNumPosition">The current hero number position</param>
        /// <pparam name="barToChange">Use either "hp" or "stamina" to affect those bars</pparam>
        private void changeHpStaminaBar(Token heroToken, int changingAmount, int heroNumPosition, string barToChange)
        {
            HeroSheet hero = heroSheets[heroNumPosition];
            float barWidth = GameConstants.HP_STAM_MAX_WIDTH, percentChange;
            if (barToChange == "hp")
            {
                hero.Health += changingAmount;
                percentChange = (float)hero.Health / (float)hero.MaxHP;
                barWidth *= percentChange;
                heroToken.HP = hero.Health;
                hpBars[heroNumPosition].drawRectWidth = (int)barWidth;
            }
            else // change Stamina
            { 
                hero.Stamina += changingAmount;
                percentChange = (float)hero.Stamina / (float)hero.MaxStam;
                barWidth *= percentChange;
                heroToken.Stam = hero.Stamina;
                stamBars[heroNumPosition].drawRectWidth = (int)barWidth;
            } 
        }

        /// <summary>
        /// Checks to see the remaining action points for the current hero
        /// </summary>
        /// <param name="heroSheet">The number of remaining points</param>
        private void checkActionPoints(HeroSheet heroSheet)
        {
            if (heroSheet.PickedClass.ClassName == "necromancer" && heroSheet.ActionPoints == 2 && familiarActive) { currentHeroState = HeroState.RefreshCards; choosingAction = true; }
            else
            {
                choosingAction = true; usedSkillOnce = false;
                currentHeroActionState = HeroActionState.ChooseAction;
                totalAttack = 0; totalDefense = 0; totalRange = 0; totalSurge = 0;
                if (heroSheet.ActionPoints != 0) { currentHeroState = HeroState.SelectActions; }
                else
                {
                    // Checks to see if the hero used a Rest Action at any time 
                    if (heroSheet.Resting) changeHpStaminaBar(heroTokens[heroNumPosition], heroSheet.MaxStam, heroNumPosition, "stamina");
                    // Checks if the current class is Necromancer and if the reanimate did not act yet
                    if (!familiarActed && heroSheet.PickedClass.ClassName == "necromancer") currentHeroState = HeroState.FamiliarActions;
                    else { ResetFamiliar(); currentHeroTurn++; currentHeroState = HeroState.EndTurn; }
                }
            }
        }

        /// <summary>
        /// Various items to be checked: Monster Info, Loot Track
        /// </summary>
        /// <param name="mouse">Mouse state for click checks</param>
        private void checkVariousItems(MouseState mouse)
        {
            // Checks the loot track
            foreach (Token lootButton in lootOLButtons)
                if (lootButton.DrawRectangle.Contains(mouse.X, mouse.Y))
                {
                    if (lootButton.Variable == 0)
                    {
                        if (lootTrack.Active) lootTrack.Active = false;
                        else lootTrack.Active = true;
                    }
                    else
                    {
                        if (overlordTrack.Active) overlordTrack.Active = false;
                        else overlordTrack.Active = true;
                    }
                }
            // Check to see if a monster token was clicked to display information about the monster
            foreach (Token mToken in monsterTokens)
            {
                if (mToken.DrawRectangle.Contains(mouse.X, mouse.Y))
                {
                    if (heroTokenClicked) heroTokenClicked = false;
                    monsterTokenClicked = true;
                    foreach (MonsterSheet mSheet in monsterSheet) { if (mSheet.Name == mToken.Name) { mSheet.Active = true; break; } }
                }
            }
            // Checks for active monster sheets and removes them if sheet is clicked again
            foreach (MonsterSheet mSheet in monsterSheet)
            {
                if (mSheet.Active)
                {
                    if (mSheet.DrawRectangle.Contains(mouse.X, mouse.Y))
                    {
                        monsterTokenClicked = false;
                        mSheet.Active = false;
                        mAlphaValue = 1;
                    }
                }
            }
        }

        /// <summary>
        /// Checks the used weapon for any bonuses
        /// </summary>
        /// <param name="monsterToken">Monster being affected by the attack</param>
        /// <param name="weaponUsed">The weapon being used to check for bonuses</param>
        private void checkWeaponBonus(Token monsterToken, Equipment weaponUsed)
        {
            //if (attackHit && thisHeroSheet.PickedClass.ClassName == "thief") 
            //{
            //    if (thisHeroSheet.PickedClass.CurrentWeapon.Bonus1 == "adjacent1dmg" || thisHeroSheet.PickedClass.OffHand.Bonus1 == "adjacent1dmg")
            //    {
            //        DetermineRanges(1, "scout", heroTokens[heroNumPosition].DrawRectangle, "");
            //        foreach (Token floorLight in floorHighlights) if (floorLight.DrawRectangle.Intersects(monsterToken.DrawRectangle))
            //            {
            //                totalAttack += 1;
            //                messages.Add(new Message("+1 Damage has been added from\nthe throwing knives bonus.", windlassFont14, new Vector2(centerWindowMessage.X, centerWindowMessage.Y + 60)));
            //            }
            //        floorHighlights.Clear();
            //    }
            //}
            if (weaponUsed.Bonus1 != "")
            {
                switch (weaponUsed.Bonus1)
                {
                    case "adjacent1dmg":
                        DetermineRanges(1, "scout", heroTokens[heroNumPosition].DrawRectangle, "");
                        foreach (Token floorLight in floorHighlights) if (floorLight.DrawRectangle.Intersects(monsterToken.DrawRectangle))
                            {
                                totalAttack += 1;
                                messages.Add(new Message("+1 Damage has been added from the throwing knives bonus.", windlassFont14, new Vector2(centerWindowMessage.X, centerWindowMessage.Y + 60)));
                            }
                        floorHighlights.Clear();
                        break;
                    case "heal1":
                        if (monsterToken.HP < 1) changeHpStaminaBar(heroTokens[heroNumPosition], 1, heroNumPosition, "hp");
                        break;
                    default: break;
                }
                messages.Add(new Message(weaponUsed.Bonus1, windlassFont14, new Vector2(centerWindowMessage.X, centerWindowMessage.Y + 75)));
            }
            if (weaponUsed.Bonus2 != "") messages.Add(new Message(weaponUsed.Bonus1, windlassFont14, new Vector2(centerWindowMessage.X, centerWindowMessage.Y + 85)));
        }

        /// <summary>
        /// Clears all dice out after damage has been dealt
        /// </summary>
        private void ClearDiceRolls()
        {
            totalAttack = 0; totalDefense = 0; totalRange = 0; totalSurge = 0; attackDiceList.Clear(); defenseDiceList.Clear(); surgeList.Clear(); surgeListRect.Clear();
            attackDice1 = null; attackDice2 = null; attackDice3 = null; defenseDice1 = null; defenseDice2 = null;
        }

        /// <summary>
        /// Determines the farthest spaces a target token is from the current token
        /// </summary>
        /// <param name="currentToken">The token doing the action</param>
        /// <param name="targetToken">The target token</param>
        /// <returns>Returns the greater number of spaces for either X or Y distance</returns>
        private int DetermineDistanceLong(Token currentToken, Token targetToken)
        {
            if (targetToken != null)
            {
                int xDis = currentToken.DrawRectangle.X - targetToken.DrawRectangle.X, yDis = currentToken.DrawRectangle.Y - targetToken.DrawRectangle.Y;
                if (xDis < 0) xDis *= -1;
                if (yDis < 0) yDis *= -1;
                xDis /= 64; yDis /= 64;
                if (xDis > yDis) return xDis;
                else return yDis;
            }
            else return 0;
        }

        /// <summary>
        /// Determines the shortest spaces a target token is from the current token
        /// </summary>
        /// <param name="currentToken">The token doing the action</param>
        /// <param name="targetToken">The target token</param>
        /// <returns>Returns the smaller number of spaces for either X or Y distance</returns>
        private int DetermineDistanceShort(Token currentToken, Token targetToken)
        {
            int xDis = currentToken.DrawRectangle.X - targetToken.DrawRectangle.X, yDis = currentToken.DrawRectangle.Y - targetToken.DrawRectangle.Y;
            if (xDis < 0) xDis *= -1;
            if (yDis < 0) yDis *= -1;
            xDis /= 64; yDis /= 64;
            if (xDis < yDis) return xDis;
            else return yDis;
        }

        /// <summary>
        /// Determines the Line of Sight from one token to another for various reasons
        /// Author: Steve Register [arns@arns.freeservers.com].txt (Source: http://www.roguebasin.com/index.php?title=Simple_Line_of_Sight)
        /// </summary>
        /// <param name="currentToken">The token that needs to find LoS</param>
        /// <param name="targetToken">The token being searched for</param>
        /// <returns>True: LoS found, False: No LoS</returns>
        private bool DetermineLineOfSight(int currentX, int currentY, int targetX, int targetY)
        {
            int t, x, y, absDeltaX, absDeltaY, signX, signY, deltaX, deltaY;
            deltaX = currentX - targetX;
            deltaY = currentY - targetY;
            absDeltaX = Math.Abs(deltaX);
            absDeltaY = Math.Abs(deltaY);
            signX = Math.Sign(deltaX);
            signY = Math.Sign(deltaY);
            x = targetX;
            y = targetY;

            if (absDeltaX > absDeltaY)
            {
                t = absDeltaY * 2 - absDeltaX;
                do
                {
                    if (t >= 0)
                    {
                        y += signY;
                        t -= absDeltaX * 2;
                    }
                    x += signX;
                    t += absDeltaY * 2;
                    if (x == currentX && y == currentY) return true;
                    // TODO: REMOVE DOTS after testing
                    dots.Add(new Token(new Rectangle(768, 0, 1, 1), new Rectangle(x, y, 1, 1)));
                } while (SightBlocked(x, y, currentX, currentY) == false);
            }
            else
            {
                t = absDeltaX * 2 - absDeltaY;
                do
                {
                    if (t >= 0)
                    {
                        x += signX;
                        t -= absDeltaY * 2;
                    }
                    y += signY;
                    t += absDeltaX * 2;
                    if (x == currentX && y == currentY) return true;
                    // TODO: REMOVE DOTS after testing
                    dots.Add(new Token(new Rectangle(768, 0, 1, 1), new Rectangle(x, y, 1, 1)));
                } while (SightBlocked(x, y, currentX, currentY) == false);
            }
            return false;
        }

        /// <summary>
        /// Determines the movement range of the selected hero based off the sheets given movement points during construction
        /// </summary>
        /// <param name="rangePoints">Number of movement points the hero has available at the start of a movement action</param>
        /// <param name="archetype">Archetype of the selected hero for floor highlight coloring</param>
        /// <param name="tokenPosition">Current position of the token's draw rectangle</param>
        public void DetermineRanges(int rangePoints, string archetype, Rectangle tokenPosition, string type)
        {
            Rectangle archeSourceRect;
            if (archetype == "healer") archeSourceRect = new Rectangle(512, 0, 64, 64);
            else if (archetype == "mage") archeSourceRect = new Rectangle(576, 0, 64, 64);
            else if (archetype == "scout") archeSourceRect = new Rectangle(640, 0, 64, 64);
            else if (archetype == "warrior") archeSourceRect = new Rectangle(704, 0, 64, 64);
            else archeSourceRect = new Rectangle(768, 0, 64, 64);

            int startX = tokenPosition.X + ((64 * rangePoints) * -1) - 64, startY = (64 * rangePoints) * -1;
            tokenPosition.X = startX; tokenPosition.Y += startY;

            floorHighlights.Clear();
            if (rangePoints > 0)
            {
                rangePoints = (rangePoints * 2) + 1;
                for (int y = 0; y < rangePoints; y++)
                {
                    for (int x = 0; x < rangePoints; x++)
                    {
                        tokenPosition.X += GameConstants.GRID_SIZE;
                        floorHighlights.Add(new Token(archeSourceRect, tokenPosition));
                        messages.Add(new Message("x: " + floorHighlights[floorHighlights.Count - 1].DrawRectangle.Center.X + "\ny: " + floorHighlights[floorHighlights.Count - 1].DrawRectangle.Center.Y, windlassFont6,
                            new Vector2(floorHighlights[floorHighlights.Count - 1].DrawRectangle.X, floorHighlights[floorHighlights.Count - 1].DrawRectangle.Y), x));
                    }
                    tokenPosition.Y += GameConstants.GRID_SIZE;
                    tokenPosition.X = startX;
                }
                foreach (Token fHL in floorHighlights)
                {
                    fHL.Active = false;
                    foreach (Tile floorTile in tiles) { if (floorTile.DrawRectangle.Intersects(fHL.DrawRectangle)) fHL.Active = true; }
                    foreach (Tile endCap in endCaps) { if (endCap.DrawRectangle.Intersects(fHL.DrawRectangle)) fHL.Active = true; }
                    foreach (Token mToken in monsterTokens) { if (mToken.DrawRectangle.Intersects(fHL.DrawRectangle)) fHL.Occupied = true; }
                    foreach (Token hToken in heroTokens) { if (hToken.DrawRectangle.Intersects(fHL.DrawRectangle)) fHL.Occupied = true; }
                }
            }
        }

        /// <summary>
        /// A general mouse click to end the current action
        /// </summary>
        /// <param name="mouse">The current mouse state</param>
        private void GeneralMouseClick(MouseState mouse)
        {
            if (mouse.LeftButton == ButtonState.Pressed && leftButtonReleased) { leftClickStarted = true; leftButtonReleased = false; }
            else if (mouse.LeftButton == ButtonState.Released)
            {
                leftButtonReleased = true;
                if (leftClickStarted)
                {
                    leftClickStarted = false;
                    messages.Clear();
                    if (loadOnce) loadOnce = false;
                    checkActionPoints(heroSheets[heroNumPosition]);
                }
            }
        }

        /// <summary>
        /// A private method used to get the archetype string equivalent of the variable indicated
        /// </summary>
        /// <returns>Returns a string based on the saved varioble: Healer/Mage/Scout/Warrior</returns>
        private string GetArchetypeName()
        {
            int tempArchetype = creatingHeroNumber - 1;
            string className = "";
            if (chosenHeroArchtype[tempArchetype].Equals(1)) className = "Healer";
            else if (chosenHeroArchtype[tempArchetype].Equals(2)) className = "Mage";
            else if (chosenHeroArchtype[tempArchetype].Equals(3)) className = "Scout";
            else if (chosenHeroArchtype[tempArchetype].Equals(4)) className = "Warrior";
            return className;
        }

        /// <summary>
        /// Used to get the class string equivalent of the passed number
        /// </summary>
        /// <param name="classNumber">Number referencing the class name</param>
        /// <returns>Returns the string name equivalent</returns>
        private string GetClassName(int classNumber)
        {
            switch (classNumber - 1)
            {
                case 0: return "disciple";
                case 1: return "spirit speaker";
                case 2: return "necromancer";
                case 3: return "runemaster";
                case 4: return "thief";
                case 5: return "wildlander";
                case 6: return "berserker";
                case 7: return "knight";
                default: return "";
            }
        }

        private Texture2D GetClassSheet(int classNumber)
        {
            switch (classNumber - 1)
            {
                case 0:
                case 1:
                case 2: return Content.Load<Texture2D>("Classes/class sheet 1");
                case 3:
                case 4:
                case 5: return Content.Load<Texture2D>("Classes/class sheet 2");
                case 6:
                case 7: return Content.Load<Texture2D>("Classes/class sheet 3");
                default: return Content.Load<Texture2D>("Classes/class sheet 1");
            }
        }

        /// <summary>
        /// Selected action number is sent in to get the correct action state for the selected hero
        /// </summary>
        /// <param name="selectedActionNumber">Designated number representing the hero's action state</param>
        private void GetHeroActionState(int selectedActionNumber)
        {
            switch (selectedActionNumber)
            {
                case 1: currentHeroActionState = HeroActionState.AttackAction; break;
                case 2: currentHeroActionState = HeroActionState.SearchAction; break;
                case 3: currentHeroActionState = HeroActionState.MoveAction; break;
                case 4: currentHeroActionState = HeroActionState.SpecialAction; break;
                case 5: currentHeroActionState = HeroActionState.RestAction; break;
                case 6: currentHeroActionState = HeroActionState.StandUpAction; break;
                case 7: currentHeroActionState = HeroActionState.PerformArrowAbilitySkillAction; break;
                case 8: currentHeroActionState = HeroActionState.OpenCloseDoorAction; break;
                case 9: currentHeroActionState = HeroActionState.ReviveHeroAction; break;
                default: break;
            }
        }

        /// <summary>
        /// Converts basic surge name into longer string
        /// </summary>
        /// <param name="surge">Shortened surge name</param>
        /// <returns>Full text of surge name</returns>
        private string GetSurgeName(string surge)
        {
            switch (surge)
            {
                case "1dmg": return "+1 Damage";
                case "2dmg": return "+2 Damage";
                case "5dmg": return "+5 Damage";
                case "1dmg1rng": return "+1 Damage, +1 Range";
                case "1range": return "+1 Range";
                case "2range": return "+2 Range";
                case "pierce1": return "Pierce 1";
                case "pierce2": return "Pierce 2";
                case "stun": return "Stun";
                case "blast": return "Blast";
                case "imm": return "Immobilize";
                case "rerollDef": return "Force target to\nreroll 1 defense die";
                case "1dmg1push": return "+1 Damage and move\ntarget 1 space";
                case "1dmg>1-3sp": return "Deal 1 Damamge to another\nmonster within 3 spaces";
                case "adj1": return "Each monster adjacent to\nyou suffers 1 Damage";
                case "recover1": return "Recover 1 Damage";

                default: return "Surge Name Invalid";
            }
        }

        /// <summary>
        /// Resets all of the familiar bool fields for the next round
        /// </summary>
        private void ResetFamiliar()
        {
            familiarActed = false; familiarAction = "";
            familiarAttacked = false; familiarMoved = false; familiarChoosing = true; calcAttack = true;
            familiarActionSheetOn = true;
        }

		private void RollAttack(Dice attackDie)
		{
            attackDie.Range = 0; attackDie.Attack = 0; attackDie.Surge = 0;
			attackDie.RollDie(random.Next(0, 6));
			attackDiceList.Add(attackDie);
			if (attackDie.DieColor == 0 && attackDie.Range == 0) attackHit = false;
            if (attackHit)
            {
                totalRange += attackDie.Range; totalAttack += attackDie.Attack; totalSurge += attackDie.Surge;
                //attackDie.Range = 0; attackDie.Attack = 0; attackDie.Surge = 0;
            }
		}

		private void RollDefense(Dice defenseDie)
		{
			defenseDie.RollDie(random.Next(0, 6));
			defenseDiceList.Add(defenseDie);
			totalDefense += defenseDie.Defense;
            defenseDie.Defense = 0;
		}

        /// <summary>
        /// Checks to see if the current points X/Y are contained withine a specific point
        /// </summary>
        /// <param name="x">X location of the point</param>
        /// <param name="y">Y location of the point</param>
        /// <returns>
        ///     If the point's in a hero: true
        ///     If the point's in a monster: true
        ///     If it follows the board, false (What we want to find for clear LoS)
        /// </returns>
        private bool SightBlocked(int x, int y, int curX, int curY)
        {
            foreach (Token hToken in heroTokens) if (hToken.DrawRectangle.Contains(new Point(x, y)) && curX != x && curY != y) return true;
            foreach (Token mToken in monsterTokens) if (mToken.DrawRectangle.Contains(new Point(x, y))) return true;
            foreach (Tile tile in tiles) if (tile.DrawRectangle.Contains(new Point(x, y))) return false;

            // Default, if none of the above trigger a return, which they all should.
            return false;
        }

        /// <summary>
        /// Uses a selected surge from a list of available surges
        /// </summary>
        /// <param name="surgeName">Name of the clicked surge to use</param>
        /// <param name="targetToken">Target token being affected by the selected surge</param>
		private void UseSurge(string surgeName, Token targetToken)
		{
			switch (surgeName)
			{
                case "+1 Damage": totalAttack += 1; break;
                case "+2 Damage": totalAttack += 2; break;
                case "+5 Damage": totalAttack += 5; break;
                case "+1 Damage, +1 Range": totalAttack += 1; totalRange += 1; break;
                case "+1 Range": totalRange += 1; break;
                case "+2 Range": totalRange += 2; break;
                case "Pierce 1": totalDefense -= 1; break;
                case "Pierce 2": totalDefense -= 2; break;
                case "Poison": targetToken.Conditions[0] = true; break;
                case "Disease": targetToken.Conditions[1] = true; break;
                case "Stun":
                    if (totalAttack - totalDefense > 0) { targetToken.Conditions[2] = true; messages.Add(new Message(targetToken.Name+" is stunned", windlassFont14, new Vector2(centerWindowMessage.X, centerWindowMessage.Y + 30))); }
                    else messages.Add(new Message("You need at least 1 damage to use Stun", windlassFont14, new Vector2(centerWindowMessage.X, centerWindowMessage.Y + 30)));
                    break;
                case "Immobilize": targetToken.Conditions[3] = true; break;
                case "Blast": blastSurge(targetToken); break;
                case "Force target to\nreroll 1 defense die": break;
                case "+1 Damage and move\ntarget 1 space": break;
                case "Deal 1 Damamge to another\nmonster within 3 spaces": break;
                case "Each monster adjacent to\nyou suffers 1 Damage": break;
                case "Recover 1 Damage": changeHpStaminaBar(heroTokens[heroNumPosition], 1, heroNumPosition, "hp"); break;
                case "Restore 1 Stamina": changeHpStaminaBar(heroTokens[heroNumPosition], 1, heroNumPosition, "stamina"); break;
                case "Suffer 1 Stamina to\ngain +2 Damage": changeHpStaminaBar(heroTokens[heroNumPosition], -1, heroNumPosition, "stamina"); totalAttack += 2; break;

				default: break;
			}
		}

        /// <summary>
        /// Checks the monster token defeated and updates the loot track accordingly. Masters award +1 shop card and the heart track adds base off the monster base. The ratio is a 1:1 of spaces used, E.G.: Barghest 1x2 spaces taken, awards 2 heart tokens to the track.
        /// </summary>
        /// <param name="monToken">The monster token to be checked</param>
        private void UpdateLootTrackWindow(Token monToken)
        {
            int xPosition = GameConstants.HP_TOKEN_START_X + lootTrack.DrawRectangle.Height, yPosition = GameConstants.HP_TOKEN_START_Y + lootTrack.DrawRectangle.Height;

            if (monToken.IsMaster) { masterMonsterKillToken.DrawRectangleY += GameConstants.STAM_TOKEN_BUFFER_Y; masterKillCount++; }
            if (monToken.Name == "zombie" || monToken.Name == "flesh moulder") lootTrackTokens.Add(new Token(lootTrack.DrawRectangle, lootTrackTokens.Count + 1, GameConstants.HP_TOKEN_START_X, yPosition + (GameConstants.HP_TOKEN_BUFFER_Y * lootTrackTokens.Count), hpTokenSource));
            else if (monToken.Name == "barghest")
                for(int x=0; x<2; x++)
                    lootTrackTokens.Add(new Token(lootTrack.DrawRectangle, lootTrackTokens.Count + 1, GameConstants.HP_TOKEN_START_X, yPosition + (GameConstants.HP_TOKEN_BUFFER_Y * lootTrackTokens.Count), hpTokenSource));
            else if(monToken.Name == "shadow dragon")
                for(int x=0; x<6; x++)
                    lootTrackTokens.Add(new Token(lootTrack.DrawRectangle, lootTrackTokens.Count + 1, GameConstants.HP_TOKEN_START_X, yPosition + (GameConstants.HP_TOKEN_BUFFER_Y * lootTrackTokens.Count), hpTokenSource));
            if (lootTrackTokens.Count > 3)
            {
                if (lootTrackTokens.Count >= 4 && numHeroesPlaying == 2) AwardShopCard(heroSheets[heroNumPosition]);
                else if (lootTrackTokens.Count >= 5 && numHeroesPlaying == 3) AwardShopCard(heroSheets[heroNumPosition]);
                else if (lootTrackTokens.Count >= 6 && numHeroesPlaying == 4) AwardShopCard(heroSheets[heroNumPosition]);
            }
        }

        /// <summary>
        /// Awards a shop card to a hero when a specific number of kills have been made
        /// </summary>
        /// <param name="hero">The hero being awarded the gear</param>
        private void AwardShopCard(HeroSheet hero)
        {
            awardingLoot = true;
            messages.Clear();
            messages.Add(new Message("You have been awarded treasure!", windlassFont23, centerWindowMessage));
            messages.Add(new Message("Choose only one item as your reward.", windlassFont14, new Vector2 (centerWindowMessage.X, centerWindowMessage.Y + 30)));
            for (int x = 0; x < masterKillCount; x++)
            {
                Rectangle drawRect;
                if (currentAct == 1) awardedShopCards.Add(shop1Deck.pullShopCard(hero));
                else awardedShopCards.Add(shop2Deck.pullShopCard(hero));
                if(x < 5) drawRect =  new Rectangle(creationRec.X + 50 + (x * 138), creationRec.Y + 200, 128, 192);
                else drawRect = new Rectangle(creationRec.X + 50 + ((x - 5) * 138), creationRec.Y + 402, 128, 192);
                awardedShopCards[awardedShopCards.Count - 1].DrawRectangle = drawRect;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            MouseState mouse = Mouse.GetState();

            spriteBatch.Begin();

            #region Main Game Components
            bgFill.Draw(spriteBatch);
            foreach (Tile floorTile in tiles) floorTile.Draw(spriteBatch);
            foreach (Tile endCap in endCaps) endCap.Draw(spriteBatch);
            foreach (Tile door in doors) door.Draw(spriteBatch);
            foreach (HeroSheet hSheet in heroSheets)
                if (hSheet.ActiveSheet)
                {
                    string theName = hSheet.Name;
                    if (theName == "Avric" || theName == "Ashrian" || theName == "Leoric" || theName == "Tarha") { hSheet.Draw(spriteBatch, heroSheet1, classSheet1); }
                    else hSheet.Draw(spriteBatch, heroSheet2, classSheet2);                    
                    bar1BG.Draw(spriteBatch, spriteSheet1); bar2BG.Draw(spriteBatch, spriteSheet1);
                    int numPos = heroTokens[heroNumPosition].Variable - 1;
                    hpBars[numPos].Draw(spriteBatch, spriteSheet1);
                    stamBars[numPos].Draw(spriteBatch, spriteSheet1);
                }
            #endregion

            #region Draw Character Creation
            if (currentGameState == GameState.CharacterCreation) 
            {
                monsterOverlay.Draw(spriteBatch, mAlphaValue);
                spriteBatch.Draw(creationBG, creationRec, Color.White);
                if (creationStep == 1) foreach (Token numPlayers in numOfPlayerTokens) numPlayers.Draw(spriteBatch, spriteSheet1);
                if (creationStep == 2) foreach (Token chosenArchetype in chooseArchetype) chosenArchetype.Draw(spriteBatch, spriteSheet1);
                if (creationStep == 3) 
                    foreach (Token chosenHero in chooseHero)
                    {
                        if (chosenHero.Variable < 5) chosenHero.Draw(spriteBatch, heroSheet1);
                        else chosenHero.Draw(spriteBatch, heroSheet2);
                    }
                if (creationStep == 4) foreach (Token chosenClass in chooseClass) chosenClass.Draw(spriteBatch, spriteSheet1);
                if (finishedSelectingHeroes)
                {
                    foreach (HeroSheet hSheet in heroSheets)
                    {
                        string className = hSheet.PickedClass.ClassName;
                        foreach (Token skillCard in hSheet.PickedClass.AllSkillCards) 
                            if (skillCard.Active)
                            {
                                if (className == "disciple" || className == "spirit speaker" || className == "necromancer" || className == "runemaster") { if (skillCard.Active) skillCard.Draw(spriteBatch, classSheet1); }
                                else { if (skillCard.Active)skillCard.Draw(spriteBatch, classSheet2); }
                            }
                    }
                    endToken.Draw(spriteBatch, spriteSheet1);
                }
            }
            if (currentGameState == GameState.StartingPositions)
            {
                foreach (Token floorLight in floorHighlights) floorLight.Draw(spriteBatch, spriteSheet1);
            }
            #endregion

            #region Draw Hero Turn Items
            if (currentGameState == GameState.HeroTurn)
            {
                if (!heroTokenClicked) foreach (Token drawToken in drawingTokens) if (drawToken.Active) drawToken.Draw(spriteBatch, spriteSheet1);
                if (currentHeroState == HeroState.StartTurnAbility && familiarActive) foreach (Token yesNo in yesNoList) yesNo.Draw(spriteBatch, spriteSheet1);
                if (heroTokenClicked && choosingAction)
                {
                    spriteBatch.Draw(actionChoiceSprite, actionChoiceRect, Color.White);
                    foreach (Token actionButton in actionButtons) actionButton.Draw(spriteBatch, spriteSheet1);
                    foreach (Message actionMessage in actionMessages) actionMessage.Draw(spriteBatch);
                }
                if (currentHeroState == HeroState.FamiliarActions)
                {
                    if (familiarActionSheetOn)
                    {
                        spriteBatch.Draw(actionChoiceSprite, actionChoiceRect, Color.White);
                        foreach (Token actionButton in actionButtons) if (actionButton.Active) actionButton.Draw(spriteBatch, spriteSheet1);
                        foreach (Message actionMessage in actionMessages) actionMessage.Draw(spriteBatch);
                    }
                    if(!familiarActionSheetOn || heroMoving && !familiarChoosing)
                    {
                        foreach (Dice attackDie in attackDiceList)
                        {
                            if (attackDie.DieColor == 0) attackDie.Draw(spriteBatch, 0, spriteSheet1);
                            else attackDie.Draw(spriteBatch, 1, spriteSheet1);
                        }
                        foreach (Dice defDie in defenseDiceList) defDie.Draw(spriteBatch, 0, spriteSheet1);
                        endToken.Draw(spriteBatch, spriteSheet1);
                    }
                }
				if (currentHeroActionState == HeroActionState.MoveAction)
				{
                    foreach (Token floorLight in floorHighlights) if (floorLight.Active) floorLight.Draw(spriteBatch, spriteSheet1);
                    if (heroMoving || zeroMovementPoints) foreach (Token moveButton in movementButtons) if (moveButton.Active) moveButton.Draw(spriteBatch, spriteSheet1);
				}
				else if (currentHeroActionState == HeroActionState.SearchAction || currentHeroActionState == HeroActionState.AttackAction ||
                         currentHeroActionState == HeroActionState.PerformArrowAbilitySkillAction || currentHeroState == HeroState.FamiliarActions && familiarChoosing) {
                             foreach (Token floorLight in floorHighlights) if (floorLight.Active) floorLight.Draw(spriteBatch, spriteSheet1); 
                }
				if(currentHeroActionState == HeroActionState.AttackAction){
                    for (int x = 0; x < 6; x++)
                    {
                        spriteBatch.Draw(spriteSheet1, new Rectangle(200 + (40 * x), 200, 32, 32), heroSheets[heroNumPosition].PickedClass.MainDie1.DiceSides[x], Color.White);
                        spriteBatch.Draw(spriteSheet1, new Rectangle(200 + (40 * x), 250, 32, 32), heroSheets[heroNumPosition].PickedClass.MainDie2.DiceSides[x], Color.White);
                    }
					foreach (Dice attackDie in attackDiceList)
					{
						if (attackDie.DieColor == 0) attackDie.Draw(spriteBatch, 0, spriteSheet1);
                        else attackDie.Draw(spriteBatch, 1, spriteSheet1);
					}
                    foreach (Dice defDie in defenseDiceList) defDie.Draw(spriteBatch, 0, spriteSheet1);
					if (hasSurges)
					{
						spriteBatch.Draw(actionChoiceSprite, actionChoiceRect, Color.White);
						for (int x = 0; x < surgeList.Count; x++) spriteBatch.DrawString(windlassFont14, surgeList[x], new Vector2(actionChoiceRect.X + GameConstants.ACTION_MESSAGE_X_LEFT, actionChoiceRect.Y + GameConstants.ACTION_MESSAGE_Y_START + (GameConstants.ACTION_MESSAGE_Y_BUFFER * x)), Color.White);
						foreach (Token surge in surgeListRect) surge.Draw(spriteBatch, spriteSheet1);
					}
                    endToken.Draw(spriteBatch, spriteSheet1);
				}
                if (currentHeroActionState == HeroActionState.RestAction)
                {

                }
                if (currentHeroActionState == HeroActionState.StandUpAction && attackDiceList.Count > 0)
                {
                    attackDiceList[0].Draw(spriteBatch, 0, spriteSheet1);
                    attackDiceList[1].Draw(spriteBatch, 1, spriteSheet1);
                }
                if (currentHeroActionState == HeroActionState.PerformArrowAbilitySkillAction)
                {
                    foreach (Dice dieRoll in attackDiceList) dieRoll.Draw(spriteBatch, 0, spriteSheet1);
                    movementButtons[0].Draw(spriteBatch, spriteSheet1);
                }
            }
            #endregion

            #region Game Board Tokens: Heros, monsters, search, objectives
            foreach (Token sToken in searchTokens) sToken.Draw(spriteBatch, spriteSheet1);
            foreach (Token mToken in monsterTokens) mToken.Draw(spriteBatch, spriteSheet1);
            foreach (Token hToken in heroTokens) if (currentGameState != GameState.CharacterCreation) hToken.Draw(spriteBatch, spriteSheet1);
            #endregion

            #region Game Tracking Info: Overlord Track, Loot Track, Dice Rolls, Hero Sheets
            foreach (Token lootButton in lootOLButtons)
            {
                lootButton.Draw(spriteBatch, spriteSheet1);
                if (lootButton.DrawRectangle.Contains(mouse.X, mouse.Y))
                    if (lootButton.Variable == 0) spriteBatch.DrawString(windlassFont14, "Loot Track", new Vector2(10, 60), Color.White);
                    else spriteBatch.DrawString(windlassFont14, "Overlord Track", new Vector2(10, 60), Color.White);
            }
            
            //foreach (Message lootMessage in lootOLTrackMessages) lootMessage.Draw(spriteBatch);
            if (lootTrack.Active)
            {
                lootTrack.Draw(spriteBatch);
                masterMonsterKillToken.Draw(spriteBatch, spriteSheet1);
                foreach (Token lTokens in lootTrackTokens) lTokens.Draw(spriteBatch, spriteSheet1);
            }
            if (overlordTrack.Active)
            {
                overlordTrack.Draw(spriteBatch);
                foreach (Token olToken in overlordTokens) olToken.Draw(spriteBatch, spriteSheet1);
            }
            #endregion

			#region Game Overlays: Monster Info, Drawn Search Card
			if (monsterTokenClicked) monsterOverlay.Draw(spriteBatch, mAlphaValue);
            if (monsterTokenClicked) foreach (MonsterSheet mSheet in monsterSheet) if (mSheet.Active) mSheet.Draw(spriteBatch, spriteSheet1, mAlphaValue);

			if (searchTokenClicked) monsterOverlay.Draw(spriteBatch, mAlphaValue);
			if (searchTokenClicked)
			{
				int discardSize = searchDeck.DiscardDeck.Count - 1;
				searchDeck.DiscardDeck[discardSize].Draw(spriteBatch, shopSheet);
                if (searchDeck.DiscardDeck[discardSize].Name == "Treasure Chest")
                {
                    heroSheets[heroNumPosition].PickedClass.BackPack[heroSheets[heroNumPosition].PickedClass.BackPack.Count - 1].Draw(spriteBatch, shopSheet);
                }
			}

            if (awardingLoot)
            {
                spriteBatch.Draw(creationBG, creationRec, Color.White);
                foreach (Card lootCard in awardedShopCards) lootCard.Draw(spriteBatch, shopSheet);
            }
			#endregion

            // TODO: REMOVE AFTER TESTING drawing LoS lines
            foreach (Token dot in dots) dot.Draw(spriteBatch, spriteSheet1);

			foreach (Message message in messages) message.Draw(spriteBatch);

            foreach (Token test in testing) test.Draw(spriteBatch, spriteSheet1);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
