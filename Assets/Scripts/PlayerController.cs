using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(LineRenderer))]
public class PlayerController : MonoBehaviour {
	private Rigidbody rb;
	private Camera mainCam;
	private LineRenderer dragVisualization;

	private Vector3 mousePos;
	private Vector3 mouseToPlayer;
	private bool playerIsInteracting = false;

	// Start is called before the first frame update
	void Start() {
		rb = GetComponent<Rigidbody>();
		mainCam = Camera.main;
		dragVisualization = GetComponent<LineRenderer>();
		dragVisualization.enabled = false;
	}

	void Update() {
		mousePos = GetMousePositionOnScreen(mainCam);
		// clamp mouse to player direction to a magnitude of 1
		mouseToPlayer = Vector3.ClampMagnitude(transform.position - mousePos, 1.0f);

		// user starts dragging
		if (Input.GetButtonDown("Fire1")) {
			playerIsInteracting = true;
			InteractionStarted();
		}

		// user ends dragging -> shoot player
		if (Input.GetButtonUp("Fire1")) {
			playerIsInteracting = false;
			InteractionEnded();
			rb.AddForce(mouseToPlayer * GameManager.instance.playerSettings.forceStrength, ForceMode.Impulse);
		}

		#region Visualization
		// show stuff 
		if (playerIsInteracting) {
			Effects.DrawArrow(dragVisualization, transform.position, mouseToPlayer);
			Effects.SetTime(GameManager.instance.playerSettings.slowDownTime, GameManager.instance.playerSettings.slowDownLerpDuration);
			Effects.SetPostProcessingWeight(GameManager.instance.slowMotionPostProcessingVolume, 1.0f, GameManager.instance.playerSettings.slowDownLerpDuration);
		} else {
			Effects.SetTime(1.0f, GameManager.instance.playerSettings.speedUpLerpDuration);
			Effects.SetPostProcessingWeight(GameManager.instance.slowMotionPostProcessingVolume, 0.0f, GameManager.instance.playerSettings.speedUpLerpDuration);
		}
		#endregion

		#region Debug
		if (GameManager.instance.playerSettings.enableDebugFeatures) {
			// reset player
			if (Input.GetKeyDown(KeyCode.Space)) {
				transform.position = new Vector3(0, 1, 0);
				rb.velocity = Vector3.zero;
			}
		}
		#endregion
	}

	private void InteractionStarted() {
		dragVisualization.enabled = true;
	}

	private void InteractionEnded() {
		dragVisualization.enabled = false;
	}

	private Vector3 GetMousePositionOnScreen(Camera cam) {
		// get mouse position
		Vector2 pos = Input.mousePosition;
		Vector3 camPos = cam.transform.position;
		// calculate worldposition of mouse
		Vector3 mouseInWorld = cam.ScreenToWorldPoint(new Vector3(pos.x, pos.y, Vector3.Distance(transform.position, camPos)));

		// correct mousePosition to be on player plane (x,y,0)
		//		cameraPosition + (mousePositionInWorld - cameraPosition) * n -> real mousePositionInWorld 
		//		(the camera rotation has to be accounted for because the mouse position is otherwise on a different z Value than the player)
		//		n was calculated by transforming the term: cameraZ + (mouseZ - cameraZ) * n = 0
		//			the correct z value for the mouse position is 0 because the player is on the same axis and the players z value won't be changed (that's why we can just assume it stays 0)
		Vector3 mouseIn2D = camPos + ((mouseInWorld - camPos) * (-camPos.z / (mouseInWorld.z - camPos.z)));

		return mouseIn2D;
	}
}
