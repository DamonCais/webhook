<template>
  <div class="scrollable only-y">member</div>
</template>

<script>
// @ is an alias to /src
import { baseGet } from "@/api/api";
export default {
  name: "home",
  mounted() {
    console.log(this.ProductCode);
    this.getEqm();
  },
  data() {
    return {
      res: null,
      Switch: "",
      MaintainProductCode: "d003",
      ProductCode: "AT4810GZ201812290001"
    };
  },
  components: {},
  methods: {
    async getEqm() {
      let obj = {
        MemberCode: "fb85ff30-8e1e-4afe-bc5b-5b287d2bf1c3",
        pageSize: 10,
        pageIndex: 1,
        Status: 1,
        token: "00001iloveyouruo"
      };
      let res = await baseGet(`/ORG/GetCorpMaster`, obj);
      this.res = JSON.stringify(res.data.entity, null, 2);
    }
  }
};
</script>
