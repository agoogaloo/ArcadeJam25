using System.Numerics;
using YarEngine.Graphics;
using YarEngine.Physics;

namespace ArcadeJam.Entities;

public class Ilene : Obstacle {
	int startMoveY = 0, maxWarnDist = 20;
	bool moving = false;
	float speed = 45, warnTime = .5f;
	Sprite warnSprite = new(Assets.warn);
	Vector2 warnLoc = new(10, 0);

	public Ilene(int y, int startMoveY, bool moveLeft = false) {

		bounds = new Circle(-10, y, 5.5f);
		collision = new(bounds, this);
		warnSprite.centered = true;
		this.startMoveY = startMoveY;
		sprite = new(Assets.ileneR, 3, new(-3.5f, 0f));
		if (moveLeft) {
			speed *= -1;
			sprite = new(Assets.ileneL, 3, new(3.5f, 0));
			bounds.CentreX = 200 + 10 - 33;
			warnLoc.X = 200 - 33 - 10;
		}
		sprite.frameDelay = 0.075f;
	}

	public override void Update(double time) {
		base.Update(time);
		sprite.Update(time);
		warnLoc.Y = bounds.Centre.Y;
		if (moving) {
			if (warnTime <= 0) {
				bounds.CentreX += (float)(speed * time);
			}
			else {
				warnTime -= (float)time;
			}

		}
	}

	public override void Draw(GameCamera cam) {
		base.Draw(cam);
		if (bounds.Centre.Y - cam.offset.Y >= startMoveY) {
			moving = true;
			if (bounds.Centre.Y - cam.offset.Y >= startMoveY + maxWarnDist) {
				warnTime = 0;
			}
		}
		if (moving && warnTime > 0) {
			warnSprite.Draw(cam, warnLoc);

		}
	}
}
