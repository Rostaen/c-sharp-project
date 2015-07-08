using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Descent_2e_Co_Op
{
    public class Token
    {
        #region Fields

        // Graphics and drawing info
        Rectangle drawRectangle, originalDrawRect, sourceRectangle = new Rectangle();
        Vector2 location, direction = Vector2.Zero, target = Vector2.Zero, originalPosition, miniTarget = Vector2.Zero;
        List<Vector2> movementPath = new List<Vector2>();

        string name = "";
        bool active = true, clicked = false, targetReached = true, xReached = false, yReached = false, negXDir = false, negYDir = false, isMaster = false,
             knockedOut = false, dead = false, hasMoved = false, occupied = false;
        bool[] conditions = { false, false, false, false }; // Conditions are as such: Poison, Disease, Stun, Immobilized

        int halfDrawRectangleWidth, halfDrawRectangleHeight, spacesMoved = 0, monsterType = -1, variable = -1, remainingMovementPoints = -1,
            hitPoints = 0, stamina = 0, movement = 0, movementUsed = 0, movementUsedX = 0, movementUsedY = 0;

        // Token sound effects
        SoundEffect movementSound = null, attackSound = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a generic token
        /// </summary>
        /// <param name="content">Content manager to load</param>
        /// <param name="spriteName">String directory of the sprite to load</param>
        /// <param name="rectangle">Rectangle of where to load the generic token</param>
        /// <param name="variable">This number is based off a specific usage, see construction details for specifics, generally it is used to give the position of the list it's from.</param>
        /// <param name="xLoc">X location of where to place the token in the provided rectangle</param>
        /// <param name="yLoc">Y location of where to place the token in the provided rectangle</param>
        public Token(Rectangle rectangle, int variable, int xLoc, int yLoc, Rectangle sourceRectangle)
        {
            this.sourceRectangle = sourceRectangle;
            drawRectangle = new Rectangle(rectangle.X + xLoc, rectangle.Y + yLoc, sourceRectangle.Width, sourceRectangle.Height);
            this.variable = variable;
        }

        /// <summary>
        /// Constructs a floor highlight for moving heroes around the board
        /// </summary>
        /// <param name="content">Content manger to load</param>
        /// <param name="spriteName">Name of the sprite to load</param>
        /// <param name="drawRectangle">Incoming rectangle to be drawn</param>
        public Token(Rectangle sourceRectangle, Rectangle drawRectangle) { this.sourceRectangle = sourceRectangle; this.drawRectangle = drawRectangle; }

        /// <summary>
        /// Constructs a ol/loot track token
        /// </summary>
        /// <param name="content"></param>
        /// <param name="spriteName"></param>
        /// <param name="incTile"></param>
        /// <param name="xLoc"></param>
        /// <param name="yLoc"></param>
        public Token(Rectangle sourceRect, string tokenName, Tracks incTile, int xLoc, int yLoc)
        {
            sourceRectangle = sourceRect; name = tokenName; drawRectangle = new Rectangle(incTile.DrawRectangle.X + xLoc, incTile.DrawRectangle.Y + yLoc, GameConstants.GRID_SIZE, GameConstants.GRID_SIZE);
        }

        /// <summary>
        /// Constructs a search token object
        /// </summary>
        /// <param name="content">The content manager</param>
        /// <param name="incTile">The tile to draw on</param>
        /// <param name="xLoc">X loc of the search token</param>
        /// <param name="yLoc">Y loc of the search token</param>
        public Token(Rectangle sourceRectangle, Tile incTile, int xLoc, int yLoc, int tokenIndex)
        {
			this.sourceRectangle = sourceRectangle; variable = tokenIndex;
            int tempX = incTile.X + xLoc, tempY = incTile.Y + yLoc;
            drawRectangle = new Rectangle(tempX, tempY, GameConstants.GRID_SIZE, GameConstants.GRID_SIZE);
        }

        /// <summary>
        /// Constructs a hero token for the selected hero
        /// </summary>
        /// <param name="contentManager">The content manager for loading content</param>
        /// <param name="sprite">The image of the selected token</param>
        /// <param name="name">Name of the selected hero</param>
        /// <param name="number">Number of the token being generated for initial board placement</param>
        public Token(string name, int number, Tile incTile, int hp, int sta, int move, Rectangle sourceRectangle)
        {
            int tempX, tempY; // used to place the hero token on the board
            switch (number)
            {
                case 1: tempX = incTile.X; tempY = incTile.Y - 64; break;
                case 2: tempX = incTile.X + 64; tempY = incTile.Y - 64; break;
                case 3: tempX = incTile.X + 128; tempY = incTile.Y - 64; break;
                case 4: tempX = incTile.X + 192; tempY = incTile.Y - 64; break;
                default: tempX = HalfWindowWidth(); tempY = HalfWindowHeight(); break;
            }
            drawRectangle = new Rectangle(tempX, tempY, sourceRectangle.Width, sourceRectangle.Height);
            this.sourceRectangle = sourceRectangle;
            location.X = tempX + 32; location.Y = tempY + 32;
			originalPosition = location;
			originalDrawRect = drawRectangle;
            halfDrawRectangleHeight = sourceRectangle.Height / 2;
            halfDrawRectangleWidth = sourceRectangle.Width / 2;
            this.name = name; variable = number; hitPoints = hp; stamina = sta; movement = move;
        }
        
        /// <summary>
        /// Constructs a monster token
        /// </summary>
        /// <param name="content">The content manager</param>
        /// <param name="sprite">A string containing the sprites location</param>
        /// <param name="isMaster">If the monster is a master</param>
        /// <param name="monsterType">number indicating which monster it is: 0 = zombie, 1 = dragon, 2 = flesh moulder, 3 = barghest</param>
        /// <param name="incTile"></param>
        public Token(string name, Rectangle sourceRectangle, bool isMaster, int monsterType, Tile incTile, int xLocation, int yLocation, int width, int height, int hp, int sta, int move)
        {
            int tokenX = incTile.X + xLocation, tokenY = incTile.Y + yLocation;

            this.monsterType = monsterType; this.isMaster = isMaster; this.name = name;
            drawRectangle = new Rectangle(tokenX, tokenY, width, height);

            if (width == 64) location.X = tokenX + 32;
            else if (width == 128) location.X = tokenX + 64;
            else location.X = tokenX + 96;
            if (height == 64) location.Y = tokenY + 32;
            else location.Y = tokenY + 64;
            this.sourceRectangle = sourceRectangle;
            halfDrawRectangleWidth = sourceRectangle.Width / 2; halfDrawRectangleHeight = sourceRectangle.Height / 2;
            hitPoints = hp; stamina = sta; movement = move;
        }

        #endregion

        #region Properties

        public List<Vector2> MovementPath { get { return movementPath; } set { movementPath = value; } }
        public int drawRectWidth { get { return drawRectangle.Width; } set { drawRectangle.Width = value; } }
        public int SourceRectangleX { get { return sourceRectangle.X; } set { sourceRectangle.X = value; } }
        public int SourceRectangleY { get { return sourceRectangle.Y; } set { sourceRectangle.Y = value; } }
        public Rectangle SourceRectangle { get { return sourceRectangle; } set { sourceRectangle = value; } }
        public bool TargetReached { get { return targetReached; } }
        public bool[] Conditions { get { return conditions; } set { conditions = value; } }
        public int MovementUsed { get { return movementUsed; } set { movementUsed = value; } }
		public Vector2 Location { get { return location; } set { location = value; } }
        public float LocationX { get { return location.X; } set { location.X = value; } }
        public float LocationY { get { return location.Y; } set { location.Y = value; } }
		public Vector2 OriginalLocation { get { return originalPosition; } set { originalPosition = value; } }
        public bool IsMaster { get { return isMaster; } }
        public int HP {get { return hitPoints; } set { hitPoints = value; } }
		public int Stam { get { return stamina; } set { stamina = value; } }
        public int Movement { get { return movement; } set { movement = value; } }
        public bool Occupied { get { return occupied; } set { occupied = value; } }
        public int RemainingMovementPoints { get { return remainingMovementPoints; } set { remainingMovementPoints = value; } }
        public bool HasMoved { get { return hasMoved; } set { hasMoved = value; } }
        public bool KnockedOut { get { return knockedOut; } }
        public bool Dead { get { return dead; } }
        public int Variable { get { return variable; } }
        public int SpacesMoved { get { return spacesMoved; } }
        public bool Active { get { return active; } set { active = value; } }
        public bool Clicked{ get { return clicked; } set { clicked = value; } }
		public Rectangle OriginalDrawRect { get { return originalDrawRect; } set { originalDrawRect = value; } }
        public Rectangle DrawRectangle { get { return drawRectangle; } set { drawRectangle = value; } }
        public int DrawRectangleX { get { return drawRectangle.X; } set { drawRectangle.X = value; } }
        public int DrawRectangleY { get { return drawRectangle.Y; } set { drawRectangle.Y = value; } }
        public int HalfDrawRectWidth { set { halfDrawRectangleWidth = value; } }
        public int HalfDrawRectHeight { set { halfDrawRectangleHeight = value; } }

        /// <summary>
        /// Sets the X location of the center of the hero token
        /// </summary>
        public int X
        {
            get { return drawRectangle.X / 2; }
            set { drawRectangle.X = value - drawRectangle.Width / 2; }
        }

        /// <summary>
        /// Sets the Y location of the center of the hero token
        /// </summary>
        public int Y
        {
            get { return drawRectangle.Y / 2; }
            set { drawRectangle.Y = value - drawRectangle.Height / 2; }
        }

        /// <summary>
        /// Gets the movement sound effect for hero tokens moving
        /// </summary>
        public SoundEffect MovementSound
        {
            get { return movementSound; }
        }

        /// <summary>
        /// Gets and sets the hero attack sound, melee/ranged/magic/thrown
        /// </summary>
        public SoundEffect AttackSound
        {
            get { return attackSound; }
            set { attackSound = value; }
        }

        public string Name
        {
            get { return name; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Draws the hero token
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to use</param>
        public void Draw(SpriteBatch spriteBatch, Texture2D spriteSheet)
        {
            spriteBatch.Draw(spriteSheet, drawRectangle, sourceRectangle, Color.White);
        }

        /// <summary>
        /// Updates the hero token around the board based on where the mouse is clicked
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="mouse">The current state of the mouse (to get X/Y position for movement)</param>
        public void Update(GameTime gameTime)
        {
            if (active)
            {
                if (!targetReached)
                {
                    location += direction * GameConstants.TOKEN_MOVE_SPEED * gameTime.ElapsedGameTime.Milliseconds;
                    drawRectangle.X = (int)location.X - halfDrawRectangleWidth;
                    drawRectangle.Y = (int)location.Y - halfDrawRectangleHeight;

                    if (!xReached)
                    {
                        if (negXDir) { if (location.X - target.X <= 0) xReached = true; } 
                        else { if (target.X - location.X <= 0) xReached = true; }
                    }
                    if (!yReached)
                    {
                        if (negYDir) { if (location.Y - target.Y <= 0) yReached = true; }
                        else { if (target.Y - location.Y <= 0) yReached = true; }
                    }
					if (xReached && yReached) { targetReached = true; negXDir = false; negYDir = false; }
                }
            }
        }

        public void Update2(GameTime gameTime)
        {
            if (active)
            {
                if (!targetReached)
                {
                    location += direction * GameConstants.TOKEN_MOVE_SPEED * gameTime.ElapsedGameTime.Milliseconds;
                    drawRectangle.X = (int)location.X - halfDrawRectangleWidth;
                    drawRectangle.Y = (int)location.Y - halfDrawRectangleHeight;
                    Vector2 currentTarget = movementPath[movementPath.Count - 1];
                    if (!xReached)
                    {
                        if (negXDir) { if ((int)location.X - currentTarget.X <= 0) xReached = true; }
                        else { if (currentTarget.X - (int)location.X <= 0) xReached = true; }
                    }
                    if (!yReached)
                    {
                        if (negYDir) { if ((int)location.Y - currentTarget.Y <= 0) yReached = true; }
                        else { if (currentTarget.Y - (int)location.Y <= 0) yReached = true; }
                    }
                    if (xReached && yReached) { targetReached = true; negXDir = false; negYDir = false; }
                }
                else if(targetReached)
                {
                    targetReached = false;
                    int pos = movementPath.Count - 1;
                    if (pos > -1) movementPath.RemoveAt(movementPath.Count - 1);
                    if (movementPath.Count != 0)
                    {
                        getNextDirection(movementPath);
                        xReached = false; yReached = false;
                        Update2(gameTime);
                    }
                    else active = false;
                }
            }
        }

        private void getNextDirection(List<Vector2> movementPath)
        {
            int lastSpot = movementPath.Count - 1;
            float dirX = movementPath[lastSpot].X - location.X;
            float dirY = movementPath[lastSpot].Y - location.Y;
            direction = new Vector2(dirX, dirY);
            direction.Normalize();
        }

        /// <summary>
        /// Trying out a new recursive search to target movement spot
        /// </summary>
        /// <param name="target">The selected target to move too</param>
        public void NewSetTarget(Vector2 target)
        {
            xReached = false; yReached = false;
            targetReached = false;
            this.target = target;

            if (target.X - location.X < 0) { negXDir = true; }
            if (target.Y - location.Y < 0) { negYDir = true; }
          
            movementPath.Add(target);
            movementPath = checkTargetToLocation(movementPath);
            movementPath.RemoveAt(movementPath.Count - 1);
            setMovementUsed(movementPath.Count, 0);
            getNextDirection(movementPath);
        }

        /// <summary>
        /// Recursively checks a cell by cell path from the target to the current token location
        /// </summary>
        /// <param name="target">Current target location being checked recursively</param>
        /// <returns>Returns: target = location ? target : updated target</returns>
        private List<Vector2> checkTargetToLocation(List<Vector2> target)
        {
            int pos = target.Count -1;
            if (target[pos].X == (int)location.X && target[pos].Y == (int)location.Y) return target;
            else
            {
                if (target[pos].X - (int)location.X == 0 && target[pos].Y > (int)location.Y)
                {
                    target.Add(new Vector2(target[pos].X, target[pos].Y - 64));
                    return checkTargetToLocation(target);
                }
                else if (target[pos].X < (int)location.X && target[pos].Y > (int)location.Y)
                {
                    target.Add(new Vector2(target[pos].X + 64, target[pos].Y - 64));
                    return checkTargetToLocation(target);
                }
                else if (target[pos].X < (int)location.X && target[pos].Y - (int)location.Y == 0)
                {
                    target.Add(new Vector2(target[pos].X + 64, target[pos].Y));
                    return checkTargetToLocation(target);
                }
                else if (target[pos].X < (int)location.X && target[pos].Y < (int)location.Y)
                {
                    target.Add(new Vector2(target[pos].X + 64, target[pos].Y + 64));
                    return checkTargetToLocation(target);
                }
                else if (target[pos].X - (int)location.X == 0 && target[pos].Y < (int)location.Y)
                {
                    target.Add(new Vector2(target[pos].X, target[pos].Y + 64));
                    return checkTargetToLocation(target);
                }
                else if (target[pos].X > (int)location.X && target[pos].Y < (int)location.Y)
                {
                    target.Add(new Vector2(target[pos].X - 64, target[pos].Y + 64));
                    return checkTargetToLocation(target);
                }
                else if (target[pos].X > (int)location.X && target[pos].Y - (int)location.Y == 0)
                {
                    target.Add(new Vector2(target[pos].X - 64, target[pos].Y));
                    return checkTargetToLocation(target);
                }
                else
                {
                    target.Add(new Vector2(target[pos].X - 64, target[pos].Y - 64));
                    return checkTargetToLocation(target);
                }
            }
        }

        /// <summary>
        /// Sets a target for the token to move too
        /// </summary>
        /// <param name="target">The vector location tile target location</param>
        public void SetTarget(Vector2 target)
        {
            xReached = false; yReached = false;
            bool xGood = false, yGood = false;
            Vector2 tempTarget;
            targetReached = false;

            this.target = target;
            target -= location;

            if (target.X < 0) { tempTarget.X = target.X * -1; negXDir = true; }
            else tempTarget.X = target.X; 
            if (target.Y < 0) { tempTarget.Y = target.Y * -1; negYDir = true; }
            else tempTarget.Y = target.Y;
            if (tempTarget.X > 32) xGood = true;
            if (tempTarget.Y > 32) yGood = true;

            if (xGood || yGood)
            {
                float xBox = checkDistance(target.X), yBox = checkDistance(target.Y);
                if (xBox == 0) xReached = true;
                else if (yBox == 0) yReached = true;
                setMovementUsed(xBox, yBox);
                direction.X = xBox; direction.Y = yBox;
                direction.Normalize();
                this.target.X = xBox + location.X;
                this.target.Y = yBox + location.Y;
            }
        }

		public void adjustPosition(Rectangle tileRect)
		{
			double xPosOnTile = drawRectangle.X - tileRect.X, yPosOnTile = drawRectangle.Y - tileRect.Y;
			xPosOnTile /= 64; yPosOnTile /= 64;
			xPosOnTile = Math.Round(xPosOnTile) * 64;
			yPosOnTile = Math.Round(yPosOnTile) * 64;
			drawRectangle.X = tileRect.X + (int)xPosOnTile; drawRectangle.Y = tileRect.Y + (int)yPosOnTile;
            originalDrawRect = drawRectangle; location.X = drawRectangle.X + 32; location.Y = drawRectangle.Y + 32; OriginalLocation = location;            
		}

        #endregion

        #region Private Methods

        private void setMovementUsed(float xBox, float yBox)
        {
            if (xBox < 0) xBox *= -1;
            movementUsedX = (int)(xBox / 64);
            if (yBox < 0) yBox *= -1;
            movementUsedY = (int)(yBox / 64);
            if (movementUsedX > movementUsedY) movementUsed = movementUsedX;
            else movementUsed = movementUsedY;
        }

        private float checkDistance(float checkValue)
        {
            bool isNeg = false;
            checkValue /= 64;
            if (checkValue < 0)
            {
                isNeg = true;
                checkValue *= -1;
            }
            int baseValue = (int)checkValue;
            float theDecimal = checkValue - baseValue;
            if (theDecimal > 0.5f) theDecimal = 1;
            else theDecimal = 0;
            if (isNeg)
            {
                isNeg = false;
                return (((float)baseValue + theDecimal) * 64) * -1;
            }
            else return ((float)baseValue + theDecimal) * 64;
        }

        /// <summary>
        /// Returns ½ the window width
        /// </summary>
        /// <returns>½ Window Width</returns>
        private int HalfWindowWidth()
        {
            return GameConstants.WINDOW_WIDTH / 2;
        }

        /// <summary>
        /// Returns ½ the window height
        /// </summary>
        /// <returns>½ Window Height</returns>
        private int HalfWindowHeight()
        {
            return GameConstants.WINDOW_HEIGHT / 2;
        }

        #endregion
    }
}
