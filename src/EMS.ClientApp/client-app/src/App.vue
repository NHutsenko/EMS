<template>
  <q-layout view="lHh Lpr lFf">
    <page-header :name="'Custom Header App Name'" />
    <q-page-container>
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script>
import { defineComponent } from "vue";
import PageHeader from "@/components/Layout/PageHeader.vue";

export default defineComponent({
  name: "LayoutDefault",
  components: {
    PageHeader,
  },
});
</script>
