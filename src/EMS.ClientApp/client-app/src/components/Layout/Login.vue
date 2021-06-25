<template>
  <div id="login-page" class="absolute-center">
    <div id="login-block-caption">Авторизація</div>
    <div id="error-block-caption" class="q-pb-xl">
      <!-- <div>{{ getAuthErrorMessage() }}</div> -->
    </div>
    <q-form @submit="onSubmit()">
      <q-input
        tabindex="1"
        clearable
        :rules="[
          (val) =>
            (!!val && val.length > 0) ||
            'Поле є обов\'язковим для заповнення',
        ]"
        v-model="login"
        label="Логін"
        class="text-input q-pb-md"
      >
        <template v-slot:prepend>
          <q-icon name="perm_identity" color="primary" size="1.2em" />
        </template>
      </q-input>
      <q-input
        tabindex="2"
        clearable
        :rules="[
          (val) =>
            (!!val && val.length > 0) ||
            'Поле є обов\'язковим для заповнення',
        ]"
        v-model="password"
        input-style="text-input"
        label="Пароль"
        class="text-input"
        type="password"
      >
        <template v-slot:prepend>
          <q-icon name="vpn_key" color="primary" size="1.1em" />
        </template>
      </q-input>
      <div id="stay-authorized-toggle-panel" class="row">
        <q-toggle v-model="stayAuthorized" color="primary" />
        <div style="padding-top: 8px">Залишатися в системі</div>
      </div>
      <div class="q-pt-xl row justify-center">
        <q-btn
          type="submit"
          :loading="isAuthWaiting"
          tabindex="1"
          class="col-1"
          color="primary"
          label="Вхід"
          style="width: 125px"
        />
      </div>
    </q-form>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";

export default defineComponent({
  name: "Login",
  setup() {
    return {
      login: ref(""),
      password: ref(""),
      isAuthWaiting: ref(false),
      stayAuthorized: ref(false)
    };
  },
  methods: {
    onSublmit(): void {
      console.log("test");
    },
  },
});
</script>

<style scoped lang="scss">
#login-page {
  font-family: Roboto-bold;
  color: gray;
  font-weight: bold;
}

#login-block-caption {
  font-size: 17pt;
  text-align: center;
}

#error-block-caption {
  text-align: center;
  height: 20px;
  color: red;
}

#stay-authorized-toggle-panel {
  margin: 15px 0px 0px -14px;
  font-size: 11pt;
  display: flex;
}

.text-input {
  width: 300px;
}
</style>
