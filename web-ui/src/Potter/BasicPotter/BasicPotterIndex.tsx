import { PotterState } from "potter-nf";
import React, { ReactElement, ReactNode } from "react";
import { Component } from "react";
import { deviceIsMobile } from "../../BrowserDetection/BrowserDetector";
import Pot from "../Pot";
import PotterUiBinder from "../PotterUiBinder";
import Pottery from "../Pottery";
import BasicModelPotter from "./BasicModelPotter";

interface IProps<TRepository, TModel, TLogic extends PotterState<TRepository, TModel>> {
    repository: TRepository;
    pot: Pot<TRepository, TModel, TLogic>;
    logic: TLogic;
    model: TModel;
    busyEvaluator?: () => boolean;
    busyComponent?: ReactNode;
    onRender: () => ReactElement;
    onStarted?: (pottery: Pottery<TRepository, TModel, TLogic, BasicModelPotter<TRepository, TModel, TLogic>>) => void;
    onExiting?: (
        pottery: Pottery<TRepository, TModel, TLogic, BasicModelPotter<TRepository, TModel, TLogic>> | undefined,
    ) => void;
}

export default class BasicPotterIndex<
    TRepository,
    TModel,
    TLogic extends PotterState<TRepository, TModel>
> extends Component<IProps<TRepository, TModel, TLogic>> {
    pottery: Pottery<TRepository, TModel, TLogic, BasicModelPotter<TRepository, TModel, TLogic>> | undefined;

    bindingIsRequired(): boolean {
        if (deviceIsMobile()) {
            return true;
        } else {
            return this.props.pot ? false : true;
        }
    }
    render() {
        return (
            <>
                {this.busyUi()}
                {this.activeUI()}
            </>
        );
    }

    activeUI(): ReactElement {
        if (this.bindingIsRequired()) {
            return this.bindUi();
        } else {
            return this.mainUi();
        }
    }

    mainUi(): ReactElement {
        return this.props.onRender();
    }

    busyUi(): ReactElement {
        if (this.props.busyEvaluator && this.props.busyEvaluator() && this.props.busyComponent) {
            return <>{this.props.busyComponent}</>;
        } else {
            return <></>;
        }
    }

    bindUi() {
        return (
            <>
                <PotterUiBinder
                    currentPotter={this.pottery?.potter}
                    newPotter={new BasicModelPotter(this.props.repository, this.props.model, this.props.logic)}
                    onStarted={(potter: BasicModelPotter<TRepository, TModel, TLogic>) => {
                        if (!this.pottery) {
                            this.pottery = new Pottery(potter);
                            if (this.props.onStarted) {
                                this.props.onStarted(this.pottery);
                            }
                        }
                    }}
                    onExiting={() => {
                        if (this.props.onExiting) {
                            this.props.onExiting(this.pottery);
                        }
                    }}
                    loadingDisplayLabel={"..."}
                    onRender={() => this.props.onRender()}
                />
            </>
        );
    }
}
