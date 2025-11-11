<template>
    <div class="forgot-container">
        <div class="ui raised padded segment forgot-box">
            <h2 class="ui header center aligned">Forgot Password</h2>

            <form class="ui form" @submit.prevent="handleSubmit">
                <!-- Email Field -->
                <div class="field">
                    <label>Email</label>
                    <input type="email" placeholder="Enter your email" v-model="email" required />
                </div>

                <!-- Submit Button -->
                <button class="ui primary button" type="submit">Send Reset Link</button>
            </form>

            <!-- Success / Error Messages -->
            <div v-if="successMessage" class="ui positive message" style="margin-top: 20px;">
                <p>{{ successMessage }}</p>
            </div>

            <div v-if="errorMessage" class="ui negative message" style="margin-top: 20px;">
                <p>{{ errorMessage }}</p>
            </div>

            <!-- Optional Links -->
            <div class="ui divider"></div>
            <div class="center aligned">
                <p>
                    Remember your password?
                    <a href="/signin">Sign in here!</a>
                </p>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        name: 'ForgotPasswordView',
        data() {
            return {
                email: '',
                successMessage: '',
                errorMessage: '',
                
                users: [
                    { email: 'vivian@example.com', name: 'Vivian' },
                ]
            };
        },
        methods: {
            handleSubmit() {
                const user = this.users.find(u => u.email === this.email);
                if (user) {
                    this.successMessage = "Reset link sent!";
                    this.errorMessage = '';
                    setTimeout(() => {
                        this.$router.push(`/reset-password?email=${encodeURIComponent(this.email)}`);
                    }, 1000);
                } else {
                    this.errorMessage = "Email not found!";
                    this.successMessage = '';
                }
            }
        }
    };
</script>


<style scoped>
    .forgot-container {
        display: flex;
        justify-content: center;
        align-items: center;
        min-height: 75vh;
        background-color: #f0f0f0;
    }

    .forgot-box {
        width: 400px;
        padding: 30px;
        border-radius: 10px;
        box-shadow: 0 6px 15px rgba(0,0,0,0.1);
        background-color: #fff;
    }

    .ui.form .field {
        margin-bottom: 15px;
    }
</style>
