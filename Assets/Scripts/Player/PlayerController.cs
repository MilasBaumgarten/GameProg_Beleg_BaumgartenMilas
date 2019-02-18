using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {
	private Rigidbody rb;
	private Camera mainCam;
	[SerializeField] private GameObject dragVisualization = null;
	private Material dragMaterial;

	private Vector3 mousePos;
	private Vector3 mouseToPlayer;
	private bool playerIsInteracting = false;

	private PlayerScriptableObject playerSettings;

	// Start is called before the first frame update
	void Start() {
		rb = GetComponent<Rigidbody>();
		mainCam = Camera.main;
		dragMaterial = dragVisualization.GetComponent<MeshRenderer>().material;
		playerSettings = GameManager.instance.playerSettings;

		// scale dragVisualization up to show 
		dragVisualization.transform.localScale *= playerSettings.maxDistanceMouseToPlayer * 1.9f;
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
			dragMaterial.SetFloat("_StepCutoff", (1 - mouseToPlayer.magnitude) / 2.0f + playerSettings.minDragStepValue);

			// rotate drag visualization towards mouse
			// 180° have to be added when y is negative because Unity flips the angle here when it is bigger then 180°
			if (mouseToPlayer.y > 0) {
				dragVisualization.transform.rotation = Quaternion.Euler(0, 180.0f, Mathf.Atan(mouseToPlayer.x / mouseToPlayer.y) * Mathf.Rad2Deg);
			} else {
				dragVisualization.transform.rotation = Quaternion.Euler(0, 180.0f, Mathf.Atan(mouseToPlayer.x / mouseToPlayer.y) * Mathf.Rad2Deg + 180);
			}
			
			Effects.SetTime(playerSettings.slowDownTime, playerSettings.slowDownLerpDuration);
			Effects.SetPostProcessingWeight(GameManager.instance.slowMotionPostProcessingVolume, 1.0f, playerSettings.slowDownLerpDuration);
		} else {
			dragMaterial.SetFloat("_StepCutoff", 0.51f);
			Effects.SetTime(1.0f,playerSettings.speedUpLerpDuration);
			Effects.SetPostProcessingWeight(GameManager.instance.slowMotionPostProcessingVolume, 0.0f, playerSettings.speedUpLerpDuration);
		}
		#endregion
	}

	private void InteractionStarted() {
		playerIsInteracting = true;
	}

	private void InteractionEnded() {
		playerIsInteracting = false;
		// shoot player
		rb.AddForce(mouseToPlayer * GameManager.instance.playerSettings.forceStrength, ForceMode.Impulse);

		GameManager.instance.stats.PlayerShot();

		ShootParticles();
	}

	private void ShootParticles() {
		// insert cool stuff
	}

	private bool MouseInPlayerRange() {
		return (Vector3.Distance(mousePos, transform.position) <= playerSettings.maxDistanceMouseToPlayer);
	}

	private Vector3 GetMousePositionOnScreen(Camera cam) {
		Vector2 pos = Input.mousePosition;
		Vector3 camPos = cam.transform.position;
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
