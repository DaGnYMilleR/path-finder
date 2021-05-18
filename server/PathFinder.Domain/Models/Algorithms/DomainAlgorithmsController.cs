﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PathFinder.Domain.Interfaces;
using PathFinder.Domain.Models.Algorithms.AStar;
using PathFinder.Domain.Models.States;

namespace PathFinder.Domain.Models.Algorithms
{
    public class DomainAlgorithmsController // TODO поменять название
    {
        private readonly IAlgorithm<State>[] algorithms;
        private readonly IAlgorithmsExecutor algorithmsExecutor;

        public DomainAlgorithmsController(IAlgorithm<State>[] algorithms, IAlgorithmsExecutor algorithmsExecutor)
        {
            this.algorithms = algorithms;
            this.algorithmsExecutor = algorithmsExecutor;
        }

        public IEnumerable<Dictionary<string, object>> GetInfoAboutAlgorithmsWithAvailableParams() // TODO refactor
        {
            foreach (var algorithm in algorithms)
            {
                var res = new Dictionary<string, object> {["name"] = algorithm.Name};
                var parameters = algorithm.GetParametersType()
                    .GetProperties()
                    .Select(x => x.GetCustomAttributes<AlgorithmSelectableParameterAttribute>(false)
                        .FirstOrDefault())
                    .Where(x => x != null);
                
                foreach (var parameter in parameters)
                {
                    res.Add(parameter.DisplayName, parameter.PossibleValues);
                }
                yield return res;
            }
        }
        
        public IEnumerable<string> AvailableAlgorithmNames()
            => algorithms.Select(x => x.Name);

        public AlgorithmExecutionInfo ExecuteAlgorithm(string name, IGrid grid, IParameters parameters)
        {
            var algorithm = algorithms.FirstOrDefault(x => x.Name == name);
            if (algorithm == null)
                throw new ArgumentException($"algorithm not found: {name}");
            return algorithmsExecutor.Execute(algorithm, grid, parameters);
        }
    }
}