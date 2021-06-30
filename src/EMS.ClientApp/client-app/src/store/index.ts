import { createStore } from "vuex";
import { ExampleStoreModule } from "./modules/ExampleModule";
import * as Getters from "./constants/getters";
import * as Mutations from "./constants/mutations";

export interface IRootState {
  operationWaitingFlag: boolean;
  operationCompleteFlag: boolean;
}

export default createStore({
  state: {
    operationWaitingFlag: false,
    operationCompleteFlag: false
  } as IRootState,
  getters: {
    [Getters.GET_OPERATION_COMPLETE_FLAG]: (state: IRootState): boolean => state.operationCompleteFlag,
    [Getters.GET_OPERATION_WAITING_FLAG]: (state: IRootState): boolean => state.operationWaitingFlag,
  },
  mutations: {
    [Mutations.SET_OPERATION_COMPLETE_FLAG] (state: IRootState, payload: boolean): void { state.operationCompleteFlag = payload; },
    [Mutations.SET_OPERATION_WAITING_FLAG] (state: IRootState, payload: boolean): void { state.operationWaitingFlag = payload; },

  },
  modules: {
    ExampleStoreModule
  }
})
