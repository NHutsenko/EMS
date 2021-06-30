import * as Getters from "../constants/getters";
import * as Mutations from "../constants/mutations";
import * as Actions from "../constants/actions";
import { ActionContext } from "vuex";
import Timeout from "@/utilities/timeout";

interface IExampleState {
    exampleStringField: string;
    exampleBooleanField: boolean;
}

export const ExampleStoreModule = {
    state: {
        exampleStringField: "",
        exampleBooleanField: false
    } as IExampleState,
    getters: {
        [Getters.GET_STRING_FIELD]: (state: IExampleState): string => state.exampleStringField,
        [Getters.GET_BOOLEAN_FIELD]: (state: IExampleState): boolean => state.exampleBooleanField,
    },
    mutations: {
        [Mutations.SET_BOOLEAN_FIELD](state: IExampleState, payload: boolean): void {
            state.exampleBooleanField = payload;
        },
        [Mutations.SET_STRING_FIELD](state: IExampleState, payload: string): void {
            state.exampleStringField = payload;
        }
    },
    actions: {
        async [Actions.EXAMPLE_ACTION](context: ActionContext<IExampleState, IExampleState>, payload: string): Promise<void> {
            context.commit(Mutations.SET_BOOLEAN_FIELD, true);
            await Timeout(5000);
            context.commit(Mutations.SET_STRING_FIELD, payload);
            context.commit(Mutations.SET_BOOLEAN_FIELD, false);
        }
    }
}