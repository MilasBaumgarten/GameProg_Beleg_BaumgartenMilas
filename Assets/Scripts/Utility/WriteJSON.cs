using System.IO;
using UnityEngine;

public static class WriteJSON {

	public static void SaveHighscore(string saveFileName, Highscore highscore) {
		try {
			// open or create new highscore save file
			FileStream file = File.Open(Application.persistentDataPath + saveFileName, FileMode.OpenOrCreate, FileAccess.Write);
			// overwrite highscore data in file with higscore data in the game
			StreamWriter writer = new StreamWriter(file);
			writer.Write(highscore.GetHighscoreAsJSON());
			writer.Flush();
			writer.Close();

			file.Close();

			Debug.Log("HighScore saved successfully in: " + (Application.persistentDataPath + saveFileName));
		} catch (IOException e) {
			Debug.LogError(e);
		}
	}

	public static Highscore LoadHighscore(string saveFileName) {
		// check if file exists
		// use empty highscore when no file was found
		FileInfo fileInfo = new FileInfo(Application.persistentDataPath + saveFileName);
		if (!fileInfo.Exists) {
			return new Highscore();
		}

		FileStream fileStream = File.Open(Application.persistentDataPath + saveFileName, FileMode.Open, FileAccess.Read);
		// overwrite highscore data in file with higscore data in the game
		StreamReader reader = new StreamReader(fileStream);

		Highscore highscore = new Highscore();

		string entry;
		// read every line until the end of the file
		while ((entry = reader.ReadLine()) != null) {
			highscore.AddEntry(new HighscoreEntry(JsonUtility.FromJson<HighscoreEntry>(entry)));
		}

		reader.Close();
		fileStream.Close();

		return highscore;
	}
}
