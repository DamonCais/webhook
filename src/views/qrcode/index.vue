<template>
  <div v-loading="loading" class="scrollable only-y">
    <el-tabs v-model="activeName">
      <el-tab-pane label="电池二维码">
        <my-form style="width:500px;">
          <my-form-item label="前缀">
            <el-input v-model="prefix"></el-input>
          </my-form-item>
          <my-form-item label="后缀范围">
            <el-input v-model="start"></el-input>
            <span>~</span>
            <el-input v-model="end"></el-input>
          </my-form-item>
          <my-form-item>
            <el-button @click="mdQrcode" type="primary">生成</el-button>
            <el-button @click="getZip" type="warning">生成图片压缩包</el-button>
            <el-button @click="getImg" type="warning">生成大图</el-button>
            <el-button @click="printQrcode" type="danger">打印</el-button>
          </my-form-item>
        </my-form>
      </el-tab-pane>
      <el-tab-pane label="设备二维码">
        <my-form style="width:500px;">
          <my-form-item label="前缀">
            <el-input v-model="prefix2"></el-input>
          </my-form-item>
          <my-form-item v-for="(item,index) in ids" :key="index" :label="`设备id${index+1}`">
            <el-input ref="ids" @keyup.enter.native="addId" v-model="ids[index]"></el-input>
          </my-form-item>
          <my-form-item>
            <el-button @click="addId" type="primary">增加</el-button>
            <el-button @click="cleanIds" type="primary">清除</el-button>
            <el-button @click="mdQrcode" type="primary">生成</el-button>
            <el-button @click="getZip" type="warning">生成图片压缩包</el-button>
            <el-button @click="getImg" type="warning">生成大图</el-button>
            <el-button @click="printQrcode" type="danger">打印</el-button>
          </my-form-item>
        </my-form>
      </el-tab-pane>
    </el-tabs>
    <div class="qrcodes" id="qrcodes" ref="qrcode"></div>
  </div>
</template>

<script>
import QRCode from "qrcodejs2";
import { html2Png } from "@/utils/html2Png";
import { exportFile } from "@/vendor/exportFile";

export default {
  data() {
    return {
      prefix: "AT4810GZ20181229",
      prefix2: "http://jvstest.juvending.cn/sp?id=",
      start: "0001",
      end: "0099",
      num: 0,
      qrcodeList: [],
      ids: [""],
      loading: false,
      activeName: "0"
    };
  },
  methods: {
    cleanIds() {
      this.ids = [""];
    },
    addId(e) {
      this.ids.push("");
      this.$nextTick(() => {
        this.$refs.ids[this.$refs.ids.length - 1].focus();
      });
    },
    async getImg() {
      let el = document.getElementById(`qrcodes`);
      let blob = await html2Png(el, false);
      const link = document.createElement("a");
      link.href = URL.createObjectURL(blob);
      link.setAttribute("download", "qrcode.png");
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
    },
    async getZip() {
      this.loading = true;
      let arr = [];
      if (this.activeName == "0") {
        for (var i = 0; i < this.qrcodeList.length; i++) {
          let val = i + 1 * this.start;
          let num = ("00000000" + val).substr(`-${this.start.length}`);
          let el = document.getElementById(`qrcode${num}`);
          let res = await html2Png(el, false);
          console.log(i);
          arr.push({
            fname: `${this.qrcodeList[i]}.png`,
            data: res
          });
        }
      } else {
        for (var i = 0; i < this.ids.length; i++) {
          if (this.ids[i]) {
            let el = document.getElementById(`qrcode${this.ids[i]}`);
            let res = await html2Png(el, false);
            arr.push({
              fname: `${this.ids[i]}.png`,
              data: res
            });
          }
        }
      }

      await exportFile(arr);
      this.loading = false;
    },
    async printQrcode() {
      let el = document.getElementById("qrcodes");
      await html2Png(el);
    },
    mdQrcode() {
      document.getElementById("qrcodes").innerHTML = "";
      if (this.activeName == "0") {
        this.qrcodeList = [];
        let length = 1 * this.end - 1 * this.start + 1;
        console.log(length);
        for (var i = 0; i < length; i++) {
          let val = i + 1 * this.start;
          let num = ("00000000" + val).substr(`-${this.start.length}`);
          this.qrcode(num);
        }
      } else {
        for (var i = 0; i < this.ids.length; i++) {
          if (this.ids[i]) {
            this.qrcode(this.ids[i]);
          }
        }
      }
    },
    qrcode(num) {
      let div = document.createElement("div");
      div.setAttribute("id", `qrcode${num}`);
      let code = (this.activeName == "0" ? this.prefix : this.prefix2) + num;
      document.getElementById("qrcodes").appendChild(div);
      let qrcode = new QRCode(`qrcode${num}`, {
        width: 180, // 设置宽度
        height: 180, // 设置高度
        text: code
      });
      const span = document.createElement("span");
      span.innerHTML = code;
      document.getElementById(`qrcode${num}`).appendChild(span);
      this.qrcodeList.push(code);

      // setTimeout(() => {
      //   this.downloadClick();
      // }, 20);
    }
  }
};
</script>

<style lang="scss">
.qrcodes {
  display: flex;
  flex-wrap: wrap;
  padding: 20px;
  div {
    display: flex;
    flex-direction: column;
    align-items: center;
    margin: 0 30px 30px 0;
  }
}
</style>
