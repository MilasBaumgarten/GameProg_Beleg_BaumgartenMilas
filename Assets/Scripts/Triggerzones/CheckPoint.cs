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

	public void SetCheckpoint() {
		SetCheckpoint(GameManager.instance.stats.hits, GameManager.instance.stats.timer.currentTime);
	}
	public void SetCheckpoint(int hits, float time) {
		// activate checkpoint only if it wasn't activated before
		if (!activated) {
			activated = true;
			GameManager.instance.SetCurrentCheckpoint(gameObject);
			currentHits = hits;
			currentTime = time;
		}
	}
}
