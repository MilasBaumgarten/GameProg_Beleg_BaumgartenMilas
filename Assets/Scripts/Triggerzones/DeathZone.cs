public class DeathZone : CollisionBehaviour {
	protected override void OnCollisionEnterWithPlayer() {
		base.OnCollisionEnterWithPlayer();
		KillPlayer();
	}

	protected override void OnTriggerEnterWithPlayer() {
		base.OnTriggerEnterWithPlayer();
		KillPlayer();
	}

	private void KillPlayer() {
		GameManager.instance.Respawn();
	}
}
