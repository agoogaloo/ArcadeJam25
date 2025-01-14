
using System.Numerics;
using YarEngine.Entities;
using YarEngine.Graphics;
using YarEngine.Physics;

namespace ArcadeJam.Entities;

public abstract class Obstacle : Entity, ScrollObj {

	protected Sprite sprite = new(Assets.rock, 2);
	protected Shape bounds = new Rect(0, 0, 11, 8);
	protected Collider<Obstacle> collision;

	public override void Update(double updateTime) {
		collision.DoCollision<PlayerCollision>(Collide);
	}

	public override void Draw(GameCamera cam) {
		if (bounds.Centre.Y - cam.offset.Y > 160) {
			shouldRemove = true;
		}
		sprite.Draw(cam, bounds.Centre);
	}
	private void Collide(Collider<PlayerCollision> p) {
		Vector2 bounceDir = Vector2.Normalize(p.Bounds.Centre - bounds.Centre);
		p.collisionObject.Damage(bounceDir * 1.5f);
	}
	public void scroll(float dist) {
		bounds.CentreY += dist;
	}
	public override void OnRemove() {
		collision.Remove();
	}
}
