import Potter, { PotterState } from "potter-nf";

export default class BasicPotter<TRepository, TModel, TLogic extends PotterState<TRepository, TModel>> extends Potter<
    TRepository,
    TModel,
    TLogic
> {}
