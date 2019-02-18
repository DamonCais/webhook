<template>
  <div>
    <el-table
      v-if="showTable"
      :max-height="maxHeight"
      :summary-method="getSummaries"
      :show-summary="showSummary"
      ref="table"
      :row-class-name="tableRowClassName"
      @filter-change="filterChange"
      @sort-change="sortChange"
      stripe
      :data="gridData"
      @row-click="rowClick"
      @select-all="handleSelectionChange"
      @select="handleSelectionChange"
      style="width:100%"
    >
      <slot name="slotHeader"/>
      <el-table-column
        v-for="(item,index) in showData"
        :width="item.width"
        :prop="item.prop"
        :key="index+1"
        :columnIndex="index+2"
        :column-key="item.prop"
        :filters="item.filters"
        :sortable="item.sortable"
        :label="(item.label)"
      >
        <template slot-scope="scope">
          <div>
            <img
              v-if="item.type ==='image'"
              :src="scope.row|deepGet(item.prop)|_formatImg"
              alt=""
              style="width: 50px;height: 50px"
            >
            <span
              v-if="item.type ==='time'"
            >{{scope.row|deepGet(item.prop)|_formatTime(item.cformat)}}</span>
            <span v-if="item.type ==='string'">{{scope.row|deepGet(item.prop)}}</span>
          </div>
        </template>
      </el-table-column>
      <slot/>
    </el-table>
  </div>
</template>

<script>
export default {
  components: {},
  computed: {},
  props: {
    tags: {
      type: Array
    },
    maxHeight: {
      type: [String, Number],
      default: "auto"
    },
    showData: {
      type: Array
    },
    gridData: {
      type: Array
    },
    tableRowClassName: {
      type: Function
    },
    getSummaries: {
      type: Function
    },
    showSummary: {
      type: Boolean
    },
    linkFunction: {
      type: Function
    },
    filterFunction: {
      type: Function
      // default: function() {
      //   return null;
      // }
    }
  },
  data() {
    return {
      showTable: true
    };
  },
  methods: {
    refresh() {
      this.showTable = false;
      this.$nextTick(() => {
        this.showTable = true;
      });
    },
    toggleSelection(rows, selected) {
      if (rows) {
        if (selected) {
          rows.forEach(row => {
            this.$refs.table.toggleRowSelection(row, selected);
          });
        } else {
          rows.forEach(row => {
            this.$refs.table.toggleRowSelection(row);
          });
        }
      }
    },
    toggleRowSelection(row) {
      this.$refs.table.toggleRowSelection(row);
    },
    handleSelectionChange(e) {
      this.$emit("selectionChange", e);
    },
    filterChange(e) {
      this.$emit("filterChange", e);
    },
    sortChange(e) {
      this.$emit("sortChange", e);
    },
    onClick(row) {
      this.$emit("onClick", row);
    },
    rowClick(row) {
      this.$emit("rowClick", row);
      // this.$refs.table.toggleRowSelection(row);
      this.$emit("selectionChange", this.$refs.table.selection);
    }
  },
  filters: {
    _formatImg(val) {
      return "https://jvstest.juvending.cn" + val;
    },
    priceFixed(val, num) {
      return (val / 100).toFixed(num);
    },
    deepGet(value, path, df = undefined) {
      return (
        "" +
          (!Array.isArray(path)
            ? path
                .replace(/\[/g, ".")
                .replace(/\]/g, "")
                .split(".")
            : path
          ).reduce((o, k) => (o || {})[k], value) || df
      );
    },
    _formatTime(val, cFormat) {
      return parseTime(val, cFormat);
      // return moment(val).format('YYYY-MM-DD hh:mm')
      // return formatTime(val)
    },
    yuan(val) {
      return "￥" + (val / 100).toFixed(2);
    }
  }
};
function parseTime(time, cFormat) {
  if (arguments.length === 0) {
    return null;
  }
  const format = cFormat || "{y}-{m}-{d} {h}:{i}";
  let date;
  if (typeof time === "object") {
    date = time;
  } else {
    if (("" + time).length === 10) time = parseInt(time) * 1000;
    date = new Date(time);
  }
  const formatObj = {
    y: date.getFullYear(),
    m: date.getMonth() + 1,
    d: date.getDate(),
    h: date.getHours(),
    i: date.getMinutes(),
    s: date.getSeconds(),
    a: date.getDay()
  };
  const time_str = format.replace(/{(y|m|d|h|i|s|a)+}/g, (result, key) => {
    let value = formatObj[key];
    if (key === "a")
      return ["一", "二", "三", "四", "五", "六", "日"][value - 1];
    if (result.length > 0 && value < 10) {
      value = "0" + value;
    }
    return value || 0;
  });
  return time_str;
}
</script>

<style lang="scss" scoped>
.my-tag-item {
  border-radius: 20px;
  margin-right: 10px;
  padding: 3px 10px;
  box-sizing: border-box;
  font-size: 12px;
  word-wrap: break-word;
  overflow: hidden;
  display: inline-block;

  // -webkit-box-orient: vertical;
  // -webkit-line-clamp: 1;
  // overflow: hidden;
}
</style>
