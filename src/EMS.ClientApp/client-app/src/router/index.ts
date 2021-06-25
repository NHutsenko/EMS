import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router'
import Test from "@/components/Test/Test.vue";

const routes: Array<RouteRecordRaw> = [
  {
    path: '/',
    name: 'Test',
    component: Test
  }
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router
