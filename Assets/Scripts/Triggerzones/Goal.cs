public class Goal : CollisionBehaviour {
	protected override void OnCollisionEnterWithPlayer() {
		base.OnCollisionEnterWithPlayer();
		EnterGoal();
	}

	protected override void OnTriggerEnterWithPlayer() {
		base.OnTriggerEnterWithPlayer();
		EnterGoal();
	}

	private void EnterGoal() {
		GameManager.instance.PlayerEnteredGoal();
	}
}
