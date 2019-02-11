using UnityEngine;
using UnityEngine.Events;

public class CollisionBehaviour : MonoBehaviour {

	[SerializeField] private UnityEvent onCollisionEnterWithPlayer = null;
	[SerializeField] private UnityEvent onTriggerEnterWithPlayer = null;

	protected virtual void OnCollisionEnterWithPlayer() {
		onCollisionEnterWithPlayer.Invoke();
	}

	protected virtual void OnTriggerEnterWithPlayer() {
		onTriggerEnterWithPlayer.Invoke();
	}


	private void OnCollisionEnter(Collision collision) {
		switch (collision.gameObject.tag) {
			case "Player":
				OnCollisionEnterWithPlayer();
				break;
			default:
				Debug.Log("No Behaviour for Collision with Object with Tag: " + collision.gameObject.tag + " defined.");
				break;
		}
	}

	private void OnTriggerEnter(Collider collider) {
		
		switch (collider.tag) {
			case "Player":
				OnTriggerEnterWithPlayer();
				break;
			default:
				Debug.Log("No Behaviour for Collision with Object with Tag: " + collider.tag + " defined.");
				break;
		}
	}
}
