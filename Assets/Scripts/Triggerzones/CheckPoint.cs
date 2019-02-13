using UnityEngine;

public class CheckPoint : CollisionBehaviour {
	public bool activated = false;

	private float currentTime;
	private int currentHits;

	protected override void OnCollisionEnterWithPlayer() {
		base.OnCollisionEnterWithPlayer();
		SetCheckpoint();
	}

	protected override void OnTriggerEnterWithPlayer() {
		base.OnTriggerEnterWithPlayer();
		SetCheckpoint();
	}

	private void SetCheckpoint() {
		// activate checkpoint only if it wasn't activated before
		if (!activated) {
			activated = true;
			GameManager.instance.SetCurrentCheckpoint(gameObject);
			currentHits = GameManager.instance.stats.hits;
			currentTime = GameManager.instance.stats.timer.currentTime;
		}
	}

	public int GetCheckpointHits() {
		return currentHits;
	}

	public float GetCheckpointTime() {
		return currentTime;
	}
}
