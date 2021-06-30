import { createStore } from 'vuex'
import { ExampleStoreModule } from "./modules/ExampleModule"

export default createStore({
  modules: {
    ExampleStoreModule
  }
})
