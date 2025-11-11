<template>
    <div id="app" class="layout">
        <AppHeader />
        <main class="content">
            <router-view />
        </main>
        <Footer />
    </div>
</template>

<script>
    import AppHeader from './components/AppHeader.vue'
    import Footer from './components/Footer.vue'

    export default {
        components: {
            AppHeader,
            Footer
        }
    }
</script>

<style>
    .layout {
        display: flex;
        flex-direction: column;
        height: 100vh;
        overflow: hidden; 
    }

    .content {
        flex: 1;
        overflow-y: auto;
        padding: 70px 0 80px 0; 
    }

    html, body {
        margin: 0;
        padding: 0;
        height: 100%;
        overflow: hidden; 
    }
</style>
