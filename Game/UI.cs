
using Raylib_cs;
using YarEngine.Graphics;

namespace ArcadeJam;
public class UI {
	public int score;
	public int lives { private get; set; }

	public void Draw(GameCamera cam) {
		Raylib.DrawTexture(Assets.ui, 0, 0, Color.White);
		Raylib.DrawTextEx(Assets.monoFont, score.ToString("D5"), new(2, 20), Assets.monoFont.BaseSize, 1, Color.White);
		Raylib.DrawTextEx(Assets.monoFont, lives + "", new(2, 140), Assets.monoFont.BaseSize, 1, Color.White);
	}
}
