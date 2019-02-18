<template>
  <div class="flex" style="flex-wrap:wrap;">
    <slot/>
  </div>
</template>

<script>
import myFormItem from "./myFormItem";
export default {
  provide() {
    return {
      myForm: this
    };
  },
  components: {
    myFormItem
  },
  props: {
    itemsInRow: {
      type: Number,
      default: 1
    },
    labelWidth: {
      type: Number,
      default: 120
    },
    errorPosition: {
      type: String,
      default: "bottom"
    },
    labelPosition: {
      type: String,
      default: "left"
    },
    model: Object,
    forms: Array
  },
  data() {
    return {
      fields: [],
      checkRequire: false,
      baseForm: {}
    };
  },
  mounted() {
    this.baseForm = _.cloneDeep(this.model);
  },
  methods: {
    addField(field) {
      if (field) {
        this.fields.push(field);
      }
    },
    validateAll() {
      let arr = [];
      this.checkRequire = true;
      this.fields.map(f => {
        arr.push(f.validate());
      });
      return Promise.all(arr);
    },
    resetField() {
      this.checkRequire = false;
      this.$emit("update:model", this.baseForm);
      this.fields.map(f => {
        f.triggerRequire = false;
      });
    }
  }
};
</script>

<style lang="scss" scoped>
</style>
