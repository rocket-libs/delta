import Potter, { PotterState } from "potter-nf";
import Pottery from "./Pottery";
import { v4 as uuidv4 } from "uuid";

export default class Pot<TRepository, TModel, TState extends PotterState<TRepository, TModel>> {
    private pottery: Pottery<TRepository, TModel, TState, Potter<TRepository, TModel, TState>>;
    private id: string;

    public pushToModel: (model: Partial<TModel>) => any;
    public pushToRepository: (repository: Partial<TRepository>) => any;
    public pushToState: (state: Partial<TState>) => any;

    constructor(pottery: Pottery<TRepository, TModel, TState, Potter<TRepository, TModel, TState>>) {
        this.pottery = pottery;
        this.pottery.state.potter = this.pottery.potter;
        this.warnIfLogicHasIsStateful(this.pottery.state);
        this.pushToRepository = this.pottery.pushToRepository;
        this.pushToModel = this.pottery.pushToModel;
        this.pushToState = this.pottery.pushToState;
        this.id = uuidv4();
    }

    public get instanceId(): string {
        return this.id;
    }

    public get repository(): TRepository {
        return this.pottery.repository;
    }

    public get model(): TModel {
        return this.pottery.model;
    }

    public get logic(): TState {
        return this.pottery.state;
    }

    public forceRerender() {
        this.pushToState({});
    }

    private warnIfLogicHasIsStateful(obj: any) {
        for (const key in obj) {
            if (obj.hasOwnProperty(key) && typeof obj[key] !== "function") {
                console.warn(
                    `Your logic class ${obj} contains properties. It is discouraged to have a stateful logic class`,
                );
                return;
            }
        }
    }
}
