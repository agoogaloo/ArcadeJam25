
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
		inputTime = 1;
		if (holdingRod) {
			//idk
			if (!InputHandler.GetButton("U").Held) {
				castTimer = inputTime;
			}
			if (InputHandler.GetButton("D").Held) {
				backTimer = inputTime / 2;
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
		castTimer = Math.Max(0, castTimer - (float)time);
		backTimer = Math.Max(0, backTimer - (float)time);
		leftTimer = Math.Max(0, leftTimer - (float)time);
		rightTimer = Math.Max(0, rightTimer - (float)time);

		if (InputHandler.GetButton("A").JustReleased && castTimer > 0 && InputHandler.GetButton("U").Held) {
			DoCast();

			if (!holdingRod) {
				castTimer = 0;
			}

		}

	}
	private void DoCast() {
		// if its angled to the right
		if (InputHandler.GetButton("R").Held) {
			if (leftTimer != 0) {
				return;
			}
			else if (rightTimer > 0) {
				fishing.Cast(5 + castTimer * 2 + backTimer * 20, 10);
			}
			else {
				fishing.Cast(5 + castTimer * 2 + backTimer * 20, 2);

			}
		}
		// if its angle to the left
		else if (InputHandler.GetButton("L").Held) {
			if (rightTimer != 0) {
				return;
			}
			else if (leftTimer > 0) {
				fishing.Cast(5 + castTimer * 2 + backTimer * 20, -10);
			}
			else {
				fishing.Cast(5 + castTimer * 2 + backTimer * 20, -2);
			}
		}
		// otherwise its straight
		else {
			fishing.Cast(5 + castTimer * 2 + backTimer * 20);
		}
	}
}
