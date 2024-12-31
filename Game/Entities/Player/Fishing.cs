
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
	float lineLen = 0, targetLen = 0, lineLenFact = 0.2f;
	float lureWeight = 0.05f;

	string[] reelInputs = ["U", "R", "D", "L"];
	int reelIndex = 0;


	public Fishing(Rect pBounds, Vector2 pVel) {
		playerBounds = pBounds;
		playerVel = pVel;
	}

	public Vector2 Update(double time) {
		lureWeight = 0.9f;
		lineLenFact = 0.25f;
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
				if (lureVel.Length() <= 0.1 || InputHandler.GetButton("A").JustPressed) {
					castState = CastState.Cast;
					lineLen = lenVec.Length();
					targetLen = lineLen;
				}
				break;

			case CastState.Cast:
				// shorten the line if your reeling
				reel();
				lineLen -= (lineLen - targetLen) * lineLenFact;
				// finish fishing if your done reeling
				if (lineLen <= 1) {
					castState = CastState.Idle;
				}
				// make sure you dont get seperated by more than the line length
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
		reelIndex = -1;
	}
	private void reel() {
		// if reeling hasnt started yet, then we start with the next direction pressed
		if (reelIndex == -1) {
			for (int i = 0; i < reelInputs.Length; i++) {
				if (InputHandler.GetButton(reelInputs[i]).JustPressed) {
					reelIndex = i + 1;
					targetLen -= 2f;
					reelIndex %= reelInputs.Length;
				}
			}
			return;
		}
		if (InputHandler.GetButton(reelInputs[reelIndex]).JustPressed) {
			reelIndex++;
			targetLen = Math.Max(0.1f, targetLen - 6f);
		}
		reelIndex %= reelInputs.Length;



	}

	public void Draw(GameCamera cam) {
		cam.DrawLine(playerBounds.Centre, lureBounds.Centre);
		lureSprite.Draw(cam, lureBounds);
	}
}
enum CastState { Idle, Casting, Cast }
