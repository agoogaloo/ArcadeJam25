
using System.Numerics;
using ArcadeJam.Entities;
using YarEngine.Entities;
using YarEngine.Graphics;

namespace ArcadeJam;

public class Levels {
	GameCamera camera;
	float curentSpeed = 0, levelBuffDist = 10;
	List<ScrollObj> staticEntities = new();
	Tutorial tut;
	private bool inTut = true;


	public Levels(GameCamera camera) {
		this.camera = camera;
		Player p = new();
		tut = new(p);
		staticEntities.Add(p);
		EntityManager.QueueEntity(p);
		spawnIntro();
	}


	public void Update(double time) {
		float scrollDist = (float)(curentSpeed * time);
		foreach (ScrollObj o in staticEntities) {
			o.scroll(scrollDist);
		}
		levelBuffDist -= scrollDist;
		if (levelBuffDist <= 0) {
			spawnSection();
		}
		if (inTut) {
			tut.Update(time);
		}
	}
	public void Draw(GameCamera cam) {
		if (inTut) {
			tut.Draw(cam);
		}
	}
	public void FishScroll(Vector2 fishBounds) {
		float oldY = camera.offset.Y;
		camera.offset.Y = MathF.Min(camera.offset.Y, fishBounds.Y - 20);
		levelBuffDist -= oldY - camera.offset.Y;
	}
	private void spawnSection() {
		levelBuffDist = 10;

		int x = Random.Shared.Next() % (int)camera.screenSize.X;

		if (Random.Shared.Next() % 2 == 0) {
			Fish f = new Fish(new(x, camera.offset.Y));
			EntityManager.QueueEntity(f);
			staticEntities.Add(f);
			return;

		}

		Obstacle o = new Obstacle(new(x, camera.offset.Y));
		EntityManager.QueueEntity(o);
		staticEntities.Add(o);

	}

	private void spawnIntro() {
		TutFish f = new(new(83, 70));
		EntityManager.QueueEntity(f);
		staticEntities.Add(f);
	}

	public void endTut() {
		inTut = false;
		curentSpeed += 3f;
	}
}
