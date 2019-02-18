<template>
  <div class="base-nav">
    <el-menu
      :default-active.sync="activeLink"
      @select="goto"
      class="el-menu-body"
      style="width:240px;height:100%;"
    >
      <el-menu-item index="/home">
        <i class="el-icon-setting"></i>
        <span slot="title">设备列表</span>
      </el-menu-item>
      <el-menu-item index="/member">
        <i class="el-icon-setting"></i>
        <span slot="title">用户列表</span>
      </el-menu-item>
      <el-menu-item index="/qrcode">
        <i class="el-icon-setting"></i>
        <span slot="title">二维码生成</span>
      </el-menu-item>
      <el-menu-item index="/test">
        <i class="el-icon-setting"></i>
        <span slot="title">测试</span>
      </el-menu-item>
    </el-menu>
  </div>
</template>

<script>
export default {
  created() {
    this.setLink(this.$router.currentRoute.path);
    this.$router.afterEach((to, from) => {
      this.setLink(this.$router.currentRoute.path);
    });
  },
  methods: {
    setLink(path) {
      console.log(path);
      let arr = path.split("/");
      this.activeLink = "/" + arr[1];
    },
    goto(index, indexPath) {
      if (index.charAt(0) === "/") {
        this.$router.push(index);
      }
    }
  },
  data() {
    return {
      activeLink: null
    };
  }
};
</script>

<style lang="scss">
$base-module-color: #00897b;
$base-module-color-isactive: #00564d;
$text-white: #fff;
.base-nav {
  // width: 240px;
  background: $base-module-color;
  color: $text-white;
  .el-menu-header,
  .el-menu-footer,
  .el-menu-body {
    border-right: none;
    background: $base-module-color;
    color: $text-white;
    .el-menu-item,
    .el-submenu {
      font-size: 16px;
      // position: relative;
      color: $text-white;
      i {
        color: $text-white;
        margin-right: 20px;
        font-size: 24px;
      }
      &:hover {
        background: $base-module-color-isactive;
      }

      &.is-active {
        color: $text-white;
        background: $base-module-color-isactive;
      }
    }
  }
}
</style>
