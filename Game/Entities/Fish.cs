
using System.Numerics;
using Raylib_cs;
using YarEngine.Entities;
using YarEngine.Graphics;
using YarEngine.Physics;
using YarEngine.Utils;

namespace ArcadeJam.Entities;

public class Fish : Entity, ScrollObj {

	public int size { get; protected set; } = 1;
	protected float fightSpeed = 2f, idleSpeed = 1, sinMaxAngle = MathF.PI / 180 * 20;
	public float weight { get; protected set; } = 0.5f;
	protected Fishing? bitLure;
	protected float fightTime = 0;
	protected Vector2 direction = new(0, 1);

	public Rect bounds = new(0, 0, 5, 5);
	protected Circle visionShape = new(0, 0, 15);
	protected Collider<Fish> collision;
	protected Collider<Fish> vision;
	protected Sprite sprite = new(Assets.smallFish, 2);




	public Fish(Vector2 start) {
		bounds.Centre = start;
		init();
	}
	protected void init() {
		UpdateIndex = 0;
		sprite.frameDelay = 0.2;
		collision = new(bounds, this);
		vision = new(visionShape, this, "vision");

	}

	public override void Update(double time) {
		idleMove(time);
		vision.DoCollision<Fishing>(seeLure);
		collision.DoCollision<Fishing>(hitLure);
		visionShape.Centre = bounds.Centre;
		if (bitLure != null) {
			fight(bitLure);
			fightTime += (float)time;
		}
		/*sprite.rotation = 0;*/
		sprite.rotation = MathF.Atan2(direction.Y, direction.X) * 180 / MathF.PI + 90;

		sprite.Update(time);
	}
	protected virtual void idleMove(double time) {
	}

	protected virtual void seeLure(Fishing lure) {
		if (bitLure != null || lure.castState == CastState.Bite) {
			return;
		}
		Vector2 displacement = lure.lureBounds.Centre - bounds.Centre;
		if (displacement != Vector2.Zero) {
			displacement = Vector2.Normalize(displacement) * idleSpeed;
		}
		bounds.Centre += displacement;
	}
	protected virtual void fight(Fishing lure) {
		direction = basicFight(lure);
		bounds.Centre += direction * fightSpeed;
	}

	protected virtual void hitLure(Fishing lure) {
		if (lure.castState == CastState.Bite) {
			return;
		}
		bitLure = lure;
		lure.Bite(this);
	}

	public override void Draw(GameCamera cam) {
		sprite.Draw(cam, bounds.Centre);
	}

	public virtual void Catch() {
		collision.Remove();
		vision.Remove();
		shouldRemove = true;
	}

	public virtual void scroll(float dist) {
		bounds.Y += dist * 0.8f;
	}

	public override void OnRemove() {
		collision.Remove();
		vision.Remove();
	}

	protected Vector2 basicFight(Fishing lure) {
		float centre = Globals.gameWidth / 2, maxDist = centre - 5;
		float cenreDist = centre - bounds.Centre.X;
		float centringFact = cenreDist / maxDist;

		Vector2 direction = bounds.Centre - lure.playerBounds.Centre;

		if (direction == Vector2.Zero) {
			direction = new(0, -1);
		}
		/*direction = new(0, -1);*/
		direction = Vector2.Normalize(direction);
		direction = Utils.Rotate(direction, MathF.PI / 2 * MathF.Pow(centringFact, 5));
		direction = Vector2.Normalize(direction);
		return direction;
	}
	protected Vector2 complexFight(Fishing lure) {
		float centre = Globals.gameWidth / 2, maxDist = centre - 5;
		float cenreDist = centre - bounds.Centre.X;
		float centringFact = cenreDist / maxDist;

		Vector2 direction = bounds.Centre - lure.playerBounds.Centre;

		if (direction == Vector2.Zero) {
			direction = new(0, -1);
		}
		/*direction = new(0, -1);*/
		direction = Vector2.Normalize(direction);
		direction = sinMove(direction);
		direction = Utils.Rotate(direction, MathF.PI / 2 * MathF.Pow(centringFact, 3));
		direction = Vector2.Normalize(direction);
		return direction;
	}
	protected Vector2 sinMove(Vector2 dir, float speed = 4) {
		float angle = MathF.Sin(fightTime * speed) * sinMaxAngle;
		return Utils.Rotate(dir, angle);
	}
}

