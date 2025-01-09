
using System.Numerics;
using YarEngine;
using YarEngine.Entities;
using YarEngine.Graphics;
using YarEngine.Inputs;
using YarEngine.Physics;

namespace ArcadeJam.Entities;

public class Player : Entity, ScrollObj {

	float accel = 0.3f, maxSpeed = 1.7f, friction = 0.1f, camSpeed = 0.0f, rollDelay = 0.5f;

	Sprite rodSprite = new(Assets.playerRod, 7);
	Sprite rollSprite = new(Assets.playerRoll, 4);
	Sprite reelSprite = new(Assets.playerReel, 4);
	Sprite paddleSprite = new(Assets.playerPaddle, 4, new(0.5f, 0));
	Sprite lureSprite = new(Assets.lure);

	Rect bounds = new(10, 10, 3, 6);
	public Vector2 vel = new(0, 0);
	Fishing fishing;
	PlayerCollision collision;
	GameCamera? camera;

	bool holdingRod = false, lockMove = false, rolling = false;
	float rollTimer = 0;

	public Player() {
		paddleSprite.frameDelay = 0.1;

		rollSprite.frameDelay = 0.1;
		rollSprite.loop = false;
		reelSprite.frameDelay = 1;
		fishing = new(bounds, vel, reelSprite);
		collision = new(bounds, rollSprite, this);
		GameBase.debugScreen.RegisterModule(delegate {
			return new PlayerInfoMod(this, fishing, collision);
		});
	}

	public override void Update(double updateTime) {
		//friction
		if (vel.X > 0) {
			vel.X -= MathF.Min(friction, vel.X);
		}
		else {
			vel.X -= MathF.Max(-friction, vel.X);
		}
		if (vel.Y > 0) {
			vel.Y -= MathF.Min(friction, vel.Y);
		}
		else {
			vel.Y -= MathF.Max(-friction, vel.Y);
		}
		// don't move if you are holding the fishing rod 
		holdingRod = InputHandler.GetButton("A").Held || fishing.castState == CastState.Casting;
		if (!holdingRod) {
			paddle((float)updateTime);
		}
		fishing.Update(updateTime);
		bounds.Centre += vel;
		LockToScreen();
		collision.Update(updateTime);
	}

	private void paddle(float time) {
		//input vector
		Vector2 applied = Vector2.Zero;
		if (InputHandler.GetButton("B").Held) {

		}
		if (InputHandler.GetButton("U").Held) {
			applied.Y -= 1f;
		}
		if (InputHandler.GetButton("D").Held) {
			applied.Y += 1f;
		}
		if (InputHandler.GetButton("L").Held) {
			applied.X -= 1f;
		}
		if (InputHandler.GetButton("R").Held) {
			applied.X += 1f;
		}
		if (applied != Vector2.Zero) {
			applied = Vector2.Normalize(applied);
			paddleSprite.Update(time);
		}
		applied *= accel;
		// only moving if they are slower than max speed (breaks with diagonals)
		if (applied.X > 0 && vel.X < maxSpeed) {
			vel.X = MathF.Min(maxSpeed, vel.X + applied.X);
		}
		else if (applied.X < 0 && vel.X > -maxSpeed) {
			vel.X = MathF.Max(-maxSpeed, vel.X + applied.X);
		}
		if (applied.Y > 0 && vel.Y < maxSpeed) {
			vel.Y = MathF.Min(maxSpeed, vel.Y + applied.Y);
		}
		else if (applied.Y < 0 && vel.Y > -maxSpeed) {
			vel.Y = MathF.Max(-maxSpeed, vel.Y + applied.Y);
		}
	}

	private void LockToScreen() {
		int uiWidth = Assets.ui.Width;
		if (camera == null) {
			return;
		}
		if (bounds.X < camera.offset.X + uiWidth) {
			bounds.X = camera.offset.X + uiWidth;
		}
		if (bounds.X + bounds.Width > camera.offset.X + camera.screenSize.X + uiWidth) {
			bounds.X = camera.offset.X + camera.screenSize.X - bounds.Width + uiWidth;
		}
		if (bounds.Y < camera.offset.Y) {
			bounds.Y = camera.offset.Y;
		}
		if (bounds.Y + bounds.Height > camera.offset.Y + camera.screenSize.Y) {
			bounds.Y = camera.offset.Y + camera.screenSize.Y - bounds.Height;
		}
	}
	public override void Draw(GameCamera cam) {
		camera = cam;

		lureSprite.Draw(cam, fishing.lureBounds);

		CurrentSprite().Draw(cam, bounds.Centre);
		cam.DrawLine(getRodLoc(), fishing.lureBounds.Centre, Globals.palette[9]);

	}
	private Sprite CurrentSprite() {
		if (collision.rolling) {
			return rollSprite;
		}
		if (InputHandler.GetButton("A").Held) {
			if (fishing.castState == CastState.Cast || fishing.castState == CastState.Bite) {
				return reelSprite;
			}

			rodSprite.frame = getRodFrame();
			return rodSprite;
		}
		return paddleSprite;


	}
	private int getRodFrame() {
		if (InputHandler.GetButton("D").Held) {
			if (InputHandler.GetButton("L").Held) {
				return 0;
			}
			else if (InputHandler.GetButton("R").Held) {
				return 2;
			}
			else {
				return 1;
			}
		}
		// facing up
		if (InputHandler.GetButton("U").Held) {
			if (InputHandler.GetButton("L").Held) {
				return 5;
			}
			else if (InputHandler.GetButton("R").Held) {
				return 3;
			}
			else {
				return 4;
			}
		}
		return 6;
	}
	private Vector2 getRodLoc() {
		if (fishing.castState == CastState.Idle) {
			return fishing.lureBounds.Centre;
		}
		Vector2 offset = new(0, -10);
		return bounds.Centre + offset;


	}

	public void scroll(float dist) {
		bounds.Y += dist * 0.3f;
	}
}
