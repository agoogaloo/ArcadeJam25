
using Raylib_cs;
using YarEngine.Graphics;

namespace ArcadeJam;
public class UI {
	public int lives { private get; set; }

	public void Draw(GameCamera cam) {
		Raylib.DrawTexture(Assets.ui, 0, 0, Color.White);
		Raylib.DrawTextEx(Assets.monoFont, Globals.score.currScore.ToString("D5"), new(2, 20), Assets.monoFont.BaseSize, 1, Color.White);
		Raylib.DrawTextEx(Assets.monoFont, lives + "", new(2, 140), Assets.monoFont.BaseSize, 1, Color.White);
		// cast score
		if (Globals.score.castScore != 0) {
			Raylib.DrawTextEx(Assets.monoFont, Globals.score.castScore + "", new(33, 20), Assets.monoFont.BaseSize, 1, Color.White);
			Raylib.DrawTextEx(Assets.monoFont, Globals.score.multi + "", new(33, 26), Assets.monoFont.BaseSize, 1, Color.White);
		}
	}
}
