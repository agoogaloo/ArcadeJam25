
using System.Numerics;
using YarEngine.Physics;

namespace ArcadeJam.Entities;

public class PlayerCollision {
	public int lives { get; private set; } = 3;
	Collider<PlayerCollision> collision;
	public float invincibility = 0;

	private Player player;

	public PlayerCollision(Shape bounds, Player player) {
		collision = new(bounds, this);
		this.player = player;
	}

	public void Update(double time) {
		invincibility = (float)Math.Max(0, invincibility - time);

	}
	public void Damage(Vector2 knockBack) {
		if (invincibility == 0) {
			lives--;
			invincibility = 1;
			Globals.ui.lives = lives;
		}
		player.vel = knockBack;

	}

}
