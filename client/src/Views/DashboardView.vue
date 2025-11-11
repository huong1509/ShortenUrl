<template>
    <div class="dashboard">
        <div class="ui container">
            <h2 class="ui teal header">
                <i class="linkify icon"></i>
                <div class="content">
                    Dashboard
                    <div class="sub header">Manage your shortened URLs</div>
                </div>
            </h2>

            <div class="ui action input fluid">
                <input type="text"
                       placeholder="Enter URL to shorten..."
                       v-model="newUrl"
                       @keyup.enter="shortenUrl" />
                <button class="ui teal button" @click="shortenUrl">Shorten</button>
            </div>

            <UrlList :urls="urls" @delete-url="deleteUrl" />
        </div>
    </div>
</template>

<script>
    import UrlList from "../components/UrlList.vue";

    export default {
        name: "DashboardView",
        components: { UrlList },
        data() {
            return {
                newUrl: "",
                urls: [
                    {
                        id: 1,
                        originalUrl: "https://www.youtube.com/watch?v=dQw4w9WgXcQ",
                        shortUrl: "https://myurl.com/rick",
                        createdAt: "2025-11-09"
                    },
                    {
                        id: 2,
                        originalUrl: "https://openai.com",
                        shortUrl: "https://myurl.com/openai",
                        createdAt: "2025-11-08"
                    }
                ]
            };
        },
        methods: {
            shortenUrl() {
                if (!this.newUrl.trim()) return alert("Please enter a URL!");
                const id = Date.now();
                const short = "https://myurl.com/" + Math.random().toString(36).substring(2, 7);
                this.urls.unshift({
                    id,
                    originalUrl: this.newUrl,
                    shortUrl: short,
                    createdAt: new Date().toISOString().split("T")[0]
                });
                this.newUrl = "";
            },
            deleteUrl(id) {
                this.urls = this.urls.filter(u => u.id !== id);
            }
        }
    };
</script>

<style scoped>
    .dashboard {
        padding-top: 2rem;
    }

    .ui.container {
        max-width: 800px;
    }
</style>
