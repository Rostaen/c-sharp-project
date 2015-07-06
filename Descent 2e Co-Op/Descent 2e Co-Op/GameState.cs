using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Descent_2e_Co_Op
{
    /// <summary>
    /// An enumeration of the possible game states
    /// </summary>
    public enum GameState
    {
        MainMenu,
        Options,
        CharacterCreation,
        StartingPositions,
        InitializeRoom,
        HeroTurn,
        OverlordTurn,
        HeroResponse,
        EndTurn,
        EndResults,
        Exiting
    }
}
