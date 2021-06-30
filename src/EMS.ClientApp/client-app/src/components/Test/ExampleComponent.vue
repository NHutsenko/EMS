<template>
  <div class="absolute-center test">
    <span>Test component</span>

    <q-input :value="inputModel" @update:model-value="handleInput"/>
    <div>This is text from input: {{ exampleStoreStringGetter }}</div>
    <q-spinner-hourglass
      color="primary"
      size="4em"
      v-if="exapmleStoreBooleanGetter"
    />
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, computed } from "vue";
import store from "@/store/index";
import * as Getters from "@/store/constants/getters";
import * as Actions from "@/store/constants/actions";

export default defineComponent({
  name: "ExampleComponent",

  setup() {
    const exampleStoreStringGetter = computed(
      () => store.getters[Getters.GET_STRING_FIELD]
    );
    const exapmleStoreBooleanGetter = computed(
      () => store.getters[Getters.GET_BOOLEAN_FIELD]
    );

    return {
      inputModel: ref(""),
      exapmleStoreBooleanGetter,
      exampleStoreStringGetter,
    };
  },
  methods: {
    async handleInput(value: string): Promise<void> {
      await store.dispatch(Actions.EXAMPLE_ACTION, value);
    }
  },
});
</script>

<style lang="scss" scoped>
@import "@/styles/quasar.scss";

$test-font-size: 16pt;

.test {
  font-family: vostok-medium;
  font-size: $test-font-size;
}
</style>