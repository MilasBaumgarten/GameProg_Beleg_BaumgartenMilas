using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Effects : MonoBehaviour {
	/// <summary>
	/// Draw an arrow from one point to another.
	/// </summary>
	/// <param name="line"> Linerenderer which should be used </param>
	public static void DrawArrow(LineRenderer line, Vector3 center, Vector3 direction) {
		line.SetPositions(new Vector3[] { center, center - direction });
	}

	/// <summary>
	/// Blend from one PostProcessingVolume to another one
	/// </summary>
	public static void BlendPostProcessingVolumes(PostProcessVolume from, PostProcessVolume to, float duration) {
		if (from.weight > 0.01f) {
			from.weight -= (1.0f / duration) * Time.unscaledDeltaTime;
			to.weight = 1.0f - from.weight;
		} else if (from.weight < 0) {   // clamp (to be sure)
			from.weight = 0;
			to.weight = 1.0f;
		}
	}

	/// <summary>
	/// Changes the PostProcessingVolume's weight over time
	/// </summary>
	public static void SetPostProcessingWeight(PostProcessVolume volume, float value, float duration) {
		if (Mathf.Abs(volume.weight - value) > 0.01f) {
			if (volume.weight < value) {
				volume.weight += (1.0f / duration) * Time.unscaledDeltaTime;
			} else {
				volume.weight -= (1.0f / duration) * Time.unscaledDeltaTime;
			}
		} else {
			// clamp value at the end
			volume.weight = value;
		}
	}

	/// <summary>
	/// Change the Timescale instantaneously to change the speed of the game.
	/// </summary>
	/// <param name="value"> new Time.timeScale </param>
	public static void SetTime(float value) {
		SetTime(value, 0);
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

		// check if the current time scale is near the goal value or not
		if (Mathf.Abs(Time.timeScale - value) > 0.01f) {
			// scale time down/ up
			// fixedDeltaTime has to be changed because otherwise the rendering looks like it stutters (low fps)
			if (Time.timeScale < value) {
				Time.timeScale += (1.0f / duration) * Time.unscaledDeltaTime;
				Time.fixedDeltaTime = Time.timeScale * GameManager.instance.fixedDeltaTimeStart;
			} else {
				// check if the time scale would be less than 0 after the change
				if ((Time.timeScale - (1.0f / duration) * Time.unscaledDeltaTime) > 0) {
					Time.timeScale -= (1.0f / duration) * Time.unscaledDeltaTime;
					Time.fixedDeltaTime = Time.timeScale * GameManager.instance.fixedDeltaTimeStart;
				} else {
					FinishScaling(value);
				}
			}
		} else {
			FinishScaling(value);
		}
	}

	private static void FinishScaling(float value) {
		Time.timeScale = value;
		Time.fixedDeltaTime = value * GameManager.instance.fixedDeltaTimeStart;
	}
}
