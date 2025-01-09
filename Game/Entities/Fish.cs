
using System.Numerics;
using YarEngine.Entities;
using YarEngine.Graphics;
using YarEngine.Physics;

namespace ArcadeJam.Entities;

public class Fish : Entity, ScrollObj {

	float fightSpeed = 2f, idleSpeed = 1;
	public float weight { get; private set; } = 0.3f;
	Fishing? bitLure;

	public Rect bounds = new(0, 0, 5, 5);
	Circle visionShape = new(0, 0, 15);
	Collider<Fish> collision;
	Collider<Fish> vision;
	Sprite sprite = new(Assets.fish);



	public Fish(Vector2 start) {
		bounds.Centre = start;
		collision = new(bounds, this);
		vision = new(visionShape, this, "vision");

	}

	public override void Update(double updateTime) {
		vision.DoCollision<Fishing>(seeLure);
		collision.DoCollision<Fishing>(hitLure);
		visionShape.Centre = bounds.Centre;
		if (bitLure != null) {
			fight(bitLure);
		}
	}

	private void seeLure(Fishing lure) {
		if (bitLure != null || lure.castState == CastState.Bite) {
			return;
		}
		Vector2 displacement = lure.lureBounds.Centre - bounds.Centre;
		if (displacement != Vector2.Zero) {
			displacement = Vector2.Normalize(displacement) * idleSpeed;
		}
		bounds.Centre += displacement;
	}
	private void fight(Fishing lure) {
		Vector2 direction = lure.playerBounds.Centre - bounds.Centre;
		if (direction == Vector2.Zero) {
			direction = new(0, 1);
		}
		direction = -Vector2.Normalize(direction);
		bounds.Centre += direction * fightSpeed;
	}

	private void hitLure(Fishing lure) {
		bitLure = lure;
		lure.Bite(this);
	}

	public override void Draw(GameCamera cam) {
		sprite.Draw(cam, bounds.Centre);
	}

	public void Catch() {
		shouldRemove = true;
		collision.Remove();
		vision.Remove();
	}

	public void scroll(float dist) {
		bounds.Y += dist * 0.8f;
	}
}

