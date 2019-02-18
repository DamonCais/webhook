<template>
  <div :id="`qrcode${code}`" ref="qrcode"></div>
</template>

<script>
import QRCode from "qrcodejs2";

export default {
  props: {
    code: {
      type: String,
      required: true
    }
  },
  methods: {
    qrcode() {
      let qrcode = new QRCode(`qrcode${this.code}`, {
        width: 50, // 设置宽度
        height: 50, // 设置高度
        text: this.code
      });
    }
  },
  mounted() {
    this.qrcode();
  }
};
</script>

<style>
</style>
