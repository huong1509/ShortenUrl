<template>
    <div class="ui fluid card url-card">
        <div class="content">
            <div class="header">{{ url.shortUrl }}</div>
            <div class="meta">Created: {{ url.createdAt }}</div>
            <div class="description">
                <p>
                    <strong>Original:</strong>
                    <a :href="url.originalUrl" target="_blank">{{ url.originalUrl }}</a>
                </p>
            </div>
        </div>

        <div class="extra content">
            <div class="ui buttons mini">
                <button class="ui teal button" @click="copyUrl">Copy</button>
                <button class="ui yellow button" @click="editUrl">Edit</button>
                <button class="ui red button" @click="$emit('delete', url.id)">Delete</button>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        name: "UrlCard",
        props: {
            url: Object
        },
        methods: {
            copyUrl() {
                navigator.clipboard.writeText(this.url.shortUrl);
                alert("Copied!");
            },
            editUrl() {
                this.$emit("edit", this.url);
            }
        }
    };
</script>

<style scoped>
    .url-card {
        margin-bottom: 1rem;
        word-break: break-all;
    }
</style>
