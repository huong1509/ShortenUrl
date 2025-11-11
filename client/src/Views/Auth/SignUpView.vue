<template>
    <div class="signup-container">
        <div class="ui raised segment signup-box">
            <h2 class="ui header center aligned">Sign Up</h2>
            
            <form class="ui form" @submit.prevent="handleSubmit">
                <div class="field">
                    <label>Name</label>
                    <input type="text" placeholder="Your Name" v-model="name" required />
                </div>

                <div class="field">
                    <label>Email</label>
                    <input type="email" placeholder="Email" v-model="email" required />
                </div>

                <div class="field">
                    <label>Password</label>
                    <input type="password" placeholder="Password" v-model="password" required />
                </div>

                <button class="ui primary button" type="submit">Sign Up</button>
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
                    Already have an account?
                    <a href="/signin">Sign in here!</a>
                </p>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        name: 'SignupView',
        data() {
            return {
                name: '',
                email: '',
                password: '',
                successMessage: '',
                errorMessage: ''
            };
        },
        methods: {
            handleSubmit() {
                if (!this.name || !this.email || !this.password) {
                    this.errorMessage = "All fields are required!";
                    this.successMessage = '';
                    return;
                }

                this.successMessage = `Welcome, ${this.name}! Your account has been created.`;
                this.errorMessage = '';

                // Reset form
                this.name = '';
                this.email = '';
                this.password = '';
            }
        }
    };
</script>

<style scoped>
    .signup-container {
        display: flex;
        justify-content: center; 
        align-items: center; 
        min-height: 75vh;
        background-color: #f0f0f0;
    }

    .signup-box {
        width: 400px;
        padding: 30px;
        box-shadow: 0 4px 12px rgba(0,0,0,0.15);
        border-radius: 10px;
    }

    .ui.form .field {
        margin-bottom: 15px;
    }
</style>
