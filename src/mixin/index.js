import Vue from "vue";
import { mapGetters } from "vuex";
import store from "@/store";

Vue.mixin({
  components: {
  },
  methods: {
    _(value, path) {
      return (
        (!Array.isArray(path)
          ? path
            .replace(/\[/g, ".")
            .replace(/\]/g, "")
            .split(".")
          : path
        ).reduce((o, k) => (o || {})[k], value) || undefined
      );
    },
    _lang(value, path) {
      return this._(value, path) || this._(value, "en");
    },
    delConfirm() {
      return new Promise((resolve, reject) => {
        return this.$confirm(
          this.$t("CONFIRM_DEL_TITLE"),
          this.$t("CONFIRM_DEL_TIPS"),
          {
            confirmButtonText: this.$t("CONFIRM_DEL_CONFIRM"),
            cancelButtonText: this.$t("CONFIRM_DEL_CANCEL"),
            type: "warning"
          }
        )
          .then(() => {
            resolve();
          })
          .catch(() => {
            this.$message({
              type: "info",
              message: this.$t("MESSAGE_DEL_CANCEL")
            });
            reject();
          });
      });
    },
    confirmSuccess() {
      return this.$message({
        type: "success",
        message: this.$t("MESSAGE_DEL_SUCCESS")
      });
    },
    goBack() {
      this.$router.go(-1);
    },
    toUrl(url, query) {
      this.$router.push({
        path: url,
        query: query
      });
    }
  },

  filters: {
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
    price(val, currency) {
      return (val / 100).toFixed(2) + (currency || "");
    },
    yuan(val) {
      return "￥" + (val / 100).toFixed(2);
    }
  },
  computed: {
    ...mapGetters([
      // "interfaceLanguage",
      // "eduYear",
      // "eduDirectory",
      // "sidebarOpen",
      // "version",
      // "commit",
      // "currency",
      // "tableSetting",
      // "schoolConfig"
    ])
  }
});

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
