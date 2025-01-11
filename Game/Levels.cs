
using System.Numerics;
using ArcadeJam.Entities;
using YarEngine;
using YarEngine.Entities;
using YarEngine.Graphics;

namespace ArcadeJam;

public class Levels {
	GameCamera camera;
	Player player;
	float curentSpeed = 0, levelBuffDist = 10;
	List<ScrollObj> staticEntities = new();
	Tutorial tut;
	HighScores highScores;
	public bool inTut = true, gameOver = false;


	public Levels(GameCamera camera) {
		this.camera = camera;
		camera.offset = new(-Assets.ui.Width, 0);
		player = new();
		tut = new(player);
		highScores = new(-1);
		staticEntities.Add(player);
		EntityManager.QueueEntity(player);
		spawnIntro();
		GameBase.debugScreen.RegisterModule(delegate {
			return new LevelInfoMod();
		});
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
		if (gameOver) {
			highScores.Update(time);
			if (highScores.finished) {
				EntityManager.ClearLayer([0, 1]);
				Globals.levels = new(camera);
			}
		}
	}
	public void Draw(GameCamera cam) {
		if (inTut) {
			tut.Draw(cam);
		}
		if (gameOver) {
			highScores.Draw();
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
	public void GameOver() {
		gameOver = true;
		curentSpeed = 0;
		highScores = new(100);
		Console.WriteLine("game over :(");
	}
}
