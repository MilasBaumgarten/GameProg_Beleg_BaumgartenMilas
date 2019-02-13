using System;

[Serializable]
public class HighscoreEntry {
	public HighscoreEntry(string name, float time, int strokes) {
		this.name = name;
		this.time = time;
		this.strokes = strokes;

		points = CalculatePoints(time, strokes);
	}

	public string name;
	public float time;
	public int strokes;
	public float points;

	private float CalculatePoints(float time, int strokes) {
		return (time * 100 + strokes * 100);
	}

	public float GetPoints() {
		return points;
	}

	public bool Equals(HighscoreEntry entry) {
		return (name.Equals(entry.name)) && (time == entry.time) && (strokes == entry.strokes) && (points == entry.GetPoints());
	}
}
