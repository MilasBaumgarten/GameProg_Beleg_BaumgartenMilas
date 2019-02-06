using UnityEngine;

[CreateAssetMenu()]
public class PlayerScriptableObject : ScriptableObject {
	[Header("Settings")]
	public bool enableDebugFeatures = true;

	[Header("Variables")]
	public float forceStrength = 4.0f;
	[Range(0.01f, 1.0f)]
	public float slowDownTime = 0.1f;
	public float slowDownLerpDuration = 0.3f;
	public float speedUpLerpDuration = 0.1f;
}
