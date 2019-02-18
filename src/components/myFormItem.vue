<template>
  <div class="flex gaps form-item p-10" :style="{'flex-basis':flexBasis}">
    <div
      v-if="labelPosition!='top'"
      :style="{'width':myForm.labelWidth+'px','text-align':labelPosition}"
      :class="[{'form-item-label__isrequired':isRequired},'form-label']"
    >{{label}}</div>
    <div class="box grow">
      <div
        v-if="labelPosition=='top'"
        :class="[{'form-item-label__isrequired':isRequired}]"
      >{{label}}</div>
      <!-- <el-tooltip
        :disabled="!(check(value).errMsg)||errorPosition!='pop'"
        :content="check(value).errMsg"
        placement="top-start"
      >-->
      <div class="flex gaps" :class="{'require':(check(value).errMsg)}" style="position:relative;">
        <slot/>
        <div
          style="position:absolute;top:100%;"
          v-if="errorPosition==='bottom'"
          class="fs-10 danger-text"
        >{{check(value).errMsg}}</div>
      </div>
      <!-- </el-tooltip> -->
    </div>
    <div
      v-if="errorPosition==='right'"
      class="fs-10 danger-text box"
      style="flex-basis:20%;"
    >{{check(value).errMsg}}</div>
  </div>
</template>

<script>
import valids from "./validate";
export default {
  provide() {
    return {
      myFormItem: this
    };
  },
  inject: ["myForm"],
  props: {
    label: String,
    prop: String,
    basis: [String, Number],
    triggerArr: {
      type: Array,
      default() {
        return [];
      }
    },
    validateRule: {
      type: [Function, String],
      default: null
    },
    required: Boolean
  },
  computed: {
    isRequired() {
      return this.required || Boolean(this.validateRule);
    },
    value() {
      return _.get(this.myForm.model, this.prop);
    },
    flexBasis() {
      return (this.basis || 100 / this.myForm.itemsInRow) + "%";
    },
    checkRequire() {
      return this.myForm.checkRequire || this.triggerRequire;
    },
    labelPosition() {
      return this.myForm.labelPosition;
    },
    errorPosition() {
      return this.myForm.errorPosition;
    }
  },
  data() {
    return {
      inputEl: null,
      triggerRequire: false
    };
  },
  methods: {
    validate() {
      return new Promise((resolve, reject) => {
        let val = _.get(this.myForm.model, this.prop);
        let obj = this.check(val, true);
        if (obj && obj.errMsg) {
          reject(obj.errMsg);
        } else {
          resolve();
        }
      });
    },
    check(val, bool) {
      bool = bool || this.checkRequire;
      if (!this.validateRule) {
        return { errMsg: "" };
      } else if (typeof this.validateRule == "function") {
        return this.validateRule(val, bool) || { errMsg: "" };
      } else if (valids[`validate${this.validateRule}`]) {
        return valids[`validate${this.validateRule}`](val, bool);
      } else {
        return { errMsg: "" };
      }
    },
    triggerFun() {
      this.triggerArr.forEach(triggerType => {
        if (triggerType === "blur") {
          for (var el of this.inputEl) {
            el.addEventListener("blur", () => {
              this.triggerRequire = true;
            });
          }
        } else if (triggerType === "change") {
          for (var el of this.inputEl) {
            el.addEventListener("change", () => {
              this.triggerRequire = true;
            });
          }
        }
      });
    }
  },
  mounted() {
    this.myForm.addField(this);
    let el = this.$el;
    // this.inputEl = el.querySelector("input") || el.querySelector("textarea");
    this.inputEl = el.getElementsByTagName("input");
    if (this.inputEl) {
      this.triggerFun();
    }
  },
  updated() {},
  beforeDestroy() {}
};
</script>

<style lang="scss" scoped>
.form-item {
  display: flex;
  flex-shrink: 0;
  align-items: center;
  box-sizing: border-box;
  .form-label {
    font-size: 14px;
    color: #606266;
    line-height: 40px;
    font-weight: 700;
  }
  .form-item-label__isrequired:before {
    content: "*";
    color: #f56c6c;
    margin-right: 4px;
  }
}
</style>
<style lang="scss">
.form-item {
  .el-date-editor,
  .el-select {
    width: 100%;
  }
}
</style>

