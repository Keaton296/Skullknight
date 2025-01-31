using UnityEngine;

namespace Skullknight.Extensions
{
    public static class GameObjectExtension
    {
        public static void ToggleSetActive(this GameObject gameObject)
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}