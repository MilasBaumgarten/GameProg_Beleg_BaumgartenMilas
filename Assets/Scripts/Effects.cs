using System.Collections;
using UnityEngine;

public class Effects : MonoBehaviour {
	private float startFixedDeltaTime; // muss ausgelagert werden

	/// <summary>
	/// Draw an arrow from one point to another.
	/// </summary>
	/// <param name="line"> Linerenderer which should be used </param>
	public static void DrawArrow(LineRenderer line, Vector3 center, Vector3 direction) {
		line.SetPositions(new Vector3[] { center, center - direction });
	}

	/// <summary>
	/// Change the Timescale to change the speed of the game.
	/// </summary>
	/// <param name="value"> new Time.timeScale </param>
	public static void SetTime(float value, float duration) {
		if (duration == 0) {
			Time.timeScale = value;
			Time.fixedDeltaTime = value * GameManager.instance.fixedDeltaTimeStart;
			return;
		}

		if (Mathf.Abs(Time.timeScale - value) > 0.01f) {
			// scale time down/ up
			// fixedDeltaTime has to be changed because otherwise the rendering looks like it stutters (low fps)
			if (Time.timeScale < value) {
				Time.timeScale += (1f / duration) * Time.unscaledDeltaTime;
				Time.fixedDeltaTime = Time.timeScale * GameManager.instance.fixedDeltaTimeStart;
			} else {
				Time.timeScale -= (1f / duration) * Time.unscaledDeltaTime;
				Time.fixedDeltaTime = Time.timeScale * GameManager.instance.fixedDeltaTimeStart;
			}
		} else {
			Debug.Log("finished");
		}
	}
}
