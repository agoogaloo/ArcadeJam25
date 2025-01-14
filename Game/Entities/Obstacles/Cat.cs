
using System.Numerics;
using YarEngine.Physics;

namespace ArcadeJam.Entities;
public class Cat : Obstacle {

	float speed = 0.25f;
	Vector2 vel = new(0, 0);
	Circle vision = new(0, 0, 50);
	Collider<Cat> visionCol;

	public Cat(Vector2 loc) {
		bounds = new Rect(0, 0, 9, 2);
		bounds.Centre = loc;
		vision.Centre = loc;
		collision = new(bounds, this);
		visionCol = new(vision, this, "vision");
		sprite = new(Assets.catR, 2, new(0, 1));
		sprite.frameDelay = 0.20f;
	}

	public override void Update(double time) {
		base.Update(time);
		vel = Vector2.Zero;
		visionCol.DoCollision<PlayerCollision>(See);
		bounds.Centre += vel;
		if (vel.X > 0) {
			sprite.Texture = Assets.catR;
		}
		if (vel.X < 0) {
			sprite.Texture = Assets.catL;
		}
		vision.Centre = bounds.Centre;
		sprite.Update(time);
	}
	private void See(Collider<PlayerCollision> pCol) {
		vel = pCol.Bounds.Centre - bounds.Centre;

		vel = Vector2.Normalize(vel) * speed;
	}
	public override void OnRemove() {
		base.OnRemove();
		visionCol.Remove();
	}
}
