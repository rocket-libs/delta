import { PotterState } from "potter-nf";
import StringsBase from "../Strings/StringsBase";
import IPotterRepository from "./IPotterRepository";

const displaySpacingMilliseconds = 1500;
const epoch = {
    year: 1970,
    month: 0,
    day: 1,
};
export default abstract class Logic<
    TStrings extends StringsBase,
    TRepository extends IPotterRepository<TStrings>,
    TModel
> extends PotterState<TRepository, TModel> {
    private lastHidAt: Date = new Date(epoch.year, epoch.month, epoch.day);
    private completedAsyncCall: boolean = true;

    protected async runAsync<TResult>(fn: () => Promise<TResult>) {
        try {
            this.completedAsyncCall = false;
            if (this.shouldSetStateToBusy) {
                this.setBusyToTrue();
            } else {
                this.scheduleToSetBusy();
            }
            return await fn();
        } finally {
            this.completedAsyncCall = true;
            this.context.repository.busy = false;
            this.lastHidAt = new Date();
            this.potter.pushToRepository(this.context.repository);
        }
    }

    private scheduleToSetBusy() {
        setTimeout(() => {
            this.setBusyToTrue();
        }, displaySpacingMilliseconds - this.elapsedMillisecondsSinceLastShown);
    }

    private setBusyToTrue() {
        if (this.completedAsyncCall === false) {
            this.context.repository.busy = true;
            this.potter.pushToRepository(this.context.repository);
        }
    }

    private get shouldSetStateToBusy(): boolean {
        return this.elapsedMillisecondsSinceLastShown >= displaySpacingMilliseconds;
    }

    private get elapsedMillisecondsSinceLastShown(): number {
        return new Date().getTime() - this.lastHidAt.getTime();
    }
}
