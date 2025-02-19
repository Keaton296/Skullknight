using UnityEngine.Events;

namespace Skullknight.Core
{
    public interface IDamageable 
    {
        int Health {get;}
        int MaxHealth {get;}
        bool IsDamageable { get;}
        bool TakeDamage(int amount);
        UnityEvent<int> OnHealthChanged { get;}
    }
}
