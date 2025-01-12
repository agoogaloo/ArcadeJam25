
using System.Numerics;
using YarEngine.Graphics;
using YarEngine.Inputs;
using YarEngine.Physics;

namespace ArcadeJam.Entities;

public class Fishing {
	float maxTension = 1f, dragFact = 0.85f, lureWeight = 0.05f;
	string[] reelInputs = ["U", "R", "D", "L"];

	public Rect lureBounds { get; private set; } = new(0, 0, 5, 5);
	public Rect playerBounds { get; private set; }
	Vector2 playerVel;

	public CastState castState { get; private set; } = CastState.Idle;
	Vector2 lureVel;
	public RodInputs inputs { get; private set; }
	Sprite reelSprite;
	Collider<Fishing> collider;
	public Fish? bitFish { get; private set; }
	float lineLen = 0, targetLen = 0, lineLenFact = 0.2f;
	float lineWeight = 0.05f;

	float castTimer = 0;

	int reelIndex = 0;


	public Fishing(Rect pBounds, Vector2 pVel, Sprite reelSprite) {
		playerBounds = pBounds;
		this.reelSprite = reelSprite;
		playerVel = pVel;
		collider = new(lureBounds, this);
		collider.Remove();
		inputs = new(this);
	}

	public void Update(double time) {
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
					Globals.score.catchEnd();
					collider.Remove();
				}
				DoLinePhysics();
				break;
			case CastState.Bite:
				biteUpdate(time);
				break;
		}
		return;

	}
	private void biteUpdate(double time) {
		if (bitFish == null) {
			return;
		}

		lureBounds.Centre = bitFish.bounds.Centre;
		Reel();
		Globals.levels.FishScroll(bitFish.bounds.Centre);
		if (lineLen <= 0.2) {
			Globals.score.catchEnd();
			castState = CastState.Idle;
			bitFish.Catch();
			collider.Remove();
			bitFish = null;
		}
		DoLinePhysics();

	}

	public void Cast(float performance, float angle = 0) {
		Globals.score.cast(performance);
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
					targetLen -= 1f;
					reelIndex %= reelInputs.Length;
				}
			}
			return;
		}
		if (InputHandler.GetButton(reelInputs[reelIndex]).JustPressed) {
			reelIndex++;
			targetLen = Math.Max(0, targetLen - (1 + (1 - lineWeight) * 3));
			reelSprite.Update(2);
			if (castState == CastState.Bite) {
				Globals.score.reelBonus();
			}
		}
		reelIndex %= reelInputs.Length;
	}

	private void DoLinePhysics() {
		Vector2 lenVec = playerBounds.Centre + new Vector2(0, -9) - lureBounds.Centre;
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
			Globals.score.bite(fish.size);
		}
	}
	public void Remove() {
		collider.Remove();
	}
}

public enum CastState { Idle, Casting, Cast, Bite }
