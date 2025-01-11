
namespace ArcadeJam;

public class Score {
	float[] castTimings = [1.3f, 1, 0.8f];
	float[] sizeMultis = [0, 1, 1.5f, 2];
	int[] castScores = [100, 50, 50, 100];
	public int currScore, castScore = 0;
	public float multi = 0;

	public void Update(double time) {

	}
	public void cast(float performance) {
		multi = 0;
		for (int i = 0; i < castTimings.Length; i++) {
			if (performance > castTimings[i]) {
				castScore += castScores[i];
			}
		}
	}
	public void sizeMulti(int size) {
		multi = sizeMultis[Math.Min(size, sizeMultis.Length)];
	}

	public void catchEnd() {
		currScore += (int)(castScore * multi);
		castScore = 0;
		multi = 0;
	}

	public void reset() {
		currScore = 0;
		castScore = 0;
	}

}
