using UnityEngine;

[CreateAssetMenu()]
public class PlayerScriptableObject : ScriptableObject {
	[Header("Settings")]
	public bool enableDebugFeatures = true;

	[Header("Variables")]
	public float forceStrength = 4.0f;
}
