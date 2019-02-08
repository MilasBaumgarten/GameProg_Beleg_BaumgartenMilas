using UnityEngine;

public class CheckPoint : MonoBehaviour {

	private void OnCollisionEnter(Collision collision) {
		// register last checkpoint which was touched by the player in the GameManager
		if (collision.gameObject.CompareTag("Player")) {
			GameManager.instance.lastactivatedSpawn = gameObject;
		}
	}
}
