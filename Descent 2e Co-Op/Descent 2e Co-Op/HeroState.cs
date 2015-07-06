using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Descent_2e_Co_Op
{
    /// <summary>
    /// An enumeration of possible hero states
    /// </summary>
    public enum HeroState
    {
		SelectHero,
        StartTurnAbility,
        FamiliarActions,
        RefreshCards,
        EquipItems,
        SelectActions,
        PerformAction,
        EndTurn
    }
}
