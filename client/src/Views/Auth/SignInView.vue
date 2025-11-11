<template>
    <div class="signin-container">
        <div class="ui raised padded segment signin-box">
            <h2 class="ui header center aligned">Sign In</h2>

            <form class="ui form" @submit.prevent="handleSubmit">
                <div class="field">
                    <label>Email</label>
                    <input type="email" placeholder="Enter your email" v-model="email" required />
                </div>

                <div class="field">
                    <label>Password</label>
                    <input type="password" placeholder="Enter your password" v-model="password" required />
                </div>

                <button class="ui primary button" type="submit">Sign In</button>
                <p>
                    <a href="/forgot-password">Forgot Password?</a>
                </p>
            </form>

            <div v-if="successMessage" class="ui positive message" style="margin-top: 20px;">
                <p>{{ successMessage }}</p>
            </div>

            <div v-if="errorMessage" class="ui negative message" style="margin-top: 20px;">
                <p>{{ errorMessage }}</p>
            </div>

            <div class="ui divider"></div>
            <div class="center aligned">
                <p>
                    Don't have an account?
                    <a href="/signup">Sign up here!</a>
                </p>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        name: 'SigninView',
        data() {
            return {
                email: '',
                password: '',
                successMessage: '',
                errorMessage: '',

                // Fake user
                users: [
                    {
                        name: "Vivian",
                        email: "vivian@example.com",
                        password: "123456",
                        avatar: "https://i.pravatar.cc/40?img=19"
                    },
                ]
            };
        },
        methods: {
            handleSubmit() {
                const user = this.users.find(
                    u => u.email === this.email && u.password === this.password
                );

                if (user) {
                    this.successMessage = `Welcome back, ${user.name}!`;
                    this.errorMessage = '';

                    // Save user info globally (ví dụ localStorage tạm thời)
                    localStorage.setItem('currentUser', JSON.stringify(user));
                    window.dispatchEvent(new Event("storage"));
                    // Redirect to home
                    this.$router.push('/');

                    // Reset form
                    this.email = '';
                    this.password = '';
                } else {
                    this.errorMessage = "Invalid email or password!";
                    this.successMessage = '';
                }
            }

        }
    };
</script>

<style scoped>
    .signin-container {
        display: flex;
        justify-content: center;
        align-items: center;
        min-height: 75vh;
        background-color: #f0f0f0;
    }

    .signin-box {
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
