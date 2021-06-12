using UnityEngine;

[CreateAssetMenu (fileName = "New Audio", menuName = "Audio/New Audio")]
public class Sound : ScriptableObject
{
public string soundName;
public AudioClip clip;
[Range(0f, 1f)] public float volume = 1f;
[Range(.1f, 3f)] public float pitch = 1f;
public bool isLoop;
public bool hasCooldown;
public float cooldown;
public AudioSource source;
}
