<template>
    <footer class="ui inverted vertical segment footer">
        <div class="ui container">
            <div class="ui stackable grid">
                <div class="eight wide column">
                    <p>
                        © 2025 <strong>MyURL</strong>. From Team <span style="color:#00b5ad;">tuanGayAMD</span> with love 💙
                    </p>
                </div>

                <div class="eight wide right aligned column">
                    <div class="ui horizontal inverted link list">
                        <a class="item" href="#">Terms</a>
                        <a class="item" href="#">Privacy</a>
                        <a class="item" href="#">Contact</a>
                    </div>
                </div>
            </div>
        </div>
    </footer>
</template>

<script>
    export default {
        name: "AppFooter",
    };
</script>

<style scoped>
    .footer {
        position: relative;
        width: 100%;
        background: #1b1c1d !important;
        color: #f1f1f1;
        padding: 1.5rem 0;
        text-align: center;
        box-shadow: 0 -2px 6px rgba(0, 0, 0, 0.2);
        margin-top: auto; /* đẩy footer xuống cuối khi nội dung ngắn */
    }

        .footer a {
            color: #f1f1f1 !important;
            opacity: 0.9;
        }

            .footer a:hover {
                color: #00b5ad !important;
                opacity: 1;
            }
</style>

