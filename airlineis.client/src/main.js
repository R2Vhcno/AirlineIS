import './assets/main.css'
import { createApp } from 'vue'
import { createWebHistory, createRouter } from 'vue-router'

import App from './App.vue'
import FlightsView from './components/pages/FlightsView.vue'
import AirportsView from './components/pages/AirportsView.vue'
import BookView from './components/pages/BookView.vue'
import TicketsView from './components/pages/TicketsView.vue'
import PlanesView from './components/pages/PlanesView.vue'
import EditFlightView from './components/pages/EditFlightView.vue'
import ClassesView from './components/pages/ClassesView.vue'

const routes = [
    { path: '/', component: FlightsView },
    { path: '/airports', component: AirportsView },
    { path: '/book/:flightid', component: BookView },
    { path: '/tickets', component: TicketsView },
    { path: '/planes', component: PlanesView },
    { path: '/classes', component: ClassesView },
    { path: '/editflight/:flightid', component: EditFlightView },
]

const router = createRouter({
    history: createWebHistory(),
    routes,
})

createApp(App)
    .use(router)
    .mount('#app')
