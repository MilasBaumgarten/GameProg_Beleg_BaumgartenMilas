using System.IO;
using UnityEngine;

public class WriteJSON {

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
		FileStream file = File.Open(Application.persistentDataPath + saveFileName, FileMode.Open, FileAccess.Read);
		// overwrite highscore data in file with higscore data in the game
		StreamReader reader = new StreamReader(file);

		Highscore highscore = new Highscore();

		string entry;
		// read every line until the end of the file
		while ((entry = reader.ReadLine()) != null){
			highscore.AddEntry(JsonUtility.FromJson<HighscoreEntry>(entry));
		}

		reader.Close();
		file.Close();

		return highscore;
	}
}
