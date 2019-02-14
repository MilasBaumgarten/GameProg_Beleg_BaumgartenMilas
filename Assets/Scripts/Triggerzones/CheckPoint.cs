using UnityEngine;

public class CheckPoint : CollisionBehaviour {
	public bool activated = false;

	public float currentTime { get; private set; }
	public int currentHits { get; private set; }

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
}
