<template>
    <div>
        <label ref="dragArea" for="fileupload" tabindex="0" class="el-upload el-upload--text">
            <div @dragover.stop.prevent="handleDragover" @dragleave.stop.prevent="handleDragleave" @drop.stop.prevent="handleDrop" class="el-upload-dragger">
                <i class="el-icon-upload"></i>
                <div class="el-upload__text">Drop file here or
                    <em>click to upload</em>
                </div>
            </div>
            <!-- webkitdirectory 文件夹上传 -->
            <input @change="handleChange" id='fileupload' ref="fileupload" type="file" name="file" multiple="multiple" class="el-upload__input">
        </label>
        <ul class="el-upload-list el-upload-list--text">
            <li v-for="(f,i) in files" :key="i" class="el-upload-list__item is-success">
                <a class="el-upload-list__item-name">
                    <i class="el-icon-document"></i>{{f.name}}
                </a>
                <label class="el-upload-list__item-status-label">
                    <i class="el-icon-upload-success el-icon-circle-check"></i>
                </label>
                <i class="el-icon-close" @click="delFile(i)"></i>
            </li>
        </ul>
    </div>
</template>

<script>
import { addClass, removeClass } from "@/utils/dom";

export default {
  data() {
    return {
      files: []
    };
  },
  methods: {
    delFile(i) {
      this.files.splice(i, 1);
    },
    async handleDrop(e) {
      let files = e.target.files || e.dataTransfer.files;
      for (let f of files) {
        console.log(f);
        if (f.type) {
          this.files.push(f);
        }
      }
      //   Array.prototype.push.apply(this.files, files);
      removeClass(e.target, "is-dragover");
      this.$forceUpdate();
      this.$emit("fileChange", this.files);
    },
    handleDragleave(e) {
      removeClass(e.target, "is-dragover");
    },
    handleDragover(e) {
      addClass(e.target, "is-dragover");
    },
    async handleChange() {
      let inputDOM = this.$refs["fileupload"];
      let files = inputDOM.files;
      for (let f of files) {
        this.files.push(f);
      }
      this.$forceUpdate();
      inputDOM.value = null;

      this.$emit("fileChange", this.files);
    }
  }
};
</script>

<style>
</style>
