using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(Rigidbody))]
public class CameraController : MonoBehaviour {
	private Rigidbody rb;

	[SerializeField] private CinemachineVirtualCamera cam = null;
	private CinemachineTransposer camBody;

	void Start() {
		rb = GetComponent<Rigidbody>();
		camBody = cam.GetCinemachineComponent<CinemachineTransposer>();
	}

	// zoom in and out according to player speed
	void Update() {
		// calculate zoom offset
		float zoomfactor = rb.velocity.magnitude / GameManager.instance.playerSettings.maxVelocityToZoom;
		// clamp zoomfactor
		zoomfactor = (zoomfactor < 1.0f) ? zoomfactor : 1.0f;
		zoomfactor = (zoomfactor > 0.0f) ? zoomfactor : 0.0f;

		// calculate the offset the camera should have
		float goalOffset = GameManager.instance.playerSettings.defaultOffsetZ + zoomfactor * GameManager.instance.playerSettings.maxOffsetZ;

		// Lerp camera offset
		camBody.m_FollowOffset.z = Mathf.Lerp(camBody.m_FollowOffset.z, goalOffset, Time.deltaTime * GameManager.instance.playerSettings.offsetLerpSpeed);
	}
}
