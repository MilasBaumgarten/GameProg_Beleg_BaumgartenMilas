using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

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
	public float maxDistanceMouseToPlayer = 2.0f;

	[Header("Camera")]
	public float maxVelocityToZoom = 30.0f;
	public float maxOffsetZ = 10.0f;
	public float defaultOffsetZ = 5.0f;
	public float offsetLerpSpeed = 2.0f;
}
