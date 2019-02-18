<template>
  <div class="scrollable only-y">
    <my-table :showData="showData0" :gridData="gridData0">
      <el-table-column label="二维码">
        <template slot-scope="scope">
          <div @click="showQrcode(scope.row.EqmUID)">
            <qrcode :code="scope.row.EqmUID"/>
          </div>
        </template>
      </el-table-column>
      <el-table-column label="状态">
        <template slot-scope="scope">
          <el-button @click="viewDetail(scope.row.EqmUID)" type="primary" size="mini">查看详情</el-button>
        </template>
      </el-table-column>
    </my-table>
    <el-dialog title="提示" :visible.sync="dialogVisible" width="30%">
      <div>
        <div class="flex column align-center" id="dialogQrcode" ref="dialogQrcode"></div>
      </div>
      <span slot="footer" class="dialog-footer">
        <el-button type="primary" @click="downloadImg">下载</el-button>
        <el-button type="primary" @click="dialogVisible = false">确 定</el-button>
      </span>
    </el-dialog>
  </div>
</template>

<script>
// @ is an alias to /src
import { baseGet } from "@/api/api";
import EQMAPI from "@/model/EQMAPI.js";
import qrcode from "@/components/qrcode";
import showData0 from "./showData";
import QRCode from "qrcodejs2";
import { exportFile } from "@/vendor/exportFile";
import { html2Png } from "@/utils/html2Png";
let EQMApi = new EQMAPI();

export default {
  name: "home",
  mounted() {
    console.log(this.ProductCode);
    this.getTopEQM();
  },
  data() {
    return {
      dialogVisible: false,
      total: 0,
      showData0: showData0,
      gridData0: [],
      res: null,
      Switch: "",
      MaintainProductCode: "d003",
      ProductCode: "AT4810GZ201812290001",
      currentEqmId: ""
    };
  },
  components: {
    qrcode
  },
  methods: {
    async downloadImg() {
      let arr = [];
      let el = document.getElementById(`dialogQrcode`);
      let res = await html2Png(el, false);
      arr.push({
        fname: `${this.currentEqmId}.png`,
        data: res
      });
      await exportFile(arr);
    },
    showQrcode(id) {
      console.log(id);
      this.currentEqmId = id;
      this.dialogVisible = true;
      this.$nextTick(() => {
        document.getElementById("dialogQrcode").innerHTML = "";
        let qrcode = new QRCode("dialogQrcode", {
          width: 200, // 设置宽度
          height: 200, // 设置高度
          text: id
        });
        const span = document.createElement("span");
        span.innerHTML = id;
        document.getElementById("dialogQrcode").appendChild(span);
      });
    },
    viewDetail(id) {
      this.$router.push({
        path: `/equiment/${id}`
      });
    },
    async getTopEQM() {
      let obj = {
        MemberCode: "fb85ff30-8e1e-4afe-bc5b-5b287d2bf1c3",
        pageSize: 10,
        pageIndex: 1,
        Latitude: 23.02067,
        Longitude: 113.75179
      };
      let res = await EQMApi.GetTopEQM(obj);

      // let res = await baseGet(`/EQM/GetTopEQM`, obj);
      let { data, total } = res.data.entity;
      this.gridData0 = data;
      this.total = total;
      // this.res = JSON.stringify(res.data.entity, null, 2);
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