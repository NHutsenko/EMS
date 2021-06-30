<template>
  <q-layout view="lHh Lpr lFf">
    <page-header :name="'Custom Header App Name'" />
    <q-page-container>
      <router-view />
      <div v-if="isOperationWaiting" class="operation-waiting">
        <q-spinner-hourglass
          class="fixed-center"
          color="primary"
          size="4em"
        />
      </div>
    </q-page-container>
  </q-layout>
</template>

<script lang="ts">
import { defineComponent, computed, ComputedRef } from "vue";
import PageHeader from "@/components/Layout/PageHeader.vue";
import store from "@/store/index";
import * as Getters from "@/store/constants/getters";

export default defineComponent({
  name: "LayoutDefault",
  components: {
    PageHeader,
  },
  setup() {
    const isOperationWaiting: ComputedRef<boolean> = computed(
      () => store.getters[Getters.GET_OPERATION_WAITING_FLAG]
    );
    return {
      isOperationWaiting,
    };
  },
});
</script>

<style lang="scss" scoped>
@import "@/styles/quasar.scss";
.operation-waiting {
  text-align: center;
  color: $primary;
  font-family: vostok-medium;
  font-size: 16pt;
  background-color: rgba(105, 103, 103, 0.15);
  height: 100%;
  width: 100%;
  position: absolute;
  z-index: 1000;
}

</style>