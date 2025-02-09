using UnityEngine.Events;

namespace Skullknight.Core
{
    public interface IDamageable 
    {
        int Health { get; set; }
        UnityEvent OnHealthChanged { get; set; }
    }
}
