import { createRouter, createWebHistory, RouteRecordRaw } from "vue-router";
import ExampleComponent from "@/components/Test/ExampleComponent.vue";

const routes: Array<RouteRecordRaw> = [
  {
    path: "/",
    name: "ExampleComponent",
    component: ExampleComponent
  }
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router
