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
    public class MonsterSheet
    {
        #region Fields

        int minHP, masHP, minMove, masMove, act,
            minDef1, minDef2, masDef1, masDef2,
            minDie1, minDie2, minDie3,
            masDie1, masDie2, masDie3;

        string name, type,
            minSkill1, minSkill2, minSkill3, minSkill4,
            masSkill1, masSkill2, masSkill3, masSkill4;

        bool active = false;

        Rectangle drawRectangle, sourceRectangle;

        #endregion

        #region Constructors

        public MonsterSheet(int xLoc, int yLoc, string name, string type, int act,
            int minMove, int minHP, int minDef1, int minDef2, int minDie1, int minDie2, int minDie3, string minSkill1, string minSkill2, string minSkill3, string minSkill4,
            int masMove, int masHP, int masDef1, int masDef2, int masDie1, int masDie2, int masDie3, string masSkill1, string masSkill2, string masSkill3, string masSkill4)
        {
            this.name = name; this.act = act; this.type = type;
            if (name == "barghest") sourceRectangle = new Rectangle(576, 584, 192, 300);
            else if (name == "flesh moulder") sourceRectangle = new Rectangle(384, 584, 192, 300);
            else if (name == "shadow dragon") sourceRectangle = new Rectangle(192, 584, 192, 300);
            else sourceRectangle = new Rectangle(0, 584, 192, 300);

            this.minMove = minMove; this.minHP = minHP; this.minDef1 = minDef1; this.minDef2 = minDef2; this.minDie1 = minDie1; this.minDie2 = minDie2; this.minDie3 = minDie3;
            this.minSkill1 = minSkill1; this.minSkill2 = minSkill2; this.minSkill3 = minSkill3; this.minSkill4 = minSkill4;

            this.masMove = masMove; this.masHP = masHP; this.masDef1 = masDef1; this.masDef2 = masDef2; this.masDie1 = masDie1; this.masDie2 = masDie2; this.masDie3 = masDie3;
            this.masSkill1 = masSkill1; this.masSkill2 = masSkill2; this.masSkill3 = masSkill3; this.masSkill4 = masSkill4;

            drawRectangle = new Rectangle(xLoc, yLoc, 192, 300);
        }
        #endregion

        #region Properties

		public int MinDef1 { get { return minDef1; } }
		public int MinDef2 { get { return minDef2; } }
		public int MasDef1 { get { return masDef1; } }
		public int MasDef2 { get { return masDef2; } }
        public int MinHP { get { return minHP; } }
        public int MasHP { get { return masHP; } }
        public int MinMove { get { return minMove; } }
        public int MasMove { get { return masMove; } }

        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        public string Name
        {
            get { return name; }
        }

        public Rectangle DrawRectangle
        {
            get { return drawRectangle; }
            set { drawRectangle = value; }
        }

        #endregion

        #region Public Methods

        public void Draw(SpriteBatch spriteBatch, Texture2D spriteSheet , int mAlphaValue)
        {
            spriteBatch.Draw(spriteSheet, drawRectangle, sourceRectangle,new Color(255, 255, 255, (byte)MathHelper.Clamp(mAlphaValue, 0, 255)));
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
