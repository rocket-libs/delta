import Potter, { PotterState } from "potter-nf";
import SafeReader from "./SafeReader";

export default class Pottery<
    TRepository,
    TModel,
    TState extends PotterState<TRepository, TModel>,
    TPotter extends Potter<TRepository, TModel, TState>
> {
    public potter: TPotter;
    public pushToModel: (model: Partial<TModel>) => any;
    public pushToRepository: (repository: Partial<TRepository>) => any;
    public pushToState: (state: Partial<TState>) => any;

    public constructor(potter: TPotter) {
        this.potter = potter;
        this.pushToModel = (model: Partial<TModel>) => potter.pushToModel(model);
        this.pushToRepository = (repository: Partial<TRepository>) => {
            potter.pushToRepository(repository);
        };

        this.pushToState = (state: Partial<TState>) => {
            potter.pushToState(state);
        };
    }

    public get repository(): TRepository {
        return this.potter.context.repository;
    }

    public get model(): TModel {
        return this.potter.context.model;
    }

    public get state(): TState {
        return this.potter.state;
    }

    public get repositoryReader(): SafeReader<TRepository> {
        return new SafeReader<TRepository>(this.potter.context.repository);
    }

    public get modelReader(): SafeReader<TModel> {
        return new SafeReader<TModel>(this.potter.context.model);
    }
}
