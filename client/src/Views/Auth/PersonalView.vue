<template>
    <div class="ui container" style="padding-top: 60px;">
        <div class="ui raised very padded segment" style="max-width: 700px; margin: auto;">
            <h3 class="ui header">Profile Management</h3>
            <h2 class="ui dividing header">Update Information</h2>

            <div class="ui center aligned basic segment">
                <img :src="preview || currentUser.avatar" class="ui circular small image centered" />
                <div class="ui hidden divider"></div>

                <div class="ui green button" @click="$refs.fileInput.click()">Select a Photo</div>
                <input type="file"
                       accept="image/*"
                       ref="fileInput"
                       style="display: none"
                       @change="onFileChange" />
            </div>

            <div class="ui form">
                <h4 class="ui dividing header">Contact Information</h4>

                <div class="field">
                    <label>Name</label>
                    <input type="text" v-model="form.name" />
                </div>

                <div class="field">
                    <label>Email Address</label>
                    <input type="email" v-model="form.email" />
                </div>

                <button class="ui primary button" @click="updateProfile">
                    Update
                </button>
            </div>

            <div class="ui divider"></div>
            <button class="ui red basic button right floated" @click="deactivateAccount">
                Deactivate Account
            </button>
        </div>
    </div>
</template>

<script>
    export default {
        data() {
            return {
                currentUser: {},
                form: { name: "", email: "" },
                preview: null,
            };
        },
        mounted() {
            const storedUser = localStorage.getItem("currentUser");
            if (storedUser) {
                this.currentUser = JSON.parse(storedUser);
                this.form.name = this.currentUser.name;
                this.form.email = this.currentUser.email;
            }
        },
        methods: {
            onFileChange(e) {
                const file = e.target.files[0];
                if (file) {
                    this.preview = URL.createObjectURL(file);
                }
            },
            updateProfile() {
                // giả lập update localStorage
                this.currentUser.name = this.form.name;
                this.currentUser.email = this.form.email;
                if (this.preview) this.currentUser.avatar = this.preview;

                localStorage.setItem("currentUser", JSON.stringify(this.currentUser));
                alert("Profile updated successfully!");
                window.dispatchEvent(new Event("storage"));
            },
            deactivateAccount() {
                if (confirm("Are you sure you want to deactivate your account?")) {
                    localStorage.removeItem("currentUser");
                    window.dispatchEvent(new Event("storage"));
                    this.$router.push("/signin");
                }
            },
        },
    };
</script>

<style scoped>
    .ui.container {
        padding-top: 60px;
    }
</style>
