<template>
    <div class="ui container home-container">
        <div class="ui raised very padded text container segment shorten-box">
            <h2 class="ui header center aligned">Shorten Your URL</h2>

            <label for="original-url" class="mr-4">Shorten a long URL:</label>
            <!-- URL Input + Shorten Button -->
            <div class="ui action input fluid mb-3">
                <input v-model="originalUrl"
                       type="text"
                       placeholder="Enter your long link here..." />
                <button class="ui teal button" @click="shortenUrl">
                    <i class="linkify icon"></i> Shorten
                </button>
            </div>

            <!-- Custom Alias Input -->
            <label for="custom-alias" class="mr-4">Customize your link:</label>
            <div class="ui fluid input mb-3">
                <input v-model="customAlias"
                       type="text"
                       placeholder="Enter alias (optional)" />
            </div>

            <!-- Result -->
            <div v-if="shortenedUrl" class="ui segment result-box">
                <h5 class="ui header">Your shortened URL:</h5>
                <a :href="shortenedUrl"
                   target="_blank"
                   rel="noopener noreferrer"
                   class="ui teal text">
                    {{ shortenedUrl }}
                </a>
            </div>

            <label class="mb-4"> ToolBox</label>
            <!-- Action Buttons -->
            <div class="ui four buttons mt-3">
                <button class="ui olive button" @click="copyUrl">
                    <i class="copy outline icon"></i> Copy
                </button>
                <button class="ui green button" @click="resetForm">
                    <i class="redo icon"></i> Shorten another
                </button>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        name: "HomeView",
        data() {
            return {
                originalUrl: "",
                shortenedUrl: "",
                showQR: false,
                customAlias: "",
            };
        },
        methods: {
            shortenUrl() {
                if (!this.originalUrl) {
                    alert("Please enter a URL first!");
                    return;
                }

                // demo generate URL
                const randomCode = Math.random().toString(36).substring(2, 8);
                const alias = this.customAlias.trim() || randomCode;
                this.shortenedUrl = `https://myurl.io/${alias}`;
            },
            copyUrl() {
                if (this.shortenedUrl) {
                    navigator.clipboard.writeText(this.shortenedUrl);
                    alert("Copied to clipboard!");
                }
            },
            resetForm() {
                this.originalUrl = "";
                this.shortenedUrl = "";
                this.customAlias = "";
                this.showQR = false;
            },
        },
    };
</script>

<style scoped>
    .home-container {
        min-height: 75vh;
        display: flex;
        justify-content: center;
        align-items: center;
        padding-top: 100px;
        background-color: #f9fafb;
        padding-bottom: 100px;
    }

    .shorten-box {
        width: 100%;
        max-width: 600px;
        background-color: white;
    }

    .result-box {
        text-align: center;
        background: #f8f9fa;
    }

    .input-group,
    .result-box,
    .action-buttons {
        margin-bottom: 1rem;
    }

    .mt-3 {
        margin-top: 1rem;
    }

    .mt-4 {
        margin-top: 1.5rem;
    }

    .qr-box {
        display: flex;
        justify-content: center;
        align-items: center;
    }

    .qr-box img {
        width: 100px !important;
        height: 100px !important;
        object-fit: contain;
    }

</style>
