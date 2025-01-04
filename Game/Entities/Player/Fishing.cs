
using System.Numerics;
using YarEngine.Graphics;
using YarEngine.Inputs;
using YarEngine.Physics;

namespace ArcadeJam.Entities;

public class Fishing {
	float maxTension = 1f, dragFact = 0.85f, lureWeight = 0.05f;
	string[] reelInputs = ["U", "R", "D", "L"];

	Sprite lureSprite = new(Assets.lure);
	public Rect lureBounds { get; private set; } = new(0, 0, 5, 5);
	public Rect playerBounds { get; private set; }
	Vector2 playerVel;

	public CastState castState { get; private set; } = CastState.Idle;
	Vector2 lureVel;
	public RodInputs inputs { get; private set; }
	Collider<Fishing> collider;
	Fish? bitFish;
	float lineLen = 0, targetLen = 0, lineLenFact = 0.2f;
	float lineWeight = 0.05f;

	float castTimer = 0;

	int reelIndex = 0;


	public Fishing(Rect pBounds, Vector2 pVel) {
		playerBounds = pBounds;
		playerVel = pVel;
		collider = new(lureBounds, this);
		collider.Remove();
		inputs = new(this);
	}

	public Vector2 Update(double time) {
		inputs.Update(time);

		switch (castState) {
			case CastState.Idle:
				lureBounds.Centre = playerBounds.Centre;
				break;

			case CastState.Casting:
				Vector2 lenVec = playerBounds.Centre - lureBounds.Centre;
				lureVel *= dragFact;
				lineWeight = lureWeight;
				lureBounds.Centre += lureVel;
				if (lureVel.Length() <= 0.5 || InputHandler.GetButton("A").JustPressed) {
					castState = CastState.Cast;
					lineLen = lenVec.Length();
					targetLen = lineLen;
					collider.Add();
				}
				break;

			case CastState.Cast:
				// shorten the line if your reeling
				Reel();
				if (lineLen <= 0.2) {
					castState = CastState.Idle;
					collider.Remove();
				}
				DoLinePhysics();
				break;
			case CastState.Bite:
				lureBounds.Centre = bitFish.bounds.Centre;
				Reel();
				if (lineLen <= 0.2) {
					castState = CastState.Idle;
					bitFish.Catch();
					collider.Remove();
				}
				DoLinePhysics();
				break;
		}
		return Vector2.Zero;

	}

	public void Cast(float performance, float angle = 0) {
		float strength = 5 + performance * 5;

		angle *= MathF.PI / 180;//converting to radians
		float x = MathF.Sin(angle) * strength;
		float y = -MathF.Cos(angle) * strength;
		lureVel = new(x, y);
		castState = CastState.Casting;
		reelIndex = -1;
	}

	private void Reel() {
		// if reeling hasnt started yet, then we start with the next direction pressed
		if (!InputHandler.GetButton("A").Held) {
			return;
		}
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
			targetLen = Math.Max(0.1f, targetLen - 5f);
		}
		reelIndex %= reelInputs.Length;
	}

	private void DoLinePhysics() {
		Vector2 lenVec = playerBounds.Centre - lureBounds.Centre;
		lineLen -= (lineLen - targetLen) * lineLenFact;
		maxTension = 10;
		// make sure you dont get seperated by more than the line length
		if (lenVec.Length() > lineLen) {
			// if its pulled more than the tension shuold allow, then line is let out
			if (lenVec.Length() - lineLen > maxTension) {
				Console.WriteLine("too much tension!" + (lenVec.Length() - lineLen));
				lineLen = lenVec.Length() - maxTension;
				targetLen += maxTension;
			}

			Vector2 neededMove = Vector2.Normalize(lenVec);
			neededMove *= (lenVec.Length() - lineLen);
			lureBounds.Centre += neededMove * (1 - lineWeight);
			playerBounds.Centre -= neededMove * lineWeight;
			if (bitFish != null) {
				bitFish.bounds.Centre = lureBounds.Centre;
			}
		}
	}

	public void Bite(Fish fish) {
		if (castState == CastState.Cast || castState == CastState.Bite) {
			bitFish = fish;
			lineWeight = fish.weight;
			castState = CastState.Bite;
		}
	}

	public void Draw(GameCamera cam) {
		cam.DrawLine(playerBounds.Centre, lureBounds.Centre, Globals.palette[9]);
		lureSprite.Draw(cam, lureBounds);
	}
}

public enum CastState { Idle, Casting, Cast, Bite }
