using System.Collections.Generic;

namespace RPG.Character
{
    public interface IModifierProvider
    {
        IEnumerable<int> GetAdditiveModifiers(BaseStats.Stat stat);
        IEnumerable<float> GetPercentageModifiers(BaseStats.Stat stat);
    }
}
