using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {
	private Rigidbody rb;
	private Camera mainCam;
	private LineRenderer dragVisualization;

	private Vector3 mousePos;
	private Vector3 mouseToPlayer;
	private bool playerIsInteracting = false;

	private PlayerScriptableObject playerSettings;

	// Start is called before the first frame update
	void Start() {
		rb = GetComponent<Rigidbody>();
		mainCam = Camera.main;
		dragVisualization = GetComponentInChildren<LineRenderer>();
		playerSettings = GameManager.instance.playerSettings;
		dragVisualization.enabled = false;
	}

	void Update() {
		mousePos = GetMousePositionOnScreen(mainCam);
		// clamp mouse to player direction to a magnitude of 1
		mouseToPlayer = Vector3.ClampMagnitude(transform.position - mousePos, 1.0f);

		// user starts dragging
		if (Input.GetButtonDown("Fire1") && MouseInPlayerRange()) {
			InteractionStarted();
		}

		// user ends dragging
		if (Input.GetButtonUp("Fire1") && playerIsInteracting) {
			InteractionEnded();
		}

		// respawn player
		if (Input.GetKeyDown(KeyCode.R)) {
			GameManager.instance.Respawn();
		}

		if (playerSettings.enableDebugFeatures) {
			// reset player
			if (Input.GetKeyDown(KeyCode.F9)) {
				GameManager.instance.Restart();
			}
		}

		#region Visualization
		// show stuff 
		if (playerIsInteracting) {
			Effects.DrawArrow(dragVisualization, transform.position, mouseToPlayer);
			Effects.SetTime(playerSettings.slowDownTime, playerSettings.slowDownLerpDuration);
			Effects.SetPostProcessingWeight(GameManager.instance.slowMotionPostProcessingVolume, 1.0f, playerSettings.slowDownLerpDuration);
		} else {
			Effects.SetTime(1.0f,playerSettings.speedUpLerpDuration);
			Effects.SetPostProcessingWeight(GameManager.instance.slowMotionPostProcessingVolume, 0.0f, playerSettings.speedUpLerpDuration);
		}
		#endregion
	}

	private void InteractionStarted() {
		playerIsInteracting = true;
		dragVisualization.enabled = true;
	}

	private void InteractionEnded() {
		playerIsInteracting = false;
		dragVisualization.enabled = false;
		// shoot player
		rb.AddForce(mouseToPlayer * GameManager.instance.playerSettings.forceStrength, ForceMode.Impulse);

		GameManager.instance.stats.PlayerShot();
	}

	private bool MouseInPlayerRange() {
		return (Vector3.Distance(mousePos, transform.position) <= playerSettings.maxDistanceMouseToPlayer);
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
