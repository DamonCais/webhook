<template>
  <div class="layout-container flex">
    <base-nav/>
    <router-view class="box grow p-20"/>
  </div>
</template>

<script>
import baseNav from "@/nav/nav";
export default {
  components: {
    baseNav: baseNav
  }
};
</script>


<style lang="scss">
.layout-container {
  height: 100%;
  width: 100%;
}
</style>
