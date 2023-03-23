using UnityEngine;

[CreateAssetMenu(fileName = "AudioFilesSO", menuName = "Learning_yogi/AudioFiles", order = 0)]
public class AudioFilesScriptableObject : ScriptableObject {
	public AudioClip startPressed;
	public AudioClip asteroidDestroy;
	public AudioClip playerExplode;
	public AudioClip backgroundMusic;
	public AudioClip gameEnded;
}
