
using System.Numerics;

namespace ArcadeJam.Entities;
public class Rock : Obstacle {

	public Rock(Vector2 loc) {
		bounds.Centre = loc;
		sprite.frame = Random.Shared.Next() % 2;
		collision = new(bounds, this);
	}
}
