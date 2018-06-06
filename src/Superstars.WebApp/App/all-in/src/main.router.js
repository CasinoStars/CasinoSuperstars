// Vue router setup
import Vue from 'vue';
import VueRouter from 'vue-router';
Vue.use(VueRouter);

import requireAuth from './helpers/requireAuth';

// Components
import Home from './components/Home.vue';
import Logout from './components/Logout.vue';
import Play from './components/Play.vue';
import Yams from './components/Yams.vue';
import BlackJack from './components/BlackJack.vue';
import Wallet from './components/Wallet.vue';

const routes = [
    { path: '', component: Home },
    { path: '/logout', component: Logout, beforeEnter: requireAuth },
    { path: '/play', component: Play, beforeEnter: requireAuth },
    { path: '/yams', component: Yams, beforeEnter: requireAuth },
    { path: '/blackJack', component: BlackJack, beforeEnter: requireAuth },
    { path: '/wallet', component: Wallet, beforeEnter: requireAuth },
];

export default new VueRouter({
    mode: 'history',
    base: '/Home',
    routes
});