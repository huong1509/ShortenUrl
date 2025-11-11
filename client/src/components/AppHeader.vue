<template>
    <div class="ui menu inverted fixed top">
        <!-- Logo + Brand -->
        <div class="header item">
            <i class="linkify icon"></i>
            <span>Shortener URL</span>
        </div>

        <!-- Navigation -->
        <router-link to="/home" class="item">
            <i class="home icon"></i>
            Home
        </router-link>
        <router-link to="/dashboard" class="item">My URLs</router-link>

        <!-- Spacer -->
        <div class="right menu">
            <div v-if="currentUser" class="ui simple dropdown item">
                <img :src="currentUser.avatar" alt="avatar" class="ui avatar image" />
                {{ currentUser.name }}
                <i class="dropdown icon"></i>
                <div class="menu">
                    <router-link to="/personal" class="item">Personal</router-link>
                    <div class="item" @click="logout">Logout</div>
                </div>
            </div>
            <router-link v-else to="/signin" class="item">
                <i class="user icon"></i> Account
            </router-link>
        </div>
    </div>
</template>

<script>
    export default {
        data() {
            return {
                currentUser: null
            };
        },
        mounted() {
            window.addEventListener("storage", this.loadUser);
            this.loadUser();
        },
        beforeUnmount() {
            window.removeEventListener("storage", this.loadUser);
        },
        methods: {
            loadUser() {
                const storedUser = localStorage.getItem('currentUser');
                this.currentUser = storedUser ? JSON.parse(storedUser) : null;
            },
            logout() {
                localStorage.removeItem('currentUser');
                this.currentUser = null;
                window.dispatchEvent(new Event("storage"));
                this.$router.push('/signin');
            }
        }
    };
</script>


<style scoped>
    .ui.menu {
        z-index: 1000;
    }

    .ui.avatar.image {
        border-radius: 50%;
        width: 32px;
        height: 32px;
        margin-right: 8px;
    }
</style>