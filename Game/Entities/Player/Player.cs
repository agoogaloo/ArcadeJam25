
using System.Numerics;
using YarEngine.Entities;
using YarEngine.Graphics;
using YarEngine.Inputs;
using YarEngine.Physics;

namespace ArcadeJam.Entities;

public class Player : Entity {


	float accel = 0.5f, maxSpeed = 2.0f, friction = 0.1f;
	Rect bounds = new(10, 10, 8, 11);
	Vector2 vel = new(0, 0);
	Sprite sprite = new(Assets.player);
	Fishing fishing;

	public Player() {
		fishing = new(bounds, vel);

	}

	public override void Update(double updateTime) {
		accel = 0.3f;
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
		if (InputHandler.GetButton("B").Held) {
			paddle();
		}
		fishing.Update(updateTime);

		bounds.Centre += vel;
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

	public override void Draw(GameCamera cam) {
		sprite.Draw(cam, bounds);
		fishing.Draw(cam);
	}
}
