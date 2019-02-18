<template>
  <div>
    <toolbar>
      <el-button @click="showUpload=true" type="primary">
        {{$t('BTN_UPLOAD_AUTOPAY_REPORT_FILE')}}
      </el-button>
      <div>
        <!-- <input style="position:absolute;left:-9999px;" id='fileupload' ref="fileupload" type="file" @change="uploadFile">
        <label class="el-button el-button--small" for='fileupload'>uploadFile</label> -->
      </div>
    </toolbar>
    <div class="p-20">
      <h2>{{$t('NAV_REPORT_FILES')}}</h2>
      <div class="mb-20">
        <my-table @sortChange="sortChange" ref="mytable" :showData="showData0" :gridData="gridData0">
          <el-table-column :label="$t('TABLE_ACTIONS')">
            <template slot-scope="scope">
              <el-button @click.stop="toUrl(`/autopay/report-files/${scope.row._id}`)" type="primary" size="mini">{{$t('BTN_VIEW')}}</el-button>
            </template>
          </el-table-column>
        </my-table>
      </div>
      <div class="flex justify-flex-start">
        <el-pagination @size-change="handleSizeChange" @current-change="handleCurrentChange" :current-page.sync="pagination.currentPage" :page-sizes="[10, 20, 30, 40, 50, 100]" :page-size="pagination.pageSize" layout="total, sizes, prev, pager, next, jumper" :total="pagination.total">
        </el-pagination>
      </div>

    </div>
    <el-dialog title="Upload report file" :visible.sync="showUpload">
      <div class="flex justify-space-around">
        <my-upload ref="myupload" @fileChange="fileChange" />
      </div>
      <span slot="footer" class="dialog-footer">
        <el-button @click="showUpload = false">{{$t('BTN_CANCEL')}}</el-button>
        <el-button type="primary" @click="confirmUpload">{{$t('BTN_CONFIRM')}}</el-button>
      </span>
    </el-dialog>
  </div>
</template>

<script>
import queryMixins from "@/mixin/queryMixins";
import defaultShowData from "./listShowdata";
import { billPostFile, billPost, billGet } from "@/api/api";
import myUpload from "./myUpload";
export default {
  name: "billAutopayReport",
  mixins: [queryMixins],
  mounted() {
    this.getAutopayReportFiles();
  },
  components: {
    myUpload
  },
  data() {
    return {
      defaultShowData: defaultShowData,
      showData0: [],
      gridData0: [],
      showUpload: false,
      files: null
    };
  },
  methods: {
    getGridData() {
      this.getAutopayReportFiles();
    },
    fileChange(files) {
      this.files = files;
    },
    async confirmUpload() {
      console.log(this.files.length);
      for (var i in this.files) {
        if (this.files[i]) {
          try {
            const formData = new FormData();
            await formData.append("autopayfile", this.files[i]);
            let res = await billPostFile("/autopay-report-files", formData);
          } catch (error) { }
        }
      }
      this.$refs.myupload.files = [];
      this.getAutopayReportFiles();
      this.showUpload = false;
    },
    async getAutopayReportFiles() {
      this.loading = true;
      let obj = {
        p: this.pagination.currentPage - 1,
        sort: this.sort,
        pageSize: this.pagination.pageSize
      };
      let res = await billGet(`/autopay-report-files`);
      this.gridData0 = res.data;
      this.pagination.total = 1 * res.headers["x-total-count"];
      this.pagination.pageSize = 1 * res.headers["x-page-size"];
      setTimeout(() => {
        this.loading = false;
      }, 50);
    }
  }
};
</script>

<style>
</style>
