using System.Numerics;
using YarEngine.Debug;
using YarEngine.Graphics;
using Raylib_cs;

namespace ArcadeJam.Entities;

public class PlayerInfoMod : DebugModule {
	private Vector2 loc;
	private Player player;
	private Fishing fishing;
	private PlayerCollision collision;
	public PlayerInfoMod(Player player, Fishing fishing, PlayerCollision collision) {
		this.player = player;
		this.fishing = fishing;
		this.collision = collision;
		name = "PlayerInfo";
		loc = LoadProp<Vector2>("pos", new(3, 110), "display position");
	}

	public override void DrawFull(GameCamera cam, float pixelScale) {
		RodInputs inputs = fishing.inputs;
		int height = (int)Raylib.MeasureTextEx(font, "P", fontSize, 1).Y + 1;
		Raylib.DrawTextEx(font, "PLAYER INFO", loc, fontSize, 1, Color.White);
		Raylib.DrawTextEx(font, "-castTimer:" + Math.Round(inputs.castTimer * 100) / 100, new(loc.X, loc.Y + 1 * height), fontSize, 1, Color.White);
		Raylib.DrawTextEx(font, "-backTimer:" + Math.Round(inputs.backTimer * 100) / 100, new(loc.X, loc.Y + 2 * height), fontSize, 1, Color.White);
		Raylib.DrawTextEx(font, "-Left:" + Math.Round(inputs.leftTimer * 100) / 100, new(loc.X, loc.Y + 3 * height), fontSize, 1, Color.White);
		Raylib.DrawTextEx(font, "-Right:" + Math.Round(inputs.rightTimer * 100) / 100, new(loc.X, loc.Y + 4 * height), fontSize, 1, Color.White);
		Raylib.DrawTextEx(font, "-health:" + collision.lives, new(loc.X, loc.Y + 5 * height), fontSize, 1, Color.White);
		Raylib.DrawTextEx(font, "-invincibility:" + Math.Round(collision.invincibility * 100) / 100, new(loc.X, loc.Y + 6 * height), fontSize, 1, Color.White);
		Raylib.DrawTextEx(font, "-rolling:" + collision.rolling, new(loc.X, loc.Y + 7 * height), fontSize, 1, Color.White);
	}
}
