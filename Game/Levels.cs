
using System.Numerics;
using ArcadeJam.Entities;
using YarEngine.Entities;
using YarEngine.Graphics;

namespace ArcadeJam;

public class Levels {
	GameCamera camera;
	float camSpeed = 0.1f, distanceCount = 0;
	bool fishBit = false;


	public Levels(GameCamera camera) {
		this.camera = camera;
	}

	public void Update(double time) {
		if (fishBit) {
			fishBit = false;
			return;
		}
		camera.offset.Y -= camSpeed;
		distanceCount += camSpeed;
		if (distanceCount > 5) {
			distanceCount = 0;
			spawnElem();
		}

	}
	public void FishScroll(Vector2 fishBounds) {
		float oldY = camera.offset.Y;
		camera.offset.Y = MathF.Min(camera.offset.Y, fishBounds.Y - 20);
		distanceCount += oldY - camera.offset.Y;
		if (distanceCount > 5) {
			distanceCount = 0;
			spawnElem();
		}
		fishBit = true;
	}
	private void spawnElem() {
		int x = Random.Shared.Next() % (int)camera.screenSize.X;
		if (Random.Shared.Next() % 2 == 0) {
			EntityManager.QueueEntity(new Fish(new(x, camera.offset.Y)));
			return;
		}
		EntityManager.QueueEntity(new Obstacle(new(x, camera.offset.Y)));

	}
}
