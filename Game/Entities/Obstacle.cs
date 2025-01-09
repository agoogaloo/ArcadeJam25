
using System.Numerics;
using YarEngine.Entities;
using YarEngine.Graphics;
using YarEngine.Physics;

namespace ArcadeJam.Entities;

public class Obstacle : Entity, ScrollObj {

	Sprite sprite = new(Assets.obstacle);
	Rect bounds = new(0, 0, 9, 11);
	Collider<Obstacle> collision;

	public Obstacle(Vector2 loc) {
		bounds.Centre = loc;
		collision = new(bounds, this);
	}

	public override void Update(double updateTime) {
		collision.DoCollision<PlayerCollision>(Collide);
	}
	public override void Draw(GameCamera cam) {
		sprite.Draw(cam, bounds.Centre);
	}
	private void Collide(Collider<PlayerCollision> p) {

		Vector2 bounceDir = Vector2.Normalize(p.Bounds.Centre - bounds.Centre);
		p.collisionObject.Damage(bounceDir * 1.5f);
	}
	public void scroll(float dist) {
		bounds.Y += dist;
	}
}
