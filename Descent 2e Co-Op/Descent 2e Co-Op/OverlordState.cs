using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Descent_2e_Co_Op
{
    /// <summary>
    /// An enumeration of possible Overlord states
    /// </summary>
    public enum OverlordState
    {
        OverlordEffect,
        Fate,
        MonsterActivation,
        DetermineMonsterGroup,
        ActiveEffect,
        ChooseMonster,
        PerformActions
    }
}
