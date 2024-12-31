
using System.Numerics;
using YarEngine.Graphics;
using YarEngine.Inputs;
using YarEngine.Physics;

namespace ArcadeJam.Entities;
public class Fishing {
	float maxRelease = 0.5f, dragFact = 0.002f;
	Vector2 lureVel;
	CastState castState = CastState.Idle;

	Sprite lureSprite = new(Assets.lure);
	Rect lureBounds = new(0, 0, 5, 5);
	Rect playerBounds;

	public Fishing(Rect pBounds) {
		playerBounds = pBounds;
	}

	public void Update(double time) {
		dragFact = 0.85f;

		switch (castState) {
			case CastState.Idle:
				lureBounds.Centre = playerBounds.Centre;
				if (InputHandler.GetButton("A").JustReleased) {
					cast(10);
				}
				break;

			case CastState.Casting:
				lureVel *= dragFact;
				lureBounds.Centre += lureVel;
				if (lureVel.Length() <= 0.1) {
					castState = CastState.Cast;
				}
				break;

			case CastState.Cast:
				if (lureBounds.Intersects(playerBounds)) {
					castState = CastState.Idle;
				}
				break;
		}
	}

	private void cast(float strength) {
		lureVel = new(0, -strength);
		castState = CastState.Casting;
	}

	public void Draw(GameCamera cam) {
		cam.DrawLine(playerBounds.Centre, lureBounds.Centre);
		lureSprite.Draw(cam, lureBounds);
	}
}

enum CastState { Idle, Casting, Cast }
