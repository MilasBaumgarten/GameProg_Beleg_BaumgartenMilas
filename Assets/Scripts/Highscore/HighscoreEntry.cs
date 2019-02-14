using System;

[Serializable]
public class HighscoreEntry {
	public HighscoreEntry(HighscoreEntry entry) {
		name = entry.name;
		time = entry.time;
		strokes = entry.strokes;
		points = CalculatePoints(entry.time, entry.strokes);
	}

	public HighscoreEntry(string name, float time, int strokes) {
		this.name = name;
		this.time = time;
		this.strokes = strokes;

		points = CalculatePoints(time, strokes);
	}

	public string name;
	public float time;
	public int strokes;
	public float points { get; private set; }

	private float CalculatePoints(float time, int strokes) {
		return (time * GameManager.instance.highscoreSettings.pointsPerSecond + strokes * GameManager.instance.highscoreSettings.pointsPerStroke);
	}

	public bool Equals(HighscoreEntry entry) {
		return (name.Equals(entry.name)) && (time == entry.time) && (strokes == entry.strokes) && (points == entry.points);
	}

	public override string ToString() {
		return "{" + name + ", " + time + ", " + strokes + ", " + points + "}";
	}
}
