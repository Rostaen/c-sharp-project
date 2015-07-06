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
    public class Tile
    {
        #region Fields

        Vector2 entrance, exit1, exit2;
        int tileWidth = 0, tileHeight = 0, variable = -1;
        string entranceSide = "N", exitSide1 = "N", exitSide2 = "N";
        Texture2D sprite;
        bool locked = false, hazard = false;
        Rectangle drawRectangle;

        #endregion

        #region Constructors

        /// <summary>
        /// Sets up a door tile to signify an open/locked door
        /// </summary>
        /// <param name="content">Content being passed in</param>
        /// <param name="doorName">name of the door to use, "Door Yellow" for unlocked, "Door Red" for locked</param>
        /// <param name="locked">Boolean expression to represent if the door is locked or not</param>
        /// <param name="exitLocation">Vector location of the tiles exit</param>
        /// <param name="exitSide">String location of the tiles exit: N, S, E, W</param>
        /// <param name="xCordinate">The X coordinate of the tile the door is being attached too</param>
        /// <param name="yCordinate">The Y coordinate of the tile the door is being attached too</param>
        /// <param name="tileHeight">The tiles height, in pixels</param>
        /// <param name="tileWidth">The tiles width, in pixels</param>
        public Tile(ContentManager content, string doorName, bool locked, Vector2 exitLocation, string exitSide, int xCordinate, int yCordinate, int tileHeight, int tileWidth)
        {
            this.locked = locked; this.exitSide1 = exitSide;
            if (exitSide == "W" || exitSide == "E") sprite = content.Load<Texture2D>("Misc/" + doorName + " a");
            else sprite = content.Load<Texture2D>("Misc/" + doorName);

            if (exitSide == "E" || exitSide == "W") drawRectangle = new Rectangle(xCordinate + (int)exitLocation.X - (sprite.Width / 2), yCordinate + (int)exitLocation.Y - (sprite.Height / 2), 128, 256);
            else drawRectangle = new Rectangle(xCordinate + (int)exitLocation.X - (sprite.Width / 2), yCordinate + (int)exitLocation.Y - (sprite.Height / 2), 256, 128);
            exit1 = exitLocation;
        }

        /// <summary>
        /// Sets up an end cap tile to attach to an alternate exit or when tiles overlap
        /// </summary>
        /// <param name="content">Content being passed in</param>
        /// <param name="xCordinate">The X coordinate of the tile the door is being attached too</param>
        /// <param name="yCordinate">The Y coordinate of the tile the door is being attached too</param>
        /// <param name="attachingTileWidth">The tiles width, in pixels</param>
        /// <param name="attachingTileHeight">The tiles height, in pixels</param>
        /// <param name="exit2Location">Vector location of the tiles exit</param>
        /// <param name="exit2Side">String location of the tiles exit: N, S, E, W</param>
        public Tile(ContentManager content, int xCordinate, int yCordinate, int attachingTileWidth, int attachingTileHeight, Vector2 exit2Location, string exit2Side, int variable)
        {
            this.variable = variable;
            if (exit2Side == "E" || exit2Side == "W") drawRectangle = new Rectangle(xCordinate, yCordinate, 64, 128);
            else drawRectangle = new Rectangle(xCordinate, yCordinate, 128, 64);
            this.exitSide2 = exit2Side;
            if (exit2Side == "N") sprite = content.Load<Texture2D>("Tiles/Tile - EndBc");
            else if (exit2Side == "E") sprite = content.Load<Texture2D>("Tiles/Tile - EndBd");
            else if (exit2Side == "S") sprite = content.Load<Texture2D>("Tiles/Tile - EndBa");
            else sprite = content.Load<Texture2D>("Tiles/Tile - EndBb");
        }

        /// <summary>
        /// Sets up a tile with the given parameters
        /// </summary>
        /// <param name="content">Content being passed in</param>
        /// <param name="ent">Location of the entrance of the tile</param>
        /// <param name="ext">Location of the exit of the tile</param>
        /// <param name="width">Tile width</param>
        /// <param name="height">Tile height</param>
        /// <param name="entSide">The side of the tile the entrance is located on: N, E, S, W</param>
        /// <param name="extSide">The side of the tile the exit is located on: N, E, S, W</param>
        /// <param name="spriteName">Name of the tile sprite to use</param>
        public Tile(ContentManager content, int xCordinate, int yCordinate, int width, int height, Vector2 entrance, Vector2 exit1, Vector2 exit2, string entranceSide, string exitSide1, string exitSide2, string spriteName, bool hazard, int variable)
        {
            this.variable = variable;
            drawRectangle = new Rectangle(xCordinate, yCordinate, width, height);
            this.entrance = entrance; this.exit1 = exit1; this.exit2 = exit2;
            this.entranceSide = entranceSide; this.exitSide1 = exitSide1; this.exitSide2 = exitSide2;
            sprite = content.Load<Texture2D>("Tiles/" + spriteName);
            this.hazard = hazard;
        }

        #endregion

        #region Properties

        public int Variable { get { return variable; } }

        /// <summary>
        /// Gets the entrance vector coordinates
        /// </summary>
        public Vector2 Entrace
        {
            get { return entrance; }
        }

        /// <summary>
        /// Gets the exit1 vector coordinates
        /// </summary>
        public Vector2 Exit1
        {
            get { return exit1; }
        }

        /// <summary>
        /// Gets the exit2 vector coordinates
        /// </summary>
        public Vector2 Exit2
        {
            get { return exit2; }
        }

        /// <summary>
        /// Gets the tile width
        /// </summary>
        public int Width
        {
            get { return tileWidth; }
        }

        /// <summary>
        /// Gets the tile height
        /// </summary>
        public int Height
        {
            get { return tileHeight; }
        }

        public Rectangle DrawRectangle
        {
            get { return drawRectangle; }
        }

        /// <summary>
        /// Gets the starting Y coordinate of the tile
        /// </summary>
        public int Y
        {
            get { return drawRectangle.Y; }
        }

        /// <summary>
        /// Gets the starting X coordinate of the tile
        /// </summary>
        public int X
        {
            get { return drawRectangle.X; }
        }

        /// <summary>
        /// Gets the tile entrance side
        /// N (North), E (East), S (South), W (West)
        /// </summary>
        public string EntranceSide
        {
            get { return entranceSide; }
        }

        /// <summary>
        /// Gets the tile exit1 side
        /// N (North), E (East), S (South), W (West)
        /// </summary>
        public string ExitSide1
        {
            get { return exitSide1; }
        }

        /// <summary>
        /// Gets the tile exit2 side
        /// N (North), E (East), S (South), W (West)
        /// </summary>
        public string ExitSide2
        {
            get { return exitSide2; }
        }

        /// <summary>
        /// Gets the center of the spirtes X value
        /// </summary>
        public int CenterX
        {
            get { return sprite.Width / 2; }
        }

        /// <summary>
        /// Gets the center of the spirtes Y value
        /// </summary>
        public int CenterY
        {
            get { return sprite.Height / 2; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This is the starting position for the first tile series of the Forgotten Souls co-op game
        /// </summary>
        /// <param name="spriteBatch">Spritebatch to draw spirte images and etc</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, drawRectangle, Color.White);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Calculates the sprites center X
        /// </summary>
        /// <returns></returns>
        private int spriteCenterX()
        {
            return sprite.Width / 2;
        }

        /// <summary>
        /// Calculates the sprites center y
        /// </summary>
        /// <returns></returns>
        private int spriteCenterY()
        {
            return sprite.Height / 2;
        }

        #endregion
    }
}
