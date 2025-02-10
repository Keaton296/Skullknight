using UnityEngine.Events;

namespace Skullknight.Core
{
    public interface IDamageable 
    {
        int Health { get; set; }
        int MaxHealth {get;}
        UnityEvent<int> OnHealthChanged { get;}
    }
}
