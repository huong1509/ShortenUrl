<template>
    <div class="ui segment">
        <h3 class="ui header">Your Shortened URLs</h3>

        <div v-if="urls.length === 0" class="ui message">
            You haven't created any links yet.
        </div>

        <div v-else class="ui divided items">
            <UrlCard v-for="url in urls"
                     :key="url.id"
                     :url="url"
                     @delete="deleteUrl" />
        </div>
    </div>
</template>

<script>
    import UrlCard from "./UrlCard.vue";

    export default {
        name: "UrlList",
        components: { UrlCard },
        props: {
            urls: Array
        },
        emits: ["delete-url"],
        methods: {
            deleteUrl(id) {
                this.$emit("delete-url", id);
            }
        }
    };
</script>
