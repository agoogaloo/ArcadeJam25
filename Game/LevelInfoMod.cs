using System.Numerics;
using YarEngine.Debug;
using YarEngine.Graphics;
using Raylib_cs;

namespace ArcadeJam.Entities;

public class LevelInfoMod : DebugModule {
	private Vector2 loc;
	public LevelInfoMod() {
		name = "LevelInfo";
		loc = LoadProp<Vector2>("pos", new(0, 10), "display position");
	}

	public override void DrawFull(GameCamera cam, float pixelScale) {
		int height = (int)Raylib.MeasureTextEx(font, "P", fontSize, 1).Y + 1;
		Raylib.DrawTextEx(font, "LEVEL INFO", loc, fontSize, 1, Color.White);
		Raylib.DrawTextEx(font, "-gameOver:" + Globals.levels.gameOver, new(loc.X, loc.Y + 1 * height), fontSize, 1, Color.White);
	}
}
