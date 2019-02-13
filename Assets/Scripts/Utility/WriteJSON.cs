using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WriteJSON : MonoBehaviour {
	private HighScore highScore = new HighScore();
	private string saveFileName = "/HighScore.json";

	void Start() {
		HighScoreEntry leet = new HighScoreEntry("1337H4x0r", 0.01f, 1);
		HighScoreEntry noob = new HighScoreEntry("xxNoobooNxx", 999.99f, 99);
		HighScoreEntry larry = new HighScoreEntry("Larry Laffer", 69.69f, 69);

		highScore.AddEntry(leet);
		highScore.AddEntry(noob);
		highScore.AddEntry(larry);


		// -> Test
		SaveHighscore();
		Debug.Log(highScore.GetHighScoreAsJSON());

		highScore = new HighScore();
		Debug.Log(highScore.GetHighScoreAsJSON());

		LoadHighScore();
		Debug.Log(highScore.GetHighScoreAsJSON());
	}

	public void SaveHighscore() {
		try {
			// open or create new highscore save file
			FileStream file = File.Open(Application.persistentDataPath + saveFileName, FileMode.OpenOrCreate, FileAccess.Write);
			// overwrite highscore data in file with higscore data in the game
			StreamWriter writer = new StreamWriter(file);
			writer.Write(highScore.GetHighScoreAsJSON());
			writer.Flush();
			writer.Close();

			Debug.Log("HighScore saved successfully in: " + (Application.persistentDataPath + saveFileName));
		} catch (IOException e) {
			Debug.LogError(e);
		}
	}

	public void LoadHighScore() {
		FileStream file = File.Open(Application.persistentDataPath + saveFileName, FileMode.Open, FileAccess.Read);
		// overwrite highscore data in file with higscore data in the game
		StreamReader reader = new StreamReader(file);

		string entry;
		highScore = new HighScore();
		// read every line until the end of the file
		while ((entry = reader.ReadLine()) != null){
			highScore.AddEntry(JsonUtility.FromJson<HighScoreEntry>(entry));
		}
	}

	private class HighScore {
		private List<HighScoreEntry> highScore = new List<HighScoreEntry>();

		public void AddEntry(HighScoreEntry entry) {
			highScore.Add(entry);
		}

		public HighScoreEntry GetEntry(int index) {
			return highScore[index];
		}

		public string GetHighScoreAsJSON() {
			string score = "";

			// save every highscore entry into a json string, seperated by a new line
			foreach(HighScoreEntry entry in highScore) {
				score += JsonUtility.ToJson(entry);
				score += Environment.NewLine;
			}

			// remove last unnecessary new line
			score = score.Trim(Environment.NewLine.ToCharArray());

			return score;
		}
	}

	[Serializable]
	public class HighScoreEntry {
		public HighScoreEntry(string name, float time, int strokes) {
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
	}
}
