using UnityEngine;

public class CheckPoint : CollisionBehaviour {
	protected override void OnCollisionEnterWithPlayer() {
		base.OnCollisionEnterWithPlayer();
		SetCheckpoint();
	}

	protected override void OnTriggerEnterWithPlayer() {
		base.OnTriggerEnterWithPlayer();
		SetCheckpoint();
	}

	private void SetCheckpoint() {
		GameManager.instance.lastactivatedSpawn = gameObject;
	}
}
