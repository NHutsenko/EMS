<template>
  <div id="header">
    <q-header elevated class="glossy bg-black">
      <q-toolbar>
        <q-toolbar-title>{{ appHeaderName }}</q-toolbar-title>
        <q-btn
          flat
          dense
          round
          @click="onSideMenuClick()"
          aria-label="Menu"
          icon="menu"
        />
      </q-toolbar>
    </q-header>

    <q-drawer
      v-model="drawerOpen"
      side="right"
      overlay
      bordered
      class="bg-grey-2"
    >
    </q-drawer>
  </div>
</template>

<script lang="ts">
import { computed, defineComponent, ref } from "vue";

export default defineComponent({
  name: "PageHeader",
  props: {
    name: {
      required: false,
      type: String,
    }
  },
  setup(props) {
    const appHeaderName = computed(() =>
      props.name ? props.name : "Default AppName header"
    );
    return {
      drawerOpen: ref(false),
      appHeaderName
    };
  },
  methods: {
    onSideMenuClick(): void {
      this.drawerOpen = !this.drawerOpen;
    },
  },
});
</script>

<style lang="scss" scoped>
.logo{
  height: 35px;
  width: 125px;
}
</style>