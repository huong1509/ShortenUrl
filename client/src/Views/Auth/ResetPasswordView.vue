<template>
    <div class="reset-container">
        <div class="ui raised padded segment reset-box">
            <h2 class="ui header center aligned">Reset Password</h2>

            <form class="ui form" @submit.prevent="handleReset">
                <div class="field">
                    <label>Email</label>
                    <input type="email" v-model="email" readonly />
                </div>

                <div class="field">
                    <label>New Password</label>
                    <input type="password" v-model="password" placeholder="Enter new password" required />
                </div>

                <div class="field">
                    <label>Confirm Password</label>
                    <input type="password" v-model="confirmPassword" placeholder="Confirm new password" required />
                </div>

                <button class="ui teal button fluid" type="submit">Reset Password</button>
            </form>

            <div v-if="successMessage" class="ui positive message" style="margin-top: 20px;">
                <p>{{ successMessage }}</p>
            </div>
            <div v-if="errorMessage" class="ui negative message" style="margin-top: 20px;">
                <p>{{ errorMessage }}</p>
            </div>
        </div>
    </div>
</template>

<script>
export default {
  name: "ResetPasswordView",
  data() {
    return {
      email: "",
      password: "",
      confirmPassword: "",
      successMessage: "",
      errorMessage: ""
    };
  },
  mounted() {
    // Lấy email từ query (VD: /reset-password?email=vivian@example.com)
    this.email = this.$route.query.email || "";
  },
  methods: {
    handleReset() {
      if (!this.email) {
        this.errorMessage = "Email not found!";
        return;
      }

      if (this.password !== this.confirmPassword) {
        this.errorMessage = "Passwords do not match!";
        return;
      }

      if (this.password.length < 6) {
        this.errorMessage = "Password must be at least 6 characters.";
        return;
      }

      // Giả lập cập nhật mật khẩu thành công
      this.successMessage = "Password reset successfully!";
      this.errorMessage = "";

      setTimeout(() => {
        this.$router.push("/signin");
      }, 1500);
    }
  }
};
</script>

<style scoped>
    .reset-container {
        display: flex;
        justify-content: center;
        align-items: center;
        min-height: 75vh;
        background-color: #f0f0f0;
    }

    .reset-box {
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
