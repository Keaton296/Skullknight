using UnityEngine;

namespace Skullknight
{
    public class ParticleTextureUpdater : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particleSystem;
        [SerializeField] private ParticleSystemRenderer system;
        [SerializeField] SpriteRenderer textureRef;
        private void Update()
        {
            var dn = particleSystem.textureSheetAnimation;
            dn.SetSprite(0, textureRef.sprite);
            
        }
    }
}
