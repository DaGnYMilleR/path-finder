﻿using System.Collections.Generic;
using PathFinder.Domain.Models.States;

namespace PathFinder.Domain.Models.Algorithms.Realizations.AStar
{
    public class AStarAlgorithmReport : IAlgorithmReport
    {
        public List<RenderedState> RenderedStates { get; set; }
    }
}