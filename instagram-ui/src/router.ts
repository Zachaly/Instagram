import { Component } from "vue";
import { createRouter, createWebHistory } from "vue-router";
import MainPage from './pages/MainPage.vue'
import RegisterPage from './pages/RegisterPage.vue'
import LoginPage from './pages/LoginPage.vue'
import ProfilePage from "./pages/ProfilePage.vue";
import UpdateProfilePage from "./pages/UpdateProfilePage.vue";
import AddPostPage from './pages/AddPostPage.vue'
import PostPage from "./pages/PostPage.vue";
import SearchTagPage from './pages/SearchTagPage.vue'
import AdminPage from './pages/AdminPage.vue'
import ModerationPage from './pages/ModerationPage.vue'
import PostReportPage from './pages/PostReportPage.vue'
import ChatPage from './pages/ChatPage.vue'
import AccounterVerificationPage from './pages/AccountVerificationPage.vue'
import VerifyAccountPage from './pages/VerifyAccountPage.vue'

const createRoute = (path: string, name: string, component: Component) => ({ path, name, component })

const routes = [
    createRoute('/', 'main', MainPage),
    createRoute('/login', 'login', LoginPage),
    createRoute('/register', 'register', RegisterPage),
    createRoute('/user/:id', 'user', ProfilePage),
    createRoute('/user/update', 'user-update', UpdateProfilePage),
    createRoute('/add-post', 'add-post', AddPostPage),
    createRoute('/post/:id', 'post', PostPage),
    createRoute('/tag/:tag', 'tag', SearchTagPage),
    createRoute('/admin', 'admin', AdminPage),
    createRoute('/moderation', 'moderation', ModerationPage),
    createRoute('/report/:id', 'post-report', PostReportPage),
    createRoute('/chat/:userId', 'chat', ChatPage),
    createRoute('/verify-account', 'verify-account', VerifyAccountPage),
    createRoute('/account-verification/:id', 'account-verification', AccounterVerificationPage)
]

export default createRouter({
    routes,
    history: createWebHistory()
})