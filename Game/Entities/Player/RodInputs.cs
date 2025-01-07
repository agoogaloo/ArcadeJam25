
using YarEngine.Inputs;

namespace ArcadeJam.Entities;

public class RodInputs {

	Fishing fishing;

	bool holdingRod = false, lockMove = false;
	float inputTime = 0.3f;
	public float castTimer { get; private set; } = 0;
	public float backTimer { get; private set; } = 0;
	public float leftTimer { get; private set; } = 0; // timers for angling left/right and diferentiating from circles
	public float rightTimer { get; private set; } = 0;

	public RodInputs(Fishing fishing) {
		this.fishing = fishing;
	}

	public void Update(double time) {
		// if you are holding the fishing rod check for casting input stuff
		holdingRod = InputHandler.GetButton("A").Held || fishing.castState == CastState.Casting;
		inputTime = 0.3f;
		if (holdingRod) {
			//idk
			if (!InputHandler.GetButton("U").Held) {
				castTimer = inputTime;
			}
			if (InputHandler.GetButton("D").Held) {
				backTimer = inputTime;
				if (InputHandler.GetButton("R").Held) {
					leftTimer = inputTime;
					rightTimer = 0;
				}
				if (InputHandler.GetButton("L").Held) {
					rightTimer = inputTime;
					leftTimer = 0;
				}
			}
		}

		if (InputHandler.GetButton("A").JustReleased && castTimer > 0 && InputHandler.GetButton("U").Held) {
			DoCast();

			if (!holdingRod) {
				castTimer = 0;
			}

		}

		castTimer = Math.Max(0, castTimer - (float)time);
		backTimer = Math.Max(0, backTimer - (float)time);
		leftTimer = Math.Max(0, leftTimer - (float)time);
		rightTimer = Math.Max(0, rightTimer - (float)time);
	}

	private void DoCast() {
		// if its angled to the right
		float performance = (castTimer + backTimer * 0.5f) / inputTime;
		Console.WriteLine("casting perf:" + performance + " casttime:" + castTimer + " backTime:" + backTimer);
		if (InputHandler.GetButton("R").Held) {
			if (leftTimer != 0) {
				return;
			}
			else if (rightTimer > 0) {
				fishing.Cast(performance, 30);
			}
			else {
				fishing.Cast(performance, 15);

			}
		}
		// if its angle to the left
		else if (InputHandler.GetButton("L").Held) {
			if (rightTimer != 0) {
				return;
			}
			else if (leftTimer > 0) {
				fishing.Cast(performance, -30);
			}
			else {
				fishing.Cast(performance, -15);
			}
		}
		// otherwise its straight
		else {
			fishing.Cast(performance);
		}
	}
}
