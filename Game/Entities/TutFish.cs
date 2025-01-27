
using System.Numerics;

namespace ArcadeJam.Entities;

public class TutFish : Fish {
	protected Vector2 moveDir = new(0, -1);


	public TutFish(Vector2 start) : base(start) {
		bounds.Centre = start;
		visionShape = new(0, 0, 30);
		size = 0;
		init();
		idleSpeed = 0.7f;
		fightSpeed = 1;
		sprite.frameDelay = 0.2;
	}
	protected override void idleMove(double time) {
		visionShape.Centre = bounds.Centre;


	}
	public override void Catch() {
		base.Catch();
		Globals.levels.endTut();
	}
}
