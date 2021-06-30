import * as Getters from "../constants/getters";
import * as Mutations from "../constants/mutations";
import * as Actions from "../constants/actions";
import { IRootState } from "../index";
import { ActionContext, ActionTree, MutationTree } from "vuex";
import Timeout from "@/utilities/timeout";

interface IExampleState {
    exampleStringField: string;
}

export const ExampleStoreModule = {
    state: {
        exampleStringField: ""
    } as IExampleState,
    getters: {
        [Getters.GET_STRING_FIELD]: (state: IExampleState): string => state.exampleStringField,
    },
    mutations: {
        [Mutations.SET_STRING_FIELD](state: IExampleState, payload: string): void {
            state.exampleStringField = payload;
        }
    } as MutationTree<IExampleState>,
    actions: {
        async [Actions.EXAMPLE_ACTION](context: ActionContext<IExampleState, IRootState>, payload: string): Promise<void> {
            context.commit(Mutations.SET_OPERATION_WAITING_FLAG, true);
            await Timeout(5000);
            context.commit(Mutations.SET_STRING_FIELD, payload);
            context.commit(Mutations.SET_OPERATION_WAITING_FLAG, false);
        }
    } as ActionTree<IExampleState, IRootState>
}