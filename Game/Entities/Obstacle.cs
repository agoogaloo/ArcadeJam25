
using System.Numerics;
using YarEngine.Entities;
using YarEngine.Graphics;
using YarEngine.Physics;

namespace ArcadeJam.Entities;

public class Obstacle : Entity, ScrollObj {

	Sprite sprite = new(Assets.rock, 2);
	Rect bounds = new(0, 0, 11, 8);
	Collider<Obstacle> collision;

	public Obstacle(Vector2 loc) {
		bounds.Centre = loc;
		sprite.frame = Random.Shared.Next() % 2;
		collision = new(bounds, this);
	}

	public override void Update(double updateTime) {
		collision.DoCollision<PlayerCollision>(Collide);
	}

	public override void Draw(GameCamera cam) {
		if (bounds.Y - cam.offset.Y > 160) {
			shouldRemove = true;
		}
		sprite.Draw(cam, bounds.Centre);
	}
	private void Collide(Collider<PlayerCollision> p) {

		Vector2 bounceDir = Vector2.Normalize(p.Bounds.Centre - bounds.Centre);
		p.collisionObject.Damage(bounceDir * 1.5f);
	}
	public void scroll(float dist) {
		bounds.Y += dist;
	}
	public override void OnRemove() {
		collision.Remove();
	}
}
