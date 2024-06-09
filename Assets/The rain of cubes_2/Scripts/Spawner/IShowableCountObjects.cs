using System;

namespace Spawner
{
    public interface IShowableCountObjects
    {
        public event Action<int, int> ChangedCountsOfObjects;
    }
}
