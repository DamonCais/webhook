import Vue from 'vue'
import Router from 'vue-router'
Vue.use(Router)

const router = new Router({
    // mode: 'history',
    routes: [
        {
            path: '/',
            redirect: '/home',
        },
        {
            path: '/home',
            name: 'home',
            component: () =>
                import('@/views/home'),
            meta: {
            }
        },
        {
            path: '/member',
            name: 'member',
            component: () =>
                import('@/views/member'),
            meta: {
            }
        },
        {
            path: '/qrcode',
            name: 'qrcode',
            component: () =>
                import('@/views/qrcode'),
            meta: {
            }
        },
        {
            path: '/test',
            name: 'test',
            component: () =>
                import('@/views/test'),
            meta: {
            }
        },
        {
            path: '/equiment/:id',
            name: 'equiment',
            component: () =>
                import('@/views/equiment'),
            meta: {
            }
        },
    ]
})


router.beforeEach((to, from, next) => {
    next();
    // let authrequired = false
    // if (to && to.meta && to.meta.auth)
    //     authrequired = true
    // if (authrequired) {
    //     if (auth.loggedIn()) {
    //         if (to.name === 'login') {
    //             window.location.href = '/'
    //             return false
    //         } else {
    //             next()
    //         }
    //     } else {
    //         if (to.name !== 'login') {
    //             window.location.href = '/login'
    //             return false
    //         }
    //         next()
    //     }
    // } else {
    //     if (auth.loggedIn() && to.name === 'login') {
    //         window.location.href = '/'
    //         return false
    //     } else {
    //         next()
    //     }
    // }

    // if (to && to.meta && to.meta.layout) {
    //     l.set(to.meta.layout)
    // }
})

// router.afterEach((to, from) => {
//     setTimeout(() => {
//         store.commit('setSplashScreen', false)
//     }, 50)
// })

export default router