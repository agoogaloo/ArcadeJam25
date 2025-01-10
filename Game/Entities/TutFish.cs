
using System.Numerics;

namespace ArcadeJam.Entities;

public class TutFish : Fish {
	protected Vector2 moveDir = new(0, -1);


	public TutFish(Vector2 start) : base(start) {
		idleSpeed = 0.7f;
		fightSpeed = 1;
		sprite.frameDelay = 0.2;
		bounds.Centre = start;
		visionShape = new(0, 0, 30);
		collision = new(bounds, this);
		vision = new(visionShape, this, "vision");
	}
	protected override void idleMove(double time) {
		visionShape.Centre = bounds.Centre;


	}
	public override void Catch() {
		base.Catch();
		Globals.levels.endTut();
	}
}
