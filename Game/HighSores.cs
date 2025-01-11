using Raylib_cs;
using YarEngine.Inputs;

namespace ArcadeJam;

public class HighScores {
	private char[] letters = {'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'
	,'!','<','>',':','.','_',' ','-'};
	private String defaultScore = "0,-----,", scoresPath = "highScores.txt", name = "-----";
	private String[] scores = new String[10];
	private int score, cursor, currentLetter;

	private double time = 0;

	private bool addingScore = false, stickPressed = false;
	public bool finished = false;

	public HighScores(int score) {
		this.score = score;
		LoadScores();
		foreach (String i in scores) {
			Console.WriteLine(i + ", ");
		}
		if (int.Parse(scores[9]) < score) {
			addingScore = true;
		}
	}
	private void LoadScores() {
		if (File.Exists(scoresPath)) {
			scores = File.ReadAllText(scoresPath).Split(",").Take(10).ToArray();
		}
		else {
			//filling the highscores with empty values
			for (int i = 0; i < 5; i++) {
				scores[i * 2] = "-----";
				scores[i * 2 + 1] = "0";
			}
			/*scores[0]= "AGOOG";*/
			/*scores[1]= "150000";*/
			/*scores[2]= "P.TDX";*/
			/*scores[3]= "68000";*/
			/*scores[4]= "CRABW";*/
			/*scores[5]= "22222";*/
		}

	}
	private void saveScores() {
		String text = "";
		bool added = false;
		for (int i = 0; i < 5; i++) {
			if (int.Parse(scores[i * 2 + 1]) < score && !added) {
				text += name + "," + score + ",";
				added = true;
			}
			text += scores[i * 2] + "," + scores[i * 2 + 1] + ",";

		}
		File.WriteAllText(scoresPath, text);
		LoadScores();
	}
	public void Update(double deltaTime) {
		time += deltaTime;
		if (addingScore) {
			AddNewScore(deltaTime);
		}
		else if (time > 90 || (time > 2 && InputHandler.GetButton("A").JustPressed)) {
			finished = true;
		}
	}

	private void AddNewScore(double gameTime) {
		if (time < 0.5) {
			return;
		}
		if (InputHandler.GetButton("U").JustPressed) {
			currentLetter++;
			if (currentLetter >= letters.Length) {
				currentLetter = 0;
			}
			name = name.Remove(cursor, 1).Insert(cursor, letters[currentLetter].ToString());
		}
		else if (InputHandler.GetButton("D").JustPressed) {
			currentLetter--;
			if (currentLetter < 0) {
				currentLetter = letters.Length - 1;
			}
			name = name.Remove(cursor, 1).Insert(cursor, letters[currentLetter].ToString());
		}
		else if (InputHandler.GetButton("L").JustPressed) {
			cursor--;
			cursor = Math.Max(0, cursor);
			currentLetter = Array.IndexOf(letters, name[cursor]);

		}
		else if (InputHandler.GetButton("R").JustPressed) {
			cursor++;
			cursor = Math.Min(4, cursor);
			currentLetter = Array.IndexOf(letters, name[cursor]);
		}
		else if (InputHandler.GetButton("A").JustPressed && time > 2) {
			//save score
			saveScores();
			addingScore = false;
			time = 0.9;

		}
	}

	public void Draw() {

		if (time < 0.66) {
			return;
		}

		/*Raylib.DrawTextEx(Assets.monoFont, "SPLOOSH!", new(Globals.globalGameCentre - (8 * 3), 15), Assets.monoFont.BaseSize, 1, Color.White);*/
		if (time < 1) {
			return;
		}
		if (addingScore) {
			Raylib.DrawTextEx(Assets.monoFont, "NEW HIGH SCORE!", new(Globals.globalGameCentre - (15 * 3), 20), Assets.monoFont.BaseSize, 1, Color.White);
			Raylib.DrawTextEx(Assets.monoFont, "NAME " + name, new(Globals.globalGameCentre - 30, 45), Assets.monoFont.BaseSize, 1, Color.White);
			if ((int)(time * 3) % 2 == 0) {
				Raylib.DrawTexture(Assets.cursor, Globals.globalGameCentre + (cursor * 6), 41, Color.White);
			}
		}
		else {
			Raylib.DrawTextEx(Assets.monoFont, "HIGH SCORES", new(Globals.globalGameCentre - (11 * 3), 30), Assets.monoFont.BaseSize, 1, Color.White);
		}
		for (int i = 0; i < 5; i++) {
			string text = scores[i * 2] + ":" + scores[i * 2 + 1];
			Raylib.DrawTextEx(Assets.monoFont, text, new(Globals.globalGameCentre - (3 * text.Length), 55 + 6 * i), Assets.monoFont.BaseSize, 1, Color.White);
		}
	}
}
