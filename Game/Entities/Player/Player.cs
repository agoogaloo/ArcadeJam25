
using System.Numerics;
using YarEngine;
using YarEngine.Entities;
using YarEngine.Graphics;
using YarEngine.Inputs;
using YarEngine.Physics;

namespace ArcadeJam.Entities;

public class Player : Entity {


	float accel = 0.4f, maxSpeed = 2.0f, friction = 0.1f;
	Rect bounds = new(10, 10, 8, 11);
	Vector2 vel = new(0, 0);
	Sprite sprite = new(Assets.player);
	Fishing fishing;
	Collider<Player> collider;
	GameCamera? camera;

	bool holdingRod = false, lockMove = false;
	float inputTime = 0.3f;
	public float castTimer { get; private set; } = 0;
	public float backTimer { get; private set; } = 0;
	public float leftTimer { get; private set; } = 0;
	public float rightTimer { get; private set; } = 0;

	public Player() {
		fishing = new(bounds, vel);
		collider = new(bounds, this);
		GameBase.debugScreen.RegisterModule(delegate {
			return new PlayerInfoMod(this, fishing);
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
			paddle();
		}
		fishing.Update(updateTime);

		bounds.Centre += vel;
		LockToScreen();
	}


	private void paddle() {
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
		if (camera == null) {
			return;
		}
		if (bounds.X < camera.offset.X) {
			bounds.X = camera.offset.X;
		}
		if (bounds.X + bounds.Width > camera.offset.X + camera.screenSize.X) {
			bounds.X = camera.offset.X + camera.screenSize.X - bounds.Width;
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
		fishing.Draw(cam);
		sprite.Draw(cam, bounds);
	}
}
