using UnityEngine;

[RequireComponent(typeof(CheckPoint))]
public class SpawnPoint : MonoBehaviour {

	[SerializeField] private GameObject playerPrefab = null;

	public GameObject SpawnPlayer() {
		GameObject player = Instantiate(playerPrefab, transform.position, transform.rotation);
		GetComponent<CheckPoint>().SetCheckpoint(0, 0);

		return player;
	}
}
