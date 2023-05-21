import { Component } from "vue";
import { createRouter, createWebHistory } from "vue-router";
import MainPage from './pages/MainPage.vue'
import RegisterPage from './pages/RegisterPage.vue'
import LoginPage from './pages/LoginPage.vue'

const createRoute = (path: string, name: string, component: Component) => ({ path, name, component })

const routes = [
    createRoute('/', 'main', MainPage),
    createRoute('/login', 'login', LoginPage),
    createRoute('/register', 'register', RegisterPage)
]

export default createRouter({
    routes,
    history: createWebHistory()
})