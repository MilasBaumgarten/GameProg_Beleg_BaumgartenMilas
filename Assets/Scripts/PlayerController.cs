using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {
	private Rigidbody rb;
	private Camera mainCam;

	private Vector3 mousePos;

	// Start is called before the first frame update
	void Start() {
		rb = GetComponent<Rigidbody>();
		mainCam = Camera.main;
	}

	// Update is called once per frame
	void Update() {
		mousePos = GetMousePositionOnScreen(mainCam);

		if (Input.GetButtonDown("Fire1")) {
			Vector2 forceDirection = transform.position - mousePos;
			rb.AddForce(forceDirection * GameManager.instance.playerSettings.forceStrength, ForceMode.Impulse);
		}

		#region Debug
		if (GameManager.instance.playerSettings.enableDebugFeatures) {
			// reset player
			if (Input.GetKeyDown(KeyCode.Space)) {
				transform.position = new Vector3(0, 1, 0);
				rb.velocity = Vector3.zero;
			}

			Debug.DrawLine(transform.position, mousePos, Color.green);
		}
		#endregion
	}

	private Vector2 GetMousePositionOnScreen(Camera cam) {
		// get mouse position
		Vector2 pos = Input.mousePosition;
		// calculate worldposition of mouse
		Vector2 mp = cam.ScreenToWorldPoint(new Vector3(pos.x, pos.y, -cam.transform.position.z));
		// return as 2D Vector
		return mp;
	}
}
