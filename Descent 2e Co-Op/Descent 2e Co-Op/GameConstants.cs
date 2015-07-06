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
    class GameConstants
    {
        public const float LOOT_MOVE_SPEED = 0.3f;

        public const float HP_BAR_X_LOCATION = (WINDOW_WIDTH / 2) - 410;
        public const float STAM_BAR_X_LOCATION = (WINDOW_WIDTH / 2) + 166;
        public const float HP_STAM_MAX_WIDTH = 244;

        public const int STAM_TOKEN_START_X = -110;
        public const int STAM_TOKEN_START_Y = -58;
        public const int STAM_TOKEN_BUFFER_Y = -85;

        public const int HP_TOKEN_START_X = 14;
        public const int HP_TOKEN_START_Y = -72;
        public const int HP_TOKEN_BUFFER_Y = -99;

        #region Token Information

        // Token movement support
        public const float TOKEN_MOVE_SPEED = 0.17f;

        #region Drawing Hero Tokens for Hero Turns
        public const float DRAWING_TOKEN_START_X = WINDOW_WIDTH * 0.35f;
        public const float DRAWING_TOKEN_START_X_BUFFER = 0.1f;
        public const float DRAWING_TOKEN_START_Y = WINDOW_HEIGHT * 0.3f;
        #endregion

        #region Monster Tokens

        public const int ZOMBIE_FLESH_W = 64;
        public const int ZOMBIE_FLESH_BARGH_H = 64;
        public const int BARGH_W = 128;
        public const int DRAGON_W = 192;
        public const int DRAGON_H = 128;

        #endregion

        #region Action Token Positions
        public const float ACTION_TOKEN_X_LEFT = 60;
        public const float ACTION_TOKEN_Y_START = 90;
        public const float ACTION_TOKEN_Y_BUFFER = 50;
        #endregion

        #region Action Message Positions
        public const float ACTION_MESSAGE_X_LEFT = 110;
        public const float ACTION_MESSAGE_Y_START = 100;
        public const float ACTION_MESSAGE_Y_BUFFER = 50;
        #endregion

        #endregion

        #region Window Support

        // Window Width/Height - static for now, implement window sizes later
        //public static int WINDOW_WIDTH = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        //public static int WINDOW_HEIGHT = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        public const int WINDOW_HEIGHT = 900;
        public const int WINDOW_WIDTH = 1600;
        public static int HALF_WINDOW_HEIGHT()
        {
            return WINDOW_HEIGHT / 2;
        }
        public static int HALF_WINDOW_WIDTH()
        {
            return WINDOW_WIDTH / 2;
        }

        #endregion

        #region Skill/Weapon Window Prefixs

        public static int MAIN_SKILL_LOC_X = (WINDOW_WIDTH / 2) - 292;
        public const int MAIN_SKILL_BUFFER_X = -133;

        public static int HAND_1_LOC_X = (WINDOW_WIDTH / 2) + 164;
        public const int EQUIPMENT_BUFFER_X = + 133;

        #endregion

        #region Dice Info

        public const int DICE_SIDES = 6,
                         DICE_DRAW_START_X = WINDOW_WIDTH / 2 - 160,
                         DICE_DRAW_START_Y = WINDOW_HEIGHT - 306,
                         DICE_DRAW_BUFFER_X = 42,
                         DICE_DEF_START_X = WINDOW_WIDTH / 2;

        /// <summary>
        /// Summary of the values for attack dice. The array is broken down into a triple array [Die Color, Die Side, Side Values]. The values are broken down into [Range, Damage, Surge].
        /// </summary>
        public static int[,,] DICE_SIDES_ATTACK =
        {
            { {0, 0, 0}, {2, 2, 1}, {3, 2, 0}, {4, 2, 0}, {5, 1, 0}, {6, 1, 1} },   // Blue
            { {0, 1, 0}, {0, 2, 0}, {0, 2, 0}, {0, 2, 0}, {0, 3, 1}, {0, 3, 0} },   // Red
            { {1, 1, 0}, {2, 1, 0}, {0, 1, 1}, {1, 0, 1}, {0, 2, 1}, {0, 2, 0} }    // Yellow
        };

        /// <summary>
        /// Summary of the values for defense dice. The array is broken down into a double array [Die Color, Die Side Value]. The values are 0-4 depending on the side rolled to add defense to an attack.
        /// </summary>
        public static int[,] DICE_SIDES_DEFENSE = 
        {
            { 0, 0, 0, 1, 1, 2 }, // Brown
            { 0, 1, 1, 1, 2, 3 }, // Gray
            { 0, 2, 2, 2, 3, 4 }  // Black
        };

        #endregion

        #region Hero Starting Gear

        public static Equipment DISCIPLE_WEAPON = new Equipment(new Rectangle(0, 0, 128, 192), "Iron Mace", "Melee", "Hammer", "", "", "stun", "", "", 50, 1, 0, 2, -1);
        public static Equipment DISCIPLE_SHIELD = new Equipment(new Rectangle(128, 0, 128, 192), "Wooden Shield", "add1def");
        public static Equipment SPIRIT_SPEAKER_WEAPON = new Equipment(new Rectangle(0, 192, 128, 192), "Oak Staff", "Melee", "Staff", "", "reach", "1dmg", "", "", 50, 2, 0, 2, -1);
        public static Equipment NECROMANCER_WEAPON = new Equipment(new Rectangle(0, 384, 128, 192), "Reaper's Scythe", "Range", "Magic", "Staff", "heal1", "1range", "", "", 50, 2, 0, 2, -1);
        public static Equipment RUNEMASTER_WEAPON = new Equipment(new Rectangle(0, 576, 128, 192), "Arcane Bolt", "Range", "Magic", "Rune", "", "1range", "pierce2", "", 50, 2, 0, 2, -1);
        public static Equipment THIEF_WEAPON = new Equipment(new Rectangle(0, 0, 128, 192), "Throwing Knives", "Range", "Blade", "", "adjacent1dmg", "1range", "", "", 50, 1, 0, 2, -1);
        public static Equipment THIEF_TRINKET = new Equipment(new Rectangle(128, 0, 128, 192), "Lucky Charm", "Reroll Attribute Test", "");
        public static Equipment WILDLANDER_WEAPON = new Equipment(new Rectangle(0, 192, 128, 192), "Yew Shortbow", "Range", "Bow", "", "", "2range", "1dmg", "", 50, 2, 0, 2, -1);
        public static Equipment BERSERKER_WEAPON = new Equipment(new Rectangle(0, 384, 128, 192), "Chipped Greataxe", "Melee", "Axe", "", "", "1dmg", "1dmg", "", 50, 2, 0, 1, -1);
        public static Equipment KNIGHT_WEAPON = new Equipment(new Rectangle(0, 576, 128, 192), "Iron Longsword", "Melee", "Blade", "", "", "rerollDef", "", "", 50, 1, 0, 1, -1);
        public static Equipment KNIGHT_SHIELD = new Equipment(new Rectangle(128, 576, 128, 192), "Wooden Shield", "add1def");

        #endregion

        #region Tile Info

        //Tile Square
        public const int GRID_SIZE = 64;

        /// <summary>
        /// The names of the tiles to be used, hazards and the locations of which walls the entrance and 1-2 exits will be on
        /// </summary>
        public static string[,] TILE_NAMES =
        {
            {"Tile - 1B", "Rubble", "W", "N", "S"},
            {"Tile - 2B", "", "N", "S", ""},
            {"Tile - 4B", "Pit", "N", "W", ""},
            {"Tile - 5B", "", "E", "S", "W"},
            {"Tile - 8B", "", "N", "W", ""},
            {"Tile - 11B", "", "W", "E", ""},
            {"Tile - 12B", "Lava", "N", "E", ""},
            {"Tile - 17B", "", "W", "N", ""},
            {"Tile - 18B", "", "N", "W", ""},
            {"Tile - 19B", "", "E", "W", ""},
            {"Tile - 20B", "", "E", "W", ""},
            {"Tile - 21B", "Water", "E", "W", ""},
            {"Tile - 22B", "", "W", "E", ""},
            {"Tile - 26B", "", "N", "S", ""},
            {"Tile - 27B", "", "E", "W", ""},
            {"Tile - 28B", "", "S", "N", ""},
            {"Tile - 29B", "", "W", "E", ""},
            {"Tile - 30B", "", "W", "E", ""},
            {"Tile - EndB", "", "N", "", ""}
        };
        /// <summary>
        /// This gives the center of the entrance and 1-2 exits on the edges of the tile. These are used to connect other tiles to each other and for placing doors.
        /// </summary>
        public static Vector2[,] TILE_ENT_EXIT_REC =
        {
            {new Vector2(0, 192), new Vector2(256, 0), new Vector2(256, 384)},      // Tile 1B
            {new Vector2(192, 0), new Vector2(192, 384), new Vector2()},            // Tile 2B
            {new Vector2(192, 0), new Vector2(0, 192), new Vector2()},              // Tile 4B
            {new Vector2(384, 128), new Vector2(192, 256), new Vector2(0, 128)},    // Tile 5B
            {new Vector2(128, 0), new Vector2(0, 128), new Vector2()},              // Tile 8B
            {new Vector2(0, 128), new Vector2(256, 128), new Vector2()},            // Tile 11B
            {new Vector2(128, 0), new Vector2(320, 192), new Vector2()},            // Tile 12B
            {new Vector2(0, 192), new Vector2(192, 0), new Vector2()},              // Tile 17B
            {new Vector2(192, 0), new Vector2(0, 192), new Vector2()},              // Tile 18B
            {new Vector2(), new Vector2(0, 128), new Vector2(384, 64)},             // Tile 19B
            {new Vector2(512, 128), new Vector2(64, 192), new Vector2()},           // Tile 20B
            {new Vector2(384, 128), new Vector2(0, 64), new Vector2()},             // Tile 21B
            {new Vector2(0, 64), new Vector2(384, 64), new Vector2()},              // Tile 22B
            {new Vector2(128, 0), new Vector2(128, 192), new Vector2()},            // Tile 26B
            {new Vector2(128, 192), new Vector2(0, 64), new Vector2()},             // Tile 27B
            {new Vector2(192, 128), new Vector2(64, 0), new Vector2()},             // Tile 28B
            {new Vector2(0, 64), new Vector2(256, 64), new Vector2()},              // Tile 29B
            {new Vector2(0, 64), new Vector2(256, 64), new Vector2()},              // Tile 30B
            {new Vector2(0, 64), new Vector2(), new Vector2()}                      // Tile EndB
        };
        public static int[,] TILE_SIZE =
        {
            {512, 384},
            {384, 384},
            {384, 384},
            {384, 256},
            {256, 256},
            {256, 256},
            {320, 320},
            {256, 256},
            {256, 256},
            {384, 192},
            {512, 192},
            {384, 192},
            {384, 128},
            {256, 192},
            {128, 256},
            {256, 128},
            {256, 128},
            {256, 128},
            {128, 64}
        };
        public static Rectangle[,] TILE_HAZARD_LOCATIONS = 
        {
            {new Rectangle(128, 0, 64, 128), new Rectangle(64, 256, 64, 128)},  // Tile 1B Rubble
            {new Rectangle(0, 64, 256, 64), new Rectangle(320, 64, 64, 64)},    // Tile 4B Pit
            {new Rectangle(128, 64, 128, 128), new Rectangle()},                // Tile 12B Lava
            {new Rectangle(128, 0, 128, 192), new Rectangle()}                  // Tile 21B Water
        };

        #endregion

        #region Monster Sheet Info

        #region Monster Sprite Info

        public const string BARG_ACT_1 = "Monster Cards/Act 1/Barghest";
        public const string FLES_ACT_1 = "Monster Cards/Act 1/Flesh Moulder";
        public const string DRAG_ACT_1 = "Monster Cards/Act 1/Shadow Dragon";
        public const string ZOMB_ACT_1 = "Monster Cards/Act 1/Zombie";

        public const string BARG_ACT_2 = "Monster Cards/Act 2/Barghest";
        public const string FLES_ACT_2 = "Monster Cards/Act 2/Flesh Moulder";
        public const string DRAG_ACT_2 = "Monster Cards/Act 2/Shadow Dragon";
        public const string ZOMB_ACT_2 = "Monster Cards/Act 2/Zombie";

        #endregion

        #region Monster Sheet Location

        public static int MON_CARD_POS_X = WINDOW_WIDTH / 2 - 96;
        public static int MON_CARD_POS_Y = WINDOW_HEIGHT / 2 - 150;

        #endregion

        #endregion

        #region Monster Locations by Tile

        public static int[] MONSTER_LOC_1B = { 64, 0 };

        public static int[,] MONSTER_LOC_2B = { { 0, 0 }, { 192, 0 } };

        public static int[,] MONSTER_LOC_4B = { { 128, 64 }, { 64, 64 }, { 0, 64 }, { 128, 0 } };

        public static int[,] MONSTER_LOC_5B = { { 320, 64 }, { 128, 128 }, { 128, 0 }, { 256, 0 } };

        public static int[,] MONSTER_LOC_11B = { { 64, 0 }, { 0, 64 }, { 0, 128 }, { 0, 192 } };

        public static int[,] MONSTER_LOC_12B = { { 128, 64 }, { 192, 64 }, { 256, 64 }, { 192, 0 } };

        public static int[,] MONSTER_LOC_17B = { { 192, 192 }, { 192, 128 } };

        public static int[,] MONSTER_LOC_18B = { { 0, 192 }, { 64, 128 } };

        public static int[, ,] MONSTER_LOC_19B =
        {
            { {0,128}, {0,64}, {0,0} },
            { {320, 0}, {256, 64}, {256, 128} }
        };

        public static int[,] MONSTER_LOC_20B = { { 320, 64 }, { 192, 64 }, { 256, 128 }, { 384, 128 }, { 448, 64 } };

        public static int[] MONSTER_LOC_21B = { 256, 0 };

        public static int[,] MONSTER_LOC_26B = { { 64, 64 }, { 64, 128 }, { 64, 192 } };

        #endregion

        #region Peril Text

        public const string EMPTY_FLAVOR_01 = "You turn a corner and see the massive Tharn disappearing into the distance. You push forward, but the dungeon's halls are endless. It's beomcing harder and harder to breathe wihthin the stone walls. Your fate may already be sealed.";
        public const string EMPTY_COND_01 = "Advance doom by 1";
        public const string EMPTY_FLAVOR_02 = "Unseen forces latch their arms around your bodies, weighing upon your very souls.";
        public const string EMPTY_COND_02 = "Each hero reduces all of his attribute values of 4 or highter to 3. Each time a hero fails an attribute test, place 1 fatigue token on this card. When this card has a number of fatigue tokens on it equal to the number of heroes, discard this card.\n\nThis card remains active until it is discarded.";
        public const string EMPTY_FLAVOR_03 = "A well-placed trap lining the door tirggers with a swift ferocity.";
        public const string EMPTY_COND_03 = "Each hero in a space on or adjacent to an entrance or exit suffers 1dmg and 2sta.";
        public const string MON_FLAVOR_03 = "The dungeon is thrust into pure darkness, allowing the sprirts to move unseen.";
        public const string MON_COND_03 = "Each hero not adjacent to another hero suffers 2dmg and 2sta.";
        public const string EMPTY_FLAVOR_04 = "Zombies scrape at the door with their brittle nails, sending a chill that runs through your whole body.";
        public const string EMPTY_COND_04 = "Roll 1 gray die. For each shield result, place 1 zombie on the exit of the current encounter. If the current encounter has no exit, place them on the entrance.";
        public const string MON_FLAVOR_04 = "Fighting is difficult work, but this skirmish is particularly rough.";
        public const string MON_COND_04 = "Each hero within 2 spaces of a monster suffers 3sta.";
        public const string EMPTY_FLAVOR_05 = "A jagged rock blocks the path. The strongest can move it, but not without consequences.";
        public const string EMPTY_COND_05 = "Each hero tests str. Each hero who passes suffers 3dmg.";
        public const string MON_FLAVOR_05 = "The effects of the last zombie assault have not faded.";
        public const string MON_COND_05 = "Each hero within 3 spaces of a zombie is Stunned. If there are no zombies on the map, each hero sufferes 3sta.";
        public const string EMPTY_FLAVOR_06 = "Fleshless hands emerge from the ground, pulling undead bodies from the earth.";
        public const string EMPTY_COND_06 = "Roll 1 gray defense die. For each shield result, place 1 zombie in an empty space adjacent to the hero with the highest knowledge, respecting group limits.";
        public const string MON_FLAVOR_06 = "Dead hounds leap from the cracks and crevices of hallow walls.";
        public const string MON_COND_06 = "Each hero tests awareness. Place 1 Barghest 10 spaces (or as close to 10 as possible) away from each hero who fails, respecting group limits.";
        public const string EMPTY_FLAVOR_07 = "Apparitions surround your party. Your senses dull and your mind becomes clouded. Then, without a sound, they are gone.";
        public const string EMPTY_COND_07 = "Each hero tests an attribute of his choice twice. Each hero who fails at least one test is Stunned.";
        public const string MON_FLAVOR_07 = "From the shadows a master flesh moulder uses all of his energy to heal a damaged ally.";
        public const string MON_COND_07 = "Roll 2 red power dice. The moster that has the most damage tokens recovers dmg equal to the amount of dmg rolled.";
        public const string EMPTY_FLAVOR_08 = "The scratch of a monster lingers on your flesh. The throbbing pain shows no sign of dissipating; you must find a remedy.";
        public const string EMPTY_COND_08 = "The hero who has the most remaining Health places this card in his play area. This hero cannot recover dmg through any means. If this hero performs 1 search action or is defeated, discard this card.\n\nThis card remains active until it is discarded.";
        public const string EMPTY_FLAVOR_09 = "A fireball rushes at your group! Who will bear the brunt of this attack?";
        public const string EMPTY_COND_09 = "Roll 2 red power dice. The heroes must collectively suffer dmg equal to the amount of dmg rolled. Damage may be unevenly distributed amont heroes.";
        public const string MON_FLAVOR_09 = "The monsteres' eyes glow red with anger; a powerful energy drives them to action.";
        public const string MON_COND_09 = "Draw 2 Activation cards instead of 1 durign the next Monster Activation step of the Overlord phase and activate mosters once for each Activation card.";
        public const string EMPTY_FLAVOR_10 = "The difficulty of your quest is weighing upon you. You must rest.";
        public const string EMPTY_COND_10 = "Each hero sufferes 1 sta. Then, each hero who has a number of fatigue tokens equal to his Stamina is Stunned.";
        public const string MON_FLAVOR_10 = "The monsters grab and swing at every hero in sight, pulling them into their deadly grasp.";
        public const string MON_COND_10 = "If a hero is adjacent to a monster, he is Stunned. Then, each hero must move 2 spaces toward the closest monster.";


        #endregion
    }
}
