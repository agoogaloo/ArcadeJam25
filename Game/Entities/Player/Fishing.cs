
using System.Numerics;
using YarEngine.Graphics;
using YarEngine.Inputs;
using YarEngine.Physics;

namespace ArcadeJam.Entities;
public class Fishing {
	float maxRelease = 0.5f, dragFact = 0.85f;

	Sprite lureSprite = new(Assets.lure);
	Rect lureBounds = new(0, 0, 5, 5);
	Rect playerBounds;
	Vector2 playerVel;

	CastState castState = CastState.Idle;
	Vector2 lureVel;
	float lineLen = 0, targetLen = 0;
	float lureWeight = 0.05f;


	public Fishing(Rect pBounds, Vector2 pVel) {
		playerBounds = pBounds;
		playerVel = pVel;
	}

	public Vector2 Update(double time) {
		float lureWeight = 0.9f;
		Vector2 lenVec = playerBounds.Centre - lureBounds.Centre;
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
					lineLen = lenVec.Length();
				}
				break;

			case CastState.Cast:
				if (lureBounds.Intersects(playerBounds)) {
					castState = CastState.Idle;
				}
				// if the lure is pulled away, the line holds them the same distance apart
				if (lenVec.Length() > lineLen) {
					Vector2 neededMove = lenVec * ((lenVec.Length() / lineLen) - 1);

					lureBounds.Centre += neededMove * (1 - lureWeight);
					playerBounds.Centre -= neededMove * lureWeight;
				}
				break;
		}
		return Vector2.Zero;
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
