using System.Numerics;
using YarEngine.Graphics;
using YarEngine.Inputs;
using YarEngine.Physics;

namespace ArcadeJam.Entities;

public class PlayerCollision {
	float rollTime = 0.6f, rollInvincibility = 0.35f;
	public int lives { get; private set; } = 3;
	Collider<PlayerCollision> collision;
	Sprite rollSprite;
	public float invincibility = 0;
	public bool rolling = false, hasDodged = false;
	float rollTimer = 0;

	private Player player;

	public PlayerCollision(Shape bounds, Sprite rollSprite, Player player) {
		collision = new(bounds, this);
		this.player = player;
		this.rollSprite = rollSprite;
		Globals.ui.lives = lives;
	}

	public void Update(double time) {
		rollTime = 0.6f;
		if (InputHandler.GetButton("B").JustPressed && rollTimer == 0) {
			rollTimer = rollTime;
			rolling = true;
			hasDodged = false;
			invincibility = MathF.Max(invincibility, 0.3f);
			rollSprite.frame = 0;
			rollSprite.playing = true;
		}
		rollSprite.Update(time);
		invincibility = MathF.Max(0, (float)(invincibility - time));
		rollTimer = (float)Math.Max(0, rollTimer - time);
		if (rollTimer == 0) {
			rolling = false;
		}
	}
	public void Damage(Vector2 knockBack) {
		if (invincibility > 0 && rolling) {
			if (!hasDodged && player.fishing.castState == CastState.Bite) {
				hasDodged = true;
				Globals.score.rollBonus();
			}
			return;
		}
		if (invincibility == 0) {
			lives--;
			invincibility = 1;
			Globals.ui.lives = lives;
		}
		player.vel = knockBack;
		if (lives <= 0) {
			player.shouldRemove = true;
			Globals.levels.GameOver();
		}

	}
	public void Remove() {
		collision.Remove();
	}

}
