
using Raylib_cs;

namespace ArcadeJam;

public class Score {
	int reelPoints = 5, rollPoints = 50;
	float countFactor = 15, maxMulti = 10;
	float[] sizeMultis = [.1f, .2f, .3f, .5f];
	int[] sizeBonus = [50, 100, 200];
	float[] castTimings = [1.3f, 1, 0.8f, 0];
	int[] castScores = [50, 50, 50, 100];
	public int castScore = 0, rollScore = 0, sizeScore, nextCast;
	public float currScore;
	public float multi = 0, castTime = 0, castpenaltyTimer = 0, countTime = 0;
	bool midCast = false, counting = false;

	public void Update(double time) {
		if (counting) {
			countTime += (float)time;
			if (counting) {
				count((float)time);
			}
			return;
		}
		if (midCast) {
			castTime += (float)time;
			if (castTime > 15) {
				castpenaltyTimer += (float)time;
				if (castpenaltyTimer > 0.3f) {
					castScore = Math.Max(0, castScore - 10);
					castpenaltyTimer = 0;
				}

			}
		}

	}
	public void Draw() {
		int y = 19;
		if (nextCast > 0) {
			/*Raylib.DrawTextEx(Assets.monoFont, nextCast + "", new(37 + 6 + (int)(castScore / 10) * 6, 13), Assets.monoFont.BaseSize, 1, Globals.palette[9]);*/
			Raylib.DrawTextEx(Assets.monoFont, "+" + nextCast + "", new(43 + 6 * castScore.ToString().Length, 13), Assets.monoFont.BaseSize, 1, Globals.palette[9]);
		}
		if (midCast || castScore > 0) {
			Raylib.DrawTextEx(Assets.monoFont, "CAST:", new(34, 7), Assets.monoFont.BaseSize, 1, Color.White);
			Raylib.DrawTextEx(Assets.monoFont, castScore + "", new(37, 13), Assets.monoFont.BaseSize, 1, Color.White);
			if (rollScore > 0) {
				Raylib.DrawTextEx(Assets.monoFont, "+" + rollScore, new(37, y), Assets.monoFont.BaseSize, 1, Color.White);
				y += 6;
			}
			if (sizeScore > 0) {
				Raylib.DrawTextEx(Assets.monoFont, "+" + sizeScore, new(37, y), Assets.monoFont.BaseSize, 1, Color.White);
				y += 6;
			}
			y += 3;
			Raylib.DrawTextEx(Assets.monoFont, "MULTI", new(34, y), Assets.monoFont.BaseSize, 1, Color.White);
			y += 6;
			Raylib.DrawTextEx(Assets.monoFont, "X" + ((int)(multi * 100) / 100f), new(37, y), Assets.monoFont.BaseSize, 1, Color.White);

		}
	}
	public void cast(float performance) {
		midCast = true;
		castTime = 0;
		if (counting) {
			for (int i = 0; i < castTimings.Length; i++) {
				if (performance > castTimings[i]) {
					nextCast += castScores[i];
				}
			}
		}
		else {
			multi = 0;
			for (int i = 0; i < castTimings.Length; i++) {
				if (performance > castTimings[i]) {
					castScore += castScores[i];
				}
			}
		}
	}
	public void reelBonus() {
		castScore += reelPoints;
	}
	public void rollBonus() {
		Console.WriteLine("rollin'");
		rollScore += rollPoints;
	}
	public void bite(int size) {
		castScore += nextCast;
		nextCast = 0;
		counting = false;
		sizeScore += sizeBonus[size];
		multi = Math.Max(1, multi);
		multi += sizeMultis[Math.Min(size, sizeMultis.Length)];
		multi = Math.Min(multi, maxMulti);
	}
	public void catchEnd() {
		midCast = false;
		counting = true;
		nextCast = 0;
		if (multi == 0) {
			castScore = 0;
			rollScore = 0;
			sizeScore = 0;
			counting = false;
			multi = 0;
		}
	}

	public void reset() {
		currScore = 0;
		castScore = 0;
	}
	private void count(float time) {
		int amount = (int)(time * countFactor * countTime);
		if (sizeScore > 0) {
			amount = Math.Min(amount, sizeScore);
			sizeScore -= amount;
		}
		else if (rollScore > 0) {
			amount = Math.Min(amount, rollScore);
			rollScore -= amount;
		}
		else if (castScore > 0) {
			amount = Math.Min(amount, castScore);
			castScore -= amount;
		}
		else {
			counting = false;
			countTime = 0;
			multi = 0;
			castScore = nextCast;
			nextCast = 0;
			amount = 0;
		}


		currScore += amount * multi;

	}

}
