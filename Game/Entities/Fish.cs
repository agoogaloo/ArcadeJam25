
using System.Numerics;
using YarEngine.Entities;
using YarEngine.Graphics;
using YarEngine.Physics;

namespace ArcadeJam.Entities;

public class Fish : Entity {

	float speed = 2;
	public float weight { get; private set; } = 0.9f;
	bool bitLure = false;

	Rect bounds = new(0, 0, 5, 5);
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
	}

	private void seeLure(Fishing lure) {
		if (bitLure) {
			return;
		}
		Vector2 displacement = lure.lureBounds.Centre - bounds.Centre;
		if (displacement != Vector2.Zero) {
			displacement = Vector2.Normalize(displacement) * speed;
		}
		bounds.Centre += displacement;

	}
	private void hitLure(Fishing lure) {
		bitLure = true;
		lure.Bite(this);

	}
	public override void Draw(GameCamera cam) {
		sprite.Draw(cam, bounds.Centre);
	}

}

