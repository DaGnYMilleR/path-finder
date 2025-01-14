﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PathFinder.Domain;

namespace PathFinder.Infrastructure.PriorityQueue.Realizations
{
    public class DictionaryPriorityQueue<TKey> : IPriorityQueue<TKey>
    {
        private readonly Dictionary<TKey, double> items = new();

        public void Add(TKey key, double value) => items.Add(key, value);

        public void Delete(TKey key) => items.Remove(key);

        public void Update(TKey key, double newValue) => items[key] = newValue;

        public (TKey key, double value) ExtractMin()
        {
            if (items.Count == 0)
                return default;
            var min = items.Min(z => z.Value);
            var key = items.FirstOrDefault(z => Math.Abs(z.Value - min) < 0.000009).Key;
            items.Remove(key);
            return (key, min);
        }

        public bool TryGetValue(TKey key, out double value) => items.TryGetValue(key, out value);

        public int Count => items.Count;

        public IEnumerator<TKey> GetEnumerator() => items
            .OrderBy(x => x.Value)
            .Select(x => x.Key)
            .GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}