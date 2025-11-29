import { createRouter, createWebHistory } from 'vue-router'
import PersonsView from '../views/PersonsView.vue'
import LoansView from '../views/LoansView.vue'
import DashboardView from '../views/DashboardView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'dashboard',
      component: DashboardView,
    },
    {
      path: '/persons',
      name: 'persons',
      component: PersonsView,
    },
    {
      path: '/loans',
      name: 'loans',
      component: LoansView,
    },
  ],
})

export default router
