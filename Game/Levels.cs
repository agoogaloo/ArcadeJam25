
using System.Numerics;
using ArcadeJam.Entities;
using YarEngine;
using YarEngine.Entities;
using YarEngine.Graphics;

namespace ArcadeJam;

public delegate List<Entity> LevelSectMethod(float y);
public class Levels {
	float timeDiff = 0.1f, catchDiff = 1, sectDiff = 5, CurrentDifficultyScale = 0.5f;
	GameCamera camera;
	Player player;
	float curentSpeed = 0, levelBuffDist = 0, sectDistance = 0;
	List<ScrollObj> movingEntities = new();
	Tutorial tut;
	HighScores highScores;
	public bool inTut = true, gameOver = false;

	public float difficulty { get; private set; } = 0;
	List<LevelSectMethod> chuncksToAdd = [], easyChunks, medChunks, hardChunks;



	public Levels(GameCamera camera) {
		easyChunks = [easyRandObs, easyRandObs, easyRandFish];
		medChunks = [easyRandObs, easyRandObs, midRandObs, midRandFish];
		hardChunks = [easyRandObs, easyRandObs, midRandObs, hardRandObs, hardRandFish];
		this.camera = camera;
		camera.offset = new(-Assets.ui.Width, 0);
		player = new();
		tut = new(player);
		highScores = new(-1);
		movingEntities.Add(player);
		EntityManager.QueueEntity(player);
		spawnIntro();
		GameBase.debugScreen.RegisterModule(delegate {
			return new LevelInfoMod();
		});
	}


	public void Update(double time) {
		float scrollDist = (float)((curentSpeed + difficulty * CurrentDifficultyScale) * time);
		foreach (ScrollObj o in movingEntities) {
			o.scroll(scrollDist);
		}
		// actual level spawning stuff
		levelBuffDist -= scrollDist;
		if (levelBuffDist <= 0) {
			spawnSection();
		}
		difficulty += (float)(time * timeDiff);

		if (inTut) {
			tut.Update(time);
			difficulty = 0;
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
		if (chuncksToAdd.Count == 0) {
			refillSections();
		}
		int index = Random.Shared.Next() % chuncksToAdd.Count();
		float y = camera.offset.Y - 10;
		List<Entity> objects = chuncksToAdd[index](y);
		sectDistance = sectDistance - difficulty;
		levelBuffDist += sectDistance;
		levelBuffDist = MathF.Max(0, levelBuffDist);
		foreach (Entity i in objects) {
			EntityManager.QueueEntity(i);
			ScrollObj o = i as ScrollObj;
			movingEntities.Add(o);
		}
		chuncksToAdd.RemoveAt(index);

	}
	private void refillSections() {
		if (difficulty < sectDiff) {
			foreach (LevelSectMethod m in easyChunks) {
				chuncksToAdd.Add(m);
			}
			return;
		}
		if (difficulty < 2 * sectDiff) {
			foreach (LevelSectMethod m in medChunks) {
				chuncksToAdd.Add(m);
			}
			return;
		}
		foreach (LevelSectMethod m in hardChunks) {
			chuncksToAdd.Add(m);
			chuncksToAdd.Add(m);
		}
	}
	private void randSect() {
		levelBuffDist = 10;

		int x = Random.Shared.Next() % (int)camera.screenSize.X;

		if (Random.Shared.Next() % 4 == 0) {
			Fish f = new Fish(new(x, camera.offset.Y));
			EntityManager.QueueEntity(f);
			movingEntities.Add(f);
			return;

		}
		else if (Random.Shared.Next() % 3 == 1) {
			Ilene i = new Ilene((int)camera.offset.Y, 20, Random.Shared.Next() % 2 == 0);
			EntityManager.QueueEntity(i);
			movingEntities.Add(i);
			return;
		}
		else if (Random.Shared.Next() % 3 == 2) {
			Obstacle c = new Cat(new(x, camera.offset.Y));
			EntityManager.QueueEntity(c);
			movingEntities.Add(c);
			return;
		}
		Obstacle o = new Rock(new(x, camera.offset.Y));
		EntityManager.QueueEntity(o);
		movingEntities.Add(o);
	}

	public void endTut() {
		inTut = false;
		curentSpeed = 6f;
	}
	public void GameOver() {
		gameOver = true;
		curentSpeed = 0;
		highScores = new((int)Globals.score.currScore);
	}

	private void spawnIntro() {
		TutFish f1 = new(new(35, 70));
		TutFish f2 = new(new(200 - 34 - 35, 70));
		EntityManager.QueueEntity(f1);
		EntityManager.QueueEntity(f2);
		movingEntities.Add(f1);
		movingEntities.Add(f2);
	}
	private List<Entity> introSect(float y) {
		List<Entity> l = new();
		l.Append(new TutFish(new(35, 70)));
		l.Append(new TutFish(new(200 - 34 - 35, 70)));
		return l;
	}

	private List<Entity> doubleRock(float y) {
		sectDistance += 40;
		int dist = Random.Shared.Next() % 20 + 30;
		int x = Random.Shared.Next() % ((int)camera.screenSize.X - dist * 2) + dist;
		List<Entity> l = new();
		l.Add(new Rock(new(x + dist, y - 5)));
		l.Add(new Rock(new(x - dist, y - 5)));
		return l;
	}
	private List<Entity> rockPile(float y) {
		sectDistance += 30;
		int x = Random.Shared.Next() % ((int)camera.screenSize.X - 40) + 20;
		List<Entity> l = new();
		l.Add(new Fish(new(x - 2, y - 18)));
		l.Add(new Rock(new(x + 10, y - 5)));
		l.Add(new Rock(new(x - 12, y - 7)));
		l.Add(new Rock(new(x, y)));
		return l;
	}
	private List<Entity> easyRandObs(float y) {
		sectDistance = 20;
		List<Entity> l = new();
		int x = Random.Shared.Next() % (int)camera.screenSize.X;
		l.Add(new Rock(new(x, y)));
		return l;
	}
	private List<Entity> easyRandFish(float y) {
		sectDistance = 20;
		List<Entity> l = new();
		int x = Random.Shared.Next() % (int)camera.screenSize.X;
		l.Add(new Fish(new(x, y)));
		return l;
	}
	private List<Entity> midRandObs(float y) {
		sectDistance += 20;
		List<Entity> l = new();
		int x = Random.Shared.Next() % (int)camera.screenSize.X;
		l.Add(new Cat(new(x, y)));
		return l;
	}
	private List<Entity> midRandFish(float y) {
		sectDistance += 20;
		List<Entity> l = new();
		int x = Random.Shared.Next() % (int)camera.screenSize.X;
		l.Add(new Fish(new(x, y)));
		return l;
	}
	private List<Entity> hardRandObs(float y) {
		sectDistance += 20;
		List<Entity> l = new();
		int x = Random.Shared.Next() % (int)camera.screenSize.X;
		int startY = Random.Shared.Next() % 50 + 10;
		l.Add(new Ilene((int)y, startY, Random.Shared.Next() % 2 == 0));
		return l;
	}
	private List<Entity> hardRandFish(float y) {
		sectDistance = 20;
		List<Entity> l = new();
		int x = Random.Shared.Next() % (int)camera.screenSize.X;
		l.Add(new Fish(new(x, y)));
		return l;
	}

	public void doCatchDifficulty() {
		difficulty += catchDiff;

	}
}
