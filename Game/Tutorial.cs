
using System.Numerics;
using ArcadeJam.Entities;
using Raylib_cs;
using YarEngine.Graphics;
using YarEngine.Inputs;

namespace ArcadeJam;

public class Tutorial {
	Player player;
	Sprite reelAnim = new(Assets.reelTut, 4);

	public Tutorial(Player player) {
		this.player = player;
		reelAnim.frameDelay = 0.15f;
	}

	public void Update(double time) {
		reelAnim.Update(time);

	}
	public void Draw(GameCamera cam) {
		if (player.fishing.castState == CastState.Bite) {
			Vector2 loc = player.fishing.lureBounds.Centre + new Vector2(0, -12);
			reelAnim.Draw(cam, loc);
		}
		else if (player.fishing.castState != CastState.Casting && player.fishing.castState != CastState.Cast) {
			if (InputHandler.GetButton("A").Held) {

				Raylib.DrawTexture(Assets.castTut, 33 + 84 - Assets.castTut.Width / 2, 20, Color.White);
			}
			else {
				Raylib.DrawTexture(Assets.rodTut, 33 + 84 - Assets.rodTut.Width / 2, 20, Color.White);

			}
		}
	}
}
