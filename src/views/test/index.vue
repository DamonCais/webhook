<template>
    <div>
        <el-input class="mb-20" v-model="func"></el-input>
        <el-button @click="addFunc">添加方法</el-button>
        <div class="mt-20">
            <el-button v-for="(f,index) in funcArr" @click="doFunc(f)" :key="index">方法{{index}}</el-button>
        </div>
    </div>
</template>

<script>
export default {
  data() {
    return {
      funcArr: [],
      func: ""
    };
  },
  methods: {
    addFunc() {
      this.funcArr.push(this.func);
    },
    doFunc(f) {
      eval(f);
    }
  }
};
</script>

<style>
</style>
