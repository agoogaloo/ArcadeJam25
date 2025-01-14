
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
				Globals.score = new();
				Globals.levels = new(camera);
			}
		}
		else {
			Globals.score.Update(time);

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

		if (Random.Shared.Next() % 3 == 0) {
			Fish f = new Fish(new(x, camera.offset.Y));
			EntityManager.QueueEntity(f);
			staticEntities.Add(f);
			return;

		}
		else if (Random.Shared.Next() % 3 == 1) {
			Ilene i = new Ilene((int)camera.offset.Y, 20, Random.Shared.Next() % 2 == 0);
			EntityManager.QueueEntity(i);
			staticEntities.Add(i);
			return;
		}

		Obstacle o = new Cat(new(x, camera.offset.Y));
		EntityManager.QueueEntity(o);
		staticEntities.Add(o);

	}

	private void spawnIntro() {
		TutFish f1 = new(new(35, 70));
		TutFish f2 = new(new(200 - 34 - 35, 70));
		EntityManager.QueueEntity(f1);
		EntityManager.QueueEntity(f2);
		staticEntities.Add(f1);
		staticEntities.Add(f2);

		Obstacle o = new Cat(new(75, 100));
		EntityManager.QueueEntity(o);
		staticEntities.Add(o);
	}

	public void endTut() {
		inTut = false;
		curentSpeed += 6f;
	}
	public void GameOver() {
		gameOver = true;
		curentSpeed = 0;
		highScores = new((int)Globals.score.currScore);
		Console.WriteLine("game over :(");
	}
}
