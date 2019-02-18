<template>
    <div>
        <h4>Applicants</h4>
        <div>
            searceDiv
        </div>
        <div>
            <my-table-action :defaultSelect="defaultSelect" :defaultShowData="defaultShowData" @confirmSelRows="confirmSelRows">
                <el-dropdown class="mr-20" trigger="click" @command="handleCommand">
                    <el-button type="primary">
                        {{$t('BTN_ACTIONS')}}
                        <i class="el-icon-arrow-down el-icon--right"></i>
                    </el-button>
                    <el-dropdown-menu slot="dropdown">
                        <el-dropdown-item command="Del">delete</el-dropdown-item>
                    </el-dropdown-menu>
                </el-dropdown>

                <el-button @click="showSelTable('seltable')">{{$t('BTN_SELECTED_NUM')}}: {{selections.length}}</el-button>
            </my-table-action>
            <my-table @sortChange="sortChange" ref="mytable" @selectionChange="mytableSelectionChange" @rowClick="rowClick($event,'mytable')" :showData="showData0" :gridData="gridData0">
                <el-table-column slot="slotHeader" type="selection" width="55">
                </el-table-column>
                <el-table-column label="Actions">
                    <template slot-scope="scope">
                        <div>
                            <el-button @click="toUrl(`/applicants/${scope.row._id}/details`)" type="primary" size="mini">view</el-button>
                        </div>
                    </template>
                </el-table-column>
            </my-table>
        </div>

        <!-- selTable -->
        <el-dialog title="已选列表" :visible.sync="showSelections">
            <div class="scrollable only-y">
                <my-table ref="seltable" maxHeight="400" @rowClick="rowClick($event,'seltable')" :showData="selectionsShowData" :gridData="selections" @selectionChange="seltableSelectionChange">
                    <el-table-column type="selection" width="55">
                    </el-table-column>
                </my-table>
            </div>
            <span slot="footer" class="dialog-footer">
                <el-button @click="showSelections = false">{{$t('BTN_CANCEL')}}</el-button>
                <el-button type="primary" @click="confirmSelTable">{{$t('BTN_CONFIRM')}}</el-button>
            </span>
        </el-dialog>
    </div>
</template>

<script>
import queryMixins from "@/mixin/queryMixins";
import defaultShowData from "./indexShowdata";
import { armGet, armDel } from "@/api/api";

export default {
  name: "armApplicants",
  mixins: [queryMixins],
  mounted() {
    this.getApplicants();
  },
  data() {
    return {
      defaultShowData: defaultShowData,
      showData0: [],
      gridData0: [],
      selectionsShowData: [
        {
          type: "string",
          label: "id",
          prop: "_id"
        }
      ],
      loading: false
    };
  },
  methods: {
    getGridData() {
      this.getApplicants();
    },
    async getApplicants() {
      this.loading = true;
      let res = await armGet(`/applicants`);
      this.pagination.total = 1 * res.headers["x-total-count"];
      this.pagination.pageSize = 1 * res.headers["x-page-size"];
      this.gridData0 = res.data;
      setTimeout(() => {
        this.toggleSelection("mytable");
        this.loading = false;
      }, 50);
    }
  }
};
</script>

<style>
</style>
