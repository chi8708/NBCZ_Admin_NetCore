<template>
  <div>
    <Tree :data="functionTree" show-checkbox check-directly></Tree>
  </div>
</template>
<script>
import { getFunctions } from "@/api/pubFunction";
import { truncate } from "fs";
export default {
  props: {
    parent: {}
  },
  data() {
    return {
      checkedCodes: [],
      functions: [],
      functionTree: [],
      functionTreeItems: []
    };
  },
  methods: {
    getFunctions() {
      this.functions = [];
      this.functionTree = [];
      this.functionTreeItems = [];
      getFunctions()
        .then(res => {
          const resData = res.data;
          const data = resData.data;
          this.functions = data;
          this.loadfunctionTree();
        })
        .catch(err => {});
    },
    loadfunctionTree() {
      var treeRoot = this.functions.filter(p => p.parentCode == 0);
      this.loadfunctionTreeChild(treeRoot);
    },
    loadfunctionTreeChild(treeData) {
      for (let index = 0; index < treeData.length; index++) {
        const element = treeData[index];
        this.loadfunctionTreeItem(element);
        const child = this.functions.filter(
          p => p.parentCode == element.functionCode
        );
        if (child && child.length > 0) {
          this.loadfunctionTreeChild(child);
        }
      }
    },
    loadfunctionTreeItem(treeItemData) {
      var treeItem = {
        title: treeItemData.functionChina,
        value: treeItemData.functionCode
      };
      if (this.checkedCodes && this.checkedCodes.length > 0) {
        treeItem.checked = this.checkedCodes.includes(
          treeItemData.functionCode
        );
      } else {
        treeItem.checked = false;
      }
      if (treeItemData.parentCode == 0) {
        treeItem.expand = true;
      }
      this.functionTreeItems.push(treeItem);
      if (treeItemData.parentCode != 0) {
        //foreach 无法终止循环
        this.functionTreeItems.every(element => {
          if (element.value == treeItemData.parentCode) {
            if (!element.children) {
              element.children = [];
            }
            element.children.push(treeItem);
            return false;
          }
          return true;
        });
      } else {
        this.functionTree.push(treeItem);
      }
    },
    functionSelect(checkedCodes) {
      this.checkedCodes = checkedCodes;
      this.getFunctions();
    }
  },
  mounted() {
    //this.getFunctions();
  }
};
</script>