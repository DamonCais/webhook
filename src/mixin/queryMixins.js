/* 
  ** queryMixins说明:
  默认参数: pagination(分页相关操作),sort(排序),
  selections, 当前选择,templateSelection,已选列表临时选择,showSelections,显示已选列表.

  原代码中加入如下字段:
  import queryMixins from "@/mixin/queryMixins";
  mixins: [queryMixins],
  name: "armFroms", //用于区别当前页面名字,
  默认 ref:mytable 为主要显示表
  ref:seltable 为已选列表

  template 中加入
   <el-button @click="showSelTable('seltable')">{{$t('BTN_SELECTED_NUM')}}: {{selections.length}}</el-button>

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

    方法中添加
    getGridData() {
      this.getForms(); //实际的getGridData方法
    },
    切换选择中,添加'ref'值
    this.toggleSelection('mytable');

*/

import myTable from "@/components/myTable";
import myTableAction from "@/components/myTableAction";

export default {
  components: {
    myTable,
    myTableAction
  },
  data() {
    return {
      pagination: {
        currentPage: 1,
        total: 0,
        pageSize: 10
      },
      sort: "",
      selections: [],
      templateSelection: [],
      showSelections: false
    };
  },
  methods: {
    //每页条数
    handleSizeChange(val) {
      this.pagination.pageSize = val;
      this.getGridData();
    },
    // 分页
    handleCurrentChange(val) {
      this.pagination.currentPage = val;
      this.getGridData();
    },
    // 排序
    sortChange(e) {
      let {
        column,
        prop,
        order
      } = e;
      this.sort = "";
      this.pagination.currentPage = 1;
      if (prop) {
        this.sort = order === "ascending" ? prop : `-${prop}`;
      }
      this.getGridData();
    },
    // 选择行
    rowClick(row, ref) {
      this.$refs[ref].toggleRowSelection(row);
    },
    // 默认通过_id来切换选择行,如果不存在此参数可对方法重写
    mytableSelectionChange(selections) {
      let arr = _.map(selections, "_id");
      let unselArr = _.map(
        this.gridData0.filter(s => arr.indexOf(s._id) === -1),
        "_id"
      );
      let templateArr = _.map(this.selections, "_id");
      this.selections = _.concat(
        this.selections,
        selections.filter(s => templateArr.indexOf(s._id) === -1)
      ); //合并
      this.selections = this.selections.filter(
        s => unselArr.indexOf(s._id) === -1
      ); //删除
    },
    // 同上,默认通过_id切换
    toggleSelection(ref) {
      let rows = this.gridData0.filter(
        s => _.map(this.selections, "_id").indexOf(s._id) !== -1
      );
      this.$refs[ref].toggleSelection(rows);
    },
    // 确认展示列
    confirmSelRows(selections) {
      let arr = selections.sort(function (a, b) {
        return a.position - b.position;
      });
      this.showData0 = _.cloneDeep(arr);
      let obj = {};
      obj[this.$options.name] = _.map(arr, "prop");
      this.$store.commit("SET_TABLE_SETTING", obj);
      this.$refs.mytable.refresh();
    },
    // for the selectTableList
    seltableSelectionChange(e) {
      this.templateSelection = e;
    },
    confirmSelTable() {
      this.selections = _.cloneDeep(this.templateSelection);
      this.getGridData();
      this.showSelections = false;
    },
    showSelTable(ref) {
      this.showSelections = true;
      this.templateSelection = _.cloneDeep(this.selections);
      setTimeout(() => {
        this.$refs[ref].toggleSelection(this.selections, true);
      }, 50);
    }
  },
  created() {
    let arr;
    let key = this.$options.name;
    console.log("key" + key);
    if (!key) {
      return;
    }
    if (this.tableSetting[key] && this.tableSetting[key].length) {
      let propList = this.tableSetting[key];
      arr = _.cloneDeep(
        this.defaultShowData.filter(s => propList.indexOf(s.prop) != -1)
      );
    } else {
      arr = _.cloneDeep(this.defaultShowData.filter(s => s.isDefault));
    }
    this.showData0 = arr.sort((a, b) => {
      a.position - b.position;
    });
  },
  mounted() {},
  computed: {
    defaultSelect() {
      let key = this.$options.name;
      if (key) {
        return this.tableSetting[key] || [];
      } else {
        return [];
      }
    }
  }
};