using System;
using System.Collections.Generic;
using UnityEngine;

public class Highscore{
	private List<HighscoreEntry> highscore = new List<HighscoreEntry>();

	public void AddEntry(HighscoreEntry entry) {
		highscore.Add(entry);
	}

	public HighscoreEntry GetEntry(int index) {
		return highscore[index];
	}

	public int GetLength() {
		return highscore.Count;
	}

	public string GetHighscoreAsJSON() {
		string score = "";

		// save every highscore entry into a json string, seperated by a new line
		foreach (HighscoreEntry entry in highscore) {
			score += JsonUtility.ToJson(entry);
			score += Environment.NewLine;
		}

		// remove last unnecessary new line
		score = score.Trim(Environment.NewLine.ToCharArray());

		return score;
	}

	// sort the entries in ascending order
	// source: https://stackoverflow.com/questions/3309188/how-to-sort-a-listt-by-a-property-in-the-object
	public void Sort() {
		highscore.Sort((x, y) => x.points.CompareTo(y.points));
	}
}
