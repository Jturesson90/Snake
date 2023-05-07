using UnityEngine;

namespace Scripts
{
    [CreateAssetMenu]
    public class AudioClipReference : ScriptableObject
    {
        [field: SerializeField] public AudioClip[] GameOver { get; set; }
        [field: SerializeField] public AudioClip[] FoodPickup { get; set; }
        [field: SerializeField] public AudioClip[] SnakeChangeDirection { get; set; }
    }
}