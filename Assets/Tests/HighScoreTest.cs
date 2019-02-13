using NUnit.Framework;
using System.IO;
using UnityEngine;

namespace Tests {
	public class HighScoreTest {
		string filename = "/testHighScore.json";

		[Test]
		public void WriteHighscoreToFile() {
			Highscore highscore = AddDebugHighscoreEntries();
			WriteJSON.SaveHighscore(filename, highscore);

			// check if file was created succesfully
			FileAssert.Exists(Application.persistentDataPath + filename,
				"The highscore couldn't be written to the file: " + Application.persistentDataPath + filename);

			// cleanup
			DeleteTestFiles();
		}

		[Test]
		public void WriteAndLoadHighscore() {
			// create highscore
			Highscore highscoreOriginal = AddDebugHighscoreEntries();
			WriteJSON.SaveHighscore(filename, highscoreOriginal);

			// load highscore
			Highscore highscore = WriteJSON.LoadHighscore(filename);

			// check if both highscores have the same amount of entries
			Assert.AreEqual(highscoreOriginal.GetLength(), highscore.GetLength(),
				"The original highscore and the saved and loaded highscore have different lengths: " + highscoreOriginal.GetLength() + "/" + highscore.GetLength() + ".");

			// check if all highscore entries are equal
			for (int i = 0; i < highscoreOriginal.GetLength(); i++) {
				Assert.IsTrue(highscoreOriginal.GetEntry(i).Equals(highscore.GetEntry(i)),
					"Entry " + i + " differs in the saved and read highscore from the original value. \n" +
					"original: " + highscoreOriginal.GetEntry(i).ToString() + "\n" +
					"new     : " + highscore.GetEntry(i).ToString());
			}

			// cleanup
			DeleteTestFiles();
		}

		[Test]
		public void LoadEmptyHighscore() {
			Highscore emptyHighscore = WriteJSON.LoadHighscore("");

			// check if highscore is assigned & empty (should be, because the JSON File can't be found)
			Assert.IsNotNull(emptyHighscore, 
				"WriteJSON.LoadHighScore() should return a new highscore when the highscore file can't be found!");
			Assert.AreEqual(emptyHighscore.GetLength(), 0,
				"WriteJSON.LoadHighScore() should return an empty highscore when the highscore file can't be found!");
		}

		private Highscore AddDebugHighscoreEntries() {
			Highscore highscore = new Highscore();

			HighscoreEntry leet = new HighscoreEntry("1337H4x0r", 0.01f, 1);
			HighscoreEntry noob = new HighscoreEntry("xxNoobooNxx", 999.99f, 99);
			HighscoreEntry larry = new HighscoreEntry("Larry Laffer", 69.69f, 69);

			highscore.AddEntry(leet);
			highscore.AddEntry(noob);
			highscore.AddEntry(larry);

			return highscore;
		}

		private void DeleteTestFiles() {
			File.Delete(Application.persistentDataPath + filename);
		}
	}
}
