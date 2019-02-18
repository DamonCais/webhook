<template>
    <div class="flex justify-space-between scrollable">
        <div style="width:400px;">
            <my-form v-if="!isEditing">
                <my-form-item label="EquipmentName">{{equipmen.EquipmentName}}</my-form-item>
                <my-form-item label="Address">{{equipmen.Address}}</my-form-item>
                <my-form-item label="DeviceName">{{equipmen.DeviceName}}</my-form-item>
                <my-form-item label="Services">{{equipmen.Services}}</my-form-item>
                <my-form-item label="Logo">
                    <img style="width: 50px;height: 50px" :src="equipmen.Logo|_formatImg">
                </my-form-item>
                <my-form-item>
                    <el-button @click="isEditing=!isEditing">编辑</el-button>
                </my-form-item>
            </my-form>
            <my-form v-else>
                <my-form-item label="EquipmentName">
                    <el-input v-model="baseEquipmen.EquipmentName"></el-input>
                </my-form-item>
                <my-form-item label="Address">
                    <el-input v-model="baseEquipmen.Address"></el-input>
                </my-form-item>
                <my-form-item label="DeviceName">
                    <el-input v-model="baseEquipmen.DeviceName"></el-input>
                </my-form-item>
                <my-form-item label="Services">
                    <el-input v-model="baseEquipmen.Services"></el-input>
                </my-form-item>
                <my-form-item label="Logo">
                    <img style="width: 50px;height: 50px" :src="baseEquipmen.Logo|_formatImg">
                </my-form-item>
                <my-form-item>
                    <el-button @click="save">保存</el-button>
                    <el-button @click="cancel">取消</el-button>
                </my-form-item>
            </my-form>
        </div>
        <div class="flex column align-center" style="width:400px;">
            <div class="mb-20">充电柜详情</div>
            <div
                class="flex justify-space-around p-10"
                style="min-height:590px;border:1px solid #aaa;"
            >
                <div class="p-10">
                    <div
                        class="switch flex column align-center justify-center mb-10"
                        :key="index+5"
                        v-for="(item,index) in bSwitch4812"
                        style="width:160px;height:100px;"
                    >
                        <span
                            class="fs-12"
                            style="word-wrap: break-word;"
                        >{{item.sw.ProductCode||item.classify}}</span>
                        <span>{{item.text}}</span>
                    </div>
                </div>
                <div class="p-10">
                    <div
                        class="switch flex column align-center justify-center mb-10"
                        :key="index"
                        v-for="(item,index) in bSwitch4810"
                        style="width:160px;height:100px;"
                    >
                        <span
                            class="fs-12"
                            style="word-wrap: break-word;"
                        >{{item.sw.ProductCode||item.classify}}</span>
                        <span>{{item.text}}</span>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import { baseGet } from "@/api/api";
import { getBatteryText } from "@/utils/util";

export default {
  mounted() {
    this.EqmUID = this.$route.params.id;
    this.getEqmDetail();
  },
  data() {
    return {
      EqmUID: "",
      res: null,
      equipmen: {},
      baseEquipmen: {},
      bSwitch: [],
      bSwitch4810: [],
      bSwitch4812: [],
      isEditing: false
    };
  },
  methods: {
    async save() {
      let obj = Object.assign({}, this.baseEquipmen);
      console.log(obj);
      let res = await baseGet(`/EQM/ChanageEQM`, obj);
    },
    cancel() {
      this.isEditing = false;
    },
    editIng() {},
    async getEqmDetail() {
      let obj = {
        MemberCode: "fb85ff30-8e1e-4afe-bc5b-5b287d2bf1c3",
        EqmUID: this.EqmUID,
        Source: 2,
        token: "00001iloveyouruo"
      };
      let res = await baseGet(`/EQM/GetEQMDetail`, obj);
      this.equipmen = res.data.entity.equipmen;
      this.baseEquipmen = _.cloneDeep(this.equipmen);
      let bSwitch = res.data.entity._switch;
      if (bSwitch && bSwitch.length) {
        bSwitch.map((item, index) => {
          item.classify = item.sw.PartitionProdType;
          let { text, icon, type } = getBatteryText(item.sw.Switch);
          item.text = text;
          item.icon = icon;
          item.type = type;
        });
      }
      this.bSwitch = bSwitch;
      this.bSwitch4812 = bSwitch.filter((s, i) => i < 5);
      this.bSwitch4810 = bSwitch.filter((s, i) => i >= 5);
      console.log(this.bSwitch);
    }
  },
  filters: {
    _formatImg(val) {
      if (val) {
        return "https://jvstest.juvending.cn" + val;
      } else {
        return "";
      }
    }
  }
};
</script>

<style>
.switch {
  border: 1px solid #aaa;
  background: #eee;
}
</style>
