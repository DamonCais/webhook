<template>
  <div class="flex column scrollable only-y">
    <div class="p-20">
      <!-- 
      <el-button @click="GetUserInfo" type="primary">GetUserInfo</el-button>
      <el-button @click="GetOrgShop" type="success">GetOrgShop</el-button>
      <el-button @click="GetCorpDetail" type="info">GetCorpDetail</el-button>
      <el-button @click="GetTopEQM" type="warning">GetTopEQM</el-button>
      <el-button @click="GetEQMDetail" type="warning">GetEQMDetail</el-button>
      <el-button @click="OpenMaintainEQM" type="error">OpenMaintainEQM</el-button>
      <el-button @click="GetProductDetail" type="error">GetProductDetail</el-button>
      <el-button @click="GetProductType" type="error">GetProductType</el-button>
    </div>
    <div class="p-20 flex align-center justify-space-between">
      <div>ProductCode:
        <el-input v-model="ProductCode"></el-input>
      </div>
      <div>Switch:
        <el-input v-model="Switch"></el-input>
      </div>
      <div>MaintainProductCode:
        <el-input v-model="MaintainProductCode"></el-input>
      </div>-->

      <el-button @click="GetEqm" type="primary">GetEqm</el-button>
      <el-button @click="CardRevise" type="primary">CardRevise</el-button>
    </div>
    <div class="p-20">
      <el-input type="textarea" :autosize="{ minRows: 9}" placeholder="请输入内容" v-model="res"></el-input>
    </div>
  </div>
</template>

<script>
// @ is an alias to /src
import { baseGet } from "@/api/api";
export default {
  name: "home",
  mounted() {
    // this.getEqm();
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
    async CardRevise() {
      let obj = {
        CardNo: "201901281848183893_9",
        CardFaceValues: "100",
        CardAmount: "0.03",
        CardDuerationDay: "80",
        CardOnLine: false,
        CardProductType: 1,
        BundleCardNo1: null,
        BundleCardNo2: null,
        BundleCardNo3: null,
        BundleType: true,
        PictureURL:
          "/fileServer/image/upload/20190128064758/thumbnail/IMG_-920566669.jpg",
        Description: "敬业卡",
        FirstUse: false,
        theMemberCode: "fb85ff30-8e1e-4afe-bc5b-5b287d2bf1c3",
        CardBundleNumber1: 0,
        CardBundleNumber2: 0,
        CardBundleNumber3: 0,
        CanBundle: true,
        token: "00001iloveyouruo"
      };
      let res = await baseGet(`ORG/CardRevise`, obj);
      this.res = JSON.stringify(res.data.entity, null, 2);
    },
    async a(){
      let obj = {
        theMemberCode:533274fc-3c94-4f20-a031-496029b679a0,
CardCorpCode:35003,
CardDescription:%E8%BD%A6%E6%9E%B6%E7%A7%9F%E7%94%A8%E5%8D%A1,
CardFaceValues:159.0,
CardAmount:159.0,
CardDuerationDay:30,
CardOnLine:true,
CardProductType:10,
BundleCardNo1:,
BundleCardNo2:,
BundleCardNo3:,
BundleType:true
PictureURL:%2FfileServer%2Fimage%2Fupload%2F20190128031746%2Fthumbnail%2FIMG_-1268329420.jpg,
ShopIDS:1_9,
FirstUse:false,
CanBundle:true,
BundleNumber1:0,
BundleNumber2:0,
BundleNumber3:0,
token:'00001iloveyouruo',
      }
    }
    async GetUserInfo() {
      let obj = {
        Uid: "13728119700",
        UidType: "Mobile",
        token: "00001iloveyouruo"
      };
      let res = await baseGet(`/MB/GetUserInfo`, obj);
      this.res = JSON.stringify(res.data.entity, null, 2);
    },
    async GetProductType() {
      let obj = {
        ProductCode: this.ProductCode,
        token: "00001iloveyouruo"
      };
      let res = await baseGet(`/PD/GetProductType`, obj);
      this.res = JSON.stringify(res.data.entity, null, 2);
    },
    async GetProductDetail() {
      let obj = {
        ProductCode: this.ProductCode,
        token: "00001iloveyouruo"
      };
      let res = await baseGet(`/PD/GetProductDetail`, obj);
      this.res = JSON.stringify(res.data.entity, null, 2);
    },
    async OpenMaintainEQM() {
      let obj = {
        MemberCode: "5ef473dd-6959-4b89-89b1-7c1f40aef3d7",
        Switch: this.Switch,
        EqmUID: "cabinet_8f818bae-4614-4c5c-b3c8-79b07dacee51",
        MaintainProductCode: this.MaintainProductCode,
        token: "00001iloveyouruo"
      };
      let res = await baseGet(`/EQM/OpenMaintainEQM`, obj);
      this.res = JSON.stringify(res.data.entity, null, 2);
    },
    async ChanageEQM() {
      let obj = {};
      let res = await baseGet(`/EQM/ChanageEQM`, obj);
      this.res = JSON.stringify(res.data.entity, null, 2);
    },
    async GetEQMDetail() {
      let obj = {
        MemberCode: "5ef473dd-6959-4b89-89b1-7c1f40aef3d7",
        EqmUID: "cabinet_8f818bae-4614-4c5c-b3c8-79b07dacee51",
        token: "00001iloveyouruo"
      };
      let res = await baseGet(`/EQM/GetEQMDetail`, obj);
      this.res = JSON.stringify(res.data.entity, null, 2);
    },
    async GetTopEQM() {
      let obj = {
        MemberCode: 222,
        Latitude: 116.397945,
        Longitude: 123.123123,
        pageSize: 10,
        pageIndex: 1,
        OnLine: 1,
        ShopID: 1,
        CorpCode: "",
        token: "00001iloveyouruo"
      };
      let res = await baseGet(`/EQM/GetTopEQM`, obj);
      this.res = JSON.stringify(res.data.entity, null, 2);
    },
    async GetCorpDetail() {
      let obj = {
        ShopID: 1,
        token: "00001iloveyouruo"
      };
      let res = await baseGet(`/ORG/GetCorpDetail`, obj);
      this.res = JSON.stringify(res.data.entity, null, 2);
    },
    async GetEqm() {
      let obj = {
        MemberCode: 222,
        pageSize: 10,
        pageIndex: 1,
        Status: 1,
        token: "00001iloveyouruo"
      };
      let res = await baseGet(`/ORG/GetCorpMaster`, obj);
      this.res = JSON.stringify(res.data.entity, null, 2);
    },
    async GetOrgShop() {
      let obj = {
        MemberCode: 222,
        Latitude: 116.397945,
        Longitude: 123.123123,
        pageSize: 10,
        pageIndex: 1,
        OnLine: 1,
        CorpCode: 35003,
        token: "00001iloveyouruo"
      };
      let res = await baseGet(`/ORG/GetOrgShop`, obj);
      this.res = JSON.stringify(res.data.entity, null, 2);
    }
  }
};
</script>
