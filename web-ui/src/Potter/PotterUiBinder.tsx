import React, { useState, useEffect, ReactElement } from "react";
import Potter, { PotterState } from "potter-nf";
import { useHistory } from "react-router-dom";

interface IProps<
    TRepository,
    TModel,
    TState extends PotterState<TRepository, TModel>,
    TPotter extends Potter<TRepository, TModel, TState>
> {
    currentPotter: TPotter | undefined;
    newPotter: TPotter;
    onStarted: (potter: TPotter) => void;
    onExiting?: () => void;
    onResetComponent?: () => void;
    onRender: () => ReactElement;
    loadingDisplayLabel: string;
}

export default function PotterUiBinder<
    TRepository,
    TModel,
    TState extends PotterState<TRepository, TModel>,
    TPotter extends Potter<TRepository, TModel, TState>
>(props: IProps<TRepository, TModel, TState, TPotter>) {
    const initialPotterId = 0;
    const [potterChangeId, setPotterChangeId] = useState(initialPotterId);
    const [jsxElement, setJsxElement] = useState(<>{props.loadingDisplayLabel}</>);
    const [started, setStarted] = useState(false);
    const potterInstance = props.currentPotter ?? props.newPotter;
    const history = useHistory();
    const currentRoute = window.location.pathname.toLowerCase();
    const [initialized, setInitialized] = useState(false);
    useEffect(
        () => {
            const initializePotter = (): (() => void) => {
                const potterCleanup = potterInstance.subscribe(() => {
                    setPotterChangeId(potterInstance.context.changeId);
                });
                window.addEventListener("popstate", function (_event) {
                    if (props.onExiting) {
                        props.onExiting();
                    }
                });

                const historyCleanup = !history
                    ? () => {}
                    : history.listen((_location, _action) => {
                          if (_location.pathname.toLowerCase() === currentRoute) {
                              if (props.onResetComponent) {
                                  props.onResetComponent();
                              }
                          } else {
                              if (props.onExiting) {
                                  props.onExiting();
                              }
                          }
                      });
                return function cleanup() {
                    historyCleanup();
                    potterCleanup();
                };
            };
            if (props.onRender && started === true) {
                setJsxElement(props.onRender());
            }
            const cleanupCallback = initializePotter();
            setInitialized(true);
            return cleanupCallback;
        },
        // eslint-disable-next-line
    [potterChangeId]);
    if (started === false && initialized === true) {
        setStarted(true);
        props.onStarted(potterInstance);
        potterInstance.broadcastContextChanged();
    }
    return jsxElement;
}
